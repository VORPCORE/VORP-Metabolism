using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpmetabolism_sv
{
    public class vorpmetabolism_init : BaseScript
    {
        public static Dictionary<Player, string> lastPlayerStatus = new Dictionary<Player, string>();

        public vorpmetabolism_init()
        {
            EventHandlers["vorpmetabolism:SaveLastStatus"] += new Action<Player, string>(SaveLastStatus);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["vorp:firstSpawn"] += new Action<int>(OnFirstSpawn);

            EventHandlers["vorpmetabolism:GetStatus"] += new Action<Player>(GetLastStatus);
            Tick += saveLastStatusTick;
        }

        private void OnFirstSpawn(int source)
        {
            PlayerList pl = new PlayerList();
            Player p = pl[source];

            JObject status = new JObject();
            status.Add("Hunger", LoadConfig.Config["FirstHungerStatus"].ToObject<int>());
            status.Add("Thirst", LoadConfig.Config["FirstThirstStatus"].ToObject<int>());
            status.Add("Metabolism", LoadConfig.Config["FirstMetabolismStatus"].ToObject<int>());

            string sid = "steam:" + p.Identifiers["steam"];

            Exports["ghmattimysql"].execute("UPDATE characters SET status=? WHERE identifier=?", new object[] { status.ToString(), sid });

            p.TriggerEvent("vorpmetabolism:StartFunctions", status.ToString());

            lastPlayerStatus.Add(p, status.ToString());
        }

        private void GetLastStatus([FromSource]Player player)
        {
            int _source = int.Parse(player.Handle);
            TriggerEvent("vorp:getCharacter", _source, new Action<dynamic>((user) =>
            {
                string status = user.status;
                player.TriggerEvent("vorpmetabolism:StartFunctions", status);
                lastPlayerStatus.Add(player, status);
            }));
        }

        private void OnPlayerDropped([FromSource]Player player, string reason)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);
            if (lastPlayerStatus.ContainsKey(player))
            {
                Exports["ghmattimysql"].execute("UPDATE characters SET status=? WHERE identifier=?", new[] { lastPlayerStatus[player], sid });
            }
        }

        private void SaveLastStatus([FromSource]Player player, string status)
        {
            lastPlayerStatus[player] = status;
        }

        [Tick]
        public async Task saveLastStatusTick()
        {
            await Delay(300000);
            foreach (var playerStatus in lastPlayerStatus)
            {
                string sid = ("steam:" + playerStatus.Key.Identifiers["steam"]);
                try
                {
                    Exports["ghmattimysql"].execute("UPDATE characters SET status=? WHERE identifier=?", new[] { playerStatus.Value, sid });
                }
                catch { continue; }
            }
        }
    }
}
