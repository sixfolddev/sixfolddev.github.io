using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RoomAid.Authorization.AuthZEnum;

namespace RoomAid.ServiceLayer
{
    
    public class Permission
    {
        public int UserID { get; }
        public List<Tuple<String, bool>> Permissions { get; }
        

        /// <summary>
        /// Permissions class to help with permissions update
        /// </summary>
        /// <param name="userID"></param>

        public Permission(int userID)
        {
            this.UserID = userID;
            Permissions = new List<Tuple<String, bool>>();
            foreach(AuthZ auth in (AuthZ[])Enum.GetValues(typeof(AuthZ)))
            {
                String authStr = auth.ToString();
                Permissions.Add(new Tuple<String, bool>(authStr,false));
            }

        }

        public void AddPermission(String str)
        {
            int index = FindPermission(str);
            if(index >= 0)
            {
                Permissions[index] = new Tuple<string, bool>(str, true);
            }
        }

        private int FindPermission(String str)
        {
            for(int i = 0; i < Permissions.Count; i++)
            {
                if(Permissions[i].Item1.Equals(str))
                {
                    return i;
                }
            }
            return -1;
        }


    }
}
