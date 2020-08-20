using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class LeaderboardViewModel
    {
        public long PlayerId { get; set; }
        public long Balance { get; set; }
        public DateTime LastUpdateDate { get; set; }

    }
}
