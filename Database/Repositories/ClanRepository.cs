using Dapper;
using Database.Models;
using Database.Options;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System.Xml.Linq;

namespace Database.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private ClansDatabaseOptions _options;
        private string _connectionString;

        private string TableName => "clans";
        private string FieldNames = "Id, Name, Leader, World, X, Y, Z, Yaw, Pitch";

        public ClanRepository(IOptions<ClansDatabaseOptions> options)
        {
            _options = options.Value;
            _connectionString = $"Server={_options.Server};Port={_options.Port};Database={_options.Database};Uid={_options.UserName};Pwd={_options.Password};";
        }

        public async Task<int> AddClan(Clan clan)
        {
            var clanId = -1;
            var sql = @$"INSERT INTO {TableName} 
                        (Name, Leader, World, X, Y, Z, Yaw, Pitch) 
                        VALUES 
                        (@Name, @Leader, @World, @X, @Y, @Z, @Yaw, @Pitch);
                        SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                clanId = connection.QueryFirstOrDefault<int>(sql, clan.GetParameters());
                return clanId;
            }
        }

        public async Task<Clan?> GetClan(string name, bool includeLocation = false, bool includeMembers = false, bool includeHomes = false)
        {
            var sql = $"SELECT {FieldNames} FROM {TableName} WHERE Name = @Name";

            Clan clan;
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                clan = await connection.QuerySingleOrDefaultAsync<Clan>(sql, new { Name = name});

                if (includeLocation)
                {
                    clan.Location = await GetClanLocation(name, connection);
                }

                if (includeMembers)
                {
                    clan.Members = await GetClanMembers(name, connection);
                }

                if (includeHomes)
                {
                    clan.Homes = await GetClanHomes(name, connection);
                }
                return clan;
            }
        }

        private async Task<Location> GetClanLocation(string name, MySqlConnection connection)
        {
            var sql = @$"SELECT World, X, Y, Z, Yaw, Pitch
                         FROM clans
                         WHERE clans.Name = @Name";
            var location = await connection.QuerySingleOrDefaultAsync<Location>(sql, new { Name = name });
            return location;
        }

        private async Task<IEnumerable<Player>> GetClanMembers(string name, MySqlConnection connection)
        {
            var sql = @$"SELECT players.PlayerId, players.ClanId
                         FROM players
                         JOIN clans ON players.ClanId = clans.Id
                         WHERE clans.Name = @Name";
            var clanMembers = await connection.QueryAsync<Player>(sql, new { Name = name });
            return clanMembers;
        }

        private async Task<IEnumerable<Location>> GetClanHomes(string name, MySqlConnection connection)
        {
            var tableName = "clan_homes";
            var sql = @$"SELECT {tableName}.World, {tableName}.X, {tableName}.Y, {tableName}.Z, {tableName}.Yaw, {tableName}.Pitch
                         FROM clan_homes
                         JOIN clans ON {tableName}.ClanId = clans.Id
                         WHERE clans.Name = @Name";
            var homes = await connection.QueryAsync<Location>(sql, new { Name = name });
            return homes;
        }

        public async Task<IEnumerable<Clan>> GetClans()
        {
            var sql = $"SELECT {FieldNames} FROM {TableName}";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var clans = await connection.QueryAsync<Clan>(sql);
                return clans;
            }
        }

        public Task<bool> UpdateClan(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteClan(string name)
        {
            var sql = $"DELETE FROM {TableName} WHERE Name = @Name";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(sql, new { Name = name });
            }
            return true;
        }

        public Task<bool> AddClanMember(string playerId, int clanId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveClanMember(string playerId, int clanId)
        {
            throw new NotImplementedException();
        }

        public Task<Clan?> GetClan(int id)
        {
            throw new NotImplementedException();
        }
    }
}
