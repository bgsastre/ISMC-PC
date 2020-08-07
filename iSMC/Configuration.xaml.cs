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
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using TPCANHandle = System.UInt16;
using TPCANBitrateFD = System.String;
using TPCANTimestampFD = System.UInt64;
using Peak.Can.Basic;

namespace iSMC
{
    /// <summary>
    /// Lógica de interacción para Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        #region Delegates
        /// <summary>
        /// Read-Delegate Handler
        /// </summary>
        private delegate void ReadDelegateHandler();
        #endregion


        private Window m_pWindow;


        string m_sIdentifier = "";
        string m_sPolePairs = "";
        string m_sRs = "";
        string m_sLs = "";
        string m_sFlux = "";
        string m_sInertia = "";
        string m_sFriction = "";
        string m_sSensorType = "";
        string m_sNLines = "";

        string m_sSpeedPIKp = "";
        string m_sSpeedPIKi = "";
        string m_sCurrentPIKp = "";
        string m_sCurrentPIKi = "";
        string m_sBandwidth = "";
        string m_sCtrlMode = "";


        string m_sImax = "";
        string m_sIavg = "";
        string m_sTIavg = "";
        string m_sSmax = "";
        string m_sTSavg = "";
        string m_sTempMax = "";
        string m_sTHB = "";
        string m_sTComms = "";
        string m_sAmax = "";

        string m_sBaudrate = "";

        string m_sFullScaleFreq = "";
        string m_sResEstIMax = "";
        string m_sIndEstIMax = "";
        string m_sFluxEstFreq = "";
        string m_sRoverLFreq = "";

        Int32 m_TabPast = 0;

        /// <summary>
        /// Read Delegate for calling the function "ReadMessages"
        /// </summary>
        private ReadDelegateHandler m_ReadDelegate;

        BitmapImage m_LogoWriteInfoEn  = new BitmapImage();
        BitmapImage m_LogoWriteInfoDis = new BitmapImage();

        BitmapImage m_SaveIconEn  = new BitmapImage();
        BitmapImage m_SaveIconDis = new BitmapImage();

        BitmapImage m_SelectISMCEn = new BitmapImage();
        BitmapImage m_SelectISMCDis = new BitmapImage();

        Int32 m_s32Idx = 0;


        private List<MemoryData> m_MemData;

        public Configuration(Window pWindow)
        {

            m_pWindow = pWindow;
            InitializeComponent();
           
            tNameA.Text = CCalibration.GetMotorName(Globals.ECU_ISMC_A);
            tPolePairsA.Text = CCalibration.GetPolePairs(Globals.ECU_ISMC_A).ToString();
            tRsA.Text = CCalibration.GetResistance(Globals.ECU_ISMC_A).ToString();
            tLsA.Text = CCalibration.GetInductance(Globals.ECU_ISMC_A).ToString();
            tRatedFluxA.Text = CCalibration.GetRatedFlux(Globals.ECU_ISMC_A).ToString();
            tInertiaA.Text = CCalibration.GetInertia(Globals.ECU_ISMC_A).ToString();
            tFrictionA.Text = CCalibration.GetFriction(Globals.ECU_ISMC_A).ToString();
            tMaxCurrentA.Text = CCalibration.GetMaxCurrent(Globals.ECU_ISMC_A).ToString();


            byte u8Sensor = 0;
            u8Sensor = CCalibration.GetSensor(Globals.ECU_ISMC_A);
            if (u8Sensor == 0)
            {

            }
            tNumberLinesA.Text = CCalibration.GetResolution(Globals.ECU_ISMC_A).ToString();


            tSpeedKpA.Text = CCalibration.GetSpdCtrlKp(Globals.ECU_ISMC_A).ToString();
            tSpeedKiA.Text = CCalibration.GetSpdCtrlKi(Globals.ECU_ISMC_A).ToString();
            tCurrentKpA.Text = CCalibration.GetIdqCtrlKp(Globals.ECU_ISMC_A).ToString();
            tCurrentKiA.Text = CCalibration.GetIdqCtrlKi(Globals.ECU_ISMC_A).ToString();
            tBandwidthA.Text = CCalibration.GetCtrlBandwidth(Globals.ECU_ISMC_A).ToString();
            // Limits
            byte u8Param = 0;

            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT;
            tMaxCurrentA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT;
            tAvgCurrentA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT;
            tTimeAvgCurrentA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM;
            tMaxSpeedA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED;
            tTimeAvgSpeedA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX;
            tMaxTempA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS;
            tTimeHeartBeat.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS;
            tTimeComms.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL;
            tMaxAccelA.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();


            tEncOffsetCurrA.Text = CCalibration.GetIdentResistance(Globals.ECU_ISMC_A).ToString();


            tNameB.Text = CCalibration.GetMotorName(Globals.ECU_ISMC_B);
            tPolePairsB.Text = CCalibration.GetPolePairs(Globals.ECU_ISMC_B).ToString();
            tRsB.Text = CCalibration.GetResistance(Globals.ECU_ISMC_B).ToString();
            tLsB.Text = CCalibration.GetInductance(Globals.ECU_ISMC_B).ToString();
            tRatedFluxB.Text = CCalibration.GetRatedFlux(Globals.ECU_ISMC_B).ToString();
            tInertiaB.Text = CCalibration.GetInertia(Globals.ECU_ISMC_B).ToString();
            tFrictionB.Text = CCalibration.GetFriction(Globals.ECU_ISMC_B).ToString();
            tMaxCurrentB.Text = CCalibration.GetMaxCurrent(Globals.ECU_ISMC_B).ToString();


            byte u8SensorB = 0;
            u8SensorB = CCalibration.GetSensor(Globals.ECU_ISMC_B);
            if (u8SensorB == 0)
            {

            }
            tNumberLinesB.Text = CCalibration.GetResolution(Globals.ECU_ISMC_B).ToString();


            tSpeedKpB.Text = CCalibration.GetSpdCtrlKp(Globals.ECU_ISMC_B).ToString();
            tSpeedKiB.Text = CCalibration.GetSpdCtrlKi(Globals.ECU_ISMC_B).ToString();
            tCurrentKpB.Text = CCalibration.GetIdqCtrlKp(Globals.ECU_ISMC_B).ToString();
            tCurrentKiB.Text = CCalibration.GetIdqCtrlKi(Globals.ECU_ISMC_B).ToString();
            tBandwidthB.Text = CCalibration.GetCtrlBandwidth(Globals.ECU_ISMC_B).ToString();
            // Limits
            

            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT;
            tMaxCurrentB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT;
            tAvgCurrentB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT;
            tTimeAvgCurrentB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM;
            tMaxSpeedB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED;
            tTimeAvgSpeedB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX;
            tMaxTempB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS;
            tTimeHeartBeatB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS;
            tTimeCommsB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL;
            tMaxAccelB.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_B, u8Param).ToString();

            tEncOffsetCurrB.Text = CCalibration.GetIdentResistance(Globals.ECU_ISMC_B).ToString();


