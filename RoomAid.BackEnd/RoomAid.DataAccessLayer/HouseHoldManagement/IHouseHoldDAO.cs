﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.DataAccessLayer
{
    public interface IHouseHoldDAO
    {
        int Insert(SqlCommand command);

        int Retrieve(SqlCommand command);
    }
}
