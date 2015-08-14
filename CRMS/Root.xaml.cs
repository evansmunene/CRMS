using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;


namespace CRMS
{
    /// <summary>
    /// Interaction logic for Root.xaml
    /// </summary>
    public partial class Root : Window
    {
        #region global fields and variables
        private bool textboxstatus = false;
        bool FetchedWholeEntity = false;

        Container AllContainer = null;
        DBManager DB1 = new DBManager();


        public List<MaterialRequest> MR = new List<MaterialRequest>();
        #endregion

        public Root()
        {
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Server s = new Server();
            Initialization();
            PayLabour();
        }

        #region field handlers and checkers

        // method to check if any control has been left empty
        private void CheckForEmpty(Grid grid)
        {
            this.textboxstatus = true;
            foreach (var control in grid.Children)
            {
                if (control.GetType() == typeof(TextBox))
                {
                    ShowEmptyField((TextBox)control);
                }
                else if (control.GetType() == typeof(DatePicker))
                {
                    ShowEmptyField((DatePicker)control);
                }
            }
        }
        private void CheckForEmpty(StackPanel grid)
        {
            this.textboxstatus = true;
            foreach (var control in grid.Children)
            {
                if (control.GetType() == typeof(TextBox))
                {
                    ShowEmptyField((TextBox)control);
                }
                else if (control.GetType() == typeof(DatePicker))
                {
                    ShowEmptyField((DatePicker)control);
                }
                else if (control.GetType() == typeof(AutoCompleteBox))
                {
                    ShowEmptyField((AutoCompleteBox)control);
                }
            }
        }

        //method that clears data from all conrols in a container
        private void EmptyAllfields(StackPanel grid)
        {
            foreach (var control in grid.Children)
            {
                
                if (control.GetType() == typeof(TextBox))
                {
                    EmptyControls((TextBox)control);
                }
                else if (control.GetType() == typeof(DatePicker))
                {
                    EmptyControls((DatePicker)control);
                }
                else if (control.GetType() == typeof(AutoCompleteBox))
                {
                    EmptyControls((AutoCompleteBox)control);
                }
            }
        }

        // methods that shows the control that has been left empty 
        private void ShowEmptyField(AutoCompleteBox control)
        {
            try
            {
                if (control.Text == "")
                {
                    control.BorderBrush = System.Windows.Media.Brushes.Red;
                    control.BorderThickness = new Thickness(3, 3, 3, 3);
                    this.textboxstatus = false;
                }
            }
            catch
            {
            }

        }
        private void ShowEmptyField(TextBox control)
        {
            try
            {
                if (control.Text == "")
                {
                    control.BorderBrush = System.Windows.Media.Brushes.Red;
                    control.BorderThickness = new Thickness(3, 3, 3, 3);
                    this.textboxstatus = false;
                }
            }
            catch
            {
            }

        }
        private void ShowEmptyField(DatePicker control)
        {
            try
            {
                if (control.SelectedDate == null)
                {
                    control.BorderBrush = System.Windows.Media.Brushes.Red;
                    control.BorderThickness = new Thickness(3, 3, 3, 3);
                    this.textboxstatus = false;
                }
            }
            catch
            {
            }

        }

