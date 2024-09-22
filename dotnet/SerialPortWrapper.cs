using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoEEPROMProgrammer
{
    public class SerialPortWrapper
    {
        private int[] SerialSpeeds = new int[] { 230400, 460800, 921600, 1000000, 1500000, 2000000 };
        private const int DefaultTimeout = 10000;
        private const int InitTimeout = 50;
        private const int InitMaxFailCount = 100;
        private readonly SerialPort _port;

        public int BytesToRead => _port.BytesToRead;
        public int BytesToWrite => _port.BytesToWrite;

        public SerialPortWrapper(string portName)
        {
            _port = new SerialPort(portName)
            {
                ReadTimeout = DefaultTimeout,
                WriteTimeout = DefaultTimeout,
                Handshake = Handshake.None
            };
        }

        public static IReadOnlyCollection<string> ListPorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool Open()
        {
            try
            {
                _port.BaudRate = 115200;
                _port.DtrEnable = true;
                _port.RtsEnable = true;
                _port.Open();
                while (!_port.IsOpen) ;
                _port.ReadTimeout = InitTimeout;
                _port.DiscardInBuffer();
                _port.DiscardOutBuffer();
                return EnsureConnection();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Selected port is inaccessible. Try elevating your access and make sure that's correct port.");
                return false;
            }
        }

        public int FindMaxSupportedSpeed(int chipMaxSpeed)
        {
            var speedsToCheck = new[] { chipMaxSpeed }.Concat(SerialSpeeds.Where(s => s < chipMaxSpeed));
            var originalSpeed = _port.BaudRate;
            var maxWorking = originalSpeed;
            foreach (var speed in speedsToCheck)
            {
                try
                {
                    _port.BaudRate = speed;

                    if (speed > maxWorking)
                        maxWorking = speed;
                }
                catch { }
            }

            _port.BaudRate = originalSpeed;

            return maxWorking;
        }

        public bool SetPortSpeed(int speed)
        {
            _port.BaudRate = speed;
            return EnsureConnection();
        }

        private bool EnsureConnection()
        {
            byte pingResponse = 0x00;
            int failCount = 0;
            while (pingResponse != Commands.PONG && failCount < InitMaxFailCount)
            {
                try
                {
                    WriteByte(Commands.PING, false);
                    pingResponse = ReadByte();
                }
                catch
                {
                    failCount++;
                }
            }
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();

            return failCount < InitMaxFailCount;
        }

        public bool ReadBool()
        {
            return ReadByte() > 0;
        }

        public byte ReadByte()
        {
            return ReadBytes(1)[0];
        }

        public byte[] ReadBytes(int count)
        {
            if (!_port.IsOpen)
                throw new InvalidOperationException("Port is not opened!");

            byte[] buffer = new byte[count];
            int receivedBytes = 0;
            while (receivedBytes < count)
            {
                receivedBytes += _port.Read(buffer, 0, count);
            }

            return buffer;
        }

        public string ReadString(int count)
        {
            var data = ReadBytes(count);
            return Encoding.UTF8.GetString(data);
        }

        public string ReadString()
        {
            List<byte> data = new();
            while (data.Count == 0 || data[data.Count - 1] != 0x00)
            {
                data.Add(ReadByte());
            }

            return Encoding.UTF8.GetString(data.ToArray());
        }

        public int ReadInt32()
        {
            var data = ReadBytes(4);
            return data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
        }

        public void WriteByte(byte data, bool checkForAck = true)
        {
            WriteBytes(new[] { data }, checkForAck);
        }

        public void WriteBytes(byte[] data, bool checkForAck = true)
        {
            _port.Write(data, 0, data.Length);
            if (checkForAck)
            {
                byte response = 0x00;
                try
                {
                    response = ReadByte();
                }
                catch (TimeoutException)
                { }

                if (response != Commands.ACK)
                {
                    throw new ApplicationException("Interface didn't respond with ACK!");
                }
            }
        }
    }
}