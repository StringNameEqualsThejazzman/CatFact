using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using DeepL;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Linq;

namespace CatFact;

public partial class MainPage : ContentPage
{
    private const string ApiBaseUrl = "https://catfact.ninja";

    public MainPage()
    {
		InitializeComponent();
        LoadCatFact();
        Output.ItemsSource = DataCatFact.GetData();

    }


    //----------------méthode async qui permet de se connecter à l'api et de récuperer le JSON----------------
    private async void LoadCatFact()
	{
        //connection a Deepl via le nugget package
        var authKey = "d4d21ea8-37b9-ab4b-defe-a9a7e2efad92:fx";
        var translator = new Translator(authKey);

        //gestion des exceptions
        try
        {
            //Je crée le client HTTP
            var client = new HttpClient();

            //Je construit le chemin de l'url pour récupérer un fait sur les chats
            var apiUrl = $"{ApiBaseUrl}/fact";

            //Envoie d'une requête GET pour récuperer le fichier texte
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var fact_string = await response.Content.ReadAsStringAsync();

            //---------------------------------------------------------

            //opération pour retirer toutes informations superflue du string reçu de l'api
            int length = fact_string.IndexOf("\"length\":");
            string length_string = fact_string.Substring(length);

            fact_string = fact_string.Replace("{\"fact\":\"", "");
            fact_string = fact_string.Replace("\",\"length\":", "");

            length = length_string.IndexOf(":");
            length_string = length_string.Substring(length + 1);
            length_string = length_string.Replace("\",\"length\":", "");

            fact_string = fact_string.Replace(length_string, "");

            //---------------------------------------------------------

            //
            var fact_string_translated = await translator.TranslateTextAsync(
                  fact_string,
                  LanguageCode.English,
                  LanguageCode.French);
            Convert.ToString(fact_string_translated);
            //---------------------------------------------------------


            //création de l'objet avec les informations de la requête
            var catFactResponse = new CatFactResponse(Convert.ToString(fact_string_translated));

            //Mise à jour de l'affichage client
            CatFactLabel.Text = catFactResponse.Fact;
        }
        catch (HttpRequestException ex)
        {
            CatFactLabel.Text = "Erreur lors de la récupération du fait sur les chats.";
            Console.WriteLine($"Erreur lors de la récupération du fait sur les chats : {ex.Message}");
        }
        catch (Exception ex)
        {
            CatFactLabel.Text = "Erreur lors de la récupération du fait sur les chats.";
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    private async void OnNewFactClicked(object sender, EventArgs e)
    {
        try
        {
            LoadCatFact();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible de charger le fait sur les chats : " + ex.Message, "OK");
        }
    }

    private void AddData(object sender, EventArgs e)
    {
        DataCatFact.AddData(CatFactLabel.Text);

        Output.ItemsSource = DataCatFact.GetData();
    }

    private void DeleteData(object sender, EventArgs e)
    {
        DataCatFact.DeleteData();
        Output.ItemsSource = DataCatFact.GetData();
    }
}

public class CatFactResponse
{
    public string Fact { get; set; }

    public CatFactResponse(string fact)
    {
        this.Fact = fact;
    }
}

