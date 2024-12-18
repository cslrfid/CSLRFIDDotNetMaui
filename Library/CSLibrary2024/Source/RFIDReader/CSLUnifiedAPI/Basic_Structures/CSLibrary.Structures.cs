using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Specialized;

namespace CSLibrary.Structures
{
    using CSLibrary.Constants;
    /// <summary>
    /// GPI Interrupt callback
    /// </summary>
    /// <param name="ip">Source IP address</param>
    /// <param name="GPI0">GPI0 Status : 1 = on, -1 = off, 0 = not available</param>
    /// <param name="GPI1">GPI1 Status : 1 = on, -1 = off, 0 = not available</param>
    /// <returns></returns>
    public delegate bool GPI_INTERRUPT_CALLBACK
    (
         String ip,
         int GPI0,
         int GPI1
    );
    /// <summary>
    /// Tag access callback
    /// </summary>
    /// <param name="pkt">packet data</param>
    /// <returns>return false will abort operation</returns>
    public delegate bool TagAccessCallbackDelegate
    (
            TAG_ACCESS_PKT pkt
    );

    /// <summary>
    /// Tag access callback
    /// </summary>
    /// <param name="pkt">packet data</param>
    /// <returns>return false will abort operation</returns>
    public delegate bool InventoryCallbackDelegate
    (
            INVENTORY_PKT pkt
    );

    /// <summary>
    /// Tag Access Packet
    /// </summary>
    public struct TAG_ACCESS_PKT
    {
        private UInt32 flags;
        /// <summary>
        /// ISO 18000-6C access command
        /// </summary>
        public RFID_18K6C cmd;
        /// <summary>
        /// Current millisecond timer/counter
        /// </summary>
        public UInt32 ms_ctr;
        /// <summary>
        /// Please check success flags first
        /// <para>If the tag backscattered an error (i.e. the tag backscatter error </para>
        /// <para>flag is set), this value is the error code that the tag </para>
        /// <para>backscattered. Values are: </para>
        /// <para>  – general error (catch-all for errors not covered by codes) </para>
        /// <para>  – specified memory location does not exist of the PC value </para>
        /// <para>      is not supported by the tag </para>
        /// <para>  – specified memory location is locked and/or permalocked </para>
        /// <para>      and is not writeable </para>
        /// <para>  – tag has insufficient power to perform the memory write </para>
        /// <para>  – tag does not support error-specific codes </para>
        /// </summary>
        public TAG_BACKSCATTERED_ERROR errorCode;
        /// <summary>
        /// Tag Access data
        /// </summary>
        public S_DATA data;
        private static readonly int ISO_18000_6C_ErrorFlag;
        private static readonly int ISO_18000_6C_BackscatterErrorFlag;
        private static readonly int ISO_18000_6C_AckTimeoutFlag;
        private static readonly int ISO_18000_6C_CRCInvalidFlag;

/*
		static TAG_ACCESS_PKT()
        {
            // Create Boolean Mask
            ISO_18000_6C_ErrorFlag = UInt32.CreateMask();
            ISO_18000_6C_BackscatterErrorFlag = UInt32.CreateMask(ISO_18000_6C_ErrorFlag);
            ISO_18000_6C_AckTimeoutFlag = UInt32.CreateMask(ISO_18000_6C_BackscatterErrorFlag);
            ISO_18000_6C_CRCInvalidFlag = UInt32.CreateMask(ISO_18000_6C_AckTimeoutFlag);
        }
*/

			/*

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flags">error flags</param>
        /// <param name="cmd">access command</param>
        /// <param name="ms">timer</param>
        /// <param name="errorCode">error code</param>
        /// <param name="data">data</param>
        public TAG_ACCESS_PKT(byte flags, RFID_18K6C cmd, UInt32 ms, TAG_BACKSCATTERED_ERROR errorCode, byte[] data)
        {
            this.flags = new UInt32(flags);
            this.cmd = cmd;
            this.ms_ctr = ms;
            this.errorCode = errorCode;
            this.data = new S_DATA(data);
        }
		*/


		/* 
			   /// <summary>
			   /// <para>false = Access operation succeeded </para>
			   /// <para>true  = An error occurred.  If one of the following 
			   /// error-specific bit fields does not indicate an 
			   /// error, the error code appears in the data
			   /// field. </para>
			   /// </summary>
			   public bool IsError
			   {
				   get { return flags[ISO_18000_6C_ErrorFlag]; }
			   }
			   /// <summary>
			   /// <para>Tag backscatter error flag: </para>
			   /// <para>false = Tag did not backscatter an error. </para>
			   /// <para>true  = Tag backscattered an error.  See 
			   /// <paramref name="error_code"/> field. </para>
			   /// </summary>
			   public bool IsBackscatterError
			   {
				   get { return flags[ISO_18000_6C_BackscatterErrorFlag]; }
			   }

			   /// <summary>
			   /// <para>ACK timeout flag: </para>
			   /// <para>false = Tag responded within timeout. </para>
			   /// <para>true  = Tag failed to respond within timeout. </para>
			   /// </summary>
			   public bool IsAckTimeout
			   {
				   get { return flags[ISO_18000_6C_AckTimeoutFlag]; }
			   }
			   /// <summary>
			   /// <para>CRC invalid flag: </para>
			   /// <para>false = CRC was valid </para>
			   /// <para>true  = CRC was invalid  </para>
			   /// </summary>
			   public bool IsCRCInvalid
			   {
				   get { return flags[ISO_18000_6C_CRCInvalidFlag]; }
			   }
	   */

	};

    /// <summary>
    /// Tag Access Packet
    /// </summary>
    public struct INVENTORY_PKT
    {
        private UInt32 flags;
        /// <summary>
        /// Current millisecond timer/counter
        /// </summary>
        public UInt32 ms_ctr;
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI).
        /// </summary>
        public Single rssi;
        /// <summary>
        /// <para>The value of the R1000’s ANA_CTRL1 register at time the RSSI </para>
        /// <para>measurement was taken - contains the IF LNA’s gain info for RSSI. </para>
        /// <para>Bits 15:6: reserved for future use </para>
        /// <para>Bits 5:4: IF LNA gain. 0=24dB, 1=18dB, 3=12dB </para>
        /// <para>Bits 3:0: reserved for future use </para>
        /// </summary>
        public UInt16 ana_ctrl;
        /// <summary>
        /// Antenna port number currently using.
        /// </summary>
        public UInt16 ana_port;
        /// <summary>
        /// EPC is truncated.
        /// </summary>
        public bool isTruncate;
        /// <summary>
        /// Protocol Control
        /// </summary>
        public string pc;
        /// <summary>
        /// Electronic Product Code
        /// </summary>
        public string epc;
        private static readonly int ISO_18000_6C_CRCInvalidFlag;

/*
        static INVENTORY_PKT()
        {
            // Create Boolean Mask
            ISO_18000_6C_CRCInvalidFlag = UInt32.CreateMask();
        }*/


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flags">error flags</param>
        /// <param name="ms">error flags</param>
        /// <param name="rssi">access command</param>
        /// <param name="ana_ctrl">
        /// <para>The value of the R1000’s ANA_CTRL1 register at time the RSSI </para>
        /// <para>measurement was taken - contains the IF LNA’s gain info for RSSI. </para>
        /// <para>Bits 15:6: reserved for future use </para>
        /// <para>Bits 5:4: IF LNA gain. 0=24dB, 1=18dB, 3=12dB </para>
        /// <para>Bits 3:0: reserved for future use </para>
        /// </param>
        /// <param name="ana_port">ana_port</param>
        /// <param name="isTruncate">isTruncate</param>
        /// <param name="pc">data</param>
        /// <param name="epc">data</param>
/*        public INVENTORY_PKT(byte flags, UInt32 ms, Single rssi, UInt16 ana_ctrl, UInt16 ana_port, bool isTruncate, string pc, string epc)
        {
            this.flags = new UInt32(flags);
            this.ms_ctr = ms;
            this.rssi = rssi;
            this.ana_ctrl = ana_ctrl;
            this.ana_port = ana_port;
            this.pc = pc;
            this.epc = epc;
            this.isTruncate = isTruncate;
        }
*/

/*
        /// <summary>
        /// <para>CRC invalid flag: </para>
        /// <para>false = CRC was valid </para>
        /// <para>true  = CRC was invalid  </para>
        /// </summary>
        public bool IsCRCInvalid
        {
            get { return flags[ISO_18000_6C_CRCInvalidFlag]; }
        }
		*/

    } ;

