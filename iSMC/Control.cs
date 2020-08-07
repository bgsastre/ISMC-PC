using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSMC
{
    class CControl
    {
        public enum CTRL_ST_FSM : byte
        {
            CTRL_ST_INIT = 0,
            CTRL_ST_CONFIG_ERROR = 1,
            CTRL_ST_DISABLED = 2,
            CTRL_ST_ENABLED = 3,
            CTRL_ST_CALC_OFFSET = 4,            
            CTRL_ST_RUN = 5,
            CTRL_ST_FAULT = 6,
            CTRL_ST_PRESTOP = 7,
            CTRL_ST_IDLE = 8,
        } // public enum ISMC_ST_IDENT : byte
        private Byte [] pu8ISMCStatus =  { (byte) CTRL_ST_FSM.CTRL_ST_IDLE, (byte)CTRL_ST_FSM.CTRL_ST_IDLE };




        // Comando de (des)activación del driver
        public enum CTRL_ST_DRIVER : byte
        {
            DRIVER_CMD_DISABLE = 0,
            DRIVER_CMD_ENABLE = 1
        }; // public enum CTRL_ST_DRIVER : byte
        private Byte [] pu8DriverCmd = { (byte)CTRL_ST_DRIVER.DRIVER_CMD_DISABLE, (byte)CTRL_ST_DRIVER.DRIVER_CMD_DISABLE };

        // Comando de (des)activación del control
        public enum CTRL_ST_CONTROL : byte
        {
            CTRL_CMD_DISABLE = 0,
            CTRL_CMD_ENABLE = 1
        }; // public enum CTRL_ST_CONTROL : byte
        private Byte [] pu8CtrlCmd = { (byte)CTRL_ST_CONTROL.CTRL_CMD_DISABLE, (byte) CTRL_ST_CONTROL.CTRL_CMD_DISABLE };

        // Comando de para determinar el modo del control
        public enum CTRL_ST_MODE : byte
        {
            CTRL_MODE_IDLE = 0,
            CTRL_MODE_TORQUE = 1,
            CTRL_MODE_SPEED = 2,
            CTRL_MODE_IDENT = 3,
            CTRL_MODE_ENC_SETUP = 4
        }; //  public enum CTRL_ST_MODE : byte
        private Byte[] pu8CtrlMode = { (byte) CTRL_ST_MODE.CTRL_MODE_SPEED, (byte)CTRL_ST_MODE.CTRL_MODE_SPEED };

        // Comando de para determinar el sentido de giro
        public enum CTRL_ST_SPIN_DIR : byte
        {
            CTRL_SPIN_DIR_CW = 0,
            CTRL_SPIN_DIR_CCW = 1
        }; // public enum CTRL_ST_SPIN_DIR : byte
        private Byte [] pu8SpinDir = { (byte)CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW, (byte)CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW };

        // Comando de para determinar si hay frenado regenerativo o no
        public enum CTRL_ST_REGEN_BRAKE : byte
        {
            CTRL_REGEN_BRAKE_OFF = 0,
            CTRL_REGEN_BRAKE_ON = 1
        } // public enum CTRL_ST_REGEN_BRAKE : byte
        private static byte [] pu8RegenBrake = { (byte)CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_ON, (byte)CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_ON };


        // Comando de para determinar si el freno está activo o no
        public enum CTRL_ST_BRAKE : byte
        {
            CTRL_BRAKE_ON = 0,
            CTRL_BRAKE_OFF = 1
        } // public enum CTRL_ST_REGEN_BRAKE : byte
        private static byte [] pu8Brake = { (byte)CTRL_ST_BRAKE.CTRL_BRAKE_ON, (byte)CTRL_ST_BRAKE.CTRL_BRAKE_ON };

        // Referencias de control (Par o Velocidad)
        private Int16 [] pi16SpeedRef  = { 0, 0 };
        private Int16 [] pi16TorqueRef = { 0, 0 };






        /***************************************************************************
         * Function Name : SetDriverCmd
         * Parameters    : u8Cmd     | Byte    | Comando de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         ***************************************************************************/
        public void SetISMCStatus(Byte u8CtrlId, Byte u8Status)
        {
            pu8ISMCStatus[u8CtrlId] = u8Status;
        }

        /***************************************************************************
         * Function Name : GetISMCStatus
         * Parameters    : None         
         * Returns       : u8Cmd     | Byte    | estado del ISMC
         ***************************************************************************
         * Notes         :         
         ***************************************************************************/
        public Byte GetISMCStatus(Byte u8CtrlId)
        {

            return pu8ISMCStatus[u8CtrlId];

        } // private static Byte  GetISMCStatus()



        /***************************************************************************
         * Function Name : SetDriverCmd
         * Parameters    : u8Cmd     | Byte    | Comando de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un comando de (des)activación a la variable que controla el switch
         * de activación del bus. Éste puede tomar dos valores:
         * BUS_CMD_IDLE ==> Bus desactivado
         * BUS_CMD_RUN  ==> Bus activado
         ***************************************************************************/
        public void SetDriverCmd(Byte u8CtrlId, Byte u8Cmd)
        {
            if (u8Cmd == (byte)CTRL_ST_DRIVER.DRIVER_CMD_DISABLE)
            {
                pu8DriverCmd[u8CtrlId] = u8Cmd;
            }
            else if (u8Cmd == (byte)CTRL_ST_DRIVER.DRIVER_CMD_ENABLE)
            {
                pu8DriverCmd[u8CtrlId] = u8Cmd;
            }
            else
            {
                pu8DriverCmd[u8CtrlId] = (byte)CTRL_ST_DRIVER.DRIVER_CMD_DISABLE;
            }
        } // private static void SetDriverCmd(Byte u8Cmd)

        /***************************************************************************
         * Function Name : GetDriverCmd
         * Parameters    : None         
         * Returns       : u8Cmd     | Byte    | estado de la activación
         ***************************************************************************
         * Notes         :
         * Devuelve el estado en que se encuentra la orden de activación del switch
         * que permite el paso del bus al inversor
         ***************************************************************************/
        public Byte GetDriverCmd(Byte u8CtrlId)
        {

            return pu8DriverCmd[u8CtrlId];

        } // private static Byte  GetDriverCmd()

        /***************************************************************************
         * Function Name : SetControlCmd
         * Parameters    : u8Cmd    | Byte    | Comando de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un comando de control a la variable de aplicación. Ésta puede
         * tomar dos valores:
         * CTRL_MODE_IDLE ==> Control desactivado
         * CTRL_CMD_RUN  ==> Control activado
         ***************************************************************************/
        public void Control_SetCmd(Byte u8CtrlId, Byte u8Cmd)
        {
            if (u8Cmd == (byte)CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                pu8CtrlCmd[u8CtrlId] = u8Cmd;
            }
            else if (u8Cmd == (byte)CTRL_ST_CONTROL.CTRL_CMD_ENABLE)
            {
                pu8CtrlCmd[u8CtrlId] = u8Cmd;
            }
            else
            {
                pu8CtrlCmd[u8CtrlId] = (byte)CTRL_ST_CONTROL.CTRL_CMD_DISABLE;
            }
        } // private static void SetControlCmd(Byte u8Cmd)

        /***************************************************************************
         * Function Name : Control_GetCmd
         * Parameters    : None
         * Returns       : u8Cmd    | Byte    | Comando de control
         ***************************************************************************
         * Notes         :
         * Devuelve el valor asignado a la variable que gestiona el comando de 
         * activación del control
         ***************************************************************************/
        public Byte Control_GetCmd(Byte u8CtrlId)
        {

            return pu8CtrlCmd[u8CtrlId];

        } // private static Byte  Control_GetCmd()

        /***************************************************************************
         * Function Name : SetControlCmd
         * Parameters    : u8Cmd    | Byte    | Comando de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un comando de control a la variable de tipo de control. Ésta puede
         * tomar tres valores:
         * CTRL_MODE_IDLE    ==> Control desactivado
         * CTRL_MODE_TORQUE  ==> Control activado en modo control de par
         * CTRL_MODE_SPEED   ==> Control activado en modo control de velocidad
         ***************************************************************************/
        public void SetMode(Byte u8CtrlId, Byte u8Mode)
        {
            if (u8Mode == (byte)CTRL_ST_MODE.CTRL_MODE_IDLE)
            {
                pu8CtrlMode[u8CtrlId] = u8Mode;
            }
            else if (u8Mode == (byte)CTRL_ST_MODE.CTRL_MODE_TORQUE)
            {
                pu8CtrlMode[u8CtrlId] = u8Mode;
            }
            else if (u8Mode == (byte)CTRL_ST_MODE.CTRL_MODE_SPEED)
            {
                pu8CtrlMode[u8CtrlId] = u8Mode;
            }
            else if (u8Mode == (byte)CTRL_ST_MODE.CTRL_MODE_IDENT)
            {
                pu8CtrlMode[u8CtrlId] = u8Mode;
            }
            else if (u8Mode == (byte)CTRL_ST_MODE.CTRL_MODE_ENC_SETUP)
            {
                pu8CtrlMode[u8CtrlId] = u8Mode;
            }
            else
            {
                pu8CtrlMode[u8CtrlId] = (byte)CTRL_ST_MODE.CTRL_MODE_IDLE;
            }
        } // private static void SetMode(Byte u8Mode)

        /***************************************************************************
         * Function Name : GetMode
         * Parameters    : None
         * Returns       : u8Cmd    | Byte    | Comando de control
         ***************************************************************************
         * Notes         :
         * Devuelve el valor asignado a la variable que gestiona el comando de 
         * control. Puede tomar tres posibles valores
         * CTRL_MODE_IDLE    ==> Control desactivado
         * CTRL_MODE_TORQUE  ==> Control activado en modo control de par
         * CTRL_MODE_SPEED   ==> Control activado en modo control de velocidad
         ***************************************************************************/
        public Byte GetMode(Byte u8CtrlId)
        {
            return pu8CtrlMode[u8CtrlId];
        } // private static Byte  GetControlType()

        /***************************************************************************
         * Function Name : SetSpeedRef
         * Parameters    : i16Speed    | Int16    | SpeedRef
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna el valor a la referencia de velocidad para el control de velocidad
         * del motor
         ***************************************************************************/
        public void SetSpeedRef(Byte u8CtrlId, Int16 i16Speed)
        {

            pi16SpeedRef[u8CtrlId] = i16Speed;

        } // private static void SetSpeedRef(Int16 i16Speed)

        /***************************************************************************
         * Function Name : SetSpeedRef
         * Parameters    : None
         * Returns       : i16Speed    | Int16    | SpeedRef
         ***************************************************************************
         * Notes         :
         * Devuelve el valor asignado a la referencia de velocidad para el control 
         * de velocidad del motor         
         ***************************************************************************/
        public Int16 GetSpeedRef(Byte u8CtrlId)
        {
            return pi16SpeedRef[u8CtrlId];
        } // private static Int16 GetSpeedRef()

        /***************************************************************************
         * Function Name : SetTorqueRef
         * Parameters    : i16Torque    | Int16    | Torque Reference
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna el valor a la referencia de par para el control de par del motor
         ***************************************************************************/
        public void SetTorqueRef(Byte u8CtrlId, Int16 i16Torque)
        {
            pi16TorqueRef[u8CtrlId] = i16Torque;
        } // public static void SetSpeedRef(Int16 i16Speed)

        /***************************************************************************
         * Function Name : GetTorqueRef
         * Parameters    : None
         * Returns       : i16Torque    | Int16    | Torque Reference
         ***************************************************************************
         * Notes         :
         * Delvuelve el valor asignado a la referencia de par para el control de par 
         * del motor
         ***************************************************************************/
        public Int16 GetTorqueRef(Byte u8CtrlId)
        {
            return pi16TorqueRef[u8CtrlId];
        } // public static Int16 GetTorqueRef()

        /***************************************************************************
         * Function Name : SetMotorCCW
         * Parameters    : byte   | Dirección de giro CCW o CW
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna el sentido de giro del motor
         ***************************************************************************/
        public void SetSpinDir(Byte u8CtrlId, byte u8Dir)
        {
            pu8SpinDir[u8CtrlId] = u8Dir;
        } // public static void SetSpinDir(byte u8Dir)

        /***************************************************************************
         * Function Name : GetSpinDir
         * Parameters    : None
         * Returns       : byte   | Dirección de giro CCW o CW
         ***************************************************************************
         * Notes         :
         * Devuelve el sentido de giro del motor
         ***************************************************************************/
        public byte GetSpinDir(Byte u8CtrlId)
        {
            return pu8SpinDir[u8CtrlId];
        } // public static byte GetSpinDir()

        /***************************************************************************
         * Function Name : SetRegenBrake
         * Parameters    : None
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Activa el frenado regenerativo
        ***************************************************************************/
        public void SetRegenBrake(Byte u8CtrlId)
        {
            pu8RegenBrake[u8CtrlId] = (byte)CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_ON;
        } // public static void SetMotorCCW(bool bCCW)

        /***************************************************************************
         * Function Name : ClrRegenBrake
         * Parameters    : None
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Desactiva el frenado regenerativo
        ***************************************************************************/
        public void ClrRegenBrake(Byte u8CtrlId)
        {
            pu8RegenBrake[u8CtrlId] = (byte)CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_OFF;
        } // public static void SetRegenBrake()

        /***************************************************************************
         * Function Name : GetRegenBrake
         * Parameters    : None
         * Returns       : bCCW   | bool    | True, CCW; False, CW.
         ***************************************************************************
         * Notes         :
         * Devuelve el estado del flag de frenado regenerativo
         ***************************************************************************/
        public byte GetRegenBrake(Byte u8CtrlId)
        {
            return pu8RegenBrake[u8CtrlId];
        } // public static bool GetMotorCCW()


        /***************************************************************************
         * Function Name : SetBrake
         * Parameters    : None
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Activa el flag de freno
        ***************************************************************************/
        public void SetBrake(Byte u8CtrlId)
        {
            pu8Brake[u8CtrlId] = (byte)CTRL_ST_BRAKE.CTRL_BRAKE_ON;
        } // public static void SetBrake(bool bCCW)

        /***************************************************************************
         * Function Name : ClrBrake
         * Parameters    : None
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Desactiva el flag de freno
        ***************************************************************************/
        public void ClrBrake(Byte u8CtrlId)
        {
            pu8Brake[u8CtrlId] = (byte)CTRL_ST_BRAKE.CTRL_BRAKE_OFF;
        } // public static void ClrBrake()

        /***************************************************************************
         * Function Name : GetBrake
         * Parameters    : None
         * Returns       : bCCW   | bool    | True / False
         ***************************************************************************
         * Notes         :
         * Devuelve el estado del flag de freno
         ***************************************************************************/
        public byte GetBrake(Byte u8CtrlId)
        {
            return pu8Brake[u8CtrlId];
        } // public static bool GetBrake()


    } // class CControl
}
