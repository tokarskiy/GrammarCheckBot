using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace GrammarCheck
{
    /// <summary>
    ///     Контроллер, що включає в себе POST-запит для обробки повідомлень
    /// </summary>
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        ///     POST-запит за адресою api/Messages
        ///     Отримує повідомлення від користувача і відповідає на нього
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //System.Diagnostics.Debug.Print("Hello world");
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                /*
                if (activity.Text.StartsWith("/"))
                {
                    var command = activity.Text.Split(new Char[] { '/' });
                    if (command[0].ToUpper().Equals("HELP"))
                    {

                    }
                    else if (command[0].ToUpper().Equals("SETLANG"))
                    {

                    }
                    else
                    {

                    }
                }
                */
                var mistakes = Mistake.FindMistakes(activity.Text, "en-US");
                if (mistakes.Count() > 0)
                {
                    foreach (var mistake in mistakes)
                    {
                        Activity reply = activity.CreateReply(mistake.ToString());
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }
                else
                {
                    Activity reply = activity.CreateReply("No mistakes.");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}