        //methods that clear data from all fiels
        private void EmptyControls(AutoCompleteBox control)
        {
            try
            {
                control.Text = "";
                control.BorderBrush = System.Windows.Media.Brushes.Black;
                control.BorderThickness = new Thickness(1, 1, 1, 1);      
            }
            catch { }


        }
        private void EmptyControls(TextBox control)
        {
            try
            {
                control.Text = "";
                control.BorderBrush = System.Windows.Media.Brushes.Black;
                control.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            catch { }


        }
        private void EmptyControls(DatePicker control)
        {
            try
            {
                control.Text = "";
                control.BorderBrush = System.Windows.Media.Brushes.Black;
                control.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            catch { }

        }
        #endregion

        #region universal
        private void Initialization()
        {
            RefreshContainer();
            ComboboxPopulating(this.AssignProjectEmployeeJobTypeComboBox, null);
            ComboboxPopulating(this.UpdateProjectProjectNameComboBox, null);
            ComboboxPopulating(this.UpdateMaterialMaterialNameComboBox, null);
            ComboboxPopulating(this.UpdateEmployeeEmployeeNameComboBox, null);
            Summaries();
            
            
        }
        private void Summaries()
        {
            ProjectSummary();
            TasksSummary();
            EmployeeSummary();
        }
        private void RefreshContainer()
        {
            DBManager DB1 = new DBManager();

            this.AllContainer = DB1.GetWholeEntity();
        }
        private void AutoCompletePopulating(object sender, PopulatingEventArgs e)
        {
            try
            {
                AutoCompleteBox control = (AutoCompleteBox)sender;

                if (this.AllContainer._employeeslist.Count == 0)
                {
                    RefreshContainer();
                }

                switch (control.Tag.ToString())
                {
                    case Constants.employees:
                        try
                        {
                            // get a list of jobs
                            var result = from item in AllContainer._employeeslist
                                         where (item.employee_name.ToLower().Contains(control.Text.ToLower()))
                                         select item;
                            //List<employees> c = (List<employees>)result.ToList();
                            List<string> names = (from n in (List<employees>)result.ToList() select (n.employee_name)).ToList();
                            if (result != null)
                            {
                                control.ItemsSource = names;
                                control.PopulateComplete();
                                sender = control;
                            }
                            else
                            {
                                // show all

                                control.ItemsSource = (from n in AllContainer._employeeslist select (n.employee_name)).ToList();
                                control.PopulateComplete();
                                sender = control;
                            }


                        }
                        catch { }
                        break;

                    case Constants.Job_Type:
                        try
                        {
                            var result = from item in AllContainer._job_typelist
                                         where (item.job_name.ToLower().Contains(control.Text.ToLower()))
                                         select item;
                            List<string> names = (from n in (List<job_type>)result.ToList() select (n.job_name)).ToList();
                            if (result != null)
                            {
                                control.ItemsSource = names;
                                control.PopulateComplete();
                                sender = control;
                            }
                            else
                            {
                                // show all
                                names = (from n in AllContainer._job_typelist select (n.job_name)).ToList();
                                control.ItemsSource = names;
                                control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;
                    case Constants.Client:
                        try
                        {
                            // get a list of client names
                            var result = from item in AllContainer._clientlist
                                         where (item.client_name.ToLower().Contains(control.Text.ToLower()))
                                         select item;
                            List<string> names = (from _client in (List<clients>)result.ToList() select (_client.client_name)).ToList();
                            if (result != null)
                            {
                                control.ItemsSource = names;
                                control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;

                    case Constants.Project:
                        try
                        {
                            // get a list of all projects
                            var result = from item in AllContainer._projectlist
                                         where (item.project_name.ToLower().Contains(control.Text.ToLower()))
                                         select item;
                            if (result != null)
                            {
                                control.ItemsSource = (from _project in (List<projects>)result.ToList() select (_project.project_name)).ToList();
                                control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;
                    case Constants.Materials:
                        try
                        {
                            // get the list of all materials
                            var result = from item in AllContainer._materialslist
                                         where (item.material_name.ToLower().Contains(this.RegisterStockMaterialNameTextBox.Text))
                                         select item;
                            if (result != null)
                            {
                                control.ItemsSource = (from _mat in (List<materials>)result.ToList() select (_mat.material_name)).ToList();
                                control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;
                }
            }
            catch { }
        }
        private void ComboboxPopulating(object sender, PopulatingEventArgs e)
        {
            try
            {
                ComboBox control = (ComboBox)sender;

                if (this.AllContainer._employeeslist.Count == 0)
                {
                    RefreshContainer();
                }

                switch (control.Tag.ToString())
                {
                    case Constants.employees:
                        try
                        {
                            // get a list of jobs
                            var result = from item in AllContainer._employeeslist                                         
                                         select item;
                            //List<employees> c = (List<employees>)result.ToList();
                            List<string> names = (from n in (List<employees>)result.ToList() select (n.employee_name)).ToList();
                            if (result != null)
                            {
                                control.ItemsSource = names;
                                //control.PopulateComplete();
                                sender = control;
                            }
                            else
                            {
                                // show all

                                control.ItemsSource = (from n in AllContainer._employeeslist select (n.employee_name)).ToList();
                                //control.PopulateComplete();
                                sender = control;
                            }


                        }
                        catch { }
                        break;

                    case Constants.Job_Type:
                        try
                        {
                            var result = from item in AllContainer._job_typelist                                         
                                         select item;
                            List<string> names = (from n in (List<job_type>)result.ToList() select (n.job_name)).ToList();
                            if (result != null)
                            {
                                control.ItemsSource = names;
                                //control.PopulateComplete();
                                sender = control;
                            }
                            else
                            {
                                // show all
                                names = (from n in AllContainer._job_typelist select (n.job_name)).ToList();
                                control.ItemsSource = names;
                                //control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;
                    case Constants.Client:
                        try
                        {
                            // get a list of client names
                            var result = from item in AllContainer._clientlist                                         
                                         select item;
                            List<string> names = (from _client in (List<clients>)result.ToList() select (_client.client_name)).ToList();
                            if (result != null)
                            {
                                control.ItemsSource = names;
                                //control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;

                    case Constants.Project:
                        try
                        {
                            // get a list of all projects
                            var result = from item in AllContainer._projectlist                                         
                                         select item;
                            if (result != null)
                            {
                                control.ItemsSource = (from _project in (List<projects>)result.ToList() select (_project.project_name)).ToList();
                                //control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;
                    case Constants.Materials:
                        try
                        {
                            // get the list of all materials
                            var result = from item in AllContainer._materialslist                                         
                                         select item;
                            if (result != null)
                            {
                                control.ItemsSource = (from _mat in (List<materials>)result.ToList() select (_mat.material_name)).ToList();
                                //control.PopulateComplete();
                                sender = control;
                            }
                        }
                        catch { }
                        break;
                }
            }
            catch { }
        }
        private void TaskPopulator(object sender, PopulatingEventArgs e)
        {
            try
            {

                AutoCompleteBox control = (AutoCompleteBox)sender;

                if (this.AllContainer._employeeslist.Count == 0)
                {
                    RefreshContainer();
                }


                try
                {
                    // get the project from the text box
                    try
                    {
                        // get a list of all projects
                        var projectresult = from item in AllContainer._projectlist
                                            where (item.project_name.ToLower().Contains(this.AssignTaskMaterialsProjectNameTextBox.Text.ToLower()))
                                            select item;
                        if (projectresult != null)
                        {
                            // get the first project
                            projects p = (projects)projectresult.ToList().First();

                            // get the list of tasks with its id
                            var taskresult = from item in AllContainer._tasklist
                                             where ((item.task_name.ToLower().Contains(control.Text.ToLower())) && (item.project_id == p.project_id))
                                             select item;

                            control.ItemsSource = (from _task in (List<task>)taskresult.ToList() select (_task.task_name)).ToList();
                            control.PopulateComplete();
                            sender = control;
                        }
                    }
                    catch { }
                }
                catch { }

            }
            catch { }
        }
        #endregion

        #region option pane handlers
        private void OptionButtonHander(object sender, RoutedEventArgs e)
        {
            try
            {
                //get the parent of the button that send the value
                Button OptionButton = (Button)sender;
                UniformGrid ParentUniformGrid = (UniformGrid)OptionButton.Parent;
                Grid ParentGrid = (Grid)ParentUniformGrid.Parent;

                switch (ParentGrid.Name)
                {
                    case Constants.Projects_Grid:
                        SwitchProjectGrid(OptionButton);
                        break;
                    case Constants.Tasks_Grid:
                        SwitchTask(OptionButton);
                        break;
                    case Constants.Employees_Grid:
                        SwitchEmployeesGrid(OptionButton);
                        break;
                    case Constants.Resources_Grid:
                        SwitchResourcesGrid(OptionButton);
                        break;
                    case Constants.Clients_Grid:
                        break;
                    case Constants.Progress_Grid:
                        SwitchProgressGrid(OptionButton);
                        break;
                }
                try
                {
                    StackPanel x = (StackPanel)this.FindName(OptionButton.Tag.ToString());
                    Grid ab = (Grid)x.Parent;
                    MakeVisible(x);
                }
                catch { }
                try
                {
                    Grid x = (Grid)this.FindName(OptionButton.Tag.ToString());
                    Grid ab = (Grid)x.Parent;
                    MakeVisible(x);
                }
                catch { }

            }
            catch { }
        }
        private void SwitchProjectGrid(Button Control)
        {
            try
            {
                //make all other grids invisble within the project content page
                this.RegisterProjectGrid.Visibility = System.Windows.Visibility.Collapsed;
                //this.ProjectsSummaryGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.AssignProjectEmployeeGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.UpdateProjectGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch { }
        }
        private void SwitchEmployeesGrid(Button Control)
        {
            try
            {
                //make all the grid invisible                
                this.RegisterEmployeeGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.AssignEmplyeeJobGrid.Visibility = System.Windows.Visibility.Collapsed;
                //this.AssignProjectEmployeeGrid.Visibility = System.Windows.Visibility.Collapsed;
                //this.EmployeesSummaryGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.UpdateEmployeeGrid.Visibility = System.Windows.Visibility.Collapsed; 
            }
            catch { }
        }
        private void SwitchResourcesGrid(Button Control)
        {
            try
            {
                // make all grid invisible                
                this.RegisterStockGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.RegisterMaterialsGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.UpdateMaterialsGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch { }
        }
        private void SwitchTask(Button Control)
        {
            try
            {
                this.RegisterTaskGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.AssignTaskMaterialsGrid.Visibility = System.Windows.Visibility.Collapsed;
                //this.TasksSummaryGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.UpdateTasksGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch { }
        }
        private void SwitchProgressGrid(Button Control) 
        {
            try 
            {
                this.ProjectsSummaryGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.TasksSummaryGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.EmployeesSummaryGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch { }
        }
        #endregion

        #region category button handlers
        private void CategoryButton_click(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the button that sent the request
                Button btn = (Button)sender;
                // get the tag of the button                 
                //make the grid visible
                Initialization();
                MakeGridVisible(btn.Tag.ToString());
            }
            catch { }
        }
        private void MakeGridVisible(string control)
        {
            try
            {
                // make all the grids or containers to be invisible
                this.ProjectsGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.TasksGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.ResourcesGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.EmployeesGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.ClientsGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.ProgressGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.ReportssGrid.Visibility = System.Windows.Visibility.Collapsed;

                var c = this.FindName(control);
                Grid x = (Grid)c;
                MakeVisible(x);
            }
            catch { }
        }
        private void MakeVisible(StackPanel Control)
        {
            try
            {
                Control.Visibility = System.Windows.Visibility.Visible;
            }
            catch { }
        }
        private void MakeVisible(Grid Control)
        {
            try
            {
                Control.Visibility = System.Windows.Visibility.Visible;
            }
            catch { }
        }
        #endregion

        #region Project  handlers
        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            // get all the values from the 
            // check if any of the values is left empty
            try
            {

                // check for empty ness
                CheckForEmpty(this.RegisterProjectGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }


                try
                {

                    Container Entity1 = new Container();
                    Entity1._project.project_id = 0;
                    Entity1._project.project_name = this.RegisterProjectProjectNameTextBox.Text;
                    Entity1._project.location = this.RegisterProjectLocationTextBox.Text;
                    Entity1._project.description = this.RegisterProjectDescriptionTextBox.Text;
                    Entity1._project.end_date = (DateTime)this.RegisterProjectEndDatePicker.SelectedDate;
                    Entity1._project.start_date = (DateTime)this.RegisterProjectStartDatePicker.SelectedDate;

                    // get the client 
                    try
                    {
                        Container container1 = new Container();
                        DB1 = new DBManager();
                        container1 = DB1.GetWholeEntity();
                        List<clients> cc = container1._clientlist;
                        var result = from item in cc
                                     where item.client_name.ToLower().Contains(this.RegisterProjectClientTextBox.Text.ToLower())
                                     select item;

                        if (result != null)
                        {
                            Entity1._project.client_id = Convert.ToInt32(((List<clients>)result.ToList()).First().client_id);
                        }


                    }
                    catch { }

                    if (DB1.InsertValue(Entity1, Constants.Project))
                    {
                        MessageBox.Show("The Project has been registered successfully");
                        EmptyAllfields(this.RegisterProjectGrid);
                    }
                }
                catch { }

            }
            catch { }

        }
        private void UpdateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check for empty ness
                CheckForEmpty(this.UpdateProjectGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }
                DB1 = new DBManager();

                Container Entity1 = new Container();
                Entity1._project.project_id = DB1.GetProjects("SELECT * FROM projects WHERE project_name = '" + this.UpdateProjectProjectNameComboBox.SelectedItem.ToString() + "'").First().project_id;
                Entity1._project.project_name = this.UpdateProjectProjectNameTextBox.Text;
                Entity1._project.location = this.UpdateProjectLocationTextBox.Text;
                Entity1._project.description = this.UpdateProjectDescriptionTextBox.Text;
                Entity1._project.end_date = (DateTime)this.UpdateProjectEndDatePicker.SelectedDate;
                Entity1._project.start_date = (DateTime)this.UpdateProjectStartDatePicker.SelectedDate;


                // get the client 
                try
                {
                    List<clients> cc = DB1.GetClients("");
                    var result = from item in cc
                                 where item.client_name.ToLower().Contains(this.RegisterProjectClientTextBox.Text.ToLower())
                                 select item;

                    if (result != null)
                    {
                        Entity1._project.client_id = Convert.ToInt32(((List<clients>)result.ToList()).First().client_id);
                    }

                }
                catch { }

                if (DB1.UpdateProject(Entity1._project))
                {
                    MessageBox.Show("Project Successfully updated");
                    EmptyAllfields(this.UpdateProjectGrid);
                    Initialization();
                    //this.UpdateProjectProjectNameComboBox = new ComboBox();
                    //ComboboxPopulating(this.UpdateProjectProjectNameComboBox, null);
                }
                
            }
            catch { }
        }
        private void ProjectListviewItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the project clicked
                //projects pr = (projects)(sender as FrameworkElement).DataContext;
                ListView l = (ListView)sender;
                projects pr = (projects)l.SelectedItem;

                this.ProjectResourcesListView.ItemsSource = DB1.GetProjectMaterials("SELECT a.*, d.*, SUM(b.quantity) AS quantity, SUM(b.quantity * b.unit_buying_price) AS Cost FROM materials a , task_materials b , tasks c , projects d WHERE a.material_id = b.material_id AND b.task_id = c.task_id AND c.project_id = d.project_id AND d.project_id = " + pr.project_id + " Group BY a.material_name");
                this.ProjectJobsListView.ItemsSource = DB1.GetProjectJobs("SELECT a.* , e.* , count(b.job_type_id) AS count FROM job_type a , job_description b , employees c , project_employee_allocation d , projects e WHERE a.job_type_id = b.job_type_id AND b.employee_id = c.employee_id AND c.employee_id = d.employee_id AND d.project_id = e.project_id AND e.project_id =  " + pr.project_id + " GROUP BY a.job_name");
                this.ProjectTasksListView.ItemsSource = DB1.GetTasks("SELECT * FROM tasks WHERE project_id = " + pr.project_id);
            }
            catch { }
        }
        private void ProjectSummary()
        {
            // get a summary of all data relating to the projects
            try
            {
                // get a list of all running projects
                try
                {
                    DB1 = new DBManager();
                    this.RunningProjectsListView.ItemsSource = DB1.GetProjects("SELECT * FROM projects WHERE start_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND end_date > '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "'");
                    Debug.WriteLine("SELECT * FROM projects WHERE start_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND end_date > '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "'");
                    ProjectListviewItem_Click(null, null);
                    this.ProjectsDatagrid.ItemsSource = DB1.GetRawData("SELECT project_name AS 'ALL PROJECTS' FROM projects ").DefaultView;
                }
                catch { }
            }
            catch { }
        }
        private void UpdateProjectProjectNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DB1 = new DBManager();
                projects p = DB1.GetProjects("SELECT * FROM projects WHERE project_name = '" + ((ComboBox)sender).SelectedItem.ToString() + "'").First();
                clients c = DB1.GetClients("SELECT a.* FROM clients a , projects b WHERE a.client_id = b.client_id AND a.client_id = " + p.client_id).First();
                Debug.WriteLine("SELECT a.* FROM clients a , projects b WHERE a.client_id = b.client_id AND a.client_id = " + p.client_id);
                //populate fields with values
                this.UpdateProjectProjectNameTextBox.Text = p.project_name;
                this.UpdateProjectDescriptionTextBox.Text = p.description;
                this.UpdateProjectLocationTextBox.Text = p.location;
                this.UpdateProjectStartDatePicker.SelectedDate = p.start_date;
                this.UpdateProjectEndDatePicker.SelectedDate = p.end_date;
                this.UpdateProjectClientTextBox.Text = c.client_name;


            }
            catch { }
        }
        #endregion

        #region employee  handlers
        private void RegisterEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check for empty value
                CheckForEmpty(this.RegisterEmployeeGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }
            }
            catch
            { }
            try
            {
                // get the name of the employee
                Container Entity1 = new Container();
                Entity1._employees.employee_id = 0;
                Entity1._employees.employee_name = this.RegisterEmployeeNameTextBox.Text;
                Entity1._employees.employee_contact_number = this.RegisterEmployeeContactNumberTextBox.Text;
                DBManager DB1 = new DBManager();
                if (DB1.InsertValue(Entity1, Constants.employees))
                {
                    MessageBox.Show(this.RegisterEmployeeNameTextBox.Text + " has been successfully registered");
                    EmptyAllfields(this.RegisterEmployeeGrid);
                    RefreshContainer();
                }
                else
                {
                    MessageBox.Show("There was a problem entering data to the database");
                    EmptyAllfields(this.RegisterEmployeeGrid);
                    RefreshContainer();
                }
            }
            catch { }
        }
        private void AssignEmployeeJobRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckForEmpty(this.AssignEmplyeeJobGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }

                // look for fields in the tables
                AllContainer = new Container();
                DB1 = new DBManager();
                AllContainer = DB1.GetWholeEntity();

                var result = from item in AllContainer._employeeslist
                             where item.employee_name.ToLower().Contains(this.AssignEmployeejobEmployeeNameTextbox.Text.ToLower())
                             select item;
                // get the id of the first value found
                employees emp = (employees)result.ToList().First();

                var rs = from item in AllContainer._job_typelist
                         where item.job_name.ToLower().Contains(this.AssignEmployeeJobJobNameTextBox.Text.ToLower())
                         select item;
                job_type job = (job_type)rs.ToList().First();


                // get the values from the two entities so as to merge them
                Container Entity1 = new Container();
                Entity1._job_description.employee_id = emp.employee_id;
                Entity1._job_description.job_type_id = job.job_type_id;
                if (DB1.InsertValue(Entity1, Constants.Job_Description))
                {
                    MessageBox.Show("the job description has been registered");
                    EmptyAllfields(this.AssignEmplyeeJobGrid);
                    RefreshContainer();
                }
            }
            catch { }
        }
        private void AssignProjectEmployeeJobTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // get a list of employees who fit the description
                DB1 = new DBManager();
                List<employees> em = DB1.GetEmployees("SELECT a.* FROM employees a , job_description b , job_type c WHERE a.employee_id = b.employee_id AND  b.job_type_id = c.job_type_id AND c.job_name = '" + this.AssignProjectEmployeeJobTypeComboBox.SelectedItem.ToString() + "'");
                Debug.WriteLine("SELECT a.* FROM employees a , job_description b , job_type c WHERE a.employee_id = b.employee_id AND  b.job_type_id = c.job_type_id AND c.job_name = '" + this.AssignProjectEmployeeJobTypeComboBox.SelectedItem.ToString() + "'");
                this.AssignProjectEmployeeEmployeeNameComboBox.ItemsSource = (List<string>)(from item in em select item.employee_name).ToList();
            }
            catch { }
        }
        private void AssignProjectEmployeeEmployeeNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // get a list of all projects the employee is in 

                DB1 = new DBManager();
                this.CommitedProjectsListView.ItemsSource = DB1.GetProjects("SELECt a.* from projects a , project_employee_allocation b , employees c where a.project_id = b.project_id AND b.employee_id = c.employee_id and c.employee_name = '" + ((ComboBox)sender).SelectedItem.ToString() + "'");

            }
            catch { }
        }
        private void AssignProjectEmployeeAssignButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check for empty value
                CheckForEmpty(this.AssignProjectEmployeeGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }
            }
            catch
            { }
            try
            {
                // get the details of employee and project
                DB1 = new DBManager();
                projects prj = (projects)DB1.GetProjects("SELECT * FROM projects where project_name = '" + this.AssignProjectEmployeeProjectNameTextBox.Text + "'").First();
                employees emp = (employees)DB1.GetEmployees("SELECT * FROM employees WHERE employee_name = '" + this.AssignProjectEmployeeEmployeeNameComboBox.SelectedItem.ToString() + "'").First();
                Container c = new Container();
                c._project_employee_allocation = new project_employee_allocation();
                c._project_employee_allocation.employee_id = emp.employee_id;
                c._project_employee_allocation.project_id = prj.project_id;

                if (DB1.InsertValue(c, Constants.Project_Employee_Allocation))
                {
                    MessageBox.Show("Employee allocated");
                    EmptyAllfields(this.AssignProjectEmployeeGrid);
                    RefreshContainer();
                }
            }
            catch { }
        }
        private void PayLabour()
        {
            try
            {
                //check if the day is a saturday
                if (!(DateTime.Now.DayOfWeek == DayOfWeek.Saturday))
                {
                    return;
                }
                //Debug.WriteLine("select * from project_employee_working_day where  working_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND working_date > '" + DateTime.Now.AddDays(-7).ToString("yyyy'-'MM'-'dd") + "'");

                MessageBox.Show("Making payments for the weeks wages");
                List<project_employee_payment> p = DB1.GetProjectEmployeePayment("select  project_employee_allocation_id , count(project_employee_allocation_id) as days_worked from project_employee_working_day where  working_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND working_date > '" + DateTime.Now.AddDays(-7).ToString("yyyy'-'MM'-'dd") + "' group by project_employee_allocation_id");

                foreach (project_employee_payment pep in p) 
                {
                    // get the employee id
                    //Debug.WriteLine(" SELECT a.* FROM job_type a , job_description b , employees c , project_employee_allocation d WHERE a.job_type_id = b.job_type_id AND b.employee_id = c.employee_id AND c.employee_id = d.employee_id AND d.project_employee_allocation_id = " + pep.project_employee_allocation_id);
                    project_employee_payment p1 = pep ;
                    int rate = DB1.GetJobType(" SELECT a.* FROM job_type a , job_description b , employees c , project_employee_allocation d WHERE a.job_type_id = b.job_type_id AND b.employee_id = c.employee_id AND c.employee_id = d.employee_id AND d.project_employee_allocation_id = " +pep.project_employee_allocation_id).First().unit_pay;
                    p1.amount = rate * p1.days_worked;
                    p1.date_paid = DateTime.Now.Date;

                    if(DB1.InsertEmployeePayment(p1))
                    {}
                }




                List<project_labour_payment> lp = DB1.GetProjectLabourPayment("select  project_labour_allocation_id , count(project_labour_allocation_id) as days_worked from project_labour_working_day where  working_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND working_date > '" + DateTime.Now.AddDays(-7).ToString("yyyy'-'MM'-'dd") + "' group by project_labour_allocation_id");
                Debug.WriteLine("select  project_labour_allocation_id , count(project_labour_allocation_id) as days_worked from project_labour_working_day where  working_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND working_date > '" + DateTime.Now.AddDays(-7).ToString("yyyy'-'MM'-'dd") + "' group by project_labour_allocation_id");
                foreach (project_labour_payment plp in lp)
                {
                    // get the labour id
                    //Debug.WriteLine(" SELECT a.* FROM job_type a , job_description b , labours c , project_labour_allocation d WHERE a.job_type_id = b.job_type_id AND b.labour_id = c.labour_id AND c.labour_id = d.labour_id AND d.project_labour_allocation_id = " + pep.project_labour_allocation_id);
                    project_labour_payment lp1 = plp;
                    //int rate = DB1.GetJobType(" SELECT a.* FROM job_type a , job_description b , labours c , project_labour_allocation d WHERE a.job_type_id = b.job_type_id AND b.labour_id = c.labour_id AND c.labour_id = d.labour_id AND d.project_labour_allocation_id = " + plp.project_labour_allocation_id).First().unit_pay;
                    lp1.amount = 500 * lp1.days_worked;
                    lp1.date_paid = DateTime.Now.Date;

                    if (DB1.InsertLabourPayment(lp1))
                    { }
                }


                
            }
            catch { }
        }
        private void EmployeeSummary() 
        {
            try 
            {
                // get all the job typesavailable
                DB1 = new DBManager();
                this.JobTypesListView.ItemsSource = DB1.GetJobTypeDetails("");
            }
            catch { }
        }
        private void JobTypesListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try 
            {
                ListView l = (ListView)sender;
                job_type_details jtd =(job_type_details) l.SelectedItem;

                //show the employee details
                DB1 = new DBManager();
                this.JobTypeEmployeeListView.ItemsSource = DB1.GetEmployees("SELECT a.* from employees a , job_description b , job_type c WHERE a.employee_id = b.employee_id AND b.job_type_id = c. job_type_id AND c.job_name = '" + jtd.job_name + "'");

            }
            catch { }
        }

        private void JobTypeEmployeeListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try 
            {
                ListView l = (ListView) sender ;
                employees emp = (employees) l.SelectedItem;
                Debug.WriteLine("SELECt a.* from projects a , project_employee_allocation b , employees c where a.project_id = b.project_id AND b.employee_id = c.employee_id and c.employee_name = '" + emp.employee_name + "'");
                this.EmployeeProjectsListView.ItemsSource = DB1.GetProjects("SELECt a.* from projects a , project_employee_allocation b , employees c where a.project_id = b.project_id AND b.employee_id = c.employee_id and c.employee_name = '" + emp.employee_name + "'");
            }
            catch { }
        }
        private void UpdateEmployeeEmployeeNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                DB1 = new DBManager();
                employees emp = DB1.GetEmployees("SELECT * from employees where employee_name = '" + ((ComboBox)sender).SelectedItem.ToString() + "'").First();

                this.UpdateEmployeeNameTextBox.Text= emp.employee_name.ToString();
                this.UpdateEmployeeContactNumberTextBox.Text = emp.employee_contact_number.ToString();
            }
            catch { }
        }

