using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaratonaBots_BotApp.Formulario
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Desculpe, não entendi \"{0}\".")]
    public class Pedido
    {
        public Salgadinhos Salgadinho { get; set; }
        public Bebidas Bebida { get; set; }
        public TipoEntrega TipoEntrega { get; set; }
        public CPFNaNota CPFNaNota { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public static IForm<Pedido> BuildForm()
        {
            var form = new FormBuilder<Pedido>();
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Configuration.Yes = new string[] { "sim", "s", "yes", "yep", "y" };
            form.Configuration.No = new string[] { "não", "nao", "n", "no", "not" };
            form.Message("Olá, seja bem vindo. Será um prazer atendê-lo.");
            form.OnCompletion(async (context, pedido) =>
            {
                //Salvar na base de dados
                //Gerar pedido
                //Integrar com serviço XPTO
                await context.PostAsync("Seu pedido número 123456 foi gerado e em instantes será entregue.");
            });
            return form.Build();
        }
    }

    [Describe("Tipo de entrega")]
    public enum TipoEntrega
    {
        [Terms("retirar no local", "passo ai", "eu pego", "retiro ai")]
        [Describe("Retirar no local")]
        RetirarNoLocal = 1,
        [Terms("entrega por motoboy", "motoboy", "motoca", "cachorro louco", "entrega", "em casa")]
        [Describe("Entrega por motoboy")]
        Motoboy
    }

    [Describe("Salgado")]
    public enum Salgadinhos
    {
        [Terms("Esfiha", "Esfirra", "isfirra", "e")]
        [Describe("Esfiha")]
        Esfiha = 1,
        [Terms("Quibe", "kibe", "q", "k")]
        Quibe,
        [Terms("Coxinha", "Cochinha", "c")]
        Coxinha
    }

    [Describe("Bebida")]
    public enum Bebidas
    {
        [Terms("Água", "água", "h2o", "a")]
        [Describe("Água")]
        Agua = 1,
        [Terms("Refrigerante", "refri", "r")]
        [Describe("Refri")]
        Refrigerante,
        [Terms("Suco", "s")]
        [Describe("Suco")]
        Suco
    }

    [Describe("CPF na Nota")]
    public enum CPFNaNota
    {
        [Terms("Sim", "s", "y", "yes", "yep")]
        [Describe("Sim")]
        Sim = 1,
        [Terms("não", "nao", "n", "no", "not")]
        [Describe("Não")]
        Nao
    }
}