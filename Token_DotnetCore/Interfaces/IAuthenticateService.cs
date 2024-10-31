using System.Data;
using Token_Ifinsys.Models;

namespace Token_Ifinsys.Interfaces
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(TokenRequest request, out string token, out int status, out string message, out DataTable branchList);
    }
}
