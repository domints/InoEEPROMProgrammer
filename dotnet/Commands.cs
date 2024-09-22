namespace InoEEPROMProgrammer
{
    /// <summary>
    /// Commands sent to I2C interface
    /// </summary>
    public static class Commands
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
        public const byte IDENT = 0x03;
        /// <summary>
        /// Get device status
        /// </summary>
        public const byte STATUS = 0x04;
        public const byte SET_SPEED = 0x05;

        /// <summary>
        /// Sets up the I2C EEPROM mode
        /// </summary>
        public const byte SETUP_I2C = 0x10;

        /// <summary>
        /// Define EEPROM size
        /// </summary>
        public const byte SET_SIZE = 0x20;
        /// <summary>
        /// Define EEPROM word size
        /// </summary>
        public const byte SET_WORD = 0x21;

        /// <summary>
        /// Scan I2C bus for devices
        /// </summary>
        public const byte SCAN = 0x30;
    }
}