using RoomAid.ManagerLayer;
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
        public IHttpActionResult LoginAccount()
        {
            try
            {
                return Ok(); // IMPLEMENT
            }
            catch (Exception e)
            {
                var newError = new Exception(e.Message, null);
                return InternalServerError(newError);
            }
        }
    }
}