#if NOUSE
    /// <summary>
    /// Tag Invnentory Raw Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TagCallbackInfo : TagCallbackInfo
    {
        /// <summary>
        /// Record index number
        /// </summary>
        public Int32 index = -1;
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI).  This is the 
        /// backscattered tag signal. It is 
        /// important to note that the IF LNA gain in the receive path can vary 
        /// each time carrier wave is turned on, so the IF LNA gain should be 
        /// taken into account. Refer to byte offsets 15:14 for a description of 
        /// the “ana_ctrl1” field, which includes the setting of the IF LNA at the 
        /// time the RSSI measurement was taken. 
        /// </summary>
        public float rssi;//4
        /// <summary>
        /// total count
        /// </summary>
        public UInt32 count = 1;//4
        /// <summary>
        /// R1000 Firmware millisecond counter tag was inventoried.
        /// </summary>
        public UInt32 ms_ctr;//4
        /// <summary>
        /// The value of the R1000's ANA_CTRL1 register at time the RSSI 
        /// measurement was taken - contains the IF LNA's gain info for RSSI. 
        /// Bits 15:6: reserved for future use 
        /// Bits 5:4: IF LNA gain. 0=24dB, 1=18dB, 3=12dB 
        /// Bits 3:0: reserved for future use 
        /// </summary>
        public UInt16 ana_ctrl1;
        /// <summary>
        /// Constructor
        /// </summary>
        public TagCallbackInfo() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi">
        /// The Receive Signal Strength Indicator (RSSI).  This is the 
        /// backscattered tag signal. It is 
        /// important to note that the IF LNA gain in the receive path can vary 
        /// each time carrier wave is turned on, so the IF LNA gain should be 
        /// taken into account. Refer to byte offsets 15:14 for a description of 
        /// the “ana_ctrl1” field, which includes the setting of the IF LNA at the 
        /// time the RSSI measurement was taken. 
        ///</param>
        /// <param name="count">total count</param>
        /// <param name="ms_ctr">R1000 Firmware millisecond counter tag was inventoried.</param>
        /// <param name="ana_ctrl1">
        /// The value of the R1000's ANA_CTRL1 register at time the RSSI 
        /// measurement was taken - contains the IF LNA's gain info for RSSI. 
        /// Bits 15:6: reserved for future use 
        /// Bits 5:4: IF LNA gain. 0=24dB, 1=18dB, 3=12dB 
        /// Bits 3:0: reserved for future use 
        ///</param>
        /// <param name="pc">Protocol Control Value</param>
        /// <param name="epc">Electronic Product Code Value</param>
        public TagCallbackInfo(float rssi, uint count, uint ms_ctr, UInt16 ana_ctrl1, S_PC pc, S_EPC epc)
        {
            this.rssi = rssi;
            this.count = count;
            this.ms_ctr = ms_ctr;
            this.ana_ctrl1 = ana_ctrl1;
            base.pc = pc;
            base.epc = epc;
        }
    }
    /// <summary>
    /// Tag Invnentory Data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class TagCallbackInfo : TagCallbackInfo
    {
        /// <summary>
        /// Record index number
        /// </summary>
        public Int32 index = -1;
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI).  This is the 
        /// backscattered tag signal. It is 
        /// important to note that the IF LNA gain in the receive path can vary 
        /// each time carrier wave is turned on, so the IF LNA gain should be 
        /// taken into account. Refer to byte offsets 15:14 for a description of 
        /// the “ana_ctrl1” field, which includes the setting of the IF LNA at the 
        /// time the RSSI measurement was taken. 
        /// </summary>
        public float rssi;//4
        /// <summary>
        /// total count
        /// </summary>
        public UInt32 count = 1;//4
        /// <summary>
        /// Constructor
        /// </summary>
        public TagCallbackInfo() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi">
        /// The Receive Signal Strength Indicator (RSSI).  This is the 
        /// backscattered tag signal. It is 
        /// important to note that the IF LNA gain in the receive path can vary 
        /// each time carrier wave is turned on, so the IF LNA gain should be 
        /// taken into account. Refer to byte offsets 15:14 for a description of 
        /// the “ana_ctrl1” field, which includes the setting of the IF LNA at the 
        /// time the RSSI measurement was taken. 
        ///</param>
        /// <param name="count">total count</param>
        /// <param name="pc">Protocol Control Value</param>
        /// <param name="epc">Electronic Product Code Value</param>
        /// <param name="type">Information type</param>
        public TagCallbackInfo(float rssi, uint count, S_PC pc, S_EPC epc, DATA_TYPE type)
        {
            this.rssi = rssi;
            this.count = count;
            base.pc = pc;
            base.epc = epc;
            base.type = type;
        }
    }

    /// <summary>
    /// Tag Searching Data
    /// </summary>
    public class TagCallbackInfo : TagCallbackInfo
    {
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI).  This is the 
        /// backscattered tag signal. It is 
        /// important to note that the IF LNA gain in the receive path can vary 
        /// each time carrier wave is turned on, so the IF LNA gain should be 
        /// taken into account. Refer to byte offsets 15:14 for a description of 
        /// the “ana_ctrl1” field, which includes the setting of the IF LNA at the 
        /// time the RSSI measurement was taken. 
        /// </summary>
        public float rssi;//4
        /// <summary>
        /// Constructor
        /// </summary>
        public TagCallbackInfo() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi">
        /// The Receive Signal Strength Indicator (RSSI).  This is the 
        /// backscattered tag signal. It is 
        /// important to note that the IF LNA gain in the receive path can vary 
        /// each time carrier wave is turned on, so the IF LNA gain should be 
        /// taken into account. Refer to byte offsets 15:14 for a description of 
        /// the “ana_ctrl1” field, which includes the setting of the IF LNA at the 
        /// time the RSSI measurement was taken. 
        ///</param>
        /// <param name="pc">Protocol Control Value</param>
        /// <param name="epc">Electronic Product Code Value</param>
        /// <param name="type">Information type</param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc, DATA_TYPE type)
        {
            this.rssi = rssi;
            base.pc = pc;
            base.epc = epc;
            base.type = type;
        }
    }
    /// <summary>
    /// Tag Access Data
    /// </summary>
    public class TAG_ACCESS_RECORD
    {
        /// <summary>
        /// callback data
        /// </summary>
        public S_DATA data;
        /// <summary>
        /// Millisecond Counter
        /// </summary>
        public UInt32 ms_ctr = 0;
        /// <summary>
        /// Access Type
        /// </summary>
        public RFID_18K6C_TAG_ACCESS tagAccess = RFID_18K6C_TAG_ACCESS.UNKNOWN;
        /// <summary>
        /// Error Type
        /// </summary>
        public RFID_ACCESS tagResult = RFID_ACCESS.UNKNOWN;
        /// <summary>
        /// default constructor
        /// </summary>
        public TAG_ACCESS_RECORD() { }
    }
#endif
    /// <summary>
    /// Tag Callback Information
    /// </summary>
    /// 
    public class TagCallbackInfo// : IComparable
    {
        /// <summary>
        /// Index number, First come with small number.
        /// </summary>
        public Int32 index = -1;
        /// <summary>
        /// Crc error flag
        /// </summary>
        public bool crcInvalid = false;
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI dBuV).
        /// </summary>
        public float rssi = 0;//4
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI dBm).
        /// </summary>
        public float rssidBm = 0;
        /// <summary>
        /// total count
        /// </summary>
        public UInt32 count = 1;//4
        /// <summary>
        /// Current antenna port using.
        /// </summary>
        public UInt32 antennaPort = 0;
        /// <summary>
        ///  Tag deteched frequency
        /// </summary>
        public uint freqChannel = 0xffffffff;
        /// <summary>
        /// Phase
        /// </summary>
        public Int16 phase;
        /// <summary>
        /// PC Data (QT Private PC)
        /// </summary>
        public S_PC pc = new S_PC();//2
        /// <summary>
        /// XPC W1 Data (QT Private XPC W1)
        /// </summary>
        public S_XPC_W1 xpc_w1 = new S_XPC_W1();//2
        /// <summary>
        /// XPC W2 Data (QT Private XPC W2)
        /// </summary>
        public S_XPC_W2 xpc_w2 = new S_XPC_W2();//2
        /// <summary>
        /// EPC Data (QT Private EPC)
        /// </summary>
        public S_EPC epc = new S_EPC();//68
        /// <summary>
        /// for extend EPC
        /// </summary>
        public UInt16 [] FastTid = new UInt16[0];
        /// <summary>
        /// EPC String Length (QT Private EPC Length)
        /// </summary>
        //public uint epcstrlen = 0;
		/// <summary>
		/// Bank 1 data
		/// </summary>
		public UInt16[] Bank1Data = new UInt16[0];
		/// <summary>
		/// Bank 2 data
		/// </summary>
		public UInt16[] Bank2Data = new UInt16[0];
        /// <summary>
        /// Bank 3 data
        /// </summary>
        public UInt16[] Bank3Data = new UInt16[0];
        /// <summary>
        /// QT Public PC
        /// </summary>
        public S_PC pcpublic = new S_PC();//2
        /// <summary>
        /// QT Publuc EPC Data
        /// </summary>
        public S_EPC epcpublic = new S_EPC();//68
        /// <summary>
        /// QT Public EPC String Length
        /// </summary>
        public uint epcpublicstrlen = 0;
        /// <summary>
        /// ms_ctr
        /// </summary>
        public UInt32 ms_ctr = 0;
        /// <summary>
        /// crc16
        /// </summary>
        public UInt16 crc16 = 0;
        /// <summary>
        /// Receive Time
        /// </summary>
        public DateTime receiveTime = DateTime.Now;
        /// <summary>
        /// RFID Device name
        /// </summary>
        public string name = String.Empty;
