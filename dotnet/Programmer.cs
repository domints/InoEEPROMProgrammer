using System;
using System.Collections.Generic;

namespace InoEEPROMProgrammer
{
    public class Programmer
    {
        private readonly SerialPortWrapper port;
        private int _bufferSize;
        private bool _speedSwitchable;
        private int _maxSpeed;

        public bool CanSwitchSpeed => _speedSwitchable;

        public Programmer(string portName)
        {
            port = new SerialPortWrapper(portName);
        }

        public (bool success, string identString) Connect()
        {
            if(!port.Open())
                return (false, null);

            var identString = Identify();

            if(_speedSwitchable)
            {
                Console.WriteLine("Trying to change speed...");
                var supportedMax = port.FindMaxSupportedSpeed(_maxSpeed);
                Console.WriteLine($"Supported max is {supportedMax}");
                SetSerialSpeed(supportedMax);
                if(!port.SetPortSpeed(supportedMax))
                    throw new OperationCanceledException("Speed change failed. Reconnect your device.");
            }

            return (true, identString);
        }

        public string Identify()
        {
            port.WriteByte(Commands.IDENT);
            var _ = port.ReadByte();
            var identString = port.ReadString();
            _bufferSize = port.ReadInt32();
            _speedSwitchable = port.ReadBool();
            _maxSpeed = port.ReadInt32();
            return identString;
        }

        public void ScanI2CBus()
        {
            port.WriteByte(Commands.SETUP_I2C);
            port.WriteByte(Commands.SCAN);
        }

        private void SetSerialSpeed(int speed)
        {
            var msg = new List<byte>();
            msg.Add(Commands.SET_SPEED);
            msg.AddRange(speed.ToBytes());
            port.WriteBytes(msg.ToArray());
        }
    }
}