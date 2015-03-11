using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.FormBattleship
{
    class UserController : Player
    {
       public UserController(Entity parent, string name) :
            base(parent, name)
        {

        }
    }
}
