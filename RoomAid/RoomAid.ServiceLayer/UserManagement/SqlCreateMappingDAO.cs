using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.ServiceLayer
{
    public class SqlCreateMappingDAO : ICreateMappingDAO
    {
        private string connectionString;
        public SqlCreateMappingDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IResult Create(User newUser)
        {
            return null;
        }
        
    }
}
