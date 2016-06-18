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

namespace Radiator2000
{
    /// <summary>
    /// Логика взаимодействия для TipovieWindows.xaml
    /// </summary>
    public partial class TipovieWindows : Window
    {
        public TipovieWindows()
        {
            InitializeComponent();
        }
       
            
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            result.Content = powerTextBox.Text;
        }

        private void AB0094_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
