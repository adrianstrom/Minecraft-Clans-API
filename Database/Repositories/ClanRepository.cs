﻿using Dapper;
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
        private string FieldNames = "Name, Leader, World, X, Y, Z, Yaw, Pitch";

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

        public async Task<Clan?> GetClan(string name)
        {
            var sql = $"SELECT {FieldNames} FROM {TableName} WHERE Name = @Name";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var clan = await connection.QuerySingleOrDefaultAsync<Clan>(sql, new { Name = name});
                return clan;
            }
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

        public async Task<IEnumerable<Player>> GetClanMembers(string name)
        {
            var sql = @$"SELECT players.PlayerId, 
                         FROM players
                         JOIN clans ON players.ClanId = clans.ClanId
                         WHERE clans.Name = @Name";

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var players = await connection.QueryAsync<Player>(sql, new { Name = name });
                return players;
            }
        }

        public async Task GetClanHomes()
        {

        }

        public Task<bool> AddClanMember(Clan clan)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveClanMember(Clan clan)
        {
            throw new NotImplementedException();
        }
    }
}