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
        /// <summary>
        /// при инициализации окна
        /// </summary>
        public TipovieWindows()
        {
            InitializeComponent();
            CalculateAllLengths();
        }
       
        /// <summary>
        /// При клике на кнопку срабатывает
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateAllLengths();
        }


        private void AB0094_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        /// <summary>
        /// считает все длины и выводит ошибку если она есть
        /// </summary>
        public void CalculateAllLengths()
        {
            try
            {
                ErrorLabel.Visibility = Visibility.Hidden;
                CalculateAB0094Length();
                CalculateAB0290Length();
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }

        }
        /// <summary>
        /// Обработчик ошибок
        /// </summary>
        /// <param name="ex"></param>
        private void ErrorHandler(Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// расчёт для AB0094
        /// </summary>
        public void CalculateAB0094Length()
        {
            AB0094.Text = (Convert.ToDouble(powerTextBox.Text) * 0.3).ToString();
        }

        /// <summary>
        /// расчёт для AB0290
        /// </summary>
        public void CalculateAB0290Length()
        {
            AB0290.Text = (Convert.ToDouble(powerTextBox.Text) * 0.42).ToString();
        }
    }
}
