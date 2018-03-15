using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace MaratonaBots_BotApp.Dialogs
{
    [Serializable]
    //[LuisModel("c8f1fdf9-de7d-44a9-b8e4-931bf1deccda", "3f5f877eb1f9425084202301d7ee4233")]
    public class CotacaoDialog : LuisDialog<object>
    {
        public CotacaoDialog(ILuisService service) : base(service) { }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não consegui entender a frase {result.Query}");
        }

        [LuisIntent("Sobre")]
        public async Task Sobre(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Eu sou um bot e estou sempre aprendendo, tenha paciência comigo.");
        }

        [LuisIntent("Cumprimento")]
        public async Task Cumprimento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Olá, eu sou um bot que informa as cotações de moedas!");
        }

        [LuisIntent("Cotacao")]
        public async Task Cotacao(IDialogContext context, LuisResult result)
        {
            var moedas = result.Entities?.Select(e => e.Entity);
            string endpoint = $"http://api-cotacoes-maratona-bots.azurewebsites.net/api/cotacoes/{string.Join(",", moedas)}";

            await context.PostAsync($"Aguarde um momento enquanto eu obtenho as cotações...");

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                    await context.PostAsync($"Ocorreu algum erro ao buscar sua cotação. Tente mais tarde :(");
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<Models.Cotacao[]>(json);
                    var cotacoes = resultado.Select(c => $"{c.Nome} ({c.Sigla}) -> R${c.Valor}");
                    await context.PostAsync($"{string.Join(",", cotacoes)}");
                }
            }
        }
    }
}