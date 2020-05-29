using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Bot.Builder.Community.Adapters.Alexa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;


namespace TeamsPresenceBot.Controllers
{
    [Route("api/alexa")]
    [ApiController]
    public class AlexaController : ControllerBase
    {
        private readonly AlexaAdapter _adapter;
        private readonly IBot _bot;

        public AlexaController(AlexaAdapter adapter, IBot bot)
        {
            _adapter = adapter;
            _bot = bot;
        }

        [HttpPost]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await _adapter.ProcessAsync(Request, Response, _bot);
        }
    }
}
