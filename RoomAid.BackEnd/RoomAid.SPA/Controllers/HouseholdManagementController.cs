
using RoomAid.ManagerLayer.HouseHoldManagement;
using RoomAid.ServiceLayer;
using RoomAid.SPA.Models;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RoomAid.SPA.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/householdmanagement")]
    public class HouseholdManagementController : ApiController
    {
        [HttpPost]
        [Route("createhousehold")]
        public IHttpActionResult Create([FromBody]HouseholdCreationModel request)
        {
            HouseHoldManager houseHoldManager = new HouseHoldManager();
            HouseholdCreationRequestDTO dto = new HouseholdCreationRequestDTO
            {Requester = request.RequesterEmail,
            StreetAddress = request.StreetAddress,
            City = request.City,
            Zip = Int32.Parse(request.ZipCode),
            Rent = request.Rent,
            SuiteNumber = request.SuiteNumber
            };
            try
            {
                var results = houseHoldManager.CreateNewHouseHold(dto);
                return Content(HttpStatusCode.OK, results.IsSuccess);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}