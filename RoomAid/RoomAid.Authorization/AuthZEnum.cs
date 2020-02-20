using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomAid.Authorization
{
    public class AuthZEnum
    {
        public enum AuthZ
        {
            None,
            EnabledAccount,
            CreateAdmin,
            EnableAccount,
            DisableAccount,
            EditProfile,
            ViewProfile,
            DeleteAccount,
            SearchHousehold,
            SendMessage,
            ReplyMessage,
            ViewMessage,
            MarkMessage,
            DeleteMessage,
            SendInvite,
            ViewInvite,
            AcceptInvite,
            DeclineInvite,
            ViewTenant,
            PromoteTenant,
            DemoteTenant,
            RemoveTenant,
            LeaveHousehold,
            CreateExpense,
            ViewExpense,
            EditExpense,
            DeleteExpense,
            CreateTask,
            ViewTask,
            EditTask,
            DeleteTask,
            CreateSupplyRequest,
            ViewSupplyRequest,
            EditSupplyRequest,
            DeleteSupplyRequest
        }

    }
}
