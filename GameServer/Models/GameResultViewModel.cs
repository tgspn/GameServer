using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class GameResultViewModel
    {
        [Required]
        public long? PlayerId { get; set; }
        [Required]
        public long? GameId { get; set; }
        [Required]
        public long? Win { get; set; }
        [Required]
        public DateTime? Timestamp { get; set; }
       
    }
}
