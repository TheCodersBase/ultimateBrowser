using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms; 
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace WpfApp1
{
    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
    /// woops 

    public partial class MainWindow : Window

    {
        ObservableCollection<TabItem> Tabs { get; set; }
        TabItem SelectedTab { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory());
            string dir = System.IO.Path.GetDirectoryName(path);
            string parent = Directory.GetParent(dir).ToString();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/c cd {parent}\\ultBrowserData & startLocalServer.bat";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            Tabs = new ObservableCollection<TabItem>();
            tbControl.ItemsSource = Tabs;

        }
        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                args.Cancel = true;
            }
            Whatassda.ExecuteScriptAsync("setTimeout(() => { document.getElementById('copyright').outerHTML = null}, 100);");
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory());
            string dir = System.IO.Path.GetDirectoryName(path);
            string parent = Directory.GetParent(dir).ToString();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/c cd {parent}\\ultBrowserData & endLocalServer.bat";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        private void newWin(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {

            {
                Whatassda.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            }
        }

        private void updateSearch(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            textBox.Text = Whatassda.Source.ToString();
            home.Header = Whatassda.CoreWebView2.DocumentTitle;

        }

        TabItem home = new TabItem();
        WebView2 Whatassda = new WebView2();
        private void AddTab(string url)
        {
            AddTab(new Uri(url));
            tbControl.SelectedIndex = tbControl.Items.Count - 1;

        }
        public int i = 0;
        private WebView2 newBrowser;
        private void AddTab(Uri uri)
        {
            i++;
            newBrowser = new WebView2();
            newBrowser.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;
            newBrowser.Source = uri;


            Tabs.Add(new TabItem { Content = newBrowser, Name = $"tab{i}", AllowDrop = true });
            tbControl.SelectedIndex++;

            textBox.Text = newBrowser.Source.ToString(); // адресная строка
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TabItem selectedTabItem = tbControl.SelectedItem as TabItem;
                WebView2 selectedTabWebView2 = (WebView2)selectedTabItem.Content;

                if (selectedTabWebView2 != null)
                {
                    textBox.Text = selectedTabWebView2.Source.ToString(); // адресная строка
                    Debug.WriteLine(tbControl.SelectedIndex);
                }

            }
            catch {
                return;
            }
        }


        private void DocumentTitleChanged(object sender, object e)
        {
            TabItem selectedTabItem = tbControl.SelectedItem as TabItem;
            WebView2 selectedTabWebView2 = (WebView2)selectedTabItem.Content;

            if (selectedTabWebView2 != null)
            {
                textBox.Text = selectedTabWebView2.Source.ToString(); // адресная строка
                Debug.WriteLine(tbControl.SelectedIndex);
            }

            Run runHyperlink = new Run("✖️")
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            TextBlock textBlock = new TextBlock();

            Hyperlink hyperlink = new Hyperlink(runHyperlink);
            hyperlink.Click += Click_av;

            Image abob = new Image()
            {
                Source = new BitmapImage(new Uri($"https://www.google.com/s2/favicons?domain={selectedTabWebView2.CoreWebView2.Source}&sz=128")),
                Width = 10,
                Height = 10
            };
            textBlock.Inlines.Add(abob);
            textBlock.Inlines.Add(selectedTabWebView2.CoreWebView2.DocumentTitle);
            textBlock.Inlines.Add("  "); // да это просто пробел
            textBlock.Inlines.Add(hyperlink);

            HeaderedContentControl head = new HeaderedContentControl
            {
                Content = textBlock
            };
            Tabs.Last().Header = head;

            //save history with NewWindowRequested Event
            string json = File.ReadAllText("../../ultBrowserData/userData/historyStorage.json");

            List<object> objectsList = JsonConvert.DeserializeObject<List<object>>(json) ?? new List<object>();

            var newObject = new { name = $"{selectedTabWebView2.CoreWebView2.DocumentTitle}", link = $"{selectedTabWebView2.Source}" };

            objectsList.Add(newObject);
            string updatedJson = JsonConvert.SerializeObject(objectsList);
            File.WriteAllText("../../ultBrowserData/userData/historyStorage.json", updatedJson);

        }
        private void Click_av(object sender, RoutedEventArgs e) // функция удаления  теперь работает
        {

            Tabs.RemoveAt(tbControl.SelectedIndex);

            if (Tabs.Count == 0)
            {
                AddTab("https://customsearch.vercel.app/");
            }

            //selectedTabWebView2.Dispose(); // удалять вкладки из памяти (а как су)
        }
        private void Button_click(object sender, RoutedEventArgs e)
        {
            AddTab("https://customsearch.vercel.app/");
        }
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {

            e.Handled = true;
            AddTab(e.Uri);
        }
        private void k(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            home.Header = newBrowser.CoreWebView2.DocumentTitle;
        }
        //Open history page
        private void openBrowserHistory(object sender, RoutedEventArgs e)
        {
            AddTab("http://localhost:8000/userData/history.html");
        }
        private void clearBrowserHistory(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("../../ultBrowserData/userData/historyStorage.json", "");

        }
        //Navigation (search and user profile) 
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (1 > 0) // nevermind
            {
                try
                {
                    string pickUrl = textBox.Text;
                    string checkUrl = pickUrl.Substring(0, 5);

                    if (checkUrl == "https" || checkUrl == "http:")
                    {
                        AddTab($"{textBox.Text}");
                    }
                    else if (Convert.ToString(pickUrl.LastIndexOf('.')) != "-1")
                    {
                        AddTab($"https://{textBox.Text}");
                    }
                    else if (checkUrl != "https")
                    {
                        AddTab($"https://google.com/search?q={textBox.Text}");
                    }
                }
                catch
                {
                    AddTab($"https://google.com/search?q={textBox.Text}");
                }
            }
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            newBrowser.GoBack();
        }

        private void goForward(object sender, RoutedEventArgs e)
        {
            newBrowser.GoForward();

        }
        private void goToGitHub(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/TheCodersBase/ultimateBrowser");
        }
        private void WebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (sender != null)
            {
                WebView2 newBrowser = (WebView2)sender;
                newBrowser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                newBrowser.CoreWebView2.DocumentTitleChanged += DocumentTitleChanged;
            }

        }
        private void Loadeddd(object sender, RoutedEventArgs e)
        {
            //tbControl.SelectedIndex = 0;
            AddTab("https://customsearch.vercel.app/");
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //Navigation end 

    }
}