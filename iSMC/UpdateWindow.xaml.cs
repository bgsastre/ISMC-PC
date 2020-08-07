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

namespace iSMC
{
    /// <summary>
    /// Lógica de interacción para UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public const byte PROCESS_ID_READ_DATA = 1;
        public const byte PROCESS_ID_RECORD_DATA = 2;

        byte m_u8ProcessType = 0;

        BitmapImage bmpProcess = new BitmapImage();

        public UpdateWindow(byte u8ProcessId)
        {
            m_u8ProcessType = u8ProcessId;

            string sMessage = "";

            switch (m_u8ProcessType)
            {
                case PROCESS_ID_READ_DATA:
                    {
                        bmpProcess.BeginInit();
                        bmpProcess.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/ReadingProgress.png");
                        bmpProcess.EndInit();
                        sMessage = "Reading Data...";
                        break;
                    } // case PROCESS_ID_READ_DATA:
                case PROCESS_ID_RECORD_DATA:
                    {
                        bmpProcess.BeginInit();
                        bmpProcess.UriSource = new Uri("pack://application:,,,/ISMC;component/Images/RecordingProgress.png");
                        bmpProcess.EndInit();
                        sMessage = "Recording Data...";
                        break;
                    } // case PROCESS_ID_READ_DATA:
                default:
                    {
                        sMessage = "";
                        break;
                    }
            } // switch(m_u8ProcessType)

            InitializeComponent();

            tProgress.Content = sMessage;
            iProgress.Source = bmpProcess;
        }
    }
}
