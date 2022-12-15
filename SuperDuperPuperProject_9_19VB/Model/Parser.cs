using HtmlAgilityPack;
using RestSharp;
using SuperCoolProject_919vb.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SuperDuperPuperProject_9_19VB.Model {
    internal class Parser {
        public delegate void AllReady();
        public event AllReady Ok;
        public delegate void GetingTableComplete(DataTable table);
        public event GetingTableComplete TableOk;
        private DataTable Table = new DataTable();
        private Dispatcher dispatcher;
        private CookieContainer Cookies;
        private RestClient client;
        private RestRequest request;
        string[] UserAgents = {
                "Mozilla/5.0 (Macintosh; PPC Mac OS X 10_8_6 rv:5.0; en-US) AppleWebKit/531.30.2 (KHTML, like Gecko) Version/5.1 Safari/531.30.2",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/536.1 (KHTML, like Gecko) Chrome/36.0.876.0 Safari/536.1",
                "Opera/8.44.(X11; Linux x86_64; tn-ZA) Presto/2.9.189 Version/12.00",
                "Mozilla/5.0 (Windows 98; Win 9x 4.90; yo-NG; rv:1.9.2.20) Gecko/2016-11-05 00:58:15 Firefox/13.0",
                "Mozilla/5.0 (compatible; MSIE 7.0; Windows NT 4.0; Trident/5.1)"
            };
        public Parser(CookieContainer cookies) {
            Cookies = cookies;

        }
        public async Task GetDataAsync(KadArbitrClass body, Dispatcher dispatcher) {
            this.dispatcher = dispatcher;
            client = new RestClient(new RestClientOptions("https://kad.arbitr.ru") {
                CookieContainer = Cookies,
            });

            body.Page = 1;
            var cookie = Cookies.GetAllCookies();
            request = new RestRequest("Kad/SearchInstances");
            request.AddJsonBody(body);
            request.AddHeader("origin", "https://kad.arbitr.ru");
            request.AddHeader("referer", "https://kad.arbitr.ru/");
            request.AddHeader("sec-ch-ua", "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Microsoft Edge\";v=\"108\"");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("x-date-format", "iso");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("user-agent", UserAgents[new Random().Next(UserAgents.Length - 1)]);
            request.AddHeader("wasm", cookie.First(x => x.Name == "wasm").Value);
            request.AddHeader("pr_fp", cookie.First(x => x.Name == "pr_fp").Value);
            RestResponse response;
            try {
                response = client.Post(request);
            }
            catch {
                await GetNewCookies();
                try {
                    response = client.Post(request);
                }
                catch (HttpRequestException) {
                    if (Table.Rows.Count != 0) {
                        Save(Table);
                    }
                    response = null;
                    System.Windows.MessageBox.Show("ip забанило, всего хоро-ше-го", "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown(1);

                }
            }
            var table = new HtmlDocument();


            Table.Columns.Add("Дело");
            Table.Columns.Add("Судья | текущая инстанция");
            Table.Columns.Add("Истец");
            Table.Columns.Add("Ответчик");
            table.LoadHtml(response.Content);
            var pages = int.Parse(table.DocumentNode.SelectNodes("//input[@id= 'documentsPagesCount']")[0].Attributes["value"].Value);
            try {
                foreach (var row in table.DocumentNode.SelectNodes("//tr[td]")) {
                    var Row = row.SelectNodes("td").Select(td => td.InnerText).ToArray();
                    for (int i = 0; i < Row.Length; i++) {
                        var clear = Row[i].Replace("&#171;", "\"").Replace("&#187;", "\"").Replace("&quot;", "\"").Replace("\r\n", string.Empty).Trim(new char[] { '\n', '\r', '\t', ' ' }).ToString();
                        clear = Regex.Replace(clear, @"\s+", "\n");
                        var lines = clear.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
                        clear = string.Join(" ", lines);
                        if (string.IsNullOrWhiteSpace(clear)) {
                            clear = "Данных нет";
                        }
                        Row[i] = clear;
                    }

                    Table.Rows.Add(Row);
                }
            }
            catch {
                Ok.Invoke();
            }
            TableOk.Invoke(Table);
            GetDataFromOtherPages(client, request, body, pages);

        }
        private void Save(DataTable Table) {
            new ToExcel(Table, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\KolhoZ_{DateTime.Now:dd_MM_yyyy_HH-mm-ss}.xlsx");
        }
        private async Task GetNewCookies() {
            bool notgood = true;
            GetingCokies c = null;
            dispatcher.Invoke(() => { c = new GetingCokies(); });
            dispatcher.Invoke(() => c.Show());
            c.CookieOk += () => {
                this.Cookies = c.Cookies;
                dispatcher.Invoke(() => c.Close());
                notgood = false;
            };
            while (notgood) {
                await Task.Delay(10);
            }
        }

        //дадада я дада я
        private async void GetDataFromOtherPages(RestClient client, RestRequest request, KadArbitrClass body, int pages) {
            int page = 2;
            for (; page <= pages; page++) {
                await Task.Delay(new Random().Next(500, 1100));
                body.Page = page;
                var cookie = Cookies.GetAllCookies();
                request = new RestRequest("Kad/SearchInstances");
                request.AddJsonBody(body);
                request.AddHeader("origin", "https://kad.arbitr.ru");
                request.AddHeader("referer", "https://kad.arbitr.ru/");
                request.AddHeader("sec-ch-ua", "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Microsoft Edge\";v=\"108\"");
                request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
                request.AddHeader("sec-fetch-dest", "empty");
                request.AddHeader("sec-fetch-mode", "cors");
                request.AddHeader("sec-fetch-site", "same-origin");
                request.AddHeader("x-date-format", "iso");
                request.AddHeader("x-requested-with", "XMLHttpRequest");
                request.AddHeader("user-agent", UserAgents[new Random().Next(UserAgents.Length - 1)]);
                request.AddHeader("wasm", cookie.First(x => x.Name == "wasm").Value);
                request.AddHeader("pr_fp", cookie.First(x => x.Name == "pr_fp").Value);
                RestResponse response;
                try {
                    response = client.Post(request);
                }
                catch {
                    await GetNewCookies();
                    try {
                        response = client.Post(request);
                    }
                    catch (HttpRequestException) {
                        if (Table.Rows.Count != 0) {
                            Save(Table);
                        }
                        response = null;
                        System.Windows.MessageBox.Show("ip забанило, всего хоро-ше-го", "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                        Application.Current.Shutdown(1);

                    }
                }
                var table = new HtmlDocument();
                table.LoadHtml(response.Content);
                try {
                    foreach (var row in table.DocumentNode.SelectNodes("//tr[td]")) {
                        var Row = row.SelectNodes("td").Select(td => td.InnerText).ToArray();
                        for (int i = 0; i < Row.Length; i++) {
                            var clear = Row[i].Replace("&#171;", "\"").Replace("&#187;", "\"").Replace("&quot;", "\"").Replace("\r\n", string.Empty).Trim(new char[] { '\n', '\r', '\t', ' ' }).ToString();
                            clear = Regex.Replace(clear, @"\s+", "\n");
                            var lines = clear.Split('\n').Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
                            clear = string.Join(" ", lines);
                            if (string.IsNullOrWhiteSpace(clear)) {
                                clear = "Данных нет";
                            }
                            Row[i] = clear;
                        }
                        Table.Rows.Add(Row);
                    }
                    TableOk.Invoke(Table);
                }
                catch {
                    Ok.Invoke();
                }
            }

            if (Table.Rows.Count != 0) {
                Save(Table);
                Ok.Invoke();
            }
        }
    }
}
