using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace iSMC
{
    class CFormat
    {

    }

    class Globals
    {
        public const Byte ECU_ISMC_A = 0x00;
        public const Byte ECU_ISMC_B = 0x01;

        static private string m_ConfigName = "";
        static private string m_CurrentDir = "";

        static public string GetConfigName()
        {
            return m_ConfigName;
        }

        static public void SetConfigName(string sConfigName)
        {
            m_ConfigName = sConfigName;
        }

        static public string GetCurrentDir()
        {
            return m_CurrentDir;
        }

        static public void SetCurrentDir(string sDir)
        {
            m_CurrentDir = sDir;
        }


    }

    class CErrors
    {
        static private byte[] u8ErrorCode = { 0, 0 };

        public enum ErrorCodes : byte
        {
            USER_ErrorCode_NoError = 0,                           // no error error code
            USER_ErrorCode_iqFullScaleCurrent_A_High = 1,         // iqFullScaleCurrent_A too high error code
            USER_ErrorCode_iqFullScaleCurrent_A_Low = 2,          // iqFullScaleCurrent_A too low error code
            USER_ErrorCode_iqFullScaleVoltage_V_High = 3,         // iqFullScaleVoltage_V too high error code
            USER_ErrorCode_iqFullScaleVoltage_V_Low = 4,          // iqFullScaleVoltage_V too low error code
            USER_ErrorCode_iqFullScaleFreq_Hz_High = 5,           // iqFullScaleFreq_Hz too high error code
            USER_ErrorCode_iqFullScaleFreq_Hz_Low = 6,            // iqFullScaleFreq_Hz too low error code
            USER_ErrorCode_numPwmTicksPerIsrTick_High = 7,        // numPwmTicksPerIsrTick too high error code
            USER_ErrorCode_numPwmTicksPerIsrTick_Low = 8,         // numPwmTicksPerIsrTick too low error code
            USER_ErrorCode_numIsrTicksPerCtrlTick_High = 9,       // numIsrTicksPerCtrlTick too high error code
            USER_ErrorCode_numIsrTicksPerCtrlTick_Low = 10,       // numIsrTicksPerCtrlTick too low error code
            USER_ErrorCode_numCtrlTicksPerCurrentTick_High = 11,  // numCtrlTicksPerCurrentTick too high error code
            USER_ErrorCode_numCtrlTicksPerCurrentTick_Low = 12,   // numCtrlTicksPerCurrentTick too low error code
            USER_ErrorCode_numCtrlTicksPerEstTick_High = 13,      // numCtrlTicksPerEstTick too high error code
            USER_ErrorCode_numCtrlTicksPerEstTick_Low = 14,       // numCtrlTicksPerEstTick too low error code
            USER_ErrorCode_numCtrlTicksPerSpeedTick_High = 15,    // numCtrlTicksPerSpeedTick too high error code
            USER_ErrorCode_numCtrlTicksPerSpeedTick_Low = 16,     // numCtrlTicksPerSpeedTick too low error code
            USER_ErrorCode_numCtrlTicksPerTrajTick_High = 17,     // numCtrlTicksPerTrajTick too high error code
            USER_ErrorCode_numCtrlTicksPerTrajTick_Low = 18,      // numCtrlTicksPerTrajTick too low error code
            USER_ErrorCode_numCurrentSensors_High = 19,           // numCurrentSensors too high error code
            USER_ErrorCode_numCurrentSensors_Low = 20,            // numCurrentSensors too low error code
            USER_ErrorCode_numVoltageSensors_High = 21,           // numVoltageSensors too high error code
            USER_ErrorCode_numVoltageSensors_Low = 22,            // numVoltageSensors too low error code
            USER_ErrorCode_offsetPole_rps_High = 23,              // offsetPole_rps too high error code
            USER_ErrorCode_offsetPole_rps_Low = 24,               // offsetPole_rps too low error code
            USER_ErrorCode_fluxPole_rps_High = 25,                // fluxPole_rps too high error code
            USER_ErrorCode_fluxPole_rps_Low = 26,                 // fluxPole_rps too low error code
            USER_ErrorCode_zeroSpeedLimit_High = 27,              // zeroSpeedLimit too high error code
            USER_ErrorCode_zeroSpeedLimit_Low = 28,               // zeroSpeedLimit too low error code
            USER_ErrorCode_forceAngleFreq_Hz_High = 29,           // forceAngleFreq_Hz too high error code
            USER_ErrorCode_forceAngleFreq_Hz_Low = 30,            // forceAngleFreq_Hz too low error code
            USER_ErrorCode_maxAccel_Hzps_High = 31,               // maxAccel_Hzps too high error code
            USER_ErrorCode_maxAccel_Hzps_Low = 32,                // maxAccel_Hzps too low error code
            USER_ErrorCode_maxAccel_est_Hzps_High = 33,           // maxAccel_est_Hzps too high error code
            USER_ErrorCode_maxAccel_est_Hzps_Low = 34,            // maxAccel_est_Hzps too low error code
            USER_ErrorCode_directionPole_rps_High = 35,           // directionPole_rps too high error code
            USER_ErrorCode_directionPole_rps_Low = 36,            // directionPole_rps too low error code
            USER_ErrorCode_speedPole_rps_High = 37,               // speedPole_rps too high error code
            USER_ErrorCode_speedPole_rps_Low = 38,                // speedPole_rps too low error code
            USER_ErrorCode_dcBusPole_rps_High = 39,               // dcBusPole_rps too high error code
            USER_ErrorCode_dcBusPole_rps_Low = 40,                // dcBusPole_rps too low error code
            USER_ErrorCode_fluxFraction_High = 41,                // fluxFraction too high error code
            USER_ErrorCode_fluxFraction_Low = 42,                 // fluxFraction too low error code
            USER_ErrorCode_indEst_speedMaxFraction_High = 43,     // indEst_speedMaxFraction too high error code
            USER_ErrorCode_indEst_speedMaxFraction_Low = 44,      // indEst_speedMaxFraction too low error code
            USER_ErrorCode_powerWarpGain_High = 45,               // powerWarpGain too high error code
            USER_ErrorCode_powerWarpGain_Low = 46,                // powerWarpGain too low error code
            USER_ErrorCode_systemFreq_MHz_High = 47,              // systemFreq_MHz too high error code
            USER_ErrorCode_systemFreq_MHz_Low = 48,               // systemFreq_MHz too low error code
            USER_ErrorCode_pwmFreq_kHz_High = 49,                 // pwmFreq_kHz too high error code
            USER_ErrorCode_pwmFreq_kHz_Low = 50,                  // pwmFreq_kHz too low error code
            USER_ErrorCode_voltage_sf_High = 51,                  // voltage_sf too high error code
            USER_ErrorCode_voltage_sf_Low = 52,                   // voltage_sf too low error code
            USER_ErrorCode_current_sf_High = 53,                  // current_sf too high error code
            USER_ErrorCode_current_sf_Low = 54,                   // current_sf too low error code
            USER_ErrorCode_voltageFilterPole_Hz_High = 55,        // voltageFilterPole_Hz too high error code
            USER_ErrorCode_voltageFilterPole_Hz_Low = 56,         // voltageFilterPole_Hz too low error code
            USER_ErrorCode_maxVsMag_pu_High = 57,                 // maxVsMag_pu too high error code
            USER_ErrorCode_maxVsMag_pu_Low = 58,                  // maxVsMag_pu too low error code
            USER_ErrorCode_estKappa_High = 59,                    // estKappa too high error code
            USER_ErrorCode_estKappa_Low = 60,                     // estKappa too low error code
            USER_ErrorCode_motor_type_Unknown = 61,               // motor type unknown error code
            USER_ErrorCode_motor_numPolePairs_High = 62,          // motor_numPolePairs too high error code
            USER_ErrorCode_motor_numPolePairs_Low = 63,           // motor_numPolePairs too low error code
            USER_ErrorCode_motor_ratedFlux_High = 64,             // motor_ratedFlux too high error code
            USER_ErrorCode_motor_ratedFlux_Low = 65,              // motor_ratedFlux too low error code
            USER_ErrorCode_motor_Rr_High = 66,                    // motor_Rr too high error code
            USER_ErrorCode_motor_Rr_Low = 67,                     // motor_Rr too low error code
            USER_ErrorCode_motor_Rs_High = 68,                    // motor_Rs too high error code
            USER_ErrorCode_motor_Rs_Low = 69,                     // motor_Rs too low error code
            USER_ErrorCode_motor_Ls_d_High = 70,                  // motor_Ls_d too high error code
            USER_ErrorCode_motor_Ls_d_Low = 71,                   // motor_Ls_d too low error code
            USER_ErrorCode_motor_Ls_q_High = 72,                  // motor_Ls_q too high error code
            USER_ErrorCode_motor_Ls_q_Low = 73,                   // motor_Ls_q too low error code
            USER_ErrorCode_maxCurrent_High = 74,                  // maxCurrent too high error code
            USER_ErrorCode_maxCurrent_Low = 75,                   // maxCurrent too low error code
            USER_ErrorCode_maxCurrent_resEst_High = 76,           // maxCurrent_resEst too high error code
            USER_ErrorCode_maxCurrent_resEst_Low = 77,            // maxCurrent_resEst too low error code
            USER_ErrorCode_maxCurrent_indEst_High = 78,           // maxCurrent_indEst too high error code
            USER_ErrorCode_maxCurrent_indEst_Low = 79,            // maxCurrent_indEst too low error code
            USER_ErrorCode_maxCurrentSlope_High = 80,             // maxCurrentSlope too high error code
            USER_ErrorCode_maxCurrentSlope_Low = 81,              // maxCurrentSlope too low error code
            USER_ErrorCode_maxCurrentSlope_powerWarp_High = 82,   // maxCurrentSlope_powerWarp too high error code
            USER_ErrorCode_maxCurrentSlope_powerWarp_Low = 83,    // maxCurrentSlope_powerWarp too low error code
            USER_ErrorCode_IdRated_High = 84,                     // IdRated too high error code
            USER_ErrorCode_IdRated_Low = 85,                      // IdRated too low error code
            USER_ErrorCode_IdRatedFraction_indEst_High = 86,      // IdRatedFraction_indEst too high error code
            USER_ErrorCode_IdRatedFraction_indEst_Low = 87,       // IdRatedFraction_indEst too low error code
            USER_ErrorCode_IdRatedFraction_ratedFlux_High = 88,   // IdRatedFraction_ratedFlux too high error code
            USER_ErrorCode_IdRatedFraction_ratedFlux_Low = 89,    // IdRatedFraction_ratedFlux too low error code
            USER_ErrorCode_IdRated_delta_High = 90,               // IdRated_delta too high error code
            USER_ErrorCode_IdRated_delta_Low = 91,                // IdRated_delta too low error code
            USER_ErrorCode_fluxEstFreq_Hz_High = 92,              // fluxEstFreq_Hz too high error code
            USER_ErrorCode_fluxEstFreq_Hz_Low = 93,               // fluxEstFreq_Hz too low error code
            USER_ErrorCode_ctrlFreq_Hz_High = 94,                 // ctrlFreq_Hz too high error code
            USER_ErrorCode_ctrlFreq_Hz_Low = 95,                  // ctrlFreq_Hz too low error code
            USER_ErrorCode_estFreq_Hz_High = 96,                  // estFreq_Hz too high error code
            USER_ErrorCode_estFreq_Hz_Low = 97,                   // estFreq_Hz too low error code
            USER_ErrorCode_RoverL_estFreq_Hz_High = 98,           // RoverL_estFreq_Hz too high error code
            USER_ErrorCode_RoverL_estFreq_Hz_Low = 99,            // RoverL_estFreq_Hz too low error code
            USER_ErrorCode_trajFreq_Hz_High = 100,                // trajFreq_Hz too high error code
            USER_ErrorCode_trajFreq_Hz_Low = 101,                 // trajFreq_Hz too low error code
            USER_ErrorCode_ctrlPeriod_sec_High = 102,             // ctrlPeriod_sec too high error code
            USER_ErrorCode_ctrlPeriod_sec_Low = 103,              // ctrlPeriod_sec too low error code
            USER_ErrorCode_maxNegativeIdCurrent_a_High = 104,     // maxNegativeIdCurrent_a too high error code
            USER_ErrorCode_maxNegativeIdCurrent_a_Low = 105       // maxNegativeIdCurrent_a too low error code

        }


        static public bool Error_QRaised(byte u8ECUId)
        {
            if (u8ErrorCode[u8ECUId] == (byte)ErrorCodes.USER_ErrorCode_NoError)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static public void Error_Set(byte u8ECUId, byte u8Code)
        {
            u8ErrorCode[u8ECUId] = u8Code;
        } // static public void Error_Set(byte u8ErrorCode)

        static public byte Error_Get(byte u8ECUId)
        {
            return u8ErrorCode[u8ECUId];
        } // static public byte Error_Get()


        static public string Error_CheckCode(byte u8ErrorCode)
        {
            string strErrorCode = "";

            switch (u8ErrorCode)
            {

                case ((byte)(ErrorCodes.USER_ErrorCode_NoError)):
                    {
                        strErrorCode = "no error error code   ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_iqFullScaleCurrent_A_High)):
                    {
                        strErrorCode = "iqFullScaleCurrent_A too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_iqFullScaleCurrent_A_Low)):
                    {
                        strErrorCode = "iqFullScaleCurrent_A too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_iqFullScaleVoltage_V_High)):
                    {
                        strErrorCode = "iqFullScaleVoltage_V too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_iqFullScaleVoltage_V_Low)):
                    {
                        strErrorCode = "iqFullScaleVoltage_V too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_iqFullScaleFreq_Hz_High)):
                    {
                        strErrorCode = "iqFullScaleFreq_Hz too high error code  ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_iqFullScaleFreq_Hz_Low)):
                    {
                        strErrorCode = "iqFullScaleFreq_Hz too low error code   ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numPwmTicksPerIsrTick_High)):
                    {
                        strErrorCode = "numPwmTicksPerIsrTick too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numPwmTicksPerIsrTick_Low)):
                    {
                        strErrorCode = "numPwmTicksPerIsrTick too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numIsrTicksPerCtrlTick_High)):
                    {
                        strErrorCode = "numIsrTicksPerCtrlTick too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numIsrTicksPerCtrlTick_Low)):
                    {
                        strErrorCode = "numIsrTicksPerCtrlTick too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerCurrentTick_High)):
                    {
                        strErrorCode = "numCtrlTicksPerCurrentTick too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerCurrentTick_Low)):
                    {
                        strErrorCode = "numCtrlTicksPerCurrentTick too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerEstTick_High)):
                    {
                        strErrorCode = "numCtrlTicksPerEstTick too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerEstTick_Low)):
                    {
                        strErrorCode = "numCtrlTicksPerEstTick too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerSpeedTick_High)):
                    {
                        strErrorCode = "numCtrlTicksPerSpeedTick too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerSpeedTick_Low)):
                    {
                        strErrorCode = "numCtrlTicksPerSpeedTick too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerTrajTick_High)):
                    {
                        strErrorCode = "numCtrlTicksPerTrajTick too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCtrlTicksPerTrajTick_Low)):
                    {
                        strErrorCode = "numCtrlTicksPerTrajTick too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCurrentSensors_High)):
                    {
                        strErrorCode = "numCurrentSensors too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numCurrentSensors_Low)):
                    {
                        strErrorCode = "numCurrentSensors too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numVoltageSensors_High)):
                    {
                        strErrorCode = "numVoltageSensors too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_numVoltageSensors_Low)):
                    {
                        strErrorCode = "numVoltageSensors too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_offsetPole_rps_High)):
                    {
                        strErrorCode = "offsetPole_rps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_offsetPole_rps_Low)):
                    {
                        strErrorCode = "offsetPole_rps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_fluxPole_rps_High)):
                    {
                        strErrorCode = "fluxPole_rps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_fluxPole_rps_Low)):
                    {
                        strErrorCode = "fluxPole_rps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_zeroSpeedLimit_High)):
                    {
                        strErrorCode = "zeroSpeedLimit too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_zeroSpeedLimit_Low)):
                    {
                        strErrorCode = "zeroSpeedLimit too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_forceAngleFreq_Hz_High)):
                    {
                        strErrorCode = "forceAngleFreq_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_forceAngleFreq_Hz_Low)):
                    {
                        strErrorCode = "forceAngleFreq_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxAccel_Hzps_High)):
                    {
                        strErrorCode = "maxAccel_Hzps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxAccel_Hzps_Low)):
                    {
                        strErrorCode = "maxAccel_Hzps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxAccel_est_Hzps_High)):
                    {
                        strErrorCode = "maxAccel_est_Hzps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxAccel_est_Hzps_Low)):
                    {
                        strErrorCode = "maxAccel_est_Hzps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_directionPole_rps_High)):
                    {
                        strErrorCode = "directionPole_rps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_directionPole_rps_Low)):
                    {
                        strErrorCode = "directionPole_rps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_speedPole_rps_High)):
                    {
                        strErrorCode = "speedPole_rps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_speedPole_rps_Low)):
                    {
                        strErrorCode = "speedPole_rps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_dcBusPole_rps_High)):
                    {
                        strErrorCode = "dcBusPole_rps too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_dcBusPole_rps_Low)):
                    {
                        strErrorCode = "dcBusPole_rps too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_fluxFraction_High)):
                    {
                        strErrorCode = "fluxFraction too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_fluxFraction_Low)):
                    {
                        strErrorCode = "fluxFraction too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_indEst_speedMaxFraction_High)):
                    {
                        strErrorCode = "indEst_speedMaxFraction too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_indEst_speedMaxFraction_Low)):
                    {
                        strErrorCode = "indEst_speedMaxFraction too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_powerWarpGain_High)):
                    {
                        strErrorCode = "powerWarpGain too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_powerWarpGain_Low)):
                    {
                        strErrorCode = "powerWarpGain too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_systemFreq_MHz_High)):
                    {
                        strErrorCode = "systemFreq_MHz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_systemFreq_MHz_Low)):
                    {
                        strErrorCode = "systemFreq_MHz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_pwmFreq_kHz_High)):
                    {
                        strErrorCode = "pwmFreq_kHz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_pwmFreq_kHz_Low)):
                    {
                        strErrorCode = "pwmFreq_kHz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_voltage_sf_High)):
                    {
                        strErrorCode = "voltage_sf too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_voltage_sf_Low)):
                    {
                        strErrorCode = "voltage_sf too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_current_sf_High)):
                    {
                        strErrorCode = "current_sf too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_current_sf_Low)):
                    {
                        strErrorCode = "current_sf too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_voltageFilterPole_Hz_High)):
                    {
                        strErrorCode = "voltageFilterPole_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_voltageFilterPole_Hz_Low)):
                    {
                        strErrorCode = "voltageFilterPole_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxVsMag_pu_High)):
                    {
                        strErrorCode = "maxVsMag_pu too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxVsMag_pu_Low)):
                    {
                        strErrorCode = "maxVsMag_pu too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_estKappa_High)):
                    {
                        strErrorCode = "estKappa too high error code  ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_estKappa_Low)):
                    {
                        strErrorCode = "estKappa too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_type_Unknown)):
                    {
                        strErrorCode = "motor type unknown error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_numPolePairs_High)):
                    {
                        strErrorCode = "motor_numPolePairs too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_numPolePairs_Low)):
                    {
                        strErrorCode = "motor_numPolePairs too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_ratedFlux_High)):
                    {
                        strErrorCode = "motor_ratedFlux too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_ratedFlux_Low)):
                    {
                        strErrorCode = "motor_ratedFlux too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Rr_High)):
                    {
                        strErrorCode = "motor_Rr too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Rr_Low)):
                    {
                        strErrorCode = "motor_Rr too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Rs_High)):
                    {
                        strErrorCode = "motor_Rs too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Rs_Low)):
                    {
                        strErrorCode = "motor_Rs too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Ls_d_High)):
                    {
                        strErrorCode = "motor_Ls_d too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Ls_d_Low)):
                    {
                        strErrorCode = "motor_Ls_d too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Ls_q_High)):
                    {
                        strErrorCode = "motor_Ls_q too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_motor_Ls_q_Low)):
                    {
                        strErrorCode = "motor_Ls_q too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrent_High)):
                    {
                        strErrorCode = "maxCurrent too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrent_Low)):
                    {
                        strErrorCode = "maxCurrent too low error code ";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrent_resEst_High)):
                    {
                        strErrorCode = "maxCurrent_resEst too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrent_resEst_Low)):
                    {
                        strErrorCode = "maxCurrent_resEst too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrent_indEst_High)):
                    {
                        strErrorCode = "maxCurrent_indEst too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrent_indEst_Low)):
                    {
                        strErrorCode = "maxCurrent_indEst too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrentSlope_High)):
                    {
                        strErrorCode = "maxCurrentSlope too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrentSlope_Low)):
                    {
                        strErrorCode = "maxCurrentSlope too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrentSlope_powerWarp_High)):
                    {
                        strErrorCode = "maxCurrentSlope_powerWarp too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxCurrentSlope_powerWarp_Low)):
                    {
                        strErrorCode = "maxCurrentSlope_powerWarp too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRated_High)):
                    {
                        strErrorCode = "IdRated too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRated_Low)):
                    {
                        strErrorCode = "IdRated too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRatedFraction_indEst_High)):
                    {
                        strErrorCode = "IdRatedFraction_indEst too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRatedFraction_indEst_Low)):
                    {
                        strErrorCode = "IdRatedFraction_indEst too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRatedFraction_ratedFlux_High)):
                    {
                        strErrorCode = "IdRatedFraction_ratedFlux too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRatedFraction_ratedFlux_Low)):
                    {
                        strErrorCode = "IdRatedFraction_ratedFlux too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRated_delta_High)):
                    {
                        strErrorCode = "IdRated_delta too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_IdRated_delta_Low)):
                    {
                        strErrorCode = "IdRated_delta too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_fluxEstFreq_Hz_High)):
                    {
                        strErrorCode = "fluxEstFreq_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_fluxEstFreq_Hz_Low)):
                    {
                        strErrorCode = "fluxEstFreq_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_ctrlFreq_Hz_High)):
                    {
                        strErrorCode = "ctrlFreq_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_ctrlFreq_Hz_Low)):
                    {
                        strErrorCode = "ctrlFreq_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_estFreq_Hz_High)):
                    {
                        strErrorCode = "estFreq_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_estFreq_Hz_Low)):
                    {
                        strErrorCode = "estFreq_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_RoverL_estFreq_Hz_High)):
                    {
                        strErrorCode = "RoverL_estFreq_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_RoverL_estFreq_Hz_Low)):
                    {
                        strErrorCode = "RoverL_estFreq_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_trajFreq_Hz_High)):
                    {
                        strErrorCode = "trajFreq_Hz too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_trajFreq_Hz_Low)):
                    {
                        strErrorCode = "trajFreq_Hz too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_ctrlPeriod_sec_High)):
                    {
                        strErrorCode = "ctrlPeriod_sec too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_ctrlPeriod_sec_Low)):
                    {
                        strErrorCode = "ctrlPeriod_sec too low error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxNegativeIdCurrent_a_High)):
                    {
                        strErrorCode = "maxNegativeIdCurrent_a too high error code";
                        break;
                    }
                case ((byte)(ErrorCodes.USER_ErrorCode_maxNegativeIdCurrent_a_Low)):
                    {
                        strErrorCode = "maxNegativeIdCurrent_a too low error code";
                        break;
                    }
                default:
                    {
                        strErrorCode = "No error code";
                        break;
                    }
            } //  switch(u8ErrorCode)

            return strErrorCode;
        } // static public string Error_CheckCode(byte u8ErrorCode)
    }


    /***************************************************************************
 * Class Name : CConversions
 ***************************************************************************
 * Notes         :
 * 
 ***************************************************************************/
    class CConversions
    {
        private static int HexToInt(char c)
        {
            switch (c)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'a':
                case 'A':
                    return 10;
                case 'b':
                case 'B':
                    return 11;
                case 'c':
                case 'C':
                    return 12;
                case 'd':
                case 'D':
                    return 13;
                case 'e':
                case 'E':
                    return 14;
                case 'f':
                case 'F':
                    return 15;
                default:
                    throw new FormatException("Unrecognized hex char " + c);
            }
        }


        private static readonly byte[,] ByteLookup = new byte[,]
        {
            // low nibble
            {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f},
            // high nibble
            {0x00, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90, 0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0}
        };

        public static byte[] ConvertHexToBytesX(string input)
        {
            var result = new byte[(input.Length + 1) >> 1];
            int lastcell = result.Length - 1;
            int lastchar = input.Length - 1;
            // count up in characters, but inside the loop will
            // reference from the end of the input/output.
            for (int i = 0; i < input.Length; i++)
            {
                // i >> 1    -  (i / 2) gives the result byte offset from the end
                // i & 1     -  1 if it is high-nibble, 0 for low-nibble.
                result[lastcell - (i >> 1)] |= ByteLookup[i & 1, HexToInt(input[lastchar - i])];
            }
            return result;

        }

        public static byte[] ConvertDoubleToByteArray(double d)

        {

            return BitConverter.GetBytes(d);

        }

        public static double ConvertByteArrayToDouble(byte[] b)

        {

            return BitConverter.ToDouble(b, 0);

        }

        public static string AsciiToHex(string asciiString)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in asciiString)
            {
                builder.Append(Convert.ToInt32(c).ToString("X"));
            }
            return builder.ToString();
        }

    } // Class CConversions
}
