using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MaratonaBots_BotApp.Dialogs
{
    [Serializable]
    public class QnaDialog : QnAMakerDialog
    {
        public QnaDialog() : base(new QnAMakerService(
            new QnAMakerAttribute(ConfigurationManager.AppSettings["QnaSubscriptionKey"], 
                                  ConfigurationManager.AppSettings["QnaKnowledgebaseId"],
                                  "Não encontrei sua resposta",
                                  0.5,
                                  1)))
        {

        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            //return base.RespondFromQnAMakerResultAsync(context, message, result);
            var primeiraResposta = result.Answers.First().Answer;
            var resposta = ((Activity)context.Activity).CreateReply();

            var dadosResposta = primeiraResposta.Split(';');
            if (dadosResposta.Length == 1)
            {
                await context.PostAsync(primeiraResposta);
                return;
            }

            //Desmembra a resposta pelo ';' para recuperar as URLs das imagens
            var titulo = dadosResposta[0];
            var imageUrl1 = dadosResposta[1];
            var imageUrl2 = dadosResposta[2];

            HeroCard card = new HeroCard
            {
                Title = titulo
            };

            card.Buttons = new List<CardAction>
            {
                new CardAction(ActionTypes.OpenUrl, "Visite meu site", value: imageUrl1)
            };

            card.Images = new List<CardImage>
            {
                new CardImage(url: imageUrl2)
            };

            resposta.Attachments.Add(card.ToAttachment());

            await context.PostAsync(resposta);
        }
    }
}