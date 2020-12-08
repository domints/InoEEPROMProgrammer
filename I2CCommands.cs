namespace InoEEPROMProgrammer
{
    /// <summary>
    /// Commands sent to I2C interface
    /// </summary>
    public static class I2CCommands
    {
        /// <summary>
        /// Acknowledge of last message
        /// </summary>
        public const byte ACK = 0x5A;
        /// <summary>
        /// Pings the device
        /// </summary>
        public const byte PING = 0x02;
        /// <summary>
        /// Pongs the device
        /// </summary>
        public const byte PONG = 0x01;
        /// <summary>
        /// Identify yourself
        /// </summary>
        public const byte IDENT = 0x10;
        /// <summary>
        /// Scan I2C bus for devices
        /// </summary>
        public const byte SCAN = 0x21;
        /// <summary>
        /// Sends n bytes to I2C bus
        /// </summary>
        public const byte SEND = 0x22;
        /// <summary>
        /// Reads n bytes from I2C bus
        /// </summary>
        public const byte READ = 0x23;
    }
}