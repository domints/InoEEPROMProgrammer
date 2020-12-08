using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace InoEEPROMProgrammer.AppCommands
{
    public class ListSerialPorts : Command
    {
        public ListSerialPorts()
            : base("ports", "Lists serial ports available on this computer")
        {
            AddAlias("p");
            Handler = CommandHandler.Create(ListPorts);
        }

        public static void ListPorts()
        {
            foreach(var p in SerialPortWrapper.ListPorts())
            {
                Console.WriteLine(p);
            }
        }
    }
}