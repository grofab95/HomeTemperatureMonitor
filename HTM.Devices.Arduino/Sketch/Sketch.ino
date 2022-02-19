#include <OneWire.h> 
#include <DallasTemperature.h>

#define ONE_WIRE_BUS 2 
#define DEBUG_LED_PIN 13
#define LED_PIN 4

String message = "";
char* buf;
int ledState;

OneWire oneWire(ONE_WIRE_BUS); 
DallasTemperature sensors(&oneWire);
void setup() 
{ 
  
  pinMode(DEBUG_LED_PIN, OUTPUT);
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
      digitalWrite(DEBUG_LED_PIN, 1);
      sensors.requestTemperatures(); 
      message.concat(sensors.getTempCByIndex(0)); 
      digitalWrite(DEBUG_LED_PIN, 0);
    }

    if(receiveVal == "TurnLedOn")
    {
      digitalWrite(LED_PIN, 1);
      message = "ok";
    }

    if(receiveVal == "TurnLedOff")
    {
      digitalWrite(LED_PIN, 0);
      message = "ok";
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