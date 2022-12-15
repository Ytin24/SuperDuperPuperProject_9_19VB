using SuperCoolProject_919vb.Models;
using SuperDuperPuperProject_9_19VB.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SuperDuperPuperProject_9_19VB {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window {
        public CookieContainer cookies;
        public MainWindow() {
            InitializeComponent();
            var c = new GetingCokies();
            c.Show();
            c.CookieOk += () => {
                this.cookies = c.Cookies;
                Dispatcher.Invoke(() => c.Close());
                Dispatcher.Invoke(() => ButtonGet.IsEnabled = true);
            };
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }

        private void Exitbtn_Click(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            ButtonGet.IsEnabled = false;
            var sides = new List<Side>();
            foreach (var side in Sides.ItemText) {
                sides.Add(new Side() {
                    Name = side,
                    ExactMatch = true,
                    Type = -1,
                });

                var courts = new List<string>();
                foreach (var court in Courts.ItemText) {
                    courts.Add(new CourtsCodes().GetCode(court));
                };

                var Body = new KadArbitrClass() {
                    Count = 25,
                    DateFrom = null,
                    DateTo = null,
                    WithVKSInstances = true,
                    CaseNumbers = CaseNumbers.ItemText,
                    Judges = new(),
                    Courts = courts,
                    Sides = sides,

                };
                Parser a = new Parser(cookies);
                a.TableOk += A_TableOk;
                a.Ok += () => Dispatcher.Invoke(() => ButtonGet.IsEnabled = true);
                a.GetDataAsync(Body, Dispatcher);
            }

        }

        private void A_TableOk(System.Data.DataTable table) {
            ExcelView.ItemsSource = table.DefaultView;
        }
    }
}
