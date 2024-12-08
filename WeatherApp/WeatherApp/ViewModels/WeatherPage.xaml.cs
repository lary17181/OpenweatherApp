using WeatherApp.Services;
using WeatherApp.Models;
using System.Collections.ObjectModel;

namespace WeatherApp;

public partial class WeatherPage : ContentPage
{
    public List<Models.ForecastList> WeatherList = new List<Models.ForecastList>();
    private double latitude;
    private double longitude;

    public WeatherPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);
    }

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        if (location != null)
        {
            latitude = location.Latitude;
            longitude = location.Longitude;
        }
    }

    private async void TapLocation_Tapped(object sender, EventArgs e)
    {
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);
    }

    public async Task GetWeatherDataByLocation(double latitude, double longitude)
    {
        try
        {
            var result = await ApiService.Getweather(latitude, longitude);
            if (result != null && result.list != null)
            {
                UpdateUI(result);
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível obter os dados do tempo.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao obter os dados: {ex.Message}", "OK");
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var response = await DisplayPromptAsync(title: "Pesquisar cidade", message: "", placeholder: "Digite o nome da cidade", accept: "Pesquisar", cancel: "Cancelar");

        if (!string.IsNullOrEmpty(response))
        {
            await GetWeatherDataByCity(response);
        }
    }

    public async Task GetWeatherDataByCity(string cidade)
    {
        try
        {
            var result = await ApiService.GetweatherByCity(cidade);
            if (result != null && result.list != null)
            {
                UpdateUI(result);
            }
            else
            {
                await DisplayAlert("Erro", "Cidade não encontrada ou erro na busca.", "OK");
            }
        }
        catch (Exception ex)
        {
         
            await DisplayAlert("Erro", $"Erro ao obter os dados: {ex.Message}", "OK");
        }
    }

    public void UpdateUI(dynamic result)
    {
        WeatherList.Clear();
        foreach (var item in result.list)
        {
            WeatherList.Add(item);
        }

        CvWeather.ItemsSource = WeatherList;
        LblCity.Text = result.city.name;
        LblWeatherDescription.Text = result.list[0].weather[0].description;
        LblTemperatura.Text = result.list[0].main.temperature + "°C";
        LblHumidity.Text = result.list[0].main.humidity + "%";
        LblWind.Text = result.list[0].wind.speed + "km/h";
        ImgWeatherIcon.Source = result.list[0].weather[0].fullIconUrl;
    }
}
