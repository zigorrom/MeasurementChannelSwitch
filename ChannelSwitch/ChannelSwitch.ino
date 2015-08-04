#include "CmdMessenger.h"

#define MAX_CHANNEL 32
unsigned long previousToggleLed = 0;
bool ledState = 0;
const int LED_PIN = 13;


CmdMessenger cmdMessenger = CmdMessenger(Serial);

enum
{
  kWatchdog,
  kAcknowledge,
  kSwitchChannel,
  kError,
  //ConfirmChannel, 
};

void attachCommandCallbacks()
{
  cmdMessenger.attach(OnUnknownCommand);
  cmdMessenger.attach(kWatchdog, OnWatchdogRequest); 
  cmdMessenger.attach(kSwitchChannel, OnChannelSwitch);
    //cmdMessenger.attach(
}

void OnWatchdogRequest()
{
  cmdMessenger.sendCmd(kWatchdog, "517cea54-8f17-4761-b735-094897c20ffd");
}

void OnUnknownCommand()
{
  cmdMessenger.sendCmd(kError,"Command without attached callback");
}

void OnChannelSwitch()
{
  uint16_t channelNumber = cmdMessenger.readBinArg<uint16_t>();
  bool state = cmdMessenger.readBinArg<bool>();

  if(cmdMessenger.isArgOk()&&channelNumber>0&&channelNumber<=MAX_CHANNEL)
  {
    SwitchChannel(channelNumber, state);
    cmdMessenger.sendCmdStart(kAcknowledge);
    cmdMessenger.sendCmdBinArg<uint16_t>(channelNumber);
    cmdMessenger.sendCmdEnd();    
  }else
  {
    cmdMessenger.sendCmd(kError,"Wrong channel number");
  }
  
}

void SwitchChannel(uint16_t number, bool state)
{
  
}

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  cmdMessenger.printLfCr(false);

  attachCommandCallbacks();

  pinMode(LED_PIN, OUTPUT);
}



bool hasExpired(unsigned long &prevTime, unsigned long interval) {
  if (  millis() - prevTime > interval ) {
    prevTime = millis();
    return true;
  } else     
    return false;
}



void toggleLed()
{
  ledState=!ledState;
  digitalWrite(LED_PIN,ledState?HIGH:LOW);
}


void loop() {
  // put your main code here, to run repeatedly:
    cmdMessenger.feedinSerialData(); 
    if(hasExpired(previousToggleLed,100))
      toggleLed();
  
}
