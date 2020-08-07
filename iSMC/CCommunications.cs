using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peak.Can.Basic;

using TPCANHandle = System.UInt16;
using TPCANBitrateFD = System.String;
using TPCANTimestampFD = System.UInt64;

namespace iSMC
{
    class CCommunications
    {

        #region ENUMERATIONS

        // Periodo del proceso de escritura de los mensajes CAN [ms]
        public const int CAN_TIME_TX_PROCESS = 100;
        // Periodo del proceso de Lectura de los mensajes CAN [ms]
        public const int CAN_TIME_RX_PROCESS = 10;
        // Periodo del proceso de escritura de los mensajes de Consumer HB [ms]
        public const int CAN_TIME_CONSUMER_HB = 100;

        public const byte NodeIdA = 0x50;
        public const byte NodeIdB = 0x51;

        /// <summary>
        /// Represents a PCAN Baud rate register value
        /// </summary>
        public enum TCONNECTION : byte
        {
            CONNECTION_WAIT_HW    = 0,
            CONNECTION_DISCONNECT = 1,
            CONNECTION_CONNECT    = 2
        }

        public enum TCAN_MESSAGES : UInt16
        {
            CAN_MSG_ID_REF_A            = 0x090,
            CAN_MSG_ID_CTRL_STATUS_A    = 0x091,
            CAN_MSG_ID_BOARD_STATUS_A   = 0x096,
            CAN_MSG_ID_CALIB_RQ_A       = 0x095,
            CAN_MSG_ID_CALIB_RS_A       = 0x094,
            CAN_MSG_ID_DIAG_RQ_A        = 0x604,
            CAN_MSG_ID_DIAG_RS_A        = 0x654,
            CAN_MSG_ID_SDO_TX_A         = 0x610,
            CAN_MSG_ID_NMT_TX_A         = 0x750,
            CAN_MSG_ID_SDO_RX_A         = 0x590,
            CAN_MSG_ID_NMT_RX_A         = 0x77F,               
            CAN_MSG_ID_REF_B            = 0x098,
            CAN_MSG_ID_CTRL_STATUS_B    = 0x099,
            CAN_MSG_ID_BOARD_STATUS_B   = 0x09E,
            CAN_MSG_ID_CALIB_RQ_B       = 0x09D,
            CAN_MSG_ID_CALIB_RS_B       = 0x09C,
            CAN_MSG_ID_DIAG_RQ_B        = 0x605,
            CAN_MSG_ID_DIAG_RS_B        = 0x655,
            CAN_MSG_ID_SDO_TX_B         = 0x611,
            CAN_MSG_ID_NMT_TX_B         = 0x751,
            CAN_MSG_ID_SDO_RX_B         = 0x591,
            CAN_MSG_ID_NMT_RX_B         = 0x711,
        }

        #endregion




        #region GLOBAL VARIABLES
        static public TPCANHandle m_PcanHandle;
        

        static public TPCANBaudrate m_Baudrate = TPCANBaudrate.PCAN_BAUD_250K;
        static public TPCANType     m_HwType   = TPCANType.PCAN_TYPE_ISA;

        static public byte m_ConnectionStatus = (byte) TCONNECTION.CONNECTION_WAIT_HW;

        static public bool m_bIsFD = false;

        #endregion


        #region Functions
        static public void SetBaudRate(TPCANBaudrate eBaudrate)
        {
            m_Baudrate = eBaudrate;
        }

        static public TPCANBaudrate GetBaudRate()
        {
            return m_Baudrate;
        }

        static public void SetHWType(TPCANType eHWType)
        {
            m_HwType = eHWType;
        }

        static public TPCANType GetHWType()
        {
            return m_HwType;
        }

        static public void SetConnectionStatus(byte u8Status)
        {
            m_ConnectionStatus = u8Status;
        }

        static public byte GetConnectionStatus()
        {
            return m_ConnectionStatus;
        }

        #endregion


    }
}
