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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TPCANHandle = System.UInt16;
using TPCANBitrateFD = System.String;
using TPCANTimestampFD = System.UInt64;
using Peak.Can.Basic;

namespace iSMC
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Delegates
        /// <summary>
        /// Read-Delegate Handler
        /// </summary>
        private delegate void ReadDelegateHandler();
        #endregion

        public System.Windows.Threading.DispatcherTimer tmrRead = new System.Windows.Threading.DispatcherTimer();



        /// <summary>
        /// Read Delegate for calling the function "ReadMessages"
        /// </summary>
        private ReadDelegateHandler m_ReadDelegate;

        /// <summary>
        /// Stores the status of received messages for its display
        /// </summary>
        private System.Collections.ArrayList m_LastMsgsList;


        private UInt32 m_u32Cnt1 = 0;
        private UInt32 m_u32Cnt2 = 0;
        private UInt32 m_u32Cnt3 = 0;
        private UInt32 m_u32Cnt4 = 0;
        private UInt32 m_u32Cnt5 = 0;
        private UInt32 m_u32Cnt6 = 0;

        BitmapImage m_LogoISMCOK = new BitmapImage();
        BitmapImage m_LogoISMCNOK = new BitmapImage();


        private string m_sMotorName = "";

        private Int32 m_s32NumCalibParams = 0;


        public MainWindow()
        {
            InitializeComponent();


            Globals.SetCurrentDir(Environment.CurrentDirectory);

            

            CCommunications.m_PcanHandle = 81;

            // Creates the delegate used for message reading
            //
            m_ReadDelegate = new ReadDelegateHandler(ReadMessages);

            // Creates the list for received messages
            //
            m_LastMsgsList = new System.Collections.ArrayList();



            m_LogoISMCOK.BeginInit();
            m_LogoISMCOK.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUOK.png");
            m_LogoISMCOK.EndInit();

            m_LogoISMCNOK.BeginInit();
            m_LogoISMCNOK.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUError.png");
            m_LogoISMCNOK.EndInit();


            TPCANStatus stsResult;
            uint iChannelsCount;
            bool bIsFD;

            // Checks for available Plug&Play channels
            //
            // Checks for available Plug&Play channels
            //
            stsResult = PCANBasic.GetValue(PCANBasic.PCAN_NONEBUS, TPCANParameter.PCAN_ATTACHED_CHANNELS_COUNT, out iChannelsCount, sizeof(uint));
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                TPCANChannelInformation[] info = new TPCANChannelInformation[iChannelsCount];

                stsResult = PCANBasic.GetValue(PCANBasic.PCAN_NONEBUS, TPCANParameter.PCAN_ATTACHED_CHANNELS, info);
                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                {
                    // Include only connectable channels
                    //

                    string sHardware = "";

                    foreach (TPCANChannelInformation channel in info)
                    {
                        if ((channel.channel_condition & PCANBasic.PCAN_CHANNEL_AVAILABLE) == PCANBasic.PCAN_CHANNEL_AVAILABLE)
                        {
                            bIsFD = (channel.device_features & PCANBasic.FEATURE_FD_CAPABLE) == PCANBasic.FEATURE_FD_CAPABLE;                            
                            sHardware = FormatChannelName(channel.channel_handle, bIsFD);
                            Console.WriteLine(sHardware);
                            lbHardware.Content = FormatChannelName(channel.channel_handle, bIsFD);
                        }
                    }
                    if (sHardware == "PCAN_USB 1 (51h)")
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ConnectOff2.PNG");
                        logo.EndInit();
                        imgConnect.Source = logo;
                        CCommunications.SetConnectionStatus((byte) CCommunications.TCONNECTION.CONNECTION_DISCONNECT);
                    }
                    else
                    {
                        BitmapImage logo = new BitmapImage();
                        logo.BeginInit();
                        logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ConnectOff.PNG");
                        logo.EndInit();
                        imgConnect.Source = logo;
                        CCommunications.SetConnectionStatus((byte)CCommunications.TCONNECTION.CONNECTION_WAIT_HW);

                        ErrorMessage wErrorMessage = new ErrorMessage("No valid CAN device has been detected");
                        this.Hide();
                        this.ShowInTaskbar = false;
                        wErrorMessage.ShowInTaskbar = true;
                        //wAppMsg.Owner = Application.Current.MainWindow;
                        wErrorMessage.Show();
                    }
                }

            } // if (stsResult == TPCANStatus.PCAN_ERROR_OK)


            // Activación del timer de lectura
            tmrRead.Tick += new EventHandler(tmrRead_Tick);
            tmrRead.Interval = new TimeSpan(0, 0, 0, 0, CCommunications.CAN_TIME_RX_PROCESS);
            tmrRead.Stop();
        }

        #region EVENTOS CONTROLES

        /*******************************************************************************
         * Function Name : Window_MouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Función que atiende al evento click sobre la ventana "Configuración". Si el
         *  botón se mantiene presionado, se hace caso a las ordenes de mover pantalla.
         *******************************************************************************/
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        /*******************************************************************************
         * Function Name : iConfig_MouseEnter
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función de atención al evento MouseEnter: Cambia color
         *******************************************************************************/
        private void iConfig_MouseEnter(object sender, MouseEventArgs e)
        {
            lConfig.Foreground = Brushes.LightSteelBlue;
        } // private void iConfig_MouseEnter(object sender, MouseEventArgs e)

        /*******************************************************************************
         * Function Name : iConfig_MouseLeave
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función de atención al evento MouseLeave: Vuelve al color original
         *******************************************************************************/
        private void iConfig_MouseLeave(object sender, MouseEventArgs e)
        {
            lConfig.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
        } // iConfig_MouseLeave

        /*******************************************************************************
         * Function Name : lControl_OnMouseEnter
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función de atención al evento MouseEnter: Cambia color
         *******************************************************************************/
        private void lControl_OnMouseEnter(object sender, MouseEventArgs e)
        {
            lControl.Foreground = Brushes.LightSteelBlue;
        } // lControl_OnMouseEnter

        /*******************************************************************************
         * Function Name : lControl_OnMouseLeave
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función de atención al evento MouseLeave: Vuelve al color original
         *******************************************************************************/
        private void lControl_OnMouseLeave(object sender, MouseEventArgs e)
        {
            lControl.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
        } // lControl_OnMouseLeave

        /*******************************************************************************
         * Function Name : lFlashMem_OnMouseEnter
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función de atención al evento MouseEnter: Cambia color
         *******************************************************************************/
        private void lFlashMem_OnMouseEnter(object sender, MouseEventArgs e)
        {
            lMemory.Foreground = Brushes.LightSteelBlue;
        } // lFlashMem_OnMouseEnter

        /*******************************************************************************
         * Function Name : lFlashMem_OnMouseLeave
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función de atención al evento MouseLeave: Vuelve al color original
         *******************************************************************************/
        private void lFlashMem_OnMouseLeave(object sender, MouseEventArgs e)
        {
            lMemory.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
        } // lFlashMem_OnMouseLeave

        #endregion
        /// <summary>
        /// Gets the formated text for a PCAN-Basic channel handle
        /// </summary>
        /// <param name="handle">PCAN-Basic Handle to format</param>
        /// <param name="isFD">If the channel is FD capable</param>
        /// <returns>The formatted text for a channel</returns>
        private string FormatChannelName(TPCANHandle handle, bool isFD)
        {
            TPCANDevice devDevice;
            byte byChannel;


            // Gets the owner device and channel for a 
            // PCAN-Basic handle
            //
            if (handle < 0x100)
            {
                devDevice = (TPCANDevice)(handle >> 4);
                byChannel = (byte)(handle & 0xF);
            }
            else
            {
                devDevice = (TPCANDevice)(handle >> 8);
                byChannel = (byte)(handle & 0xFF);
            }

            // Constructs the PCAN-Basic Channel name and return it
            //
            if (isFD)
                return string.Format("{0}:FD {1} ({2:X2}h)", devDevice, byChannel, handle);
            else
                return string.Format("{0} {1} ({2:X2}h)", devDevice, byChannel, handle);
        }




        #region Eventos de click

        /*******************************************************************************
         * Function Name : iReturn_OnMouseDown
         * Parameters    : None
         * Returns       : None
         *******************************************************************************
         * Notes         :
         * Función que configura los filtros para la recepción de los mensajes CAN
         * necesarios para implementar la configuración
         *******************************************************************************/
        private void iReturn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            TPCANStatus stsResult;

            TPCANBaudrate eBaudrate = CCommunications.GetBaudRate();
            TPCANType eHwType = CCommunications.GetHWType();

            stsResult = PCANBasic.Initialize(CCommunications.m_PcanHandle, eBaudrate, eHwType, 256, 3);

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                if (stsResult != TPCANStatus.PCAN_ERROR_CAUTION)
                {
                    //MessageBox.Show(GetFormatedError(stsResult));
                }
                else
                {
                    /*
                    IncludeTextMessage("******************************************************");
                    IncludeTextMessage("The bitrate being used is different than the given one");
                    IncludeTextMessage("******************************************************");
                    */
                    stsResult = TPCANStatus.PCAN_ERROR_OK;


                    

                }
            }
            else
            {
                // Prepares the PCAN-Basic's PCAN-Trace file
                //
                //ConfigureTraceFile();
            }
            // Sets the connection status of the main-form
            //
            //SetConnectionStatus(stsResult == TPCANStatus.PCAN_ERROR_OK);
        }

        private void iConfig_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Window pWindow = this;

            PCANBasic.Uninitialize(CCommunications.m_PcanHandle);
            tmrRead.Stop();


            // Sets the connection status of the main-form
            //
            SetConnectionStatus(false);

            Configuration wConfiguration = new Configuration(pWindow);
            wConfiguration.ShowInTaskbar = true;
            //wCalibration.Owner = Calibration.Current.MainWindow;
            wConfiguration.Show();
            this.Hide();
        }

        private void iControl_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Window pWindow = this;

            PCANBasic.Uninitialize(CCommunications.m_PcanHandle);
            tmrRead.Stop();


            // Sets the connection status of the main-form
            //
            SetConnectionStatus(false);

            Control wControl = new Control(pWindow);
            wControl.ShowInTaskbar = true;
            wControl.Owner = Application.Current.MainWindow;
            wControl.Show();
            tmrRead.Stop();
            this.Hide();
        }

        private void Connect_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte u8ConnectionStatus;

            u8ConnectionStatus = CCommunications.GetConnectionStatus();

            if (u8ConnectionStatus == ((byte) CCommunications.TCONNECTION.CONNECTION_DISCONNECT))
            {
                TPCANStatus stsResult;

                CCommunications.SetConnectionStatus((byte)CCommunications.TCONNECTION.CONNECTION_CONNECT);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ConnectOn.PNG");
                logo.EndInit();
                imgConnect.Source = logo;
                CCommunications.SetConnectionStatus((byte)CCommunications.TCONNECTION.CONNECTION_CONNECT);

                // Connects a selected PCAN-Basic channel
                //
                stsResult = PCANBasic.Initialize(
                    CCommunications.m_PcanHandle,
                    CCommunications.m_Baudrate,
                    CCommunications.m_HwType,
                    256,
                    3);

                Enable_Controls1();

                
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    if (stsResult != TPCANStatus.PCAN_ERROR_CAUTION)
                    {
                        ErrorMessage wErrorMessage = new ErrorMessage("No valid CAN device has been detected");
                        this.Hide();
                        this.ShowInTaskbar = false;
                        wErrorMessage.ShowInTaskbar = true;
                        //wAppMsg.Owner = Application.Current.MainWindow;
                        wErrorMessage.Show();
                        //MessageBox.Show(GetFormatedError(stsResult));


                    }
                    else
                    {
                        stsResult = TPCANStatus.PCAN_ERROR_OK;
                    }
                }
                else
                {
                    // Prepares the PCAN-Basic's PCAN-Trace file
                    //
                    ConfigureTraceFile();

                    Console.WriteLine("************************************************************************");
                    Console.WriteLine("Captura de los parámetros del ISMC A");
                    Console.WriteLine("************************************************************************");
                    GetParams(Globals.ECU_ISMC_A);

                    Console.WriteLine("************************************************************************");
                    Console.WriteLine("Captura de los parámetros del ISMC B");
                    Console.WriteLine("************************************************************************");
                    GetParams(Globals.ECU_ISMC_B);
                }

                // Sets the connection status of the main-form
                //
                SetConnectionStatus(stsResult == TPCANStatus.PCAN_ERROR_OK);

            }
            else
            {
                CCommunications.SetConnectionStatus((byte)CCommunications.TCONNECTION.CONNECTION_DISCONNECT);
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ConnectOff2.PNG");
                logo.EndInit();
                imgConnect.Source = logo;
                CCommunications.SetConnectionStatus((byte)CCommunications.TCONNECTION.CONNECTION_DISCONNECT);

                tmrRead.Stop();

                Disable_Controls1();

                // Releases a current connected PCAN-Basic channel
                //
                PCANBasic.Uninitialize(CCommunications.m_PcanHandle);
                tmrRead.Stop();


                // Sets the connection status of the main-form
                //
                SetConnectionStatus(false);
            }
        }

        #endregion

        #region HELP FUNCTIONS

        private void Enable_Controls1()
        {
            tmrRead.Start();
            // Enable Control
            iConfig.IsEnabled = true;            
            iFlashMem.IsEnabled = true;
            lConfig.IsEnabled = true;
            lMemory.IsEnabled = true;
            BitmapImage logo1 = new BitmapImage();
            logo1.BeginInit();
            logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/IconoConfig2.PNG");
            logo1.EndInit();
            iConfig.Source = logo1;


            BitmapImage logo3 = new BitmapImage();
            logo3.BeginInit();
            logo3.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/WriteFlash.PNG");
            logo3.EndInit();
            iFlashMem.Source = logo3;

            BitmapImage logo4 = new BitmapImage();
            logo4.BeginInit();
            logo4.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUInfo.PNG");
            logo4.EndInit();
            iISMCAQStatus.Source = logo4;
            iISMCBQStatus.Source = logo4;

            lConfig.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));            
            lMemory.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
            lFWaa.Foreground   = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
            lFWa.Foreground    = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
            lFWbb.Foreground   = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
            lFWb.Foreground    = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
        } // private void Enable_Controls1()


        private void Enable_Controls2()
        {

            iCtrl.IsEnabled = true;
            lControl.IsEnabled = true;

            BitmapImage logo2 = new BitmapImage();
            logo2.BeginInit();
            logo2.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/PMSM.PNG");
            logo2.EndInit();
            iCtrl.Source = logo2;
            lControl.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(19, 98, 164));
        } // private void Enable_Controls1()

        private void Disable_Controls1()
        {
            iConfig.IsEnabled = false;
            iCtrl.IsEnabled = false;
            iFlashMem.IsEnabled = false;
            lConfig.IsEnabled = false;
            lControl.IsEnabled = false;
            lMemory.IsEnabled = false;
            BitmapImage logo1 = new BitmapImage();
            logo1.BeginInit();
            logo1.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/IconoConfig2d.PNG");
            logo1.EndInit();
            iConfig.Source = logo1;

            BitmapImage logo2 = new BitmapImage();
            logo2.BeginInit();
            logo2.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/PMSMd.PNG");
            logo2.EndInit();
            iCtrl.Source = logo2;

            BitmapImage logo3 = new BitmapImage();
            logo3.BeginInit();
            logo3.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/WriteFlashd.PNG");
            logo3.EndInit();
            iFlashMem.Source = logo3;

            BitmapImage logo4 = new BitmapImage();
            logo4.BeginInit();
            logo4.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ECUInfod.PNG");
            logo4.EndInit();
            iISMCAQStatus.Source = logo4;
            iISMCBQStatus.Source = logo4;

            lConfig.Foreground = Brushes.DarkGray;
            lControl.Foreground = Brushes.DarkGray;
            lMemory.Foreground = Brushes.DarkGray;
            lFWaa.Foreground = Brushes.DarkGray;
            lFWa.Foreground = Brushes.DarkGray;
            lFWbb.Foreground = Brushes.DarkGray;
            lFWb.Foreground = Brushes.DarkGray;

            m_u32Cnt1 = 0;
            m_u32Cnt2 = 0;
            m_u32Cnt3 = 0;
            m_u32Cnt4 = 0;
            m_u32Cnt5 = 0;
            m_u32Cnt6 = 0;
        } // private void Disable_Controls1()

        /// <summary>
        /// Configures the PCAN-Trace file for a PCAN-Basic Channel
        /// </summary>
        private void ConfigureTraceFile()
        {
            UInt32 iBuffer;
            TPCANStatus stsResult;

            // Configure the maximum size of a trace file to 5 megabytes
            //
            iBuffer = 5;
            stsResult = PCANBasic.SetValue(CCommunications.m_PcanHandle, TPCANParameter.PCAN_TRACE_SIZE, ref iBuffer, sizeof(UInt32));
            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                IncludeTextMessage(GetFormatedError(stsResult));

            // Configure the way how trace files are created: 
            // * Standard name is used
            // * Existing file is ovewritten, 
            // * Only one file is created.
            // * Recording stopts when the file size reaches 5 megabytes.
            //
            iBuffer = PCANBasic.TRACE_FILE_SINGLE | PCANBasic.TRACE_FILE_OVERWRITE;
            stsResult = PCANBasic.SetValue(CCommunications.m_PcanHandle, TPCANParameter.PCAN_TRACE_CONFIGURE, ref iBuffer, sizeof(UInt32));
            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                IncludeTextMessage(GetFormatedError(stsResult));
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


        /// <summary>
        /// Includes a new line of text into the information Listview
        /// </summary>
        /// <param name="strMsg">Text to be included</param>
        private void IncludeTextMessage(string strMsg)
        {

        }

        /// <summary>
        /// Activates/deaactivates the different controls of the main-form according
        /// with the current connection status
        /// </summary>
        /// <param name="bConnected">Current status. True if connected, false otherwise</param>
        private void SetConnectionStatus(bool bConnected)
        {
            //tmrDisplay.Enabled = bConnected;
        }

        #endregion

        #region Timer event-handler
        private void tmrRead_Tick(object sender, EventArgs e)
        {
            // Checks if in the receive-queue are currently messages for read
            // 
            ReadMessages();
        }

        private void tmrDisplay_Tick(object sender, EventArgs e)
        {
            DisplayMessages();
        }

        #endregion

        #region Message Processing Functions


        /// <summary>
        /// Display CAN messages in the Message-ListView
        /// </summary>
        private void DisplayMessages()
        {
            
        }

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
        }

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
                // We process the received message
                //
                ProcessMessage(CANMsg, CANTimeStamp);

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

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CALIB_RS_A))
                {
                    
                    byte u8CalibMsgSt = CAN_OnCalibMsg(CANMsg.DATA, Globals.ECU_ISMC_A);
                    if (u8CalibMsgSt == CCalibration.CALIB_CMD_OK)
                    {
                        m_u32Cnt5++;
                        if (m_u32Cnt5 == m_s32NumCalibParams)
                        {
                            iISMCAQStatus.Source = m_LogoISMCOK;
                            Enable_Controls2();
                        }
                    }

                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CALIB_RS_B))
                {
                    byte u8CalibMsgSt = CAN_OnCalibMsg(CANMsg.DATA, Globals.ECU_ISMC_B);
                    if (u8CalibMsgSt == CCalibration.CALIB_CMD_OK)
                    {
                        m_u32Cnt6++;
                        if (m_u32Cnt6 == m_s32NumCalibParams)
                        {
                            iISMCBQStatus.Source = m_LogoISMCOK;
                            Enable_Controls2();
                        }
                    }
                }


                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CTRL_STATUS_A))
                {
                    m_u32Cnt1++;
                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_BOARD_STATUS_A))
                {
                    m_u32Cnt2++;
                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CTRL_STATUS_B))
                {
                    m_u32Cnt3++;
                }

                if (CANMsg.ID == Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_BOARD_STATUS_B))
                {
                    m_u32Cnt4++;
                }
                // We process the received message
                //
                //ProcessMessage(CANMsg, CANTimeStamp);
            }
            return stsResult;
        }

        /// <summary>
        /// Processes a received message, in order to show it in the Message-ListView
        /// </summary>
        /// <param name="theMsg">The received PCAN-Basic message</param>
        /// <returns>True if the message must be created, false if it must be modified</returns>
        private void ProcessMessage(TPCANMsgFD theMsg, TPCANTimestampFD itsTimeStamp)
        {
            // We search if a message (Same ID and Type) is 
            // already received or if this is a new message
            //
            
        }

        private TPCANStatus WriteFrame(UInt32 u32MsgId, byte u8Length, byte [] pu8Data)
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


        private void GetParams(byte u8ECUId)
        {
            TPCANStatus stsResult;
            byte[] pu8Data = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            UInt32 u32MsgId = 0;

            pu8Data[0] = CCalibration.CALIB_CMD_GET_PARAM;
            pu8Data[1] = CCalibration.CALIB_CMD_OK;
            pu8Data[3] = CCalibration.CALIB_MSG_EOF;
            pu8Data[2] = (byte) CCalibration.CALIB_PARAMS.CALIB_PARAM_FW_VERSION;

            // Send the message
            //


            byte[] pu8ParamsToRead =
            {
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_FW_VERSION),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_POLE_PAIRS),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RS),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LD),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LQ),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_FLUX),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_INERTIA),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_FRICTION),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_CURRENT),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_CURRENT),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_SENSOR),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_SENSOR_RESOLUTION),
                // CONTROL
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_SPEED_KP),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_SPEED_KI),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KP),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KI),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_BANDWIDTH),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_1_4),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_2_4),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_3_4),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_4_4),
                // LIMITS
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_CONFIG_RES_IMAX),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS),
                Convert.ToByte(CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL)
            };

            m_s32NumCalibParams = pu8ParamsToRead.Length;

            pu8Data[0] = CCalibration.CALIB_CMD_GET_PARAM;
            pu8Data[1] = CCalibration.CALIB_CMD_OK;
            pu8Data[3] = CCalibration.CALIB_MSG_EOF;

            if (Globals.ECU_ISMC_A == u8ECUId)
            {
                u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CALIB_RQ_A);                
            }
            else
            {
                u32MsgId = Convert.ToUInt32(CCommunications.TCAN_MESSAGES.CAN_MSG_ID_CALIB_RQ_B);
            }

            for (byte u8ParamCnt = 0; u8ParamCnt < pu8ParamsToRead.Length; u8ParamCnt++)
            {
                byte u8Param = pu8ParamsToRead[u8ParamCnt];
                pu8Data[2] = u8Param;
                stsResult = WriteFrame(u32MsgId, 8, pu8Data);
                System.Threading.Thread.Sleep(10);
                // Recepción del mensaje de respuesta al comando de calibración                
            }
        }

        #endregion

        #region Message Decofication functions
        /*******************************************************************************
         * Function Name : CAN_OnCalibMsg
         * Parameters    : sMessageData | string | CAN Message data (8 Bytes)
         * Returns       : None
         *******************************************************************************
         * Notes         :
         *  Subrutina de atención a mensaje de calibración
         *******************************************************************************/
        private byte CAN_OnCalibMsg(byte[] pu8Bytes, byte u8ECUId)
        {
            byte u8Return = CCalibration.CALIB_CMD_ERROR;

            if (pu8Bytes[1] == CCalibration.CALIB_CMD_OK)
            {
                // Se ha recibido un comando de respuesta correcto
                switch (pu8Bytes[0])
                {
                    case CCalibration.CALIB_CMD_GET_PARAM:
                        {
                            if (u8ECUId == Globals.ECU_ISMC_A)
                            {
                                Console.WriteLine("iSMC A  ==> Comando: " + pu8Bytes[0].ToString() + " Parámetro: " + pu8Bytes[2].ToString());
                            }
                            else
                            {
                                Console.WriteLine("iSMC B  ==> Comando: " + pu8Bytes[0].ToString() + " Parámetro: " + pu8Bytes[2].ToString());
                            }
                            byte u8ParamId = pu8Bytes[2];
                            switch (u8ParamId)
                            {

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_GET_ERROR):
                                    {
                                        CErrors.Error_Set(u8ECUId, pu8Bytes[4]);
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_GET_ERROR):


                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_FW_VERSION):
                                    {
                                        string sVersion    = pu8Bytes[4].ToString();
                                        string sSubversion = pu8Bytes[5].ToString();
                                        string sVariant    = pu8Bytes[6].ToString();




                                        if (u8ECUId == Globals.ECU_ISMC_B)
                                        {
                                            lFWb.Content = sVersion + "." + sSubversion + "." + sVariant;
                                            //tFWVersionB.Text = sVersion;
                                            //tFWSubversionB.Text = sSubversion;
                                            //tFWVariantB.Text = sVariant;
                                        }
                                        else
                                        {
                                            lFWa.Content = sVersion + "." + sSubversion + "." + sVariant;
                                            //tFWVersionA.Text = sVersion;
                                            //tFWSubversionA.Text = sSubversion;
                                            //tFWVariantA.Text = sVariant;
                                        }

                                        Console.WriteLine("Versión de FW: " + sVersion.ToString() + "." + sSubversion.ToString() + "." + sVariant.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_FW_VERSION):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_POLE_PAIRS):
                                    {

                                        byte u8Value = 0;
                                        Int32 i32Value = 0;
                                        i32Value  = (Convert.ToInt32(pu8Bytes[4]));

                                        u8Value = Convert.ToByte(i32Value);


                                        CCalibration.SetPolePairs(u8ECUId, u8Value);
                                        Console.WriteLine("Pares de polos : " + u8Value.ToString());


                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_POLE_PAIRS):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RS):
                                    {

                                        double dValue = 0.0;
                                        Int32 i32Value = 0;

                                        i32Value =  (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetResistance(u8ECUId, dValue);

                                        Console.WriteLine("Rs: " + dValue.ToString());



                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RS):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LD):
                                    {

                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        Console.WriteLine("Inductancia: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LD):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LQ):
                                    {
                                        string sValue = "";
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetInductance(u8ECUId, dValue);
                                        Console.WriteLine("Inductancia: " + dValue.ToString());
                                        sValue = dValue.ToString();
                                        sValue = string.Format("{0:0.###}", sValue);

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LQ):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_FLUX):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;

                                        i32Value  = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetRatedFlux(u8ECUId, dValue);
                                        Console.WriteLine("Flujo: " + dValue.ToString());
                                        dValue = CCalibration.GetRatedFlux(u8ECUId);

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_FLUX):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_INERTIA):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetInertia(u8ECUId, dValue);
                                        Console.WriteLine("Inercia: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_INERTIA):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_FRICTION):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetFriction(u8ECUId, dValue);
                                        Console.WriteLine("Friccion: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_INERTIA):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_CURRENT):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetRatedCurrent(u8ECUId, dValue);
                                        Console.WriteLine("Corriente Nominal: " + dValue.ToString());

                                        dValue = CCalibration.GetMaxCurrent(u8ECUId);

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_CURRENT):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_CURRENT):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetMaxCurrent(u8ECUId, dValue);
                                        Console.WriteLine("Máxima Corriente: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_CURRENT):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_SENSOR):
                                    {
                                        byte u8Value = 0;
                                        Int32 i32Value = 0;
                                        i32Value  = Convert.ToInt32(pu8Bytes[4]);
                                        u8Value = Convert.ToByte(i32Value);


                                        CCalibration.SetSensor(u8ECUId, u8Value);
                                        Console.WriteLine("Sensor : " + u8Value.ToString());


                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_SENSOR):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_SENSOR_RESOLUTION):
                                    {
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 8);
                                        i32Value |= Convert.ToInt32(pu8Bytes[5]) ;
                                        CCalibration.SetResolution(u8ECUId, Convert.ToUInt16(i32Value));
                                        Console.WriteLine("Resolucion : " + i32Value.ToString());


                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_SENSOR):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_1_4):
                                    {

                                        char[] pcValue = new char[4];

                                        Encoding ascii = Encoding.ASCII;

                                        pcValue[0] = Convert.ToChar(pu8Bytes[4]);
                                        pcValue[1] = Convert.ToChar(pu8Bytes[5]);
                                        pcValue[2] = Convert.ToChar(pu8Bytes[6]);
                                        pcValue[3] = Convert.ToChar(pu8Bytes[7]);


                                        m_sMotorName = "";
                                        m_sMotorName = new string(pcValue);




                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_2_4):
                                    {

                                        char[] pcValue = new char[4];

                                        Encoding ascii = Encoding.ASCII;

                                        pcValue[0] = Convert.ToChar(pu8Bytes[4]);
                                        pcValue[1] = Convert.ToChar(pu8Bytes[5]);
                                        pcValue[2] = Convert.ToChar(pu8Bytes[6]);
                                        pcValue[3] = Convert.ToChar(pu8Bytes[7]);


                                        string sMotorName = "";
                                        sMotorName = new string(pcValue);
                                        m_sMotorName += sMotorName;


                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_3_4):
                                    {

                                        char[] pcValue = new char[4];

                                        Encoding ascii = Encoding.ASCII;

                                        pcValue[0] = Convert.ToChar(pu8Bytes[4]);
                                        pcValue[1] = Convert.ToChar(pu8Bytes[5]);
                                        pcValue[2] = Convert.ToChar(pu8Bytes[6]);
                                        pcValue[3] = Convert.ToChar(pu8Bytes[7]);


                                        string sMotorName = "";
                                        sMotorName = new string(pcValue);
                                        m_sMotorName += sMotorName;




                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_NAME_4_4):
                                    {

                                        char[] pcValue = new char[4];

                                        Encoding ascii = Encoding.ASCII;

                                        pcValue[0] = Convert.ToChar(pu8Bytes[4]);
                                        pcValue[1] = Convert.ToChar(pu8Bytes[5]);
                                        pcValue[2] = Convert.ToChar(pu8Bytes[6]);
                                        pcValue[3] = Convert.ToChar(pu8Bytes[7]);


                                        string sMotorName = "";
                                        sMotorName = new string(pcValue);
                                        m_sMotorName += sMotorName;

                                        CCalibration.SetMotorName(u8ECUId, m_sMotorName);

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):


                                // CONTROL PARAMS
                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_SPEED_KP):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetSpdCtrlKp(u8ECUId, dValue);
                                        Console.WriteLine("Constante de Control Kp para el control de velocidad: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_SPEED_KP):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_SPEED_KI):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetSpdCtrlKi(u8ECUId, dValue);
                                        Console.WriteLine("Constante de Control Ki para el control de velocidad: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_SPEED_KI):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KP):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetIdqCtrlKp(u8ECUId, dValue);
                                        Console.WriteLine("Constante de Control Kp para el control de corriente: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KI):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KI):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetIdqCtrlKi(u8ECUId, dValue);
                                        Console.WriteLine("Constante de Control Ki para el control de corriente: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KI):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_BANDWIDTH):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 4096;
                                        dValue = Math.Round(dValue, 9);

                                        CCalibration.SetCtrlBandwidth(u8ECUId, dValue);
                                        Console.WriteLine("Ancho de banda del control activo de perturbaciones: " + dValue.ToString());
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CTRL_IDQ_KI):

                                // LIMITS PARAMS
                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT;

                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, dValue);
                                        Console.WriteLine("Corriente  Máxima: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_CURRENT):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT):
                                    {

                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, dValue);
                                        Console.WriteLine("Corriente  Media: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_AVG_CURRENT):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT):
                                    {

                                        Int32 i32Value = 0;
                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        CCalibration.SetLimitParam(u8ECUId, u8Param, i32Value);
                                        Console.WriteLine("Tiempo para el cálculo de la corriente  media (ms): " + i32Value.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_AVG_CURRENT):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_SPEED_KRPM;
                                        i32Value  = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, dValue);

                                        Console.WriteLine("Velocidad Máxima: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_SPEED_KRPM):


                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED):
                                    {
                                        Int32 i32Value = 0;
                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED;

                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, i32Value);
                                        Console.WriteLine("Tiempo para calcular la velocidad media (ms): " + i32Value.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIME_SPEED):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;

                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, dValue);

                                        Console.WriteLine("Temperatura Máxima: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_CONFIG_RES_IMAX):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;
    
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        //dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetIdentResistance(u8ECUId, dValue);

                                        Console.WriteLine("Resistencia para posicionamiento del encoder: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TEMP_MAX):

                                    
                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS):
                                    {


                                        Int32 i32Value = 0;

                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS;

                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, i32Value);
                                        Console.WriteLine("Tiempo para el envio del heart beat (ms): " + i32Value.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_HEARTBEAT_COMMS):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS):
                                    {

                                        Int32 i32Value = 0;
                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS;

                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, i32Value);
                                        Console.WriteLine("Timeout de las comunicaciones (ms): " + i32Value.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_TIMEOUTS_COMMS):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL):
                                    {
                                        double dValue = 0.0;
                                        Int32 i32Value = 0;

                                        byte u8Param = (byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_LIMITS_MAX_ACCEL;
                                        i32Value = (Convert.ToInt32(pu8Bytes[4]) << 24);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[5]) << 16);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[6]) << 8);
                                        i32Value |= (Convert.ToInt32(pu8Bytes[7]));
                                        dValue = Convert.ToDouble(i32Value);
                                        dValue = dValue / 16777216;
                                        dValue = Math.Round(dValue, 1);

                                        CCalibration.SetLimitParam(u8ECUId, u8Param, dValue);

                                        Console.WriteLine("Aceleración Máxima: " + dValue.ToString());

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):

                                default:
                                    {

                                        Console.WriteLine("Parámetro desconocido o erróneo");
                                        break;
                                    } // default:

                            } // switch (u8ParamId)

                            break;
                        } // case CCalibration.CALIB_CMD_GET_PARAM:

                    case CCalibration.CALIB_CMD_SET_PARAM:
                        {

                            byte u8ParamId = pu8Bytes[2];
                            switch (u8ParamId)
                            {
                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_FW_VERSION):
                                    {
                                        Console.WriteLine("Comando de escritura de versión de FW ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_FW_VERSION):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_POLE_PAIRS):
                                    {
                                        Console.WriteLine("Comando de escritura de pares de polo ejecutado con éxito");

                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_POLE_PAIRS):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RS):
                                    {
                                        Console.WriteLine("Comando de escritura de resistencia del estator ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RS):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LD):
                                    {
                                        Console.WriteLine("Comando de escritura de inductancia del estator (eje D) ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LD):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LQ):
                                    {
                                        Console.WriteLine("Comando de escritura de inductancia del estator (eje Q) ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_LQ):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_FLUX):
                                    {
                                        Console.WriteLine("Comando de escritura de flujo del motor ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_RATED_FLUX):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_CURRENT):
                                    {
                                        Console.WriteLine("Comando de escritura de maxíma corriente del motor ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_CURRENT):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_SPEED_KRPM):
                                    {
                                        Console.WriteLine("Comando de escritura de velocidad máxima del motor ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_SPEED_KRPM):

                                case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):
                                    {
                                        Console.WriteLine("Comando de aceleración máxima del motor ejecutado con éxito");
                                        break;
                                    } // case ((byte)CCalibration.CALIB_PARAMS.CALIB_PARAM_MOTOR_MAX_ACCEL):

                                default:
                                    {

                                        Console.WriteLine("Parámetro desconocido o erróneo");
                                        break;
                                    } //  default:


                            } // switch (u8ParamId)

                            break;
                        } // case CCalibration.CALIB_CMD_SET_PARAM:
                    default:
                        {
                            Console.WriteLine("Comando desconocido: " + pu8Bytes[0].ToString());
                            break;
                        }
                } // switch (pu8Bytes[0])
                u8Return = CCalibration.CALIB_CMD_OK;
            } // if (pu8Bytes[1] == CCalibration.CALIB_CMD_OK)
            else
            {
                // No se ha reconocido el comando de calibración o el resultado ha sido erróneo
                u8Return = CCalibration.CALIB_CMD_ERROR;
            }


            return u8Return;

        } // private void CAN_OnCtrlStatusMsg(string sMessageData)
        #endregion


    }
}
