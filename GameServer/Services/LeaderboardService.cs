using GameServer.BD;
using GameServer.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace GameServer.Services
{
    public class LeaderboardService
    {
        private readonly ConcurrentDictionary<Guid, GameResultModel> memoryDb;
        private readonly Timer leaderBoardTimer;
        private readonly ConcurrentDictionary<long, ImmutableList<LeaderboardViewModel>> leaderboards;
        private readonly object lockObject = new object();
        private static readonly object lockInstanceObject = new object();
        private static LeaderboardService instance;

        private LeaderboardService()
        {
            this.memoryDb = GameMemoryDB.Instance.DataBase;
            this.leaderBoardTimer = new Timer()
            {
                Interval = 500,
                Enabled = true
            };

            leaderBoardTimer.Elapsed += LeaderBoardTimer_Elapsed; ;
            leaderboards = new ConcurrentDictionary<long, ImmutableList<LeaderboardViewModel>>();
        }

        public bool ContainsGame(long gameId)
        {
            return leaderboards.ContainsKey(gameId);
        }
        public void Start()
        {
            leaderBoardTimer.Start();
        }
        public static LeaderboardService Instance
        {
            get
            {
                lock (lockInstanceObject)
                {
                    if (instance is null)
                    {
                        instance = new LeaderboardService();
                    }
                }
                return instance;
            }
        }
        public IEnumerable<LeaderboardViewModel> this[long gameId] { get => leaderboards[gameId]; }
        private void LeaderBoardTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                leaderBoardTimer.Enabled = false;

                var memoryDbSnap = memoryDb.Values.ToImmutableList();

                memoryDbSnap
                    .AsParallel()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .ForAll(game =>
                    {
                        var gameId = game.GameId;
                        var newLeaderboard = GenerateLeadeboard(memoryDbSnap, gameId);
                        if (!leaderboards.ContainsKey(gameId))
                        {
                            while (!leaderboards.TryAdd(gameId, newLeaderboard.ToImmutableList()))
                            {
                                Console.WriteLine("try add item");
                            }
                        }
                        else
                        {
                            leaderboards[gameId].Clear();
                            leaderboards[gameId] = newLeaderboard.ToImmutableList();
                        }
                    });
            }
            catch (Exception ex)
            {
               Console.WriteLine("error on update leaderboard");
            }
            finally
            {
                leaderBoardTimer.Enabled = true;
            }
        }

        private List<LeaderboardViewModel> GenerateLeadeboard(ImmutableList<GameResultModel> memoryDbSnap, long gameId)
        {
            return memoryDbSnap
                .Where(x => x.GameId == gameId)
                .GroupBy(x => x.PlayerId)
                .Select(games => new LeaderboardViewModel()
                {
                    Balance = games.Sum(x => x.Win),
                    LastUpdateDate = DateTime.Now,
                    PlayerId = games.Key,
                })
                .OrderByDescending(x => x.Balance)
                .Take(100)
                .ToList();
        }
    }
}
