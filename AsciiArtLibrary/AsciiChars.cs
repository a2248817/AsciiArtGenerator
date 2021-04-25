using System;
using System.Drawing;

namespace AsciiArtLibrary
{
    public static class AsciiChars
    {
        // [R G B]
        public static char[] Chars = { '⠁', '⠉', '⠋', '⠛', '⠟', '⠿', '⡿', '⣿' };
        public static char GetAsciiChar(Color rgb)
        {
            double sum = 0.2126 * rgb.R + 0.7152 * rgb.G + 0.0722 * rgb.B;
            int intensity = (int)Math.Round(sum / 255 * 7);
            return Chars[intensity];
        }

    }
}