            // Params to write
            // Limits
            u8Param = 0;

            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT;
            tMaxCurrent.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT;
            tAvgCurrent.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT;
            tTimeAvgCurrent.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM;
            tMaxSpeed.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED;
            tTimeAvgSpeed.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX;
            tMaxTemp.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS;
            tTimeHeartBeat.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS;
            tTimeComms.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL;
            tMaxAccel.Text = CCalibration.GetLimitParam(Globals.ECU_ISMC_A, u8Param).ToString();
            tEncOffsetCurr.Text = CCalibration.GetIdentResistance(Globals.ECU_ISMC_A).ToString();
            tNumberLines.Text = CCalibration.GetResolution(Globals.ECU_ISMC_A).ToString();


            m_LogoWriteInfoEn.BeginInit();
            m_LogoWriteInfoEn.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/WriteFlash.png");
            m_LogoWriteInfoEn.EndInit();

            m_LogoWriteInfoDis.BeginInit();
            m_LogoWriteInfoDis.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/WriteFlashd.png");
            m_LogoWriteInfoDis.EndInit();



            m_SaveIconEn.BeginInit();
            m_SaveIconEn.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/Save.jpg");
            m_SaveIconEn.EndInit();

            m_SaveIconDis.BeginInit();
            m_SaveIconDis.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/Saved.jpg");
            m_SaveIconDis.EndInit();


            m_SelectISMCEn.BeginInit();
            m_SelectISMCEn.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/iSMCA.PNG");
            m_SelectISMCEn.EndInit();

            m_SelectISMCDis.BeginInit();
            m_SelectISMCDis.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/HeartBeatOffd.PNG");
            m_SelectISMCDis.EndInit();
            // Check Current Motor
            byte u8ECUId = Globals.ECU_ISMC_A;

            u8ECUId = CCalibration.GetECUSel();

            UInt32[] pu32Id = { 0, 0 };

            BitmapImage logo1 = new BitmapImage();

            if (u8ECUId == Globals.ECU_ISMC_B)
            {
                CCalibration.SetECUSel(Globals.ECU_ISMC_A);
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/iSMCA.PNG");
                logo1.EndInit();
            }
            else
            {
                CCalibration.SetECUSel(Globals.ECU_ISMC_B);
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/iSMCB.PNG");
                logo1.EndInit();
            }
            ImgMotorSel.Source = logo1;


            m_MemData = new List<MemoryData>();

            Mem_FillList();

            dgFlashData.ItemsSource = m_MemData;


            int s32SelIdx = lbMotors.SelectedIndex;

            if (s32SelIdx < 0)
            {
                iWriteInfo.Source = m_LogoWriteInfoDis;
                iWriteInfo.IsEnabled = false;
            }
            else
            {
                iWriteInfo.Source = m_LogoWriteInfoEn;
                iWriteInfo.IsEnabled = true;
            }

            
            // Creates the delegate used for message reading
            //
            m_ReadDelegate = new ReadDelegateHandler(ReadMessages);

