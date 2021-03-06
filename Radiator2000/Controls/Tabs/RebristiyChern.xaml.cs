﻿using Radiator2000.Logic;
using SolidWorks.Interop.sldworks;
using System;

using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Radiator2000.Controls.Tabs
{
    /// <summary>
    /// Interaction logic for RebristiyChern.xaml
    /// </summary>
    public partial class RebristiyChern : UserControl
    {
        public RebristiyChern()
        {
            InitializeComponent();
            SetChernCoefficients();
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

        private RebristiyChernCalculation Calculate()
        {
            var calculations = new RebristiyChernCalculation();   //конвектируем введенные данные в дабл

            var ts = Convert.ToDouble(TS.Text);      //Температура среды
            var rpk = Convert.ToDouble(Rpk.Text);    //Сопротивление п-н переход-корпус
            var rkr = Convert.ToDouble(Rkr.Text);   //сопротивление корпус-радиатор
            var p = Convert.ToDouble(P.Text);       //мощность
            var tmax = Convert.ToDouble(Tmax.Text); //Максимальная температура
            var e = Convert.ToDouble(E.Text); 
            var chernCoefficients = new RebristiyChernCoefficients()  //конвектируем введенные данные в дабл
            {
                b = Convert.ToDouble(b_Label.Text),
                d = Convert.ToDouble(d_Label.Text),
                k4 = Convert.ToDouble(k4_Label.Text),       //коэф. формы основания
                h = Convert.ToDouble(h_Label.Text),         // высота ребер
                delt = Convert.ToDouble(delt_Label.Text),   //толщина основания
                
            };
            calculations.Calculate(ts, rpk, rkr, p, tmax, e, chernCoefficients);
            return calculations;
        }

        public void SetChernCoefficients()
        {
           
            k4_Label.Text = "1";    //коэф. формы основания
            h_Label.Text = "0,03";  // высота ребер
            b_Label.Text = "0,008";  // межреберное
            delt_Label.Text = "0,004";  //толщина основания
            d_Label.Text = "0,003";  // толщина ребер
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
        public void SwBuild(RebristiyChernCalculation calculations)
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
            // строим прямоугольник
            swModel.SketchManager.CreateCornerRectangle(0, 0, 0, calculations.L, calculations.l, 0);
            //отчистка выделения
            swModel.ClearSelection2(true);
            //выбираем грани
            swModel.Extension.SelectByID2("Line2", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.Extension.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            swModel.Extension.SelectByID2("Line4", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            swModel.Extension.SelectByID2("Line3", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            //операция выдавливания
            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, calculations.ChernCoefficients.delt, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, true, true, true, 0, 0, false);
            swModel.SelectionManager.EnableContourSelection = true;
            //boolstatus = swModel.Extension.SelectByID2("", "", 0.035317911637093857, 0.0099999999999909051, 0.0043238272226631125, false, 0, null, 0);

            swModel.SketchManager.InsertSketch(true);
            swModel.ClearSelection2(true);
            // запускаем цикл построения пластин радиатора

            double tempX = calculations.L - calculations.ChernCoefficients.delt;
            swModel.SketchManager.CreateCornerRectangle(calculations.L, calculations.l, 0, tempX, 0, 0);
            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, calculations.ChernCoefficients.h + calculations.ChernCoefficients.delt, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, true, true, true, 0, 0, false);
            swModel.Extension.SelectByID2("Бобышка-Вытянуть2", "BODYFEATURE", 0.051638235435518709, 0.046942041222166608, 0.023999999999944066, false, 4, null, 0);
            swModel.Extension.SelectByID2("", "EDGE", 0.017168894188387185, calculations.L, calculations.ChernCoefficients.delt, true, 1, null, 0);
            swModel.FeatureManager.FeatureLinearPattern2(calculations.Count, calculations.b5 + calculations.ChernCoefficients.delt, 1, calculations.ChernCoefficients.delt, false, false, "NULL", "NULL", false);
        }
        #endregion
        #region additional methods
        public void InitCalculationsWindow(RebristiyChernCalculation calculations)
        {
            var window = new CalculationsWindow { Owner = (MainWindow)Window.GetWindow(this) };
            window.ShirinaLabel.Content = string.Format("{0:0.0000}", calculations.l);
            window.VisotaLabel.Content = string.Format("{0:0.0000}", calculations.L);
            window.VisotaReberLabel.Content = string.Format("{0:0.000}", calculations.ChernCoefficients.h);
            window.MegreberLabel.Content = string.Format("{0:0.0000}", calculations.b5);
            window.KolishestvoReberLabel.Content = string.Format("{0:0}", calculations.Count);
            window.TolshinaOsnLabel.Content = string.Format("{0:0.000}", calculations.ChernCoefficients.delt);
            window.PlosadOsnLabel.Content = string.Format("{0:0.000000000}", calculations.sp);
            window.TolshinaReberLabel.Content = string.Format("{0:0.0000}", calculations.ChernCoefficients.delt);

            window.ShowDialog();
        }

        //public void InitTipovieWindows(RebristiyChernCalculation calculations)
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
            SetChernCoefficients();
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
