using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace InoEEPROMProgrammer.AppCommands
{
    public class WriteFromFile : Command
    {
        public WriteFromFile()
            : base("write", "Writes contents of file to EEPROM")
        {
            AddAlias("w");
            AddArgument(new Argument("portName"));
            var deviceNameOption = new Option<string>("--deviceName", "Sets the device name (e.g. at24c02)");
            deviceNameOption.AddAlias("-d");
            AddOption(deviceNameOption);
            var deviceAddressOption = new Option<string>("--deviceAddress", "Sets the device address (default 0x50)");
            deviceAddressOption.AddAlias("-a");
            AddOption(deviceAddressOption);
            var outFileOption = new Option<string>("--inFile", "Path to file to read from");
            outFileOption.AddAlias("-i");
            outFileOption.AddAlias("--in");
            outFileOption.LegalFilePathsOnly();
            AddOption(outFileOption);
            Handler = CommandHandler.Create<string, string, string, string>(Write);
        }

        public static void Write(string portName, string deviceName, string deviceAddress, string inFile)
        {
            var definition = new DefinitionProvider().Get(deviceName);

            byte address = string.IsNullOrWhiteSpace(deviceAddress) ? (byte)0x50 : deviceAddress.ToByte();
            var eeprom = new I2CEEPROM(portName, address, definition);
            var fileContent = File.ReadAllBytes(inFile);
            eeprom.WriteToAddress(0, fileContent);
            Console.WriteLine($"Succesfully wrote {fileContent.Length} bytes to EEPROM.");
        }
    }
}