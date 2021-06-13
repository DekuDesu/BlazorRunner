using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.Helpers.Formatting
{
    public static class Html
    {
        public const string NumberStyle = "color: #a92ea7; font-weight: bold;";

        public const string MethodNameStyle = "color: black; font-weight: bold;";

        public static string AsCode(this object code)
        {
            return code.Surround("code");
        }

        public static string AsNumber(this object number)
        {
            return number.AsStyle(NumberStyle);
        }

        public static string AsDiv(this object obj)
        {
            return obj.Surround("div");
        }

        public static string AsParagraph(this object obj)
        {
            return obj.Surround('p');
        }

        public static string AsMethodName(this object number)
        {
            return number.AsStyle(MethodNameStyle);
        }

        public static string AsBold(this object str)
        {
            return str.Surround("b");
        }

        public static string AsStyle(this object str, object style)
        {
            return $"<span style=\"{style};\">{str}</span>";
        }

        public static string AsColor(this object str, object color)
        {
            return str.AsStyle($"color={color}");
        }

        public static string Surround(this object obj, object tag)
        {
            return $"<{tag}>{obj}</{tag}>";
        }
    }
}
