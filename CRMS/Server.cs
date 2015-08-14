using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace CRMS
{
    public class Server
    {
        private static DBManager DB1 = new DBManager();
        private static byte[] _buffer = new byte[10000];
        private static List<Socket> _clientsockets = new List<Socket>();
        private static Socket _serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        

        public Server()
        {
            SetupServer();
        }

        #region server connection managers
        private static void SetupServer()
        {
            try
            {
                Debug.WriteLine("Setting up server");
                _serversocket.Bind(new IPEndPoint(IPAddress.Any, 100));
                _serversocket.Listen(5);
                _serversocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                Debug.WriteLine("Server set up");
            }
            catch 
            {
                Debug.WriteLine("unable to Set up server");
            }
        }

        private static void AcceptCallback(IAsyncResult AR)
        {

            Socket socket = _serversocket.EndAccept(AR);
            _clientsockets.Add(socket);
            Debug.WriteLine("Client connected");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serversocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }
        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);

        }

        private static void SendMessage()
        {
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                int received = socket.EndReceive(AR);
                byte[] databuf = new byte[received];
                Array.Copy(_buffer, databuf, received);

                string frame = Encoding.ASCII.GetString(databuf);
                Debug.WriteLine("Text received: " + frame);

                Request req = new Request();
                req = JsonConvert.DeserializeObject<Request>(frame);

                string r = "no response";


                r = JsonConvert.SerializeObject(RequestHandler(req));

                byte[] response = Encoding.UTF8.GetBytes(r);
                socket.BeginSend(response, 0, response.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                //MessageBox.Show("sent response"); 
                //Thread.Sleep(5000);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch { }
        }
        #endregion


        #region client request handlers
        private static Request RequestHandler(Request req)
        {
            try
            {
                switch (req.category)
                {
                    case Constants.employees:
                        req = EmployeeHandler(req);
                        break;
                    case Constants.Materials:
                        req = MaterialRequestHandler(req);
                        break;
                    case Constants.Labourers :
                        req = LabourerRequestHander(req);
                        break; 
                }

                return req;
            }
            catch { return null; }
        }
        private static Request EmployeeHandler(Request req)
        {
            try
            {
                string response = "invalid id or password";
                employees emp = JsonConvert.DeserializeObject<employees>(req.content);
                Request reply = new Request();

                switch (req.purpose)
                {
                    case Constants.Log_In:
                        // get the name of the employee
                        List<employees> emplist = DB1.GetEmployees("");
                        //check if the id and the password
                        var result = from item in emplist
                                     where ((item.employee_id == emp.employee_id) && (item.password == emp.password))
                                     select item;

                        if (result.Count() > 0)
                        {
                            // get the employee
                            emp = ((List<employees>)result.ToList()).First();
                            //response =  emp.employee_name; 

                            //check if the employee is a foreman
                            List<job_type> foremanjobtypelist = DB1.GetJobType("SELECT a.* FROM job_type a, job_description b WHERE (a.job_type_id = b.job_type_id) AND (b.employee_id = " + emp.employee_id + " ) AND (a.job_name =  '" + Constants.Foreman + "')");
                            if (foremanjobtypelist.Count == 0)
                            {
                                // the person is a not a foreman foreman
                                List<job_type> supervisorjobtypelist = DB1.GetJobType("SELECT a.* FROM job_type a, job_description b WHERE (a.job_type_id = b.job_type_id) AND (b.employee_id = " + emp.employee_id + " ) AND((a.job_name = '" + Constants.Supervisor + "'))");
                            }
                            else
                            {
                                // the person is a foreman 
                                // get a list of all tasks that are related to him
                                List<task> tasklist = DB1.GetTasks("SELECT a.* FROM tasks a WHERE a.employee_id = " + emp.employee_id);
                                //return the tasks
                                string taskstring = JsonConvert.SerializeObject(tasklist);
                                // generate the request
                                req = new Request();
                                req.category = Constants.Foreman;
                                req.purpose = Constants.Log_In;
                                Container c = new Container();
                                c._tasklist = tasklist;
                                req.content = JsonConvert.SerializeObject(c);

                            }


                        }
                        else { }

                        break;
                }
                return req;
            }
            catch
            {
                return null;
            }

        }
        private static Request MaterialRequestHandler(Request req)
        {
            try
            {
                //switch based on the purpose
                switch (req.purpose)
                {
                    case Constants.Request_Material:
                        // show the input to the user
                        MessageBox.Show("A request has been made for\n " + (JsonConvert.DeserializeObject<MaterialRequest>(req.content).MaterialName) + "\n quantity " + (JsonConvert.DeserializeObject<MaterialRequest>(req.content).quantity) , "Field Request", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
                        //Application.Current.Dispatcher.BeginInvoke( DispatcherPriority.Background, new Action( () => ) );
                        //Application.Current.Dispatcher.Invoke(new Action(() => FillRequests));
                        
                        
                        break;
                    case Constants.Remainder_Materials:
                        //show the user the request
                        MessageBox.Show("There is a remainder of \n " + (JsonConvert.DeserializeObject<MaterialRequest>(req.content).MaterialName) + "\n in " + (JsonConvert.DeserializeObject<RemainderRequest>(req.content).quantity) + "amount", "Field Request", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);
                        DB1 = new DBManager();
                        if (DB1.UpdateValue((JsonConvert.DeserializeObject<RemainderRequest>(req.content).quantity), (JsonConvert.DeserializeObject<MaterialRequest>(req.content).MaterialName)))
                        {
                            MessageBox.Show("stock has been updated");
                        }
                        break;
                }
            }
            catch { }

            return req;
        }
        private static Request LabourerRequestHander(Request req) 
        {
            try 
            {
                LabourerRequest lr = JsonConvert.DeserializeObject<LabourerRequest>(req.content);
                switch (req.purpose) 
                {

                    case Constants.New_Labourer:
                        //inform the user
                        MessageBoxResult mr =  MessageBox.Show("A request has been made to register a new labourer proceed" ,"Labourer registration" , MessageBoxButton.YesNo);
                        if(mr == MessageBoxResult.No)
                        {
                            return null; 
                        }
                        Container c = new Container();
                        c._labourers = new labourers();
                        c._project_labour_allocation = new project_labour_allocation();
                        
                        // insert a new value to the labourers table
                        c._labourers.national_id = lr.national_id;
                        c._labourers.labourer_id = lr.labourer_id;
                        c._labourers.labourer_name = lr.labourer_name;
                        c._labourers.labourer_contact = lr.labourer_contact;
                        DB1 = new DBManager();
                        if (DB1.InsertValue(c, Constants.Labourers)) 
                        {
                            MessageBox.Show("the labourer has been registered");
                        }
                        
                        // get the derived labourer id
                        //allocate the registered labourer
                        c._project_labour_allocation.project_id = lr.project_id;                                                
                        c._project_labour_allocation.labourer_id = DB1.GetLabourers("SELECT * FROM labourers WHERE national_id = '" + lr.national_id + "'").First().labourer_id;
                        
                        
                        if (DB1.InsertValue(c, Constants.Project_Labour_Allocation))
                        {
                            MessageBox.Show("the labourer has been registered to the project");
                        }
                        break;
                    case Constants.Labourers_List:
                        // return the list of labourers to the foreman                        
                        //
                        DB1 = new DBManager();
                        c = new Container();
                        c._labourerslist = new List<labourers>();
                        c._labourerslist = DB1.GetLabourers("SELECT a.* FROM labourers a , project_labour_allocation b WHERE a.labourer_id = b.labourer_id AND b.project_id = " + lr.project_id + "");
                        Debug.WriteLine("SELECT a.* FROM labourers a , project_labour_allocation b WHERE a.labourer_id = b.labourer_id AND b.project_id = " + lr.project_id + "");
                       

                        c._employeeslist = new List<employees>();
                        c._employeeslist = DB1.GetEmployees("SELECT a.* FROM employees a , project_employee_allocation b WHERE a.employee_id = b.employee_id AND b.project_id = " + lr.project_id + "");
                        Debug.WriteLine("SELECT a.* FROM employees a , project_employee_allocation b WHERE a.employee_id = b.employee_id AND b.project_id = " + lr.project_id + "");


                        //c._employees_list = DB1.GetLabourers()
                        req.content = JsonConvert.SerializeObject(c);
                        req.category = Constants.Labourers;
                        req.purpose = Constants.Labourers_List;
                        
                        break;
                    case Constants.Submit_Work_Day:
                        DB1 = new DBManager();
                        c = JsonConvert.DeserializeObject<Container>(req.content);
                        foreach (labourers labour in c._labourerslist) 
                        {
                            // get the project allocation id where the labourer id is entered
                            c._project_labour_working_day = new project_labour_working_day();
                            c._project_labour_working_day.project_labour_allocation_id = DB1.GetProjectLabourAllocation("SELECT a.* FROM project_labour_allocation a , labourers b WHERE a.labourer_id = b.labourer_id AND b.labourer_id = " + labour.labourer_id + "").First().project_labour_allocation_id;
                            c._project_labour_working_day.working_date = DateTime.Now.Date; 
                            
                            if(!DB1.InsertValue(c , Constants.Project_Labour_Working_Day))
                            {
                                MessageBox.Show("labourer not entered");
                            }
                        }
                        foreach (employees emp in c._employeeslist) 
                        {
                            c._project_employee_working_day = new project_employee_working_day();
                            c._project_employee_working_day.project_employee_allocation_id = DB1.GetProjectEmployeeAllocation("SELECT a.* FROM project_employee_allocation a , employees b WHERE a.employee_id = b.employee_id AND b.employee_id = " + emp.employee_id + "").First().project_employee_allocation_id;
                            c._project_employee_working_day.working_date = DateTime.Now.Date;

                            if (!DB1.InsertValue(c, Constants.Project_employee_Working_Day)) 
                            {
                                MessageBox.Show("employee not entered");
                            }
                        }
                        MessageBox.Show("data entered");
                        break;
                        
                }
                return req; 
            }
            catch
            {
                return null ; 
            }
        }
        #endregion

    }
}
