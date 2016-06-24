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
                answer.Add(new ComboboxItem("Белоусов О. А.", "RebristiyBel"));
                answer.Add(new ComboboxItem("Бородин С. М.", "RebristiyTab"));
                answer.Add(new ComboboxItem("Скрипников Ю. Ф.", "Vrazrab"));
                answer.Add(new ComboboxItem("Чернышев А. А.", "RebristiyChern"));
            }
            else if (type == Constants.RadiatorTypes.IgolchatoStyrevoy)
            {
                answer.Add(new ComboboxItem("Белоусов О. А.", "Vrazrab"));
                answer.Add(new ComboboxItem("Бородин С. М.", "IgolShtirTab"));
                answer.Add(new ComboboxItem("Скрипников Ю. Ф.", "Vrazrab"));
            }
            else if (type == Constants.RadiatorTypes.Zhaluziynyi)
            {
                answer.Add(new ComboboxItem("Белоусов О. А.", "Vrazrab"));
                answer.Add(new ComboboxItem("Бородин С. М.", "Vrazrab"));
                answer.Add(new ComboboxItem("Скрипников Ю. Ф.", "Vrazrab"));
            }
            else if (type == Constants.RadiatorTypes.Igolchatiy)
            {
                answer.Add(new ComboboxItem("Белоусов О. А.", "Vrazrab"));
                answer.Add(new ComboboxItem("Бородин С. М.", "Vrazrab"));
                answer.Add(new ComboboxItem("Скрипников Ю. Ф.", "Vrazrab"));
            }
            return answer;
        }
    }
}
