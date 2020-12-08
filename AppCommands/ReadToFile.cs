using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace InoEEPROMProgrammer.AppCommands
{
    public class ReadToFile : Command
    {
        public ReadToFile()
            : base("read", "Reads contents of EEPROM to file")
        {
            AddAlias("r");
            AddArgument(new Argument("portName"));
            var deviceNameOption = new Option<string>("--deviceName", "Sets the device name (e.g. at24c02)");
            deviceNameOption.AddAlias("-d");
            AddOption(deviceNameOption);
            var deviceAddressOption = new Option<string>("--deviceAddress", "Sets the device address (default 0x50)");
            deviceAddressOption.AddAlias("-a");
            AddOption(deviceAddressOption);
            var outFileOption = new Option<string>("--outFile", "Path to file to write file to");
            outFileOption.AddAlias("-o");
            outFileOption.AddAlias("--out");
            outFileOption.LegalFilePathsOnly();
            AddOption(outFileOption);
            Handler = CommandHandler.Create<string, string, string, string>(Read);
        }

        public static void Read(string portName, string deviceName, string deviceAddress, string outFile)
        {
            var definition = new DefinitionProvider().Get(deviceName);

            byte address = string.IsNullOrWhiteSpace(deviceAddress) ? 0x50 : deviceAddress.ToByte();
            var eeprom = new I2CEEPROM(portName, address, definition);
            var eepromContent = eeprom.ReadFromAddress(0, definition.MemorySize / definition.WordSize);
            File.WriteAllBytes(outFile, eepromContent);
            Console.WriteLine($"Succesfully read {eepromContent.Length} bytes from EEPROM.");
        }
    }
}