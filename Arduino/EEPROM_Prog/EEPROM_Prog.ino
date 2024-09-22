#include <Wire.h>

#include "defines.h"
#include "macros.h"
#include "commands.h"

byte buffer[BUFFER_SIZE];
byte protocol = NUL;

int32_t mem_size = 0;
byte word_size = 0;

void setup() {
  Serial.begin(115200);
  while (!Serial);
  
}

void loop() {
  if(Serial.available())
  {
    byte cmd = Serial.read();
    switch(cmd)
    {
      case PING:
        Serial.write(PONG);
        return;
      case IDENT:
        identifyMyself();
        return;
      case SETUP_I2C:
        resetProto();
        Wire.begin();
        protocol = PROTO_I2C;
        Serial.write(ACK);
        return;
      case SET_SIZE:
        mem_size = read_int32();
        Serial.write(ACK);
        return;
      case SET_WORD:
        word_size = Serial.read();
        Serial.write(ACK);
        return;
      case SET_SPEED:
        uint32_t newSpeed = read_int32();
        Serial.write(ACK);
        Serial.begin(newSpeed);

      case SCAN_I2C_BUS:
        scanI2CBus();
        return;

      default:
        Serial.write(ERR);
        return;
    }
  }
}

void identifyMyself()
{
  Serial.write(ACK);
  Serial.write(19 + 9); // 19 bytes of label + 9 bytes of descriptors
  Serial.print("EEPROM Writer v0.1");
  Serial.write(NUL);
  send_int32(BUFFER_SIZE);
  Serial.write(SPEED_SWITCHABLE);
  send_int32(MAX_SPEED);
}

void resetProto()
{
  protocol = NUL;
}
