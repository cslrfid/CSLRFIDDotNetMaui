using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSLibrary;
using static CSLibrary.RFIDDEVICE;


namespace CSLRFIDMobile.Model
{
    public class CONFIG
    {
        public class MAINMENUSHORTCUT
        {
            public enum FUNCTION
            {
                NONE,
                INVENTORY,
                BARCODE,
            }

            public FUNCTION Function = FUNCTION.NONE;
            public uint DurationMin = 0;
            public uint DurationMax = 0;
        }

        public string readerID = ""; // device GUID
        public MODEL readerModel = MODEL.UNKNOWN;
        public int country = 0;

        public int BatteryLevelIndicatorFormat = 1; // 0 = voltage, other = percentage 

        //public int RFID_Power;
        public uint RFID_Profile;
        public int RFID_CompactInventoryDelayTime; // for CS108 only
        public int RFID_IntraPacketDelayTime;   // for CS710S only
        public CSLibrary.Constants.RadioOperationMode RFID_OperationMode;
        public bool RFID_ToggleTarget = true;
        public CSLibrary.Structures.TagGroup RFID_TagGroup;
        public CSLibrary.Constants.SingulationAlgorithm RFID_Algorithm;
        public CSLibrary.Structures.DynamicQParms RFID_DynamicQParms;
        public CSLibrary.Structures.FixedQParms RFID_FixedQParms;

        public string RFID_Region = "";
        public int RFID_FrequenceSwitch = 0; // 0 = hopping, 1 = fixed, 2 = agile
        public int RFID_FixedChannel = 0;

        // Multi Bank Inventory Setting
        public bool RFID_MBI_MultiBank1Enable;
        public CSLibrary.Constants.MemoryBank RFID_MBI_MultiBank1;
        public UInt16 RFID_MBI_MultiBank1Offset;
        public UInt16 RFID_MBI_MultiBank1Count;
        public bool RFID_MBI_MultiBank2Enable;
        public CSLibrary.Constants.MemoryBank RFID_MBI_MultiBank2;
        public UInt16 RFID_MBI_MultiBank2Offset;
        public UInt16 RFID_MBI_MultiBank2Count;

        // Main Menu Shortcut
        public MAINMENUSHORTCUT[] RFID_Shortcut = new MAINMENUSHORTCUT[6];

        public bool RFID_InventoryAlertSound = true;
        public bool RFID_DBm = true;

        public bool RFID_QOverride = false;
        public uint RFID_TagPopulation = 60;

        // Backend Server
        public bool RFID_SavetoFile = false;
        public bool RFID_SavetoCloud = true;
        public int RFID_CloudProtocol = 0;
        public string RFID_IPAddress;

        public bool RFID_Vibration = false;
        //public bool RFID_VibrationTag = false;      // false = New, true = All // only for CS108
        public uint RFID_VibrationWindow = 2;      // 2 seconds
        public uint RFID_VibrationTime = 300;       // 300 ms

        public bool[] RFID_AntennaEnable = new bool[16] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public uint[] RFID_Antenna_Power = new uint[16] { 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300 };
        public uint[] RFID_Antenna_Dwell = new uint[16] { 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000 };

        public int RFID_PowerSequencing_NumberofPower = 0;
        public uint[] RFID_PowerSequencing_Level = new uint[16] { 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300 };
        public uint[] RFID_PowerSequencing_DWell = new uint[16] { 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000 };

        public bool RFID_NewTagLocation = false;
        public int RFID_ShareFormat = 0;  // 0 = JSON, 1 = CSV, 2 = Excel CSV
        public bool RFID_Focus = false;
        public bool RFID_FastId = false;

        public uint RFID_BatteryPollingTime = 300;

        public string Impinj_AuthenticateServerURL;
        public string Impinj_AuthenticateEmail;
        public string Impinj_AuthenticatePassword;

        public int PowerUpperLimitIndBm = -50;
        public int PowerLowerLimitIndBm = -90;

        public bool _keepScreenOn = false;

        public byte RFID_DuplicateEliminationRollingWindow = 0;

