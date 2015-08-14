using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMS
{
    class Request
    {
        // class handling the sending of requests to the server and back
        public string purpose { get; set; }
        public string category { get; set; }
        public string content { get; set; }
    }
    public class Container
    {
        public projects _project;
        public task _task;
        public clients _client;
        public materials _materials;
        public stock_materials _stock_materials;
        public job_description _job_description;
        public job_type _job_type;
        public labourers _labourers; 
        public employees _employees;
        public project_labour_allocation _project_labour_allocation;
        public project_labour_working_day _project_labour_working_day; 
        public project_employee_allocation _project_employee_allocation;
        public project_employee_working_day _project_employee_working_day; 
        public task_materials _task_materials; 
        


        public List<projects> _projectlist;
        public List<task> _tasklist;
        public List<clients> _clientlist;
        public List<labourers> _labourerslist; 
        public List<materials> _materialslist;
        public List<stock_materials> _stock_materialslist;
        public List<job_description> _job_description_list;
        public List<job_type> _job_typelist;
        public List<employees> _employeeslist;
        public List<project_labour_allocation> _project_labour_allocationlist;
        public List<project_labour_working_day> _project_labour_working_daylist; 
        public List<project_employee_allocation> _project_employee_allocationlist;
        public List<project_employee_working_day> _project_employee_working_daylist ; 
        public List<task_materials> _task_materiallist; 

    }
    public class Constants
    {
        public const string Project = "project";
        public const string Task = "task";
        public const string Client = "client";
        public const string Materials = "materials";
        public const string Stock_Materails = "stock_materials";
        public const string Job_Description = "job_description";
        public const string Job_Type = "job_type";
        public const string employees = "employees";
        public const string Project_Labour_Allocation = "project_labour_allocation";
        public const string Project_Labour_Working_Day = "project_labour_working_day"; 
        public const string Project_Employee_Allocation = "project_employee_allocation";
        public const string Project_employee_Working_Day = "project_employee_working_day"; 
        public const string Task_Materials = "task_materials";
        public const string Labourers = "labourers";
        public const string Labourers_List = "labourers_list";


        //controls on the Root page
        public const string Progress_Grid = "ProgressGrid";
        public const string Projects_Grid = "ProjectsGrid";
        public const string Tasks_Grid = "TasksGrid";
        public const string Resources_Grid = "ResourcesGrid";
        public const string Employees_Grid = "EmployeesGrid";
        public const string Clients_Grid = "ClientsGrid";


        public const string Log_In = "log_in";
        public const string Foreman = "Foreman";
        public const string Supervisor = "supervisor";
        public const string Request_Material = "request_materials";
        public const string Remainder_Materials = "remainder_materials";
        public const string New_Labourer = "new_labourer";
        public const string Submit_Work_Day = "submit_work_day"; 
    }

    // database table types
    public struct clients
    {
        public int client_id;
        public string client_name;
        public string contact;
        public string email;
    }
    public struct employees
    {
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_contact_number { get; set; }
        public string password { get; set; }
    }
    public struct job_description
    {
        public int employee_id;
        public int job_type_id;
        public int job_description_id;
    }
    public struct job_type
    {
        public int job_type_id;
        public string job_name;
        public int unit_pay;
    }
    public struct labourers
    {
        public int labourer_id;
        public string labourer_name;
        public string national_id;
        public string labourer_contact;
    }
    public struct materials
    {
        public int material_id;
        public string material_name;
        public string measuring_unit;
    }
    public struct projects
    {
        public int project_id { get; set; }
        public string project_name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int client_id { get; set; }
        public string status { get; set; }
    }
    public struct project_employee_allocation
    {
        public int project_employee_allocation_id;
        public int project_id;
        public int employee_id;        
    }
    public struct project_employee_working_day 
    {        
        public int project_employee_allocation_id ;
        public DateTime working_date; 
    }
    public struct project_employee_payment
    {
        public int project_employee_allocation_id;
        public int days_worked;
        public DateTime date_paid;
        public int amount; 
    }
    public struct project_labour_allocation
    {
        public int project_labour_allocation_id;
        public int labourer_id;
        public int project_id;
        //public DateTime working_date;
    }
    public struct project_labour_working_day 
    {
        public int project_labour_allocation_id;
        public DateTime working_date; 
    }
    public struct project_labour_payment
    {
        public int project_labour_allocation_id;
        public int days_worked;
        public DateTime date_paid;
        public int amount;
    }

    public struct stock_materials
    {
        public int stock_id;
        public int material_id;
        public int quanitity;
    }
    public struct task
    {
        public int task_id { get; set; }
        public int project_id { get; set; }
        public int budget { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public int employee_id { get; set; }
        public string task_name { get; set; }
        public string status { get; set; }
    }
    public struct task_materials 
    {
        public int task_materials_id;
        public int task_id;
        public int material_id;
        public int quantity;
        public int unit_buying_price;
        public DateTime date_allocated; 
    }

    public struct project_materials 
    {
        public int project_id { get; set; }
        public string project_name { get; set; }
        public int material_id { get; set; }
        public string material_name { get; set; }
        public string measuring_unit { get; set; }
        public int total_quantity { get; set; }
        public int total_cost { get; set; }
    }
    public struct project_jobs 
    {
        public int project_id { get; set; }
        public string project_name { get; set; }
        public int job_type_id { get; set; }
        public string job_name {get ; set;}
        public int employee_count { get; set; }

    }
    public struct task_details 
    {
        public int task_id { get; set; }
        public string task_name { get; set; }
        public int project_id { get; set; }
        public string project_name { get; set; }
        public int duration { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string foreman { get; set; }
        public int _days_left; 
        public int days_left 
        { 
            get 
            { 
                _days_left = (int)(end_date - DateTime.Now.Date).TotalDays;
                return  _days_left ;
            }
            set { this.days_left = _days_left; }
        }
    }
    public struct job_type_details 
    {
        public int job_type_id { get; set; }
        public string job_name { get; set; }
        public int employee_count { get; set; }
    }

    public struct MaterialRequest
    {
        public int quantity { get; set; }
        public string MaterialName { get; set; }
    }
    public struct RemainderRequest
    {
        public int quantity { get; set; }
        public string MaterialName { get; set; }
    }
    public struct LabourerRequest
    {
        public int labourer_id;
        public string labourer_name;
        public string national_id;
        public string labourer_contact;
        public int project_id;
    }
    
    class EntityStructures
    {

    }
}
