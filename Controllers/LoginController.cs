using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Wolmart_Ecommerce_Marketplace.Models;

namespace Wolmart_Ecommerce_Marketplace.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration config;

        public LoginController(IConfiguration config) 
        {
            this.config = config;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginUser([FromBody] Users user)
        {
            try
            {
                var conn = config.GetConnectionString("dbcs");

                using (var connection = new SqlConnection(conn))
                {
                    string query = "SELECT u.*, r.RoleName FROM Users u JOIN UserRoles ur ON u.UserId = ur.UserId JOIN Roles r ON ur.RoleId = r.RoleId WHERE Email = @Email AND PasswordHash = @Password";

                    var result = connection.QueryFirstOrDefault<Users>(query, new
                    {
                        Email = user.Email,
                        Password = user.Password
                    });

                    if (result != null)
                    {
                        HttpContext.Session.SetString("UserEmail", result.Email);
                        HttpContext.Session.SetString("UserRole", result.RoleName);
                        return Json(new
                        {
                            status = "success",
                            message = "Login successful.",
                            role = result.RoleName
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = "error",
                            message = "Invalid email or password."
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    message = "An error occurred during login.",
                    exception = ex.Message // optional: useful for debugging
                });
            }
        }

    }
}
