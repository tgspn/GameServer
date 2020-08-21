using GameServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.BD
{
    public class ApplicationDbContext : DbContext
    {
        private static DatabaseType _type;
        private static string _connectionString;

        public DbSet<GameResultModel> GameResult { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            switch (_type)
            {
                case DatabaseType.SQLServer:
                    optionsBuilder.UseSqlServer(_connectionString);
                    break;
                case DatabaseType.InMemoryDatabase:
                case DatabaseType.Default:
                default:
                    optionsBuilder.UseInMemoryDatabase("GameResult");
                    break;
            }
        }
        public static void SetConfiguration(DatabaseType type,string connectionString="")
        {
            _type = type;
            _connectionString = connectionString;
        }
    }
    public enum DatabaseType
    {
        Default,
        SQLServer,
        InMemoryDatabase
    }
}
