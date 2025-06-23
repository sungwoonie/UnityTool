using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

namespace StarCloudgamesLibrary
{
    public static class TextChange
    {
        static readonly string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

        /// <summary>
        /// double Çü µ¥ÀÌÅÍ¸¦ Å¬¸®Ä¿ °ÔÀÓÀÇ È­Æó ´ÜÀ§·Î Ç¥Çö
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToCurrencyString(this double number)
        {
            string zero = "0";

            if (number < 1.0f)
            {
                return string.Format("{0}{1}{2}", "", number.ToString("F2"), "");
            }

            if (-1d < number && number < 1d)
            {
                return zero;
            }

            if (double.IsInfinity(number))
            {
                return "Infinity";
            }

            //  ºÎÈ£ Ãâ·Â ¹®ÀÚ¿­
            string significant = (number < 0) ? "-" : string.Empty;

            //  º¸¿©ÁÙ ¼ýÀÚ
            string formattedNumber = string.Empty;

            //  ´ÜÀ§ ¹®ÀÚ¿­
            string unityString = string.Empty;

            //  ÆÐÅÏÀ» ´Ü¼øÈ­ ½ÃÅ°±â À§ÇØ ¹«Á¶°Ç Áö¼ö Ç¥Çö½ÄÀ¸·Î º¯°æÇÑ ÈÄ Ã³¸®
            string[] partsSplit = number.ToString("E").Split('+');

            //  ¿¹¿Ü
            if (partsSplit.Length < 2)
            {
                return zero;
            }

            //  Áö¼ö (ÀÚ¸´¼ö Ç¥Çö)
            if (!int.TryParse(partsSplit[1], out int exponent))
            {
                Debug.LogWarningFormat("Failed to parse exponent: {0}", partsSplit[1]);
                return zero;
            }

            //  ¸òÀº ¹®ÀÚ¿­ ÀÎµ¦½º
            int quotient = exponent / 3;

            //  ³ª¸ÓÁö´Â Á¤¼öºÎ ÀÚ¸´¼ö °è»ê¿¡ »ç¿ë(10ÀÇ °ÅµìÁ¦°öÀ» »ç¿ë)
            int remainder = exponent % 3;

            //  10ÀÇ °ÅµìÁ¦°öÀ» ±¸ÇØ¼­ ÀÚ¸´¼ö Ç¥Çö°ªÀ» ¸¸µé¾î ÁØ´Ù.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  ¼Ò¼ö µÑÂ°ÀÚ¸®±îÁö¸¸ Ãâ·ÂÇÑ´Ù.
            formattedNumber = temp.ToString("F2");

            unityString = CurrencyUnits[quotient];

            return string.Format("{0}{1}{2}", significant, formattedNumber, unityString);
        }

        /// <summary>
        /// Å¬¸®Ä¿ °ÔÀÓÀÇ È­Æó ´ÜÀ§ ¹®ÀÚ¿­À» double Çü ¼ýÀÚ·Î º¯È¯
        /// </summary>
        /// <param name="currencyString"></param>
        /// <returns></returns>
        public static double FromCurrencyString(this string currencyString)
        {
            if (string.IsNullOrEmpty(currencyString))
            {
                return 0.0;
            }

            string numberPart = string.Empty;
            string unitPart = string.Empty;

            // ¼ýÀÚ ºÎºÐ°ú ´ÜÀ§ ºÎºÐÀ» ºÐ¸®
            for (int i = 0; i < currencyString.Length; i++)
            {
                if (char.IsDigit(currencyString[i]) || currencyString[i] == '.' || currencyString[i] == '-')
                {
                    numberPart += currencyString[i];
                }
                else
                {
                    unitPart = currencyString.Substring(i);
                    break;
                }
            }

            // ¼ýÀÚ ÆÄ½Ì
            if (!double.TryParse(numberPart, out double number))
            {
                Debug.LogWarningFormat("Failed to parse number part: {0}", numberPart);
                return 0.0;
            }

            // ´ÜÀ§ ÆÄ½Ì
            int unitIndex = Array.IndexOf(CurrencyUnits, unitPart);
            if (unitIndex == -1)
            {
                Debug.LogWarningFormat("Failed to find unit part: {0}", unitPart);
                return 0.0;
            }

            // ½ÇÁ¦ ¼ýÀÚ °è»ê
            double result = number * Math.Pow(10, unitIndex * 3);

            return result;
        }

        public static string RemoveBlankAndSymbol(this string text)
        {
            var symbolRemoved = Regex.Replace(text, @"[^a-zA-Z0-9°¡-ÆR]", string.Empty, RegexOptions.Singleline);
            var blankRemoved = string.Concat(symbolRemoved.Where(x => !Char.IsWhiteSpace(x)));
            return blankRemoved;
        }

        public static bool ContainsBlankAndSymbol(this string text)
        {
            return Regex.Match(text, @"[^a-zA-Z0-9°¡-ÆR]").Success;
        }
    }
}