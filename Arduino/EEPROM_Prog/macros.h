#define send_int32(x) Serial.write(x & 0xFF); Serial.write((x >> 8) & 0xFF); Serial.write((x >> 16) & 0xFF); Serial.write((x >> 24) & 0xFF)
#define read_int32() Serial.read() | (Serial.read() << 8) | (Serial.read() << 16) | (Serial.read() << 24)
