#include <OneWire.h> 
#include <DallasTemperature.h>

#define ONE_WIRE_BUS 2 
#define DEBUG_LED_PIN 13
#define LED_PIN 4
#define BUTTON_PIN 7

const String Event = "Event";
const String CommandResponse = "CommandResponse";

String message = "";
char* buf;
bool ledState;
int buttonValue = HIGH;

OneWire oneWire(ONE_WIRE_BUS); 
DallasTemperature sensors(&oneWire);
void setup() 
{ 
  
  pinMode(DEBUG_LED_PIN, OUTPUT);
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  
  sensors.begin();

   Serial.begin(9600);
} 
void loop() 
{ 
  String command;  

  int currentButtonValue = digitalRead(BUTTON_PIN);
  if (currentButtonValue != buttonValue)
  {
    buttonValue = currentButtonValue;
    joinMessage(Event, buttonValue == LOW ? "ButtonLow" : "ButtonHigh");
  }
     
  if (Serial.available() > 0)
  {        
    command = Serial.readString();
      
    if(command == "GetTemperature")    
    {
      digitalWrite(DEBUG_LED_PIN, 1);
      sensors.requestTemperatures(); 

      String value = "";
      value.concat(sensors.getTempCByIndex(0)); 
      joinMessage(CommandResponse, value);
      digitalWrite(DEBUG_LED_PIN, 0);
    }

    if(command == "TurnLedOn")
    {
      digitalWrite(LED_PIN, 1);
      ledState = true;

      joinMessage(CommandResponse, "ok");
    }

    if(command == "TurnLedOff")
    {
      digitalWrite(LED_PIN, 0);
      ledState = false;

      joinMessage(CommandResponse, "ok");
    }

    if(command == "GetLedState")
    {
      joinMessage(CommandResponse, ledState ? "1" : "0");
    }
  }   
  
  sendMessage();
  delay(10);
} 

void joinMessage(String type, String text)
{
  message += type + "_" + text + "|";
}

void sendMessage()
{
  if (message != "")
  {
    Serial.println(message);
    message = "";
  }
}