using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsPresenceBot
{
    public class PresenceInfo : TableEntity
    {
    
        public Guid id { get; set; }
        public string availability { get; set; }
        public string activity { get; set; }

    }
}
