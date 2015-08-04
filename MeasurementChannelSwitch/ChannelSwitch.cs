using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementChannelSwitch
{
    using CommandMessenger;
    using CommandMessenger.Queue;
    using CommandMessenger.Transport;
    using CommandMessenger.Transport.Serial;

    enum Command
    {
        Watchdog,
        Acknowledge,
        SwitchChannel,
        Error
    }

    class ChannelSwitch
    {
        private const string UniqueDeviceId = "517cea54-8f17-4761-b735-094897c20ffd";

        private ITransport _transport;
        private CmdMessenger _cmdMessenger;
        private ConnectionManager _connectionManager;

        public void Setup()
        {
            _transport = new SerialTransport
            {
                CurrentSerialSettings = { DtrEnable = false }
            };

            _cmdMessenger = new CmdMessenger(_transport, BoardType.Bit32)
            {
                PrintLfCr = false
            };

            AttachCommandCallbacks();

            _cmdMessenger.AddReceiveCommandStrategy(new StaleGeneralStrategy(1000));

            _cmdMessenger.NewLineReceived += NewLineReceived;

            _cmdMessenger.NewLineSent += NewLineSent;

            _connectionManager = new SerialConnectionManager(_transport as SerialTransport, _cmdMessenger, (int)Command.Watchdog, UniqueDeviceId);

            _connectionManager.WatchdogEnabled = true;

            _connectionManager.ConnectionFound += ConnectionFound;

            _connectionManager.ConnectionTimeout += ConnectionTimeout;

            _connectionManager.Progress+=Progress;

            _connectionManager.StartConnectionManager();
        }

        public void Exit()
        {

            _connectionManager.Progress -= Progress;
            _connectionManager.ConnectionTimeout -= ConnectionTimeout;
            _connectionManager.ConnectionFound -= ConnectionFound;

            _connectionManager.Dispose();
            _cmdMessenger.Disconnect();
            _cmdMessenger.Dispose();
            _transport.Dispose();
        }

        private void AttachCommandCallbacks()
        {
            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.Error, OnError);
            //_cmdMessenger.Attach((int)Command.)
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

        private void Progress(object sender, ConnectionManagerProgressEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void ConnectionTimeout(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void ConnectionFound(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        

        void NewLineSent(object sender, CommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void NewLineReceived(object sender, CommandEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void SwitchChannel(int ChannelNumber, bool state)
        {
            var command = new SendCommand((int)Command.SwitchChannel, (int)Command.Acknowledge, 500);
            command.AddBinArgument(ChannelNumber);
            command.AddBinArgument(state);
            _cmdMessenger.SendCommand(command, SendQueue.ClearQueue, ReceiveQueue.ClearQueue, UseQueue.BypassQueue);
        }

    }
}