#if nouse
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(int index, float rssi, uint count, uint antennaPort, byte freqChannel, S_PC pc, S_EPC epc, uint ms_ctr, string name)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
            this.antennaPort = antennaPort;
            this.freqChannel = freqChannel;
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(int index, float rssi, uint count, uint antennaPort, byte freqChannel, S_PC pc, S_EPC epc, string name)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
            this.antennaPort = antennaPort;
            this.freqChannel = freqChannel;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc, string name)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(int index, S_PC pc, S_EPC epc, string name)
        {
            this.index = index;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(S_PC pc, S_EPC epc, string name)
        {
            this.pc = pc;
            this.epc = epc;
            this.name = name;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="xpc_w1"></param>
        /// <param name="xpc_w2"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_XPC_W1 xpc_w1, S_XPC_W2 xpc_w2, S_EPC epc, uint ms_ctr)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.xpc_w1 = xpc_w1;
            this.xpc_w2 = xpc_w2;
            this.epc = epc;
            this.epcstrlen = (pc.EPCLength - 2) * 4;
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_EPC epc, uint ms_ctr)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }

            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_EPC epc)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_EPC epc, UInt16 AntennaPort)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.epcstrlen = pc.EPCLength;
            this.antennaPort = AntennaPort;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc, uint ms_ctr)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc, uint ms_ctr, UInt16 AntennaPort)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            this.antennaPort = AntennaPort;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, S_PC pc, S_EPC epc, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.name = readerName;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, uint antennaPort, byte freqChannel, S_PC pc, S_EPC epc, uint ms_ctr, UInt16 crc16, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.name = readerName;
            this.antennaPort = antennaPort;
            this.freqChannel = freqChannel;
            this.ms_ctr = ms_ctr;
            this.crc16 = crc16;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, uint antennaPort, byte freqChannel, S_PC pc, S_EPC epc, S_PC pcpublic, S_EPC epcpublic, uint ms_ctr, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.pcpublic = pcpublic;
            this.epcpublic = epcpublic;
            this.name = readerName;
            this.antennaPort = antennaPort;
            this.freqChannel = freqChannel;
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, uint antennaPort, byte freqChannel, S_PC pc, S_EPC epc, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.name = readerName;
            this.antennaPort = antennaPort;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(int index, S_PC pc, S_EPC epc)
        {
            this.index = index;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(S_PC pc, S_EPC epc)
        {
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public TagCallbackInfo()
        {

        }
        /*
        public int CompareTo(object item)
        {
            Type type = item.GetType();
            if (type == typeof(Int32))
            {
                return index.CompareTo((Int32)item);
            }
            else if (type == typeof(float))
            {
                return rssi.CompareTo((float)item);
            }
            else if (type == typeof(UInt32))
            {
                return count.CompareTo((UInt32)item);
            }
            else if (type == typeof(S_EPC))
            {
                return epc.CompareTo((S_EPC)item);
            }
            else if (type == typeof(S_PC))
            {
                return pc.CompareTo((S_PC)item);
            }
            return 0;
        }*/

#endif
    }

















#if nouse
    public class TagCallbackInfo// : IComparable
    {
        /// <summary>
        /// Index number, First come with small number.
        /// </summary>
        public Int32 index = -1;
        /// <summary>
        /// Crc error flag
        /// </summary>
        public bool crcInvalid = false;
        /// <summary>
        /// The Receive Signal Strength Indicator (RSSI).
        /// </summary>
        public float rssi = 0;//4
        /// <summary>
        /// total count
        /// </summary>
        public UInt32 count = 1;//4
        /// <summary>
        /// Current antenna port using.
        /// </summary>
        public UInt32 antennaPort = 0;
        /// <summary>
        /// PC Data
        /// </summary>
        public S_PC pc = new S_PC();//2
        /// <summary>
        /// XPC W1 Data
        /// </summary>
        public S_XPC_W1 xpc_w1 = new S_XPC_W1();//2
        /// <summary>
        /// XPC W2 Data
        /// </summary>
        public S_XPC_W2 xpc_w2 = new S_XPC_W2();//2
        /// <summary>
        /// EPC Data
        /// </summary>
        public S_EPC epc = new S_EPC();//68
        /// <summary>
        /// EPC String Length
        /// </summary>
        public uint epcstrlen = 0;
        /// <summary>
        /// ms_ctr
        /// </summary>
        public UInt32 ms_ctr = 0;
        /// <summary>
        /// RFID Device name
        /// </summary>
        public string name = String.Empty;
#if CS203
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(int index, float rssi, uint count, uint antennaPort, S_PC pc, S_EPC epc, uint ms_ctr, string name)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
            this.antennaPort = antennaPort;
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(int index, float rssi, uint count, uint antennaPort, S_PC pc, S_EPC epc, string name)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
            this.antennaPort = antennaPort;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc, string name)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(int index, S_PC pc, S_EPC epc, string name)
        {
            this.index = index;
            this.pc = pc;
            this.epc = epc;
            this.name = name;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="name"></param>
        public TagCallbackInfo(S_PC pc, S_EPC epc, string name)
        {
            this.pc = pc;
            this.epc = epc;
            this.name = name;
        }
#endif
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="xpc_w1"></param>
        /// <param name="xpc_w2"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_XPC_W1 xpc_w1, S_XPC_W2 xpc_w2, S_EPC epc, uint ms_ctr)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.xpc_w1 = xpc_w1;
            this.xpc_w2 = xpc_w2;
            this.epc = epc;
            this.epcstrlen = (pc.EPCLength - 2) * 4;
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_EPC epc, uint ms_ctr)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort [] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC (epc.ToString().Substring (8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }

            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rssi"></param>
        /// <param name="count"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(int index, float rssi, uint count, S_PC pc, S_EPC epc)
        {
            this.index = index;
            this.rssi = rssi;
            this.count = count;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc, uint ms_ctr)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(float rssi, S_PC pc, S_EPC epc)
        {
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, S_PC pc, S_EPC epc, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.name = readerName;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="ms_ctr"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, uint antennaPort, S_PC pc, S_EPC epc, uint ms_ctr, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.name = readerName;
            this.antennaPort = antennaPort;
            this.ms_ctr = ms_ctr;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crcInvalid"></param>
        /// <param name="rssi"></param>
        /// <param name="antennaPort"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        /// <param name="readerName"></param>
        public TagCallbackInfo(bool crcInvalid, float rssi, uint antennaPort, S_PC pc, S_EPC epc, string readerName)
        {
            this.crcInvalid = crcInvalid;
            this.rssi = rssi;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
            this.name = readerName;
            this.antennaPort = antennaPort;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(int index, S_PC pc, S_EPC epc)
        {
            this.index = index;
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="epc"></param>
        public TagCallbackInfo(S_PC pc, S_EPC epc)
        {
            this.pc = pc;
            this.epcstrlen = pc.EPCLength;
            if (pc.XI == true)
            {
                ushort[] epcushort = epc.ToUshorts();

                this.epcstrlen--;
                this.xpc_w1 = new S_XPC_W1(epcushort[0]);

                if (this.xpc_w1.XEB == true)
                {
                    this.epcstrlen--;
                    this.xpc_w2 = new S_XPC_W2(epcushort[1]);
                    if (pc.EPCLength > 2)
                        this.epc = new S_EPC(epc.ToString().Substring(8));
                    else
                        this.epc = new S_EPC();
                }
                else
                {
                    if (pc.EPCLength > 1)
                        this.epc = new S_EPC(epc.ToString().Substring(4));
                    else
                        this.epc = new S_EPC();
                }
            }
            else
            {
                this.epc = epc;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public TagCallbackInfo()
        {

        }
        /*
        public int CompareTo(object item)
        {
            Type type = item.GetType();
            if (type == typeof(Int32))
            {
                return index.CompareTo((Int32)item);
            }
            else if (type == typeof(float))
            {
                return rssi.CompareTo((float)item);
            }
            else if (type == typeof(UInt32))
            {
                return count.CompareTo((UInt32)item);
            }
            else if (type == typeof(S_EPC))
            {
                return epc.CompareTo((S_EPC)item);
            }
            else if (type == typeof(S_PC))
            {
                return pc.CompareTo((S_PC)item);
            }
            return 0;
        }*/
    }
#endif

#if NOUSE
    /// <summary>
    /// Error Data Structure
    /// </summary>
    public class TAG_ERROR_RECORD
    {
        /// <summary>
        /// Error Code
        /// </summary>
        public ErrorCode ErrorCode = ErrorCode.UNKNOWN;
        /// <summary>
        /// Error Type
        /// </summary>
        public ErrorType ErrorType = ErrorType.UNKNOWN;
        /// <summary>
        /// default constructor
        /// </summary>
        public TAG_ERROR_RECORD() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="errType"></param>
        public TAG_ERROR_RECORD(ErrorCode errCode, ErrorType errType)
        {
            ErrorCode = errCode;
            ErrorType = errType;
        }
    }
#endif
    /// <summary>
    /// Electronic Product Code
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_EPC : IBANK
    {
#region Private Member
        private ushort[] m_data;
#endregion

#region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_EPC() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="epc">epc in string format, must be smaller than or equal to 62 hex numbers</param>
        public S_EPC(string epc)
        {
            m_data = CSLibrary.Tools.Hex.ToUshorts(epc);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="epc">epc in ushort array format, must be smaller than or equal to 31</param>
        public S_EPC(ushort[] epc)
        {
            m_data = (ushort[])epc.Clone();
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="epc">epc in ushort array format, must be smaller than or equal to 31</param>
        /// <param name="count">number of ushort copy to local</param>
        public S_EPC(ushort[] epc, int count)
        {
            m_data = new ushort[count];
            Array.Copy(epc, m_data, count);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="epc">epc in byte array format, must be smaller than or equal to 62 bytes</param>
        public S_EPC(Byte[] epc)
        {
            m_data = (ushort[])CSLibrary.Tools.Hex.ToUshorts(epc).Clone();
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? m_data : null;
        }

        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToBytes(m_data) : null;
        }

        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToString(m_data) : "";
        }

        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }

        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_EPC))
            {
                return Compare(m_data, ((S_EPC)item).ToUshorts());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (ushort[])item);
            }
            return 0;
        }

        private  int Compare(ushort[] b1, ushort[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

            for (int i = 0; i < len; i++)
            {
                if (b2[i] > b1[i])
                    return 1;
                else if (b2[i] < b1[i])
                    return -1;
            }
            return 0;

#if nouse
            fixed (ushort* bp1 = b1)
            {
                fixed (ushort* bp2 = b2)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (bp2[i] > bp1[i])
                            return 1;
                        else if (bp2[i] < bp1[i])
                            return -1;
                    }
                    return 0;
                }
            }
#endif
        }
#endregion
    }

#if CSharpLibrary
 * /// <summary>
    /// Protocol Control(must be 2 Bytes)
    /// </summary>
    public class S_PC : IBANK
    {
#region Private Member
        private byte[] m_data;
#endregion

#region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_PC() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in string format, must be 4 hex numbers</param>
        public S_PC(string pc)
        {
            m_data = new byte[2];
            m_data = CSLibrary.Tools.HexEncoding.ToBytes(pc);
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="src"></param>
        public S_PC(S_PC src)
        {
            if (src == null) return;
            m_data = (Byte[])src.m_data.Clone();
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in ushort format</param>
        public S_PC(ushort pc)
        {
            m_data = new byte[] { (byte)(pc >> 8), (byte)(pc & 0xff) };
        }
        internal S_PC(byte[] src)
        {
            m_data = (byte[])(src.Clone());
        }
        internal S_PC(byte [] src, int length)
        {
            m_data = new byte[length];

            Array.Copy(src, m_data, length);
            
/*            
            fixed (byte* pdest = m_data)
            {
                Win32Wrapper.memcpy(pdest, src, length);
            }
 */
        }

#endregion

#region extern function
        /*
		/// <summary>
        /// Get 16bit EPC Length from current PC value
        /// </summary>
        public uint EPCLength
        {
            get { return (uint)(m_data[0] >> 11 & 0x1F); }
        }
		*/
        /// <summary>
        /// Get 16bit EPC Length from current PC value
        /// </summary>
        public uint EPCLength
        {
            get { return (uint)(m_data[0] >> 3 & 0x1F); }
        }
        /// <summary>
        /// User Memory Indicator, true if user memory contains data.
        /// Notes: Not all tags support this function
        /// </summary>
        public bool UMI
        {
            get { return ((m_data[0] >> 10 & 0x1) == 0x1); }
        }
        /// <summary>
        /// An XPC_W1 Indicator, true if XPC_W1 is non-zero value
        /// Notes: Not all tags support this function
        /// </summary>
        public bool XI
        {
            get { return ((m_data[0] >> 9 & 0x1) == 0x1); }
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? CSLibrary.Tools.HexEncoding.ToShorts(m_data) : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? CSLibrary.Tools.HexEncoding.ToUshorts(m_data) : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.HexEncoding.ToString(m_data);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_PC))
            {
                return Compare(m_data, ((S_PC)item).ToBytes());
            }
            else if (item.GetType() == typeof(byte[]))
            {
                return Compare(m_data, (byte[])item);
            }
            return 0;
        }
        private int Compare(byte[] b1, byte[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;
            return Win32.memcmp(b1, b2, len);
        }
#endregion

#region implicit
        /// <summary>
        /// Convert to string
        /// </summary>
        public static implicit operator string(S_PC pc)
        {
            return pc.ToString();
        }

        public static implicit operator UInt16(S_PC p)
        {
            return (UInt16)(p.m_data[0] << 8 | p.m_data[1]);
        }

        /// <summary>
        /// Convert ushort to pc
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static implicit operator S_PC(ushort pc)
        {
            return new S_PC(pc);
        }
#endregion
    }
#endif
    
    /// <summary>
    /// Protocol Control(must be 2 Bytes)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_PC : IBANK
    {
#region Private Member
        private ushort[] m_data;
#endregion

#region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_PC() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in string format, must be 4 hex numbers</param>
        public S_PC(string pc)
        {
            m_data = new ushort[1];
            m_data[0] = CSLibrary.Tools.Hex.ToUshort(pc);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in ushort format</param>
        public S_PC(ushort pc)
        {
            m_data = new ushort[] { pc };
        }

#endregion

#region extern function
        /// <summary>
        /// Get EPC Word(16bit) Length from current PC value
        /// </summary>
        public uint EPCLength
        {
            get {
                uint length = (uint)(m_data[0] >> 11 & 0x1F);

                if (this.XI)
                    length--;

                return length; 
            }
        }
        /// <summary>
        /// User Memory Indicator, true if user memory contains data.
        /// Notes: Not all tags support this function
        /// </summary>
        public bool UMI
        {
            get { return ((m_data[0] >> 10 & 0x1) == 0x1); }
        }
        /// <summary>
        /// An XPC_W1 Indicator, true if XPC_W1 is non-zero value
        /// Notes: Not all tags support this function
        /// </summary>
        public bool XI
        {
            get { return ((m_data[0] >> 9 & 0x1) == 0x1); }
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? m_data : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToBytes(m_data) : null;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.Hex.ToString(m_data);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }

        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_PC))
            {
                return Compare(m_data, ((S_PC)item).ToUshorts());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (ushort[])item);
            }
            return 0;
        }

        private  int Compare(ushort[] b1, ushort[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;
        
            for (int i = 0; i < len; i++)
            {
                if (b2[i] > b1[i])
                    return 1;
                else if (b2[i] < b1[i])
                    return -1;
            }

            return 0;
        }

        public static implicit operator UInt16(S_PC p)
        {
            return p.m_data[0];
        }

        public static implicit operator S_PC(UInt16 p)
        {
            return new S_PC(p);
        }
#endregion
    }

    /// <summary>
    /// X Protocol Control W1 (must be 2 Bytes)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_XPC_W1 : IBANK
    {
#region Private Member
        private ushort[] m_data;
#endregion

#region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_XPC_W1() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in string format, must be 4 hex numbers</param>
        public S_XPC_W1(string pc)
        {
            m_data = new ushort[1];
            m_data[0] = CSLibrary.Tools.Hex.ToUshort(pc);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in ushort format</param>
        public S_XPC_W1(ushort pc)
        {
            m_data = new ushort[] { pc };
        }
#endregion

#region extern function
        /// <summary>
        /// An XPC_W2 Indicator, true if XPC_W2 is non-zero value
        /// Notes: Not all tags support this function
        /// </summary>
        public bool XEB
        {
            get { return ((m_data[0] >> 15 & 0x1) == 0x1); }
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? m_data : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToBytes(m_data) : null;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (m_data == null)
                return "";

            return CSLibrary.Tools.Hex.ToString(m_data);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_PC))
            {
                return Compare(m_data, ((S_PC)item).ToUshorts());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (ushort[])item);
            }
            return 0;
        }
        private  int Compare(ushort[] b1, ushort[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

            for (int i = 0; i < len; i++)
            {
                if (b2[i] > b1[i])
                    return 1;
                else if (b2[i] < b1[i])
                    return -1;
            }
            return 0;
        }
#endregion
    }

    /// <summary>
    /// X Protocol Control W2 (must be 2 Bytes)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_XPC_W2 : IBANK
    {
#region Private Member
        private ushort[] m_data;
#endregion

#region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_XPC_W2() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in string format, must be 4 hex numbers</param>
        public S_XPC_W2(string pc)
        {
            m_data = new ushort[1];
            m_data[0] = CSLibrary.Tools.Hex.ToUshort(pc);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pc">pc in ushort format</param>
        public S_XPC_W2(ushort pc)
        {
            m_data = new ushort[] { pc };
        }
#endregion

#region extern function
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? m_data : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToBytes(m_data) : null;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (m_data == null)
                return "";

            return CSLibrary.Tools.Hex.ToString(m_data);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_PC))
            {
                return Compare(m_data, ((S_PC)item).ToUshorts());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (ushort[])item);
            }
            return 0;
        }
        private  int Compare(ushort[] b1, ushort[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

            for (int i = 0; i < len; i++)
            {
                if (b2[i] > b1[i])
                    return 1;
                else if (b2[i] < b1[i])
                    return -1;
            }
            return 0;
        }
#endregion
    }


    /// <summary>
    /// TID
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_TID : IBANK
    {
#region Private Member
        private ushort[] m_data;
#endregion

#region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_TID() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="tid">pc in string format, must be 4 hex numbers</param>
        public S_TID(string tid)
        {
            m_data = CSLibrary.Tools.Hex.ToUshorts(tid);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="tid">Tid in byte array format</param>
        public S_TID(ushort[] tid)
        {
            m_data = (ushort[])tid.Clone();
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="tid">Tid in byte array format</param>
        /// <param name="count">number of ushort copy to local</param>
        public S_TID(ushort[] tid, int count)
        {
            m_data = new ushort[count];
            Array.Copy(tid, m_data, count);
        }
#endregion

#region extern function
        /// <summary>
        /// Get Allocation Class ID
        /// </summary>
        public ACID GetACID
        {
            get
            {
                if (m_data != null && m_data.Length > 0)
                {
                    return (ACID)(m_data[0] >> 8);
                }
                return ACID.UNKNOWN;
            }
        }
        /// <summary>
        /// Get EPC Mask Designer ID
        /// </summary>
        public EpcMDID GetEpcID
        {
            get
            {
                if (m_data != null && m_data.Length > 2)
                {
                    return (EpcMDID)((m_data[0] & 0xff) | m_data[1] >> 12);
                }
                return EpcMDID.UNKNOWN;
            }
        }
        /// <summary>
        /// Get ISO Mask Designer ID
        /// </summary>
        public IsoMDID GetIsoID
        {
            get
            {
                if (m_data != null && m_data.Length > 1)
                {
                    return (IsoMDID)(m_data[0] & 0xff);
                }
                return IsoMDID.UNKNOWN;
            }
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? m_data : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToBytes(m_data) : null;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.Hex.ToString(m_data);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_TID))
            {
                return Compare(m_data, ((S_TID)item).ToUshorts());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (ushort[])item);
            }
            return 0;
        }
        private  int Compare(ushort[] b1, ushort[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

//            fixed (ushort* bp1 = b1)
            {
//                fixed (ushort* bp2 = b2)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (b2[i] > b1[i])
                            return 1;
                        else if (b2[i] < b1[i])
                            return -1;
                    }
                    return 0;
                }
            }
        }
