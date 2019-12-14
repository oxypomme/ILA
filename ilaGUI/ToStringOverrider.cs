using System;
using System.Collections.Generic;
using System.Text;

namespace ilaGUI
{
    internal class ToStringOverrider
    {
        public ToStringOverrider(object content, Func<string> toStringFct)
        {
            Content = content;
            ToStringFct = toStringFct;
        }

        public object Content { get; set; }
        public Func<string> ToStringFct { get; set; }

        public override string ToString()
        {
            return ToStringFct?.Invoke();
        }
    }
}