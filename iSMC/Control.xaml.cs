using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TPCANHandle = System.UInt16;
using TPCANBitrateFD = System.String;
using TPCANTimestampFD = System.UInt64;
using Peak.Can.Basic;

namespace iSMC
{
    /// <summary>
    /// Lógica de interacción para Control.xaml
    /// </summary>
    public partial class Control : Window
    {
        #region Delegates
        /// <summary>
        /// Read-Delegate Handler
        /// </summary>
        private delegate void ReadDelegateHandler();
        #endregion



        Window m_pWindow;


        CControl m_CControl;


        private UInt32 m_u32Cnt1 = 0;

        private UInt32 m_u32Cnt3 = 0;

        BitmapImage m_LogoLedEnable  = new BitmapImage();
        BitmapImage m_LogoLedDisable = new BitmapImage();
        BitmapImage m_LogoHBOff      = new BitmapImage();
        BitmapImage m_LogoHBOn       = new BitmapImage();
        BitmapImage m_ECUIdle        = new BitmapImage();
        BitmapImage m_ECUStop        = new BitmapImage();        
        BitmapImage m_ECUSynch       = new BitmapImage();
        BitmapImage m_ECUPlay        = new BitmapImage();
        BitmapImage m_ECUError       = new BitmapImage();
        BitmapImage m_HeartBeatDis   = new BitmapImage();
        BitmapImage m_HeartBeatEn    = new BitmapImage();



        private bool m_bTestHB = true;

        /// <summary>
        /// Read Delegate for calling the function "ReadMessages"
        /// </summary>
        private ReadDelegateHandler m_ReadDelegate;

        public System.Windows.Threading.DispatcherTimer tmrRead  = new System.Windows.Threading.DispatcherTimer();
        public System.Windows.Threading.DispatcherTimer tmrWrite = new System.Windows.Threading.DispatcherTimer();
        public System.Windows.Threading.DispatcherTimer tmrConHB = new System.Windows.Threading.DispatcherTimer();


        private byte m_u8StatusPastA = 0;
        private byte m_u8StatusPastB = 0;
        private bool m_bHeartBeatA = false;
        private bool m_bHeartBeatB = false;

        private byte m_u8HeartBeatA = 0;
        private byte m_u8HeartBeatB = 0;

        private Int32 m_i32DeltaPosA = 0;
        private Int32 m_i32DeltaPosB = 0;

        private UInt16 m_u16RxTickCnt = 0;

        public Control(Window pWindow)
        {

            m_pWindow = pWindow;

            m_CControl = new CControl();
            m_CControl = new CControl();

            // Creates the delegate used for message reading
            //
            m_ReadDelegate = new ReadDelegateHandler(ReadMessages);

            m_LogoLedEnable.BeginInit();
            m_LogoLedEnable.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedOn.jpg");
            m_LogoLedEnable.EndInit();

            m_LogoLedDisable.BeginInit();
            m_LogoLedDisable.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedGris.png");
            m_LogoLedDisable.EndInit();

            m_LogoHBOff.BeginInit();
            m_LogoHBOff.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/HeartBeatOff.png");
            m_LogoHBOff.EndInit();

            m_LogoHBOn.BeginInit();
            m_LogoHBOn.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/HeartBeatOn.png");
            m_LogoHBOn.EndInit();

            m_ECUIdle.BeginInit();
            m_ECUIdle.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUIdle.png");
            m_ECUIdle.EndInit();

            m_ECUStop.BeginInit();
            m_ECUStop.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUStop.png");
            m_ECUStop.EndInit();

            m_ECUSynch.BeginInit();
            m_ECUSynch.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUPlay.png");
            m_ECUSynch.EndInit();

            m_ECUPlay.BeginInit();
            m_ECUPlay.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUPlay.png");
            m_ECUPlay.EndInit();

            m_ECUError.BeginInit();
            m_ECUError.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUError.png");
            m_ECUError.EndInit();


            m_HeartBeatDis.BeginInit();
            m_HeartBeatDis.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/HeartBeatOffd.png");
            m_HeartBeatDis.EndInit();

            m_HeartBeatEn.BeginInit();
            m_HeartBeatEn.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/HeartBeatOn.png");
            m_HeartBeatEn.EndInit();


            InitializeComponent();

           


            // Activación del timer de lectura
            tmrRead.Tick += new EventHandler(tmrRead_Tick);
            tmrRead.Interval = new TimeSpan(0, 0, 0, 0, CCommunications.CAN_TIME_RX_PROCESS);
            

            tmrWrite.Tick += new EventHandler(tmrWrite_Tick);
            tmrWrite.Interval = new TimeSpan(0, 0, 0, 0, CCommunications.CAN_TIME_TX_PROCESS);


            tmrConHB.Tick += new EventHandler(tmrConHB_Tick);
            tmrConHB.Interval = new TimeSpan(0, 0, 0, 0, CCommunications.CAN_TIME_CONSUMER_HB);

            CANConnect();

        }

        #region EVENTOS GENERALES
        /*******************************************************************************
         * Function Name : Window_MouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento click sobre la ventana principal
         *******************************************************************************/
        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        } //  private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        #endregion

        #region EVENTOS DE CLICK


