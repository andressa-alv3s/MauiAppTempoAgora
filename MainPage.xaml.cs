using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Net;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao =
                            $"Lat: {t.lat}\n" +
                            $"Lon: {t.lon}\n" +
                            $"Temp. Min: {t.temp_min} ºC\n" +
                            $"Temp. Max: {t.temp_max} ºC\n" +
                            $"Visibilidade: {t.visibility} m\n" +
                            $"Velocidade do Vento: {t.speed} m/s\n" +
                            $"Clima: {t.main}\n" +
                            $"Descrição: {t.description}\n" +
                            $"Nascer do Sol: {t.sunrise}\n" +
                            $"Pôr do Sol: {t.sunset}\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else 
                    {
                        await DisplayAlert("Cidade não encontrada",
                            $"Não localizamos \"{txt_cidade.Text}\". Verifique o nome e tente novamente.",
                            "Ok"); // Mensagem de erro se a cidade não for encontrada
                    }
                }
                else
                {
                    lbl_res.Text = "Informe uma cidade válida";// Mensagem de erro se o campo estiver vazio
                }
            }
            catch (HttpRequestException) 
            {
                await DisplayAlert("Sem conexão",
                    "Não foi possível conectar. Verifique sua internet e tente novamente.",
                    "Ok"); // Mensagem de erro se não houver conexão com a internet
            }
            catch (WebException webEx) when ((webEx.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound) //
            {
                await DisplayAlert("Cidade não encontrada",
                    $"A cidade \"{txt_cidade.Text}\" não foi localizada.",
                    "Ok");             }
            catch (Exception ex)
            {
                await DisplayAlert("Erro inesperado", ex.Message, "OK");
            } // Mensagem de erro geral
        }
    }
}
