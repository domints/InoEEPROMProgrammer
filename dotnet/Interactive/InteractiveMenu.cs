
using System;
using System.Linq;

namespace InoEEPROMProgrammer.Interactive
{
    public static class InteractiveMenu
    {
        public static string SelectPort()
        {
            var ports = SerialPortWrapper.ListPorts();
            int portNo = 0;
            foreach(var port in ports)
            {
                Console.WriteLine($"[{portNo++}]. {port}");
            }

            Console.Write("Select port: ");
            int selectedPort;
            if (ports.Count < 10)
            {
                var k = Console.ReadKey().KeyChar - 0x30;
                Console.WriteLine();
                if (k < 0 || k > 9 || k > ports.Count - 1)
                {
                    Console.WriteLine("Invalid selection.");
                    return null;
                }

                selectedPort = k;
            }
            else if (!int.TryParse(Console.ReadLine().Trim(), out selectedPort) || selectedPort < 0 || selectedPort > ports.Count - 1)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                return null;
            }

            Console.WriteLine();
            return ports.Skip(selectedPort).First();
        }
    }
}