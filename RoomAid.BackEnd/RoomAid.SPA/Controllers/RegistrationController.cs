
using RoomAid.DataAccessLayerLayer;
using RoomAid.ManagerLayer;
using RoomAid.SPA.Models;
using System;
using System.Globalization;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoomAid.SPA.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/registration")]
    public class RegistrationController : ApiController
    {
        [HttpPost]
        [Route("registerUser")]
        public IHttpActionResult Register([FromBody]RegistrationModel request)
        {
            RegistrationRequestDTO registrationRequestDTO = new RegistrationRequestDTO
            {
                Email = request.Email,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Password = request.Password,
                Repeatpassword = request.Password,
                Dob = request.DateofBirth
            };

            RegistrationManager registrationManager = new RegistrationManager(registrationRequestDTO);
            try
            {
                var result = registrationManager.RegisterUser();
                return Content(HttpStatusCode.OK, result.IsSuccess);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}