using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radiator2000.Logic
{
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public ComboboxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
