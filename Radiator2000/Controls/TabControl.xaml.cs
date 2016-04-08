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

namespace Radiator2000.Controls
{
    /// <summary>
    /// Interaction logic for TabControl.xaml
    /// </summary>
    public partial class TabControl : UserControl
    {
        public TabControl()
        {
            _controlsList = new Dictionary<string, UserControl>();
            InitializeComponent();

            //инициализируем типы радиаторов в комбобоксе
            var radiatorTypes = new List<ComboboxItem>()
            {
                new ComboboxItem(Constants.RadiatorTypes.Igolchatiy, ""),
                new ComboboxItem(Constants.RadiatorTypes.IgolchatoStyrevoy, ""),
                new ComboboxItem(Constants.RadiatorTypes.Zhaluziynyi, "")
            };
            radiatorTypeComboBox.ItemsSource = radiatorTypes;
            //выключаем комбобоксы
            EnableCalculationMethodCheckbox(false);
            EnableRadiatorTypeCheckbox(false);
            OpenPage(Constants.Pages.SelectSolidVersionControl);
        }

        /// <summary>
        /// Переключает состояние комбобокса с типом радиаторов
        /// </summary>
        /// <param name="enable"></param>
        public void EnableRadiatorTypeCheckbox(bool enable)
        {
            radiatorTypeComboBox.IsEnabled = enable;
        }

        /// <summary>
        /// Переключает состояние комбобокса с методиками расчёта
        /// </summary>
        /// <param name="enable"></param>
        public void EnableCalculationMethodCheckbox(bool enable)
        {
            calculationMethodComboBox.IsEnabled = enable;
        }
        private Dictionary<string, UserControl> _controlsList;

        private void CalculationMethod_OnChange(object sender, RoutedEventArgs e)
        {
            
            var comboBox = sender as ComboBox;
            ComboboxItem selectedItem = (ComboboxItem)comboBox.SelectedItem;
            if (selectedItem == null) return;
            var tag = Convert.ToString(selectedItem.Value);
            OpenPage(tag);

        }

        public void OpenPage(string tag)
        {
            UserControl tab;
            if (_controlsList.ContainsKey(tag))
            {
                tab = _controlsList[tag];
                tab.Width = double.NaN;
                tab.Height = double.NaN;
                Panel.Children.Clear();
                Panel.Children.Add(tab);
                return;
            }
            try
            {
                tab = (UserControl)Activator.CreateInstance(Type.GetType("Radiator2000.Controls.Tabs." + tag));
                _controlsList.Add(tag, tab);
            }
            catch
            {
                MessageBox.Show("Этот функционал находится в разработке");
                return;
            }
            tab.Width = double.NaN;
            tab.Height = double.NaN;
            Panel.Children.Clear();
            Panel.Children.Add(tab);
        }

        private void RadiatorType_OnChange(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            ComboboxItem selectedItem = (ComboboxItem)comboBox.SelectedItem;

            calculationMethodComboBox.ItemsSource = Helpers.GetItemsForRadiatorType(selectedItem.Text);
            OpenPage(Constants.Pages.SelectCalculationMethod);
            EnableCalculationMethodCheckbox(true);

            //calculationMethodComboBox.Items.Add(new KeyValuePair<string, string>("IgolchatiyTab", "Бородин С.М."));
            //calculationMethodComboBox.Items.Add(new KeyValuePair<string, string>("IgolchatiyTab", "Скрипников Ю.Ф."));
            //calculationMethodComboBox.Items.Add(new KeyValuePair<string, string>("IgolchatiyTab", "Роткоп Л.Л."));
            //calculationMethodComboBox.Items.Add(new KeyValuePair<string, string>("IgolchatiyTab", "ОСТ 4.012.001"));

            //            Бородин С.М.
            //Скрипников Ю.Ф.
            //Роткоп Л.Л.
            //ОСТ 4.012.001
        }
    }
}
