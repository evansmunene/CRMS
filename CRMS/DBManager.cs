using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CRMS
{
    public class DBManager
    {

        MySqlConnection conn = new MySqlConnection();
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataReader datareader;

        public bool Connect()
        {
            try
            {
                //establish a connection to the database
                string server = "localhost";
                string username = "root";
                string password = "";
                string database = "resources";
                string connectionstring = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false", server, username, password, database);

                this.conn = new MySqlConnection(connectionstring);
                this.conn.Open();
                var a = conn.ServerVersion;
                if (conn == null)
                {
                    Debug.WriteLine("unable to connect to the data base ");
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
        public MySqlCommand GenerateCommand(Container c, string entitytype)
        {
            string CmdText = null;
            //MySqlCommand cmd = new MySqlCommand(); 
            try
            {
                switch (entitytype)
                {
                    case Constants.Project:
                        CmdText = "INSERT INTO projects( project_id , project_name , location , description ,start_date , end_date  , client_id ) VALUES(@project_id , @project_name , @location, @description , @start_date , @end_date , @client_id )";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add nodes to the values
                        cmd.Parameters.AddWithValue("@project_id", null);
                        cmd.Parameters.AddWithValue("@project_name", c._project.project_name);
                        cmd.Parameters.AddWithValue("@location", c._project.location);
                        cmd.Parameters.AddWithValue("@description", c._project.description);
                        cmd.Parameters.AddWithValue("@start_date", c._project.start_date);
                        cmd.Parameters.AddWithValue("@end_date", c._project.end_date);
                        cmd.Parameters.AddWithValue("@client_id", c._project.client_id);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        string tmp = cmd.CommandText.ToString();
                        foreach (MySql.Data.MySqlClient.MySqlParameter p in cmd.Parameters)
                        {
                            try
                            {
                                tmp = tmp.Replace('@' + p.ParameterName.ToString(), "'" + p.Value.ToString() + "'");
                            }
                            catch { }

                        }
                        Debug.WriteLine(tmp);
                        break;
                    case Constants.Task:
                        CmdText = "INSERT INTO tasks( task_id , project_id , budget , start_date , end_date , employee_id , task_name  ) VALUES ( @task_id , @project_id , @budget , @start_date , @end_date , @employee_id ,@task_name )";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add nodes to the values
                        cmd.Parameters.AddWithValue("@task_id", null);
                        cmd.Parameters.AddWithValue("@project_id", c._task.project_id);
                        cmd.Parameters.AddWithValue("@budget", c._task.budget);
                        cmd.Parameters.AddWithValue("@start_date", c._task.start_date);
                        cmd.Parameters.AddWithValue("@end_date", c._task.end_date);
                        cmd.Parameters.AddWithValue("@employee_id", c._task.employee_id);
                        cmd.Parameters.AddWithValue("@task_name", c._task.task_name);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.Client:
                        CmdText = "INSERT INTO clients(client_id , client_name , email , contact ) VALUES( @client_id , @client_name , @email , @contact)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the nodes
                        cmd.Parameters.AddWithValue("@client_id", null);
                        cmd.Parameters.AddWithValue("@client_name", c._client.client_name);
                        cmd.Parameters.AddWithValue("@email", c._client.email);
                        cmd.Parameters.AddWithValue("@contact", c._client.contact);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.Materials:
                        CmdText = "INSERT INTO materials( material_id , material_name , measuring_unit ) VALUES( @material_id , @material_name , @measuring_unit )";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@material_id", null);
                        cmd.Parameters.AddWithValue("@material_name", c._materials.material_name);
                        cmd.Parameters.AddWithValue("@measuring_unit", c._materials.measuring_unit);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.Stock_Materails:
                        CmdText = "INSERT INTO stock_materials(stock_id ,material_id, quantity) VALUES(@stock_id ,@material_id, @quantity)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@stock_id", null);
                        cmd.Parameters.AddWithValue("@material_id", c._stock_materials.material_id);
                        cmd.Parameters.AddWithValue("@quantity", c._stock_materials.quanitity);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.Job_Description:
                        CmdText = "INSERT INTO job_description ( employee_id , job_type_id , job_description_id ) VALUES( @employee_id, @job_type_id, @job_description_id)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@employee_id", c._job_description.employee_id);
                        cmd.Parameters.AddWithValue("@job_type_id", c._job_description.job_type_id);
                        cmd.Parameters.AddWithValue("@job_description_id", null);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.Job_Type:
                        CmdText = "INSERT INTO job_type(job_type_id, job_name, unit_pay) VALUES(@job_type_id, @job_name, @unit_pay)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@job_type_id", null);
                        cmd.Parameters.AddWithValue("@job_name", c._job_type.job_name);
                        cmd.Parameters.AddWithValue("@unit_pay", c._job_type.unit_pay);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.employees:
                        CmdText = "INSERT INTO employees(employee_id, employee_name, employee_contact_number) VALUES(@employee_id, @employee_name, @employee_contact_number)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@employee_id", null);
                        cmd.Parameters.AddWithValue("@employee_name", c._employees.employee_name);
                        cmd.Parameters.AddWithValue("@employee_contact_number", c._employees.employee_contact_number);
                        Debug.WriteLine(cmd.Parameters.ToString());
                        break;
                    case Constants.Project_Labour_Allocation:
                        CmdText = "INSERT INTO project_labour_allocation ( project_labour_allocation_id , labourer_id, project_id ) VALUES ( @project_labour_allocation_id , @labourer_id,  @project_id )";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@project_labour_allocation_id",null);
                        cmd.Parameters.AddWithValue("@labourer_id", c._project_labour_allocation.labourer_id);
                        cmd.Parameters.AddWithValue("@project_id", c._project_labour_allocation.project_id);                        
                        Debug.WriteLine(cmd.Parameters.ToString());
                        break;
                    case  Constants.Project_Labour_Working_Day:
                        CmdText = "INSERT INTO project_labour_working_day ( project_labour_allocation_id , working_date) VALUES ( @project_labour_allocation_id , @working_date)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@project_labour_allocation_id", c._project_labour_working_day.project_labour_allocation_id);
                        cmd.Parameters.AddWithValue("@working_date", c._project_labour_working_day.working_date); 
                        break;
                    case Constants.Project_Employee_Allocation:
                        CmdText = "INSERT INTO project_employee_allocation( project_employee_allocation_id , employee_id , project_id ) VALUES ( @project_employee_allocation_id , @employee_id , @project_id )";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add node values
                        cmd.Parameters.AddWithValue("@project_employee_allocation_id", null);
                        cmd.Parameters.AddWithValue("@employee_id", c._project_employee_allocation.employee_id);
                        cmd.Parameters.AddWithValue("@project_id", c._project_employee_allocation.project_id);                        
                        break;
                    case Constants.Project_employee_Working_Day:
                        CmdText = "INSERT INTO project_employee_Working_day (project_employee_allocation_id , working_date) VALUES (@project_employee_allocation_id , @working_date) ";
                        cmd = new MySqlCommand(CmdText, this.conn); 
                        // add the node values
                        cmd.Parameters.AddWithValue("@project_employee_allocation_id", c._project_employee_working_day.project_employee_allocation_id);
                        cmd.Parameters.AddWithValue("@working_date", c._project_employee_working_day.working_date);
                        break; 
                    case Constants.Task_Materials:
                        CmdText = "INSERT INTO task_materials ( task_material_id , task_id , material_id , quantity , unit_buying_price , date_allocated ) VALUES ( @task_material_id , @task_id  , @material_id, @quantity , @unit_buying_price , @date_allocated)";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@task_material_id", c._task_materials.task_materials_id);
                        cmd.Parameters.AddWithValue("@task_id", c._task_materials.task_id);
                        cmd.Parameters.AddWithValue("@material_id", c._task_materials.material_id);
                        cmd.Parameters.AddWithValue("@quantity", c._task_materials.quantity);
                        cmd.Parameters.AddWithValue("@unit_buying_price", c._task_materials.unit_buying_price);
                        cmd.Parameters.AddWithValue("@date_allocated", c._task_materials.date_allocated);
                        Debug.WriteLine(cmd.CommandText.ToString());
                        break;
                    case Constants.Labourers:
                        CmdText = "INSERT INTO labourers (labourer_id , labourer_name , labourer_contact , national_id ) VALUES ( @labourer_id , @labourer_name , @labourer_contact , @national_id ); ";
                        cmd = new MySqlCommand(CmdText, this.conn);
                        // add the node values
                        cmd.Parameters.AddWithValue("@labourer_id", c._labourers.labourer_id);
                        cmd.Parameters.AddWithValue("@labourer_name", c._labourers.labourer_name);
                        cmd.Parameters.AddWithValue("@labourer_contact", c._labourers.labourer_contact);
                        cmd.Parameters.AddWithValue("@national_id", c._labourers.national_id);
                        break;
                }


                return cmd;
            }
            catch
            {
                return null;
            }
        }

        public bool InsertValue(Container c, string option)
        {
            try
            {
                MySqlCommand InsertCommand = new MySqlCommand();
                // get a connection to the database
                if (Connect())
                {
                    // enter the value
                    // get a command
                    InsertCommand = GenerateCommand(c, option);
                    InsertCommand.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    Debug.WriteLine("Unable to connect or insert into the database ");
                    return false;
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
                return false;
            }

        }
        public bool InsertEmployeePayment(project_employee_payment p) 
        {
            try 
            {
                string CmdText = "INSERT INTO project_employee_payment (project_employee_allocation_id , amount , date_payed) VALUES (@project_employee_allocation_id , @amount , @date_payed)";
                cmd = new MySqlCommand(CmdText, this.conn);
                cmd.Parameters.AddWithValue("@project_employee_allocation_id", p.project_employee_allocation_id);
                cmd.Parameters.AddWithValue("@amount", p.amount);
                cmd.Parameters.AddWithValue("@date_payed", p.date_paid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }

        public bool InsertLabourPayment(project_labour_payment p)
        {
            try
            {
                string CmdText = "INSERT INTO project_labour_payment (project_labour_allocation_id , amount , date_payed) VALUES (@project_labour_allocation_id , @amount , @date_payed)";
                cmd = new MySqlCommand(CmdText, this.conn);
                cmd.Parameters.AddWithValue("@project_labour_allocation_id", p.project_labour_allocation_id);
                cmd.Parameters.AddWithValue("@amount", p.amount);
                cmd.Parameters.AddWithValue("@date_payed", p.date_paid);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }

        public MySqlDataReader GetReader(string statement)
        {
            try
            {
                
                cmd.Connection = conn;
                cmd.CommandText = statement;

                datareader = cmd.ExecuteReader();

                return datareader;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message); 
                return null; 
            }
        }
        // function to get a whole table from the database
        public Container GetWholeEntity()
        {
            try
            {
                // switch based on the option
                string CmdText = "";
                Container c = new Container();
                if (Connect())
                {
                    // get all clients
                    {
                        CmdText = "SELECT * FROM clients ";
                        datareader = GetReader(CmdText);
                        c._clientlist = new List<clients>();
                        while (datareader.Read())
                        {
                            clients Client = new clients();
                            Client.client_id = Convert.ToInt32(datareader["client_id"]);
                            Client.client_name = datareader["client_name"].ToString();
                            Client.email = datareader["email"].ToString();
                            Client.contact = datareader["contact"].ToString();
                            c._clientlist.Add(Client);
                        }
                        datareader.Close();
                        datareader = null;
                    }

                    // get all employees
                    {
                        CmdText = "SELECT * FROM employees ";
                        datareader = GetReader(CmdText);
                        c._employeeslist = new List<employees>();
                        while (datareader.Read())
                        {

                            employees emp = new employees();
                            emp.employee_name = datareader["employee_name"].ToString();
                            emp.employee_contact_number = datareader["employee_contact_number"].ToString();
                            emp.employee_id = Convert.ToInt32(datareader["employee_id"].ToString());
                            c._employeeslist.Add(emp);
                        }
                        datareader.Close();
                        datareader = null;
                    }


                    //get all job types
                    {
                        CmdText = "SELECT * FROM job_type ";
                        datareader = GetReader(CmdText);
                        c._job_typelist = new List<job_type>();
                        while (datareader.Read())
                        {
                            job_type job = new job_type();
                            job.job_name = datareader["job_name"].ToString();
                            job.job_type_id = Convert.ToInt32(datareader["job_type_id"].ToString());
                            job.unit_pay = Convert.ToInt32(datareader["unit_pay"].ToString());
                            c._job_typelist.Add(job);
                        }
                        datareader.Close();
                        datareader = null;
                    }

                    // get all projects
                    {
                        CmdText = "SELECT * FROM Projects ";
                        datareader = GetReader(CmdText);
                        c._projectlist = new List<projects>();
                        while (datareader.Read())
                        {
                            projects proj = new projects();
                            proj.project_id = Convert.ToInt32(datareader["project_id"].ToString());
                            proj.project_name = datareader["project_name"].ToString();
                            proj.location = datareader["location"].ToString();
                            proj.description = datareader["description"].ToString();
                            proj.start_date = Convert.ToDateTime(datareader["start_date"].ToString());
                            proj.end_date = Convert.ToDateTime(datareader["end_Date"].ToString());
                            proj.status = datareader["status"].ToString();
                            proj.client_id = Convert.ToInt32(datareader["client_id"].ToString());
                            c._projectlist.Add(proj);
                        }
                        datareader.Close();
                        datareader = null;
                    }

                    // get all stock material names
                    {
                        CmdText = "SELECT * FROM stock_materials";
                        datareader = GetReader(CmdText);
                        c._stock_materialslist = new List<stock_materials>();
                        while (datareader.Read())
                        {
                            stock_materials stock = new stock_materials();
                            stock.stock_id = Convert.ToInt32(datareader["stock_id"]);
                            stock.material_id = Convert.ToInt32(datareader["material_id"]);
                            stock.quanitity = Convert.ToInt32(datareader["quantity"]);
                            c._stock_materialslist.Add(stock);
                        }
                        datareader.Close();
                        datareader = null;
                    }

                    // get all material names
                    {
                        CmdText = "SELECT * FROM materials";
                        datareader = GetReader(CmdText);
                        c._materialslist = new List<materials>();
                        while (datareader.Read())
                        {
                            materials mat = new materials();
                            mat.material_id = Convert.ToInt16(datareader["material_id"]);
                            mat.material_name = datareader["material_name"].ToString();
                            mat.measuring_unit = datareader["measuring_unit"].ToString();
                            c._materialslist.Add(mat);
                        }
                        datareader.Close();
                        datareader = null;
                    }

                    //get all tasks
                    {
                        CmdText = "SELECT * FROM tasks";
                        datareader = GetReader(CmdText);
                        c._tasklist = new List<task>();
                        while (datareader.Read())
                        {
                            task t = new task();
                            t.task_id = Convert.ToInt32(datareader["task_id"].ToString());
                            t.task_name = datareader["task_name"].ToString();
                            t.start_date = Convert.ToDateTime(datareader["start_date"].ToString());
                            t.end_date = Convert.ToDateTime(datareader["end_date"].ToString());
                            t.budget = Convert.ToInt32(datareader["budget"].ToString());
                            t.status = datareader["status"].ToString();
                            t.employee_id = Convert.ToInt32(datareader["employee_id"]);
                            t.project_id = Convert.ToInt32(datareader["project_id"].ToString());
                            c._tasklist.Add(t);
                        }
                        datareader.Close();
                        datareader = null;
                    }

                    return c;
                }
                else { return null; }

            }
            catch
            {
                return null;
            }
        }

        #region get individual entities
        public List<projects> GetProjects(string CmdText)
        {
            try
            {

                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM projects ";
                }
                if (!Connect()) 
                {
                    return null;
                }
                datareader = GetReader(CmdText);
                List<projects> _projectlist = new List<projects>();
                while (datareader.Read())
                {
                    projects proj = new projects();
                    proj.project_id = Convert.ToInt32(datareader["project_id"].ToString());
                    proj.project_name = datareader["project_name"].ToString();
                    proj.location = datareader["location"].ToString();
                    proj.description = datareader["description"].ToString();
                    proj.start_date = Convert.ToDateTime(datareader["start_date"].ToString());
                    proj.end_date = Convert.ToDateTime(datareader["end_date"].ToString());
                    proj.status = datareader["status"].ToString();
                    proj.client_id = Convert.ToInt32(datareader["client_id"].ToString());
                    _projectlist.Add(proj);
                }
                datareader.Close();
                datareader = null;
                return _projectlist;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetRawData(string CmdText)
        {
            DataTable data = new DataTable();
            if (Connect())
            {

                MySqlDataAdapter da = new MySqlDataAdapter(CmdText, conn);
                MySqlCommandBuilder cd = new MySqlCommandBuilder(da);
                da.Fill(data);
            }
            return data;
        }
        public List<task> GetTasks(string CmdText)
        {
            try
            {
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM tasks ";
                }

                if (!(Connect())) 
                {
                    return null; 
                }


                datareader = GetReader(CmdText);
                List<task> _tasklist = new List<task>();
                while (datareader.Read())
                {
                    task t = new task();
                    t.task_id = Convert.ToInt32(datareader["task_id"].ToString());
                    t.task_name = datareader["task_name"].ToString();
                    t.start_date = Convert.ToDateTime(datareader["start_date"].ToString());
                    t.end_date = Convert.ToDateTime(datareader["end_date"].ToString());
                    t.budget = Convert.ToInt32(datareader["budget"].ToString());
                    t.status = datareader["status"].ToString();
                    t.employee_id = Convert.ToInt32(datareader["employee_id"]);
                    t.project_id = Convert.ToInt32(datareader["project_id"].ToString());
                    _tasklist.Add(t);
                }
                datareader.Close();
                datareader = null;
                return _tasklist;
            }
            catch
            {
                return null;
            }
        }
        public List<clients> GetClients(string CmdText) 
        {
            try
            {
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM clients ";
                }

                if (!(Connect()))
                {
                    return null;
                }


                datareader = GetReader(CmdText);
                List<clients> clientlist = new List<clients>();
                while (datareader.Read())
                {
                    clients clt = new clients();
                    clt.client_id = Convert.ToInt16(datareader["client_id"].ToString());
                    clt.client_name = datareader["client_name"].ToString();
                    clt.contact = datareader["contact"].ToString();
                    clt.email = datareader["email"].ToString();

                    clientlist.Add(clt);
                }
                datareader.Close();
                datareader = null;
                return clientlist;
            }
            catch
            {
                return null;
            }
        }
        public List<employees> GetEmployees(string CmdText)
        {
            try
            {
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM employees ";
                }
                // get all employees
                Connect();
                datareader = GetReader(CmdText);
                List<employees> employeeslist = new List<employees>();
                while (datareader.Read())
                {

                    employees emp = new employees();
                    emp.employee_name = datareader["employee_name"].ToString();
                    emp.employee_contact_number = datareader["employee_contact_number"].ToString();
                    emp.employee_id = Convert.ToInt32(datareader["employee_id"].ToString());
                    emp.password = datareader["password"].ToString();
                    employeeslist.Add(emp);
                }
                datareader.Close();
                datareader = null;

                return employeeslist;
            }
            catch
            {
                return null;
            }
        }
        public List<job_description> GetJobDescription(string CmdText)
        {
            try
            {
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM job_description ";
                }

                // get all employees jobs
                Connect();
                datareader = GetReader(CmdText);
                List<job_description> employeejoblist = new List<job_description>();
                while (datareader.Read())
                {

                    job_description jobdesc = new job_description();
                    jobdesc.job_description_id = Convert.ToInt32(datareader["job_description_id"].ToString());
                    jobdesc.job_type_id = Convert.ToInt32(datareader["job_type_id"].ToString());
                    jobdesc.employee_id = Convert.ToInt32(datareader["employee_id"]);
                    employeejoblist.Add(jobdesc);
                }
                datareader.Close();
                datareader = null;

                return employeejoblist;
            }
            catch { return null; }
        }
        public List<job_type> GetJobType(string CmdText)
        {
            try
            {
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM job_type";
                }
                // get all employees jobs
                Connect();
                datareader = GetReader(CmdText);
                List<job_type> joblist = new List<job_type>();
                while (datareader.Read())
                {

                    job_type job = new job_type();
                    job.job_name = datareader["job_name"].ToString();
                    job.unit_pay = Convert.ToInt32(datareader["unit_pay"].ToString());
                    job.job_type_id = Convert.ToInt32(datareader["job_type_id"]);
                    joblist.Add(job);
                }
                datareader.Close();
                datareader = null;

                return joblist;
            }
            catch
            {
                return null;
            }
        }
        public List<materials> GetMaterials(string CmdText) 
        {
            try 
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM materials";
                }
                

                datareader = GetReader(CmdText);
                List<materials> materialslist = new List<materials>();
                while (datareader.Read())
                {
                    materials mat = new materials();
                    mat.material_id = Convert.ToInt16(datareader["material_id"]);
                    mat.material_name = datareader["material_name"].ToString();
                    mat.measuring_unit = datareader["measuring_unit"].ToString();
                    materialslist.Add(mat);
                }
                datareader.Close();
                datareader = null;

                return materialslist; 
            }
            catch { return null; }
        }
        public List<stock_materials> GetStock(String CmdText) 
        {
            try
            {
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM stock_materials";
                }

                if (!Connect()) 
                {
                    return null; 
                }
                datareader = GetReader(CmdText);

                List<stock_materials> stock_materialslist = new List<stock_materials>();
                while (datareader.Read())
                {
                    stock_materials stock = new stock_materials();
                    stock.stock_id = Convert.ToInt32(datareader["stock_id"]);
                    stock.material_id = Convert.ToInt32(datareader["material_id"]);
                    stock.quanitity = Convert.ToInt32(datareader["quantity"]);
                    stock_materialslist.Add(stock);
                }
                datareader.Close();
                datareader = null;

                return stock_materialslist;
            }
            catch { return null; }
        }
        public List<labourers> GetLabourers(String CmdText)
        {
            try 
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM labourers";
                }
                
                datareader = GetReader(CmdText);
                List<labourers> labourers_list = new List<labourers>();
                while (datareader.Read()) 
                {
                    labourers labour = new labourers();
                    labour.labourer_id = Convert.ToInt32( datareader["labourer_id"].ToString());
                    labour.national_id = datareader["national_id"].ToString();
                    labour.labourer_contact = datareader["labourer_contact"].ToString();
                    labour.labourer_name = datareader["labourer_name"].ToString();
                    labourers_list.Add(labour);
                }
                datareader.Close();
                datareader = null;

                return labourers_list; 
            }
            catch { return null;  }
        }
        public List<project_materials> GetProjectMaterials(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM labourers";
                }

                datareader = GetReader(CmdText);
                List<project_materials> pmlist = new List<project_materials>();
                while (datareader.Read())
                {
                    project_materials pm = new project_materials();
                    pm.project_id = Convert.ToInt16(datareader["project_id"].ToString());
                    pm.material_id = Convert.ToInt16(datareader["material_id"].ToString());
                    pm.total_cost = Convert.ToInt16(datareader["cost"].ToString());
                    pm.material_name = datareader["material_name"].ToString();
                    pm.project_name = datareader["project_name"].ToString();
                    pm.total_quantity = Convert.ToInt16(datareader["quantity"].ToString());
                    pm.measuring_unit = datareader["measuring_unit"].ToString();
                    
                    pmlist.Add(pm);
                }
                datareader.Close();
                datareader = null;

                return pmlist;
            }
            catch { return null; }
        }
        public List<project_jobs> GetProjectJobs(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM labourers";
                }

                datareader = GetReader(CmdText);
                List<project_jobs> pjlist = new List<project_jobs>();
                while (datareader.Read())
                {
                    project_jobs pj = new project_jobs();
                    pj.project_id = Convert.ToInt16(datareader["project_id"].ToString());
                    pj.project_name = datareader["project_name"].ToString();
                    pj.job_name = datareader["job_name"].ToString();
                    pj.job_type_id = Convert.ToInt16(datareader["job_type_id"].ToString());
                    pj.employee_count = Convert.ToInt16(datareader["count"].ToString());
                    pjlist.Add(pj);
                }
                datareader.Close();
                datareader = null;

                return pjlist;
            }
            catch { return null; }
        }
        public List<project_labour_allocation> GetProjectLabourAllocation(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM project_labour_allocation";
                }

                datareader = GetReader(CmdText);
                List<project_labour_allocation> plalist = new List<project_labour_allocation>();
                while (datareader.Read())
                {
                    project_labour_allocation pla = new project_labour_allocation();
                    pla.project_labour_allocation_id = Convert.ToInt16(datareader["project_labour_allocation_id"].ToString());
                    pla.project_id = Convert.ToInt16(datareader["project_id"].ToString());
                    pla.labourer_id = Convert.ToInt16(datareader["labourer_id"].ToString());
                    
                    plalist.Add(pla);
                }
                datareader.Close();
                datareader = null;

                return plalist;
            }
            catch { return null; }
        }
        public List<project_labour_payment> GetProjectLabourPayment(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM project_labour_working_D";
                }

                datareader = GetReader(CmdText);
                List<project_labour_payment> plalist = new List<project_labour_payment>();
                while (datareader.Read())
                {
                    project_labour_payment pla = new project_labour_payment();
                    pla.project_labour_allocation_id = Convert.ToInt16(datareader["project_labour_allocation_id"].ToString());
                    pla.days_worked = Convert.ToInt16((datareader["days_worked"].ToString()));

                    plalist.Add(pla);
                }
                datareader.Close();
                datareader = null;

                return plalist;
            }
            catch { return null; }
        }
        public List<project_employee_payment> GetProjectEmployeePayment(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM project_employee_working_day";
                }

                datareader = GetReader(CmdText);
                List<project_employee_payment> plalist = new List<project_employee_payment>();
                while (datareader.Read())
                {
                    project_employee_payment pla = new project_employee_payment();
                    pla.project_employee_allocation_id = Convert.ToInt16(datareader["project_employee_allocation_id"].ToString());
                    pla.days_worked = Convert.ToInt16((datareader["days_worked"].ToString()));

                    plalist.Add(pla);
                }
                datareader.Close();
                datareader = null;

                return plalist;
            }
            catch { return null; }
        }
        public List<project_employee_allocation> GetProjectEmployeeAllocation(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM project_employee_allocation";
                }

                datareader = GetReader(CmdText);
                List<project_employee_allocation> plelist = new List<project_employee_allocation>();
                while (datareader.Read())
                {
                    project_employee_allocation ple = new project_employee_allocation();
                    ple.project_employee_allocation_id = Convert.ToInt16(datareader["project_employee_allocation_id"].ToString());
                    ple.project_id = Convert.ToInt16(datareader["project_id"].ToString());
                    ple.employee_id = Convert.ToInt16(datareader["employee_id"].ToString());

                    plelist.Add(ple);
                }
                datareader.Close();
                datareader = null;

                return plelist;
            }
            catch { return null; }
        }               
        public List<task_details> GetTaskDetails(String CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM labourers";
                }

                datareader = GetReader(CmdText);
                List<task_details> tdlist = new List<task_details>();
                while (datareader.Read())
                {
                    task_details td = new task_details();
                    td.task_id = Convert.ToInt16(datareader["task_id"].ToString());
                    td.project_id = Convert.ToInt16(datareader["project_id"].ToString());
                    td.task_name = datareader["task_name"].ToString();
                    td.project_name = datareader["project_name"].ToString();
                    td.duration = Convert.ToInt16(datareader["duration"].ToString());                    
                    td.foreman = datareader["employee_name"].ToString();
                    td.start_date = Convert.ToDateTime(datareader["start_date"].ToString());
                    td.end_date = Convert.ToDateTime(datareader["end_date"].ToString());                    
                    tdlist.Add(td);
                }
                datareader.Close();
                datareader = null;

                return tdlist;
            }
            catch { return null; }
        }
        public List<job_type_details> GetJobTypeDetails(String CmdText) 
        {
            try 
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT a.* , count(c.employee_id) AS 'employee_count' FROM job_type a , job_description b , employees c WHERE a.job_type_id = b.job_type_id AND b.employee_id = c.employee_id GROUP BY a.job_name";
                }

                datareader = GetReader(CmdText);
                List<job_type_details> jtdlist = new List<job_type_details>();
                while (datareader.Read())
                {
                    job_type_details jtd = new job_type_details();
                    jtd.employee_count = Convert.ToInt16(datareader["employee_count"].ToString());
                    jtd.job_name = datareader["job_name"].ToString();
                    jtd.job_type_id = Convert.ToInt16(datareader["job_type_id"].ToString());
                   
                    jtdlist.Add(jtd);
                }
                datareader.Close();
                datareader = null;

                return jtdlist;
            }
            catch { return null;  }
        }
        public List<project_employee_working_day> GetProjectEmployeeWorkingDay(string CmdText) 
        {
            try 
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM project_employee_working_day";
                }

                datareader = GetReader(CmdText);
                List<project_employee_working_day> pewdlist = new List<project_employee_working_day>();
                while (datareader.Read())
                {
                    project_employee_working_day pewd = new project_employee_working_day();
                    pewd.project_employee_allocation_id = Convert.ToInt16(datareader["project_employee_allocation_id"].ToString());
                    pewd.working_date = Convert.ToDateTime(datareader["working_date"]);                    
                    pewdlist.Add(pewd);
                }
                datareader.Close();
                datareader = null;

                return pewdlist;
            }
            catch
            {
                return null; 
            }
        }
        public List<project_labour_working_day> GetProjectLabourWorkingDay(string CmdText)
        {
            try
            {
                Connect();
                Debug.WriteLine(CmdText);
                if (CmdText == "")
                {
                    CmdText = "SELECT * FROM project_labour_working_day";
                }

                datareader = GetReader(CmdText);
                List<project_labour_working_day> plwdlist = new List<project_labour_working_day>();
                while (datareader.Read())
                {
                    project_labour_working_day plwd = new project_labour_working_day();
                    plwd.project_labour_allocation_id = Convert.ToInt16(datareader["project_labour_allocation_id"].ToString());
                    plwd.working_date = Convert.ToDateTime(datareader["working_date"]);
                    plwdlist.Add(plwd);
                }
                datareader.Close();
                datareader = null;

                return plwdlist;
            }
            catch
            {
                return null;
            }
        }
        #endregion
       
        
        public bool UpdateValue(stock_materials stock)
        {
            try
            {
                // establish connection
                if (Connect())
                {
                    // update a value
                    String CmdText = "UPDATE  stock_materials set quantity = " + stock.quanitity + " WHERE stock_id = " + stock.stock_id + ";";
                    Debug.WriteLine(CmdText);
                    cmd = new MySqlCommand(CmdText, this.conn);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else { return false; }
            }
            catch { return false; }

        }
        public bool UpdateValue(string CmdText)
        {
            try
            {
                // establish connection
                if (Connect())
                {
                    // update a value
                    //String CmdText = "UPDATE  stock_materials set quantity = " + stock.quanitity + " WHERE stock_id = " + stock.stock_id + ";";
                    Debug.WriteLine(CmdText);
                    cmd = new MySqlCommand(CmdText, this.conn);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
        }
        public bool UpdateProject(projects p) 
        {
            try 
            {
                string CmdText = "Update projects set project_id = @project_id , project_name = @project_name , location = @location , description = @description ,start_date = @start_date , end_date = @end_date , client_id = @client_id WHERE project_id = @project_id" ;
                cmd = new MySqlCommand(CmdText, this.conn);
                // add nodes to the values
                cmd.Parameters.AddWithValue("@project_id", p.project_id);
                cmd.Parameters.AddWithValue("@project_name", p.project_name);
                cmd.Parameters.AddWithValue("@location", p.location);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@start_date", p.start_date);
                cmd.Parameters.AddWithValue("@end_date", p.end_date);
                cmd.Parameters.AddWithValue("@client_id", p.client_id);
                Debug.WriteLine(cmd.CommandText.ToString());                                
                cmd.ExecuteNonQuery();
                return true;
            }
            catch 
            {
                return false;
            }

            
        }
        public bool UpdateTask(task t) 
        {
            try 
            {
                string CmdText = "UPDATE tasks set task_id = @task_id , project_id = @project_id , budget = @budget  , start_date =  @start_date  , end_date = @end_date , employee_id = @employee_id , task_name = @task_name WHERE task_id = " + t.task_id;
                cmd = new MySqlCommand(CmdText, this.conn);
                // add nodes to the values
                cmd.Parameters.AddWithValue("@task_id", t.task_id);
                cmd.Parameters.AddWithValue("@project_id", t.project_id);
                cmd.Parameters.AddWithValue("@budget", t.budget);
                cmd.Parameters.AddWithValue("@start_date", t.start_date);
                cmd.Parameters.AddWithValue("@end_date", t.end_date);
                cmd.Parameters.AddWithValue("@employee_id", t.employee_id);
                cmd.Parameters.AddWithValue("@task_name", t.task_name);
                Debug.WriteLine(cmd.CommandText.ToString());
                cmd.ExecuteNonQuery();

                return true;
            }
            catch 
            {
                return false; 
            }
        }
        public bool UpdateMaterial(materials m) 
        {
            try 
            {
                string CmdText = "UPDATE materials set material_name = @material_name , measuring_unit = @measuring_unit where material_id = " + m.material_id;
                cmd = new MySqlCommand(CmdText, this.conn);
                //add node values
                cmd.Parameters.AddWithValue("@material_name", m.material_name);
                cmd.Parameters.AddWithValue("@measuring_unit", m.measuring_unit);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch 
            {
                return false; 
            }
        }
        public bool UpdateEmployee(employees e) 
        {
            try 
            {
                string CmdText = "UPDATE employees set employee_name = @employee_name , employee_contact_number = @employee_contact_number where employee_id = " + e.employee_id;
                cmd = new MySqlCommand(CmdText, this.conn);
                cmd.Parameters.AddWithValue("@employee_name", e.employee_name);
                cmd.Parameters.AddWithValue("@employee_Contact_number", e.employee_contact_number);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false;  }
        }
        public bool UpdateValue(int NewQuantity, string MaterialsName)
        {
            try
            {
                // establish connection
                if (Connect())
                {
                    // get all stock materials with that name

                    stock_materials stock = ( GetStock("(SELECT a.* FROM stock_materials a , materials b WHERE a.material_id = b.material_id and b.material_name = '" + MaterialsName + "' )")).First();
                    
                    // update a value
                    stock.quanitity += NewQuantity; 
                    String CmdText = "UPDATE  stock_materials set quantity = " + stock.quanitity + " WHERE stock_id = " + stock.stock_id + ";";
                    Debug.WriteLine(CmdText);
                    cmd = new MySqlCommand(CmdText, this.conn);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else { return false; }
            }
            catch { return false; }

        }
        
    }

}
