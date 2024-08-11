using System;
using System.Collections.ObjectModel;
using OpenInWhatsapp.Model;

namespace OpenInWhatsapp.View;

public partial class MainPage : ContentPage
{
    string code = "India", Value;
    string index;
    CountryCodeModel countryCodeModel = new CountryCodeModel();
    public MainPage()
    {
        InitializeComponent();
        List<KeyValuePair<string, string>> countryCodeList = countryCodeModel.ToKeyValuePairList();
        CountrySelector.ItemsSource = new ObservableCollection<string>(countryCodeList.Select(kvp => kvp.Key));
        if (Preferences.Default.ContainsKey(index))
        {
            CountrySelector.SelectedItem = Preferences.Default.Get(index, "1");
        }
        else { CountrySelector.SelectedItem = code; }
    }

    private async void openNumber(object sender, EventArgs e)
    {
        code = CountrySelector.SelectedItem.ToString();
        index = CountrySelector.SelectedIndex.ToString();
        if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(ContactNumber.Text))
        {
            Preferences.Default.Set(index, "1");
            countryCodeModel.CountryCodeMapping.TryGetValue(code, out Value);
            try
            {
                Uri uri = new Uri($"https://api.whatsapp.com/send?phone=+{Value}{ContactNumber.Text}");
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"{ex}", "Ok");
            }

        }
        

    }
}
