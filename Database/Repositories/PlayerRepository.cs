using Dapper;
using Database.Models;
using Database.Options;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private ClansDatabaseOptions _options;
        private string _connectionString;

        private string TableName => "players";
        private string FieldNames = "Id, PlayerId, ClanId";

        public PlayerRepository(IOptions<ClansDatabaseOptions> options)
        {
            _options = options.Value;
            _connectionString = $"Server={_options.Server};Port={_options.Port};Database={_options.Database};Uid={_options.UserName};Pwd={_options.Password};";
        }

        public async Task<bool> AddPlayer(Player player)
        {
            var sql = @$"INSERT INTO {TableName} 
                        (PlayerId) 
                        VALUES 
                        (@PlayerId)";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { PlayerId = player.PlayerId});
            }
            return true;
        }

        public async Task<Player?> GetPlayer(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Player>> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetMemberOfClan(string playerId, int clanId)
        {
            var sql = @$"UPDATE {TableName}
                         SET ClanId = @ClanId
                         WHERE PlayerId = @PlayerId";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { ClanId = clanId, PlayerId = playerId });
            }
            return true;
        }
    }
}
