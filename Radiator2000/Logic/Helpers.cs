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

            if (type == Constants.RadiatorTypes.Rebrisiy)
            {
                answer.Add(new ComboboxItem("Бородин С.М.", "RebristiyTab"));
                answer.Add(new ComboboxItem("Тест", "Test"));
                answer.Add(new ComboboxItem("Скрипников", "RebristiyTab"));
                answer.Add(new ComboboxItem("Чернышев", "RebristiyChern"));
            }
            else if (type == Constants.RadiatorTypes.IgolchatoStyrevoy)
            {
                answer.Add(new ComboboxItem("2Бородин С.М.", "RebristiyTab"));
                answer.Add(new ComboboxItem("2Тест", "RebristiyTab"));
                answer.Add(new ComboboxItem("2Тест2", "RebristiyTab"));
            }
            else if (type == Constants.RadiatorTypes.Zhaluziynyi)
            {
                answer.Add(new ComboboxItem("3Бородин С.М.", "RebristiyTab"));
                answer.Add(new ComboboxItem("3Тест", "RebristiyTab"));
                answer.Add(new ComboboxItem("3Тест3", "RebristiyTab"));
            }
            else if (type == Constants.RadiatorTypes.Igolchatiy)
            {
                answer.Add(new ComboboxItem("4Бородин С.М.", "RebristiyTab"));
                answer.Add(new ComboboxItem("4Тест 4", "RebristiyTab"));
                answer.Add(new ComboboxItem("4Тест", "RebristiyTab"));
            }
            return answer;
        }
    }
}