        private void iReturn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            CANDisconnect();
            this.Close();
            m_pWindow.Show();
        }

        #endregion

        #region CONSIGNAS


        /*******************************************************************************
         * Function Name : DriverCmdA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del comando de activación del
         *  driver de potencia A
         *******************************************************************************/
        private void DriverCmdA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8DriverCmd = (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_DISABLE;

            u8DriverCmd = m_CControl.GetDriverCmd(0);

            if (u8DriverCmd == (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_DISABLE)
            {
                ControlA_WakeUp();
            } // if (u8DriverCmd == CMotorControl.BUS_CMD_IDLE)
            else if (u8DriverCmd == (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_ENABLE)
            {
                ControlA_Sleep();
            } // else if (u8DriverCmd == CMotorControl.BUS_CMD_RUN)
            else
            {

            } // else

        } // private void DriverCmdA_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : DriverCmdA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del comando de activación del
         *  driver de potencia B
         *******************************************************************************/
        private void DriverCmdB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8DriverCmd = (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_DISABLE;

            u8DriverCmd = m_CControl.GetDriverCmd(1);

            if (u8DriverCmd == (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_DISABLE)
            {
                ControlB_WakeUp();
            } // if (u8DriverCmd == CMotorControl.BUS_CMD_IDLE)
            else if (u8DriverCmd == (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_ENABLE)
            {
                ControlB_Sleep();
            } // else if (u8DriverCmd == CMotorControl.BUS_CMD_RUN)
            else
            {

            } // else

        } // private void DriverCmdA_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : RunA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación de la acticación del control
         *  para el motor A
         *******************************************************************************/
        private void RunA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8ControlCmd = (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE;

            u8ControlCmd = m_CControl.Control_GetCmd(0);

            if (u8ControlCmd == (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                ControlA_Run(); 
            } // if (u8ControlCmd == (byte) CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            else
            {
                ControlA_Stop();

            } // ! if (u8ControlCmd == CMotorControl.CTRL_CMD_IDLE)

        } // private void iRun_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : RunB_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación de la acticación del control
         *  para el motor B
         *******************************************************************************/
        private void RunB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8ControlCmd = (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE;

            u8ControlCmd = m_CControl.Control_GetCmd(1);

            if (u8ControlCmd == (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                ControlB_Run(); 
            } // if (u8ControlCmd == (byte) CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            else
            {
                ControlB_Stop();

            } // ! if (u8ControlCmd == CMotorControl.CTRL_CMD_IDLE)

        } // private void iRun_OnMouseDown(object sender, MouseButtonEventArgs e)


        /*******************************************************************************
         * Function Name : iSpeedCCWA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de cambio del sentido de giro para el motor B
         *******************************************************************************/
        private void iSpeedCCWA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8MotorCCW = (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW;

            u8MotorCCW = m_CControl.GetSpinDir(0);

            if (u8MotorCCW == (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW)
            {
                m_CControl.SetSpinDir(0, (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CW);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/MotorCW.png");
                logo.EndInit();
                iSpeedCCWA.Source = logo;
                lSpinCmdA.Content = "CW SPIN";
            }
            else
            {
                m_CControl.SetSpinDir(0, (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/MotorCCW.png");
                logo.EndInit();
                iSpeedCCWA.Source = logo;
                lSpinCmdA.Content = "CCW SPIN";
            }

        } // private void iSpeedCCWA_OnMouseDown(object sender, MouseButtonEventArgs e)



        /*******************************************************************************
         * Function Name : iSpeedCCWB_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de cambio del sentido de giro para el motor B
         *******************************************************************************/
        private void iSpeedCCWB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8MotorCCW = (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW;

            u8MotorCCW = m_CControl.GetSpinDir(1);

            if (u8MotorCCW == (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW)
            {
                m_CControl.SetSpinDir(1, (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CW);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/MotorCW.png");
                logo.EndInit();
                iSpeedCCWB.Source = logo;
                lSpinCmdB.Content = "CW SPIN";
            }
            else
            {
                m_CControl.SetSpinDir(1, (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/MotorCCW.png");
                logo.EndInit();
                iSpeedCCWB.Source = logo;
                lSpinCmdB.Content = "CCW SPIN";
            }

        } // private void iSpeedCCWB_OnMouseDown(object sender, MouseButtonEventArgs e)


        /*******************************************************************************
         * Function Name : iRegenBrkA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Habilitar o deshabilitar el frenado regenerativo para el motor A
         *******************************************************************************/
        private void iRegenBrkA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8RegenBrake = (byte)CControl.CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_OFF;

            u8RegenBrake = m_CControl.GetRegenBrake(0);


            if (u8RegenBrake == (byte)CControl.CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_OFF)
            {
                m_CControl.SetRegenBrake(0);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/RegenBrakingOn.png");
                logo.EndInit();
                iRegenBrkA.Source = logo;
                lRegenBrkA.Content = "REGEN ON";
            }
            else
            {
                m_CControl.ClrRegenBrake(0);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/RegenBrakingOff.png");
                logo.EndInit();
                iRegenBrkA.Source = logo;
                lRegenBrkA.Content = "REGEN OFF";
            }
        } // private void iRegenBrkA_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : iRegenBrkB_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Habilitar o deshabilitar el frenado regenerativo para el motor B
         *******************************************************************************/
        private void iRegenBrkB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8RegenBrake = (byte)CControl.CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_OFF;

            u8RegenBrake = m_CControl.GetRegenBrake(1);


            if (u8RegenBrake == (byte)CControl.CTRL_ST_REGEN_BRAKE.CTRL_REGEN_BRAKE_OFF)
            {
                m_CControl.SetRegenBrake(1);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/RegenBrakingOn.png");
                logo.EndInit();
                iRegenBrkB.Source = logo;
                lRegenBrkB.Content = "REGEN ON";
            }
            else
            {
                m_CControl.ClrRegenBrake(1);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/RegenBrakingOff.png");
                logo.EndInit();
                iRegenBrkB.Source = logo;
                lRegenBrkB.Content = "REGEN OFF";
            }
        } // private void iRegenBrkB_OnMouseDown(object sender, MouseButtonEventArgs e)


        /*******************************************************************************
         * Function Name : iBrakeA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Habilitar o deshabilitar el flag de freno para el motor A
         *******************************************************************************/
        private void iBrakeA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8Brake = (byte)CControl.CTRL_ST_BRAKE.CTRL_BRAKE_ON;

            u8Brake = m_CControl.GetBrake(0);


            if (u8Brake == (byte)CControl.CTRL_ST_BRAKE.CTRL_BRAKE_OFF)
            {
                m_CControl.SetBrake(0);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BrakeOn.png");
                logo.EndInit();
                iBrakeA.Source = logo;
                lBrakeA.Content = "BRAKE ON";
            }
            else
            {
                m_CControl.ClrBrake(0);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BrakeOff.png");
                logo.EndInit();
                iBrakeA.Source = logo;
                lBrakeA.Content = "BRAKE OFF";
            }
        } // private void iBrakeA_OnMouseDown(object sender, MouseButtonEventArgs e)


        /*******************************************************************************
         * Function Name : iBrakeB_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Habilitar o deshabilitar el flag de freno para el motor B
         *******************************************************************************/
        private void iBrakeB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8Brake = (byte)CControl.CTRL_ST_BRAKE.CTRL_BRAKE_ON;

            u8Brake = m_CControl.GetBrake(1);


            if (u8Brake == (byte)CControl.CTRL_ST_BRAKE.CTRL_BRAKE_OFF)
            {
                m_CControl.SetBrake(1);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BrakeOn.png");
                logo.EndInit();
                iBrakeB.Source = logo;
                lBrakeB.Content = "BRAKE ON";
            }
            else
            {
                m_CControl.ClrBrake(1);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BrakeOff.png");
                logo.EndInit();
                iBrakeB.Source = logo;
                lBrakeB.Content = "BRAKE OFF";
            }
        } // private void iBrakeB_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : iTorqueA_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del modo de conducción
         *  par para el motor A
         *******************************************************************************/
        private void iTorqueA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8CtrlType = (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE;

            u8CtrlType = m_CControl.GetMode(0);


            if (u8CtrlType == (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                m_CControl.SetMode(0, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_TORQUE);
                BitmapImage logo1 = new BitmapImage();
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedON.jpg");
                logo1.EndInit();
                iLedTorqueA.Source = logo1;
            }
            else if (u8CtrlType == (byte)CControl.CTRL_ST_MODE.CTRL_MODE_TORQUE)
            {
                m_CControl.SetMode(0, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedGris.png");
                logo.EndInit();
                iLedTorqueA.Source = logo;

            }
            else
            {

            }

        } // private void iTorqueA_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : iTorqueB_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del modo de conducción
         *  par para el motor B
         *******************************************************************************/
        private void iTorqueB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8CtrlType = (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE;

            u8CtrlType = m_CControl.GetMode(1);


            if (u8CtrlType == (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                m_CControl.SetMode(1, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_TORQUE);
                BitmapImage logo1 = new BitmapImage();
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedON.jpg");
                logo1.EndInit();
                iLedTorqueB.Source = logo1;
            }
            else if (u8CtrlType == (byte)CControl.CTRL_ST_MODE.CTRL_MODE_TORQUE)
            {
                m_CControl.SetMode(1, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedGris.png");
                logo.EndInit();
                iLedTorqueB.Source = logo;

            }
            else
            {

            }

        } // private void iTorqueB_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : iSpeed_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del modo de conducción
         *  velocidad para el motor A
         *******************************************************************************/
        private void iSpeedA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8CtrlType = (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE;

            u8CtrlType = m_CControl.GetMode(0);

            if (u8CtrlType == (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                m_CControl.SetMode(0, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_SPEED);
                BitmapImage logo1 = new BitmapImage();
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedON.jpg");
                logo1.EndInit();
                iLedSpeedA.Source = logo1;
            }
            else if (u8CtrlType == (byte)CControl.CTRL_ST_MODE.CTRL_MODE_SPEED)
            {
                m_CControl.SetMode(0, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedGris.png");
                logo.EndInit();
                iLedSpeedA.Source = logo;
            }
            else
            {

            }

        } // private void iSpeed_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : iSpeed_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del modo de conducción
         *  velocidad para el motor B
         *******************************************************************************/
        private void iSpeedB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            Byte u8CtrlType = (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE;

            u8CtrlType = m_CControl.GetMode(1);

            if (u8CtrlType == (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE)
            {
                m_CControl.SetMode(1, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_SPEED);
                BitmapImage logo1 = new BitmapImage();
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedON.jpg");
                logo1.EndInit();
                iLedSpeedB.Source = logo1;
            }
            else if (u8CtrlType == (byte)CControl.CTRL_ST_MODE.CTRL_MODE_SPEED)
            {
                m_CControl.SetMode(1, (byte)CControl.CTRL_ST_MODE.CTRL_MODE_IDLE);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/LedGris.png");
                logo.EndInit();
                iLedSpeedB.Source = logo;
            }
            else
            {

            }

        } // private void iSpeed_OnMouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : slTorqueA_OnValueChanged
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del valor del par de ref.
         *  para el motor A
         *******************************************************************************/
        private void slTorqueA_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            string sTorque = slTorqueA.Value.ToString();
            double dTorque = 0;
            short i16Torque;

            dTorque = Convert.ToDouble(sTorque);

            dTorque = Math.Round(dTorque, 2);

            tTorqueValueA.Text = dTorque.ToString();

            i16Torque = Convert.ToInt16(dTorque * 256);

            m_CControl.SetTorqueRef(0, i16Torque);

        } // private void slTorqueA_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)

        /*******************************************************************************
         * Function Name : slTorqueB_OnValueChanged
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del valor del par de ref.
         *  para el motor B
         *******************************************************************************/
        private void slTorqueB_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            string sTorque = slTorqueB.Value.ToString();
            double dTorque = 0;
            short i16Torque;

            dTorque = Convert.ToDouble(sTorque);

            dTorque = Math.Round(dTorque, 2);

            tTorqueValueA.Text = dTorque.ToString();

            i16Torque = Convert.ToInt16(dTorque * 256);

            m_CControl.SetTorqueRef(1, i16Torque);

        } // private void slTorqueB_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)

        /*******************************************************************************
         * Function Name : slSpeedA_OnValueChanged
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del valor velocidad de ref.
         *  para el motor
         *******************************************************************************/
        private void slSpeedA_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            string sSpeed = slSpeedA.Value.ToString();
            short i16Speed = 0;

            i16Speed = Int16.Parse(sSpeed);

            tSpeedValueA.Text = sSpeed;

            m_CControl.SetSpeedRef(0, i16Speed);

        } // private void slSpeedA_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)

        /*******************************************************************************
         * Function Name : slSpeedB_OnValueChanged
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención al evento de modificación del valor velocidad de ref.
         *  para el motor
         *******************************************************************************/
        private void slSpeedB_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            string sSpeed = slSpeedB.Value.ToString();
            short i16Speed = 0;

            i16Speed = Int16.Parse(sSpeed);

            tSpeedValueB.Text = sSpeed;

            m_CControl.SetSpeedRef(1, i16Speed);

        } // private void slSpeedB_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)


        private void RefreshA_OnMouseDown(object sender, MouseButtonEventArgs e)
        {


            // Eliminar fallos
            Console.WriteLine("Refrescar iSMC A");
            TPCANStatus stsResult;
            byte[] pu8Data = new byte[8];
            /* 
             * Byte 0    Comando Atendido
             * Byte 1    Resultado del comando(0: Erróneo | 1: OK)
             * Byte 2    Parámetro Leido / Escrito
             * Byte 3    Tipo de mensaje(MSG_EOF)
             * Byte 4    Parámetro(1 / 4) Hi
             * Byte 5    Parámetro(2 / 4)
             * Byte 6    Parámetro(3 / 4)
             * Byte 7    Parámetro(4 / 4) Lo
            */
            pu8Data[0] = (byte) CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_FAULT_RESET;
            //pu8Data[0] |= Convert.ToByte((Convert.ToUInt16(u8AppCmd)   << 4));
            pu8Data[1] = 0x01;
            pu8Data[2] = 0x00;
            pu8Data[3] = 0x03;
            pu8Data[4] = 0x00;
            pu8Data[5] = 0x00;
            pu8Data[6] = 0x00;
            pu8Data[7] = 0x00;
            UInt32 u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_DIAG_RQ_A);
            stsResult = WriteFrame(u32MsgId, 8, pu8Data);


            ControlA_Stop();
            //ControlA_Sleep();

        }

        private void RefreshB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {


            // Eliminar fallos
            Console.WriteLine("Refrescar iSMC B");
            TPCANStatus stsResult;
            byte[] pu8Data = new byte[8];
            /* 
             * Byte 0    Comando Atendido
             * Byte 1    Resultado del comando(0: Erróneo | 1: OK)
             * Byte 2    Parámetro Leido / Escrito
             * Byte 3    Tipo de mensaje(MSG_EOF)
             * Byte 4    Parámetro(1 / 4) Hi
             * Byte 5    Parámetro(2 / 4)
             * Byte 6    Parámetro(3 / 4)
             * Byte 7    Parámetro(4 / 4) Lo
            */
            pu8Data[0] = (byte)CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_FAULT_RESET;
            //pu8Data[0] |= Convert.ToByte((Convert.ToUInt16(u8AppCmd)   << 4));
            pu8Data[1] = 0x01;
            pu8Data[2] = 0x00;
            pu8Data[3] = 0x03;
            pu8Data[4] = 0x00;
            pu8Data[5] = 0x00;
            pu8Data[6] = 0x00;
            pu8Data[7] = 0x00;
            UInt32 u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_DIAG_RQ_B);
            stsResult = WriteFrame(u32MsgId, 8, pu8Data);

            ControlB_Stop();
            //ControlB_Sleep();

        }

        #endregion


        #region funciones asociadas a los controles

        /*******************************************************************************
         * Function Name : ControlA_Sleep
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Deshabilita el driver A
         *******************************************************************************/
        private void ControlA_Sleep()
        {
            m_CControl.SetDriverCmd(0, (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_DISABLE);
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BatteryOFF.PNG");
            logo.EndInit();
            DriverCmdA.Source = logo;
            lBusCmdA.Content = "DRV OFF";
        } // private void ControlA_Sleep()


        /*******************************************************************************
         * Function Name : ControlA_Sleep
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Deshabilita el driver B
         *******************************************************************************/
        private void ControlB_Sleep()
        {
            m_CControl.SetDriverCmd(1, (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_DISABLE);
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BatteryOFF.PNG");
            logo.EndInit();
            DriverCmdB.Source = logo;
            lBusCmdB.Content = "DRV OFF";
        } // private void ControlB_Sleep()


        /*******************************************************************************
         * Function Name : ControlA_WakeUp
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Habilita el driver A
         *******************************************************************************/
        private void ControlA_WakeUp()
        {
            m_CControl.SetDriverCmd(0, (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_ENABLE);
            BitmapImage logo1 = new BitmapImage();
            logo1.BeginInit();
            logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BatteryON.PNG");
            logo1.EndInit();
            DriverCmdA.Source = logo1;
            lBusCmdA.Content = "DRV ON";
        } // private void ControlA_WakeUp()

        /*******************************************************************************
         * Function Name : ControlA_WakeUp
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Habilita el driver B
         *******************************************************************************/
        private void ControlB_WakeUp()
        {
            m_CControl.SetDriverCmd(1, (byte)CControl.CTRL_ST_DRIVER.DRIVER_CMD_ENABLE);
            BitmapImage logo1 = new BitmapImage();
            logo1.BeginInit();
            logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/BatteryON.PNG");
            logo1.EndInit();
            DriverCmdB.Source = logo1;
            lBusCmdA.Content = "DRV ON";
        } // private void ControlB_WakeUp()

        /*******************************************************************************
         * Function Name : ControlA_Stop
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Desactiva el control A
         *******************************************************************************/
        private void ControlA_Stop()
        {
            // El control está activo, se desactiva
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/IconoPlay.PNG");
            logo.EndInit();
            iControlCmdA.Source = logo;
            m_CControl.Control_SetCmd(0, (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE);
            lControlCmdA.Content = "CTRL OFF";
        } // private void ControlA_Stop()

        /*******************************************************************************
         * Function Name : ControlB_Stop
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Desactiva el control B
         *******************************************************************************/
        private void ControlB_Stop()
        {
            // El control está activo, se desactiva
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/IconoPlay.PNG");
            logo.EndInit();
            iControlCmdB.Source = logo;
            m_CControl.Control_SetCmd(1, (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_DISABLE);
            lControlCmdB.Content = "CTRL OFF";
        } // private void ControlB_Stop()

        /*******************************************************************************
         * Function Name : ControlA_Run
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Activa el control A
         *******************************************************************************/
        private void ControlA_Run()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/IconoStop.PNG");
            logo.EndInit();
            iControlCmdA.Source = logo;
            m_CControl.Control_SetCmd(0, (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_ENABLE);
            lControlCmdA.Content = "CTRL ON";
        } // private void ControlA_Run()

        /*******************************************************************************
         * Function Name : ControlB_Run
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Activa el control A
         *******************************************************************************/
        private void ControlB_Run()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/IconoStop.PNG");
            logo.EndInit();
            iControlCmdB.Source = logo;
            m_CControl.Control_SetCmd(1, (byte)CControl.CTRL_ST_CONTROL.CTRL_CMD_ENABLE);
            lControlCmdB.Content = "CTRL ON";
        } // private void ControlB_Run()

        #endregion


        #region Timer event-handler

        private void tmrRead_Tick(object sender, EventArgs e)
        {
            // Checks if in the receive-queue are currently messages for read
            // 
            ReadMessages();

            if (m_u16RxTickCnt++ == 100)
            {
                m_u16RxTickCnt = 0;
                lFaultMsgA.Content = "";
                lFaultMsgA.Visibility = Visibility.Hidden;
                lFaultMsgA2.Content = "";
                lFaultMsgA2.Visibility = Visibility.Hidden;
                lFaultMsgB.Content = "";
                lFaultMsgB.Visibility = Visibility.Hidden;
                lFaultMsgB2.Content = "";
                lFaultMsgB2.Visibility = Visibility.Hidden;
            }
        }


        private void tmrWrite_Tick(object sender, EventArgs e)
        {
            CAN_SendCtrlRefsA();
            CAN_SendCtrlRefsB();
        }

        private void tmrConHB_Tick(object sender, EventArgs e)
        {
            CAN_SendConsumerHBA();
            CAN_SendConsumerHBB();
        }

        
        #endregion

        #region Connection functions

        private void CANConnect()
        {
            byte u8ConnectionStatus;

            u8ConnectionStatus = CCommunications.GetConnectionStatus();


            TPCANStatus stsResult;


            // Connects a selected PCAN-Basic channel
            //
            stsResult = PCANBasic.Initialize(
                        CCommunications.m_PcanHandle,
                        CCommunications.m_Baudrate,
                        CCommunications.m_HwType,
                        256,
                        3);


            tmrRead.Start();
            tmrWrite.Start();
            tmrConHB.Start();

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                if (stsResult != TPCANStatus.PCAN_ERROR_CAUTION)
                {
                    MessageBox.Show(GetFormatedError(stsResult));
                }
                else
                {
                    stsResult = TPCANStatus.PCAN_ERROR_OK;
                    
                }
            }
            else
            {
                CCommunications.SetConnectionStatus((byte)CCommunications.TCONNECTION.CONNECTION_CONNECT);
            }


            
        }

        private void CANDisconnect()
        {
            PCANBasic.Uninitialize(CCommunications.m_PcanHandle);
            tmrRead.Stop();
            tmrWrite.Stop();
            tmrConHB.Stop();


        }

        /// <summary>
        /// Help Function used to get an error as text
        /// </summary>
        /// <param name="error">Error code to be translated</param>
        /// <returns>A text with the translated error</returns>
        private string GetFormatedError(TPCANStatus error)
        {
            StringBuilder strTemp;

            // Creates a buffer big enough for a error-text
            //
            strTemp = new StringBuilder(256);
            // Gets the text using the GetErrorText API function
            // If the function success, the translated error is returned. If it fails,
            // a text describing the current error is returned.
            //
            if (PCANBasic.GetErrorText(error, 0, strTemp) != TPCANStatus.PCAN_ERROR_OK)
            {
                return string.Format("An error occurred. Error-code's text ({0:X}) couldn't be retrieved", error);
            }
            else
            {
                return strTemp.ToString();
            }
        }

        #endregion

        #region Message Processing
        /// <summary>
        /// Function for reading PCAN-Basic messages
        /// </summary>
        private void ReadMessages()
        {
            TPCANStatus stsResult;

            // We read at least one time the queue looking for messages.
            // If a message is found, we look again trying to find more.
            // If the queue is empty or an error occurr, we get out from
            // the dowhile statement.
            //			
            do
            {
                stsResult = CCommunications.m_bIsFD ? ReadMessageFD() : ReadMessage();
                if (stsResult == TPCANStatus.PCAN_ERROR_ILLOPERATION)
                {
                    break;
                }
            } while ((CCommunications.GetConnectionStatus() == (byte)CCommunications.TCONNECTION.CONNECTION_CONNECT) && (!Convert.ToBoolean(stsResult & TPCANStatus.PCAN_ERROR_QRCVEMPTY)));
        } // private void ReadMessages()

        /// <summary>
        /// Function for reading messages on FD devices
        /// </summary>
        /// <returns>A TPCANStatus error code</returns>
        private TPCANStatus ReadMessageFD()
        {
            TPCANMsgFD CANMsg;
            TPCANTimestampFD CANTimeStamp;
            TPCANStatus stsResult;

            // We execute the "Read" function of the PCANBasic                
            //
            stsResult = PCANBasic.ReadFD(CCommunications.m_PcanHandle, out CANMsg, out CANTimeStamp);
            if (stsResult != TPCANStatus.PCAN_ERROR_QRCVEMPTY)
            {
                // We process the received message
                //

            }
            return stsResult;
        }

        /// <summary>
        /// Function for reading CAN messages on normal CAN devices
        /// </summary>
        /// <returns>A TPCANStatus error code</returns>
        private TPCANStatus ReadMessage()
        {
            TPCANMsg CANMsg;
            TPCANTimestamp CANTimeStamp;
            TPCANStatus stsResult;

            // We execute the "Read" function of the PCANBasic                
            //
            stsResult = PCANBasic.Read(CCommunications.m_PcanHandle, out CANMsg, out CANTimeStamp);
            if (stsResult != TPCANStatus.PCAN_ERROR_QRCVEMPTY)
            {


                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CTRL_STATUS_A))
                {
                    
                    CAN_OnCtrlStatusMsgA(CANMsg.DATA);

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_BOARD_STATUS_A))
                {
                    CAN_OnBoardStatusMsgA(CANMsg.DATA);

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_DIAG_RS_A))
                {
                    CAN_OnDiagMsgA(CANMsg.DATA);

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_NMT_TX_A))
                {
                    CAN_OnProducerHBA(CANMsg.DATA);

                }
                

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CTRL_STATUS_B))
                {
                    CAN_OnCtrlStatusMsgB(CANMsg.DATA);

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_BOARD_STATUS_B))
                {
                    CAN_OnBoardStatusMsgB(CANMsg.DATA);

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_DIAG_RS_B))
                {
                    CAN_OnDiagMsgB(CANMsg.DATA);

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_NMT_TX_B))
                {
                    CAN_OnProducerHBB(CANMsg.DATA);

                }


                // We process the received message
                //
                //ProcessMessage(CANMsg, CANTimeStamp);
            }
            return stsResult;
        }


        private TPCANStatus WriteFrame(UInt32 u32MsgId, byte u8Length, byte[] pu8Data)
        {
            TPCANMsg CANMsg;


            // We create a TPCANMsg message structure 
            //
            CANMsg = new TPCANMsg();
            CANMsg.DATA = new byte[8];

            // We configurate the Message.  The ID,
            // Length of the Data, Message Type
            // and the data
            //
            CANMsg.ID = u32MsgId;
            CANMsg.LEN = u8Length;
            CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

            // We get so much data as the Len of the message
            //
            for (int i = 0; i < u8Length; i++)
            {
                CANMsg.DATA[i] = pu8Data[i];
            }


            // The message is sent to the configured hardware
            //
            return PCANBasic.Write(CCommunications.m_PcanHandle, ref CANMsg);
        }

        #endregion

        #region Decodificación de mensajes

        /*******************************************************************************
         * Function Name : CAN_OnCtrlStatusMsgA
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de status
         *******************************************************************************/
        private void CAN_OnCtrlStatusMsgA(byte [] pu8Data)
        {

            byte u8Status = pu8Data[1];
            m_CControl.SetISMCStatus(0, u8Status);


            UpdateStatusA(u8Status);

            Int32 i32Current = (Convert.ToInt32(pu8Data[3]) << 8);
            i32Current |= (Convert.ToInt32(pu8Data[2]));
            if (i32Current >= 32768)
            {
                i32Current = i32Current - 65535;
            }

            double dCurrent = Convert.ToDouble(i32Current);
            dCurrent = dCurrent / 256.0;
            dCurrent = Math.Round(dCurrent, 3);

            string sCurrent = dCurrent.ToString();

            sCurrent = string.Format("{0:0.###}", sCurrent);

            lCurrentStatusValA.Content = sCurrent;

            Int32 i32Speed  = (Convert.ToInt32(pu8Data[5]) << 8);
                  i32Speed |= (Convert.ToInt32(pu8Data[4]));
            if (i32Speed >= 32768)
            {
                i32Speed = i32Speed - 65535;
            }

            Int32 i32Torque = (Convert.ToInt32(pu8Data[7]) << 8);
            i32Torque |= (Convert.ToInt32(pu8Data[6]));
            if (i32Torque >= 32768)
            {
                i32Torque = i32Torque - 65535;
            }
            double dTorque = Convert.ToDouble(i32Torque);
            dTorque = dTorque / 4096.0;
            dTorque = Math.Round(dTorque, 3);

            string sTorque = dTorque.ToString();

            sTorque = string.Format("{0:0.###}", sTorque);

            lSpeedStatusValA.Content = i32Speed.ToString();
            lTorqueStatusValA.Content = sTorque;

            



        } // private void CAN_OnCtrlStatusMsg(string sMessageData)


        /*******************************************************************************
          * Function Name : CAN_OnCtrlStatusMsgB
          * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
          * Returns       : None
          *******************************************************************************
          * Notes         :
          *  Subrutina de atención a mensaje de status
          *******************************************************************************/
        private void CAN_OnCtrlStatusMsgB(byte[] pu8Data)
        {

            byte u8Status = pu8Data[1];
            m_CControl.SetISMCStatus(1, u8Status);


            UpdateStatusB(u8Status);

            Int32 i32Current = (Convert.ToInt32(pu8Data[3]) << 8);
            i32Current |= (Convert.ToInt32(pu8Data[2]));
            if (i32Current >= 32768)
            {
                i32Current = i32Current - 65535;
            }

            double dCurrent = Convert.ToDouble(i32Current);
            dCurrent = dCurrent / 256.0;
            dCurrent = Math.Round(dCurrent, 3);

            string sCurrent = dCurrent.ToString();

            sCurrent = string.Format("{0:0.###}", sCurrent);

            lCurrentStatusValB.Content = sCurrent;

            Int32 i32Speed = (Convert.ToInt32(pu8Data[5]) << 8);
            i32Speed |= (Convert.ToInt32(pu8Data[4]));
            if (i32Speed >= 32768)
            {
                i32Speed = i32Speed - 65535;
            }

            Int32 i32Torque = (Convert.ToInt32(pu8Data[7]) << 8);
            i32Torque |= (Convert.ToInt32(pu8Data[6]));
            if (i32Torque >= 32768)
            {
                i32Torque = i32Torque - 65535;
            }
            double dTorque = Convert.ToDouble(i32Torque);
            dTorque = dTorque / 4096.0;
            dTorque = Math.Round(dTorque, 3);

            string sTorque = dTorque.ToString();

            sTorque = string.Format("{0:0.###}", sTorque);

            lSpeedStatusValB.Content = i32Speed.ToString();
            lTorqueStatusValB.Content = sTorque;





        } // private void CAN_OnCtrlStatusMsg(string sMessageData)

        /*******************************************************************************
         * Function Name : CAN_OnBoardStatusMsgA
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de status
         *******************************************************************************/
        private void CAN_OnBoardStatusMsgA(byte[] pu8Data)
        {
            m_i32DeltaPosA  = (Convert.ToInt32(pu8Data[2]) << 8);
            m_i32DeltaPosA |= (Convert.ToInt32(pu8Data[1]));

            if (m_i32DeltaPosA > 32767)
            {
                m_i32DeltaPosA = 65535 - m_i32DeltaPosA;
            }

            lPosEncA.Content = m_i32DeltaPosA.ToString();


            Int32 i32BusVoltage  = (Convert.ToInt32(pu8Data[5]) << 8);
                  i32BusVoltage |= (Convert.ToInt32(pu8Data[4]));
            double dBusVoltage = Convert.ToDouble(i32BusVoltage);
            dBusVoltage = dBusVoltage / 4;

            Int32 i32BoardTemp  = (Convert.ToInt32(pu8Data[7]) << 8);
                  i32BoardTemp |= (Convert.ToInt32(pu8Data[6]));
            double dBoardTemp = Convert.ToDouble(i32BoardTemp);
            dBoardTemp = dBoardTemp / 4;

            lBusVoltageValA.Content = dBusVoltage.ToString();
            lBoardTempValA.Content = dBoardTemp.ToString();


        } // private void CAN_OnBoardStatusMsg(string sMessageData)

        /*******************************************************************************
         * Function Name : CAN_OnBoardStatusMsgB
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de status
         *******************************************************************************/
        private void CAN_OnBoardStatusMsgB(byte[] pu8Data)
        {
            m_i32DeltaPosB = (Convert.ToInt32(pu8Data[2]) << 8);
            m_i32DeltaPosB |= (Convert.ToInt32(pu8Data[1]));

            if (m_i32DeltaPosB > 32767)
            {
                m_i32DeltaPosB = 65535 - m_i32DeltaPosB;
            }

            lPosEncB.Content = m_i32DeltaPosB.ToString();

            Int32 i32BusVoltage = (Convert.ToInt32(pu8Data[5]) << 8);
            i32BusVoltage |= (Convert.ToInt32(pu8Data[4]));
            double dBusVoltage = Convert.ToDouble(i32BusVoltage);
            dBusVoltage = dBusVoltage / 4;

            Int32 i32BoardTemp = (Convert.ToInt32(pu8Data[7]) << 8);
            i32BoardTemp |= (Convert.ToInt32(pu8Data[6]));
            double dBoardTemp = Convert.ToDouble(i32BoardTemp);
            dBoardTemp = dBoardTemp / 4;

            lBusVoltageValB.Content = dBusVoltage.ToString();

            lBoardTempValB.Content = dBoardTemp.ToString();


        } // private void CAN_OnBoardStatusMsg(string sMessageData)


        /*******************************************************************************
         * Function Name : CAN_OnProducerHB
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de status
         *******************************************************************************/
        private void CAN_OnProducerHBA(byte[] pu8Data)
        {
            m_u32Cnt1++;

            if (m_bHeartBeatA)
            {
                m_bHeartBeatA = false;
                iCommStatusA.Source = m_LogoHBOff;

            }
            else
            {
                m_bHeartBeatA = true;
                iCommStatusA.Source = m_LogoHBOn;
            }
        } // private void CAN_OnBoardStatusMsg(string sMessageData)

        /*******************************************************************************
         * Function Name : CAN_OnProducerHB
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de status
         *******************************************************************************/
        private void CAN_OnProducerHBB(byte[] pu8Data)
        {
            m_u32Cnt3++;

            if (m_bHeartBeatB)
            {
                m_bHeartBeatB = false;
                iCommStatusB.Source = m_LogoHBOff;

            }
            else
            {
                m_bHeartBeatB = true;
                iCommStatusB.Source = m_LogoHBOn;
            }
        } // private void CAN_OnBoardStatusMsg(string sMessageData)

        /*******************************************************************************
         * Function Name : CAN_SendCtrlRefsA
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Envía los mensajes de referencias para las placas ISMC
         *******************************************************************************/
        private void CAN_SendCtrlRefsA()
        {
            byte[] pu8Data = new byte[8];
            byte u8BrakeCmd = 0;
            byte u8DriveCmd = 0;
            byte u8CtrlCmd = 0;
            byte u8CtrlType = 0;
            Int16 s16Speed = 0;
            UInt16 u16Speed = 0;
            Int16 s16Torque = 0;
            UInt16 u16Torque = 0;
            UInt32 u32MsgId = 0;

            TPCANStatus stsResult;

            u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_REF_A);
            

            byte u8MotorSpin = (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW;

            if (m_u8HeartBeatA == 7)
            {
                m_u8HeartBeatA = 0;
            }
            else
            {
                m_u8HeartBeatA++;
            }

            u8BrakeCmd = m_CControl.GetBrake(0);

            u8DriveCmd = m_CControl.GetDriverCmd(0);
            u8CtrlCmd = m_CControl.Control_GetCmd(0);
            u8CtrlType = m_CControl.GetMode(0);

            u8MotorSpin = m_CControl.GetSpinDir(0);
            s16Speed = m_CControl.GetSpeedRef(0);
            s16Torque = m_CControl.GetTorqueRef(0);

            if (u8MotorSpin == (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CW)
            {
                u16Speed = Convert.ToUInt16(65535 - Convert.ToInt32(s16Speed));
                u16Torque = Convert.ToUInt16(65535 - Convert.ToInt32(s16Torque));
            }
            else
            {
                u16Speed = Convert.ToUInt16(s16Speed);
                u16Torque = Convert.ToUInt16(s16Torque);
            }

            pu8Data[0] = m_u8HeartBeatA;
            pu8Data[0] |= (0x08);
            pu8Data[0] |= Convert.ToByte((Convert.ToUInt16(u8BrakeCmd) << 7));
            pu8Data[1] = Convert.ToByte((Convert.ToUInt16(u8DriveCmd)));
            pu8Data[1] |= Convert.ToByte((Convert.ToUInt16(u8CtrlCmd) << 2));
            pu8Data[1] |= Convert.ToByte((Convert.ToUInt16(u8CtrlType) << 4));
            pu8Data[2] = Convert.ToByte(u16Speed & 0x00FF);
            pu8Data[3] = Convert.ToByte(u16Speed >> 8);
            pu8Data[4] = Convert.ToByte(u16Torque & 0x00FF);
            pu8Data[5] = Convert.ToByte(u16Torque >> 8);
            pu8Data[6] = 0;
            pu8Data[7] = 0;

            stsResult = WriteFrame(u32MsgId, 8, pu8Data);
            /*
            Console.WriteLine("=====================================================");
            Console.WriteLine("            Mensaje de Referncia ISMC A              ");
            Console.WriteLine("=====================================================");
            Console.WriteLine("Estado: " + ControlA_GetStatus().ToString());
            Console.WriteLine(" - Driver:  " + u8DriveCmd.ToString());
            Console.WriteLine(" - Control: " + u8CtrlCmd.ToString());
            Console.WriteLine(" - Tipo: " + u8CtrlType.ToString());
            */

        } // private void CAN_SendCtrlRefsA()

        /*******************************************************************************
         * Function Name : CAN_SendCtrlRefsB
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Envía los mensajes de referencias para las placas ISMC
         *******************************************************************************/
        private void CAN_SendCtrlRefsB()
        {
            byte u8BrakeCmd = 0;
            byte[] pu8Data = new byte[8];
            byte u8DriveCmd = 0;
            byte u8CtrlCmd = 0;
            byte u8CtrlType = 0;
            Int16 s16Speed = 0;
            UInt16 u16Speed = 0;
            Int16 s16Torque = 0;
            UInt16 u16Torque = 0;
            UInt32 u32MsgId = 0;

            TPCANStatus stsResult;

            u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_REF_B);

            u8BrakeCmd = m_CControl.GetBrake(1);

            byte u8MotorSpin = (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CCW;

            if (m_u8HeartBeatB == 7)
            {
                m_u8HeartBeatB = 0;
            }
            else
            {
                m_u8HeartBeatB++;
            }

            u8DriveCmd = m_CControl.GetDriverCmd(1);
            u8CtrlCmd = m_CControl.Control_GetCmd(1);
            u8CtrlType = m_CControl.GetMode(1);

            u8MotorSpin = m_CControl.GetSpinDir(1);
            s16Speed = m_CControl.GetSpeedRef(1);
            s16Torque = m_CControl.GetTorqueRef(1);

            if (u8MotorSpin == (byte)CControl.CTRL_ST_SPIN_DIR.CTRL_SPIN_DIR_CW)
            {
                u16Speed = Convert.ToUInt16(65535 - Convert.ToInt32(s16Speed));
                u16Torque = Convert.ToUInt16(65535 - Convert.ToInt32(s16Torque));
            }
            else
            {
                u16Speed = Convert.ToUInt16(s16Speed);
                u16Torque = Convert.ToUInt16(s16Torque);
            }

            pu8Data[0] = m_u8HeartBeatB;
            pu8Data[0] |= (0x08);
            pu8Data[0] |= Convert.ToByte((Convert.ToUInt16(u8BrakeCmd) << 7));
            pu8Data[1] = Convert.ToByte((Convert.ToUInt16(u8DriveCmd)));
            pu8Data[1] |= Convert.ToByte((Convert.ToUInt16(u8CtrlCmd) << 2));
            pu8Data[1] |= Convert.ToByte((Convert.ToUInt16(u8CtrlType) << 4));
            pu8Data[2] = Convert.ToByte(u16Speed & 0x00FF);
            pu8Data[3] = Convert.ToByte(u16Speed >> 8);
            pu8Data[4] = Convert.ToByte(u16Torque & 0x00FF);
            pu8Data[5] = Convert.ToByte(u16Torque >> 8);
            pu8Data[6] = 0;
            pu8Data[7] = 0;

            stsResult = WriteFrame(u32MsgId, 8, pu8Data); ;
            /*
            Console.WriteLine("=====================================================");
            Console.WriteLine("            Mensaje de Referncia ISMC A              ");
            Console.WriteLine("=====================================================");
            Console.WriteLine("Estado: " + ControlA_GetStatus().ToString());
            Console.WriteLine(" - Driver:  " + u8DriveCmd.ToString());
            Console.WriteLine(" - Control: " + u8CtrlCmd.ToString());
            Console.WriteLine(" - Tipo: " + u8CtrlType.ToString());
            */

        } // private void CAN_SendCtrlRefsB()

        /*******************************************************************************
         * Function Name : CAN_SendConsumerHBB
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Envía el mensaje de consumer herat beat para la placa ISMC B
         *******************************************************************************/
        private void CAN_SendConsumerHBA()
        {
            byte[] pu8Data = new byte[2];
            UInt32 u32MsgId = 0;

            TPCANStatus stsResult;

            u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_NMT_RX_A);

            pu8Data[0] = CCommunications.NodeIdA;
            pu8Data[1] = (0x00);


            stsResult = WriteFrame(u32MsgId, 2, pu8Data); ;


        } // private void CAN_SendCtrlRefsB()


        /*******************************************************************************
         * Function Name : CAN_SendConsumerHBB
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Envía el mensaje de consumer herat beat para la placa ISMC B
         *******************************************************************************/
        private void CAN_SendConsumerHBB()
        {
            byte[] pu8Data = new byte[2];
            UInt32 u32MsgId = 0;

            TPCANStatus stsResult;

            u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_NMT_TX_B);

            pu8Data[0] = CCommunications.NodeIdB;
            pu8Data[1] |= (0x00);


            stsResult = WriteFrame(u32MsgId, 2, pu8Data); ;


        } // private void CAN_SendCtrlRefsB()





        /*******************************************************************************
         * Function Name : CAN_OnDiagMsgA
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de calibración
         *******************************************************************************/
        private void CAN_OnDiagMsgA(byte [] pu8Bytes)
        {

            


            Console.WriteLine(" Diagnostic Msg A Received ");



            Console.WriteLine("Data:");
            Console.WriteLine("Comando de diagnóstico: " + pu8Bytes[0]);
            Console.WriteLine("Par de diagnóstico: " + pu8Bytes[2].ToString());

            // Se ha recibido un comando de respuesta correcto
            switch (pu8Bytes[0])
            {
                case ((byte)CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_NOTIFY_FAULT):
                    {
                        UInt32 u32DiagMask = 0;


                        byte[] pu8DiagMask = new byte[4] { 0, 0, 0, 0 };

                        pu8DiagMask[0] = pu8Bytes[4];
                        pu8DiagMask[1] = pu8Bytes[5];
                        pu8DiagMask[2] = pu8Bytes[6];
                        pu8DiagMask[3] = pu8Bytes[7];

                        //u32DiagMask = Convert.ToUInt32(pu8DiagMask);
                        u32DiagMask =  BitConverter.ToUInt32(pu8DiagMask, 0 /* Which byte position to convert */);


                        switch (u32DiagMask)
                        {

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_OCP):
                                {
                                    lFaultMsgA.Content = "Fault Message: Driver overcurrent";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32) CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_GDF):
                                {
                                    lFaultMsgA.Content = "Fault Message: Driver gate drive";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_UVLO):
                                {
                                    lFaultMsgA.Content = "Fault Message: Driver undervoltage";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_OTSD):
                                {
                                    lFaultMsgA.Content = "Fault Message: Driver overtemperature";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_HA):
                                {
                                    lFaultMsgA.Content = "Fault Message:  overcurrent on the A high-side MOSFET";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_LA):
                                {
                                    lFaultMsgA.Content = "Fault Message:  overcurrent on the A low-side MOSFET";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_HB):
                                {
                                    lFaultMsgA.Content = "Fault Message:  overcurrent on the B high-side MOSFET";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_LB):
                                {
                                    lFaultMsgA.Content = "Fault Message:  overcurrent on the B low-side MOSFET";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_HC):
                                {
                                    lFaultMsgA.Content = "Fault Message:  overcurrent on the C high-side MOSFET";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_LC):
                                {
                                    lFaultMsgA.Content = "Fault Message:  overcurrent on the C low-side MOSFET";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_COMM_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Producer Heartbeat";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32) CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_CTRL_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Control";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32) CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_TEMP_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Temperature";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32) CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_SPEED_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Average Speed ";
                                    if (m_i32DeltaPosA == 0)
                                    {
                                        lFaultMsgA2.Content = "The encoder does not detect movement";                                           
                                    }

                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    lFaultMsgA2.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32) CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_IAVG_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Average Current";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_MOTOR_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Is the motor connected?";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_ENC_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Is the encoder connected?";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_STO_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: STO Active";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_BRAKE_FAULT):
                                {
                                    lFaultMsgA.Content = "Fault Message: Brake Active";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                            default:
                                {
                                    lFaultMsgA.Content = "Fault Message: Unknow Code (" + u32DiagMask.ToString() + ")";
                                    lFaultMsgA.Visibility = Visibility.Visible;
                                    break;
                                }
                        } // switch (pu8Bytes[2])
                        break;
                    }
                case ((byte)CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_FAULT_RESET):
                    {
                        lFaultMsgA.Visibility = Visibility.Hidden;
                        lFaultMsgA2.Visibility = Visibility.Hidden;
                        break;
                        
                    }
                default:
                    {
                        break;
                    }
            } // switch (pu8Bytes[0])
        } // private void CAN_OnDiagMsgA(string sMessageData)

        /*******************************************************************************
         * Function Name : CAN_OnDiagMsgB
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de calibración
         *******************************************************************************/
        private void CAN_OnDiagMsgB(byte[] pu8Bytes)
        {
            Console.WriteLine(" Diagnostic Msg B Received ");




            Console.WriteLine("Data:");
            Console.WriteLine("Comando de diagnóstico: " + pu8Bytes[0]);
            Console.WriteLine("Par de diagnóstico: " + pu8Bytes[2]);

            // Se ha recibido un comando de respuesta correcto
            switch (pu8Bytes[0])
            {
                case ((byte)CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_NOTIFY_FAULT):
                    {
                        UInt32 u32DiagMask = 0;

                        byte[] pu8DiagMask = new byte[4] { 0, 0, 0, 0 };

                        pu8DiagMask[0] = pu8Bytes[4];
                        pu8DiagMask[1] = pu8Bytes[5];
                        pu8DiagMask[2] = pu8Bytes[6];
                        pu8DiagMask[3] = pu8Bytes[7];

                        u32DiagMask = BitConverter.ToUInt32(pu8DiagMask, 0 /* Which byte position to convert */);

                        switch (u32DiagMask)
                        {

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_OCP):
                                {
                                    lFaultMsgB.Content = "Fault Message: Driver overcurrent";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_GDF):
                                {
                                    lFaultMsgB.Content = "Fault Message: Driver gate drive";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_UVLO):
                                {
                                    lFaultMsgB.Content = "Fault Message: Driver undervoltage";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_OTSD):
                                {
                                    lFaultMsgB.Content = "Fault Message: Driver overtemperature";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_HA):
                                {
                                    lFaultMsgB.Content = "Fault Message:  overcurrent on the A high-side MOSFET";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_LA):
                                {
                                    lFaultMsgB.Content = "Fault Message:  overcurrent on the A low-side MOSFET";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_HB):
                                {
                                    lFaultMsgB.Content = "Fault Message:  overcurrent on the B high-side MOSFET";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_LB):
                                {
                                    lFaultMsgB.Content = "Fault Message:  overcurrent on the B low-side MOSFET";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_HC):
                                {
                                    lFaultMsgB.Content = "Fault Message:  overcurrent on the C high-side MOSFET";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_DRIVER_VDS_LC):
                                {
                                    lFaultMsgB.Content = "Fault Message:  overcurrent on the C low-side MOSFET";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_COMM_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Producer Heartbeat";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }

                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_CTRL_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Control";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_TEMP_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Temperature";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_SPEED_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Average Speed ";
                                    if (m_i32DeltaPosA == 0)
                                    {
                                        lFaultMsgB2.Content = "The encoder does not detect movement";
                                    }

                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    lFaultMsgB2.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_IAVG_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Average Current";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_MOTOR_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Is the motor connected?";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_ENC_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Is the encoder connected?";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_STO_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: STO Active";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                            case ((UInt32)CDiagnosis.TDIAGNOSIS_FAULT_MASK.DIAG_FLAG_MASK_BRAKE_FAULT):
                                {
                                    lFaultMsgB.Content = "Fault Message: Brake Active";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }                                 
                            default:
                                {
                                    lFaultMsgB.Content = "Fault Message: Unknow Code (" + pu8Bytes[2].ToString() + ")";
                                    lFaultMsgB.Visibility = Visibility.Visible;
                                    break;
                                }
                        } // switch (pu8Bytes[2])
                        break;
                    }
                case ((byte)CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_FAULT_RESET):
                    {
                        lFaultMsgB.Visibility = Visibility.Hidden;
                        lFaultMsgB2.Visibility = Visibility.Hidden;
                        break;
                    }
                default:
                    {
                        break;
                    }
            } // switch (pu8Bytes[0])
        } // private void CAN_OnDiagMsgA(string sMessageData)

        #endregion

        #region Help Functions

        private void UpdateStatusA(byte u8Status)
        {
            if (u8Status != m_u8StatusPastA)
            {
                switch(u8Status)
                {
                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:
                        {
                            iStatusDisabledA.Source = m_LogoLedEnable;
                            iStatusEnabledA.Source = m_LogoLedDisable;
                            iStatusSynchA.Source = m_LogoLedDisable;
                            iStatusRunA.Source = m_LogoLedDisable;
                            iStatusFaultA.Source = m_LogoLedDisable;
                            iECUStatusA.Source = m_ECUIdle;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_ENABLED:
                        {
                            iStatusDisabledA.Source = m_LogoLedDisable;
                            iStatusEnabledA.Source = m_LogoLedEnable;
                            iStatusSynchA.Source = m_LogoLedDisable;
                            iStatusRunA.Source = m_LogoLedDisable;
                            iStatusFaultA.Source = m_LogoLedDisable;
                            iECUStatusA.Source = m_ECUStop;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:


                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_CALC_OFFSET:
                        {
                            iStatusDisabledA.Source = m_LogoLedDisable;
                            iStatusEnabledA.Source = m_LogoLedDisable;
                            iStatusSynchA.Source = m_LogoLedEnable;
                            iStatusRunA.Source = m_LogoLedDisable;
                            iStatusFaultA.Source = m_LogoLedDisable;
                            iECUStatusA.Source = m_ECUSynch;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_RUN:
                        {
                            iStatusDisabledA.Source = m_LogoLedDisable;
                            iStatusEnabledA.Source = m_LogoLedDisable;
                            iStatusSynchA.Source = m_LogoLedDisable;
                            iStatusRunA.Source = m_LogoLedEnable;
                            iStatusFaultA.Source = m_LogoLedDisable;
                            iECUStatusA.Source = m_ECUPlay;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_FAULT:
                        {
                            iStatusDisabledA.Source = m_LogoLedDisable;
                            iStatusEnabledA.Source = m_LogoLedDisable;
                            iStatusSynchA.Source = m_LogoLedDisable;
                            iStatusRunA.Source = m_LogoLedDisable;
                            iStatusFaultA.Source = m_LogoLedEnable;
                            iECUStatusA.Source = m_ECUError;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_CONFIG_ERROR:
                        {
                            iStatusDisabledA.Source = m_LogoLedEnable;
                            iStatusEnabledA.Source = m_LogoLedDisable;
                            iStatusSynchA.Source = m_LogoLedDisable;
                            iStatusRunA.Source = m_LogoLedDisable;
                            iStatusFaultA.Source = m_LogoLedDisable;
                            iECUStatusA.Source = m_ECUIdle;
                            lFaultMsgA.Content = "Fault Message: Invalid Parameter ";
                            lFaultMsgA2.Content = "Error Code:";
                            

                            lFaultMsgA.Visibility = Visibility.Visible;
                            lFaultMsgA2.Visibility = Visibility.Visible;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_PARAM_ERROR:

                    default:
                        {
                            break;
                        }
                } // switch(u8Status)
            }
            else
            {

            }
            m_u8StatusPastA = u8Status;


            

            // Initialize Fault Flags

        }

        private void UpdateStatusB(byte u8Status)
        {
            if (u8Status != m_u8StatusPastB)
            {
                switch (u8Status)
                {
                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:
                        {
                            iStatusDisabledB.Source = m_LogoLedEnable;
                            iStatusEnabledB.Source = m_LogoLedDisable;
                            iStatusSynchB.Source = m_LogoLedDisable;
                            iStatusRunB.Source = m_LogoLedDisable;
                            iStatusFaultB.Source = m_LogoLedDisable;
                            iECUStatusB.Source = m_ECUIdle;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_ENABLED:
                        {
                            iStatusDisabledB.Source = m_LogoLedDisable;
                            iStatusEnabledB.Source = m_LogoLedEnable;
                            iStatusSynchB.Source = m_LogoLedDisable;
                            iStatusRunB.Source = m_LogoLedDisable;
                            iStatusFaultB.Source = m_LogoLedDisable;
                            iECUStatusB.Source = m_ECUStop;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:


                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_CALC_OFFSET:
                        {
                            iStatusDisabledB.Source = m_LogoLedDisable;
                            iStatusEnabledB.Source = m_LogoLedDisable;
                            iStatusSynchB.Source = m_LogoLedEnable;
                            iStatusRunB.Source = m_LogoLedDisable;
                            iStatusFaultB.Source = m_LogoLedDisable;
                            iECUStatusB.Source = m_ECUSynch;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_RUN:
                        {
                            iStatusDisabledB.Source = m_LogoLedDisable;
                            iStatusEnabledB.Source = m_LogoLedDisable;
                            iStatusSynchB.Source = m_LogoLedDisable;
                            iStatusRunB.Source = m_LogoLedEnable;
                            iStatusFaultB.Source = m_LogoLedDisable;
                            iECUStatusB.Source = m_ECUPlay;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_FAULT:
                        {
                            iStatusDisabledB.Source = m_LogoLedDisable;
                            iStatusEnabledB.Source = m_LogoLedDisable;
                            iStatusSynchB.Source = m_LogoLedDisable;
                            iStatusRunB.Source = m_LogoLedDisable;
                            iStatusFaultB.Source = m_LogoLedEnable;
                            iECUStatusB.Source = m_ECUError;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_DISABLED:

                    case (byte)CControl.CTRL_ST_FSM.CTRL_ST_CONFIG_ERROR:
                        {
                            iStatusDisabledB.Source = m_LogoLedDisable;
                            iStatusEnabledB.Source = m_LogoLedDisable;
                            iStatusSynchB.Source = m_LogoLedDisable;
                            iStatusRunB.Source = m_LogoLedDisable;
                            iStatusFaultB.Source = m_LogoLedEnable;
                            iECUStatusB.Source = m_ECUError;
                            lFaultMsgB.Content = "Fault Message: Invalid Parameter ";
                            lFaultMsgB2.Content = "Error Code:";


                            lFaultMsgB.Visibility = Visibility.Visible;
                            lFaultMsgB2.Visibility = Visibility.Visible;
                            break;
                        } // case (byte)CControl.CTRL_ST_FSM.CTRL_ST_PARAM_ERROR:

                    default:
                        {
                            break;
                        }
                } // switch(u8Status)
            }
            else
            {

            }
            m_u8StatusPastB = u8Status;

            // Initialize Fault Flags

        }

        #endregion

        private void TestHeartBeat_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_bTestHB == true)
            {
                m_bTestHB = false;
                tmrConHB.Stop();

                iHTB.Source = m_HeartBeatDis;
            }
            else
            {
                m_bTestHB = false;
                tmrConHB.Start();
                iHTB.Source = m_HeartBeatEn;
            }
        }

        private void Control_OnKeyDown(object sender, KeyEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F1)
            {
                tTestHeartBeat.Visibility = Visibility.Visible;


            }
        }

        private void iPHB_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

        }


    }
}
