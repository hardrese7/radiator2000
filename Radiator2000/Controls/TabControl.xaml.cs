using Radiator2000.Logic;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Dictionary<string, object> _processDictionary = new Dictionary<string, object>();
        //Dictionary<String, Guid> guids = new Dictionary<string, Guid>();
        //
        /// <summary>
        /// проверка на то, установлена версия солида или нет, если установлена, то добавляем в список
        /// </summary>
        /// <param name="version">версия</param>
        /// <param name="guid">ид данной версии солида</param>
        private void CheckSolidVersion(string version, Guid guid)
        {
            try
            {
                var type = Type.GetTypeFromCLSID(guid, true);
                var proc = Activator.CreateInstance(type);

                if (proc != null)
                {
                    _processDictionary.Add(version, proc);
                    solidWersion.Items.Add(version);
                }
            }
            catch
            {

            }
        }


        /// <summary>
        /// инициализация окна
        /// </summary>
        public TabControl()
        {
            _controlsList = new Dictionary<string, UserControl>();
            InitializeComponent();

            //инициализируем типы радиаторов в комбобоксе
            var radiatorTypes = new List<ComboboxItem>()
            {
                new ComboboxItem(Constants.RadiatorTypes.Rebrisiy, ""),
                new ComboboxItem(Constants.RadiatorTypes.IgolchatoStyrevoy, ""),
                new ComboboxItem(Constants.RadiatorTypes.Zhaluziynyi, ""),
                new ComboboxItem(Constants.RadiatorTypes.Igolchatiy, ""),
            };
            radiatorTypeComboBox.ItemsSource = radiatorTypes;
            //выключаем комбобоксы
            EnableCalculationMethodCheckbox(false);
            EnableRadiatorTypeCheckbox(false);


            #region Проверка версий солида, инициализация
            CheckSolidVersion("2011", new Guid("B4875E89-91F6-4124-BB63-2539727E98F0"));
            CheckSolidVersion("2012", new Guid("B4875E89-91F6-4124-BB63-2539727E98FA"));
            CheckSolidVersion("2013", new Guid("0D825E02-9000-4D82-B4AB-D6BDC2872797"));
            CheckSolidVersion("2014", new Guid("CF33D714-2C34-4608-8766-2536E6C41536"));

            if (solidWersion.Items.Count == 0)                         //если версий солида не установлено
            {
                MessageBox.Show(Constants.Messages.SolidNotInstalled); //показываем сообщение
            }
            solidWersion.Items.Add(Constants.Offline);                  //добавляем автономный режим
            OpenPage(Constants.Pages.SelectSolidVersionControl);        //открываем страницу для выбора солида
            #endregion
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


        /// <summary>
        /// Срабатывает при изменении методики расчёта.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculationMethod_OnChange(object sender, RoutedEventArgs e)
        {

            var comboBox = sender as ComboBox;
            ComboboxItem selectedItem = (ComboboxItem)comboBox.SelectedItem;
            if (selectedItem == null) return;
            var tag = Convert.ToString(selectedItem.Value);
            OpenPage(tag);                                                       //открываем нужную страницу

        }

        /// <summary>
        /// Открывает нужную страницу
        /// </summary>
        /// <param name="tag"></param>
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
            // отсыл на страницу Radiator2000.Controls.Tabs.*
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


        /// <summary>
                                                                                                         /// Срабатывает при выборе типа радиатора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadiatorType_OnChange(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            ComboboxItem selectedItem = (ComboboxItem)comboBox.SelectedItem;
            if (selectedItem == null)
                return;
            calculationMethodComboBox.ItemsSource = Helpers.GetItemsForRadiatorType(selectedItem.Text);
            OpenPage(Constants.Pages.SelectCalculationMethod);
            EnableCalculationMethodCheckbox(true);
        }

        /// <summary>
        /// срабатывает при выборе версии солида. Обнуляет все комбобоксы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solidWersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenPage(Constants.Pages.SelectRadiatorType);
            EnableRadiatorTypeCheckbox(true);
            radiatorTypeComboBox.SelectedIndex = -1;
            calculationMethodComboBox.SelectedIndex = -1;
        }
    }
}
