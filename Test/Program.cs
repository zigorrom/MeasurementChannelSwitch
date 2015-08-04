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
            //sw.Setup(new ConnectionParams { BaudRate = 115200, PortName = "COM7" });
            sw.Initialize();
            System.Threading.Thread.Sleep(1000);
            while (!sw.Initialized) ;

            for (short i = 0; i < 40; i++)
            {
                sw.Switch(i, true);
                System.Threading.Thread.Sleep(100);
            }
            Console.ReadKey();
            
        }
    }
}
