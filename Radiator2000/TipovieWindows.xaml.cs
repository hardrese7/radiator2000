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
      
        private void AB0094_Copy_TextChanged1(object sender, TextChangedEventArgs e)
        {

        }
        private void AB0094_Copy_TextChanged2(object sender, TextChangedEventArgs e)
        {

        }
        private void AB0094_Copy_TextChanged3(object sender, TextChangedEventArgs e)
        {

        }
        private void AB0094_Copy_TextChanged4(object sender, TextChangedEventArgs e)
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
                Calculate566029Length();
                Calculate566028Length();
                Calculate461580Length();
                Calculate461495Length();
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
            AB0094.Text = (Convert.ToDouble(powerTextBox.Text) * 3.5).ToString();
        }

        /// <summary>
        /// расчёт для AB0290
        /// </summary>
        public void CalculateAB0290Length()
        {
            AB0290.Text = (Convert.ToDouble(powerTextBox.Text) * 5).ToString();
        }
        /// <summary>
        /// расчёт для AB0290
        /// </summary>
        public void Calculate566029Length()
        {
            n_566029.Text = (Convert.ToDouble(powerTextBox.Text) * 4.2).ToString();
        }
        /// <summary>
        /// расчёт для AB0290
        /// </summary>
        public void Calculate566028Length()
        {
            n_5660228.Text = (Convert.ToDouble(powerTextBox.Text) * 5.4).ToString();
        }
        /// <summary>
        /// расчёт для AB0290
        /// </summary>
        public void Calculate461580Length()
        {
            n_461580.Text = (Convert.ToDouble(powerTextBox.Text) * 7).ToString();
        }
        /// <summary>
        /// расчёт для AB0290
        /// </summary>
        public void Calculate461495Length()
        {
            n_461495.Text = (Convert.ToDouble(powerTextBox.Text) * 7.5).ToString();
        }
    }
}
