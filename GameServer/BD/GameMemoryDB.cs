using GameServer.Models;
using System;
using System.Collections.Concurrent;

namespace GameServer.BD
{
    public class GameMemoryDB
    {
        private readonly ConcurrentDictionary<Guid, GameResultModel> db;
        private static GameMemoryDB instance;
        private static readonly object lockObject = new object();

        private GameMemoryDB()
        {
            db = new ConcurrentDictionary<Guid, GameResultModel>();
        }
        public static GameMemoryDB Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance is null)
                        instance = new GameMemoryDB();

                    return instance;
                }
            }
        }

        public ConcurrentDictionary<Guid, GameResultModel> DataBase { get => db; }

    }
}
