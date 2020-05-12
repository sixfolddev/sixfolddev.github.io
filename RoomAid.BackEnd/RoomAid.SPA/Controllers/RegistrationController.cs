using Roomaid.Controllers.Models;
using RoomAid.DataAccessLayer.HouseHoldManagement;
using RoomAid.DataAccessLayerLayer;
using RoomAid.ManagerLayer;
using RoomAid.ManagerLayer.HouseHoldManagement;
using RoomAid.ServiceLayer.HouseHoldManagement;
using System;
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
        [Route("registerNewUser")]
        public IHttpActionResult Register(RegistrationRequestDTO request)
        {
            RegistrationManager registrationManager = new RegistrationManager(request);
            try
            {
                var results = registrationManager.RegisterUser();
                return Content(HttpStatusCode.OK, results);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}