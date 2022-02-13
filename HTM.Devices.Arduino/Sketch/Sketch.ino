#include <OneWire.h> 
#include <DallasTemperature.h>

#define ONE_WIRE_BUS 2 
#define LED_PIN 4

String message = "";
char* buf;

OneWire oneWire(ONE_WIRE_BUS); 
DallasTemperature sensors(&oneWire);
void setup() 
{ 
  
  pinMode(LED_PIN, OUTPUT);
  sensors.begin();

   Serial.begin(9600);
} 
void loop() 
{ 
  String receiveVal;  
     
  if(Serial.available() > 0)
  {        
      receiveVal = Serial.readString();
      
    if(receiveVal == "GetTemperature")    
    {
      sensors.requestTemperatures(); 
      message.concat(sensors.getTempCByIndex(0));
      
      //dtostrf(sensors.getTempCByIndex(0), 6, 2, message);

      //message = sensors.getTempCByIndex(0);
      //message = "dupa jasia";
      
      digitalWrite(LED_PIN, 1);         
      delay(500);
      digitalWrite(LED_PIN, 0);   
    }
  }   
  
  sendMessage();
  delay(50);
} 

void sendMessage()
{
  if (message != "")
  {
    Serial.println(message);
    message = "";
  }
}