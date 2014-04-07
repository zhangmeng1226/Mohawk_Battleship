using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MBC.Core.Controllers
{
    public class NetController : IController2
    {
        private Socket clientSocket;

        private IController2 controllerWrap;

        public NetController(Socket connection, IController2 controller)
        {
            clientSocket = connection;
            controllerWrap = controller;
        }

        public Player Player
        {
            get;
            set;
        }
    }
}