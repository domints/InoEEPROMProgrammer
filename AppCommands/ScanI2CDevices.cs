using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace InoEEPROMProgrammer.AppCommands
{
    public class ScanI2CDevices : Command
    {
        public ScanI2CDevices()
            : base("scan", "Scans for I2C devices on the bus")
        {
            AddAlias("s");
            AddArgument(new Argument("portName"));
            Handler = CommandHandler.Create((Action<string>)Scan);
        }

        public static void Scan(string portName)
        {
            var port = new SerialPortWrapper(portName);
            if(!port.Open())
            {
                Console.WriteLine("Failed to open specified port. Exiting.");
                return;
            }

            port.WriteByte(I2CCommands.SCAN);
            var devicesCount = port.ReadByte();
            if(devicesCount == 0)
            {
                Console.WriteLine("No devices found.");
                return;
            }
            
            var devices = port.ReadBytes(devicesCount);
            Console.WriteLine($"Found {devicesCount} devices:");
            foreach(byte d in devices)
            {
                if(d > 127) 
                {
                    Console.WriteLine($"{d - 128:X2} [ERROR]");
                }
                else
                {
                    Console.WriteLine($"{d:X2}");
                }
            }
        }
    }
}