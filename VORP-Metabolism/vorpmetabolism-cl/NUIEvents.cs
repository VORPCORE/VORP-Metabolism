using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpmetabolism_cl
{
    class NUIEvents : BaseScript
    {
        public static async Task UpdateHUD()
        {
            JObject msgNUI = new JObject();

            double thirst = vorpmetabolism_init.pStatus["Thirst"].ToObject<double>() / 1000;
            double hunger = vorpmetabolism_init.pStatus["Hunger"].ToObject<double>() / 1000;

            msgNUI.Add("action", "update");
            msgNUI.Add("water", thirst);
            msgNUI.Add("food", hunger);

            API.SendNuiMessage(msgNUI.ToString());
        }

        public static async Task ShowHUD(bool show)
        {
            JObject msgNUI = new JObject();
            if (show)
            {
                msgNUI.Add("action", "show");
            }
            else
            {
                msgNUI.Add("action", "hide");
            }

            API.SendNuiMessage(msgNUI.ToString());
        }
    }
}
