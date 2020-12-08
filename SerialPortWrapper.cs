using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace InoEEPROMProgrammer
{
    public class SerialPortWrapper
    {
        private readonly SerialPort _port;

        public int BytesToRead => _port.BytesToRead;
        public int BytesToWrite => _port.BytesToWrite;

        public SerialPortWrapper(string portName)
        {
            _port = new SerialPort(portName);
            _port.ReadTimeout = 10000;
            _port.WriteTimeout = 10000;
            _port.Handshake = Handshake.None;
        }

        public static IReadOnlyCollection<string> ListPorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool Open()
        {
            _port.BaudRate = 115200;
            _port.Open();
            _port.DtrEnable = true;
            _port.RtsEnable = true;
            while (!_port.IsOpen) ;
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();
            WriteByte(I2CCommands.PING, false);
            return ReadByte() == I2CCommands.PONG;
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
            while(receivedBytes < count)
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

                if (response != I2CCommands.ACK)
                {
                    throw new ApplicationException("Interface didn't respond with ACK!");
                }
            }
        }
    }
}