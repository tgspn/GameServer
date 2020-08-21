using GameServer.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;

namespace GameServer.BD
{
    public class GameMemoryDB
    {
        private readonly ConcurrentBag<GameResultModel> db;
        private readonly Timer persistenceTimer;
        private GameMemoryDB()
        {
            Console.WriteLine("new instance");
            db = new ConcurrentBag< GameResultModel>();
            this.persistenceTimer = new Timer()
            {
                Interval = TimeSpan.FromMinutes(1).TotalMilliseconds,
                Enabled = true
            };

            persistenceTimer.Elapsed += PersistenceTimer_Elapsed;
            persistenceTimer.Start();


            LoadFromBd();
        }
        public static GameMemoryDB Instance { get; } = new GameMemoryDB();

        public ConcurrentBag<GameResultModel> DataBase { get => db; }
        public void SetPersistenceInterval(TimeSpan time)
        {
            persistenceTimer.Interval = time.TotalMilliseconds;
        }
        private void LoadFromBd()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    foreach (var item in context.GameResult)
                    {
                        db.Add(item);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private void PersistenceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("persisting datas");
            using (ApplicationDbContext conext = new ApplicationDbContext())
            {
                foreach (var item in db.Where(x => x.Id == default(Guid)))
                {
                    conext.Add(item);
                }
                conext.SaveChanges();
            }
        }

    }
}
