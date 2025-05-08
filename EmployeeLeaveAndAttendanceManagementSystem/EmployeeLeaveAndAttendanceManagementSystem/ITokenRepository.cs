using Microsoft.AspNetCore.Identity;

namespace EmployeeLeaveAndAttendanceManagementSystem
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);

    }
}
