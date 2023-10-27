using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Net.Http.Json;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using static System.Net.Mime.MediaTypeNames;

namespace TestNetwork
{
    public sealed partial class MainPage : Page
    {
        readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void GetButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetUrl is null)
            {
                ShowAlert("Error", "Please enter a URL for a GET request");
                return;
            }
            using var client = new HttpClient();
            var response = await client.GetAsync(GetUrl);
            ShowAlert(response.IsSuccessStatusCode ? "Success" : $"Error ({response.StatusCode})", await response.Content.ReadAsStringAsync());
        }

        private async void PostButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PostUrl) || string.IsNullOrEmpty(PostBody))
            {
                ShowAlert("Error", "Please enter a URL and BODY for a a POST request");
                return;
            }
            using var client = new HttpClient();
            var result = await client.PostAsJsonAsync(PostUrl, PostBody);
        }

        public string GetUrl 
        {
            get => GetSetting("GetUrl");
            set => SetSetting("GetUrl", value); 
        }
        
        public string PostUrl 
        {
            get => GetSetting("PostUrl");
            set => SetSetting("PostUrl", value); 
        }
        
        public string PostBody
        {
            get => GetSetting("PostBody");
            set => SetSetting("PostBody", value); 
        }

        private void SetSetting(string key, string value)
        {
            localSettings.Values[key] = value;
        }

        private string GetSetting(string key)
        {
            return localSettings.Values[key] as string;
        }

        private async void ShowAlert(string title, string message)
        {
            ContentDialog messageDialog = new ContentDialog()
            {
                Title = title,
                Content = new ScrollViewer()
                {
                    Content = new TextBlock() { Text = message },
                },
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };
            await messageDialog.ShowAsync();
            
        }
    }
}