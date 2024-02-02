using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginToDataBase
{
    static class DbConstatnts
    {
        const string BannedStatus = "Banned";
        const string AdminStatus = "Admin";
        const string UserStatus = "User";
        static public string bannedStatus { get; } = BannedStatus;
        static public string adminStatus { get; } = AdminStatus;
        static public string userStatus { get; } = UserStatus;
    }
}
