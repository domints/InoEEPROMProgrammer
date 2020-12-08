#include <Wire.h>

#define ACK 0x5A
#define NUL (byte)0x00
#define PING 0x02
#define PONG 0x01
#define IDENT 0x10
#define SCAN 0x21
#define SEND 0x22
#define READ 0x23
#define ERR 0xFE

#define LED 13
#define LED_DEFAULT LOW

void setup() {
#ifdef LED
  pinMode(LED, OUTPUT);
  digitalWrite(LED, LED_DEFAULT);
#endif
  Wire.begin();
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
      case SCAN:
        scanBus();
        return;
      case SEND:
        sendI2C();
        return;
      case READ:
        readI2C();
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
  Serial.write(16);
  Serial.print("I2C Bridge v0.1");
  Serial.write(NUL);
}

void scanBus()
{
  Serial.write(ACK);
  byte addresses[127];
  byte foundCount = 0;
  for(byte address = 1; address < 127; address++ )
  {
#ifdef LED
    digitalWrite(LED, !LED_DEFAULT);
#endif

    Wire.beginTransmission(address);
    byte error = Wire.endTransmission();

#ifdef LED
    digitalWrite(LED, LED_DEFAULT);
#endif
    if(error == 0)
    {
      addresses[foundCount] = address;
      foundCount++;
    }
    if(error == 4)
    {
      addresses[foundCount] = address | 0x80;
      foundCount++;
    }
  }

  Serial.write(foundCount);
  for(byte i = 0; i < foundCount; i++)
  {
    Serial.write(addresses[i]);
  }
}

void sendI2C()
{
  waitForSerial(3);
  byte devAddress = Serial.read();
  uint16_t count = 0x00;
  count |= Serial.read();
  count |= (Serial.read() << 8);
  waitForSerial(count);
  Wire.beginTransmission(devAddress);
  for(uint16_t i = 0; i < count; i++)
  {
#ifdef LED
    digitalWrite(LED, !LED_DEFAULT);
#endif

    waitForSerial(1);
    Wire.write(Serial.read());

#ifdef LED
    digitalWrite(LED, LED_DEFAULT);
#endif
  }
  Wire.endTransmission();
  Serial.write(ACK);
}

void readI2C()
{
  waitForSerial(3);
  byte devAddress = Serial.read();
  uint16_t count = 0x00;
  count |= Serial.read();
  count |= (Serial.read() << 8);
  Serial.write(ACK);
  uint16_t received = Wire.requestFrom(devAddress, count);
  Serial.write(received & 0xFF);
  Serial.write((received >> 8) & 0xFF);
  for(uint16_t i = 0; i < received; i++)
  {
#ifdef LED
    digitalWrite(LED, !LED_DEFAULT);
#endif

    while(!Wire.available());

#ifdef LED
    digitalWrite(LED, LED_DEFAULT);
#endif

    Serial.write(Wire.read());
  }
}

void waitForSerial(uint16_t count)
{
  while(Serial.available() < count);
}
