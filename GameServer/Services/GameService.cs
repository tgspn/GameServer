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
        private readonly ConcurrentDictionary<Guid, GameResultModel> memoryDb;
        private readonly Timer persistenceTimer;

        public GameService(TimeSpan periodicSave)
        {
            this.memoryDb = GameMemoryDB.Instance.DataBase;
            this.persistenceTimer = new Timer()
            {
                Interval = periodicSave.TotalMilliseconds,
                Enabled = true
            };

            persistenceTimer.Elapsed += PersistenceTimer_Elapsed;
            persistenceTimer.Start();


            LoadFromBd();
        }

        private void PersistenceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

        }
        private void LoadFromBd()
        {

        }

        public void Save(GameResultModel model)
        {
            if (model.Id == default(Guid))
                model.Id = Guid.NewGuid();

            memoryDb[model.Id] = model;
        }
    }
}
