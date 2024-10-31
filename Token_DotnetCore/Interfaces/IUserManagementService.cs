using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Token_Ifinsys.Interfaces
{
    public interface IUserManagementService
    {
        bool IsValidUser(string username, string password, out string messageEx, out DataTable branchList); //string imei
    }
}
