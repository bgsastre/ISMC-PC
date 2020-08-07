using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSMC
{
    class CDiagnosis
    {

        /// <summary>
        /// Diagnosis Commands
        /// </summary>
        public enum TDIAGNOSIS_CMD : byte
        {
            DIAG_CMD_GET_FW_VERSION = 1,
            DIAG_CMD_NOTIFY_FAULT   = 2,
            DIAG_CMD_FAULT_RESET    = 3,
            DIAG_CMD_PARAM_ERROR    = 4,
            DIAG_CMD_ISMC_REINIT    = 6,
        } // public enum TDIAGNOSIS_CMD : byte

        /// <summary>
        /// Diagnosis Commands
        /// </summary>
        public enum TDIAGNOSIS_FAULT_MASK : UInt32
        {
            /*
            DIAG_NO_FAULT = 1,
            DIAG_COMM_FAULT = 2,
            DIAG_DRIVER_FAULT = 3,
            DIAG_CONTROL_FAULT = 4,
            DIAG_TEMP_FAULT = 5,
            DIAG_SPEED_FAULT = 6,
            DIAG_IAVG_FAULT = 7,
            */
            DIAG_FLAG_MASK_DRIVER_VDS_OCP   = 0x00000200, // Indicates VDS monitor overcurrent fault condition
            DIAG_FLAG_MASK_DRIVER_GDF       = 0x00000100, // Indicates gate drive fault condition
            DIAG_FLAG_MASK_DRIVER_UVLO      = 0x00000080, // Indicates undervoltage lockout fault condition
            DIAG_FLAG_MASK_DRIVER_OTSD      = 0x00000040, // Indicates overtemperature shutdown
            DIAG_FLAG_MASK_DRIVER_VDS_HA    = 0x00000020, // Indicates VDS overcurrent fault on the A high-side MOSFET
            DIAG_FLAG_MASK_DRIVER_VDS_LA    = 0x00000010, // Indicates VDS overcurrent fault on the A low-side MOSFET
            DIAG_FLAG_MASK_DRIVER_VDS_HB    = 0x00000008, // Indicates VDS overcurrent fault on the B high-side MOSFET
            DIAG_FLAG_MASK_DRIVER_VDS_LB    = 0x00000004, // Indicates VDS overcurrent fault on the B low-side MOSFET
            DIAG_FLAG_MASK_DRIVER_VDS_HC    = 0x00000002, // Indicates VDS overcurrent fault on the C high-side MOSFET
            DIAG_FLAG_MASK_DRIVER_VDS_LC    = 0x00000001, // Indicates VDS overcurrent fault on the C low-side MOSFET
            DIAG_FLAG_MASK_COMM_FAULT       = 0x00000400,
            DIAG_FLAG_MASK_CTRL_FAULT       = 0x00000800,
            DIAG_FLAG_MASK_TEMP_FAULT       = 0x00001000,
            DIAG_FLAG_MASK_SPEED_FAULT      = 0x00002000,
            DIAG_FLAG_MASK_IAVG_FAULT       = 0x00004000,
            DIAG_FLAG_MASK_MOTOR_FAULT      = 0x00008000,
            DIAG_FLAG_MASK_ENC_FAULT        = 0x00010000,
            DIAG_FLAG_MASK_STO_FAULT        = 0x00020000,
            DIAG_FLAG_MASK_BRAKE_FAULT      = 0x00040000,

        } // public enum TDIAGNOSIS_CMD : byte




    }
}
