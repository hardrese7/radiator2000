using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public static class Helpers
    {
        public static List<ComboboxItem> GetItemsForRadiatorType(string type)
        {
            var answer = new List<ComboboxItem>();

            if (type == Constants.RadiatorTypes.Igolchatiy)
            {
                answer.Add(new ComboboxItem("Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("Тест", "Test"));
                answer.Add(new ComboboxItem("Скрипников", "IgolchatiyTab"));
            }
            else if (type == Constants.RadiatorTypes.IgolchatoStyrevoy)
            {
                answer.Add(new ComboboxItem("2Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("2Тест", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("2Тест2", "IgolchatiyTab"));
            }
            else if (type == Constants.RadiatorTypes.Zhaluziynyi)
            {
                answer.Add(new ComboboxItem("3Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("3Тест", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("3Тест3", "IgolchatiyTab"));
            }
            else if (type == Constants.RadiatorTypes.Igolchatiynew)
            {
                answer.Add(new ComboboxItem("4Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("4Тест 4", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("4Тест", "IgolchatiyTab"));
            }
            return answer;
        }
    }
}
