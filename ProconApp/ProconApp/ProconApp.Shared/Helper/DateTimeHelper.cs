using System;
using System.Collections.Generic;
using System.Text;

namespace ProconApp
{
    public static class DateTimeHelper
    {

        /// <summary>
        /// 与えられた2つの時刻オブジェクトから間隔に合わせた文字列を返す
        /// n日前 / n時間前 / n分前
        /// </summary>
        /// <param name="unixTime">以前の時刻</param>
        /// <param name="nowTime">現在時刻</param>
        /// <returns></returns>
        public static string DiffTimeString(int unixTime, DateTime nowTime)
        {
            return DiffTimeString(FromUnixTime(unixTime), nowTime);
        }
        /// <summary>
        /// 与えられた2つの時刻オブジェクトから間隔に合わせた文字列を返す
        /// n日前 / n時間前 / n分前
        /// </summary>
        /// <param name="targetTime">以前の時刻</param>
        /// <param name="nowTime">現在時刻</param>
        /// <returns></returns>
        public static string DiffTimeString(DateTime targetTime, DateTime nowTime)
        {
            var diff = nowTime - targetTime; 
            string result = "";

            if (diff.Days > 0)
                result = diff.Days.ToString() + "日前";
            else if (diff.Hours > 0)
                result = diff.Hours.ToString() + "時間前";
            else if (diff.Minutes > 0)
                result = diff.Minutes.ToString() + "分前";
            else
                result = "数秒前";

            return result;
        }

        // unix epochをDateTimeで表した定数
        public readonly static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
       
        /// <summary>
        /// UNIX時間からDateTimeに変換するメソッド
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime)
        {
            // unix epochからunixTime秒だけ経過した時刻を求める
            return UnixEpoch.AddSeconds(unixTime);
        }

        public static DateTime FromTweetTime(string txt)
        {
            var format = "ddd MMM dd HH:mm:ss zzz yyyy";
            return  DateTime.ParseExact(txt, format, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
        }

    }
}
