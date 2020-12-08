using System.Collections.Generic;

namespace InoEEPROMProgrammer
{
    public static class OperationSequenceGenerator
    {
        public static List<OperationPart> Generate(int partSize, int startingAddress, int count)
        {
            var result = new List<OperationPart>();

            var currentChunk = startingAddress / partSize;
            var currentChunkAddr = currentChunk * partSize;
            var currentAddress = startingAddress;
            var currentDataAddr = 0;
            
            while((currentChunk + 1) * partSize < startingAddress + count)
            {
                currentChunkAddr = currentChunk * partSize;
                var nextChunkAddr = (currentChunk + 1) * partSize;

                var part = new OperationPart();
                part.OperationBlockSize = nextChunkAddr - currentAddress;
                part.AbsoluteMemBlockNumber = currentChunk;
                part.BlockOperationStart = currentAddress - currentChunkAddr;
                part.DataOperationStart = currentDataAddr;
                result.Add(part);

                currentAddress = nextChunkAddr;
                currentChunk++;
                currentDataAddr += part.OperationBlockSize;
            }
            
            currentChunkAddr = currentChunk * partSize;
            var lastPart = new OperationPart
            {
                OperationBlockSize = count - currentAddress,
                AbsoluteMemBlockNumber = currentChunk,
                BlockOperationStart = currentAddress - currentChunkAddr,
                DataOperationStart = currentAddress
            };

            result.Add(lastPart);

            return result;
        }
    }

    public class OperationPart
    {
        public int AbsoluteMemBlockNumber { get; set; }
        public int BlockOperationStart { get; set; }
        public int OperationBlockSize { get; set; }
        public int DataOperationStart { get; set; }

        public override string ToString()
        {
            return $"Block#:{AbsoluteMemBlockNumber};BS:{BlockOperationStart};DS:{DataOperationStart};SZ:{OperationBlockSize}";
        }
    }
}