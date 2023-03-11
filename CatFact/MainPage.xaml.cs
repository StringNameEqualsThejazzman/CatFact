using System.Net.Http;
using System.Text.Json;

namespace CatFact;

public partial class MainPage : ContentPage
{
    private const string ApiBaseUrl = "https://catfact.ninja";

    public MainPage()
	{
		InitializeComponent();
	}

    //----------------méthode async qui permet de se connecter à l'api et de récuperer le JSON----------------
    private async void LoadCatFact()
	{
        //gestion des exceptions
        try
        {
            //Je crée le client HTTP
            using var client = new HttpClient();

            //Je construit le chemin de l'url pour récupérer un fait sur les chats
            var apiUrl = $"{ApiBaseUrl}/fact";

            //Envoie d'une requête GET pour récuperer le fichier JSON
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            //deserialisation du fuchier Json en un obejt
            var catFactResponse = JsonSerializer.Deserialize<CatFactResponse>(jsonString);

            //Mise à jour de l'affichage client
            CatFactLabel.Text = catFactResponse.Fact;
        }
        catch (HttpRequestException ex)
        {
            CatFactLabel.Text = "Erreur lors de la récupération du fait sur les chats.";
            Console.WriteLine($"Erreur lors de la récupération du fait sur les chats : {ex.Message}");
        }
        catch (JsonException ex)
        {
            CatFactLabel.Text = "Erreur lors de la récupération du fait sur les chats.";
            Console.WriteLine($"Erreur de la désérialisation du fichier JSON : {ex.Message}");
        }
        catch (Exception ex)
        {
            CatFactLabel.Text = "Erreur lors de la récupération du fait sur les chats.";
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }
}

public class CatFactResponse
{
    public string Fact { get; set; }
}

