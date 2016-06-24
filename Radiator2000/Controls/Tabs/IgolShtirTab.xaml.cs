using Radiator2000.Logic;
using SolidWorks.Interop.sldworks;
using System;

using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Radiator2000.Controls.Tabs
{
    /// <summary>
    /// Interaction logic for IgolShtirTab.xaml
    /// </summary>
    public partial class IgolShtirTab : UserControl
    {
        public IgolShtirTab()
        {
            InitializeComponent();
            SetISCoefficients();
            MainWindow win = (MainWindow)Window.GetWindow(this);
        }


        #region onclick events
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BuildClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorLabel.Visibility = Visibility.Hidden;
                SwBuild(Calculate());
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private IgolShtirCalculation Calculate()
        {
            var calculations = new IgolShtirCalculation();   //конвектируем введенные данные в дабл

            var ts = Convert.ToDouble(TS.Text);      //Температура среды
            var rpk = Convert.ToDouble(Rpk.Text);    //Сопротивление п-н переход-корпус
            var rkr = Convert.ToDouble(Rkr.Text);   //сопротивление корпус-радиатор
            var p = Convert.ToDouble(P.Text);       //мощность
            var tmax = Convert.ToDouble(Tmax.Text); //Максимальная температура
            var aP = Convert.ToDouble(aP1.Text);
            var igolshtirCoefficients = new IgolShtirCoefficients()  //конвектируем введенные данные в дабл
            {
                d2 = Convert.ToDouble(d2_Label.Text),
                d1 = Convert.ToDouble(d1_Label.Text),
                s = Convert.ToDouble(s_Label.Text),       //коэф. формы основания
                h = Convert.ToDouble(h_Label.Text),         // высота ребер
                delt = Convert.ToDouble(delt_Label.Text),   //толщина основания

            };
            calculations.Calculate(ts, rpk, rkr, p, tmax, aP, igolshtirCoefficients);
            return calculations;
        }

        public void SetISCoefficients()
        {

            s_Label.Text = "0,006";    //коэф. формы основания
            h_Label.Text = "0,03";  // высота ребер
            d2_Label.Text = "0,001";  // межреберное
            delt_Label.Text = "0,004";  //толщина основания
            d1_Label.Text = "0,002";  // толщина ребер
        }

        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                InitCalculationsWindow(Calculate());
            }
            catch (Exception ex)
            {
                ErrorHandler(ex);
            }
        }
        #endregion

        #region SwBuild
        public void SwBuild(IgolShtirCalculation calculations)
        {
            SldWorks SwApp;
            IModelDoc2 swModel;
            //progressBar1.Value += 15;
            //убиваем
            Process[] processes = Process.GetProcessesByName("SLDWORKS.exe");
            //проходим по всем процессам, если такой процесс есть
            foreach (Process process in processes)
            {
                process.CloseMainWindow();//закрываем главное окно
                process.Kill();// убиваем процесс
            }

            //запускаем СолидВоркс
            //Guid myGuid1 = new Guid("B4875E89-91F6-4124-BB63-2539727E98FA");//номер приложения для запуска
            //object selectedProcessSW = System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(myGuid1));// запускаем объект по нашему гуиду

            //var tc = (TabControl)Parent;
            MainWindow win = (MainWindow)Window.GetWindow(this);
            var version = (string)win._tabControl.solidWersion.SelectedItem;
            if (version == Constants.Offline)
            {
                MessageBox.Show(Constants.Messages.CantBuildOffline);
                return;
            }
            SwApp = (SldWorks)win._tabControl._processDictionary[version];             // передаем переменной SwApp полученный в солиде объект
            SwApp.Visible = true;                           //делаем процесс видимым
            // создает 3д документ
            // новая 3д деталь
            SwApp.NewPart();

            //передаем докупент с 3д деталью в IModelDoc2 swModel; для работы с ним
            swModel = SwApp.IActiveDoc2;
            // построение 3д модели
            // выбираем плоскость
            swModel.Extension.SelectByID2("Спереди", "PLANE", 0, 0, 0, false, 0, null, 0);
            //создание на этой плоскости эскиза
            swModel.SketchManager.InsertSketch(true);
            // отчистка выделения
            swModel.ClearSelection2(true);
            // строим окружность
            swModel.SketchManager.CreateCornerRectangle(0, 0, 0, calculations.l, calculations.l, 0);
            //отчистка выделения
            swModel.ClearSelection2(true);
            //выбираем грани
            swModel.Extension.SelectByID2("Line2", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.Extension.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            swModel.Extension.SelectByID2("Line4", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            swModel.Extension.SelectByID2("Line3", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            //операция выдавливания
            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, calculations.ISCoefficients.delt, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, true, true, true, 0, 0, false);
            swModel.SelectionManager.EnableContourSelection = true;

            //boolstatus = swModel.Extension.SelectByID2("", "", 0.035317911637093857, 0.0099999999999909051, 0.0043238272226631125, false, 0, null, 0);

            swModel.SketchManager.CreateCircle(calculations.ISCoefficients.s, calculations.ISCoefficients.s, 0, 0.000551 + calculations.ISCoefficients.s, 0.0007 + calculations.ISCoefficients.s, 0.000000);
            swModel.ClearSelection2(true);
            swModel.Extension.SelectByID2("Arc1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);

            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, calculations.ISCoefficients.h, 0.01, true, false, false, false, 0.017453292519943334, 0.022689280275926337, false, false, false, false, true, true, true, 0, 0, false);

            swModel.ISelectionManager.EnableContourSelection = false;
            swModel.ActivateSelectedFeature();
            //swModel.DeSelectByID("Бобышка-Вытянуть2", "BODYFEATURE", 0.33337157021946495, 0.33312167997432468, 0.018792649567387798);
            swModel.Extension.SelectByID2("Бобышка-Вытянуть2", "BODYFEATURE", 0.0070402339392407016, 0.0072696291275506138, 0.013921012519915621, false, 4, null, 0);
            swModel.Extension.SelectByID2("", "EDGE", 0, calculations.l / 2, calculations.ISCoefficients.delt, true, 1, null, 0);
            swModel.Extension.SelectByID2("", "EDGE", calculations.l / 2, 0, calculations.ISCoefficients.delt, true, 2, null, 0);
            //Int16 realCount = Convert.ToInt16(Math.Sqrt(calculations.Count));
            swModel.FeatureManager.FeatureLinearPattern2(calculations.Count-1, 0.006, calculations.Count-1, 0.006, true, false, "NULL", "NULL", false);
        }
           
        #endregion
            #region additional methods
        public void InitCalculationsWindow(IgolShtirCalculation calculations)
        {
            var window = new CalculationsWindow { Owner = (MainWindow)Window.GetWindow(this) };
            window.ShirinaLabel.Content = string.Format("{0:0.0000}", calculations.l);
            window.VisotaLabel.Content = string.Format("{0:0.0000}", calculations.l);
            window.VisotaReberLabel.Content = string.Format("{0:0.000}", calculations.ISCoefficients.h);
          
            window.KolishestvoReberLabel.Content = string.Format("{0:0}", calculations.Count);
            window.TolshinaOsnLabel.Content = string.Format("{0:0.000}", calculations.ISCoefficients.delt);
            window.PlosadOsnLabel.Content = string.Format("{0:0.000000000}", calculations.sp);
            window.TolshinaReberLabel.Content = string.Format("{0:0.0000}", calculations.ISCoefficients.delt);

            window.ShowDialog();
        }

        //public void InitTipovieWindows(IgolShtirTabCalculation calculations)
        // {
        //var window = new TipovieWindow { Owner = (MainWindow)Window.GetWindow(this) };
        //window.ShirinaLabel.Content = string.Format("{0:0.0000}", calculations.D);
        // window.VisotaLabel.Content = string.Format("{0:0.0000}", calculations.H);
        // window.ShowDialog();
        // }

        private void ErrorHandler(Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.Visibility = Visibility.Visible;
        }
        #endregion

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            SetISCoefficients();
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new TipovieWindows { Owner = (MainWindow)Window.GetWindow(this) };
            window.powerTextBox.Text = P.Text;
            window.okButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent)); // имитируем клик по кнопке
            window.ShowDialog();
        }
    }
}
