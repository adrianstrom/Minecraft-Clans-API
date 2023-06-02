using ApiSite.Protos;
using Database.Models;

namespace ApiSite.Mapper
{
    public static class ClanMapper
    {
        public static Database.Models.Clan ToDbClan(AddClanRequest source)
        {
            var clan = new Database.Models.Clan();
            clan.Name = source.Name;
            clan.Leader = source.Leader;
            clan.Location = new Database.Models.Location()
            {
                World = source.Location.World,
                X = source.Location.X,
                Y = source.Location.Y,
                Z = source.Location.Z,
                Pitch = source.Location.Pitch,
                Yaw = source.Location.Yaw,
            };
            return clan;
        }
    }
}
