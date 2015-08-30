using System;
using System.Collections.Generic;
using System.Text;

namespace ProconApp
{
    /// <summary>
    /// 
    /// </summary>
    public static class HTMLHelper
    {
        private readonly static string headder = @"<!DOCTYPE html><html><body>";
        private readonly static string footer = @"</body></html>";

        /// <summary>
        /// 渡された文字列にHTMLのヘッダとフッタを付与して、HTMLソースとして返す
        /// </summary>
        /// <param name="body">bodyタグの"中身"に相当する文字列</param>
        /// <returns>HTMLソース</returns>
        public static string GetFullHTMLString(string body)
        {
            return headder + body + footer;
        }
    }
}
