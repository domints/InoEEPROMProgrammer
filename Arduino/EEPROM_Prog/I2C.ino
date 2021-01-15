void scanI2CBus()
{
  if(protocol != PROTO_I2C){
    Serial.write(ERR);
    return;
  }
  
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
