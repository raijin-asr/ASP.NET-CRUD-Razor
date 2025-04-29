using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUD_Razor.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo userInfo= new UserInfo();
        public String errorMessage="";
        public String successMessage="";


        public void OnGet()
        {
        }
        public void OnPost()
        {
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.phone = Request.Form["phone"];
            userInfo.address = Request.Form["address"];

            if(userInfo.name.Length == 0 || userInfo.email.Length == 0 || userInfo.phone.Length == 0 || userInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            if (userInfo.email.Contains("@") == false)
            {
                errorMessage = "Email is not valid";
                return;
            }

            if (userInfo.phone.Length < 10)
            {
                errorMessage = "Phone number is not valid";
                return;
            }

            //save to database
            try
            {
                String connectionString = "Data Source=RAIJIN\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Users (Name, Email, Phone, Address) VALUES (@name, @email, @phone, @address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.name);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@phone", userInfo.phone);
                        command.Parameters.AddWithValue("@address", userInfo.address);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            userInfo.name = "";
            userInfo.email = "";
            userInfo.phone = "";
            userInfo.address = "";

            successMessage = "User created successfully";
        }
    }
}
