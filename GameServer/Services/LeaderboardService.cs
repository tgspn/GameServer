﻿using GameServer.BD;
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
        private readonly ConcurrentBag<GameResultModel> memoryDb;
        private readonly Timer leaderBoardTimer;
        private readonly ConcurrentDictionary<long, ImmutableList<LeaderboardViewModel>> leaderboards;

        private LeaderboardService()
        {
            Console.WriteLine("leaderboard");
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
        public static LeaderboardService Instance { get; } = new LeaderboardService();
        public IEnumerable<LeaderboardViewModel> this[long gameId] { get => leaderboards[gameId]; }
        private void LeaderBoardTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                leaderBoardTimer.Enabled = false;

                var memoryDbSnap = memoryDb.ToImmutableList();

                memoryDbSnap
                    .AsParallel()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .ForAll(game =>
                    {
                        var gameId = game.GameId;
                        var newLeaderboard = GenerateLeadeboard(memoryDbSnap, gameId);
                        if (!leaderboards.ContainsKey(gameId))
                        {
                            if (!leaderboards.TryAdd(gameId, newLeaderboard.ToImmutableList()))
                            {
                                if (leaderboards.ContainsKey(gameId))
                                {
                                    leaderboards[gameId].Clear();
                                    leaderboards[gameId]= newLeaderboard.ToImmutableList();
                                }
                            }
                            
                        }
                        else
                        {
                            leaderboards[gameId].Clear();
                            leaderboards[gameId] = newLeaderboard.ToImmutableList();
                        }
                    });
            }
            catch
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
