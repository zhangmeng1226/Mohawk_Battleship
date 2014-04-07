using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MBC.Shared.Entities
{
    public delegate void NetEventHandler(NetEvent ev);

    /// <summary>
    /// Allows communication between the server and client through a Socket.
    /// </summary>
    public class NetCom : Entity
    {
        public const int PORT = 41826;
        private Socket connection;
        private Action eventCallback;
        private Action eventLengthCallback;
        private int expectedReceiveSize = 0;
        private BinaryFormatter formatter;
        private Action handshakeCallback;
        private byte[] packetBuffer = new byte[1024];
        private int packetOffset = 0;
        private BinaryReader packetReader;
        private MemoryStream packetStream;
        private AsyncCallback receiverCallback;

        public NetCom(Entity parent)
            : base(parent)
        {
            receiverCallback = new AsyncCallback(Receive);
            handshakeCallback = new Action(HandshakeAction);
            eventCallback = new Action(EventAction);
            formatter = new BinaryFormatter();
            packetStream = new MemoryStream(packetBuffer);
            packetReader = new BinaryReader(packetStream);
        }

        public event NetEventHandler OnNetEvent;

        public bool IsConnected
        {
            get
            {
                return connection != null && connection.Connected;
            }
        }

        public bool Connect(string serverIp)
        {
            IPHostEntry hosts = Dns.GetHostEntry(serverIp);
            foreach (var address in hosts.AddressList)
            {
                var endPoint = new IPEndPoint(address, PORT);
                var tempConnection = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

                tempConnection.Connect(endPoint);

                if (tempConnection.Connected)
                {
                    connection = tempConnection;
                    BeginReceive(handshakeCallback, Marshal.SizeOf(typeof(NetConnectEvent)));
                    break;
                }
            }
            return IsConnected;
        }

        public void Disconnect()
        {
            if (connection == null) { return; }
            connection.Disconnect(false);
            connection = null;
            OnNetEvent(new NetDisconnectEvent(this));
        }

        private void BeginReceive(Action callback, int expectedSize)
        {
            expectedReceiveSize = expectedSize;
            packetOffset = 0;
            packetStream.Seek(0, SeekOrigin.Begin);
            connection.BeginReceive(packetBuffer, 0, expectedReceiveSize, 0, receiverCallback, callback);
        }

        private void EventAction()
        {
            try
            {
                Event receivedEvent = (Event)formatter.Deserialize(packetStream);
                OnNetEvent(new NetIncomingEvent(this, receivedEvent));
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        private void EventLengthAction()
        {
            try
            {
                int eventSize = packetReader.ReadInt32();
                BeginReceive(eventCallback, eventSize);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        private void HandshakeAction()
        {
            try
            {
                var handshake = (NetConnectEvent)formatter.Deserialize(new MemoryStream(packetBuffer));
                BeginReceive(eventLengthCallback, 4);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        private void Receive(IAsyncResult ar)
        {
            try
            {
                int bytesRead = connection.EndReceive(ar);

                if (bytesRead > 0)
                {
                    packetOffset += bytesRead;
                    connection.BeginReceive(packetBuffer, packetOffset, expectedReceiveSize - packetOffset, 0, receiverCallback, ar.AsyncState);
                }
                else
                {
                    var action = (Action)ar.AsyncState;
                    action.Invoke();
                }
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }
    }
}