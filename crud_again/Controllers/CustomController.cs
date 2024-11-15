using crud_again.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CrudUsingNonQuery.Controllers
{
    public class CustomController : Controller
    {
        private readonly IConfiguration _configuration;
        public CustomController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            List<Customer> cusObj = new List<Customer>();


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //string query = "select * from customer_list";
                string query = "sp_GetCustomer";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cusObj.Add(new Customer
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Age = (int)reader["Age"]
                    });
                }
            }

            return View(cusObj);
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customer cust)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //string query = "INSERT INTO customer_list (Name, Age) VALUES (@Name, @Age)";
                string query = "sp_InsertCustomer";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", cust.Name);
                cmd.Parameters.AddWithValue("@Age", cust.Age);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer cust)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "update customer_list set Name=@Name, Age=@Age where Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);


                cmd.Parameters.AddWithValue("@Name", cust.Name);
                cmd.Parameters.AddWithValue("@Age", cust.Age);
                cmd.Parameters.AddWithValue("@Id", cust.Id);

                conn.Open();
                cmd.ExecuteNonQuery();



            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteCustomer(Customer cust)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM customer_list WHERE Name = @Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", cust.Name);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
