using CitizenFX.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpmetabolism_cl
{
    public class vorpmetabolism_init : BaseScript
    {
        public static JObject pStatus = new JObject();

        public vorpmetabolism_init()
        {
            EventHandlers["vorpmetabolism:StartFunctions"] += new Action<string>(StartFunctions);
            TriggerServerEvent("vorpmetabolism:GetStatus");
        }

        private async void StartFunctions(string status)
        {
            pStatus = JObject.Parse(status);

            await Delay(1000);

            NUIEvents.UpdateHUD();
        }
    }
}
