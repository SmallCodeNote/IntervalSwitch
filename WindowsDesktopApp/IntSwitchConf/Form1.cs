using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.Runtime.InteropServices.WindowsRuntime;        //Asbuffer

namespace IntSwitchConf
{
    public partial class Form1 : Form
    {
        static BluetoothLEAdvertisementWatcher watcher;
        public Form1()
        {
            InitializeComponent();
            StartBle();                 //起動と同時にBleデバイスとの接続を試みる。
        }
        static public string BTaddress;
        static public string DeviceInfo;
        static public string DeviceName;
        static public string DeviceUUID;
        static public string textLog;
        static public string textWriteData;

        static public Dictionary<string, string> DeviceInfoDictionary = new Dictionary<string, string>();
        static public Dictionary<string, ulong> DeviceAddressDictionary = new Dictionary<string, ulong>();
        static public Dictionary<string, string> DeviceNameDictionary = new Dictionary<string, string>();
        static public Dictionary<string, string> DeviceUuidDictionary = new Dictionary<string, string>();

        static public  string targetUUID = "6E400001-B5A3-F393-E0A9-E50E24DCCA9E";
        static public string s_CHARACTERISTIC_UUID_RX = "6E400002-B5A3-F393-E0A9-E50E24DCCA9E";
        static public string s_CHARACTERISTIC_UUID_TX = "6E400003-B5A3-F393-E0A9-E50E24DCCA9E";

        //ラベルの文字をタイマーで更新する
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelNotifyData.Text = textLog;
            labelWriteData.Text = textWriteData;

            string Lines = "";

            foreach(var Line in DeviceInfoDictionary)
            {

                Lines += Line.Key +"\t"+ Line.Value + "\t" + DeviceNameDictionary[Line.Key] + "\t[" + DeviceUuidDictionary[Line.Key] + "]\r\n";

            }

            textBox1.Text = Lines;

            if(DeviceAddressDictionary.Count != comboBox_DiveceList.Items.Count)
            {
                comboBox_DiveceList.Items.Clear();

                foreach(var item in DeviceAddressDictionary)
                {
                    comboBox_DiveceList.Items.Add(item.Key);
                }

            }

            if (comboBox_DiveceList.Text.Length ==12)
            {
                if (DeviceNameDictionary.ContainsKey(comboBox_DiveceList.Text))
                {
                    label_DeviceName.Text = DeviceNameDictionary[comboBox_DiveceList.Text];
                }

            }

        }

        //スキャン他　起動時に呼び出し
        public static async void StartBle()
        {
            watcher = new BluetoothLEAdvertisementWatcher();
            watcher.Received += Watcher_Received;
            watcher.ScanningMode = BluetoothLEScanningMode.Active;
            watcher.Start();
            textWriteData = "Write : 0";
        }
        static bool isBleFind = false;
        static bool isBleNotify = false;
        static bool isBleWrite = false;

        static ulong targetBTaddress = 0;

        public static void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var bleServiceUUIDs = args.Advertisement.ServiceUuids;
            BTaddress =  args.BluetoothAddress.ToString("X12");
            DeviceInfo = "["+args.AdvertisementType.ToString() + "\t" + args.Timestamp.ToString()+"]";
            DeviceName = args.Advertisement.LocalName;
            DeviceUUID = "";

            foreach (var uuidone in bleServiceUUIDs)
            {
                DeviceUUID = uuidone.ToString();
            }