#endregion

    }
    /// <summary>
    /// Custom Password Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_PWD : IBANK
    {
#region Private Member
        private ushort[] m_data;
        /// <summary>
        /// Password
        /// </summary>
        private UInt32 m_pwd
        {
            set
            {
                m_data = new ushort[2];
                m_data[0] = (ushort)(value >> 16);
                m_data[1] = (ushort)value;
            }
            get
            {
                return (UInt32)((m_data[0] << 16) | m_data[1]);
            }
        }  
#endregion

#region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_PWD() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="data">pc in string format, must be 8 hex numbers</param>
        public S_PWD(string data)
        {
            m_pwd = UInt32.Parse(data, System.Globalization.NumberStyles.HexNumber);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="pwd">password in Uint32 format</param>
        public S_PWD(uint pwd)
        {
            m_pwd = pwd;
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? m_data : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToBytes(m_data) : null;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.Hex.ToString(m_data);
        }
        public string ToString(string format)
        {
            return m_pwd.ToString (format);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_PWD))
            {
                return Compare(m_data, ((S_PWD)item).ToUshorts());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (ushort[])item);
            }
            return 0;
        }
        private  int Compare(ushort[] b1, ushort[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

//            fixed (ushort* bp1 = b1)
            {
//                fixed (ushort* bp2 = b2)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (b2[i] > b1[i])
                            return 1;
                        else if (b2[i] < b1[i])
                            return -1;
                    }
                    return 0;
                }
            }
        }

        public static implicit operator UInt32(S_PWD p)
        {
            return p.m_pwd;
        }

        public static implicit operator S_PWD(UInt32 p)
        {
            return new S_PWD (p);
        }
#endregion
    }

    /// <summary>
    /// Custom Data Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class S_DATA : IBANK
    {       
#region Private Member
        private byte[] m_data;
#endregion

#region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public S_DATA() { }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="src">data in string format</param>
        public S_DATA(string src)
        {
            this.m_data = CSLibrary.Tools.Hex.ToBytes(src);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="src">data in byte array format</param>
         public S_DATA(byte[] src)
        {
            m_data = src;
/*
 * 
            m_data = new byte[src.Length];

             fixed (byte* pDest = m_data, pSrc = src)
            {
                Win32Wrapper.memcpy(pDest, pSrc, src.Length);
            }
 */
         }

/*
 * internal S_DATA(byte[] src, int length)
        {
            byte[] dest = new byte[length];

             //m_data = new ushort[length >> 1];
            //fixed (byte* pdest = dest)
            {
                Win32Wrapper.memcpy(pdest, src, length);
                Array.Copy (src, dest, length);
            }
        }
 */
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="src">data in ushort array format</param>
        public S_DATA(ushort[] src)
        {
            m_data = new byte[src.Length * 2];

            for (int cnt = 0; cnt < src.Length; cnt++)
            {
                m_data[cnt * 2] = (byte)(src[cnt] >> 8);
                m_data[cnt * 2 + 1] = (byte)(src[cnt] & 0xff);
            }
        }

#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_data != null ? (short[])m_data.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_data != null ? CSLibrary.Tools.Hex.ToUshorts(m_data) : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_data;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.Hex.ToString(m_data);
        }
        /// <summary>
        /// Get Byte Data Length.
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_data != null ? m_data.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_DATA))
            {
                return Compare(m_data, ((S_DATA)item).ToBytes());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_data, (byte[])item);
            }
            return 0;
        }

        private  int Compare(byte[] b1, byte[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

//            fixed (byte* bp1 = b1)
            {
//                fixed (byte* bp2 = b2)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (b2[i] > b1[i])
                            return 1;

                        else if (b2[i] < b1[i])
                            return -1;
                    }
                    return 0;
                }
            }
        }

        public static implicit operator UInt16[](S_DATA p)
        {
            return p.ToUshorts();
        }

        public static implicit operator S_DATA(UInt16 [] p)
        {
            return new S_DATA(p);
        }

#endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IBANK : IComparable
    {
        /// <summary>
        /// Convert Value to ushort array
        /// </summary>
        ushort[] ToUshorts();
        /// <summary>
        /// Convert Value to short array
        /// </summary>
        short[] ToShorts();
        /// <summary>
        /// Convert Value to Byte array
        /// </summary>
        byte[] ToBytes();
        /// <summary>
        /// Convert Data to Hex String
        /// </summary>
        /// <returns></returns>
        string ToString();
        /// <summary>
        /// Get Data Length, Data in Ushort format
        /// </summary>
        /// <returns></returns>
        Int32 GetLength();
    }

    /// <summary>
    /// Custom Data Structure
    /// </summary>
    public struct S_MASK : IBANK
    {
        /// <summary>
        /// Protocol Control Value
        /// </summary>
        private byte[] m_mask;

#region constructor
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in string format</param>
        public S_MASK(string mask)
        {
            m_mask = CSLibrary.Tools.HexEncoding.ToBytes(mask);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in byte array format</param>
        public S_MASK(byte[] mask)
        {
            m_mask = (byte[])mask.Clone();
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in byte array format</param>
        public S_MASK(ushort[] mask)
        {
            m_mask = Tools.Hex.ToBytes(mask);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in byte array format</param>
        /// <param name="count">total byte length to copy</param>
        public S_MASK(byte[] mask, uint count)
        {
            m_mask = new byte[count];
            Array.Copy(mask, m_mask, (int)count);
        }
#endregion

#region extern function
        /// <summary>
        /// total byte length of mask
        /// </summary>
        public Int32 Length
        {
            get { return (m_mask == null || m_mask.Length == 0) ? 0 : m_mask.Length; }
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_mask != null ? (short[])m_mask.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_mask != null ? CSLibrary.Tools.HexEncoding.ToUshorts(m_mask) : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_mask;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.HexEncoding.ToString(m_mask);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_mask != null ? m_mask.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_DATA))
            {
                return Compare(m_mask, ((S_DATA)item).ToBytes());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_mask, (byte[])item);
            }
            return 0;
        }
        private int Compare(byte[] b1, byte[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;
            //fixed (byte* bp1 = b1)
            {
                //fixed (byte* bp2 = b2)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (b2[i] > b1[i])
                            return 1;
                        else if (b2[i] < b1[i])
                            return -1;
                    }
                    return 0;
                }
            }
        }
#endregion

#region implicit
        /// <summary>
        /// Convert to string
        /// </summary>
        public static implicit operator string(S_MASK mask)
        {
            return mask.ToString();
        }

        /// <summary>
        /// Convert mask to ushort[]
        /// </summary>
        public static implicit operator ushort[](S_MASK mask)
        {
            return mask.m_mask != null ? CSLibrary.Tools.HexEncoding.ToUshorts(mask.m_mask) : null;
        }

        /// <summary>
        /// Convert ushort[] to mask
        /// </summary>
        public static implicit operator S_MASK(ushort[] mask)
        {
            return new S_MASK(mask);
        }
#endregion
    }

    
#if oldcode
    /// <summary>
    /// Custom Data Structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct S_MASK : IBANK
    {
        /// <summary>
        /// Protocol Control Value
        /// </summary>
        private byte[] m_mask ;

#region constructor
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in string format</param>
        public S_MASK(string mask)
        {
            m_mask = CSLibrary.Tools.Hex.ToBytes(mask);
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in byte array format</param>
        public S_MASK(byte[] mask)
        {
            m_mask = (byte[])mask.Clone();
        }
        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="mask">mask in byte array format</param>
        /// <param name="count">total byte length to copy</param>
        public S_MASK(byte[] mask, uint count)
        {
            m_mask = new byte[count];
            Array.Copy(mask, m_mask, (int)count);
        }
#endregion

#region extern function
        /// <summary>
        /// total byte length of mask
        /// </summary>
        public Int32 Length
        {
            get { return (m_mask == null || m_mask.Length == 0) ? 0 : m_mask.Length; }
        }
#endregion

#region Interface
        /// <summary>
        /// Convert to short array
        /// </summary>
        /// <returns></returns>
        public short[] ToShorts()
        {
            return m_mask != null ? (short[])m_mask.Clone() : null;
        }
        /// <summary>
        /// Convert to ushort array
        /// </summary>
        /// <returns></returns>
        public ushort[] ToUshorts()
        {
            return m_mask != null ? CSLibrary.Tools.Hex.ToUshorts(m_mask) : null;
        }
        /// <summary>
        /// Convert to byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return m_mask;
        }
        /// <summary>
        /// Convert to HexString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CSLibrary.Tools.Hex.ToString(m_mask);
        }
        /// <summary>
        /// Get Data Length, Data in word format
        /// </summary>
        /// <returns></returns>
        public Int32 GetLength()
        {
            return m_mask != null ? m_mask.Length : 0;
        }
        /// <summary>
        /// Compare Data
        /// if equal return 0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 CompareTo(object item)
        {
            if (item.GetType() == typeof(S_DATA))
            {
                return Compare(m_mask, ((S_DATA)item).ToBytes());
            }
            else if (item.GetType() == typeof(ushort[]))
            {
                return Compare(m_mask, (byte[])item);
            }
            return 0;
        }
        private  int Compare(byte[] b1, byte[] b2)
        {
            int len = b1.Length > b2.Length ? b2.Length : b1.Length;

//            fixed (byte* bp1 = b1)
            {
//                fixed (byte* bp2 = b2)
                {
                    for (int i = 0; i < len; i++)
                    {
                        if (b2[i] > b1[i])
                            return 1;
                        else if (b2[i] < b1[i])
                            return -1;
                    }
                    return 0;
                }
            }
        }
#endregion
    }
#endif



#if NOUSE
#region Basic function Parms
    /// <summary>
    /// The ISO 18000-6C tag-read operation parameters
    /// </summary>
    public class TAG_READ_PARMS
    {
        /// <summary>
        /// The maximum number of times the write should be retried in the event 
        /// of write-verify failure(s).  In the event of write-verify failure(s), the write 
        /// will be retried either for the specified number of retries or until the data 
        /// written is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the written data, the write 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// </summary>
        public byte retryCount = 0;
        /// <summary>
        /// Starting offset
        /// </summary>
        public ushort offset = 0;
        /// <summary>
        /// Total number of words written to user memory
        /// </summary>
        public ushort count = 0;
        /// <summary>
        /// Target Memory Bank
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;
        /// <summary>
        /// Target Access Password
        /// </summary>
        public uint accessPassword = 0;
        /// <summary>
        /// Read data
        /// </summary>
        public S_DATA data = new S_DATA();
        /*/// <summary>
        /// Specifies which tag group will have subsequent tag-protocol 
        /// operations (e.g., inventory, tag read, etc.) applied to it.  The tag 
        /// group may not be changed while a radio module is executing a 
        /// tag-protocol operation. 
        /// </summary>
        public TagGroup TagGroup = new TagGroup();
        /// <summary>
        /// The singulation algorithm that is to be used for 
        /// subsequent tag-access operations.  If this 
        /// parameter does not represent a valid 
        /// singulation algorithm
        /// </summary>
        public SingulationAlgorithm Singulation = SingulationAlgorithm.UNKNOWN;
        /// <summary>
        /// Allows the application to configure the settings for a particular 
        /// singulation algorithm.
        /// </summary>
        public SingulationAlgorithmParms SingulationParms = new SingulationAlgorithmParms();
        /// <summary>
        /// Configures the tag-selection criteria for the ISO 18000-6C select 
        /// command.
        /// </summary>
        public SelectCriteria SelectCriteria = new SelectCriteria();
        /// <summary>
        /// Configures the post-singulation match criteria to be used by the 
        /// RFID radio module. 
        /// </summary>
        public SingulationCriteria PostMatchCriteria = new SingulationCriteria();*/
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.UNKNOWN;
    }
    /// <summary>
    /// The ISO 18000-6C tag-write operation parameters
    /// </summary>
    public class TAG_WRITE_PARMS
    {
        /*/// <summary>
        /// A flag that indicates if the data written to the tag should be read back 
        /// from the tag to verify that it was successfully written.  A non-zero value 
        /// indicates that the tag's memory should be read to verify that the data 
        /// was written successfully.  A zero value indicates that write verification is 
        /// not performed. 
        /// If this parameter is non-zero, verifyRetryCount indicates the 
        /// maximum number of times that the write/verify operation will be retried. 
        /// </summary>
        public int Verify;
        /// <summary>
        /// The maximum number of times the write should be retried in the event 
        /// of write-verify failure(s).  In the event of write-verify failure(s), the write 
        /// will be retried either for the specified number of retries or until the data 
        /// written is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the written data, the write 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored.
        /// </summary>
        public uint VerifyRetryCount;*/
        /// <summary>
        /// retry count for write
        /// Notes : max. value is 15
        /// </summary>
        public byte retryCount;
        /// <summary>
        /// Starting offset
        /// </summary>
        public ushort offset;
        /// <summary>
        /// Total number of words written to user memory
        /// </summary>
        public ushort count;
        /// <summary>
        /// Write Buffer data to target tag
        /// </summary>
        public S_DATA data = new S_DATA();
        /// <summary>
        /// Target Memory Bank
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;
        /// <summary>
        /// Target Access Password
        /// </summary>
        public uint accessPassword = 0;
        /*/// <summary>
        /// Specifies which tag group will have subsequent tag-protocol 
        /// operations (e.g., inventory, tag read, etc.) applied to it.  The tag 
        /// group may not be changed while a radio module is executing a 
        /// tag-protocol operation. 
        /// </summary>
        public TagGroup TagGroup = new TagGroup();
        /// <summary>
        /// The singulation algorithm that is to be used for 
        /// subsequent tag-access operations.  If this 
        /// parameter does not represent a valid 
        /// singulation algorithm
        /// </summary>
        public SingulationAlgorithm Singulation = SingulationAlgorithm.UNKNOWN;
        /// <summary>
        /// Allows the application to configure the settings for a particular 
        /// singulation algorithm.
        /// </summary>
        public SingulationAlgorithmParms SingulationParms = new SingulationAlgorithmParms();
        /// <summary>
        /// Configures the tag-selection criteria for the ISO 18000-6C select 
        /// command.
        /// </summary>
        public SelectCriteria SelectCriteria = new SelectCriteria();
        /// <summary>
        /// Configures the post-singulation match criteria to be used by the 
        /// RFID radio module. 
        /// </summary>
        public SingulationCriteria PostMatchCriteria = new SingulationCriteria();*/
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.UNKNOWN;
    }
    /// <summary>
    /// The ISO 18000-6C tag-block write operation parameters
    /// </summary>
    public class TAG_BLOCK_WRITE_PARMS
    {
        /// <summary>
        /// The maximum number of times the write should be retried in the event 
        /// of write-verify failure(s).  In the event of write-verify failure(s), the write 
        /// will be retried either for the specified number of retries or until the data 
        /// written is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the written data, the write 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored.
        /// </summary>
        public uint retryCount;
        /// <summary>
        /// Starting offset
        /// </summary>
        public ushort offset;
        /// <summary>
        /// Total number of words written to user memory
        /// </summary>
        public ushort count;
        /// <summary>
        /// Write Buffer data to target tag
        /// </summary>
        public S_DATA data = new S_DATA();
        /// <summary>
        /// Target Memory Bank
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;
        /// <summary>
        /// Target Access Password
        /// </summary>
        public uint accessPassword;
        /*/// <summary>
        /// Specifies which tag group will have subsequent tag-protocol 
        /// operations (e.g., inventory, tag read, etc.) applied to it.  The tag 
        /// group may not be changed while a radio module is executing a 
        /// tag-protocol operation. 
        /// </summary>
        public TagGroup TagGroup = new TagGroup();
        /// <summary>
        /// The singulation algorithm that is to be used for 
        /// subsequent tag-access operations.  If this 
        /// parameter does not represent a valid 
        /// singulation algorithm
        /// </summary>
        public SingulationAlgorithm Singulation = SingulationAlgorithm.UNKNOWN;
        /// <summary>
        /// Allows the application to configure the settings for a particular 
        /// singulation algorithm.
        /// </summary>
        public SingulationAlgorithmParms SingulationParms = new SingulationAlgorithmParms();
        /// <summary>
        /// Configures the tag-selection criteria for the ISO 18000-6C select 
        /// command.
        /// </summary>
        public SelectCriteria SelectCriteria = new SelectCriteria();
        /// <summary>
        /// Configures the post-singulation match criteria to be used by the 
        /// RFID radio module. 
        /// </summary>
        public SingulationCriteria PostMatchCriteria = new SingulationCriteria();*/
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.UNKNOWN;
    }
