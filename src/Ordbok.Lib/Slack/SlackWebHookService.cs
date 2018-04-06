using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebHooks;
using OrdbokApi.Lib.Models;

namespace OrdbokApi.Lib.Slack
{
    public class SlackWebHookService
    {
        private readonly IOrdbokService _service;

        public SlackWebHookService(IOrdbokService service)
        {
            _service = service;
        }

        public async Task<SlackSlashResponse> GenerateSlackWebHookResponse(SlackSlashCommandUserInput input)
        {
            if (string.IsNullOrEmpty(input.Phrase))
            {
                return CreatePrivateResponse("Tomme ord, tomme løfter, hæh?! Prøv igjen.");
            }

            try
            {
                var response = await _service.GetOrdbokResponse(input.Phrase);
                if (response.Artikler.Any())
                {
                    return CreatePublicResponse(response, input);
                }
                return CreatePrivateResponse($"Fant ikke '{input.Phrase}' :/");
            }
            catch (NonSuccessfulHttpQueryAgainstUiBException e)
            {
                Trace.TraceError(e.ToString());
                return CreatePrivateResponse("Ser ikke ut som vi klarte å kalle Bergen idag. Prøv igjen senere. :/");
            }
        }

        private SlackSlashResponse CreatePublicResponse(OrdbokResponse ordbokResponse, SlackSlashCommandUserInput input)
        {
            var slashReply = new SlackSlashResponse("");

            foreach (var response in ordbokResponse.Artikler)
            {
                var att = CreateAtt(response);
                slashReply.Attachments.Add(att);
            }

            slashReply.ResponseType = "in_channel";
            //<http://www.foo.com|www.foo.com>
            slashReply.Text =  $"'<{ordbokResponse.Link}|{input.Phrase}>' (via {input.Username}). ";
            return slashReply;
        }

        private static SlackSlashResponse CreatePrivateResponse(string responseToUser)
        {
            return new SlackSlashResponse(responseToUser);
        }

        private SlackAttachment CreateAtt(OrdbokArticle ordbokArticle)
        {
            var text = !ordbokArticle.Tydinger.Any() ? ordbokArticle.Forklaring : "";
            var att = new SlackAttachment("", "")
            {
                Color = "#aa6a0a",
                Fallback = ordbokArticle.Oppslagsord + " :: " + ordbokArticle.Forklaring + ". " + string.Join(",", ordbokArticle.Tydinger.Select(d => d.ToString())),
                Text = text
            };

            foreach (var tyding in ordbokArticle.Tydinger)
            {
                var sf = new SlackField($"{tyding.Nummer}. {tyding.Tekst}", string.Join(", ", tyding.Eksempler));
                att.Fields.Add(sf);
            }

            return att;
        }
    }
}