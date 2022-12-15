using CefSharp;
using CefSharp.Wpf;
using System;
using System.Windows;

namespace SuperDuperPuperProject_9_19VB {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            string[] UserAgents = {
                "Mozilla/5.0 (Macintosh; PPC Mac OS X 10_8_6 rv:5.0; en-US) AppleWebKit/531.30.2 (KHTML, like Gecko) Version/5.1 Safari/531.30.2",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/536.1 (KHTML, like Gecko) Chrome/36.0.876.0 Safari/536.1",
                "Opera/8.44.(X11; Linux x86_64; tn-ZA) Presto/2.9.189 Version/12.00",
                "Mozilla/5.0 (Windows 98; Win 9x 4.90; yo-NG; rv:1.9.2.20) Gecko/2016-11-05 00:58:15 Firefox/13.0",
                "Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 4.0; Trident/5.1)"
            };
            CefSettings _browserSettings = new CefSettings() {
                UserAgent = UserAgents[new Random().Next(UserAgents.Length - 1)]
            };
            Cef.Initialize(_browserSettings);
        }
    }
}
