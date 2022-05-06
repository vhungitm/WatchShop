using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace Common
{
    public class StringFormat
    {
        public static string formatToLink(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");

                str = str.Normalize(NormalizationForm.FormD);
                str = regex.Replace(str, string.Empty).Replace("đ", "d").Replace("Đ", "D").ToLower().Replace(" ", "-");
                str = Regex.Replace(str, @"[^a-z0-9-]+", "");
                str = Regex.Replace(str, @"[\-]+", "-");

                return str;
            }

            return "";
        }
    }
}