            if (!DeviceInfoDictionary.ContainsKey(BTaddress))
            {
                DeviceInfoDictionary.Add(BTaddress, DeviceInfo + "[" + DeviceUUID+"]");
                DeviceAddressDictionary.Add(BTaddress, args.BluetoothAddress);
                DeviceNameDictionary.Add(BTaddress, DeviceName);
                DeviceUuidDictionary.Add(BTaddress, DeviceUUID);
            }
            else
            {
                DeviceInfoDictionary[BTaddress] = DeviceInfo;
                if (DeviceNameDictionary[BTaddress].Length == 0) { DeviceNameDictionary[BTaddress] = DeviceName; }
                if (DeviceUuidDictionary[BTaddress].Length == 0) { DeviceUuidDictionary[BTaddress] = DeviceUUID; }
            }

        }
        public static byte[] RxData;
        public static GattCharacteristic CHARACTERISTIC_UUID_RX;
        public static GattCharacteristic CHARACTERISTIC_UUID_TX;
        //Notifyによる受信時の処理
        public static void characteristicBleDevice(GattCharacteristic sender, GattValueChangedEventArgs eventArgs)
        {
            byte[] data = new byte[eventArgs.CharacteristicValue.Length];
            Windows.Storage.Streams.DataReader.FromBuffer(eventArgs.CharacteristicValue).ReadBytes(data);
            RxData = data;
            textLog = "Notfy : " + RxData[0].ToString() + "," + RxData[1].ToString();
            return;
        }
        //終了時の処理
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isBleFind == true) CHARACTERISTIC_UUID_RX.Service.Dispose();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        
        async private void button_Connect_Click(object sender, EventArgs e)
        {
            watcher.Stop();
            textBox1.Text = "";
            textBox1.Enabled = false;


            BluetoothLEDevice dev = await BluetoothLEDevice.FromBluetoothAddressAsync(DeviceAddressDictionary[comboBox_DiveceList.Text]);
            targetUUID = DeviceUuidDictionary[comboBox_DiveceList.Text];
            GattDeviceServicesResult serviceSet = await dev.GetGattServicesForUuidAsync(new Guid(targetUUID));

            GattDeviceService service = serviceSet.Services[0];

            GattCharacteristicsResult characteristicsTx = await service.GetCharacteristicsForUuidAsync(new Guid(s_CHARACTERISTIC_UUID_RX));
            
            GattCharacteristicsResult characteristicsRx = await service.GetCharacteristicsForUuidAsync(new Guid(s_CHARACTERISTIC_UUID_TX));

            CHARACTERISTIC_UUID_RX = characteristicsRx.Characteristics[0];
            CHARACTERISTIC_UUID_TX = characteristicsTx.Characteristics[0];
            isBleNotify = true;
            isBleWrite = true;

            CHARACTERISTIC_UUID_RX.ValueChanged += characteristicBleDevice;

            button_AUTO.Enabled = true;
            button_OFF.Enabled = true;
            button_ON.Enabled = true;
            button_LenON.Enabled = true;
            button_LenOFF.Enabled = true;
            button_SAVE.Enabled = true;

        }

//コマンドボタン処理
        byte[] TXdata = { 0 };
        private async void button_ON_Click(object sender, EventArgs e)
        {
            if (isBleWrite == true)
            {
                TXdata = Encoding.GetEncoding("Shift_JIS").GetBytes("on\0");
                GattCommunicationStatus result = await CHARACTERISTIC_UUID_TX.WriteValueAsync(TXdata.AsBuffer());
            }
        }

        private async void button_OFF_Click(object sender, EventArgs e)
        {
            if (isBleWrite == true)
            {
                TXdata = Encoding.GetEncoding("Shift_JIS").GetBytes("off\0");
                GattCommunicationStatus result = await CHARACTERISTIC_UUID_TX.WriteValueAsync(TXdata.AsBuffer());
            }
        }

        private async void button_AUTO_Click(object sender, EventArgs e)
        {
            if (isBleWrite == true)
            {
                TXdata = Encoding.GetEncoding("Shift_JIS").GetBytes("auto\0");
                GattCommunicationStatus result = await CHARACTERISTIC_UUID_TX.WriteValueAsync(TXdata.AsBuffer());
            }
        }

        private async void button_SAVE_Click(object sender, EventArgs e)
        {
            if (isBleWrite == true)
            {
                TXdata = Encoding.GetEncoding("Shift_JIS").GetBytes("save\0");
                GattCommunicationStatus result = await CHARACTERISTIC_UUID_TX.WriteValueAsync(TXdata.AsBuffer());
            }
        }

        private async void button_LenON_Click(object sender, EventArgs e)
        {
            if (isBleWrite == true)
            {
                TXdata = Encoding.GetEncoding("Shift_JIS").GetBytes("lenon "+textBox_LenON.Text +"\0");
                GattCommunicationStatus result = await CHARACTERISTIC_UUID_TX.WriteValueAsync(TXdata.AsBuffer());
            }
        }
        private async void button_LenOFF_Click(object sender, EventArgs e)
        {
            if (isBleWrite == true)
            {
                TXdata = Encoding.GetEncoding("Shift_JIS").GetBytes("lenoff " + textBox_LenOFF.Text + "\0");
                GattCommunicationStatus result = await CHARACTERISTIC_UUID_TX.WriteValueAsync(TXdata.AsBuffer());
            }
        }
    }
}
