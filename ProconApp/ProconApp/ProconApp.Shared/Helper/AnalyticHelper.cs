using System;
using System.Collections.Generic;
using System.Text;

namespace ProconApp
{
    public static class AnalyticHelper
    {

        public enum ViewParam
        {
            Home,
            Social,
        }

        /// <summary>
        /// GoogleAnaliticsに情報を送信する。
        /// </summary>
        /// <param name="param"></param>
        public static void SendGAnalytics(ViewParam param)
        {
#if WINDOWS_PHONE_APP
            var sendString = Enum.GetName(typeof(ViewParam), param);
            GoogleAnalytics.EasyTracker.GetTracker().SendView(sendString);
#endif
        }
    }
}
