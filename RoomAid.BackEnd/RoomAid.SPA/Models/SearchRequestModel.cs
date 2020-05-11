using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roomaid.Controllers.Models
{
    public class SearchRequest
    {
        public string CityName { get; set; }
        public int Page { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string HouseholdType { get; set; }
    }
}