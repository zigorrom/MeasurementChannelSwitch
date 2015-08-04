using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelSwitchLibrary
{
    using CommandMessenger;
    using CommandMessenger.Queue;
    using CommandMessenger.Transport;
    using CommandMessenger.Transport.Serial;

    

    public struct ConnectionParams
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        
    }
    
    enum Command
    {
        Watchdog,
        Acknowledge,
        SwitchChannel,
        Error
    }

    public class ChannelSwitch
    {
        //public bool RunLoop { get; set; }
        private ITransport _transport;
        private CmdMessenger _cmdMessenger;
        private ConnectionManager _connectionManager;
        public bool Initialized { get; private set; }

        const string UniqueDeviceID = "517cea54-8f17-4761-b735-094897c20ffd";

        ~ChannelSwitch()
        {
            Exit();
        }

        public ChannelSwitch()
        {

        }

        [Obsolete("",true)]
        public void Setup()
        {
            _transport = new SerialTransport
            {
                CurrentSerialSettings = { DtrEnable = false }
            };

            _cmdMessenger = new CmdMessenger(_transport, BoardType.Bit16)
            {
                PrintLfCr = false
            };
            //_cmdMessenger.AddReceiveCommandStrategy(new )
            AttachCallbacks();
            _connectionManager = new SerialConnectionManager((_transport as SerialTransport), _cmdMessenger, (int)Command.Watchdog, UniqueDeviceID);
            _connectionManager.WatchdogEnabled = true;
            _connectionManager.ConnectionFound += _connectionManager_ConnectionFound;
            _connectionManager.StartConnectionManager();
            //throw new Exception();
        }

        void _connectionManager_ConnectionFound(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

       

        public void Setup(ConnectionParams connectionParams)
        {
            _transport = new SerialTransport
            {
                CurrentSerialSettings = { PortName = connectionParams.PortName, BaudRate = connectionParams.BaudRate, DtrEnable = false }
            };

            _cmdMessenger = new CmdMessenger(_transport, BoardType.Bit16)
            {
                PrintLfCr = false
            };

            AttachCallbacks();

            _cmdMessenger.NewLineReceived += _cmdMessenger_NewLineReceived;
            _cmdMessenger.NewLineSent += _cmdMessenger_NewLineSent;
            _cmdMessenger.Connect();
        }

        void _cmdMessenger_NewLineSent(object sender, CommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void _cmdMessenger_NewLineReceived(object sender, CommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void AttachCallbacks()
        {
            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.Error, OnError);
        }

        private void OnError(ReceivedCommand receivedCommand)
        {
            //throw new NotImplementedException();
        }

        private void OnAcknowledge(ReceivedCommand receivedCommand)
        {
            //throw new NotImplementedException();
        }

        private void OnUnknownCommand(ReceivedCommand receivedCommand)
        {
            //throw new NotImplementedException();
        }

        void Initialize()
        {

        }

        public void Exit()
        {
            if (_connectionManager != null)
                _connectionManager.Dispose();

            _cmdMessenger.Disconnect();
            _cmdMessenger.Dispose();
            _transport.Dispose();

            
            
        }

        public bool Switch(int ChannelName, bool State)
        {
            var command = new SendCommand((int)Command.SwitchChannel, (int)Command.Acknowledge, 1000);
            command.AddBinArgument(ChannelName);
            command.AddBinArgument(State);
            var response = _cmdMessenger.SendCommand(command);
            return false;
        }

    }
}
