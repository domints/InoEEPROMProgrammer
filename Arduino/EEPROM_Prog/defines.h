#define NUL (byte)0x00
#define PROTO_I2C 0x01

#if defined(__AVR_ATmega32U4__)

#define BUFFER_SIZE 1536 // 1.5K
#define SPEED_SWITCHABLE 1
#define MAX_SPEED 2000000

#elif defined(__AVR_ATmega16U4__)

#define BUFFER_SIZE 512 // 0.5K
#define SPEED_SWITCHABLE 1
#define MAX_SPEED 2000000

#elif defined(__AVR_ATmega328P__)

#define BUFFER_SIZE 1024 // 1K
#define SPEED_SWITCHABLE 0
#define MAX_SPEED 0

#elif defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__) || defined(__AVR_ATmega1284__) || defined(__AVR_ATmega1284P__) || defined(__AVR_ATmega644__) || defined(__AVR_ATmega644A__) || defined(__AVR_ATmega644P__) || defined(__AVR_ATmega644PA__)

#define BUFFER_SIZE 4 * 1024 // 4K
#define SPEED_SWITCHABLE 0
#define MAX_SPEED 0

#elif defined(STM32F4xx)
  #if defined(STM32F410Cx) || defined(STM32F410Rx) || defined(STM32F410Tx)
    #define BUFFER_SIZE 16 * 1024 // 19K
    #define SPEED_SWITCHABLE 1
    #define MAX_SPEED 2000000
  #elif defined(STM32F401xE) || defined(STM32F401xC)
    #define BUFFER_SIZE 32 * 1024 // 32K
    #define SPEED_SWITCHABLE 1
    #define MAX_SPEED 2000000
  #else
    #define BUFFER_SIZE 64 * 1024 // 64K
    #define SPEED_SWITCHABLE 1
    #define MAX_SPEED 2000000
  #endif
#else

//#define BUFFER_SIZE 64 * 1024 // 64K
//#define SPEED_SWITCHABLE 1
//#define MAX_SPEED 2000000

#endif