#endregion
#endif

#region Custom function Parms

#region Custom Search
    /// <summary>
    /// The ISO 18000-6C tag-inventory operation parameters
    /// </summary>
    public class TagInventoryParms
    {
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.UNKNOWN;
        /// <summary>
        /// <para>The maximum number of tags to which the tag-protocol operation will be </para>
        /// <para>applied.  If this number is zero, then the operation is applied to all </para>
        /// <para>tags that match the selection, and optionally post-singulation, match  </para>
        /// <para>criteria.  If this number is non-zero, the antenna-port dwell-time and </para>
        /// <para>inventory-round-count constraints still apply, however the operation   </para>
        /// <para>will be prematurely terminated if the maximum number of tags have the  </para>
        /// <para>tag-protocol operation applied to them.                                </para>
        /// </summary>
        public ushort tagStopCount = 0;
        /// <summary>
        /// number of bank to read after inventory
        /// </summary>
        public uint multibanks;
        /// <summary>
        /// Memory bank 1
        /// </summary>
        public MemoryBank bank1;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to read.
        /// </summary>
        public UInt16 offset1;
        /// <summary>
        /// The number of 16-bit words that will be read. This field must be
        /// between 1 and 31, inclusive.                        
        /// </summary>          
        public UInt16 count1;
        /// <summary>
        /// Memory bank 2
        /// </summary>
        public MemoryBank bank2;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to read.
        /// </summary>
        public UInt16 offset2;
        /// <summary>
        /// The number of 16-bit words that will be read. This field must be
        /// between 1 and 31, inclusive.                        
        /// </summary>          
        public UInt16 count2;
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;

    }
    /// <summary>
    /// this parms is same as inventory parms
    /// </summary>
    public class TagRangingParms
    {
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
        /// <summary>
        /// <para>The maximum number of tags to which the tag-protocol operation will be </para>
        /// <para>applied.  If this number is zero, then the operation is applied to all </para>
        /// <para>tags that match the selection, and optionally post-singulation, match  </para>
        /// <para>criteria.  If this number is non-zero, the antenna-port dwell-time and </para>
        /// <para>inventory-round-count constraints still apply, however the operation   </para>
        /// <para>will be prematurely terminated if the maximum number of tags have the  </para>
        /// <para>tag-protocol operation applied to them.                                </para>
        /// </summary>
        public ushort tagStopCount = 0;
        /// <summary>
        /// number of bank to read after inventory
        /// </summary>
        public uint multibanks;
        internal uint multibankswithreply; // special for kiloway tag
        /// <summary>
        /// Memory bank 1
        /// </summary>
        public MemoryBank bank1;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to read.
        /// </summary>
        public UInt16 offset1;
        /// <summary>
        /// The number of 16-bit words that will be read. This field must be
        /// between 1 and 31, inclusive.                        
        /// </summary>          
        public UInt16 count1;
        /// <summary>
        /// Memory bank 2
        /// </summary>
        public MemoryBank bank2;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to read.
        /// </summary>
        public UInt16 offset2;
        /// <summary>
        /// The number of 16-bit words that will be read. This field must be
        /// between 1 and 31, inclusive.                        
        /// </summary>          
        public UInt16 count2;
        /// <summary>
        /// Memory bank 3
        /// </summary>
        public MemoryBank bank3;
        /// <summary>
        /// The offset, in the memory bank, of the first 16-bit word to read.
        /// </summary>
        public UInt16 offset3;
        /// <summary>
        /// The number of 16-bit words that will be read. This field must be
        /// between 1 and 31, inclusive.                        
        /// </summary>          
        public UInt16 count3;
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword;
        /// <summary>
        /// Inventory Tag on QT Mode
        /// </summary>
        public bool QTMode = false;
        /// <summary>
        /// compact inventory mode.
        /// </summary>
        public bool compactmode = false;
        /// <summary>
        /// focus mode.
        /// </summary>
        public bool focus = false;
        /// <summary>
        /// fast id mode.
        /// </summary>
        public bool fastid = false;

        public TagRangingParms Clone()
        {
            return (TagRangingParms)this.MemberwiseClone();//boxing into object
        }
    }
    /// <summary>
    /// search one tag parms
    /// </summary>
    public class TagSearchingParms
    {
        /// <summary>
        /// averaging the RSSI value during search
        /// </summary>
        public bool avgRssi = true;
    }
#endregion

