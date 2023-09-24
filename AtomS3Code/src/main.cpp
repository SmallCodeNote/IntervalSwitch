#include <Arduino.h>
#include <M5Unified.h>

#include <EEPROM.h>

#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#include "forms.hpp"

/// @brief signal pin assign A-Phase
const int Pin_signalA = 7;

/// @brief signal pin assign B-Phase
const int Pin_signalB = 8;

/// @brief SwitchPinStatus
String SwitchStatus = "OFF";

/// @brief RunMode: Auto,ON,OFF
String RunMode = "Auto";

String DeviceConnected_String = "OFF";
String BLEDeviceAddress;

String EEPROMStatus_String = "initial";

/// @brief On time length[sec]
int LengthON = 5;
/// @brief Off time length[sec]
int LengthOFF = 10;
/// @brief last Timer count[sec]
int SwitchCount = 0;
/// @brief  showing monitor Timer count [sec]
int SwitchCount_ShowMonitorValue = 0;

/// @brief Encorder Profile Struct
struct DATA_SET
{
  /// @brief On time length[sec]
  int LengthON;

  /// @brief Off time length[sec]
  int LengthOFF;
};

/// @brief switching setting
DATA_SET data;

/// @brief Main Display
M5GFX Display_Main;
/// @brief Main Display Canvas
M5Canvas Display_Main_Canvas(&Display_Main);

/// @brief BLE Server
BLEServer *pServer = NULL;
BLECharacteristic *pTxCharacteristic;

bool deviceConnected = false;
bool oldDeviceConnected = false;

uint8_t txValue = 0;

// See the following for generating UUIDs:
// https://www.uuidgenerator.net/

#define SERVICE_UUID "6E400001-B5A3-F393-E0A9-E50E24DCCA9E" // UART service UUID
#define CHARACTERISTIC_UUID_RX "6E400002-B5A3-F393-E0A9-E50E24DCCA9E"
#define CHARACTERISTIC_UUID_TX "6E400003-B5A3-F393-E0A9-E50E24DCCA9E"

class MyServerCallbacks : public BLEServerCallbacks
{
  void onConnect(BLEServer *pServer)
  {
    deviceConnected = true;
    DeviceConnected_String = "ON";
  };

  void onDisconnect(BLEServer *pServer)
  {
    deviceConnected = false;
    DeviceConnected_String = "OFF";
  }
};

class MyCallbacks : public BLECharacteristicCallbacks
{
  void onWrite(BLECharacteristic *pCharacteristic)
  {
    std::string rxValue = pCharacteristic->getValue();
    String receiveString = String(rxValue.c_str());
    String modeswitch = getStringSplit(receiveString, ' ', 0);

    if (modeswitch.indexOf("lenon") >= 0)
    {
      LengthON = (getStringSplit(receiveString, ' ', 1).toInt());
    }
    else if (modeswitch.indexOf("lenoff") >= 0)
    {
      LengthOFF = (getStringSplit(receiveString, ' ', 1).toInt());
    }
    else if (modeswitch.indexOf("on") >= 0)
    {
      RunMode = "ON";
    }
    else if (modeswitch.indexOf("off") >= 0)
    {
      RunMode = "OFF";
    }
    else if (modeswitch.indexOf("auto") >= 0)
    {
      RunMode = "Auto";
    }
    else if (modeswitch.indexOf("save") >= 0)
    {
      data.LengthOFF = LengthOFF;
      data.LengthON = LengthON;

      EEPROM.put<DATA_SET>(0, data);
      EEPROM.commit();
      EEPROMStatus_String = "saved";
    }
    std::string answerString = ("[" + modeswitch + "]").c_str();
    pTxCharacteristic->setValue(answerString);
    pTxCharacteristic->notify();
  }
};

void InitializeComponent()
{
  M5.Lcd.printf("Start MainMonitor Initialize\r\n"); // LCDに表示

  // Main Display initialize
  M5.setPrimaryDisplayType(m5gfx::board_M5StackCore2);

  // M5GFX initialize
  Display_Main = M5.Display;
  int w = Display_Main.width();
  int h = Display_Main.height();

  // Create sprite for MainDisplay
  Display_Main_Canvas.createSprite(w, h);
  Display_Main_Canvas.setTextColor(0xffd500);
  Display_Main_Canvas.setFont(&fonts::lgfxJapanGothic_12);
  Display_Main_Canvas.setTextColor(0xffd500);
}

