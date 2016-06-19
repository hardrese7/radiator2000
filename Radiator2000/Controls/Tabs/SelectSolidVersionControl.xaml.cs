using Radiator2000.Logic;
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

namespace Radiator2000.Controls.Tabs
{
    /// <summary>
    /// Interaction logic for SelectSolidVersionControl.xaml
    /// </summary>
    public partial class SelectSolidVersionControl : UserControl
    {
        public SelectSolidVersionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие, срабатывающее при выборе версии солидворкс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solidVersionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win._tabControl.OpenPage(Constants.Pages.SelectRadiatorType);//открываем страницу с выбором радиаторов
            win._tabControl.EnableRadiatorTypeCheckbox(true);//делаем активным чекбокс с радиаторами
        }
    }
}
