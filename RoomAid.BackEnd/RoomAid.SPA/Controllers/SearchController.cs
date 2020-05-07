using Roomaid.Controllers.Models;
using RoomAid.DataAccessLayer.HouseHoldManagement;
using RoomAid.ManagerLayer.HouseHoldManagement;
using RoomAid.ServiceLayer.HouseHoldManagement;
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
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Search([FromUri] SearchRequest request)
        {
            HouseholdSearchDAO searchDAO = new HouseholdSearchDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            HouseholdSearchService searchService = new HouseholdSearchService(searchDAO);
            HouseholdSearchManager searchManager = new HouseholdSearchManager(searchService);
            try
            {
                var results = searchManager.Search(request.CityName, request.Page, request.MinPrice, request.MaxPrice, request.HouseholdType);
                return Content(HttpStatusCode.OK, results);
            }
            catch (Exception e)
            {
                // TODO: Log
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult Count([FromUri] SearchRequest request)
        {
            HouseholdSearchDAO searchDAO = new HouseholdSearchDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            HouseholdSearchService searchService = new HouseholdSearchService(searchDAO);
            HouseholdSearchManager searchManager = new HouseholdSearchManager(searchService);
            try
            {
                var results = searchManager.GetTotalResultCountForQuery(request.CityName, request.MinPrice, request.MaxPrice, request.HouseholdType);
                return Content(HttpStatusCode.OK, results);
            }
            catch (Exception e)
            {
                // TODO: Log
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        [Route("autocomplete")]
        public IHttpActionResult Autocomplete()
        {
            HouseholdSearchDAO searchDAO = new HouseholdSearchDAO(Environment.GetEnvironmentVariable("sqlConnectionSystem", EnvironmentVariableTarget.User));
            HouseholdSearchService searchService = new HouseholdSearchService(searchDAO);
            HouseholdSearchManager searchManager = new HouseholdSearchManager(searchService);
            return Content(HttpStatusCode.OK, searchManager.GetAutocompleteCities());
        }
        [HttpGet]
        [Route("test")]
        public IHttpActionResult testMethod()
        {
            return Ok("Success!!!!");
        }
    }
}