hw_timer_t *timer = NULL;
void IRAM_ATTR onTimer()
{
  SwitchCount++;

  if (SwitchCount >= LengthON + LengthOFF)
  {
    SwitchCount = 0;
  };

  if (RunMode == "Auto")
  {
    if (SwitchCount < LengthON)
    {
      digitalWrite(Pin_signalA, HIGH);
      digitalWrite(Pin_signalB, HIGH);
      SwitchStatus = "ON";
    }
    else
    {
      digitalWrite(Pin_signalA, LOW);
      digitalWrite(Pin_signalB, LOW);
      SwitchStatus = "OFF";
    }
  }
  else if (RunMode == "ON")
  {
    digitalWrite(Pin_signalB, HIGH);
    SwitchStatus = "ON";
  }
  else
  {
    digitalWrite(Pin_signalB, LOW);
    SwitchStatus = "OFF";
  }

  return;
}

void setup()
{
  pinMode(Pin_signalA, OUTPUT);
  pinMode(Pin_signalB, OUTPUT);

  EEPROM.begin(50); // 50byte
  EEPROM.get<DATA_SET>(0, data);

  if (data.LengthON <= 0)
  {
    data.LengthON = LengthON;
    data.LengthOFF = LengthOFF;
    EEPROMStatus_String = "initial";
  }
  else
  {
    LengthON = data.LengthON;
    LengthOFF = data.LengthOFF;
    EEPROMStatus_String = "loaded";
  }

  Serial.begin(115200);
  timer = timerBegin(0, 80, true);
  timerAttachInterrupt(timer, &onTimer, true);
  timerAlarmWrite(timer, 1000000, true);
  timerAlarmEnable(timer);

  m5::M5Unified::config_t cfg = M5.config();
  cfg.internal_rtc = false;

  M5.begin(cfg);
  M5.Power.begin();
  M5.Lcd.setBrightness(50);

  InitializeComponent();

  Form_Top = form_Top(Display_Main_Canvas, 0);
  FormView = &Form_Top;

  FormView->formEnable = true;

  // Create the BLE Device
  BLEDevice::init("AtomS3-UARTsv");

  // Create the BLE Server
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  // Create the BLE Service
  BLEService *pService = pServer->createService(SERVICE_UUID);

  // Create a BLE Characteristic
  pTxCharacteristic = pService->createCharacteristic(
      CHARACTERISTIC_UUID_TX,
      BLECharacteristic::PROPERTY_NOTIFY);

  pTxCharacteristic->addDescriptor(new BLE2902());

  BLECharacteristic *pRxCharacteristic = pService->createCharacteristic(
      CHARACTERISTIC_UUID_RX,
      BLECharacteristic::PROPERTY_WRITE);

  pRxCharacteristic->setCallbacks(new MyCallbacks());

  // Start the service
  pService->start();

  // Start advertising
  pServer->getAdvertising()->start();
  BLEAdvertising *pAdvertising = pServer->getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pServer->getAdvertising()->start();

  // Get BLE Device Info
  uint8_t macBT[6];
  esp_read_mac(macBT, ESP_MAC_BT);
  char BTaddress[20];
  sprintf(BTaddress, "%02X:%02X:%02X:%02X:%02X:%02X", macBT[0], macBT[1], macBT[2], macBT[3], macBT[4], macBT[5]);
  BLEDeviceAddress = String(BTaddress);
}

String TimePhase(struct tm timeinfo)
{
  char buff[9];
  sprintf(buff, "%02d:%02d:%02d", timeinfo.tm_hour, timeinfo.tm_min, timeinfo.tm_sec);
  return String(buff);
}

void loop()
{

  // BLE disconnecting
  if (!deviceConnected && oldDeviceConnected)
  {
    delay(500);                  // give the bluetooth stack the chance to get things ready
    pServer->startAdvertising(); // restart advertising
    Serial.println("start advertising");
    oldDeviceConnected = deviceConnected;
  }

  // BLE connecting
  if (deviceConnected && !oldDeviceConnected)
  {
    // do stuff here on connecting
    oldDeviceConnected = deviceConnected;
  }

  // get M5 last status
  M5.update();
  int BatteryVoltage = M5.Power.getBatteryVoltage();

  // Switch run mode
  if (M5.BtnA.wasDoubleClicked())
  {
    if (RunMode == "Auto")
    {
      RunMode = "ON";
    }
    else if (RunMode == "ON")
    {
      RunMode = "OFF";
    }
    else
    {
      RunMode = "Auto";
      SwitchCount = 0;
    };
  }

  // Update Display
  if (SwitchCount_ShowMonitorValue != SwitchCount)
  {
    SwitchCount_ShowMonitorValue = SwitchCount;
    struct tm timeinfo;
    getLocalTime(&timeinfo, 10U);
    FormView->draw(0, TimePhase(timeinfo) + "\t" + String(LengthON) + "\t" + String(LengthOFF) + "\t" + String(SwitchCount) + "\t" + RunMode + "\t" + SwitchStatus + "\t" + BLEDeviceAddress + "\t" + DeviceConnected_String + "\t" + EEPROMStatus_String);
  }

  delay(1);
}
