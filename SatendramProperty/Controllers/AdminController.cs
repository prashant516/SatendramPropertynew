using SatendramProperty.Models;
using SatendramProperty.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using static System.Net.Mime.MediaTypeNames;

namespace Bareillyproperty.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;
        public AdminController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add_Property()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> POSTProperty([FromBody]  PropertyMaster axpost)
        {

            try
            {

                var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "propertyImages");

                if (!Directory.Exists(imageFolderPath))
                {
                    Directory.CreateDirectory(imageFolderPath);
                }

                List<string> list = new List<string> { };

                foreach (var base64Data in axpost.PropertyMedia.Split('\n'))
                {
                    // Extract the Base64 data (removing the data URL prefix)
                    var base64 = base64Data.Substring(base64Data.IndexOf(",") + 1);
                    string extension = Path.GetExtension(base64);
                    var fileBytes = Convert.FromBase64String(base64);
                    var fileName = $"{Guid.NewGuid()}.png"; // Generate a unique file name

                    list.Add(fileName);
                    // Save the file
                    var filePath = Path.Combine(imageFolderPath, fileName);
                    await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
                }
                string result = string.Join("~", list);
                axpost.PropertyMedia = result;
                using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertPropertyMasterData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters individually
                        cmd.Parameters.AddWithValue("@Ax_postData", JsonSerializer.Serialize(axpost));

                        // Add other parameters as needed

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                // Optionally rethrow the exception or handle it accordingly
                throw;
            }
        }

     
        public IActionResult View_Property()
        {
            dynamic model = new ExpandoObject();
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
            SqlCommand cmd = new SqlCommand("GetPropertyMasterData", connection);
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.AddWithValue("@Ax_postData", JsonSerializer.Serialize(axpost));
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;                  
            adapter.Fill(dt);
            model.property = dt;
            return View(model);
        }
        public IActionResult Add_User()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserMaster user)
        {

            try
            {
                var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "propertyImages");

                if (!Directory.Exists(imageFolderPath))
                {
                    Directory.CreateDirectory(imageFolderPath);
                }

                List<string> list = new List<string> { };

                foreach (var base64Data in user.Profileimage.Split('\n'))
                {
                    // Extract the Base64 data (removing the data URL prefix)
                    var base64 = base64Data.Substring(base64Data.IndexOf(",") + 1);
                    var fileBytes = Convert.FromBase64String(base64);
                    var fileName = $"{Guid.NewGuid()}.png"; // Generate a unique file name

                    list.Add(fileName);
                    // Save the file
                    var filePath = Path.Combine(imageFolderPath, fileName);
                    await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
                }
                string result = string.Join("~", list);
                user.Profileimage = result;
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

        public IActionResult View_User()
        {
            dynamic model = new ExpandoObject();
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
            SqlCommand command = new SqlCommand("Select * from UserMaster", connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dt);
            model.User = dt;
            return View(model);
        }

        public IActionResult View_Requirment()
        {
            dynamic model = new ExpandoObject();
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
            SqlCommand command = new SqlCommand("Select * from RequirmentMaster", connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dt);
            model.Requirment = dt;
            return View(model);
        }
        public IActionResult View_Contact()
        {
            dynamic model = new ExpandoObject();
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
            SqlCommand command = new SqlCommand("Select Name,Phone,Email,Address from ContactMaster", connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dt);
            model.User = dt;
            return View(model);
        }

    }
}
