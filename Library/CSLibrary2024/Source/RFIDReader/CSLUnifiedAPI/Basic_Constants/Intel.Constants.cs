/*******************************************************************************
 *  INTEL CONFIDENTIAL
 *  Copyright 2007 Intel Corporation All Rights Reserved.
 *
 *  The source code contained or described herein and all documents related to
 *  the source code ("Material") are owned by Intel Corporation or its suppliers
 *  or licensors. Title to the Material remains with Intel Corporation or its
 *  suppliers and licensors. The Material may contain trade secrets and
 *  proprietary and confidential information of Intel Corporation and its
 *  suppliers and licensors, and is protected by worldwide copyright and trade
 *  secret laws and treaty provisions. No part of the Material may be used,
 *  copied, reproduced, modified, published, uploaded, posted, transmitted,
 *  distributed, or disclosed in any way without Intel's prior express written
 *  permission. 
 *  
 *  No license under any patent, copyright, trade secret or other intellectual
 *  property right is granted to or conferred upon you by disclosure or delivery
 *  of the Materials, either expressly, by implication, inducement, estoppel or
 *  otherwise. Any license under such intellectual property rights must be
 *  express and approved by Intel in writing.
 *
 *  Unless otherwise agreed by Intel in writing, you may not remove or alter
 *  this notice or any other notice embedded in Materials by Intel or Intel's
 *  suppliers or licensors in any way.
 ******************************************************************************/

/******************************************************************************
 *
 * Description:
 *   This is the RFID Library header file that specifies the enumeration
 *   and various other constants.
 *
 ******************************************************************************/


