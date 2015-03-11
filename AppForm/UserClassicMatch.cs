using MBC.Core.Game;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.FormBattleship
{
    class UserClassicMatch : ClassicMatch
    {
        public void AddUser(UserController user)
        {
            PlayerAdd(user);
        }
    }
}
