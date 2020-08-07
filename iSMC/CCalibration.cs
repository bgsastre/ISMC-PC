using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSMC
{
    /***************************************************************************
     * Class Name : CCalibration
     ***************************************************************************
     * Notes         :
     * 
     ***************************************************************************/
    class CCalibration
    {
        public const Byte CTRL_CMD_IDLE = 0;
        public const Byte CALIB_CMD_ERROR = 0x00;
        public const Byte CALIB_CMD_OK = 0x01;


        public const Byte CALIB_MSG_SOF = 0x01;
        public const Byte CALIB_MSG_COF = 0x02;
        public const Byte CALIB_MSG_EOF = 0x03;

        // Operaciones RD/WR de versión de Software
        public const Byte CALIB_CMD_GET_SW_VERSION = 0xA0;
        public const Byte CALIB_CMD_SET_SW_VERSION = 0xA1;
        public const Byte CALIB_CMD_RD_SW_VERSION = 0xA2;
        public const Byte CALIB_CMD_WR_SW_VERSION = 0xA3;
        public const Byte CALIB_CMD_MEM_TEST = 0xA4;
        public const Byte CALIB_CMD_MEM_SET_DEFAULT = 0xA5;
        public const Byte CALIB_CMD_MEM_GET_DEFAULT = 0xA6;


        public const Byte CALIB_CMD_IDENT_COMP = 0xB0;

        // Operaciones RD/WR de parámetros con el FW
        public const Byte CALIB_CMD_GET_MOTOR_PARAM = 0x40;
        public const Byte CALIB_CMD_GET_ENC_PARAM = 0x41;
        public const Byte CALIB_CMD_SET_MOTOR_PARAM = 0x30;
        public const Byte CALIB_CMD_SET_ENC_PARAM = 0x31;
        public const Byte CALIB_CMD_WR_PARAM = 0x50;
        public const Byte CALIB_CMD_WR_ENC_PARAM = 0x51;
        public const Byte CALIB_CMD_RD_PARAM = 0x60;
        public const Byte CALIB_CMD_RD_ENC_PARAM = 0x61;

        public const Byte CALIB_CMD_WRITE_DATA = 0x60;
        public const Byte CALIB_CMD_READ_DATA = 0x61;
        public const Byte CALIB_CMD_GET_PARAM = 0x62;
        public const Byte CALIB_CMD_SET_PARAM = 0x63;
        public const Byte CCALIB_CMD_PUSH_EEPROM = 0x64;
        public const Byte CCALIB_CMD_POP_EEPROM = 0x65;
        public const Byte CCALIB_CMD_SAVE_IMAGE = 0x66;
        public const Byte CCALIB_CMD_READ_IMAGE = 0x67;

        public const Byte CALIB_CMD_GET_EST_STATUS = 0x72;

        public const Byte CALIB_CMD_IDENT_START = 0x80;
        public const Byte CALIB_CMD_IDENT_ABORT = 0x81;


        //public const Byte CALIB_CMD_SET_ACK         = 0x80;

        // Parámetros de calibración del motor
        public enum CALIB_PARAMS : byte
        {
            CALIB_PARAM_FW_VERSION = 0,
            CALIB_PARAM_MOTOR_POLE_PAIRS = 10,
            CALIB_PARAM_MOTOR_RS = 11,
            CALIB_PARAM_MOTOR_LD = 12,
            CALIB_PARAM_MOTOR_LQ = 13,
            CALIB_PARAM_MOTOR_RATED_FLUX = 14,
            CALIB_PARAM_MOTOR_INERTIA = 15,
            CALIB_PARAM_MOTOR_FRICTION = 16,
            CALIB_PARAM_MOTOR_RATED_CURRENT = 17,
            CALIB_PARAM_MOTOR_MAX_CURRENT = 18,
            CALIB_PARAM_MOTOR_MAX_SPEED_KRPM = 19,
            CALIB_PARAM_MOTOR_SENSOR = 20,
            CALIB_PARAM_MOTOR_SENSOR_RESOLUTION = 21,

            CALIB_PARAM_MOTOR_NAME_1_4 = 22,
            CALIB_PARAM_MOTOR_NAME_2_4 = 23,
            CALIB_PARAM_MOTOR_NAME_3_4 = 24,
            CALIB_PARAM_MOTOR_NAME_4_4 = 25,



            CALIB_PARAM_MOTOR_MAX_ACCEL = 30,
            CALIB_PARAM_MOTOR_BWIDTH = 31,

            CALIB_PARAM_MOTOR_SETUP = 32,
            CALIB_PARAM_MOTOR_ERROR = 33,
            CALIB_PARAM_CTRL_SPEED_KP = 36,
            CALIB_PARAM_CTRL_SPEED_KI = 37,
            CALIB_PARAM_CTRL_IDQ_KP = 38,
            CALIB_PARAM_CTRL_IDQ_KI = 39,
            CALIB_PARAM_CTRL_BLDC_KP = 40,
            CALIB_PARAM_CTRL_BLDC_KI = 41,
            CALIB_PARAM_EEPROM_CHECK = 42,

            CALIB_PARAM_GET_ERROR = 43,

            CALIB_PARAM_CTRL_BANDWIDTH = 49,

            // LIMITS
            CALIB_PARAM_LIMITS_MAX_CURRENT      = 50,
            CALIB_PARAM_LIMITS_AVG_CURRENT      = 51,
            CALIB_PARAM_LIMITS_TIME_AVG_CURRENT = 52,
            CALIB_PARAM_LIMITS_MAX_SPEED_KRPM   = 53,
            CALIB_PARAM_LIMITS_TIME_SPEED       = 54,
            CALIB_PARAM_LIMITS_TEMP_MAX         = 55,
            CALIB_PARAM_LIMITS_HEARTBEAT_COMMS  = 56,
            CALIB_PARAM_LIMITS_TIMEOUTS_COMMS   = 57,
            CALIB_PARAM_LIMITS_MAX_ACCEL        = 58,
            



            CALIB_PARAM_CONFIG_FULL_SCALE_FREQ = 70,
            CALIB_PARAM_CONFIG_RES_IMAX = 71,
            CALIB_PARAM_CONFIG_IND_IMAX = 72,
            CALIB_PARAM_CONFIG_FLUX_EST_FREQ = 73,
            CALIB_PARAM_CONFIG_ROVERL_FREQ = 74

        };



        public enum CALIB_IDENT_STATUS : byte
        {
            CALIB_IDENT_ST_ERROR = 0,  // error
            CALIB_IDENT_ST_IDLE = 1,  // idle
            CALIB_IDENT_ST_ROVERL = 2,  // R/L estimation
            CALIB_IDENT_ST_RS = 3,  // Rs estimation state
            CALIB_IDENT_ST_RAMPUP = 4,  // ramp up the speed
            CALIB_IDENT_ST_IDRATED = 5,  // control Id and estimate the rated flux
            CALIB_IDENT_ST_RATEDFLUXOL = 6,  // estimate the open loop rated flux
            CALIB_IDENT_ST_RATEDFLUX = 7,  // estimate the rated flux 
            CALIB_IDENT_ST_RAMPDOWN = 8,  // ramp down the speed 
            CALIB_IDENT_ST_LOCKROTOR = 9,  // lock the rotor
            CALIB_IDENT_ST_LS = 10, // stator inductance estimation state
            CALIB_IDENT_ST_RR = 11, // rotor resistance estimation state
            CALIB_IDENT_ST_MOTORIDENTIFIED = 12, // motor identified state
            CALIB_IDENT_ST_ONLINE = 13  // online parameter estimation

        };

        static private byte[] pu8FWVersion = { 0x00, 0x00 };
        static private byte[] pu8FWSubversion = { 0x00, 0x00 };
        static private byte[] pu8FWVariant = { 0x00, 0x00 };
        // Pares de polos del motor
        private static Byte[] pu8PolePairs = { 0, 0 };
        // Resistencia del stator [ohm]
        private static double[] pdResistance = { 0.0, 0.0 };
        // Inductancioa del stator [H]
        private static double[] pdInductance = { 0.0, 0.0 };
        // Flujo de los imanes permanentes [V/Hz]
        private static double[] pdRatedFlux = { 0.0, 0.0 };
        // Inercia del sistema
        private static double[] pdInertia = { 0.0, 0.0 };
        // Fricción del sistema
        private static double[] pdFriction = { 0.0, 0.0 };
        // Flujo de los imanes permanentes [Vs/rad]
        private static double[] pdFluxLinkage = { 0.0, 0.0 };
        // Constante de par del motor [NM/Apeak]
        private static double[] pdTorqueConstant = { 0.0, 0.0 };
        // Corriente nominal para el motor [A]
        private static double[] pdRatedCurrent = { 0.0, 0.0 };
        // Máxima corriente para el motor [A]
        private static double[] pdMaxCurrent = { 0.0, 0.0 };
        // Máxima velocidad del motor [KRPM]
        private static double[] pdMaxSpeed = { 0.0, 0.0 };
        // Tipo de sensor
        private static byte[] pu8Sensor = { 0, 0 };
        // Resolución del sensor
        private static UInt16[] pu16Resolution = { 0, 0 };
        // Máxima aceleración para el motor [KRPM/s]
        private static double[] pdMaxAccel = { 0.0, 0.0 };


        private static double[] pdSpdCtrlKp = { 0.0, 0.0 };

        private static double[] pdSpdCtrlKi = { 0.0, 0.0 };

        private static double[] pdIdqCtrlKp = { 0.0, 0.0 };

        private static double[] pdIdqCtrlKi = { 0.0, 0.0 };

        private static double[] pdCtrlBandwidth = { 0.0, 0.0 };


        private static double[] pdLimitImax = { 0.0, 0.0 };
        private static double[] pdLimitIavg = { 0.0, 0.0 };
        private static UInt32[] pu32LimitTimeIavg = { 0, 0 };
        private static double[] pdLimitSpeedAvg = { 0.0, 0.0 };
        private static UInt32[] pu32LimitTimeSpeedAvg = { 0, 0 };
        private static double[] pdLimitTempMax = { 0.0, 0.0 };
        private static UInt32[] pu32LimitTimeHeartBeat = { 0, 0 };
        private static UInt32[] pu32LimitTimeComms = { 0, 0 };
        private static double[] pdLimitAccel = { 0.0, 0.0 };


        // Pares de polos del motor
        private static Byte[] pu8IdentPolePairs = { 0, 0 };
        // Resistencia del stator [ohm]
        private static double[] pdIdentResistance = { 0.0, 0.0 };
        // Inductancioa del stator [H]
        private static double[] pdIdentInductance = { 0.0, 0.0 };
        // Flujo de los imanes permanentes [V/Hz]
        private static double[] pdIdentRatedFlux = { 0.0, 0.0 };
        // Flujo de los imanes permanentes [Vs/rad]
        private static double[] pdIdentFluxLinkage = { 0.0, 0.0 };
        // Constante de par del motor [NM/Apeak]
        private static double[] pdIdentTorqueConstant = { 0.0, 0.0 };

        private static string[] psMotorName = { "", "" };




        static private byte u8ECUSelected = Globals.ECU_ISMC_B;


        /*******************************************************************************
         * Function Name : GetECUSel
         * Parameters    : None
         * Returns       : u8ECUId    | byte    | Version de ECU
         *******************************************************************************
         * Notes         :
         * Devuelve la ECU que está seleccionada
         *******************************************************************************/
        static public byte GetECUSel()
        {
            return u8ECUSelected;
        } // static public bool GetECUSel()

        /*******************************************************************************
         * Function Name : SetFWVersion
         * Parameters    : u8ECUId      | byte    | ECU Identifier
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Selecciona una de las dos ECUS controlables
         *******************************************************************************/
        static public void SetECUSel(byte u8ECUId)
        {
            u8ECUSelected = u8ECUId;
        } // static public void SetECUSel(byte u8Version)


        /*******************************************************************************
         * Function Name : GetFWVersion
         * Parameters    : u8ECUId      | byte    | ECU Identifier
         * Returns       : u8Version    | byte    | Version de Firmware
         *******************************************************************************
         * Notes         :
         * Devuelve la versión de firmware
         *******************************************************************************/
        static public byte GetFWVersion(byte u8ECUId)
        {
            return pu8FWVersion[u8ECUId];
        } // static public bool GetFWVersion()

        /*******************************************************************************
         * Function Name : SetFWVersion
         * Parameters    : u8ECUId      | byte    | ECU Identifier
         *               : u8Version    | byte    | Version de Firmware
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Asigna la versión de firmware
         *******************************************************************************/
        static public void SetFWVersion(byte u8ECUId, byte u8Version)
        {
            pu8FWVersion[u8ECUId] = u8Version;
        } // static public void SetFWVersion(byte u8Version)

        /*******************************************************************************
         * Function Name : GetFWSubversion
         * Parameters    : u8ECUId      | byte    | ECU Identifier
         * Returns       : u8Subversion | byte    | Subversion de Firmware
         *******************************************************************************
         * Notes         :
         * Devuelve la subversión de firmware
         *******************************************************************************/
        static public byte GetFWSubversion(byte u8ECUId)
        {
            return pu8FWSubversion[u8ECUId];
        } // static public bool GetFWSubversion()

        /*******************************************************************************
         * Function Name : SetFWSubversion
         * Parameters    : u8ECUId      | byte    | ECU Identifier
         *               : u8Subversion | byte    | Subversion de Firmware
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Asigna la Subversión de firmware
         *******************************************************************************/
        static public void SetFWSubversion(byte u8ECUId, byte u8Subversion)
        {
            pu8FWSubversion[u8ECUId] = u8Subversion;
        } // static public void SetFWSubversion(byte u8Subversion)

        /*******************************************************************************
         * Function Name : GetFWVariant
         * Parameters    : u8ECUId      | byte    | ECU Identifier
         * Returns       : u8Variant    | byte    | Variant de Firmware
         *******************************************************************************
         * Notes         :
         * Devuelve la variante de firmware
         *******************************************************************************/
        static public byte GetFWVariant(byte u8ECUId)
        {
            return pu8FWVariant[u8ECUId];
        } // static public bool GetFWVariant()

        /*******************************************************************************
         * Function Name : SetFWVariant
         * Parameters    : u8ECUId     | byte    | ECU Identifier
         *               : u8Variant   | byte    | Variant de Firmware
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Asigna la variante de firmware
         *******************************************************************************/
        static public void SetFWVariant(byte u8ECUId, byte u8Variant)
        {
            pu8FWVariant[u8ECUId] = u8Variant;
        } // static public void SetFWVariant(byte u8Variant)

        /***************************************************************************
         * Function Name : SetPolePairs
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : u8PolePairs | Byte    | Número de pares de poles
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al número de pares de polos
         ***************************************************************************/
        public static void SetPolePairs(Byte u8MotorId, Byte u8PolePairs)
        {
            pu8PolePairs[u8MotorId] = u8PolePairs;
        } // public static void SetPolePairs(Byte u8MotorId, Byte u8PolePairs)

        /***************************************************************************
         * Function Name : GetPolePairs
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : u8PolePairs | Byte    | Comando de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de número de pares de polos
         ***************************************************************************/
        public static byte GetPolePairs(Byte u8MotorId)
        {
            return pu8PolePairs[u8MotorId];
        } // public static byte GetPolePairs(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetResistance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dResistance | double  | Valor de la resistencia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la resistencia del stator
         ***************************************************************************/
        public static void SetResistance(Byte u8MotorId, double dResistance)
        {
            pdResistance[u8MotorId] = dResistance;
        } // public static void SetResistance(Byte u8MotorId, double dResistance)

        /***************************************************************************
         * Function Name : GetResistance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dResistance | double  | Valor de la resistencia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la resistencia del stator
         ***************************************************************************/
        public static double GetResistance(Byte u8MotorId)
        {
            return pdResistance[u8MotorId];
        } // public static double GetResistance(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetInductance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dInductance | double  | Valor de la Inductancia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la Inductancia del stator
         ***************************************************************************/
        public static void SetInductance(Byte u8MotorId, double dInductance)
        {
            pdInductance[u8MotorId] = dInductance;
        } // public static void SetInductance(Byte u8MotorId, double dInductance)

        /***************************************************************************
         * Function Name : GetInductance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dInductance | double  | Valor de la Inductancia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la Inductancia del stator
         ***************************************************************************/
        public static double GetInductance(Byte u8MotorId)
        {
            return pdInductance[u8MotorId];
        } // public static double GetInductance(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetRatedFlux
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dRatedFlux  | double  | Valor del Flujo
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al Flujo del rotor
         ***************************************************************************/
        public static void SetRatedFlux(Byte u8MotorId, double dRatedFlux)
        {
            byte u8PolePairs = GetPolePairs(u8MotorId);
            pdRatedFlux[u8MotorId] = dRatedFlux;
            pdFluxLinkage[u8MotorId] = dRatedFlux / (2 * Math.PI);
            pdTorqueConstant[u8MotorId] = dRatedFlux / (2 * Math.PI) * (2 * u8PolePairs) * (3 / 4);

        } // public static void SetRatedFlux(Byte u8MotorId, double dRatedFlux)

        /***************************************************************************
         * Function Name : GetRatedFlux
         * Parameters    : u8MotorId  | Byte    | Motor Id
         *               : dRatedFlux | double  | Valor del Flujo
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor del flujo del  rotor
         ***************************************************************************/
        public static double GetRatedFlux(Byte u8MotorId)
        {
            return pdRatedFlux[u8MotorId];
        } // public static double GetRatedFlux(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetInertia
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dValue      | double  | Valor de la inercia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la Inercia
         ***************************************************************************/
        public static void SetInertia(Byte u8MotorId, double dValue)
        {
            pdInertia[u8MotorId] = dValue;
        } // public static void SetInertia(Byte u8MotorId, double dInductance)

        /***************************************************************************
         * Function Name : GetInertia
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dInertia    | double  | Valor de la Inercoa
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la Inductancia del stator
         ***************************************************************************/
        public static double GetInertia(Byte u8MotorId)
        {
            return pdInertia[u8MotorId];
        } // public static double GetInertia(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetFriction
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dValue      | double  | Valor de la fricción
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la Fricción
         ***************************************************************************/
        public static void SetFriction(Byte u8MotorId, double dValue)
        {
            pdFriction[u8MotorId] = dValue;
        } // public static void SetInertia(Byte u8MotorId, double dInductance)

        /***************************************************************************
         * Function Name : GetInertia
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dInertia    | double  | Valor de la Fricción
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la fricción
         ***************************************************************************/
        public static double GetFriction(Byte u8MotorId)
        {
            return pdFriction[u8MotorId];
        } // public static double GetInertia(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetRatedCurrent
         * Parameters    : u8MotorId   | Byte      | Motor Id
         *               : dRatedCurrent | double  | Valor de corriente [A]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la corriente nominal por el motor
         ***************************************************************************/
        public static void SetRatedCurrent(Byte u8MotorId, double dValue)
        {
            pdRatedCurrent[u8MotorId] = dValue;
        } // public static void SetRatedCurrent(Byte u8MotorId, double dMaxCurrent)

        /***************************************************************************
         * Function Name : GetRatedCurrent
         * Parameters    : u8MotorId   | Byte    | Motor Id
         * Returns       : dMaxCurrent | double  | Valor de corriente [A]
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la corriente nominal por el motor
         ***************************************************************************/
        public static double GetRatedCurrent(Byte u8MotorId)
        {
            return pdRatedCurrent[u8MotorId];
        } // public static double GetRatedCurrent(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetMaxCurrent
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dMaxCurrent | double  | Valor de corriente [A]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la máxima corriente por el motor
         ***************************************************************************/
        public static void SetMaxCurrent(Byte u8MotorId, double dMaxCurrent)
        {
            pdMaxCurrent[u8MotorId] = dMaxCurrent;
        } // public static void SetMaxCurrent(Byte u8MotorId, double dMaxCurrent)

        /***************************************************************************
         * Function Name : GetMaxCurrent
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dMaxCurrent | double  | Valor de corriente [A]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la máxima corriente por el motor
         ***************************************************************************/
        public static double GetMaxCurrent(Byte u8MotorId)
        {
            return pdMaxCurrent[u8MotorId];
        } // public static double GetMaxCurrent(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetMaxSpeed
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dMaxSpeed | double  | Valor de velocidad [KRPM]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la máxima velocidad del motor
         ***************************************************************************/
        public static void SetMaxSpeed(Byte u8MotorId, double dMaxSpeed)
        {
            pdMaxSpeed[u8MotorId] = dMaxSpeed;
        } // public static void SetMaxSpeed(Byte u8MotorId, double dMaxSpeed)

        /***************************************************************************
         * Function Name : GetMaxSpeed
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dMaxSpeed | double  | Valor de Valor de velocidad [KRPM]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la máxima velocidad del motor
         ***************************************************************************/
        public static double GetMaxSpeed(Byte u8MotorId)
        {
            return pdMaxSpeed[u8MotorId];
        } // public static double GetMaxSpeed(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetPolePairs
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : u8Sensor    | Byte    | Tipo de sensor
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al tipo de sensor
         ***************************************************************************/
        public static void SetSensor(Byte u8MotorId, Byte u8Sensor)
        {
            pu8Sensor[u8MotorId] = u8Sensor;
        } // public static void SetSensor(Byte u8MotorId, Byte u8Sensor)

        /***************************************************************************
         * Function Name : GetPolePairs
         * Parameters    : u8MotorId   | Byte    | Motor Id              
         * Returns       : u8Sensor    | Byte    | Sensot
         ***************************************************************************
         * Notes         :
         * Devuelve el tipo de sensor
         ***************************************************************************/
        public static byte GetSensor(Byte u8MotorId)
        {
            return pu8Sensor[u8MotorId];
        } // public static byte GetSensor(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetResolution
         * Parameters    : u8MotorId       | Byte    | Motor Id
         *               : u8Resolution    | UIny16  | Resolution
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al Resolution de sensor
         ***************************************************************************/
        public static void SetResolution(Byte u8MotorId, UInt16 u16Resolution)
        {
            pu16Resolution[u8MotorId] = u16Resolution;
        } // public static void SetResolution(Byte u8MotorId, Byte u8Resolution)

        /***************************************************************************
         * Function Name : GetResolution
         * Parameters    : u8MotorId       | Byte    | Motor Id              
         * Returns       : u8Resolution    | UIny16  | Resolution
         ***************************************************************************
         * Notes         :
         * Devuelve la Resolution del sensor
         ***************************************************************************/
        public static UInt16 GetResolution(Byte u8MotorId)
        {
            return pu16Resolution[u8MotorId];
        } // public static byte GetResolution(Byte u8MotorId)


        /***************************************************************************
         * Function Name : SetLimitParam
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : u8Param   | Byte    | Parámetro
         *               : dValue    | double  | Valor asignar
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * 
         ***************************************************************************/
        public static void SetLimitParam(Byte u8MotorId, byte u8Param, double dValue)
        {
            if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT))
            {
                pdLimitImax[u8MotorId] = dValue;
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT))
                    {
                        pdLimitIavg[u8MotorId] = dValue;
                    }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT))
                    {
                        pu32LimitTimeIavg[u8MotorId] = Convert.ToUInt32(dValue);                         
                    }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM))
                    {
                        pdLimitSpeedAvg[u8MotorId] = dValue;                        
                    }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED))
                   {
                        pu32LimitTimeSpeedAvg[u8MotorId] = Convert.ToUInt32(dValue);                         
                   }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX))
                   {
                        pdLimitTempMax[u8MotorId] = dValue;                        
                   }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS))
                   {
                       pu32LimitTimeHeartBeat[u8MotorId] = Convert.ToUInt32(dValue);                        
                   }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS))
                   {
                        pu32LimitTimeComms[u8MotorId] = Convert.ToUInt32(dValue);
                   }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL))
                   {
                        pdLimitAccel[u8MotorId] = dValue;
                   }

        } // public static void SetLimitParam(Byte u8MotorId, byte u8Param, double dValue)

        /***************************************************************************
         * Function Name : GetMaxAccel
         * Parameters    : u8MotorId | Byte    | Motor Id
         * Returns       :           | double  | Valor del parámetro  
         ***************************************************************************
         * Notes         :
         ***************************************************************************/
        public static double GetLimitParam(Byte u8MotorId, byte u8Param)
        {
            double dValue = 0.0;
            UInt32 u32Value = 0;
            if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT))
            {
                dValue = pdLimitImax[u8MotorId];
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT))
            {
                dValue = pdLimitIavg[u8MotorId];
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT))
            {
                u32Value = pu32LimitTimeIavg[u8MotorId];
                dValue = Convert.ToDouble(u32Value);
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM))
            {
                dValue = pdLimitSpeedAvg[u8MotorId];
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED))
            {
                u32Value = pu32LimitTimeSpeedAvg[u8MotorId];
                dValue = Convert.ToDouble(u32Value);
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX))
            {
                dValue = pdLimitTempMax[u8MotorId];
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS))
            {
                u32Value = pu32LimitTimeHeartBeat[u8MotorId];
                dValue = Convert.ToDouble(u32Value);
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS))
            {
                u32Value = pu32LimitTimeComms[u8MotorId];
                dValue = Convert.ToDouble(u32Value);
            }
            else if (u8Param == Convert.ToByte(CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL))
            {
                dValue = pdLimitAccel[u8MotorId];
            }

            return dValue;
        } // public static double GetLimitImax(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetMaxAccel
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dMaxAccel | double  | Valor de aceleración [KRPM/S]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la máxima aceleración para el motor
         ***************************************************************************/
        public static void SetMaxAccel(Byte u8MotorId, double dMaxAccel)
        {
            pdMaxAccel[u8MotorId] = dMaxAccel;
        } // public static void SetMaxAccel(Byte u8MotorId, double dMaxAccel)

        /***************************************************************************
         * Function Name : GetMaxAccel
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dMaxAccel | double  | Valor de aceleración [KRPM/S]
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la máxima aceleración para el motor
         ***************************************************************************/
        public static double GetMaxAccel(Byte u8MotorId)
        {
            return pdMaxAccel[u8MotorId];
        } // public static double GetMaxAccel(Byte u8MotorId)
    


        /***************************************************************************
         * Function Name : SetSpdCtrlKp
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dValue    | double  | Valor de la constante de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la constante de control
         ***************************************************************************/
        public static void SetSpdCtrlKp(Byte u8MotorId, double dValue)
        {
            pdSpdCtrlKp[u8MotorId] = dValue;
        } // public static void SetSpdCtrlKp(Byte u8MotorId, double dValue)

        /***************************************************************************
         * Function Name : GetSpdCtrlKp
         * Parameters    : u8MotorId | Byte    | Motor Id
         * Returns       : dValue    | double  | Valor de la constante de control
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la constante de control
         ***************************************************************************/
        public static double GetSpdCtrlKp(Byte u8MotorId)
        {
            return pdSpdCtrlKp[u8MotorId];
        } // public static double GetSpdCtrlKp(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetSpdCtrlKi
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dValue    | double  | Valor de la constante de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la constante de control
         ***************************************************************************/
        public static void SetSpdCtrlKi(Byte u8MotorId, double dValue)
        {
            pdSpdCtrlKi[u8MotorId] = dValue;
        } // public static void SetSpdCtrlKi(Byte u8MotorId, double dValue)

        /***************************************************************************
         * Function Name : GetSpdCtrlKi
         * Parameters    : u8MotorId | Byte    | Motor Id
         * Returns       : dValue    | double  | Valor de la constante de control
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la constante de control
         ***************************************************************************/
        public static double GetSpdCtrlKi(Byte u8MotorId)
        {
            return pdSpdCtrlKi[u8MotorId];
        } // public static double GetSpdCtrlKp(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetIdqCtrlKp
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dValue    | double  | Valor de la constante de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la constante de control
         ***************************************************************************/
        public static void SetIdqCtrlKp(Byte u8MotorId, double dValue)
        {
            pdIdqCtrlKp[u8MotorId] = dValue;
        } // public static void SetIdqCtrlKp(Byte u8MotorId, double dValue)

        /***************************************************************************
         * Function Name : GetIdqCtrlKp
         * Parameters    : u8MotorId | Byte    | Motor Id
         * Returns       : dValue    | double  | Valor de la constante de control
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la constante de control
         ***************************************************************************/
        public static double GetIdqCtrlKp(Byte u8MotorId)
        {
            return pdIdqCtrlKp[u8MotorId];
        } // public static double GetIdqCtrlKp(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetIdqCtrlKi
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dValue    | double  | Valor de la constante de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la constante de control
         ***************************************************************************/
        public static void SetIdqCtrlKi(Byte u8MotorId, double dValue)
        {
            pdIdqCtrlKi[u8MotorId] = dValue;
        } // public static void SetIdqCtrlKi(Byte u8MotorId, double dValue)

        /***************************************************************************
         * Function Name : GetIdqCtrlKi
         * Parameters    : u8MotorId | Byte    | Motor Id
         * Returns       : dValue    | double  | Valor de la constante de control
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la constante de control
         ***************************************************************************/
        public static double GetIdqCtrlKi(Byte u8MotorId)
        {
            return pdIdqCtrlKi[u8MotorId];
        } // public static double GetIdqCtrlKi(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetCtrlBandwidth
         * Parameters    : u8MotorId | Byte    | Motor Id
         *               : dValue    | double  | Valor del ancho de banda
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al ancho de banda del control
         ***************************************************************************/
        public static void SetCtrlBandwidth(Byte u8MotorId, double dValue)
        {
            pdCtrlBandwidth[u8MotorId] = dValue;
        } // public static void SetCtrlBandwidth(Byte u8MotorId, double dValue)

        /***************************************************************************
         * Function Name : GetCtrlBandwidth
         * Parameters    : u8MotorId | Byte    | Motor Id
         * Returns       : dValue    | double  | Valor del ancho de banda
         ***************************************************************************
         * Notes         :
         * Devuelve el valor del ancho de banda del control
         ***************************************************************************/
        public static double GetCtrlBandwidth(Byte u8MotorId)
        {
            return pdCtrlBandwidth[u8MotorId];
        } // public static double GetCtrlBandwidth(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetIdentPolePairs
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : u8PolePairs | Byte    | Número de pares de poles
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al número de pares de polos
         ***************************************************************************/
        public static void SetIdentPolePairs(Byte u8MotorId, Byte u8PolePairs)
        {
            pu8IdentPolePairs[u8MotorId] = u8PolePairs;
        } // public static void SetIdentPolePairs(Byte u8MotorId, Byte u8PolePairs)

        /***************************************************************************
         * Function Name : GetIdentPolePairs
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : u8PolePairs | Byte    | Comando de control
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de número de pares de polos
         ***************************************************************************/
        public static byte GetIdentPolePairs(Byte u8MotorId)
        {
            return pu8IdentPolePairs[u8MotorId];
        } // public static byte GetIdentPolePairs(Byte u8MotorId)


        /***************************************************************************
         * Function Name : SetIdentResistance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dResistance | double  | Valor de la resistencia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la resistencia del stator
         ***************************************************************************/
        public static void SetIdentResistance(Byte u8MotorId, double dResistance)
        {
            pdIdentResistance[u8MotorId] = dResistance;
        } // public static void SetIdentResistance(Byte u8MotorId, double dResistance)

        /***************************************************************************
         * Function Name : GetResistance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dResistance | double  | Valor de la resistencia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la resistencia del stator
         ***************************************************************************/
        public static double GetIdentResistance(Byte u8MotorId)
        {
            return pdIdentResistance[u8MotorId];
        } // public static double GetIdentResistance(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetIdentInductance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dInductance | double  | Valor de la Inductancia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor a la Inductancia del stator
         ***************************************************************************/
        public static void SetIdentInductance(Byte u8MotorId, double dInductance)
        {
            pdIdentInductance[u8MotorId] = dInductance;
        } // public static void SetIdentInductance(Byte u8MotorId, double dInductance)

        /***************************************************************************
         * Function Name : GetIdentInductance
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dInductance | double  | Valor de la Inductancia
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor de la Inductancia del stator
         ***************************************************************************/
        public static double GetIdentInductance(Byte u8MotorId)
        {
            return pdIdentInductance[u8MotorId];
        } // public static double GetIdentInductance(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetIdentRatedFlux
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : dRatedFlux  | double  | Valor del Flujo
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un valor al Flujo del rotor
         ***************************************************************************/
        public static void SetIdentRatedFlux(Byte u8MotorId, double dRatedFlux)
        {
            byte u8PolePairs = GetPolePairs(u8MotorId);
            pdIdentRatedFlux[u8MotorId] = dRatedFlux;
            pdIdentFluxLinkage[u8MotorId] = dRatedFlux / (2 * Math.PI);
            pdIdentTorqueConstant[u8MotorId] = dRatedFlux / (2 * Math.PI) * (2 * u8PolePairs) * (3 / 4);

        } // public static void SetIdentRatedFlux(Byte u8MotorId, double dRatedFlux)

        /***************************************************************************
         * Function Name : GeIdenttRatedFlux
         * Parameters    : u8MotorId  | Byte    | Motor Id
         *               : dRatedFlux | double  | Valor del Flujo
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Devuelve el valor del flujo del  rotor
         ***************************************************************************/
        public static double GetIdentRatedFlux(Byte u8MotorId)
        {
            return pdIdentRatedFlux[u8MotorId];
        } // public static double GetIdentRatedFlux(Byte u8MotorId)

        /***************************************************************************
         * Function Name : SetMotorName
         * Parameters    : u8MotorId   | Byte    | Motor Id
         *               : sMotorName  | string  | Nombre del motor
         * Returns       : None
         ***************************************************************************
         * Notes         :
         * Asigna un nombre al motor
         ***************************************************************************/
        public static void SetMotorName(byte u8MotorId, string sMotorName)
        {
            psMotorName[u8MotorId] = sMotorName;
        } // public static void SetMotorName(byte u8MotorId, string sMotorName)

        /***************************************************************************
         * Function Name : SetMotorName
         * Parameters    : u8MotorId   | Byte    | Motor Id
         * Returns       : sMotorName  | string  | Nombre del motor
         ***************************************************************************
         * Notes         :
         * Asigna un nombre al motor
         ***************************************************************************/
        public static string GetMotorName(byte u8MotorId)
        {
            return psMotorName[u8MotorId];
        } // public static string GetMotorName(byte u8MotorId)

    } // class CCalibration
}
