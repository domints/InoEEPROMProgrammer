using System;
using System.Linq;
using InoEEPROMProgrammer.Interactive;

namespace InoEEPROMProgrammer
{
    class Program
    {
        static Programmer programmer;
        static void Main(string[] args)
        {
            var portName = GetStringValue(args, InteractiveMenu.SelectPort, "-p", "--port");
            programmer = new Programmer(portName);
            var result = programmer.Connect();
            if(!result.success)
            {
                Console.WriteLine("Can't connect to programmer!");
                return;
            }

            Console.WriteLine($"Connected to programmer: {result.identString} on selected port.");

            if(args.Length > 1 && string.Equals(args[1], "scan_i2c", StringComparison.OrdinalIgnoreCase))
            {
                
            }
        }

        static int FindParam(string[] args, int startingIndex, params string[] names)
        {
            return Array.FindIndex(args, startingIndex, a => names.Contains(a.ToLower()));
        }

        static string GetStringValue(string[] args, Func<string> interactive, params string[] names)
        {
            var pIx = FindParam(args, 0, names);
            if (pIx != -1)
            {
                return args[pIx + 1];
            }
            else
            {
                return interactive();
            }
        }
    }
}
