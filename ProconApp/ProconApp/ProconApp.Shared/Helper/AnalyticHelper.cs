using ProconApp.ViewModels;
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
            var sendString = Enum.GetName(typeof(ViewParam), param);
            GoogleAnalytics.EasyTracker.GetTracker().SendView(sendString);
        }
    }
}
