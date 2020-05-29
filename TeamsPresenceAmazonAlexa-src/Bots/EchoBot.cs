// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using TeamsPresenceBot;
using System.Linq;
using System.Text;
using System;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var storage = new TableStorage();
            await storage.Initialise();
            var info = await storage.GetTodaysPresenceInfo();
                                 
            var replyText = TurnPresenceInfoListIntoResponse(info);

            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        private string TurnPresenceInfoListIntoResponse(List<PresenceInfo> presenceInfo)
        {
            var currentPresence = presenceInfo.OrderByDescending(p => p.Timestamp).First();
            var nextDiscreteState = presenceInfo.Where(p => p.activity != currentPresence.activity).OrderByDescending(p => p.Timestamp).FirstOrDefault();
            var earliestSimilarState = presenceInfo.Where(p => p.activity == currentPresence.activity && p.Timestamp > nextDiscreteState.Timestamp).OrderBy(p => p.Timestamp).First();

            var sb = new StringBuilder();
            sb.Append("Right now, Tom ");
            sb.Append(ConvertActivity(currentPresence.activity));
            sb.Append(".");
            if (nextDiscreteState != null)
            {
                var length = DateTime.UtcNow.Subtract(earliestSimilarState.Timestamp.UtcDateTime).TotalMinutes;
                sb.Append(" He has been like this for ");
                sb.Append(length.ToString("N0"));
                sb.Append(" minutes.");                
            }
            return sb.ToString();   
        }

        private string ConvertActivity(string activity)
        {
            switch (activity)
            {
                case "Available": return "is free";
                case "Away": return "is away from his computer";
                case "BeRightBack": return "has stepped away";
                case "Busy": return "is busy";
                case "DoNotDisturb": return "is in Do Not Disturb state";
                case "InACall": return "is in a call";
                case "InAConferenceCall": return "is in a meeting";
                case "Inactive": return "is not showing as active";
                case "InAMeeting": return "is in a meeting";
                case "Offline": return "is offline";
                case "OffWork": return "is off work";
                case "OutOfOffice": return "it out of the office";
                case "PresenceUnknown": return "is in an unknown state";
                case "Presenting": return "is sharing his screen in a presentation";
                case "UrgentInterruptionsOnly": return "has marked himself for urgent interruptions only";
                default: return "is in an unknown state";
            }
        }
    }
}
