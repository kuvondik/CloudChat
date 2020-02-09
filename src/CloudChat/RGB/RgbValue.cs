using System;
using System.Collections.Generic;

namespace CloudChat
{
    public class RgbValue
    {
        private static readonly string[] colors = new string []
        {
            "ea3e19",
            "1fc5c0",
            "28a424",
            "041dbd",
        };
        public string GrayColor { get; } = "bdbdbd";
        public string GetRandomRgbValue()
        {
            var random = new Random();
            var randomRgb = (string)colors.GetValue(random.Next(colors.Length));
            return randomRgb;
        }


    }
}
