using SatendramProperty.Models;
using SatendramProperty.Data;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace SatendramProperty.Controllers
{
    public class AgentController : Controller
    {
        private readonly AppDBContext _context;
        public AgentController(AppDBContext context)
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
        public async Task<IActionResult> POSTProperty([FromBody] PropertyMaster axpost)
        {

            try
            {
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

        public IActionResult Enquiries()
        {
            return View();
        }
        public  IActionResult Add_Requirment()
        {
            return View();
        }
        public async Task<IActionResult> POSTRequirment([FromBody] RequirmentMaster repost)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertRequirmentMasterData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters individually
                        cmd.Parameters.AddWithValue("@Re_postData", JsonSerializer.Serialize(repost));

                        // Add other parameters as needed

                        conn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction("Agent","Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred: " + ex.Message);
                // Optionally rethrow the exception or handle it accordingly
                throw;
            }
        }

    }
}
