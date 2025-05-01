using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CRUD_Razor.Pages.Users
{
    public class EditModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id= Request.Query["id"];
            try
            {
                String connectionString = "Data Source=RAIJIN\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Users WHERE Id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userInfo.id = "" + reader.GetInt32(0);
                                userInfo.name = reader.GetString(1);
                                userInfo.email = reader.GetString(2);
                                userInfo.phone = reader.GetString(3);
                                userInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return; 
            }

        }

        public void OnPost()
        {
            userInfo.id = Request.Form["id"];
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.phone = Request.Form["phone"];
            userInfo.address = Request.Form["address"];

            if (userInfo.name.Length == 0 || userInfo.email.Length == 0 || userInfo.phone.Length == 0 || userInfo.address.Length == 0)
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
                    string sql = "UPDATE Users SET Name=@name, Email=@email, Phone=@phone, Address=@address WHERE Id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.name);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@phone", userInfo.phone);
                        command.Parameters.AddWithValue("@address", userInfo.address);
                        command.Parameters.AddWithValue("@id", userInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "User updated successfully";
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
      
    }
}
