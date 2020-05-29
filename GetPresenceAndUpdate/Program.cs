using Microsoft.BotBuilderSamples.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GetPresenceAndUpdate
{
    class Program
    {
        private static GraphClient graphClient;
        private static TableStorage tableStorage;
        private static string latestActivity;
        private static string lastestAvailability;

        static void Main(string[] args)
        {
            graphClient = new GraphClient();
            tableStorage = new TableStorage();
            tableStorage.Initialise().Wait();
            Timer presenceTimer = new Timer();
            presenceTimer.Elapsed += new ElapsedEventHandler(TimeToGetPresence);
            presenceTimer.Interval = 60000;
            presenceTimer.Enabled = true;
            TimeToGetPresence(null,null);

            Console.WriteLine("Press enter to stop");
            Console.ReadLine();

        }

        private static async void TimeToGetPresence(object sender, ElapsedEventArgs e)
        {
            var presence = await graphClient.GetPresence();
            if (presence.activity != latestActivity || presence.availability != lastestAvailability)
            {
                await tableStorage.AddPresenceInfo(presence);
                latestActivity = presence.activity;
                lastestAvailability = presence.availability;
                Console.WriteLine(latestActivity + " - " + lastestAvailability);
            }
        }

       

    }
}
