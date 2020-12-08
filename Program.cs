using System;
using System.CommandLine;
using System.IO.Ports;
using InoEEPROMProgrammer.AppCommands;

namespace InoEEPROMProgrammer
{
    /// <summary>
    /// Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// I2C EEPROM programmer with Arduino HW interface
        /// </summary>
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand();
            rootCommand.AddCommand(new Identify());
            rootCommand.AddCommand(new ListSerialPorts());
            rootCommand.AddCommand(new ScanI2CDevices());
            rootCommand.AddCommand(new ReadToFile());
            rootCommand.AddCommand(new WriteFromFile());
            rootCommand.InvokeAsync(args).Wait();
        }
    }
}
