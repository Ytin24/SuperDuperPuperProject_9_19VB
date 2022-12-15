using CefSharp;
using CefSharp.Wpf;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SuperDuperPuperProject_9_19VB {
    /// <summary>
    /// Логика взаимодействия для GetingCokies.xaml
    /// </summary>

    public partial class GetingCokies : Window {
        private ChromiumWebBrowser browser;
        public delegate void GetingCokiesCompletedEventHandler();
        public event GetingCokiesCompletedEventHandler CookieOk;
        public GetingCokies() {
            InitializeComponent();
            this.Topmost = true;
            this.Height = 600;
            this.Width = 600;
            browser = new CefSharp.Wpf.ChromiumWebBrowser("https://kad.arbitr.ru/") {

                Height = 500,
                Width = 500,
            };
            RootG.Children.Add(browser);
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            RootG.Children.Add(new TextBlock() {
                Text = "Не закрывайте это окно, оно закроется автоматически",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = Brushes.Red,
                FontSize = 20,
            });
        }
        public CookieContainer Cookies = new();
        private async void Browser_LoadingStateChanged(object? sender, CefSharp.LoadingStateChangedEventArgs e) {
            if (!e.IsLoading && Dispatcher.Invoke(() => browser.Title == "Картотека арбитражных дел.")) {
                browser.LoadingStateChanged -= Browser_LoadingStateChanged;
                browser.ExecuteScriptAsync("document.querySelector('button[type=submit]').click()");
                await Task.Delay(3000);
                var cookies = await browser.GetCookieManager().VisitAllCookiesAsync();
                foreach (var c in cookies) {
                    System.Net.Cookie cookie = new() {
                        Name = c.Name,
                        Domain = c.Domain,
                        Path = c.Path,
                        Value = c.Value,
                        HttpOnly = c.HttpOnly,
                    };
                    Cookies.Add(cookie);
                }

                Dispatcher.Invoke(() => RootG.Children.Remove(browser));
                CookieOk.Invoke();
            }
        }
    }
}
