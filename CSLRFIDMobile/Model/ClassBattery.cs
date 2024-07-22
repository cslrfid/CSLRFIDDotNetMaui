using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLRFIDMobile.Model
{
    public static class ClassBattery
    {
        public enum BATTERYMODE
        {
            INVENTORY = 1,
            IDLE = 2,
        }

        public enum BATTERYLEVELSTATUS
        {
            NORMAL = 0,
            LOW = 1,
        }



        // for inventory mode
        readonly static double[] voltageTable1 = new double[] { 4.106, 4.017, 3.98, 3.937, 3.895, 3.853, 3.816, 3.779, 3.742, 3.711, 3.679, 3.658, 3.637, 3.626, 3.61, 3.584, 3.547, 3.515, 3.484, 3.457, 3.431, 3.399, 3.362, 3.32, 3.251, 3.135 };
        readonly static double[] capacityTable1 = new double[] {  100,    96,   92,    88,    84,    80,    76,    72,    67,    63,    59,    55,    51,    47,   43,    39,    35,    31,    27,    23,    19,    15,    11,    7,     2,     0 };        
        readonly static double[] voltageSlope1 = new double[voltageTable1.Length - 1];

        // for non-inventory mode
        readonly static double[] voltageTable2 = new double[] { 4.212, 4.175, 4.154, 4.133, 4.112, 4.085, 4.069, 4.054, 4.032, 4.011, 3.99, 3.969, 3.953, 3.937, 3.922, 3.901, 3.885, 3.869, 3.853, 3.837, 3.821, 3.806, 3.79, 3.774, 3.769, 3.763, 3.758, 3.753, 3.747, 3.742, 3.732, 3.721, 3.705, 3.684, 3.668, 3.652, 3.642, 3.626, 3.615, 3.605, 3.594, 3.584, 3.568, 3.557, 3.542, 3.531, 3.510, 3.494, 3.473, 3.457, 3.436, 3.41, 3.362, 3.235, 2.987, 2.982 };
        readonly static double[] capacityTable2 = new double[] {  100,    98,    96,    95,    93,    91,    89,    87,    85,    84,   82,    80,    78,    76,    75,    73,    71,    69,    67,    65,    64,    62,   60,    58,    56,    55,    53,    51,    49,    47,    45,    44,    42,    40,    38,    36,    35,    33,    31,    29,    27,    25,    24,    22,    20,    18,    16,    15,    13,    11,     9,    7,     5,     4,     2,     0 };        
        readonly static double[] voltageSlope2 = new double[voltageTable2.Length - 1];

        static double[] voltageTable = new double[] { };
        static double[] capacityTable = new double[] { };
        static double[] voltageSlope = new double[] { };

        static BATTERYMODE _currentInventoryMode;

        static ClassBattery()
        {
            int cnt;

            for (cnt = 0; cnt < voltageTable1.Length - 2; cnt++)
                voltageSlope1[cnt] = (capacityTable1[cnt] - capacityTable1[cnt + 1]) / (voltageTable1[cnt] - voltageTable1[cnt + 1]);

            for (cnt = 0; cnt < voltageTable2.Length - 2; cnt++)
                voltageSlope2[cnt] = (capacityTable2[cnt] - capacityTable2[cnt + 1]) / (voltageTable2[cnt] - voltageTable2[cnt + 1]);

            SetBatteryMode(BATTERYMODE.IDLE);
        }

        public static void SetBatteryMode(BATTERYMODE bm)
        {
            _currentInventoryMode = bm;

            if (bm == BATTERYMODE.INVENTORY)
            {
                voltageTable = voltageTable1;
                capacityTable = capacityTable1;
                voltageSlope = voltageSlope1;
            }
            else
            {
                voltageTable = voltageTable2;
                capacityTable = capacityTable2;
                voltageSlope = voltageSlope2;
            }
        }

        public static BATTERYLEVELSTATUS BatteryLow(double voltage)
        {
            if (Voltage2Percent(voltage) <= 20.0)
                return BATTERYLEVELSTATUS.LOW;

            return BATTERYLEVELSTATUS.NORMAL;
        }

        public static double Voltage2Percent(double voltage)
        {
            int cnt;

            if (voltage > voltageTable[0])
                return 100;

            if (voltage <= voltageTable[voltageTable.Length - 1])
                return 0;

            for (cnt = voltageTable.Length - 2; cnt >= 0; cnt--)
            {
                if (voltage > voltageTable[cnt])
                    continue;

                if (voltage == voltageTable[cnt])
                    return capacityTable[cnt];

                double percent = 0;

                percent = (voltage - voltageTable[cnt + 1]) * voltageSlope[cnt] + capacityTable[cnt + 1];

                return percent;
            }

            return 0;
        }
    }

}
