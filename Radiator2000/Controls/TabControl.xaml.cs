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



        public TabControl()
        {
            _controlsList = new Dictionary<string, UserControl>();
            InitializeComponent();

            //инициализируем типы радиаторов в комбобоксе
            var radiatorTypes = new List<ComboboxItem>()
            {
                new ComboboxItem(Constants.RadiatorTypes.Igolchatiy, ""),
                new ComboboxItem(Constants.RadiatorTypes.IgolchatoStyrevoy, ""),
                new ComboboxItem(Constants.RadiatorTypes.Zhaluziynyi, ""),
                new ComboboxItem(Constants.RadiatorTypes.Igolchatiynew, ""),
            };
            radiatorTypeComboBox.ItemsSource = radiatorTypes;
            //выключаем комбобоксы
            EnableCalculationMethodCheckbox(false);
            EnableRadiatorTypeCheckbox(false);


            #region Проверка версий солида, инициализация
            Process[] processes = Process.GetProcessesByName("SLDWORKS");
            foreach (Process process in processes)
            {
                process.CloseMainWindow();
                process.Kill();
            }
            CheckSolidVersion("2011", new Guid("B4875E89-91F6-4124-BB63-2539727E98F0"));
            CheckSolidVersion("2012", new Guid("B4875E89-91F6-4124-BB63-2539727E98FA"));
            CheckSolidVersion("2013", new Guid("0D825E02-9000-4D82-B4AB-D6BDC2872797"));
            CheckSolidVersion("2014", new Guid("CF33D714-2C34-4608-8766-2536E6C41536"));

            if (solidWersion.Items.Count == 0)
            {
                MessageBox.Show("На компьютере не установлен SolidWorks, работа программы невозможна. \n Программа будет закрыта.");
                MainWindow win = (MainWindow)Window.GetWindow(this);
                win.Close();
            }

            OpenPage(Constants.Pages.SelectSolidVersionControl);
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
            // отсыл на страницу Radiator2000.Controls.Tabs.IgolchatiyTab
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

        private void solidWersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Блаблабла хуета
            //инициализируем чёто там нах, а потом открываем другую страничку.
            OpenPage(Constants.Pages.SelectRadiatorType);
            EnableRadiatorTypeCheckbox(true);
        }
    }
}
