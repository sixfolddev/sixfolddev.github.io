using RoomAid.ManagerLayer;
using RoomAid.ServiceLayer.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoomAid.SPA.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private readonly UserManagementManager _userManager;
        

        // CONSTRUCTORS
        public LoginController()
        {
            _userManager= new UserManagementManager();
        }

        [HttpPost]
        [Route("loginaccount")]
        public IHttpActionResult LoginAccount([FromBody] LoginAttemptModel loginAttempt)
        {
            try
            {
                // Model Binding Validation
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                // Try to login and get token
                return Ok(_userManager.LoginAccount(loginAttempt));
            }
            catch (Exception e)
            {
                var newError = new Exception(e.Message, null);
                return InternalServerError(newError);
            }
        }
    }
}
