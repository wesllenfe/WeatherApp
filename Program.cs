using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Write("Informe o nome da cidade: ");
        string cityName = Console.ReadLine();

        Console.WriteLine($"Obtendo informações climáticas da cidade {cityName}");

        string apiKey = Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Chave de API não encontrada. Certifique-se de definir a variável de ambiente OPEN_WEATHER_API_KEY.");
            return;
        }

        string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>();

                    Console.WriteLine($"Condição: {weatherData.Weather[0].Description}");
                    Console.WriteLine($"Temperatura: {weatherData.Main.Temp}ºC");
                    Console.WriteLine($"Umidade:  {weatherData.Main.Humidity}%");
                    double windSpeedKM = weatherData.Wind.Speed * 3.6;
                    Console.WriteLine($"Velocidade do Vento: {weatherData.Wind.Speed} m/s ou {windSpeedKM:F2} KM/H");
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na requisição: {ex.Message}");
            }
        }
    }
}

public class WeatherData
{
    public Weather[] Weather { get; set; }
    public Main Main { get; set; }
    public Wind Wind { get; set; }
}

public class Weather
{
    public string Description { get; set; }
}

public class Main
{
    public float Temp { get; set; }
    public int Humidity { get; set; }
}

public class Wind
{
    public float Speed { get; set; }
}
