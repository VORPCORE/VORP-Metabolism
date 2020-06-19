using CitizenFX.Core;
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

            Tick += saveLastStatusTick;
        }

        private void OnPlayerDropped(Player player, string reason)
        {
            string sid = ("steam:" + player.Identifiers["steam"]);
            if (lastPlayerStatus.ContainsKey(player))
            {
                Exports["ghmattimysql"].execute("UPDATE characters SET status=? WHERE identifier=?", new[] { lastPlayerStatus[player], sid });
            }
        }

        private void SaveLastStatus(Player player, string status)
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
