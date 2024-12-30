using SatendramProperty.Models;
using SatendramProperty.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Bareillyproperty.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDBContext _context;
        public LoginController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginUser([FromBody] Login admin)
        {
            if (admin == null || string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
            {
                return Json(new { status = false, message = "Email and Password are required!" });
            }

            using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                string query = "SELECT Username, Email, Phone, RoleName FROM UserMaster u " +
                               "INNER JOIN RoleMaster r ON u.RoleID = r.RoleID " +
                               "WHERE Email = @Email AND Password = @Password"; // Password should be hashed in real-world scenarios.

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", admin.Email);
                    command.Parameters.AddWithValue("@Password", admin.Password); // Hash the password in production.

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        var json = new Dictionary<string, object>();

                        foreach (DataColumn column in dt.Columns)
                        {
                            json[column.ColumnName] = row[column];
                        }

                        string jsonString = JsonConvert.SerializeObject(json);
                        HttpContext.Session.SetString("Username", dt.Rows[0]["Username"].ToString());
                        HttpContext.Session.SetString("Email", dt.Rows[0]["Email"].ToString());
                        HttpContext.Session.SetString("Phone", dt.Rows[0]["Phone"].ToString());
                        HttpContext.Session.SetString("RoleName", dt.Rows[0]["RoleName"].ToString());

                        return Json(new { status = true, Data = jsonString });
                    }
                    else
                    {
                        return Json(new { status = false, message = "Invalid Email or Password!" });
                    }
                }
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserMaster user)
        {

            try
            {
             
                using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertUserData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters individually
                        cmd.Parameters.AddWithValue("@UserData", JsonSerializer.Serialize(user));

                        // Add other parameters as needed

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                // Optionally rethrow the exception or handle it accordingly
                throw;
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();

        }
    }
}
