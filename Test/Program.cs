using ChannelSwitchLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new ChannelSwitch();
            sw.Setup(new ConnectionParams { BaudRate = 115200, PortName = "COM3" });
            //sw.Setup();
            
            for (int i = 0; i < 100; i++)
            {
                sw.Switch(1, true);
                System.Threading.Thread.Sleep(100);
            }

            
        }
    }
}
