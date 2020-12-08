using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoEEPROMProgrammer
{
    public class I2CEEPROM
    {
        private const int INTERFACE_SUPPORTED_PAGE_SIZE = 16;
        private readonly SerialPortWrapper _port;
        private readonly byte _deviceAddress;
        private readonly int _blockSize;
        private readonly int _pageSize;
        private readonly EEPROMDefinition _definition;

        public I2CEEPROM(string portName, byte deviceAddress, EEPROMDefinition definition)
        {
            _port = new SerialPortWrapper(portName);
            if (!_port.Open())
            {
                throw new ApplicationException("Failed to open specified port. Exiting.");
            }

            _deviceAddress = deviceAddress;

            _definition = definition;
            _pageSize = Math.Min(INTERFACE_SUPPORTED_PAGE_SIZE, _definition.PageSize);
            _blockSize = (definition.MemorySize / definition.WordSize) / definition.BlockCount;
        }

        public byte[] ReadFromAddress(int address, int count)
        {
            List<byte> output = new List<byte>();
            var ops = OperationSequenceGenerator.Generate(_blockSize, address, count);
            foreach (var op in ops)
            {
                output.AddRange(ReadBlock(op.AbsoluteMemBlockNumber, op.BlockOperationStart, op.OperationBlockSize));
            }

            return output.ToArray();
        }

        private byte[] ReadBlock(int blockNumber, int address, int count)
        {
            List<byte> output = new List<byte>();
            var ops = OperationSequenceGenerator.Generate(_pageSize, address, count);
            foreach (var op in ops)
            {
                var opAddr = (op.AbsoluteMemBlockNumber * _pageSize) + op.BlockOperationStart;
                output.AddRange(ReadPage((byte)(_deviceAddress + blockNumber), opAddr, op.OperationBlockSize));
            }

            return output.ToArray();
        }

        private byte[] ReadPage(byte devAddr, int address, int count)
        {
            List<byte> initBuffer = new List<byte> { I2CCommands.SEND, devAddr, (byte)_definition.AddressSize, 0 };
            initBuffer.AddRange(GetDeviceSpecificAddressBytes(address));
            byte[] readCmd = new[] { I2CCommands.READ, devAddr, count.GetNthByte(0), count.GetNthByte(1) };
            _port.WriteBytes(initBuffer.ToArray());
            _port.WriteBytes(readCmd);
            int receiveCount = _port.ReadByte() | (_port.ReadByte() << 8);
            return _port.ReadBytes(receiveCount);
        }

        public void WriteToAddress(int address, byte[] data)
        {
            if(data.Length + address > _definition.MemorySize / 8)
                throw new ApplicationException("Data too long to write at given address!");

            var ops = OperationSequenceGenerator.Generate(_blockSize, address, data.Length);
            foreach (var op in ops)
            {
                var blockData = data.Skip(op.DataOperationStart).Take(_blockSize).ToArray();
                WriteBlock(op.AbsoluteMemBlockNumber, op.BlockOperationStart, blockData);
            }
        }

        private void WriteBlock(int blockNumber, int address, byte[] data)
        {
            var ops = OperationSequenceGenerator.Generate(_pageSize, address, data.Length);
            foreach (var op in ops)
            {
                var opAddr = (op.AbsoluteMemBlockNumber * _pageSize) + op.BlockOperationStart;
                var pageData = data.Skip(op.DataOperationStart).Take(_pageSize).ToArray();
                WritePage((byte)(_deviceAddress + blockNumber), opAddr, pageData);
            }
        }

        private void WritePage(byte devAddress, int address, byte[] data)
        {
            var msgLength = data.Length + _definition.AddressSize;
            List<byte> buffer = new List<byte>
            {
                I2CCommands.SEND,
                devAddress,
                msgLength.GetNthByte(0),
                msgLength.GetNthByte(1),
            };

            buffer.AddRange(GetDeviceSpecificAddressBytes(address));

            buffer.AddRange(data);

            _port.WriteBytes(buffer.ToArray());
        }

        private IEnumerable<byte> GetDeviceSpecificAddressBytes(int address)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < _definition.AddressSize; i++)
            {
                result.Add(address.GetNthByte(_definition.AddressSize - 1 - i));
            }

            return result;
        }
    }
}