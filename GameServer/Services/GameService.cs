using GameServer.BD;
using GameServer.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace GameServer.Services
{
    public class GameService
    {
        private readonly ConcurrentBag<GameResultModel> memoryDb;

        public GameService()
        {
            this.memoryDb = GameMemoryDB.Instance.DataBase;           
        }
                  

        public void Save(GameResultModel model)
        {
            memoryDb.Add(model);
        }
    }
}
