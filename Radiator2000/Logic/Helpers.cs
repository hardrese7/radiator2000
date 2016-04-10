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
            
            if(type == Constants.RadiatorTypes.Igolchatiy)
            {
                answer.Add(new ComboboxItem("Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("Тест", "Test"));
                answer.Add(new ComboboxItem("Пиздётин С.М.", "IgolchatiyTab"));
            }
            else if (type == Constants.RadiatorTypes.IgolchatoStyrevoy)
            {
                answer.Add(new ComboboxItem("2Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("2Хуёдин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("2Пиздётин С.М.", "IgolchatiyTab"));
            }
            else if (type == Constants.RadiatorTypes.Zhaluziynyi)
            {
                answer.Add(new ComboboxItem("3Бородин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("3Хуёдин С.М.", "IgolchatiyTab"));
                answer.Add(new ComboboxItem("2Пиздётин С.М.", "IgolchatiyTab"));
            }
            return answer;
        }
    }
}
