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
    /// Lógica de interacción para ErrorMessage.xaml
    /// </summary>
    public partial class ErrorMessage : Window
    {
        public ErrorMessage(string strMsg)
        {
            InitializeComponent();

            lAppMsg.Content = strMsg;           
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }


        private void pbContinue_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void pbExit_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
