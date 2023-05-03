﻿namespace Avionera.Extensions
{
    public static class StringExtensions
    {
        public static string Chop(this string text, int chopLength, string postfix = "...")
        {
            if (text == null || text.Length < chopLength)
                return text;
            else
                return text.Substring(0, chopLength - postfix.Length) + postfix;
        }
    }
}
