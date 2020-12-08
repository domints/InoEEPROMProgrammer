using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace InoEEPROMProgrammer.AppCommands
{
    public class Identify : Command
    {
        public Identify()
         : base("identify", "Connects to the device and checks it's identification message")
        {
            AddAlias("id");
            AddArgument(new Argument("portName"));
            Handler = CommandHandler.Create((Action<string>)IdentifyInteface);
        }

        public static void IdentifyInteface(string portName)
        {
            var port = new SerialPortWrapper(portName);
            if(!port.Open())
            {
                Console.WriteLine("Failed to open specified port. Exiting.");
                return;
            }

            port.WriteByte(I2CCommands.IDENT);
            var size = port.ReadByte();
            Console.WriteLine(port.ReadString(size));
        }
    }
}