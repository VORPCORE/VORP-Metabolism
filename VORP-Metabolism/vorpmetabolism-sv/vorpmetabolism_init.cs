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
            RegisterUsableItems();
        }

        public async Task RegisterUsableItems()
        {
            await Delay(4000);
            Debug.WriteLine($"Metabolism: Loading {LoadConfig.Config["ItemsToUse"].Count().ToString()} items usables ");
            for (int i = 0; i < LoadConfig.Config["ItemsToUse"].Count(); i++)
            {
                await Delay(200);
                int index = i;
                TriggerEvent("vorpCore:registerUsableItem", LoadConfig.Config["ItemsToUse"][i]["Name"].ToString(), new Action<dynamic>((data) =>
                {
                    PlayerList pl = new PlayerList();
                    Player p = pl[data.source];
                    string itemLabel = data.item.label;
                    p.TriggerEvent("vorpmetabolism:useItem", index, itemLabel);
                    TriggerEvent("vorpCore:subItem", data.source, LoadConfig.Config["ItemsToUse"][index]["Name"].ToString(), 1);
                }));
                
            }
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
                lastPlayerStatus.Remove(player);
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