#endregion

    /// <summary>
    /// Carrier wave parms
    /// </summary>
    public class CarrierWaveParms
    {
        /// <summary>
        /// data mode enable
        /// </summary>
        public bool dataMode = false;
    }

    public enum SENREP
    {
        STORE,
        SEND,
    }

    public enum INCREPLEN
    {
        OMIT,
        INCLUDE,
    }

    /// <summary>
    /// Untraceable parms
    /// </summary>
    public class AuthenticateParms
    {
        public UInt32 password = 0;
        public SENREP SenRep = SENREP.STORE;
        public INCREPLEN IncRepLen = INCREPLEN.OMIT;
        public UInt16 CSI = 0x00;
        public UInt16 Length = 0;
        public string Message;
        public UInt16 ResponseLen = 0;
        public S_DATA pData;
    }

    /// <summary>
    /// Untraceable parms
    /// </summary>
    public class ReadBufferParms
    {
        public UInt32 Offset = 0;
        public UInt16 Length = 0;
    }

    public enum UNTRACEABLE_RANGE
    {
        Normal = 0,
        Toggle,
        Reduced,
    }

    public enum UNTRACEABLE_USER
    {
        View,
        Hide,
    }

    public enum UNTRACEABLE_TID
    {
        HideNone,
        HideSome,
        HideAll,
    }

    public enum UNTRACEABLE_EPC
    {
        Show,
        Hide,
    }

    public enum UNTRACEABLE_U
    {
        DeassertU,
        AssertU,
    }


    /// <summary>
    /// Untraceable parms
    /// </summary>
    public class UntraceableParms
    {
        public UNTRACEABLE_RANGE Range = UNTRACEABLE_RANGE.Normal;
        public UNTRACEABLE_USER User = UNTRACEABLE_USER.View;
        public UNTRACEABLE_TID TID = UNTRACEABLE_TID.HideNone;
        public UNTRACEABLE_EPC EPC = UNTRACEABLE_EPC.Show;
        public uint EPCLength = 0;
        public UNTRACEABLE_U U = UNTRACEABLE_U.DeassertU;
    }

    /// <summary>
    /// offset = Starting address
    /// count = Read size (byte)
    /// data = Result
    /// </summary>
    public class FM13DTReadMemoryParms
    {
        public uint offset;
        public uint count;
        public byte [] data;
    }

    /// <summary>
    /// offset = Starting address
    /// count = Write size (byte)
    /// data = Data
    /// </summary>
    public class FM13DTWriteMemoryParms
    {
        public uint offset;
        public uint count;
        public byte [] data;
    }

    public class FM13DTDeepSleepParms
    {
        public bool enable;
    }

    public class FM13DTOpModeChkParms
    {
        public bool enable;
        public bool user_access_en;
        public bool RTC_logging;
        public bool vdet_process_flag;
        public bool light_chk_flag;
        public bool vbat_pwr_flag;
    }

    public class FM13DTLedCtrlParms
    {
        public bool enable;
    }

    /// <summary>
    /// Operation Paramemter
    /// </summary>
    public class CSLibraryOperationParms
    {
        /// <summary>
        /// Config this before search
        /// </summary>
        //public TagInventoryParms TagInventory = new TagInventoryParms();
        /*/// <summary>
        /// Config this before read
        /// </summary>
        public TAG_READ_PARMS TagRead = new TAG_READ_PARMS();
        /// <summary>
        /// Config this before write
        /// </summary>
        public TAG_WRITE_PARMS TagWrite = new TAG_WRITE_PARMS();*/
        /// <summary>
        /// Config this before kill
        /// </summary>
        public TagKillParms TagKill = new TagKillParms();
        /// <summary>
        /// Config this before lock
        /// </summary>
        public TagLockParms TagLock = new TagLockParms();
        /// <summary>
        /// User Bank Perm-Lock
        /// </summary>
        public TagBlockPermalockParms TagBlockLock = new TagBlockPermalockParms();
        /*/// <summary>
        /// Config this before block write
        /// </summary>
        public TAG_BLOCK_WRITE_PARMS TagBlockWrite = new TAG_BLOCK_WRITE_PARMS();*/
        /// <summary>
        /// Config this before read EPC
        /// </summary>
        public TagReadEpcParms TagReadEPC = new TagReadEpcParms();


        //        public CSLibrary.Structures.clm

        /// <summary>
        /// Config this before read data
        /// </summary>
        public TagReadParms TagRead = new TagReadParms();
        /// <summary>
        /// Config this before read PC
        /// </summary>
        public TagReadPcParms TagReadPC = new TagReadPcParms();
        /// <summary>
        /// Config this before read Access password
        /// </summary>
        public TagReadPwdParms TagReadAccPwd = new TagReadPwdParms();
        /// <summary>
        /// Config this before read Kill password
        /// </summary>
        public TagReadPwdParms TagReadKillPwd = new TagReadPwdParms();
        /// <summary>
        /// Config this before read TID
        /// </summary>
        public TagReadTidParms TagReadTid = new TagReadTidParms();
        /// <summary>
        /// Config this before read USER
        /// </summary>
        public TagReadUserParms TagReadUser = new TagReadUserParms();
        /// <summary>
        /// Config this before write data
        /// </summary>
        public TagWriteParms TagWrite = new TagWriteParms();
        /// <summary>
        /// Config this before write kill password
        /// </summary>
        public TagWritePwdParms TagWriteKillPwd = new TagWritePwdParms();
        /// <summary>
        /// Config this before write access password
        /// </summary>
        public TagWritePwdParms TagWriteAccPwd = new TagWritePwdParms();
        /// <summary>
        /// Config this before write PC
        /// </summary>
        public TagWritePcParms TagWritePC = new TagWritePcParms();
        /// <summary>
        /// Config this before write EPC
        /// </summary>
        public TagWriteEpcParms TagWriteEPC = new TagWriteEpcParms();
        /// <summary>
        /// Config this before write USER
        /// </summary>
        public TagWriteUserParms TagWriteUser = new TagWriteUserParms();
        /// <summary>
        /// Config this before block write
        /// </summary>
        public TAG_BLOCK_WRITE_PARMS TagBlockWrite = new TAG_BLOCK_WRITE_PARMS();
        /// <summary>
        /// Config this before ranging all tags
        /// </summary>
        public TagRangingParms TagRanging = new TagRangingParms();
        /*/// <summary>
        /// Config this before search all tags
        /// </summary>
        public CUST_SEARCH_ANY_PARMS TagSearchAny = new CUST_SEARCH_ANY_PARMS();*/
        /// <summary>
        /// Config this before search one tag
        /// </summary>
        public TagSearchingParms TagSearchOne = new TagSearchingParms();
        /// <summary>
        /// Selected a tag for read, write, lock, kill, and search one operation
        /// </summary>
        public TagSelectedParms TagSelected = new TagSelectedParms();
        /// <summary>
        /// Set Mask
        /// </summary>
        public TagGeneralSelectedParms TagGeneralSelected = new TagGeneralSelectedParms();
        /// <summary>
        /// Selected a tag for read, write, lock, kill, and search one operation
        /// </summary>
        public TagPostMachParms TagPostMatch = new TagPostMachParms();
        /// <summary>
        /// Custom command TagReadProtect
        /// </summary>
        public TagReadProtectParms TagReadProtect = new TagReadProtectParms();
        /// <summary>
        /// Custom command TagResetReadProtect
        /// </summary>
        public TagReadProtectParms TagResetReadProtect = new TagReadProtectParms();
        /// <summary>
        /// Custom command for internal use.
        /// </summary>
        public CarrierWaveParms CarrierWave = new CarrierWaveParms();
        /// <summary>
        /// Untraceable command
        /// </summary>
        public AuthenticateParms TagAuthenticate = new AuthenticateParms();
        /// <summary>
        /// Untraceable command
        /// </summary>
        public ReadBufferParms TagReadBuffer = new ReadBufferParms();
        /// <summary>
        /// Untraceable command
        /// </summary>
        public UntraceableParms TagUntraceable = new UntraceableParms();

        public QTCommandParms QTCommand = new QTCommandParms();

        public FM13DTReadMemoryParms FM13DTReadMemory = new FM13DTReadMemoryParms();

        public FM13DTWriteMemoryParms FM13DTWriteMemory = new FM13DTWriteMemoryParms();

        public FM13DTDeepSleepParms FM13DTDeepSleep = new FM13DTDeepSleepParms();

        public FM13DTOpModeChkParms FM13DTOpModeChk = new FM13DTOpModeChkParms();

        public FM13DTLedCtrlParms FM13DTLedCtrl = new FM13DTLedCtrlParms();



#if old
		/// <summary>
		/// Tag Bank 1 Filter
		/// </summary>        
		public BNK1_MSK_FILTER_CFG TagFilter = new BNK1_MSK_FILTER_CFG();

        public CLSetPasswordParms CLSetPassword = new CLSetPasswordParms();

        public CLSetLogModeParms CLSetLogMode = new CLSetLogModeParms();

        public CLSetLogLimitsParms CLSetLogLimits = new CLSetLogLimitsParms();

        public CLGetMesurementSetupParms CLGetMesurementSetup = new CLGetMesurementSetupParms();

        public CLSetSFEParaParms CLSetSFEPara = new CLSetSFEParaParms();

        public CLSetCalDataParms CLSetCalData = new CLSetCalDataParms();

        public CLStartLogParms CLStartLog = new CLStartLogParms();

        public CLGetLogStateParms CLGetLogState = new CLGetLogStateParms();

        public CLGetCalDataParms CLGetCalData = new CLGetCalDataParms();

        public CLGetBatLvParms CLGetBatLv = new CLGetBatLvParms();

        public CLSetShelfLifeParms CLSetShelfLife = new CLSetShelfLifeParms();

        public CLInitParms CLInit = new CLInitParms();

        public CLGetSensorValueParms CLGetSensorValue = new CLGetSensorValueParms();

        public CLOpenAreaParms CLOpenArea = new CLOpenAreaParms();

        public CLAccessFifoParms CLAccessFifo = new CLAccessFifoParms();

        public G2ConfigParms G2Config = new G2ConfigParms();


#endif
    }

    #region Custom function Parms

    #region rfid parms
    /*
    public class rfid_inventory_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public uint tagStopCount = 0;
        /// <summary>
        /// A radio module may operate either in continuous or non-continuous mode.  In 
        /// continuous mode, when a tag-protocol-operation cycle (i.e., one iteration through 
        /// all enabled antenna ports) has completed, the radio module will begin a new tag-
        /// protocol-operation cycle with the first enabled antenna port and will continue to 
        /// do so until the operation is explicitly cancelled by the application.  In non-
        /// continuous mode, only a single tag-protocol-operation cycle is executed upon the 
        /// radio module. 
        /// </summary>
        public bool continuous = false;
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_read_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public UInt32 tagStopCount = 0;
        /// <summary>
        /// The memory bank from which to read.  Valid values are: 
        /// MemoryBank.RESERVED 
        /// MemoryBank.EPC  
        /// MemoryBank.TID 
        /// MemoryBank.USER 
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;
        /// <summary>
        /// The offset of the first 16-bit word, where zero is the first 16-bit 
        /// word in the memory bank, to read from the specified memory 
        /// bank. 
        /// </summary>
        public UInt16 offset = 0;
        /// <summary>
        /// The number of 16-bit words to read.  If this value is zero and 
        /// bank is MemoryBank.EPC, the read will return the 
        /// contents of the tag's EPC memory starting at the 16-bit word 
        /// specified by offset through the end of the EPC.  If this value is 
        /// zero and bank is not MemoryBank.EPC, the read 
        /// will return, for the tag's chosen memory bank, data starting from 
        /// the 16-bit word specified by offset to the end of the memory 
        /// bank.  If this value is non-zero, it must be in the range 1 to 255, 
        /// inclusive. 
        /// </summary>
        public UInt16 count = 0;
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no 
        /// access password. 
        /// </summary>
        public UInt32 accessPassword = 0;
        /// <summary>
        /// The maximum number of times the read should be retried in the event 
        /// of read-verify failure(s).  In the event of read-verify failure(s), the read 
        /// will be retried either for the specified number of retries or until the data 
        /// read is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the read data, the read 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored. 
        /// </summary>
        public UInt32 verifyRetryCount = 0;
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_write_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public UInt32 tagStopCount = 0;
        /// <summary>
        /// The memory bank from which to write.  Valid values are: 
        /// MemoryBank.RESERVED 
        /// MemoryBank.EPC  
        /// MemoryBank.TID 
        /// MemoryBank.USER 
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;
        /// <summary>
        /// The number of 16-bit words to be written to the tag's specified 
        /// memory bank.  For version 1.1 of the RFID Reader Library, this 
        /// parameter must contain a value between 1 and 8, inclusive. 
        /// </summary>
        public UInt16 count = 0;
        /// <summary>
        /// Use for sequential write
        /// The offset of the first 16-bit word, where zero is the first 16-bit 
        /// word in the memory bank, to write in the specified memory bank. 
        /// </summary>
        public UInt16 offset = 0;
        /// <summary>
        /// Use for random write
        /// An array of count 16-bit values that specify 16-bit tag-
        /// memory-bank offsets, with zero being the first 16-bit word in the 
        /// memory bank, where the corresponding 16-bit words in the pData 
        /// array will be written.  i.e., the 16-bit word in pData[n] will be written to 
        /// the 16-bit tag-memory-bank offset contained in pOffset[n].  This 
        /// field must not be NULL. 
        /// </summary>
        public UInt16[] pOffset = new UInt16[0];
        /// <summary>
        /// A buffer of count 16-bit values to be written 
        /// sequentially to the tag's specified memory bank.  The high-order 
        /// byte of pData[n] will be written to the tag's memory-bank byte at 
        /// 16-bit offset (offset+n).  The low-order byte will be written to the 
        /// next byte.  For example, if offset is 2 and pData[0] is 0x1122, 
        /// then the tag-memory byte at 16-bit offset 2 (byte offset 4) will have 
        /// 0x11 written to it and the next byte (byte offset 5) will have 0x22 
        /// written to it.  This field must not be NULL. 
        /// </summary>
        public UInt16[] pData = new UInt16[0];
        /// <summary>
        /// The type of write that will be performed – i.e., sequential or random.  
        /// The value of this field determines which of the structures within 
        /// parameters contains the write parameters.  
        /// </summary>
        public WriteType writeType = WriteType.UNKNOWN;
        /// <summary>
        /// A flag that indicates if the data written to the tag should be read back 
        /// from the tag to verify that it was successfully written.  A non-zero value 
        /// indicates that the tag's memory should be read to verify that the data 
        /// was written successfully.  A zero value indicates that write verification is 
        /// not performed. 
        /// If this parameter is non-zero, verifyRetryCount indicates the 
        /// maximum number of times that the write/verify operation will be retried. 
        /// </summary>
        public Int32 verify = 0;
        /// <summary>
        /// The maximum number of times the write should be retried in the event 
        /// of write-verify failure(s).  In the event of write-verify failure(s), the write 
        /// will be retried either for the specified number of retries or until the data 
        /// written is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the written data, the write 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored. 
        /// </summary>
        public UInt32 verifyRetryCount = 0;
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no access 
        /// password. 
        /// </summary>
        public UInt32 accessPassword = 0;
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_block_write_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public UInt32 tagStopCount = 0;
        /// <summary>
        /// A flag that indicates if the data written to the tag should be read back from 
        /// the tag to verify that it was successfully written.  A non-zero value 
        /// indicates that the tag's memory should be read to verify that the data was 
        /// written successfully.  A zero value indicates that write verification is not 
        /// performed. 
        /// If this parameter is non-zero, verifyRetryCount indicates the 
        /// maximum number of times that the write/verify operation will be retried. 
        /// </summary>
        public Int32 verify = 0;                    //4
        /// <summary>
        ///  The maximum number of times the write should be retried in the event of 
        /// write-verify failure(s).  In the event of write-verify failure(s), the write will 
        /// be retried either for the specified number of retries or until the data written 
        /// is successfully verified.  If the specified number of retries are performed 
        /// without successfully verifying the written data, the write operation is 
        /// considered to have failed and the tag-access operation-response packet 
        /// will indicate the error.  This value must be between 0 and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored. 
        /// </summary>
        public UInt32 verifyRetryCount = 0;                    //4
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no access 
        /// password. 
        /// </summary>
        public UInt32 accessPassword = 0;                    //4
        /// <summary>
        /// The memory bank from which to write.  Valid values are: 
        /// MemoryBank.RESERVED 
        /// MemoryBank.EPC  
        /// MemoryBank.TID 
        /// MemoryBank.USER 
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;   //4
        /// <summary>
        /// The number of 16-bit words to be written to the tag's specified memory 
        /// bank.  For version 1.2 of the RFID Reader Library, this parameter must 
        /// contain a value between 1 and 255, inclusive. 
        /// </summary>
        public UInt16 count = 0;                    //4
        /// <summary>
        /// A buffer of count 16-bit values to be written sequentially to 
        /// the tag's specified memory bank.  This field must not be NULL. 
        /// </summary>
        public UInt16[] pData = new UInt16[0];                  //IntPtr.Size
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_block_erase_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public UInt32 tagStopCount = 0;
        /// <summary>
        /// A flag that indicates if the data written to the tag should be read back from 
        /// the tag to verify that it was successfully written.  A non-zero value 
        /// indicates that the tag's memory should be read to verify that the data was 
        /// written successfully.  A zero value indicates that write verification is not 
        /// performed. 
        /// If this parameter is non-zero, verifyRetryCount indicates the 
        /// maximum number of times that the write/verify operation will be retried. 
        /// </summary>
        public Int32 verify = 0;                    //4
        /// <summary>
        ///  The maximum number of times the write should be retried in the event of 
        /// write-verify failure(s).  In the event of write-verify failure(s), the write will 
        /// be retried either for the specified number of retries or until the data written 
        /// is successfully verified.  If the specified number of retries are performed 
        /// without successfully verifying the written data, the write operation is 
        /// considered to have failed and the tag-access operation-response packet 
        /// will indicate the error.  This value must be between 0 and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored. 
        /// </summary>
        public UInt32 verifyRetryCount = 0;                    //4
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no access 
        /// password. 
        /// </summary>
        public UInt32 accessPassword = 0;                    //4
        /// <summary>
        /// The memory bank from which to write.  Valid values are: 
        /// MemoryBank.RESERVED 
        /// MemoryBank.EPC  
        /// MemoryBank.TID 
        /// MemoryBank.USER 
        /// </summary>
        public MemoryBank bank = MemoryBank.UNKNOWN;   //4
        /// <summary>
        /// The number of 16-bit words to be written to the tag's specified memory 
        /// bank.  For version 1.2 of the RFID Reader Library, this parameter must 
        /// contain a value between 1 and 255, inclusive. 
        /// </summary>
        public UInt16 count = 0;                    //4
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_kill_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public UInt32 tagStopCount = 0;
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no access 
        /// password. 
        /// </summary>
        public UInt32 accessPassword = 0;
        /// <summary>
        /// The kill password for the tags. 
        /// </summary>
        public UInt32 killPassword = 0;
        /// <summary>
        /// The maximum number of times the kill should be retried in the event 
        /// of kill-verify failure(s).  In the event of kill-verify failure(s), the kill 
        /// will be retried either for the specified number of retries or until the data 
        /// kill is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the kill data, the kill 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored. 
        /// </summary>
        public UInt32 verifyRetryCount = 0;
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_lock_parms : rfid_parms
    {
        /// <summary>
        /// The maximum number of tags to which the tag-protocol operation 
        /// will be applied.  If this number is zero, then the operation is applied 
        /// to all tags that match the selection, and optionally post-singulation, 
        /// match criteria (within the constraints of the antenna-port dwell time 
        /// and inventory cycle count – see ).  If this number is non-zero, the 
        /// antenna-port dwell-time and inventory-cycle-count constraints still 
        /// apply, however the operation will be prematurely terminated if the 
        /// maximum number of tags have the tag-protocol operation applied to 
        /// them.  For version 1.0, this field may have a maximum value 1. 
        /// </summary>
        public UInt32 tagStopCount = 0;
        /// <summary>
        ///  A structure that contains the access permissions to be set for the tag. 
        /// </summary>
        public TagPerm permissions = new TagPerm();
        /// <summary>
        /// The access password for the tags.  A value of zero indicates no access 
        /// password. 
        /// </summary>
        public UInt32 accessPassword = 0;
        /// <summary>
        /// The maximum number of times the lock should be retried in the event 
        /// of lock-verify failure(s).  In the event of lock-verify failure(s), the lock 
        /// will be retried either for the specified number of retries or until the data 
        /// lock is successfully verified.  If the specified number of retries are 
        /// performed without successfully verifying the lock data, the lock 
        /// operation is considered to have failed and the tag-access operation-
        /// response packet will indicate the error.  This value must be between 0 
        /// and 7, inclusive. 
        /// If verify is non-zero, this parameter is ignored. 
        /// </summary>
        public UInt32 verifyRetryCount = 0;
        /// <summary>
        /// Flag - Zero or combination of  Select or Post-Match
        /// </summary>
        public SelectFlags flags = SelectFlags.ZERO;
    }
    public class rfid_search_parms : rfid_parms
    {
        public bool AveragingRssi = false;
        public bool continuous = false;
        public bool bUseSelect = true;
        public SearchFlags searchFlags = SearchFlags.EPC_ONLY;
        public S_EPC epc = new S_EPC();
        public S_PC pc = new S_PC();
    }
    public class rfid_parms
    {
        public rfid_parms() { }
    }*/

    #endregion


    #endregion
    /// <summary>
    /// Frequency Band Parms
    /// </summary>
    public struct FrequencyBandParms
    {
        /// <summary>
        /// AffinityBand
        /// </summary>
        public ushort AffinityBand;
        /// <summary>
        /// Frequency Band
        /// </summary>
        public UInt32 Band;
        /// <summary>
        /// DivideRation
        /// </summary>
        public UInt16 DivideRatio;
        /// <summary>
        /// GuardBand
        /// </summary>
        public UInt16 GuardBand;
        /// <summary>
        /// MaximumDACBand
        /// </summary>
        public UInt16 MaximumDACBand;
        /// <summary>
        /// MinimumDACBand
        /// </summary>
        public UInt16 MinimumDACBand;
        /// <summary>
        /// MultiplyRatio
        /// </summary>
        public UInt16 MultiplyRatio;
        /// <summary>
        /// State
        /// </summary>
        public BandState State;
        /// <summary>
        /// Frequency
        /// </summary>
        public double Frequency;
    }
    /// <summary>
    /// Temperature Parms
    /// </summary>
    public struct TemperatureParms
    {
        /// <summary>
        /// Ambient Temperature
        /// </summary>
        public uint amb;
        /// <summary>
        /// Transciever Temperature
        /// </summary>
        public uint xcvr;
        /// <summary>
        /// Power Amp Temperature
        /// </summary>
        public uint pwramp;
    }
    /// <summary>
    /// ThresholdTemperatureParms
    /// </summary>
    public struct ThresholdTemperatureParms
    {
        /// <summary>
        /// Ambient Temperature
        /// </summary>
        public uint amb;
        /// <summary>
        /// Transciever Temperature
        /// </summary>
        public uint xcvr;
        /// <summary>
        /// Power Amp Temperature
        /// </summary>
        public uint pwramp;
        /// <summary>
        /// pa-delta
        /// </summary>
        public uint delta;
    }

    /// <summary>
    /// QT Mode
    /// </summary>
    public enum QTMode
    {
        /// <summary>
        /// Permanent Public
        /// </summary>
        PermPublic,
        /// <summary>
        /// Temporary Public
        /// </summary>
        TempPublic,
        /// <summary>
        /// Permanent Private
        /// </summary>
        PermPrivate,
        /// <summary>
        /// Temporary Private
        /// </summary>
        TempPrivate,
    }

    /// <summary>
    /// RFID Device Status Structures
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DEVICE_STATUS
    {
        /// <summary>
        /// Whether RFID is on.
        /// </summary>
        public bool IsPowerOn;
        /// <summary>
        /// Whether AutoReset is on.
        /// </summary>
        public bool IsErrorReset;
        /// <summary>
        /// Whether UDP keep alive is on.
        /// </summary>
        public bool IsKeepAlive;
        /// <summary>
        /// Whether connection is connected or listening.
        /// </summary>
        public bool IsConnected;
        public byte day;
        public byte hrs;
        public byte min;
        public byte sec;
        /// <summary>
        /// Crc filter flag
        /// </summary>
        public bool IsCRCFilter;

        /// <summary>
        /// Get last elapsed time since last received packet.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetElapsedTime()
        {
            return new TimeSpan(day, hrs, min, sec);
        }
    }

    /*/// <summary>
    /// Mac Error
    /// </summary>
    public struct MacErrorParms
    {
        /// <summary>
        /// Error Code in integer
        /// </summary>
        public ushort ErrorNumber;
        /// <summary>
        /// Error Code Name
        /// </summary>
        public string Name;
        /// <summary>
        /// Description of Error Code
        /// </summary>
        public string Description;
    }*/
    /*
    class TAG_AVERAGE_RSSI
    {
        public Queue queue = new Queue(50);
        public uint lastTime = 0;
        public uint avgNbRssi = 0; // running average of NbRssi Reg value
        public uint totalNbRssi = 0;
        public uint numRds = 0; // denominator of the average Rssi
    } ;*/
}