        public CONFIG(MODEL model)
        {
            int port = 16;

            PowerUpperLimitIndBm = -50;
            PowerLowerLimitIndBm = -90;

            RFID_TagPopulation = 60;

            RFID_CompactInventoryDelayTime = 0; // for CS108 only
            RFID_IntraPacketDelayTime = 4; // for CS710S only

            RFID_AntennaEnable = new bool[port];
            RFID_Antenna_Power = new uint[port];
            RFID_Antenna_Dwell = new uint[port];
            for (uint cnt = 0; cnt < port; cnt++)
            {
                RFID_Antenna_Power[cnt] = 300;

                if (cnt == 0)
                {
                    RFID_AntennaEnable[0] = true;
                    if (port == 1)
                        RFID_Antenna_Dwell[0] = 0;
                    else
                        RFID_Antenna_Dwell[0] = 2000;
                }
                else
                {
                    RFID_AntennaEnable[cnt] = false;
                    RFID_Antenna_Dwell[cnt] = 2000;
                }
            }

            RFID_OperationMode = CSLibrary.Constants.RadioOperationMode.CONTINUOUS;
            RFID_TagGroup = new CSLibrary.Structures.TagGroup(CSLibrary.Constants.Selected.ALL, CSLibrary.Constants.Session.S0, CSLibrary.Constants.SessionTarget.A);
            RFID_Algorithm = CSLibrary.Constants.SingulationAlgorithm.DYNAMICQ;
            switch (model)
            {
                case MODEL.CS710S:
                    //                  Set profile to 241 if CS710S-1 in application
                    //                    if (country == 1)
                    //                        RFID_Profile = 241;
                    //                    else
                    RFID_Profile = 343;
                    break;

                default:
                    RFID_Profile = 1;
                    break;
            }

            RFID_DynamicQParms = new CSLibrary.Structures.DynamicQParms();
            RFID_DynamicQParms.minQValue = 0;
            RFID_DynamicQParms.startQValue = 7;
            RFID_DynamicQParms.maxQValue = 15;
            RFID_DynamicQParms.toggleTarget = 1;
            RFID_DynamicQParms.MinQCycles = 3;
            RFID_DynamicQParms.QIncreaseUseQuery = true;
            RFID_DynamicQParms.QDecreaseUseQuery = true;
            RFID_DynamicQParms.NoEPCMaxQ = 8;

            RFID_FixedQParms = new CSLibrary.Structures.FixedQParms();
            RFID_FixedQParms.qValue = 7;
            RFID_FixedQParms.toggleTarget = 1;

            RFID_MBI_MultiBank1Enable = false;
            RFID_MBI_MultiBank2Enable = false;
            RFID_MBI_MultiBank1 = CSLibrary.Constants.MemoryBank.TID;
            RFID_MBI_MultiBank1Offset = 0;
            RFID_MBI_MultiBank1Count = 2;
            RFID_MBI_MultiBank2 = CSLibrary.Constants.MemoryBank.USER;
            RFID_MBI_MultiBank2Offset = 0;
            RFID_MBI_MultiBank2Count = 2;

            RFID_InventoryAlertSound = true;
            RFID_QOverride = false;
            RFID_DBm = true;
            RFID_Focus = false;
            RFID_FastId = false;
            RFID_SavetoFile = false;
            RFID_SavetoCloud = true;
            RFID_CloudProtocol = 0;
            RFID_IPAddress = "";

            RFID_Vibration = false;
            //RFID_VibrationTag = false;      // false = New, true = All
            RFID_VibrationWindow = 2;      // 2 seconds
            RFID_VibrationTime = 300;       // 500 ms

            RFID_BatteryPollingTime = 300;  // 300s

            RFID_DuplicateEliminationRollingWindow = 0;

            Impinj_AuthenticateServerURL = "https://democloud.convergence.com.hk/ias";
            Impinj_AuthenticateEmail = "";
            Impinj_AuthenticatePassword = "";

            _keepScreenOn = false;

            for (int cnt = 0; cnt < RFID_Shortcut.Length; cnt++)
            {
                MAINMENUSHORTCUT item = new MAINMENUSHORTCUT();

                switch (cnt)
                {
                    case 0:
                        item.Function = MAINMENUSHORTCUT.FUNCTION.INVENTORY;
                        item.DurationMin = 0;
                        item.DurationMax = 500;
                        break;
                    case 1:
                        item.Function = MAINMENUSHORTCUT.FUNCTION.BARCODE;
                        item.DurationMin = 500;
                        item.DurationMax = 10000;
                        break;
                }

                RFID_Shortcut[cnt] = item;
            }

        }
    }
}
