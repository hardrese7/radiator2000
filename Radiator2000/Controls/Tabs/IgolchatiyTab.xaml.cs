using Radiator2000.Logic;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
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
    /// Interaction logic for IgolchatiyTab.xaml
    /// </summary>
    public partial class IgolchatiyTab : UserControl
    {
        public IgolchatiyTab()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            var borodin = new BorodinCalculation();

            var ts = Convert.ToDouble(TS.Text);      //конвектируем введенные данные в дабл
            var rpk = Convert.ToDouble(Rpk.Text);
            var rkr = Convert.ToDouble(Rkr.Text);
            var p = Convert.ToDouble(P.Text);
            var tmax = Convert.ToDouble(Tmax.Text);
            try
            {
                borodin.Calculate(ts, rpk, rkr, p, tmax);
                SwBuild(borodin);
            }
            catch (Exception ex)
            {
                ErrorLabel.Content = ex.Message.FirstOrDefault();
                ErrorLabel.Visibility = Visibility.Visible;
            }
        }

        public void SwBuild(BorodinCalculation borodin)
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
            swModel.SketchManager.CreateCornerRectangle(0, 0, 0, borodin.H, borodin.D, 0);
            //отчистка выделения
            swModel.ClearSelection2(true);
            //выбираем грани
            swModel.Extension.SelectByID2("Line2", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            swModel.Extension.SelectByID2("Line1", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            swModel.Extension.SelectByID2("Line4", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            swModel.Extension.SelectByID2("Line3", "SKETCHSEGMENT", 0, 0, 0, true, 0, null, 0);
            //операция выдавливания
            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, borodin.delt, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, true, true, true, 0, 0, false);
            swModel.SelectionManager.EnableContourSelection = true;
            //boolstatus = swModel.Extension.SelectByID2("", "", 0.035317911637093857, 0.0099999999999909051, 0.0043238272226631125, false, 0, null, 0);

            swModel.SketchManager.InsertSketch(true);
            swModel.ClearSelection2(true);
            // запускаем цикл построения пластин радиатора

            double tempX = borodin.H - borodin.q;
            swModel.SketchManager.CreateCornerRectangle(borodin.H, borodin.D, 0, tempX, 0, 0);
            swModel.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, borodin.h + borodin.delt, 0.01, false, false, false, false, 1.74532925199433E-02, 1.74532925199433E-02, false, false, false, false, true, true, true, 0, 0, false);
            swModel.Extension.SelectByID2("Бобышка-Вытянуть2", "BODYFEATURE", 0.051638235435518709, 0.046942041222166608, 0.023999999999944066, false, 4, null, 0);
            swModel.Extension.SelectByID2("", "EDGE", 0.017168894188387185, borodin.H, borodin.delt, true, 1, null, 0);
            swModel.FeatureManager.FeatureLinearPattern2(borodin.Count, borodin.b + borodin.q, 1, borodin.q, false, false, "NULL", "NULL", false);
        }


    }
}
