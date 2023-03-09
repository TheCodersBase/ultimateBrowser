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

using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Reflection;

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

            //string urlAdress = Whatassda.Source.ToString();
            //textBox.Text = urlAdress;
            Whatassda.NavigationStarting += EnsureHttps;

            Tabs = new ObservableCollection<TabItem>();
            tbControl.ItemsSource = Tabs;
        }
        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {

            Whatassda.ExecuteScriptAsync("setTimeout(() => { document.getElementById('copyright').outerHTML = null}, 100);");
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

        }
        public int i = 0;
        private WebView2 newBrowser;
        private void AddTab(Uri uri)
        {
            i++;
            newBrowser = new WebView2();
            newBrowser.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;
            newBrowser.Source = uri;

            //string title = newBrowser.CoreWebView2.DocumentTitle;


            Tabs.Add(new TabItem {Content = newBrowser, Name = $"tab{i}" });
            tbControl.SelectedIndex++;
            
            textBox.Text = newBrowser.Source.ToString(); // адресная строка
        }
        private void DocumentTitleChanged(object sender, object e)
        {
            Run runHyperlink = new Run("✖️")
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            TextBlock textBlock = new TextBlock();

            Hyperlink hyperlink = new Hyperlink(runHyperlink);
            hyperlink.Click += Click_av;

            textBlock.Inlines.Add(newBrowser.CoreWebView2.DocumentTitle);
            textBlock.Inlines.Add("  "); // да это просто пробел
            textBlock.Inlines.Add(hyperlink);

            HeaderedContentControl head = new HeaderedContentControl
            {
                Content = textBlock
            };
            Tabs[tbControl.SelectedIndex].Header = head;
        }
        private void Click_av(object sender, RoutedEventArgs e) // функция удаления  теперь работает
        {

            Tabs.RemoveAt(tbControl.SelectedIndex);

        }
        private void Button_click(object sender, RoutedEventArgs e)
        {
            AddTab("https://customsearch.vercel.app/");
        }
        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            AddTab(e.Uri);
        }
        private void k(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            home.Header = Whatassda.CoreWebView2.DocumentTitle;
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
            Whatassda.GoBack();
        }

        private void goForward(object sender, RoutedEventArgs e)
        {
            Whatassda.GoForward();

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
        //Navigation end 
    }
}