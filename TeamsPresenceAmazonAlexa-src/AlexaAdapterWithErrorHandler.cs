using Bot.Builder.Community.Adapters.Alexa;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsPresenceBot
{
    public class AlexaAdapterWithErrorHandler : AlexaAdapter
    {
        public AlexaAdapterWithErrorHandler(ILogger<AlexaAdapter> logger)
            : base(new AlexaAdapterOptions(), logger)
        {
            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

                // Send a message to the user
                await turnContext.SendActivityAsync("The bot encountered an error or bug.");
                await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.");
            };
        }
    }
}
