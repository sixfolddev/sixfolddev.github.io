using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RoomAid.Authorization.AuthZEnum;

namespace RoomAid.ServiceLayer.UserManagement
{
    
    public class Permission
    {
        private readonly String _userID;
        private readonly List<Tuple<String, bool>> _permissions;
        

        /// <summary>
        /// Permissions class to help with permissions update
        /// </summary>
        /// <param name="userID"></param>

        public Permission(String userID)
        {
            this._userID = userID;
            _permissions = new List<Tuple<String, bool>>();
            foreach(AuthZ auth in (AuthZ[])Enum.GetValues(typeof(AuthZ)))
            {
                String authStr = auth.ToString();
                _permissions.Add(new Tuple<String, bool>(authStr,false));
            }

        }

        public void AddPermission(String str)
        {
            int index = FindPermission(str);
            if(index >= 0)
            {
                _permissions[index] = new Tuple<string, bool>(str, true);
            }
        }

        private int FindPermission(String str)
        {
            for(int i = 0; i < _permissions.Count; i++)
            {
                if(_permissions[i].Item1.Equals(str))
                {
                    return i;
                }
            }
            return -1;
        }


    }
}
