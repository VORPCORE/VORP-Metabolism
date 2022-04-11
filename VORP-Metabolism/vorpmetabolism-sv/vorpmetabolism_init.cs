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
        public static dynamic CORE;

        PlayerList _players;

        public vorpmetabolism_init()
        {
            _players = Players;

            EventHandlers["vorpmetabolism:SaveLastStatus"] += new Action<Player, string>(SaveLastStatus);
            EventHandlers["vorpmetabolism:GetStatus"] += new Action<Player>(GetLastStatus);

            EventHandlers.Add("onResourceStart", new Action<string>(resourceName =>
            {
                if (resourceName == "vorp_inventory")
                    RegisterUsableItemsAsync();
            }));

            TriggerEvent("getCore", new Action<dynamic>((dic) =>
            {
                CORE = dic;
            }));

            RegisterUsableItemsAsync();
        }

        public async Task RegisterUsableItemsAsync()
        {
            await Delay(3000);
            Debug.WriteLine($"Metabolism: Loading {LoadConfig.Config["ItemsToUse"].Count().ToString()} items usables ");
            for (int i = 0; i < LoadConfig.Config["ItemsToUse"].Count(); i++)
            {
                int index = i;
                TriggerEvent("vorpCore:registerUsableItem", LoadConfig.Config["ItemsToUse"][i]["Name"].ToString(), new Action<dynamic>((data) =>
                {
                    Player p = _players[data.source];
                    string itemLabel = data.item.label;
                    p.TriggerEvent("vorpmetabolism:useItem", index, itemLabel);
                    TriggerEvent("vorpCore:subItem", data.source, LoadConfig.Config["ItemsToUse"][index]["Name"].ToString(), 1);
                }));
                
            }
        }

        private void GetLastStatus([FromSource]Player player)
        {
            int _source = int.Parse(player.Handle);
            dynamic UserCharacter = CORE.getUser(int.Parse(player.Handle)).getUsedCharacter;
            string s_status = UserCharacter.status;

            if (s_status.Length > 5)
            {
                player.TriggerEvent("vorpmetabolism:StartFunctions", s_status);
            }
            else
            {
                JObject status = new JObject();
                status.Add("Hunger", LoadConfig.Config["FirstHungerStatus"].ToObject<int>());
                status.Add("Thirst", LoadConfig.Config["FirstThirstStatus"].ToObject<int>());
                status.Add("Metabolism", LoadConfig.Config["FirstMetabolismStatus"].ToObject<int>());

                UserCharacter.setStatus(status.ToString());
        
                player.TriggerEvent("vorpmetabolism:StartFunctions", status.ToString());
            }

            
        }

        private void SaveLastStatus([FromSource]Player player, string status)
        {
            dynamic UserCharacter = CORE.getUser(int.Parse(player.Handle)).getUsedCharacter;
            UserCharacter.setStatus(status);
        }

    }
}
