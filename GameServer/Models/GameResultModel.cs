using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class GameResultModel
    {
        public Guid Id { get; set; }
        public long PlayerId { get; set; }
        public long GameId { get; set; }
        public long Win { get; set; }
        public DateTime Timestamp { get; set; }

        public static explicit operator GameResultModel(GameResultViewModel model)
        {
            return new GameResultModel()
            {
                GameId = model.GameId.Value,
                PlayerId = model.PlayerId.Value,
                Timestamp = model.Timestamp.Value,
                Win = model.Win.Value
            };
        }
    }
}
