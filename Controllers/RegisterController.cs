using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using Wolmart_Ecommerce_Marketplace.Models;

namespace Wolmart_Ecommerce_Marketplace.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> logger;
        private readonly IConfiguration config;

        public RegisterController(ILogger<RegisterController> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]Users user)
        {
            try
            {

           
            var conn = config.GetConnectionString("dbcs");
            using (var connection = new SqlConnection(conn))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@FirstName", user.FirstName);
                parameter.Add("@LastName", user.LastName);
                parameter.Add("@Email", user.Email);
                parameter.Add("@Password", user.Password);
                parameter.Add("@CreatedAT", user.CreatedAt ?? DateTime.Now);
                parameter.Add("@CreatedBy", user.CreatedBy);
                parameter.Add("@UpdatedAt", user.UpdatedAt ?? DateTime.Now);
                parameter.Add("@UpdatedBy", user.UpdatedBy);

                var result = await connection.QuerySingleAsync<dynamic>("RegisterUser",
            parameter,
            commandType: CommandType.StoredProcedure
        );

                return Json(new { status = result.Status, message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

    }
}
