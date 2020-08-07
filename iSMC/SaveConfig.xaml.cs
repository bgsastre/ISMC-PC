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
    /// Lógica de interacción para SaveConfig.xaml
    /// </summary>
    public partial class SaveConfig : Window
    {
        public SaveConfig()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void pbAccept_Click(object sender, RoutedEventArgs e)
        {
            string sText = tConfigName.Text;

            if (sText.Length > 0)
            {
                Globals.SetConfigName(sText);
            }
            this.Close();

        }

        private void pbCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