        private void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                employees emp = DB1.GetEmployees("SELECT * from employees where employee_name = '" + ((ComboBox)this.UpdateEmployeeEmployeeNameComboBox).SelectedItem.ToString() + "'").First();
                emp.employee_name = this.UpdateEmployeeNameTextBox.Text;
                emp.employee_contact_number = this.UpdateEmployeeContactNumberTextBox.Text;

                if (DB1.UpdateEmployee(emp)) 
                {
                    MessageBox.Show("Employee details have been updated");
                    EmptyAllfields(this.UpdateEmployeeGrid);
                    Initialization();
                }
            }
            catch { }
        }
        #endregion

        #region client  handers
        private void RegisterClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    // check for empty value
                    CheckForEmpty(this.RegisterClientGrid);
                    if (this.textboxstatus == false)
                    {
                        MessageBox.Show("A field has not been entered");
                        textboxstatus = false;
                        return;
                    }
                }
                catch
                { }

                // get the name of the client
                Container Entity1 = new Container();
                Entity1._client.client_name = this.RegisterClientNameTextBox.Text;
                Entity1._client.email = this.RegisterClientEmailTextBox.Text;
                Entity1._client.contact = this.RegisterClientEmailTextBox.Text;
                Entity1._client.client_id = 0;
                DBManager DB1 = new DBManager();
                if (DB1.InsertValue(Entity1, Constants.Client))
                {
                    MessageBox.Show("has been successfully registered");
                    EmptyAllfields(this.RegisterClientGrid);
                    RefreshContainer();
                }
            }
            catch { }
        }
        #endregion

        #region resources handlers
        private void RegisterMaterialCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check for empty value
                CheckForEmpty(this.RegisterMaterialsGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }

                // get the values from the fields
                Container Entity1 = new Container();
                Entity1._materials.material_name = this.RegisterMaterialMaterialNameTextBox.Text;
                Entity1._materials.measuring_unit = this.RegisterMaterialMeasuringUnitTextBox.Text;
                DB1 = new DBManager();
                if (DB1.InsertValue(Entity1, Constants.Materials))
                {
                    MessageBox.Show("has been successfully registered");
                    EmptyAllfields(this.RegisterMaterialsGrid);
                    RefreshContainer();
                }
            }
            catch { }
        }
        private void RegisterStockCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check for empty value
                CheckForEmpty(this.RegisterStockGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }

                // check if the material already exists
                if (this.AllContainer._employeeslist.Count == 0)
                {
                    RefreshContainer();
                }

                // get the list of all materials
                var result = from item in AllContainer._materialslist
                             where (item.material_name.ToLower().Contains(this.RegisterStockMaterialNameTextBox.Text))
                             select item;
                if (result.Count() > 0)
                {
                    // get the id of the material
                    int material_id = (from _mat in (List<materials>)result.ToList() select (_mat.material_id)).ToList().First();

                    //find the row with the material id provided
                    var s = from item in AllContainer._stock_materialslist
                            where (item.material_id == material_id)
                            select item;
                    if (s.Count() > 0)
                    {
                        stock_materials stock = ((List<stock_materials>)s.ToList()).First();

                        // change the values of the quantity of the stock
                        stock.quanitity += Convert.ToInt32(this.RegisterStockQuantityTextBox.Text);

                        //update the db

                        if (DB1.UpdateValue(stock))
                        {
                            MessageBox.Show("Stock has been updated");
                        }
                        EmptyAllfields(this.RegisterStockGrid);
                    }
                    else
                    {
                        // create a new row for the material
                        Container Entity1 = new Container();
                        Entity1._stock_materials = new stock_materials();
                        Entity1._stock_materials.material_id = material_id;
                        Entity1._stock_materials.quanitity = Convert.ToInt32(this.RegisterStockQuantityTextBox.Text);
                        DB1 = new DBManager();
                        if (DB1.InsertValue(Entity1, Constants.Stock_Materails))
                        {
                            MessageBox.Show("New value entered successfull");
                            EmptyAllfields(this.RegisterStockGrid);
                            RefreshContainer();
                        }
                    }
                }


            }
            catch { }
        }
        private void UpdateMaterialMaterialNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                materials m = DB1.GetMaterials("SELECT * FROM materials WHERE material_name = '" + this.UpdateMaterialMaterialNameComboBox.SelectedItem.ToString() + "'").First();
                this.UpdateMaterialMaterialNameTextBox.Text = m.material_name;
                this.UpdateMaterialMeasuringUnitTextBox.Text = m.measuring_unit; 

                
            }
            catch { }
        }
        private void UpdateMaterialCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                materials m = DB1.GetMaterials("SELECT * FROM materials WHERE material_name = '" + this.UpdateMaterialMaterialNameComboBox.SelectedItem.ToString() + "'").First();
                m.material_name = this.UpdateMaterialMaterialNameTextBox.Text;
                m.measuring_unit = this.UpdateMaterialMeasuringUnitTextBox.Text;

                if (DB1.UpdateMaterial(m)) 
                {
                    MessageBox.Show("material has been successfully updated");
                    EmptyAllfields(this.UpdateMaterialsGrid); 
                    Initialization();
                }
            }
            catch { }
        }

        #endregion

        #region task handler
        private void RegisterTaskCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshContainer();
                // check for empty value
                CheckForEmpty(this.RegisterTaskGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }

                // get the project id
                // check if the material already exists
                if (this.AllContainer._employeeslist.Count == 0)
                {
                    RefreshContainer();
                }

                var result = from item in AllContainer._projectlist
                             where (item.project_name.Contains(this.RegisterTaskProjectNameTextBox.Text))
                             select (item.project_id);
                int p_id = ((List<int>)result.ToList()).First();
                result = from item in AllContainer._employeeslist
                         where (item.employee_name.Contains(this.RegisterTaskForemanTextBox.Text))
                         select (item.employee_id);
                int e_id = ((List<int>)result.ToList()).First();


                Container Entity1 = new Container();
                Entity1._task = new task();
                Entity1._task.task_name = this.RegisterTaskTaskNameTextBox.Text;
                Entity1._task.project_id = p_id;
                Entity1._task.employee_id = e_id;
                Entity1._task.budget = Convert.ToInt32(this.RegisterTaskBudgetTextBox.Text);
                Entity1._task.start_date = (DateTime)this.RegisterTaskStartDatePicker.SelectedDate;
                Entity1._task.end_date = (DateTime)this.RegisterTaskDeadlineDatePicker.SelectedDate;

                //register in database
                DB1 = new DBManager();
                if (DB1.InsertValue(Entity1, Constants.Task))
                {
                    MessageBox.Show("the task has been registered");
                    EmptyAllfields(this.RegisterTaskGrid);
                    Initialization();
                }


            }
            catch { }
        }
        private void AssignTaskMaterialsAssignButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshContainer();
                // check for empty value
                CheckForEmpty(this.AssignTaskMaterialsGrid);
                if (this.textboxstatus == false)
                {
                    MessageBox.Show("A field has not been entered");
                    textboxstatus = false;
                    return;
                }
                // check if the material already exists
                if (this.AllContainer._employeeslist.Count == 0)
                {
                    RefreshContainer();
                }

                // get the detials of the assignment
                Container Entity1 = new Container();
                Entity1._task_materials = new task_materials();

                //get the project id 
                var projectresult = from item in DB1.GetProjects("")
                                    where ((item.project_name).Contains(this.AssignTaskMaterialsProjectNameTextBox.Text))
                                    select item;
                int pid = projectresult.ToList().First().project_id; 

                // get the task id
                var taskresult = from item in AllContainer._tasklist
                                 where (  (item.task_name.ToLower().Contains(this.AssignTaskMaterialsTaskNameTextBox.Text.ToLower())  ) && (item.project_id == pid)  )
                                 select item;
                if (taskresult.Count() > 0)
                {
                    // get the first id
                    Entity1._task_materials.task_id = taskresult.ToList().First().task_id;
                }
                else
                {
                    return;
                }

                //get the material id
                var materialresult = from item in AllContainer._materialslist
                                     where (item.material_name.ToLower().Contains(this.AssignTaskMaterialsMaterialNameTextBox.Text.ToLower()))
                                     select item;
                if (materialresult.Count() > 0)
                {
                    Entity1._task_materials.material_id = materialresult.ToList().First().material_id;
                }
                else
                {
                    return;
                }


                // get the type of assignment the administrator wishes to do
                MessageBoxResult r = MessageBox.Show("Do you wish to assign from stock", "Assign materials", MessageBoxButton.YesNoCancel);
                if (r == MessageBoxResult.Yes) 
                {
                    // remove the values from the stock and update it
                    Entity1._task_materials.unit_buying_price = 0;

                    //update the stock
                    stock_materials s = DB1.GetStock("SELECT * FROM stock_materials WHERE material_id = " + Entity1._task_materials.material_id).First();
                    s.quanitity -= Convert.ToInt32(this.AssignTaskMaterialsQuantityTextBox.Text);
                    DB1.UpdateValue(s);
                }
                else if (r == MessageBoxResult.No) 
                {
                    Entity1._task_materials.unit_buying_price = Convert.ToInt32(this.AssignTaskMaterialsUnitBuyingPriceTextBox.Text);
                }
                else if (r == MessageBoxResult.Cancel) 
                {
                    return; 
                }


                Entity1._task_materials.quantity = Convert.ToInt32(this.AssignTaskMaterialsQuantityTextBox.Text);                
                Entity1._task_materials.date_allocated = DateTime.Now;

                DB1 = new DBManager();
                if (DB1.InsertValue(Entity1, Constants.Task_Materials))
                {
                    MessageBox.Show("the materials have been allocated");
                    EmptyAllfields(this.AssignTaskMaterialsGrid);
                    RefreshContainer();
                }


            }
            catch { }
        }
        private void AssignTaskMaterialsMaterialNameTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the quantity of materials
                DB1 = new DBManager();
                this.AssignTaskMaterialsStockQuantityTextBlock.Text = DB1.GetStock("SELECT b.* , a.material_name FROM materials a ,  stock_materials b WHERE a.material_id = b.material_id AND a.material_name = '" + this.AssignTaskMaterialsMaterialNameTextBox.Text + "'").First().quanitity.ToString();
            }
            catch { }
        }
        private void TasksSummary()
        {
            try
            {
                DB1 = new DBManager();
                this.RunningTasksListView.ItemsSource = DB1.GetTasks("SELECT * FROM tasks WHERE start_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND end_date > '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "'");
                Debug.WriteLine("SELECT * FROM tasks WHERE start_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND end_date > '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "'");
                this.AllTasksDatagrid.ItemsSource = DB1.GetRawData("SELECT a.task_name AS 'ALL TASKS'  , b.project_name  FROM tasks a , projects b WHERE a.project_id = b.project_id").DefaultView;
            }
            catch { }
        }
        private void TasksListViewItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the task clicked
                ListView l = (ListView)sender;
                task t = (task)l.SelectedItem;
                DB1 = new DBManager();
                this.TaskDetailsListView.ItemsSource = null;
                this.TaskDetailsListView.ItemsSource = DB1.GetTaskDetails("SELECT a.* , b.project_name , c.employee_name , TIMESTAMPDIFF(DAY, a.start_date, a.end_date) AS duration FROM tasks a , projects b , employees c WHERE a.project_id = b.project_id AND a.employee_id = c.employee_id AND a.task_id = " + t.task_id);
                this.TaskResourcesListView.ItemsSource = DB1.GetProjectMaterials("SELECT a.*, d.*, SUM(b.quantity) AS quantity, SUM(b.quantity * b.unit_buying_price) AS Cost FROM materials a , task_materials b , tasks c , projects d WHERE a.material_id = b.material_id AND b.task_id = c.task_id AND c.project_id = d.project_id AND c.task_id = " + t.task_id + " Group BY a.material_name");
                Debug.WriteLine("SELECT a.*, d.*, SUM(b.quantity) AS quantity, SUM(b.quantity * b.unit_buying_price) AS Cost FROM materials a , task_materials b , tasks c , projects d WHERE a.material_id = b.material_id AND b.task_id = c.task_id AND c.project_id = d.project_id AND c.task_id = " + t.task_id + " Group BY a.material_name");
            }
            catch { }
        }
        private void UpdateTaskTaskNameTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            try 
            {
                // get a list of all project with same name
                DB1 = new DBManager();
                Debug.WriteLine("SELECT a.* FROM tasks a , projects b WHERE a.project_id = b.project_id AND a.task_name = '" + this.UpdateTaskTaskNameTextBox.Text +" AND b.project_name = '" + this.UpdateTaskProjectNameTextBox.Text + "'");
                task t = DB1.GetTasks("SELECT a.* FROM tasks a , projects b WHERE a.project_id = b.project_id AND a.task_name = '" + this.UpdateTaskTaskNameTextBox.Text + "' AND b.project_name = '" + this.UpdateTaskProjectNameTextBox.Text + "'").First();
                
                //  fill the records
                this.UpdateTaskStartDatePicker.SelectedDate = t.start_date;
                this.UpdateTaskDeadlineDatePicker.SelectedDate = t.end_date;
                this.UpdateTaskForemanTextBox.Text = DB1.GetEmployees("select a.* from employees a , tasks b where a.employee_id = b.employee_id AND b.task_id = " + t.task_id).First().employee_name.ToString();
                this.UpdateTaskBudgetTextBox.Text = t.budget.ToString();
                
            }
            catch { }
        }
        private void UpdateTaskCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try             
            {
                // get id of the task
                DB1 = new DBManager();                
                task t = DB1.GetTasks("SELECT a.* FROM tasks a , projects b WHERE a.project_id = b.project_id AND a.task_name = '" + this.UpdateTaskTaskNameTextBox.Text + "' AND b.project_name = '" + this.UpdateTaskProjectNameTextBox.Text + "'").First();


                //  fill the records
                t.start_date = (DateTime) this.UpdateTaskStartDatePicker.SelectedDate;
                t.end_date = (DateTime) this.UpdateTaskDeadlineDatePicker.SelectedDate;
                t.employee_id = DB1.GetEmployees("SELECT * from employees where employee_name = '" + this.UpdateTaskForemanTextBox.Text +  "'").First().employee_id;
                t.budget = Convert.ToInt32(this.UpdateTaskBudgetTextBox.Text);

                if (DB1.UpdateTask(t)) 
                {
                    MessageBox.Show("Task has been updated");                    
                    EmptyAllfields(this.UpdateTasksGrid);
                    Initialization();
                }
            }
            catch { }
        }

        #endregion


        #region report handlers
        private void ReportsProjectsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                DB1 = new DBManager();
                ComboBoxItem c = (ComboBoxItem) ((ComboBox)sender).SelectedItem; 
                switch (c.Content.ToString().ToLower()) 
                {
                    case "all projects":
                        // get a list of all projects
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT * FROM projects ").DefaultView;
                        break;
                    case  "running projects":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT * FROM projects WHERE start_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND end_date > '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "'").DefaultView;
                        break;
                    case "job allocation":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT e.project_name , a.job_name , count(b.job_type_id) AS count FROM job_type a , job_description b , employees c , project_employee_allocation d , projects e WHERE a.job_type_id = b.job_type_id AND b.employee_id = c.employee_id AND c.employee_id = d.employee_id AND d.project_id = e.project_id  GROUP BY  e.project_name , a.job_name ").DefaultView;
                        break;
                    case "resource expenses":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT  d.project_name , a.material_name , SUM(b.quantity) AS quantity, SUM(b.quantity * b.unit_buying_price) AS Cost FROM materials a , task_materials b , tasks c , projects d WHERE a.material_id = b.material_id AND b.task_id = c.task_id AND c.project_id = d.project_id  Group BY d.project_name ,  a.material_name").DefaultView;
                        break; 
                    case "tasks in project":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT b.project_name , a.* FROM tasks a , projects b WHERE a.project_id = b.project_id" ).DefaultView;
                        break;
                }
            }
            catch { }
        }
        private void ReportsTasksCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            try 
            {
                DB1 = new DBManager();
                ComboBoxItem c = (ComboBoxItem) ((ComboBox)sender).SelectedItem;
                switch (c.Content.ToString().ToLower()) 
                {
                    case "all tasks":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT * FROM tasks").DefaultView; 
                        break;
                    case "running tasks":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT * FROM tasks WHERE start_date < '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "' AND end_date > '" + DateTime.Now.ToString("yyyy'-'MM'-'dd") + "'").DefaultView;
                        break;
                    case "resource allocation":                        
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT a.task_name  , b.material_name , c.quantity , c.unit_buying_price , (c.quantity * c.unit_Buying_price) AS 'cost' , c.date_allocated AS 'Date' FROM tasks a , materials b , task_materials c  WHERE c.material_id = b.material_id  AND c.task_id = a.task_id ").DefaultView;
                        break;
                    case "resource expenses":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT c.task_name, a.material_name,  SUM(b.quantity) AS quantity, SUM(b.quantity * b.unit_buying_price) AS Cost FROM materials a , task_materials b , tasks c , projects d WHERE a.material_id = b.material_id AND b.task_id = c.task_id AND c.project_id = d.project_id  Group BY c.task_name, a.material_name").DefaultView;
                        break;
                }
            }
            catch { }
        }
        private void ReportsEmployeesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            try 
            {
                  DB1 = new DBManager();
                ComboBoxItem c = (ComboBoxItem) ((ComboBox)sender).SelectedItem;
                switch (c.Content.ToString().ToLower())
                {
                    case "all employees":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT employee_name , employee_contact_number FROM employees").DefaultView;
                        break;
                    case "job descriptions":
                        this.ReportsDataGrid.ItemsSource = DB1.GetRawData("SELECT a.employee_name , a.employee_contact_number , c.job_name FROM employees a , job_description b , job_type c WHERE c.job_type_id = b.job_type_id AND b.employee_id = a.employee_id ").DefaultView;
                        break;
                }
            }
            catch { } 
        }
        

        #endregion

       

        

       
        
        
      












    }
}
