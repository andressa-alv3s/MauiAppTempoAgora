using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace MauiAppTempoAgora.Services
{
    internal class DataService
    {
       public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "f23d755c5cc93b7b7c11b5e59cc62378";

            string url = $"https://api.openweathermap.org/data/2.5/weather" +
                $"?q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync(); //
                    var rascunho = JObject.Parse(json); //

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]?["sunrise"]).ToLocalTime(); 
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]?["sunset"]).ToLocalTime();

                    t = new()
                    {

                        lat = rascunho["coord"]?["lat"]?.Value<double>() ?? 0,      
                        lon = rascunho["coord"]?["lon"]?.Value<double>() ?? 0,
                        description = rascunho["weather"]?[0]?["description"]?.Value<string>() ?? "",
                        main = rascunho["weather"]?[0]?["main"]?.Value<string>() ?? "",
                        temp_min = rascunho["main"]?["temp_min"]?.Value<double>() ?? 0,
                        temp_max = rascunho["main"]?["temp_max"]?.Value<double>() ?? 0,
                        visibility = rascunho["visibility"]?.Value<int>() ?? 0,
                        speed = rascunho["wind"]?["speed"]?.Value<double>() ?? 0,
                        sunrise = sunrise.ToString("HH:mm"),
                        sunset = sunset.ToString("HH:mm")

                    }; // fechamento do new Tempo{

                }// fechamento do if (resp.IsSuccessStatusCode){
            } // fechamento do using (HttpClient client = new HttpClient()){


            return t;
        }// fechamento do public static async Task<Tempo?> GetPrevisao(string cidade){
    }// fechamento do namespace MauiAppTempoAgora.Services
}// fechamento do namespace MauiAppTempoAgora.Services
