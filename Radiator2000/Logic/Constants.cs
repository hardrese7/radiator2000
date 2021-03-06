﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public static class Constants
    {
        public static class RadiatorTypes
        {
            public static string Rebrisiy = "Ребристый";
            public static string IgolchatoStyrevoy = "Игольчато-Штыревой";
            public static string Zhaluziynyi = "Жалюзийный";
            public static string Igolchatiy = "Игольчатый";
        }

        public static class Messages
        {
            public static string SolidNotInstalled = "На компьютере не установлен SolidWorks, работа программы будет производиться в автономном режиме.";
            public static string CantBuildOffline = "Построение радиаторов в автономном режиме не доступно. \nВыберите доступную версию SolidWorks из списка, если версии SolidWorks отсутствуют в списке, их необходимо установить.";
        }

        public static class Pages
        {
            public static string SelectSolidVersionControl = "SelectSolidVersionControl";
            public static string SelectCalculationMethod = "SelectCalculationMethod";
            public static string SelectRadiatorType = "SelectRadiatorType";
        }
        public static string Offline = "Автономный режим";

    }
}
