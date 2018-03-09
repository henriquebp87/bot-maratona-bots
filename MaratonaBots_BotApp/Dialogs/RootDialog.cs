using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MaratonaBots_BotApp.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //Negrito
            await context.PostAsync("**Olá, tudo bem?**");

            var message = activity.CreateReply();

            if (activity.Text.Equals("herocard", StringComparison.InvariantCultureIgnoreCase))
            {
                HeroCard heroCard = CreateHeroCard();
                message.Attachments.Add(heroCard.ToAttachment());
            }
            else if (activity.Text.Equals("videocard", StringComparison.InvariantCultureIgnoreCase))
            {
                VideoCard videoCard = CreateVideoCard();
                message.Attachments.Add(videoCard.ToAttachment());
            }
            else if (activity.Text.Equals("audiocard", StringComparison.InvariantCultureIgnoreCase))
            {
                AudioCard audioCard = CreateAudioCard();
                message.Attachments.Add(audioCard.ToAttachment());
            }
            else if (activity.Text.Equals("animationcard", StringComparison.InvariantCultureIgnoreCase))
            {
                AnimationCard animationCard = CreateAnimationCard();
                message.Attachments.Add(animationCard.ToAttachment());
            }
            else if (activity.Text.Equals("carousel", StringComparison.InvariantCultureIgnoreCase))
            {
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                var audio = CreateAudioCard();
                var animation = CreateAnimationCard();

                message.Attachments.Add(audio.ToAttachment());
                message.Attachments.Add(animation.ToAttachment());
            }

            await context.PostAsync(message);

            context.Wait(MessageReceivedAsync);
        }

        private static HeroCard CreateHeroCard()
        {
            return new HeroCard
            {
                Title = "Planeta",
                Subtitle = "Universo",
                Images = new List<CardImage>
                {
                    new CardImage("https://img.purch.com/h/1000/aHR0cDovL3d3dy5zcGFjZS5jb20vaW1hZ2VzL2kvMDAwLzAwOS80NzQvb3JpZ2luYWwvbmFzYS1zb2xhci1zeXN0ZW0tZ3JhcGhpYy03Mi5qcGc=", "Planeta")
                },
                Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Text= "Botão 1",
                        DisplayText = "Display",
                        Title = "Title",
                        Type= ActionTypes.PostBack,
                        Value="audiocard"
                    }
                }
            };
        }

        private static VideoCard CreateVideoCard()
        {
            return new VideoCard
            {
                Title = "Um vídeo qualquer",
                Subtitle = "Aqui vai um subtítulo",
                Autostart = true,
                Autoloop = false,
                Media = new List<MediaUrl>
                    {
                        new MediaUrl { Url = "http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4"}
                    }
            };
        }

        private static AudioCard CreateAudioCard()
        {
            return new AudioCard
            {
                Title = "Um áudio qualquer",
                Subtitle = "Aqui vai um subtítulo do áudio",
                Image = new ThumbnailUrl("https://img.purch.com/h/1000/aHR0cDovL3d3dy5zcGFjZS5jb20vaW1hZ2VzL2kvMDAwLzAwOS80NzQvb3JpZ2luYWwvbmFzYS1zb2xhci1zeXN0ZW0tZ3JhcGhpYy03Mi5qcGc="),
                Autostart = true,
                Autoloop = false,
                Media = new List<MediaUrl>
                    {
                        new MediaUrl { Url = "http://www.wavlist.com/movies/004/father.wav"}
                    }
            };
        }

        private static AnimationCard CreateAnimationCard()
        {
            return new AnimationCard
            {
                Title = "Uma animação qualquer",
                Subtitle = "Aqui vai um subtítulo da animação",
                Autostart = true,
                Autoloop = false,
                Media = new List<MediaUrl>
                {
                    new MediaUrl { Url = "http://3.bp.blogspot.com/-Zayhbr7-BAI/VEuO_tl6QRI/AAAAAAAAETI/EDwX8NmwfPY/s1600/iceage_small2.jpg.gif"}
                }
            };
        }
    }
}