            CANConnect();
        }



        #region Funciones de atención a botones
        /*******************************************************************************
         * Function Name : Window_MouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Función que atiende al evento click sobre la ventana "Calibración". Si el
         *  botón se mantiene presionado, se hace caso a las ordenes de mover pantalla.
         *******************************************************************************/
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        } // private void Window_MouseDown(object sender, MouseButtonEventArgs e)

        /*******************************************************************************
         * Function Name : iReturn_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * 
         *******************************************************************************/
        private void iReturn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            m_pWindow.Show();
        } //  private void iReturn_OnMouseDown(object sender, MouseButtonEventArgs e)

        private void ISMCSel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8ECUId = Globals.ECU_ISMC_A;

            u8ECUId = CCalibration.GetECUSel();

            UInt32[] pu32Id = { 0, 0 };

            BitmapImage logo1 = new BitmapImage();

            if (u8ECUId == Globals.ECU_ISMC_B)
            {
                CCalibration.SetECUSel(Globals.ECU_ISMC_A);
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/iSMCA.PNG");
                logo1.EndInit();
            }
            else
            {
                CCalibration.SetECUSel(Globals.ECU_ISMC_B);
                logo1.BeginInit();
                logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/iSMCB.PNG");
                logo1.EndInit();
            }
            ImgMotorSel.Source = logo1;
        } // private void MotorASel_OnMouseDown(object sender, MouseButtonEventArgs e)

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

        private void imgSave_OnMouseDown(object sender, MouseButtonEventArgs e)
        {





            SaveConfig wSaveConfig = new SaveConfig();

            wSaveConfig.ShowDialog();



            string sConfigName = Globals.GetConfigName();

            if (sConfigName.Length > 0)
            {

                sConfigName += ".xml";

                XmlWriter writer = XmlWriter.Create(sConfigName);


                
                string sName =  CCalibration.GetMotorName(Globals.ECU_ISMC_A);
                string sPolePairs = tPolePairsA.Text;
                sPolePairs = sPolePairs.Replace('.', ',');
                string sRs =tRsA.Text;
                sRs = sRs.Replace('.', ',');
                string sLs =  tLsA.Text;
                sLs = sLs.Replace('.', ',');
                string sFlux =  tRatedFluxA.Text;
                sFlux = sFlux.Replace('.', ',');
                string sInertia = tInertiaA.Text;
                sInertia = sInertia.Replace('.', ',');
                string sFriction = tFrictionA.Text;
                sFriction = sFriction.Replace('.', ',');
                string sMaxCurrent = tMaxCurrentA.Text;
                sMaxCurrent = sMaxCurrent.Replace('.', ',');
                string sSensor = "2";
                sSensor = sSensor.Replace('.', ',');



                string sSpeedKp = tSpeedKpA.Text;
                sSpeedKp = sSpeedKp.Replace('.', ',');
                string sSpeedKi = tSpeedKiA.Text;
                sSpeedKi = sSpeedKi.Replace('.', ',');
                string sCurrentKp = tCurrentKpA.Text;
                sCurrentKp = sCurrentKp.Replace('.', ',');
                string sCurrentKi = tCurrentKiA.Text;
                sCurrentKi = sCurrentKi.Replace('.', ',');
                string sBandwidth = tBandwidthA.Text;
                sBandwidth = sBandwidth.Replace('.', ',');
                // Limits

                /* Parámetros modificables */
                string sLimMaxCurrent = tMaxCurrent.Text;
                sLimMaxCurrent = sLimMaxCurrent.Replace('.', ',');
                string sLimAVGCurrent = tAvgCurrent.Text;
                sLimAVGCurrent = sLimAVGCurrent.Replace('.', ',');
                string sLimTimeAVGCurrent = tTimeAvgCurrent.Text;
                sLimTimeAVGCurrent = sLimTimeAVGCurrent.Replace('.', ',');
                string sLimMaxSpeed = tMaxSpeed.Text;
                sLimMaxSpeed = sLimMaxSpeed.Replace('.', ',');
                string sLimAvgSpeed = tTimeAvgSpeed.Text;
                sLimAvgSpeed = sLimAvgSpeed.Replace('.', ',');
                string sLimMaxTemp = tMaxTemp.Text;
                sLimMaxTemp = sLimMaxTemp.Replace('.', ',');
                string sEncMaxCurr = tEncOffsetCurr.Text;
                sEncMaxCurr = sEncMaxCurr.Replace('.', ',');
                string sLimTimeHB = tTimeHeartBeat.Text;
                sLimTimeHB = sLimTimeHB.Replace('.', ',');
                string sLimTimeComms = tTimeComms.Text;
                sLimTimeComms = sLimTimeComms.Replace('.', ',');
                string sLimMaxAccel = tMaxAccel.Text;
                sLimMaxAccel = sLimMaxAccel.Replace('.', ',');
                string sResolution = tNumberLines.Text;
                sResolution = sResolution.Replace('.', ',');

                writer.WriteStartElement("motor");
                int start = 0;
                int count = 0;

                int at = sName.IndexOf("\0", start, count);
                sName = Regex.Replace(sName, "[^a-zA-Z0-9]", "");
                writer.WriteElementString("Identifier", sName);
                writer.WriteElementString("PolePairs", sPolePairs);
                writer.WriteElementString("Rs", sRs);
                writer.WriteElementString("Ls", sLs);
                writer.WriteElementString("Flux", sFlux);
                writer.WriteElementString("Inertia", sInertia);
                writer.WriteElementString("Friction", sFriction);
                writer.WriteElementString("SensorType", sSensor);
                writer.WriteElementString("NLines", sResolution);
                writer.WriteElementString("Offset", "0");

                writer.WriteElementString("SpeedPIKp", sSpeedKp);
                writer.WriteElementString("SpeedPIKi", sSpeedKi);
                writer.WriteElementString("CurrentPIKp", sCurrentKp);
                writer.WriteElementString("CurrentPIKi", sCurrentKi);
                writer.WriteElementString("Bandwidth", sBandwidth);
                writer.WriteElementString("CtrlMode", "2");

                writer.WriteElementString("Imax", sLimMaxCurrent);
                writer.WriteElementString("Iavg", sLimAVGCurrent);
                writer.WriteElementString("TIavg", sLimTimeAVGCurrent);
                writer.WriteElementString("Smax", sLimMaxSpeed);                
                writer.WriteElementString("TSavg", sLimAvgSpeed);
                writer.WriteElementString("TempMax", sLimMaxTemp);
                writer.WriteElementString("THB", sLimTimeHB);
                writer.WriteElementString("TComms", sLimTimeComms);
                writer.WriteElementString("Amax", sLimMaxAccel);
                writer.WriteElementString("Baudrate", "2,0");
                writer.WriteElementString("FullScaleFreq", "330,0");
                writer.WriteElementString("ResEstIMax", sEncMaxCurr);
                writer.WriteElementString("IndEstIMax", "2,0");
                writer.WriteElementString("FluxEstFreq", "13,0");
                writer.WriteElementString("RoverLFreq", "100,0");

                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
                /*
                writer = XmlWriter.Create("CurrentMotor.xml");

                writer.WriteStartElement("motor");
                writer.WriteElementString("Identifier", sName);
                writer.WriteElementString("PolePairs", sPolePairs);
                writer.WriteElementString("Rs", sRs);
                writer.WriteElementString("Ls", sLs);
                writer.WriteElementString("Flux", sFlux);
                writer.WriteElementString("Inertia", sInertia);
                writer.WriteElementString("Friction", sFriction);
                writer.WriteElementString("SensorType", sSensor);
                writer.WriteElementString("NLines", sResolution);
                writer.WriteElementString("Offset", "0");

                writer.WriteElementString("SpeedPIKp", sSpeedKp);
                writer.WriteElementString("SpeedPIKi", sSpeedKi);
                writer.WriteElementString("CurrentPIKp", sCurrentKp);
                writer.WriteElementString("CurrentPIKi", sCurrentKi);
                writer.WriteElementString("Bandwidth", sBandwidth);
                writer.WriteElementString("CtrlMode", "2");

                writer.WriteElementString("Imax", sLimMaxCurrent);
                writer.WriteElementString("Iavg", sLimAVGCurrent);
                writer.WriteElementString("TIavg", sLimTimeAVGCurrent);
                writer.WriteElementString("Smax", sLimMaxSpeed);
                writer.WriteElementString("TSavg", sLimAvgSpeed);
                writer.WriteElementString("TempMax", sLimMaxTemp);
                writer.WriteElementString("THB", sLimTimeHB);
                writer.WriteElementString("TComms", sLimTimeComms);
                writer.WriteElementString("Amax", sLimMaxAccel);
                writer.WriteElementString("Baudrate", "2,0");
                writer.WriteElementString("FullScaleFreq", "330,0");
                writer.WriteElementString("ResEstIMax", sEncMaxCurr);
                writer.WriteElementString("IndEstIMax", "2,0");
                writer.WriteElementString("FluxEstFreq", "13,0");
                writer.WriteElementString("RoverLFreq", "100,0");
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
                */

            }



        }

        private void lbMotors_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iWriteInfo.Source = m_LogoWriteInfoEn;
            iWriteInfo.IsEnabled = true;


            Int32 s32Idx = lbMotors.SelectedIndex;


            if ((s32Idx < 8) && (s32Idx >= 0))
            { 
                m_s32Idx = s32Idx;



                string strName = lbMotors.Items[s32Idx].ToString();


                ReadFromFile(strName);




                string sValue = "";

                {
                    int s32Address = 0;

                    // PolePairs
                    sValue = m_sPolePairs;
                    sValue = sValue.Replace('.', ',');


                    byte[] pu8MotorData = new byte[60];

                    m_MemData.Where(w => w.Name == "PolePairs").ToList().ForEach(s => s.Value = sValue);
                    double dValue = Convert.ToDouble(sValue);
                    UInt32 u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q8.24
                    byte[] pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Rs
                    sValue = m_sRs;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Rs").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;



                    // Ls
                    sValue = m_sLs;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Ls").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // RatedFlux
                    sValue = m_sFlux;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "RatedFlux").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Inertia
                    sValue = m_sInertia;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Inertia").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Friction
                    sValue = m_sFriction;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Friction").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;


                    // RatedCurrent
                    sValue = m_sImax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Rated Current").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Max Current
                    sValue = m_sImax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Max. Current").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Max Speed
                    sValue = m_sSmax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Max. Speed").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Sensor
                    sValue = m_sSensorType;
                    sValue = sValue.Replace('.', ',');
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    m_MemData.Where(w => w.Name == "Sensor").ToList().ForEach(s => s.Value = sValue);
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Sensor Resolution
                    sValue = m_sNLines;
                    sValue = sValue.Replace('.', ',');
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    m_MemData.Where(w => w.Name == "Sensor Resolution").ToList().ForEach(s => s.Value = sValue);
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8MotorData, s32Address);
                    s32Address += 4;

                    // Identifier
                    sValue = m_sIdentifier;
                    m_MemData.Where(w => w.Name == "Motor Name").ToList().ForEach(s => s.Value = sValue);
                    int iNumberChars = sValue.Length;
                    byte[] pu8Bytes2 = Encoding.ASCII.GetBytes(sValue);
                    Array.Resize(ref pu8Bytes2, 16);

                    for (int i = sValue.Length; i < 16; i++)
                    {
                        //pu8Bytes12[i / 2] = Convert.ToByte(strHex.Substring(i, 2), 16);
                        pu8Bytes2[i] = 0;
                    }
                    Array.Reverse(pu8Bytes2, 0, pu8Bytes2.Length);
                    pu8Bytes2.CopyTo(pu8MotorData, s32Address);
                    s32Address += 16;

                    string strCRC = CRC_Sector(pu8MotorData, 60);
                    m_MemData[12].Value = strCRC;

                } // MOTOR

                {

                    byte[] pu8ControlData = new byte[60];
                    int s32Address = 0;
                    // Speed PI Kp
                    sValue = m_sSpeedPIKp;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Speed PI Kp").ToList().ForEach(s => s.Value = sValue);
                    double dValue = Convert.ToDouble(sValue);
                    UInt32 u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    byte[] pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8ControlData, s32Address);
                    s32Address += 4;

                    // Speed PI Kp
                    sValue = m_sSpeedPIKi;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Speed PI Ki").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8ControlData, s32Address);
                    s32Address += 4;

                    // Idq PI Kp
                    sValue = m_sCurrentPIKp;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Idq PI Kp").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8ControlData, s32Address);
                    s32Address += 4;

                    // Idq PI Ki
                    sValue = m_sCurrentPIKi;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Idq PI Ki").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8ControlData, s32Address);
                    s32Address += 4;

                    // Bandwidth
                    sValue = m_sBandwidth;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Control Bandwidth").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8ControlData, s32Address);
                    s32Address += 4;

                    // Control Mode
                    sValue = m_sCtrlMode;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Control Mode").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8ControlData, s32Address);
                    s32Address += 4;

                    string strCRC = CRC_Sector(pu8ControlData, 24);
                    m_MemData[19].Value = strCRC;
                } // CONTROL

                {
                    byte[] pu8LimitData = new byte[36];
                    int s32Address = 0;

                    // Max Current
                    sValue = m_sImax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Current Max.").ToList().ForEach(s => s.Value = sValue);
                    double dValue = Convert.ToDouble(sValue);
                    UInt32 u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    byte[] pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Current Avg
                    sValue = m_sIavg;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Current Avg").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Time to current average
                    sValue = m_sTIavg;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Time to Current Avg.").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Max. Speed
                    sValue = m_sSmax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Speed Max.").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Time to speed average
                    sValue = m_sTSavg;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Time to Speed Avg.").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Temp. Max
                    sValue = m_sTempMax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Temp. Max.").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Heart Beat Comms
                    sValue = m_sTHB;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Heart Beat Comms").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Timeout Comms
                    sValue = m_sTComms;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Timeout Comms").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    // Accel Max
                    sValue = m_sAmax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Amax").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8LimitData, s32Address);
                    s32Address += 4;

                    string strCRC = CRC_Sector(pu8LimitData, 36);
                    m_MemData[29].Value = strCRC;
                } // LIMITS

                {

                    byte[] pu8CommData = new byte[4];


                    // Baudrate
                    sValue = m_sBaudrate;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "Baudrate").ToList().ForEach(s => s.Value = sValue);
                    double dValue = Convert.ToDouble(sValue);
                    UInt32 u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                    byte[] pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8CommData, 0);

                    string strCRC = CRC_Sector(pu8CommData, 4);
                    m_MemData[31].Value = strCRC;
                } // COMMS

                {
                    byte[] pu8IdentData = new byte[20];

                    // Full Scale Frequency
                    sValue = m_sFullScaleFreq;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "FullScaleFreq").ToList().ForEach(s => s.Value = sValue);
                    double dValue = Convert.ToDouble(sValue);
                    UInt32 u32Value = Convert.ToUInt32(Math.Floor(dValue * 65536.0)); // Q16.16
                    byte[] pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8IdentData, 0);

                    // Current used for the Rs identification process
                    sValue = m_sResEstIMax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "ResEstIMax").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8IdentData, 4);

                    // Current used for the Ls identification process
                    sValue = m_sIndEstIMax;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "IndEstIMax").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8IdentData, 8);

                    // Frequency used for the Flux identification process
                    sValue = m_sFluxEstFreq;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "FluxEstFreq").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8IdentData, 12);

                    // Frequency used for the Flux identification process
                    sValue = m_sRoverLFreq;
                    sValue = sValue.Replace('.', ',');
                    m_MemData.Where(w => w.Name == "RoverLEstFreq").ToList().ForEach(s => s.Value = sValue);
                    dValue = Convert.ToDouble(sValue);
                    u32Value = Convert.ToUInt32(Math.Floor(dValue * 65536.0)); // Q16.16
                    pu8Bytes1 = BitConverter.GetBytes(u32Value);
                    Array.Reverse(pu8Bytes1, 0, pu8Bytes1.Length);
                    pu8Bytes1.CopyTo(pu8IdentData, 16);

                    string strCRC = CRC_Sector(pu8IdentData, 20);
                    m_MemData[37].Value = strCRC;
                } // IDENT

                dgFlashData.ItemsSource = null;
                dgFlashData.ItemsSource = m_MemData;
            }
        }

        private void TabParam_OnSelChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine("SelectionChanged");

            int s32Index = TabPar.SelectedIndex;
            Console.WriteLine("Tab:" + s32Index.ToString());



            if ((s32Index == 5))
            {
                if (m_TabPast != 5)
                {
                    TabParam_Fill();
                }

                ImgMotorSel.Source = m_SelectISMCEn;
                CCalibration.SetECUSel(Globals.ECU_ISMC_A);
            }
            else
            {
                ImgMotorSel.Source = m_SelectISMCDis;
                CCalibration.SetECUSel(Globals.ECU_ISMC_A);
            }



            if (s32Index == 4) 
            {
                ImgSave.Source = m_SaveIconEn;
            }
            else
            {
                ImgSave.Source = m_SaveIconDis;
            }

            m_TabPast = s32Index;
        }


        private void TabParam_Fill()
        {
            int s32Items = lbMotors.Items.Count;

            if (s32Items > 0)
            {
                lbMotors.Items.Clear();
            }

            string sCurrentDir = Globals.GetCurrentDir();
            DirectoryInfo d = new DirectoryInfo(sCurrentDir);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
            string[] str = new string[128];
            byte u8FileCnt = 0;
            foreach (FileInfo file in Files)
            {
                str[u8FileCnt] = file.Name;
                u8FileCnt++;
            }

            byte u8Cnt = 0;
            for (u8Cnt = 0; u8Cnt < u8FileCnt; u8Cnt++)
            {
                string strMotorName1 = str[u8Cnt];
                string strMotorName2 = "CurrentMotor.xml";
                if (strMotorName1 == strMotorName2)
                {

                }
                else
                {
                    string strAddMotor = System.IO.Path.GetFileNameWithoutExtension(str[u8Cnt]);
                    lbMotors.Items.Add(strAddMotor);
                }
            }



        }


        /*******************************************************************************
         * Function Name : CRC_Sector
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         : 
         *  Calculo del CRC para la sección de parámetros de comunicaciones
         *******************************************************************************/
        private string CRC_Sector(byte[] pu8Data, int s32Lenght)
        {



            UInt16 u16CRC = ModRTU_CRC(pu8Data, s32Lenght);

            byte[] pu8Bytes = BitConverter.GetBytes(u16CRC);

            double dValue = pu8Bytes[0] * 256;
            dValue += pu8Bytes[1];

            dgFlashData.ItemsSource = null;
            dgFlashData.ItemsSource = m_MemData;

            return dValue.ToString();

        } //  private void CRC_Limit()

        /*******************************************************************************
         * Function Name : ModRTU_CRC
         * Parameters    : byte [] Array de datos
         *               : int     Longitud del array de datos
         * Returns       : Uint16  CRC16
         *******************************************************************************
         * Notes         : 
         * Compute the MODBUS RTU CRC
         *******************************************************************************/
        private UInt16 ModRTU_CRC(byte[] pu8Buffer, int len)
        {
            UInt16 u16CRC = 0xFFFF;

            for (int s32BufferIndex = 0; s32BufferIndex < len; s32BufferIndex++)
            {
                u16CRC ^= (UInt16)pu8Buffer[s32BufferIndex];          // XOR byte into least sig. byte of u16CRC

                for (int i = 8; i != 0; i--)
                {    // Loop over each bit
                    if ((u16CRC & 0x0001) != 0)
                    {      // If the LSB is set
                        u16CRC >>= 1;                    // Shift right and XOR 0xA001
                        u16CRC ^= 0xA001;
                    }
                    else                            // Else LSB is not set
                        u16CRC >>= 1;              // Just shift right
                }
            }
            // Note, this number has low and high bytes swapped, so use it accordingly (or swap bytes)
            return u16CRC;
        } // private UInt16 ModRTU_CRC(byte[] pu8Buffer, int len)

        /*******************************************************************************
         * Function Name : Mem_FillList
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Inicializa los valores de la lista
         *******************************************************************************/
        private void Mem_FillList()
        {
            // Motor
            m_MemData.Add(new MemoryData() { Address = "0x0000", Name = "PolePairs",            Value = "", Description = "PAIRS, not total poles. Used to calculate user RPM from rotor Hz only" });
            m_MemData.Add(new MemoryData() { Address = "0x0004", Name = "Rs",                   Value = "", Description = "Identified phase to neutral resistance in a Y equivalent circuit (Ohms)" });
            m_MemData.Add(new MemoryData() { Address = "0x0008", Name = "Ls",                   Value = "", Description = "Identified average stator inductance  (Henry)" });
            m_MemData.Add(new MemoryData() { Address = "0x000C", Name = "RatedFlux",            Value = "", Description = "Identified TOTAL flux linkage between the rotor and the stator (V/Hz)" });
            m_MemData.Add(new MemoryData() { Address = "0x0010", Name = "Inertia",              Value = "", Description = "Identified motor inertia" });
            m_MemData.Add(new MemoryData() { Address = "0x0014", Name = "Friction",             Value = "", Description = "Identified motor friction" });
            m_MemData.Add(new MemoryData() { Address = "0x0018", Name = "Rated Current",        Value = "", Description = "Reserved. Not Used" });
            m_MemData.Add(new MemoryData() { Address = "0x001C", Name = "Max. Current",         Value = "", Description = "Reserved. Not Used" });
            m_MemData.Add(new MemoryData() { Address = "0x0020", Name = "Max. Speed",           Value = "", Description = "Reserved. Not Used" });
            m_MemData.Add(new MemoryData() { Address = "0x0024", Name = "Sensor",               Value = "", Description = "Type of sensor. 0) Sensorless, 1) Hall, 2) Encoder" });
            m_MemData.Add(new MemoryData() { Address = "0x0028", Name = "Sensor Resolution",    Value = "", Description = "Sensor resolution (Encoder)" });
            m_MemData.Add(new MemoryData() { Address = "0x002C", Name = "Motor Name",           Value = "", Description = "Motor Name" });
            m_MemData.Add(new MemoryData() { Address = "0x003C", Name = "CRC",                  Value = "", Description = "CRC16-Modbus for Motor Param section" });
            // Control
            m_MemData.Add(new MemoryData() { Address = "0x003E", Name = "Speed PI Kp",          Value = "", Description = "Kp constant for the Speed Controller PI" });
            m_MemData.Add(new MemoryData() { Address = "0x0042", Name = "Speed PI Ki",          Value = "", Description = "Ki constant for the Speed Controller PI" });
            m_MemData.Add(new MemoryData() { Address = "0x0046", Name = "Idq PI Kp",            Value = "", Description = "Kp constant for the Torque Controller PI" });
            m_MemData.Add(new MemoryData() { Address = "0x004A", Name = "Idq PI Ki",            Value = "", Description = "Ki constant for the Torque Controller PI" });
            m_MemData.Add(new MemoryData() { Address = "0x004E", Name = "Control Bandwidth",    Value = "", Description = "Bandwidth value for the active disturbance rejection control" });
            m_MemData.Add(new MemoryData() { Address = "0x0052", Name = "Control Mode",         Value = "", Description = "Control Mode" });
            m_MemData.Add(new MemoryData() { Address = "0x0056", Name = "CRC",                  Value = "", Description = "CRC16-Modbus value for Controllers section" });
            // Supervisiones
            m_MemData.Add(new MemoryData() { Address = "0x0058", Name = "Current Max.",         Value = "", Description = "Limit on the maximum current command output of the provided Speed PI Controller to the Iq controller" });
            m_MemData.Add(new MemoryData() { Address = "0x005C", Name = "Current Avg",          Value = "", Description = "Limit on the average current command output of the provided Speed PI Controller to the Iq controller" });
            m_MemData.Add(new MemoryData() { Address = "0x0060", Name = "Time to Current Avg.", Value = "", Description = "Limit on the average current command output of the provided Speed PI Controller to the Iq controller" });
            m_MemData.Add(new MemoryData() { Address = "0x0064", Name = "Speed Max.",           Value = "", Description = "Velocidad máxima en KRPM para el motor" });
            m_MemData.Add(new MemoryData() { Address = "0x0068", Name = "Time to Speed Avg.",   Value = "", Description = "Timeout for speed disturbance detection" });
            m_MemData.Add(new MemoryData() { Address = "0x006C", Name = "Temp. Max.",           Value = "", Description = "Maximum temperature allowed in the ISMC" });
            m_MemData.Add(new MemoryData() { Address = "0x0070", Name = "Heart Beat Comms",     Value = "", Description = "Timeout to detected loss of communication in the ISMC side" });
            m_MemData.Add(new MemoryData() { Address = "0x0074", Name = "Timeout Comms",        Value = "", Description = "Timeout to detected loss of communication in the APP side" });
            m_MemData.Add(new MemoryData() { Address = "0x0078", Name = "Amax",                 Value = "", Description = "Maximun Acceleration for the speed control [KRPM/s]" });
            m_MemData.Add(new MemoryData() { Address = "0x007C", Name = "CRC",                  Value = "", Description = "CRC16-Modbus value for Limits sections" });
            // Comunicaciones
            m_MemData.Add(new MemoryData() { Address = "0x007E", Name = "Baudrate",             Value = "", Description = "Baudrate 0) 1000 1) 500 2) 250 " });
            m_MemData.Add(new MemoryData() { Address = "0x0082", Name = "CRC",                  Value = "", Description = "CRC16-Modbus value for Communications sections" });
            // Parámetros de identificación
            m_MemData.Add(new MemoryData() { Address = "0x0084", Name = "FullScaleFreq",        Value = "", Description = "Full Scale Frequency in Hz" });
            m_MemData.Add(new MemoryData() { Address = "0x0088", Name = "ResEstIMax",           Value = "", Description = "Maximum current used for Resistance identification" });
            m_MemData.Add(new MemoryData() { Address = "0x008C", Name = "IndEstIMax",           Value = "", Description = "Maximum current used for Inductance identification" });
            m_MemData.Add(new MemoryData() { Address = "0x0090", Name = "FluxEstFreq",          Value = "", Description = "Frequency estimation identification in Hz" });
            m_MemData.Add(new MemoryData() { Address = "0x0094", Name = "RoverLEstFreq",        Value = "", Description = "Frequency estimation identification in Hz" });
            m_MemData.Add(new MemoryData() { Address = "0x0098", Name = "CRC",                  Value = "", Description = "CRC16-Modbus value for Calibration Params" });


        } // private void Mem_FillList()

        private void iWriteInfo_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] pu8ListData = new byte[154];

            Table_ConvertToArray(pu8ListData);

            UInt16 u16Address = 0x0000;
            uint u32MsgId = 0x00000000;
            byte u8ECUId = 0;
            byte[] pu8Data = new byte[8];

            u8ECUId = CCalibration.GetECUSel();

            if (u8ECUId == Globals.ECU_ISMC_A)
            {
                // ECU A seleccionada para atender a los comandos de calibración
                u32MsgId = (byte)CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CALIB_RQ_A;
            }
            else
            {
                u32MsgId = (byte)CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CALIB_RQ_B;
            }

            UpdateWindow gwUpdateWindow = new UpdateWindow(UpdateWindow.PROCESS_ID_RECORD_DATA);
            gwUpdateWindow.ShowInTaskbar = true;
            gwUpdateWindow.Show();

            // Activación del flag de programación en progreso


            Console.WriteLine("**************************************************************");
            Console.WriteLine(" List Read ");
            Console.WriteLine("**************************************************************");

            TPCANStatus stsResult;

            for (byte u8Idx = 0; u8Idx < 154; u8Idx++)
            {

                

                byte[] pu8Address = BitConverter.GetBytes(u16Address);

                Console.WriteLine("Address: " + u16Address.ToString() + "\t" + "Data: " + pu8ListData[u8Idx].ToString());

                pu8Data[0] = CCalibration.CALIB_CMD_WRITE_DATA;
                pu8Data[1] = CCalibration.CALIB_CMD_OK;
                pu8Data[2] = 0x00;
                pu8Data[3] = CCalibration.CALIB_MSG_EOF;
                pu8Data[4] = pu8Address[1]; // Motor In Use
                pu8Data[5] = pu8Address[0];
                pu8Data[6] = 0x00;
                pu8Data[7] = pu8ListData[u8Idx];



                stsResult = WriteFrame(u32MsgId, 8, pu8Data);
                
                System.Threading.Thread.Sleep(10);



                u16Address++;
            }

            // Envío de la orden de grabación en SRAM

            pu8Data[0] = CCalibration.CCALIB_CMD_PUSH_EEPROM;
            pu8Data[1] = CCalibration.CALIB_CMD_OK;
            pu8Data[2] = 0x00;
            pu8Data[3] = CCalibration.CALIB_MSG_EOF;
            pu8Data[4] = 0x00;
            pu8Data[5] = 0x00;
            pu8Data[6] = 0x00;
            pu8Data[7] = 0x00;

            stsResult = WriteFrame(u32MsgId, 8, pu8Data);

            gwUpdateWindow.Close();
        }

        /*******************************************************************************
         * Function Name : Table_ConvertToArray
         * Parameters    : pu8BytesTotal | Array de bytes con el contenido de la lista
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *   Captura el contenido de la lista de parámetros y los transfiere a un array
         *   de bytes.
         *******************************************************************************/
        private void Table_ConvertToArray(byte[] pu8BytesTotal)
        {
            string strData = "";
            byte[] pu8BytesData = new byte[4] { 0, 0, 0, 0 };
            byte[] pu8BytesData2 = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] pu8BytesData3 = new byte[2] { 0, 0 };
            double dValue = 0.0;
            UInt32 u32Value = 0;

            {
                // PolePairs 
                strData = m_MemData[0].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 0);
                // Rs  
                strData = m_MemData[1].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 4);
                // Ls  
                strData = m_MemData[2].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 8);
                // RatedFlux 
                strData = m_MemData[3].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 12);
                // Inertia 
                strData = m_MemData[4].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 16);
                // Friction    
                strData = m_MemData[5].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 20);
                // Rated Current  
                strData = m_MemData[6].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 24);
                // Max. Current  
                strData = m_MemData[7].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 28);
                // Max. Speed  
                strData = m_MemData[8].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 32);
                // Sensor  
                strData = m_MemData[9].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 36);
                // Sensor Resolution
                strData = m_MemData[10].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 40);
                // Motor Name   
                strData = m_MemData[11].Value.ToString();

                int iNumberChars = strData.Length;

                pu8BytesData2 = Encoding.ASCII.GetBytes(strData);
                Array.Resize(ref pu8BytesData2, 16);

                for (int i = strData.Length; i < 16; i++)
                {
                    //pu8Bytes12[i / 2] = Convert.ToByte(strHex.Substring(i, 2), 16);
                    pu8BytesData2[i] = 0;
                    //Console.WriteLine(pu8Bytes12[i / 2].ToString("X2"));
                }
                Array.Reverse(pu8BytesData2, 0, pu8BytesData2.Length);
                pu8BytesData2.CopyTo(pu8BytesTotal, 44);
                // CRC            
                strData = m_MemData[12].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData3 = BitConverter.GetBytes(u32Value);
                Array.Resize(ref pu8BytesData3, 2);
                Array.Reverse(pu8BytesData3, 0, pu8BytesData3.Length);
                pu8BytesData3.CopyTo(pu8BytesTotal, 60);
            } // Motor Data

            {
                // Speed PI Kp
                strData = m_MemData[13].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 62);
                // Speed PI Ki    
                strData = m_MemData[14].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 66);
                // Idq PI Kp   
                strData = m_MemData[15].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 70);
                // Idq PI Ki  
                strData = m_MemData[16].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 74);
                // Control Bandwidth
                strData = m_MemData[17].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q16.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 78);
                // Control Mode   
                strData = m_MemData[18].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 82);
                // CRC     
                strData = m_MemData[19].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q16.0
                pu8BytesData3 = BitConverter.GetBytes(u32Value);
                Array.Resize(ref pu8BytesData3, 2);
                Array.Reverse(pu8BytesData3, 0, pu8BytesData3.Length);
                pu8BytesData3.CopyTo(pu8BytesTotal, 86);
            } // Control Data

            {
                // Current Max. 
                strData = m_MemData[20].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 88);
                // Current Avg    
                strData = m_MemData[21].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 92);
                // Time to Current Avg.
                strData = m_MemData[22].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 96);
                // Speed Max.  
                strData = m_MemData[23].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 100);
                // Time to Speed Avg. 
                strData = m_MemData[24].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 104);
                // Temp. Max. 
                strData = m_MemData[25].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 108);
                // Heart Beat Comms   
                strData = m_MemData[26].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 112);
                // Timeout Comms   
                strData = m_MemData[27].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 116);
                // Amax    
                strData = m_MemData[28].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 120);
                // CRC 
                strData = m_MemData[29].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q8.24
                pu8BytesData3 = BitConverter.GetBytes(u32Value);
                Array.Resize(ref pu8BytesData3, 2);
                Array.Reverse(pu8BytesData3, 0, pu8BytesData3.Length);
                pu8BytesData3.CopyTo(pu8BytesTotal, 124);
            } // Limits Data

            {
                // Baudrate    
                strData = m_MemData[30].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 126);
                // CRC 
                strData = m_MemData[31].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q8.24
                pu8BytesData3 = BitConverter.GetBytes(u32Value);
                Array.Resize(ref pu8BytesData3, 2);
                Array.Reverse(pu8BytesData3, 0, pu8BytesData3.Length);
                pu8BytesData3.CopyTo(pu8BytesTotal, 130);
            } // Comms Data

            {
                // Full scale frequency    
                strData = m_MemData[32].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 65536.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 132);
                // Current for Rs identification process   
                strData = m_MemData[33].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 136);
                // Current for Ls identification process   
                strData = m_MemData[34].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 140);
                // Frequency for Flux identification process   
                strData = m_MemData[35].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 16777216.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 144);
                // Frequency for Rs/Ls ratio identification process   
                strData = m_MemData[36].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue * 65536.0)); // Q8.24
                pu8BytesData = BitConverter.GetBytes(u32Value);
                Array.Reverse(pu8BytesData, 0, pu8BytesData.Length);
                pu8BytesData.CopyTo(pu8BytesTotal, 148);
                // CRC 
                strData = m_MemData[37].Value.ToString();
                strData = strData.Replace('.', ',');
                dValue = Convert.ToDouble(strData);
                u32Value = Convert.ToUInt32(Math.Floor(dValue)); // Q32.0
                pu8BytesData3 = BitConverter.GetBytes(u32Value);
                Array.Resize(ref pu8BytesData3, 2);
                Array.Reverse(pu8BytesData3, 0, pu8BytesData3.Length);
                pu8BytesData3.CopyTo(pu8BytesTotal, 152);
            } // Ident Data


        } // private void Table_ConvertToArray(byte[] pu8BytesTotal)


        private void tmrRead_Tick(object sender, EventArgs e)
        {
            // Checks if in the receive-queue are currently messages for read
            // 
            ReadMessages();
        }

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


        #region CAN Message Decofication


        #endregion

        void ReadFromFile(string strFilename)
        {
            string strFile = "\\" + strFilename + ".xml";
            if (!File.Exists(Environment.CurrentDirectory + strFile))
            {
                Console.WriteLine("El fichero XML para motores NO existe");
            } // if (!File.Exists(Environment.CurrentDirectory + "\\ISMC.xml"))
            else
            {
                Console.WriteLine("El fichero XML para motores SÍ existe");

                XmlTextReader xmlISMC = new XmlTextReader(Environment.CurrentDirectory + strFile);



                string sCurrentElement = "";

                while (xmlISMC.Read())
                {

                    switch (xmlISMC.NodeType)
                    {

                        case XmlNodeType.Element: // The node is an element.
                            {
                                //Console.Write("Element:" + xmlISMC.Name);
                                sCurrentElement = xmlISMC.Name;
                                break;
                            } //  case XmlNodeType.Element: // The node is an element.

                        case XmlNodeType.Text: //Display the text in each element.
                            {
                                int iAttrCnt = xmlISMC.AttributeCount;

                                if (sCurrentElement == "Identifier")
                                {
                                    Console.WriteLine(" ******************************************************************* ");
                                    Console.WriteLine("Identifier: " + xmlISMC.Value);
                                    m_sIdentifier = xmlISMC.Value;

                                    ListBoxItem lbItm = new ListBoxItem();

                                    //lbItm.Content = xmlISMC.Value;
                                    //lbMotors.Items.Add(lbItm);
                                } // if (sCurrentElement == "Identifier")
                                else if (sCurrentElement == "PolePairs")
                                {

                                    Console.WriteLine("PolePairs: " + xmlISMC.Value);
                                    m_sPolePairs = xmlISMC.Value;

                                } // PolePairs
                                else if (sCurrentElement == "Rs")
                                {

                                    Console.WriteLine("Rs: " + xmlISMC.Value);
                                    m_sRs = xmlISMC.Value;
                                } // Rs
                                else if (sCurrentElement == "Ls")
                                {

                                    Console.WriteLine("Ld: " + xmlISMC.Value);
                                    m_sLs = xmlISMC.Value;
                                } // Ld
                                else if (sCurrentElement == "Flux")
                                {

                                    Console.WriteLine("Flux: " + xmlISMC.Value);
                                    m_sFlux = xmlISMC.Value;
                                } // Flux
                                else if (sCurrentElement == "Inertia")
                                {

                                    Console.WriteLine("Inertia: " + xmlISMC.Value);
                                    m_sInertia = xmlISMC.Value;
                                } // Inertia
                                else if (sCurrentElement == "Friction")
                                {

                                    Console.WriteLine("Fricition: " + xmlISMC.Value);
                                    m_sFriction = xmlISMC.Value;
                                } // Inertia

                                else if (sCurrentElement == "SensorType")
                                {

                                    Console.WriteLine("SensorType: " + xmlISMC.Value);
                                    m_sSensorType = xmlISMC.Value;
                                } // SensorType
                                else if (sCurrentElement == "NLines")
                                {

                                    Console.WriteLine("NLines: " + xmlISMC.Value);
                                    m_sNLines = xmlISMC.Value;
                                } // NLines

                                // Control
                                else if (sCurrentElement == "SpeedPIKp")
                                {

                                    Console.WriteLine("SpeedPIKp: " + xmlISMC.Value);
                                    m_sSpeedPIKp = xmlISMC.Value;
                                } // SpeedPIKp
                                else if (sCurrentElement == "SpeedPIKi")
                                {

                                    Console.WriteLine("SpeedPIKi: " + xmlISMC.Value);
                                    m_sSpeedPIKi = xmlISMC.Value;
                                } // SpeedPIKi
                                else if (sCurrentElement == "CurrentPIKp")
                                {

                                    Console.WriteLine("CurrentPIKp: " + xmlISMC.Value);
                                    m_sCurrentPIKp = xmlISMC.Value;
                                } // CurrentPIKp
                                else if (sCurrentElement == "CurrentPIKi")
                                {

                                    Console.WriteLine("CurrentPIKi: " + xmlISMC.Value);
                                    m_sCurrentPIKi = xmlISMC.Value;
                                } // CurrentPIKi
                                else if (sCurrentElement == "Bandwidth")
                                {

                                    Console.WriteLine("Bandwidth: " + xmlISMC.Value);
                                    m_sBandwidth = xmlISMC.Value;
                                } // Bandwidth

                                else if (sCurrentElement == "CtrlMode")
                                {

                                    Console.WriteLine("CtrlMode: " + xmlISMC.Value);
                                    m_sCtrlMode = xmlISMC.Value;
                                } // CtrlMode

                                else if (sCurrentElement == "Imax")
                                {
                                    Console.WriteLine("Imax: " + xmlISMC.Value);
                                    m_sImax = xmlISMC.Value;
                                } // Imax

                                else if (sCurrentElement == "Iavg")
                                {
                                    Console.WriteLine("Iavg: " + xmlISMC.Value);
                                    m_sIavg = xmlISMC.Value;
                                } // Iavg

                                else if (sCurrentElement == "TIavg")
                                {
                                    Console.WriteLine("TIavg: " + xmlISMC.Value);
                                    m_sTIavg = xmlISMC.Value;
                                } // Iavg

                                else if (sCurrentElement == "Smax")
                                {

                                    Console.WriteLine("Smax: " + xmlISMC.Value);
                                    m_sSmax = xmlISMC.Value;
                                } // Smax

                                else if (sCurrentElement == "TSavg")
                                {
                                    Console.WriteLine("TSavg: " + xmlISMC.Value);
                                    m_sTSavg = xmlISMC.Value;
                                } // TSmax

                                else if (sCurrentElement == "TempMax")
                                {
                                    Console.WriteLine("TempMax: " + xmlISMC.Value);
                                    m_sTempMax = xmlISMC.Value;
                                } // TSmax

                                else if (sCurrentElement == "THB")
                                {
                                    Console.WriteLine("THB: " + xmlISMC.Value);
                                    m_sTHB = xmlISMC.Value;
                                } // THB

                                else if (sCurrentElement == "TComms")
                                {
                                    Console.WriteLine("TComms: " + xmlISMC.Value);
                                    m_sTComms = xmlISMC.Value;
                                } // TComms

                                else if (sCurrentElement == "Amax")
                                {
                                    Console.WriteLine("Amax: " + xmlISMC.Value);
                                    m_sAmax = xmlISMC.Value;
                                } // Amax

                                else if (sCurrentElement == "Baudrate")
                                {
                                    Console.WriteLine("Baudrate: " + xmlISMC.Value);
                                    m_sBaudrate = xmlISMC.Value;
                                } // Amax



                                else if (sCurrentElement == "FullScaleFreq")
                                {

                                    Console.WriteLine("FullScaleFreq: " + xmlISMC.Value);
                                    m_sFullScaleFreq = xmlISMC.Value;
                                } // FullScaleFreq
                                else if (sCurrentElement == "ResEstIMax")
                                {

                                    Console.WriteLine("ResEstIMax: " + xmlISMC.Value);
                                    m_sResEstIMax = xmlISMC.Value;
                                } // ResEstIMax
                                else if (sCurrentElement == "IndEstIMax")
                                {

                                    Console.WriteLine("IndEstIMax: " + xmlISMC.Value);
                                    m_sIndEstIMax = xmlISMC.Value;
                                } // IndEstIMax
                                else if (sCurrentElement == "FluxEstFreq")
                                {

                                    Console.WriteLine("FluxEstFreq: " + xmlISMC.Value);
                                    m_sFluxEstFreq = xmlISMC.Value;
                                } // FluxEstFreq
                                else if (sCurrentElement == "RoverLFreq")
                                {

                                    Console.WriteLine("RoverLFreq: " + xmlISMC.Value);
                                    m_sRoverLFreq = xmlISMC.Value;
                                } // RoverLFreq

                                break;
                            } // case XmlNodeType.Text: //Display the text in each element.

                        case XmlNodeType.EndElement: //Display the end of the element.
                            {
                                //Console.Write("EndElement: " + xmlISMC.Name);
                                break;
                            } // case XmlNodeType.EndElement: //Display the end of the element.

                    } // switch (xmlISMC.NodeType)
                } //  while (xmlISMC.Read())

                xmlISMC.Close();


            } // ! if (!File.Exists(Environment.CurrentDirectory + "\\ISMC.xml")))
        

        }

        private void iReset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8ECUId = CCalibration.GetECUSel();

            TPCANStatus stsResult;

            byte[] pu8Data = { 0, 0, 0, 0, 0, 0, 0, 0 };

            UInt32 u32MsgId;

            if (u8ECUId == Globals.ECU_ISMC_A)
            {
                // ECU A seleccionada para atender a los comandos de calibración
                u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_DIAG_RQ_A);
            }
            else
            {
                u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_DIAG_RQ_B);
            }


            pu8Data[0] = (byte)CDiagnosis.TDIAGNOSIS_CMD.DIAG_CMD_ISMC_REINIT;
            pu8Data[1] = CCalibration.CALIB_CMD_OK;
            pu8Data[2] = 0x00;
            pu8Data[3] = CCalibration.CALIB_MSG_EOF;
            pu8Data[4] = 0x00;
            pu8Data[5] = 0x00;
            pu8Data[6] = 0x00;
            pu8Data[7] = 0x00;


            stsResult = WriteFrame(u32MsgId, 8, pu8Data);




        }

    }

    class MemoryData
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public String Description { get; set; }
        /*
        public MemoryData(string  Address, string Name, string Value, string Description)
        {
            this.Address = Address;
            this.Name = Name;
            this.Value = Value;
            this.Description = Description;
        }
        */
    }

    public static class StringExtensions
    {
        public static string ToHex(this string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
                sb.AppendFormat("0x{0:X2} ", (int)c);
            return sb.ToString().Trim();
        }

    }
}