using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace CSLibrary
{

    namespace Constants
    {
#if nouse
        /// <summary>
        /// For internal function RFID_Startup use
        /// </summary>
        [Flags]
        public enum LibraryMode : uint
        {
            /// <summary>
            /// Normal mode
            /// </summary>
            DEFAULT   = 0x0,
            /// <summary>
            ///  Places the RFID Reader Library in emulation mode.  
            ///  In emulation mode, the RFID Reader Library emulates a 
            ///  simplified radio operational mode. 
            /// </summary>
            EMULATION = 0x1,
            /// <summary>
            /// TCP Debug mode for engineering test
            /// </summary>
            DEBUG_TCP = 0x10000000,
            /// <summary>
            /// Debug mode for engineering test
            /// </summary>
            DEBUG_TRACE = 0x40000000,
            /// <summary>
            /// Debug mode for engineering test
            /// </summary>
            DEBUG_FILE = 0x80000000,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN   = 0xFFFF
        };

         
        /// <summary>
        /// For internal function RFID_RadioOpen use
        /// </summary>
        public enum MacMode : uint
        {
            /// <summary>
            /// Default mode
            /// </summary>
            DEFAULT   = 0x0,
            //// <summary>
            /// Places the RFID radio module's MAC firmware in emulation 
            /// mode.  In emulation mode, the RFID radio module's MAC 
            ///firmware simulates all radio responses. 
            /// </summary>
            EMULATION = 0x1,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN   = 0xFFFF
        };
#endif

        /// <summary>
        /// A radio module may operate either in continuous or non-continuous mode.  In 
        /// continuous mode, when a tag-protocol-operation cycle (i.e., one iteration through 
        /// all enabled antenna ports) has completed, the radio module will begin a new tag-
        /// protocol-operation cycle with the first enabled antenna port and will continue to 
        /// do so until the operation is explicitly cancelled by the application.  In non-
        /// continuous mode, only a single tag-protocol-operation cycle is executed upon the 
        /// radio module. 
        /// </summary>
        public enum RadioOperationMode : uint
        {
            /// <summary>
            /// Continuous operation
            /// </summary>
            CONTINUOUS,
            /// <summary>
            /// Non-continuous
            /// </summary>
            NONCONTINUOUS,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Based upon the usage scenario, an application is given the flexibility to place the 
        /// radio module into specific power states.  A radio module that is in the low-power 
        /// state will remain so until either a tag-protocol operation (e.g., 
        /// TagInventory, etc.) is executed on the radio module or until it is 
        /// explicitly instructed to leave low-power state.  Note that if an RFID radio module 
        /// is brought out of low-power state by the execution of a tag-protocol operation, it 
        /// will not automatically return to low-power state. 
        /// Note  that  the  radio-module  power  state  should  not  be  confused  with  the  per-
        /// antenna RF power-level
        /// </summary>
        public enum RadioPowerState : uint
        {
            /// <summary>
            /// Full power mode
            /// </summary>
            FULL,
            /// <summary>
            /// Standby mode
            /// </summary>
            STANDBY,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Indicates the state of the logical antenna port. 
        /// </summary>
        public enum AntennaPortState : uint
        {
            /// <summary>
            /// Disable Port
            /// </summary>
            DISABLED,
            /// <summary>
            /// Enable Port
            /// </summary>
            ENABLED,
            /// <summary>
            /// Unknown action
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        
        /// <summary>
        /// The modulation type that is used by the profile. 
        /// </summary>
        public enum ModulationType : uint
        {
            /// <summary>
            /// DSB_ASK Modulation
            /// </summary>
            DSB_ASK,
            /// <summary>
            /// SSB_ASK Modulation
            /// </summary>
            SSB_ASK,
            /// <summary>
            /// PR_ASK Modulation
            /// </summary>
            PR_ASK,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The difference, in Tari, between an data zero and a data one. 
        /// </summary>
        public enum DataDifference : uint
        {
            /// <summary>
            /// HALF_TARI
            /// </summary>
            HALF_TARI,
            /// <summary>
            /// ONE_TARI
            /// </summary>
            ONE_TARI,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The tag-to-interrogator divide ratio that is sent as part of the 
        /// Query command.
        /// </summary>
        public enum DivideRatio : uint
        {
            /// <summary>
            /// 
            /// </summary>
            RATIO_8,
            /// <summary>
            /// 
            /// </summary>
            RATIO_64DIV3,
            /// <summary>
            /// 
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The miller number (i.e., cycles per symbol) that is sent as 
        /// part of the Query command. 
        /// </summary>
        public enum MillerNumber : uint
        {
            /// <summary>
            /// A binary '0' has a transition in the middle of a symbol, whereas a binary '1' does not.
            /// </summary>
            NUMBER_FM0,
            /// <summary>
            /// The FM0 signal is multiplied by a square wave with either 2 periods for each FM0 symbol. 
            /// </summary>
            NUMBER_2,
            /// <summary>
            /// The FM0 signal is multiplied by a square wave with either 4 periods for each FM0 symbol. 
            /// </summary>
            NUMBER_4,
            /// <summary>
            /// The FM0 signal is multiplied by a square wave with either 8 periods for each FM0 symbol. 
            /// </summary>
            NUMBER_8,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The tag protocol for which this link profile has been 
        /// configured.  The value of this field determines which of 
        /// the structures within the profileConfig contains the 
        /// link profile configuration information. 
        /// </summary>
        public enum RadioProtocol : uint
        {
            /// <summary>
            /// ISO 18000-6C
            /// </summary>
            ISO18K6C,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Tag's memory bank
        /// </summary>
        public enum MemoryBank : uint
        {
            /// <summary>
            /// Bank 0
            /// </summary>
            BANK0 = 0x00,
            /// <summary>
            /// Bank 1
            /// </summary>
            BANK1 = 0x01,
            /// <summary>
            /// Bank 2
            /// </summary>
            BANK2 = 0x02,
            /// <summary>
            /// Bank 3
            /// </summary>
            BANK3 = 0x03,

            /// <summary>
            /// PC and EPC memory bank
            /// </summary>
            RESERVED = 0x00,
            /// <summary>
            /// EPC memory bank (start data at offset 32)
            /// </summary>
            EPC = 0x01,
            /// <summary>
            /// TID bank
            /// </summary>
            TID = 0x02,
            /// <summary>
            /// User memory bank
            /// </summary>
            USER = 0x03,

            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Specifies what flag, selected (i.e., SL) or one of the four inventory 
        /// flags (i.e., S0, S1, S2, or S3), will be modified by the action. 
        /// </summary>
        public enum Target : uint
        {
            /// <summary>
            /// Tag energized : Indefinite
            /// Tag not energized : None
            /// </summary>
            S0,
            /// <summary>
            /// Tag energized :
            ///     Norminal temperature range : 5s > persistence > 500ms
            ///     Exttended temperature range : Not specified
            /// Tag not energized :
            ///     Norminal temperature range : 5s > persistence > 500ms
            ///     Exttended temperature range : Not specified
            /// </summary>
            S1,
            /// <summary>
            /// Tag energized : Indefinite
            /// Tag not energized :
            ///     Norminal temperature range : persistence > 2s
            ///     Exttended temperature range : Not specified
            /// </summary>
            S2,
            /// <summary>
            /// Tag energized : Indefinite
            /// Tag not energized :
            ///     Norminal temperature range : persistence > 2s
            ///     Exttended temperature range : Not specified
            /// </summary>
            S3,
            /// <summary>
            /// Tag energized : Indefinite
            /// Tag not energized :
            ///     Norminal temperature range : persistence > 2s
            ///     Exttended temperature range : Not specified
            /// </summary>
            SELECTED,
            /// <summary>
            /// only for CTESIUS Tag
            /// </summary>
            CTESIUS = 0x07,
            /// <summary>
            /// 
            /// </summary>
            UNKNOWN = 0xFFFF
        };

       /// <summary>
       /// Specifies the action that will be applied to the tag populations (i.e, the 
        /// matching and non-matching tags). 
       /// </summary>
        public enum Action : uint
        {
            /// <summary>
            /// Match : Assert SL or inventoried -> A      
            /// Non-Match : Deassert SL or inventoried -> B 
            /// </summary>
            ASLINVA_DSLINVB,
            /// <summary>
            /// Match : Assert SL or inventoried -> A
            /// Non-Match : Nothing
            /// </summary>
            ASLINVA_NOTHING,
            /// <summary>
            /// Match : Nothing
            /// Non-Match : Deassert SL or inventoried -> B
            /// </summary>
            NOTHING_DSLINVB,
            /// <summary>
            /// Match : Negate SL or (A -> B, B -> A)
            /// Non-Match : Nothing                      
            /// </summary>
            NSLINVS_NOTHING,
            /// <summary>
            /// Match : Deassert SL or inventoried -> B
            /// Non-Match : Assert SL or inventoried -> A
            /// </summary>
            DSLINVB_ASLINVA,
            /// <summary>
            /// Match : Deassert SL or inventoried -> B
            /// Non-Match : Nothing
            /// </summary>
            DSLINVB_NOTHING,
            /// <summary>
            /// Match : Nothing 
            /// Non-Match : Assert SL or inventoried -> A
            /// </summary>
            NOTHING_ASLINVA,
            /// <summary>
            /// Match : Nothing 
            /// Non-Match : Negate SL or (A -> B, B -> A)
            /// </summary>
            NOTHING_NSLINVS,
            /// <summary>
            /// Unknown action
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Specifies the state of the selected (SL) flag for tags that will have 
        /// the operation applied to them. 
        /// </summary>
        public enum Selected : uint
        {
            /// <summary>
            /// Select all
            /// </summary>
            ALL = 0,
            /// <summary>
            /// Select off
            /// </summary>
            DEASSERTED = 2,
            /// <summary>
            /// Select on
            /// </summary>
            ASSERTED = 3,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Specifies which inventory session flag (i.e., S0, S1, S2, or S3) 
        /// will be matched against the inventory state specified by target.
        /// </summary>
        public enum Session : uint
        {
            /// <summary>
            /// <para>Tag energized : Indefinite</para>
            /// <para>Tag not energized : None</para>
            /// </summary>
            S0,
            /// <summary>
            /// <para>Tag energized :</para>
            /// <para>    Norminal temperature range : 5s > persistence > 500ms</para>
            /// <para>    Exttended temperature range : Not specified</para>
            /// <para>Tag not energized :</para>
            /// <para>    Norminal temperature range : 5s > persistence > 500ms</para>
            /// <para>    Exttended temperature range : Not specified</para>
            /// </summary>
            S1,
            /// <summary>
            /// <para>Tag energized : Indefinite</para>
            /// <para>Tag not energized :</para>
            /// <para>    Norminal temperature range : persistence > 2s</para>
            /// <para>    Exttended temperature range : Not specified</para>
            /// </summary>
            S2,
            /// <summary>
            /// <para>Tag energized : Indefinite</para>
            /// <para>Tag not energized :</para>
            /// <para>    Norminal temperature range : persistence > 2s</para>
            /// <para>    Exttended temperature range : Not specified</para>
            /// </summary>
            S3,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Specifies the state of the inventory session flag (i.e., A or B), 
        /// specified by session, for tags that will have the operation 
        /// applied to them. 
        /// </summary>
        public enum SessionTarget : uint
        {
            /// <summary>
            /// Session A
            /// </summary>
            A,
            /// <summary>
            /// Session B
            /// </summary>
            B,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// Based upon usage scenarios, different singulation algorithms (i.e., Q-adjustment, 
        /// etc.) may be desired.  This document simply documents the mechanisms by 
        /// which an application can choose and configure singulation algorithms.  This 
        /// document does not provide specific information about the singulation algorithms.  
        /// </summary>
        public enum SingulationAlgorithm : uint
        {
            /// <summary>
            /// Fixed Q value.  This is the MAC's 
            /// singulation algorithm 0 (see [MAC-EDS]). 
            /// NOTE:  when performing non-inventory 
            /// tag-access operations (i.e., read, write, 
            /// kill, or lock), the MAC always uses this 
            /// singulation algorithm. 
            /// </summary>
            FIXEDQ  = 0,
            /// <summary>
            /// Adjusts the Q value based on the presence
            /// or absence of tags.  This is the MAC's 
            /// singulation algorithm 1 (see [MAC-EDS]). 
            /// </summary>
//            DYNAMICQ_1 = 1,
            /// <summary>
            /// Adjusts the Q value based on the presence
            /// or absence of tags.  This is the MAC's 
            /// singulation algorithm 2 (see [MAC-EDS]). 
            /// </summary>
//            DYNAMICQ_2 = 2,
            /// <summary>
            /// Adjusts the Q value based on the presence
            /// or absence of tags.  This is the MAC's 
            /// singulation algorithm 3 (see [MAC-EDS]). 
            /// </summary>
            DYNAMICQ = 3,
            /// <summary>
            /// Unknown
            /// </summary>
            UNKNOWN         = 0xFFFF
        };

        /// <summary>
        /// The type of write that will be performed – i.e., sequential or random.  
        /// The value of this field determines which of the structures within 
        /// parameters contains the write parameters.  
        /// </summary>
        public enum WriteType : uint
        {
            /// <summary>
            /// Write tag method by sequential
            /// </summary>
            SEQUENTIAL,
            /// <summary>
            /// Write tag method by random
            /// </summary>
            RANDOM,
            /// <summary>
            /// Unknown type
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /*
        public enum Permission : uint
        {
            /// <summary>
            /// The password may be read and written when the tag is in either the open 
            /// or secured states. 
            /// </summary>
            ACCESSIBLE,
            /// <summary>
            /// The password may be read and written when the tag is in either the open 
            /// or secured states and this access permission should be set permanently. 
            /// </summary>
            ALWAYS_ACCESSIBLE,
            /// <summary>
            /// The password may be read or written only when the tag is in the 
            /// secured state. 
            /// </summary>
            SECURED_ACCESSIBLE,
            /// <summary>
            /// The password may not be read or written and this access permission 
            /// should be set permanently. 
            /// </summary>
            ALWAYS_NOT_ACCESSIBLE,
            /// <summary>
            /// The password's access permission should remain unchanged. 
            /// </summary>
            UNCHANGED,
            /// <summary>
            /// Unknown permission
            /// </summary>
            UNKNOWN                = 0xFFFF
        };*/
        /// <summary>
        /// A  tag-permission  command  (aka,  tag  lock)  allows  the  application  to  set  the 
        /// access permissions of a tag.  These include the following: 
        /// •    Set whether or not an access password is required to write to the EPC, 
        /// TID, or user memory banks.  
        /// •    Set  whether  or  not  the  above  memory-write  permission  is  permanently 
        /// set.    Once  the  memory-write  permission  has  been  permanently  set, 
        /// attempts to change the permission or turn off the permanent setting will 
        /// fail. 
        /// •    Set a memory bank to be read-only. 
        /// •    Set whether or not the individual passwords (i.e., access and kill) may be 
        /// accessed (i.e., read and written) and, if they are accessible, whether or 
        /// not an access password is required to read the individual passwords (i.e., 
        /// access and kill). 
        /// •    Set whether or not the above password-access permission is 
        /// permanently set.  Once the password-access permission has been 
        /// permanently set, attempts to change the permission or turn off the 
        /// permanent setting will fail. 
        /// •    Set the individual passwords to be inaccessible (i.e., unable to be read or 
        /// written). 
        /// </summary>
        public enum Permission : uint
        {
            /// <summary>
            /// This bank can be read and written
            /// </summary>
            UNLOCK = 0x0,
            /// <summary>
            /// This bank can be read and written permanently.
            /// Note : The security of this bank can not be changed
            /// </summary>
            PERM_UNLOCK = 0x1,
            /// <summary>
            /// This bank can not be accessible unless access password is provided
            /// </summary>
            LOCK = 0x2,
            /// <summary>
            /// This bank can not be accessible unless access password is provided.
            /// Note : The security of this bank can not be changed
            /// </summary>
            PERM_LOCK = 0x3,
            /// <summary>
            /// The permission should remain unchanged. 
            /// </summary>
            UNCHANGED = 0x4,
            /// <summary>
            /// Unknown permission
            /// </summary>
            UNKNOWN = 0xFFFF
        };
/*
 * public enum Permission : uint
        {
            /// <summary>
            /// This bank can be read and written
            /// </summary>
            UNLOCK = 0x0,
            /// <summary>
            /// This bank can not be accessible unless access password is provided.
            /// Note : The security of this bank can not be changed
            /// </summary>
            P_LOCK = 0x1,
            /// <summary>
            /// This bank can be read and written permanently.
            /// Note : The security of this bank can not be changed
            /// </summary>
            P_UNLOCK = 0x2,
            /// <summary>
            /// This bank can not be accessible unless access password is provided
            /// </summary>
            LOCK = 0x3,
            /// <summary>
            /// The permission should remain unchanged. 
            /// </summary>
            UNCHANGED = 0x4,
            /// <summary>
            /// Unknown permission
            /// </summary>
            UNKNOWN = 0xFFFF
        };
*/
        /*  
         * public enum Permission : uint
        {
            /// <summary>
            /// The memory bank is writeable when the tag is in either the open or secured states. 
            /// </summary>
            WRITEABLE,
            /// <summary>
            /// The memory bank is writeable when the tag is in either the open 
            /// or secured states and this access permission should be set permanently. 
            /// </summary>
            ALWAYS_WRITEABLE,
            /// <summary>
            /// The memory bank is writeable only when the tag is in the secured state. 
            /// </summary>
            SECURED_WRITEABLE,
            /// <summary>
            /// The memory bank is not writeable and this access permission should be set permanently. 
            /// </summary>
            ALWAYS_NOT_WRITEABLE,
            /// <summary>
            /// The memory bank's access permission should remain unchanged. 
            /// </summary>
            UNCHANGED,
            /// <summary>
            /// Unknown Permission
            /// </summary>
            UNKNOWN              = 0xFFFF
        };*/
        /**
         * See native RFID_18K6C_QT_OPT_CMD_TYPE*
        **/

        public enum OptType : uint
        {
            OPT_NONE,
            OPT_READ,
            OPT_WRITE_TYPE_SEQUENTIAL,
            OPT_WRITE_TYPE_RANDOM,
            UNKNOWN = 0xFFFF
        };


        /**
         * See native RFID_18K6C_QT_CTRL_TYPE*
        **/

        public enum QTCtrlType : uint
        {
            READ,
            WRITE,
            UNKNOWN = 0xFFFF
        };

        /**
         * See native RFID_18K6C_QT_PERSISTENCE_TYPE*
        **/

        public enum QTPersistenceType : uint
        {
            TEMPORARY,
            PERMANENT,
            UNKNOWN = 0xFFFF
        };


        /**
         * See native RFID_18K6C_QT_SR_TYPE*
        **/

        public enum QTShortRangeType : uint
        {
            DISABLE,
            ENABLE,
            UNKNOWN = 0xFFFF
        };

        /**
         * See native RFID_18K6C_QT_MEMMAP_TYPE*
        **/

        public enum QTMemMapType : uint
        {
            PRIVATE,
            PUBLIC,
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The type of data that will have its 
        /// mode set to the mode specified by 
        /// responseMode.  For version 1.1 of the 
        /// RFID Reader Library, the only valid 
        /// value is RFID_RESPONSE_TYPE_DATA. 
        /// </summary>
        public enum ResponseType : uint
        {
            /// <summary>
            /// an application can control the mode of data reporting in response to 
            /// a tag-access operation (i.e., inventory, read, etc.).
            /// </summary>
            DATA    = 0xFFFFFFFF,
            /// <summary>
            /// Unknown mode
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The requested data-reporting mode for 
        /// the data type specified by 
        /// responseType. 
        /// </summary>
        public enum ResponseMode : uint
        {
            /// <summary>
            /// The response data is limited to provide the application with the 
            /// pertinent tag-access operation data, but minimize the amount of MAC-to-
            /// host communication overhead. 
            /// </summary>
            COMPACT  = 0x00000001,
            /// <summary>
            ///  The response data builds on the compact mode to provide the 
            ///  application with status and contextual information to give additional finer-
            /// grained feedback such as the beginning of inventory cycles, etc. 
            /// </summary>
            NORMAL   = 0x00000003,
            /// <summary>
            ///  The response data builds on the normal mode by providing 
            /// additional diagnostics and statistical information. 
            /// </summary>
            EXTENDED = 0x00000007,
            /// <summary>
            /// Unknown Mode
            /// </summary>
            UNKNOWN  = 0xFFFF
        };

        /// <summary>
        /// The type of reset that the MAC should perform. 
        /// </summary>
        public enum MacResetType : uint
        {
            /// <summary>
            /// Soft reset
            /// </summary>
            SOFT,
            /// <summary>
            /// Unknown action
            /// </summary>
            UNKNOWN = 0xFFFF
        };

        /// <summary>
        /// The new region of operation for the radio module. 
        /// </summary>
        public enum MacRegion : uint
        {
            /// <summary>
            /// RFID_MAC_REGION_FCC_GENERIC
            /// </summary>
            FCC_GENERIC,
            /// <summary>
            /// RFID_MAC_REGION_ETSI_GENERIC
            /// </summary>
            ETSI_GENERIC,
            /// <summary>
            /// RFID_MAC_REGION_NEWETSI_GENERIC
            /// </summary>
            NEWETSI_GENERIC,
            /// <summary>
            /// RFID_MAC_REGION_JAPAN_GENERIC
            /// </summary>
            JAPAN_GENERIC,
            /// <summary>
            /// RFID_MAC_REGION_KOREA_LBT
            /// </summary>
            KOREA_LBT,
            /// <summary>
            /// RFID_MAC_REGION_KOREA_FHSS
            /// </summary>
            KOREA_FHSS,
#if false
            /// <summary>
            /// RFID_MAC_REGION_ISRAEL_GENERIC
            /// </summary>
            ISRAEL_GENERIC,
#endif
            /// <summary>
            /// UNKNOWN_REGION
            /// </summary>
            UNKNOWN = 0xFFFF
        };
        /*
         *      RFID_MAC_REGION_FCC_GENERIC,
                RFID_MAC_REGION_ETSI_GENERIC,
                RFID_MAC_REGION_NEWETSI_GENERIC,
                RFID_MAC_REGION_JAPAN_GENERIC,
                RFID_MAC_REGION_KOREA_LBT,
                RFID_MAC_REGION_KOREA_FHSS,
                RFID_MAC_REGION_ISRAEL_GENERIC
         * 
         * 
         */
        /**
         * See native RFID_RADIO_GPIO_*
        **/

        public enum GpioPin : uint
        {
            /// <summary>
            /// Pin 0
            /// </summary>
            PIN_0  = ( uint ) 0x1 << 0,
            /// <summary>
            /// Pin 1
            /// </summary>
            PIN_1  = ( uint ) 0x1 << 1,
            /// <summary>
            /// Pin 2
            /// </summary>
            PIN_2  = ( uint ) 0x1 << 2,
            /// <summary>
            /// Pin 3
            /// </summary>
            PIN_3  = ( uint ) 0x1 << 3
            /*
            PIN_4  = ( uint ) 0x1 << 4,
            PIN_5  = ( uint ) 0x1 << 5,
            PIN_6  = ( uint ) 0x1 << 6,
            PIN_7  = ( uint ) 0x1 << 7,
            PIN_8  = ( uint ) 0x1 << 8,
            PIN_9  = ( uint ) 0x1 << 9,
            PIN_10 = ( uint ) 0x1 << 10,
            PIN_11 = ( uint ) 0x1 << 11,
            PIN_12 = ( uint ) 0x1 << 12,
            PIN_13 = ( uint ) 0x1 << 13,
            PIN_14 = ( uint ) 0x1 << 14,
            PIN_15 = ( uint ) 0x1 << 15,
            PIN_16 = ( uint ) 0x1 << 16,
            PIN_17 = ( uint ) 0x1 << 17,
            PIN_18 = ( uint ) 0x1 << 18,
            PIN_19 = ( uint ) 0x1 << 19,
            PIN_20 = ( uint ) 0x1 << 20,
            PIN_21 = ( uint ) 0x1 << 21,
            PIN_22 = ( uint ) 0x1 << 22,
            PIN_23 = ( uint ) 0x1 << 23,
            PIN_24 = ( uint ) 0x1 << 24,
            PIN_25 = ( uint ) 0x1 << 25,
            PIN_26 = ( uint ) 0x1 << 26,
            PIN_27 = ( uint ) 0x1 << 27,
            PIN_28 = ( uint ) 0x1 << 28,
            PIN_29 = ( uint ) 0x1 << 29,
            PIN_30 = ( uint ) 0x1 << 30,
            PIN_31 = ( uint ) 0x1 << 31
            */
        };



        /// <summary>
        /// function Result value definitions
        /// </summary>
        public enum Result : int
        {
            /// <summary>
            /// Success
            /// </summary>
            OK           =  0,

            /// <summary>
            /// Attempted to open a radio that is already open
            /// </summary>
            ALREADY_OPEN = -9999,

            /// <summary>
            /// Buffer supplied is too small
            /// </summary>
            BUFFER_TOO_SMALL,

            /// <summary>
            /// General failure 
            /// </summary>
            FAILURE,

            /// <summary>
            /// Failed to load radio bus driver
            /// </summary>
            DRIVER_LOAD,

            /// <summary>
            /// Library cannot use version of radio bus driver present on system
            /// </summary>
            DRIVER_MISMATCH,

            /// <summary>
            /// Operation cannot be performed while library is in emulation mode
            /// </summary>
            EMULATION_MODE,

            /// <summary>
            /// Antenna number is invalid
            /// </summary>
            INVALID_ANTENNA,

            /// <summary>
            /// Radio handle provided is invalid
            /// </summary>
            INVALID_HANDLE,

            /// <summary>
            /// One of the parameters to the function is invalid
            /// </summary>
            INVALID_PARAMETER,

            /// <summary>
            /// Attempted to open a non-existent radio
            /// </summary>
            NO_SUCH_RADIO,

            /// <summary>
            /// Library has not been successfully initialized
            /// </summary>
            NOT_INITIALIZED,

            /// <summary>
            /// Function not supported
            /// </summary>
            NOT_SUPPORTED,

            /// <summary>
            /// Op cancelled by cancel op func, close radio, or library shutdown
            /// </summary>
            OPERATION_CANCELLED,

            /// <summary>
            /// Library encountered an error allocating memory
            /// </summary>
            OUT_OF_MEMORY,

            /// <summary>
            /// The operation cannot be performed because the radio is currently busy
            /// </summary>
            RADIO_BUSY,

            /// <summary>
            /// The underlying radio module encountered an error
            /// </summary>
            RADIO_FAILURE,

            /// <summary>
            /// The radio has been detached from the system
            /// </summary>
            RADIO_NOT_PRESENT,

            /// <summary>
            /// The RFID library function is not allowed at this time.
            /// </summary>
            CURRENTLY_NOT_ALLOWED,

            /// <summary>
            /// The radio module's MAC firmware is not responding to requests.
            /// </summary>
            RADIO_NOT_RESPONDING,

            /// <summary>
            /// The MAC firmware encountered an error while initiating the nonvolatile
            /// memory update.  The MAC firmware will return to its normal idle state
            /// without resetting the radio module.
            /// </summary>
            NONVOLATILE_INIT_FAILED,

            /// <summary>
            /// An attempt was made to write data to an address that is not in the
            /// valid range of radio module nonvolatile memory addresses.        
            /// </summary>
            NONVOLATILE_OUT_OF_BOUNDS,

            /// <summary>
            /// The MAC firmware encountered an error while trying to write to the
            /// radio module's nonvolatile memory region. 
            /// </summary>
            NONVOLATILE_WRITE_FAILED,

            /// <summary>
            /// The underlying transport layer detected that there was an overflow
            /// error Resulting in one or more bytes of the incoming data being   
            /// dropped.  The operation was aborted and all data in the pipeline was
            /// flushed.      
            /// </summary>
            RECEIVE_OVERFLOW,

            /*------------------WaterYu define----------------*/
            /// <summary>
            /// System catch exception
            /// </summary>
            SYSTEM_CATCH_EXCEPTION,
            /// <summary>
            /// No Tag Found
            /// </summary>
            NO_TAG_FOUND,
            /// <summary>
            /// Tag access maximum retry error
            /// </summary>
            MAX_RETRY_EXIT,
            /// <summary>
            /// Unknown Operation
            /// </summary>
            UNKNOWN_OPERATION,
            /// <summary>
            /// Pre-allocated buffer is full
            /// </summary>
            PREALLOCATED_BUFFER_FULL,
            /// <summary>
            /// RFID Power up failed
            /// </summary>
            POWER_UP_FAIL,
            /// <summary>
            /// RFID Power down failed
            /// </summary>
            POWER_DOWN_FAIL,
            /// <summary>
            /// Network reset
            /// </summary>
            NETWORK_RESET,
            /// <summary>
            /// Network lost
            /// </summary>
            NETWORK_LOST,
            /// <summary>
            /// OEM Country code invalid
            /// </summary>
            INVALID_OEM_COUNTRY_CODE,

            /*------------------Mephist define----------------*/
            /// <summary>
            /// Device not support
            /// </summary>
            DEVICE_NOT_SUPPORT,
            /// <summary>
            /// Device Connected
            /// </summary>
            DEVICE_CONNECTED,

            /// Process thread error
            THREAD_ERROR,

            /// <summary>
            /// MAC ERROR
            /// </summary>
            MAC_ERROR,
        };
#if NOUSE
        public enum RFID_OPERATION
        {
            TAG_INVENTORY = 0,
            TAG_READ,
            TAG_WRITE,
            TAG_LOCK,
            TAG_KILL,
            TAG_BLOCK_WRITE,
            TAG_BLOCK_ERASE,
        	TAG_SEARCH,
        }

        public enum RFID_REQUEST_TYPE_MSGID
        {
            RFID_REQUEST_TYPE_MSGID_TAG_INVENTORY = 0x440,
            RFID_REQUEST_TYPE_MSGID_TAG_READ,
            RFID_REQUEST_TYPE_MSGID_TAG_WRITE,
            RFID_REQUEST_TYPE_MSGID_TAG_KILL,
            RFID_REQUEST_TYPE_MSGID_TAG_LOCK,
            RFID_REQUEST_TYPE_MSGID_TAG_BLOCK_WRITE,
            RFID_REQUEST_TYPE_MSGID_TAG_BLOCK_ERASE,
            RFID_REQUEST_TYPE_MSGID_TAG_SEARCH,

            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_INVENTORY_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_INVENTORY_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_INVENTORY_END,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_ACCESS_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_ACCESS_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_ACCESS_END,
            /*RFID_REQUEST_TYPE_MSGID_THREAD_TAG_READ_BEGIN, 
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_READ_PROCESS, 
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_READ_END, 
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_WRITE_BEGIN, 
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_WRITE_PROCESS, 
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_WRITE_END, 
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_LOCK_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_LOCK_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_LOCK_END,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_KILL_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_KILL_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_KILL_END,*/
            RFID_REQUEST_TYPE_MSGID_THREAD_BLOCK_WRITE_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_BLOCK_WRITE_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_BLOCK_WRITE_END,
            RFID_REQUEST_TYPE_MSGID_THREAD_BLOCK_ERASE_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_BLOCK_ERASE_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_BLOCK_ERASE_END,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_SEARCH_BEGIN,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_SEARCH_PROCESS,
            RFID_REQUEST_TYPE_MSGID_THREAD_TAG_SEARCH_END,

            RFID_REQUEST_TYPE_MSGID_PACKET_COMMAND_BEGIN,
            RFID_REQUEST_TYPE_MSGID_PACKET_COMMAND_END,
        }
#endif
        /// <summary>
        /// Tag Parameters Selected flags
        /// </summary>
        [Flags]
        public enum SelectMaskFlags
        {
            /// <summary>
            /// a flag that enable all disable
            /// </summary>
            DISABLE_ALL = 0,   
            /// <summary>
            /// A flag that indicates if, after performing the inventory cycle for 
            /// the specified target (i.e., A or B), if the target should be toggled 
            /// (i.e., A to B or B to A) and another inventory cycle run.  A non-
            /// zero value indicates that the target should be toggled.  A zero 
            /// value indicates that the target should not be toggled.  
            /// </summary>
            ENABLE_TOGGLE = 1,
            /// <summary>
            /// A flag that enable using PC mask to select a tag.
            /// </summary>
            ENABLE_PC_MASK = 2,
            /// <summary>
            /// A flag that select a tag if the tag is not matching to you selected criteria.
            /// </summary>
            ENABLE_NON_MATCH = 4,
            /// <summary>
            /// a flag that enable all items
            /// </summary>
            ENABLE_ALL = ENABLE_TOGGLE | ENABLE_PC_MASK | ENABLE_NON_MATCH,
        }
        /// <summary>
        /// Firmware Update flags
        /// </summary>
        public enum FwUpdateFlags
        {
            /// <summary>
            /// Update Firmware without test
            /// </summary>
            NVMEM_UPDATE = 0x00000000,
            /// <summary>
            /// Update Firmware with test
            /// </summary>
            NVMEM_UPDATE_TEST,
            /// <summary>
            /// Update Bootloader without test
            /// </summary>
            NVMEM_UPDATE_BL,
            /// <summary>
            /// Update Bootloader with test
            /// </summary>
            NVMEM_UPDATE_BL_TEST,
            /// <summary>
            /// Update Application without test
            /// </summary>
            NVMEM_UPDATE_APP,
            /// <summary>
            /// Update Application with test
            /// </summary>
            NVMEM_UPDATE_APP_TEST
        }
        /// <summary>
        /// GPIO Trigger flags
        /// </summary>
        public enum GPIOTrigger : byte
        {
            /// <summary>
            /// Turn off trigger
            /// </summary>
            OFF,
            /// <summary>
            /// Raising edge trigger
            /// </summary>
            RAISING_EDGE,
            /// <summary>
            /// Falling edge trigger
            /// </summary>
            FALLING_EDGE,
            /// <summary>
            /// Raising edge and falling edge trigger
            /// </summary>
            ANY_TRIGGER,
        }
        /// <summary>
        /// Permalock access flags
        /// </summary>
        public enum PermalockFlags : int
        {
            /// <summary>
            /// Set permalock status
            /// </summary>
            SET_VALUE = 1,
            /// <summary>
            /// Get permalock status
            /// </summary>
            GET_VALUE = 0
        }

    } // Constants end

} // rfid_csharp end

