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
    public class vorpmetabolism_init : BaseScript
    {
        public static JObject pStatus = new JObject();
        public static bool radarShow = true;

        public static bool loaded = false;
        public static bool metabolismActive = false;

        public vorpmetabolism_init()
        {
            EventHandlers["vorpmetabolism:StartFunctions"] += new Action<string>(StartFunctions);

            EventHandlers["vorp:PlayerForceRespawn"] += new Action(ForceRespawn);
            TriggerServerEvent("vorpmetabolism:GetStatus");
        }

        private void ForceRespawn()
        {
            pStatus["Thirst"] = GetConfig.Config["OnRespawnThirstStatus"].ToObject<int>();
            pStatus["Hunger"] = GetConfig.Config["OnRespawnHungerStatus"].ToObject<int>();
            NUIEvents.UpdateHUD();
        }

        private async void StartFunctions(string status)
        {
            if (status.Length < 2)
            {
                return;
            }

            Debug.WriteLine(status);

            pStatus = JObject.Parse(status);

            await Delay(1000);

            NUIEvents.UpdateHUD();

            Tick += MetabolismTimers;
            Tick += MetabolismUpdaters;
            Tick += MetabolismSaveDB;
            Tick += RadarControlHud;
            Tick += MetabolismSet;
            Tick += MetabolismSet;

            loaded = true;
        }

        [Tick]
        private async Task MetabolismSet()
        {
            if (!loaded || !metabolismActive) { return; }

            await Delay(10000);
            int pPedID = API.PlayerPedId();
            switch (pStatus["Metabolism"].ToObject<int>() / 1000)
            {
                case 10:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[20]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[20]);
                    break;
                case 9:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[19]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[19]);
                    break;
                case 8:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[18]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[18]);
                    break;
                case 7:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[17]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[17]);
                    break;
                case 6:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[16]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[16]);
                    break;
                case 5:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[15]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[15]);
                    break;
                case 4:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[14]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[14]);
                    break;
                case 3:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[13]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[13]);
                    break;
                case 2:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[12]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[12]);
                    break;
                case 1:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[11]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[11]);
                    break;
                case 0:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[10]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[10]);
                    break;
                case -1:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[9]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[9]);
                    break;
                case -2:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[8]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[8]);
                    break;
                case -3:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[7]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[7]);
                    break;
                case -4:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[6]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[6]);
                    break;
                case -5:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[5]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[5]);
                    break;
                case -6:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[4]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[4]);
                    break;
                case -7:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[3]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[3]);
                    break;
                case -8:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[2]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[2]);
                    break;
                case -9:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[1]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[1]);
                    break;
                case -10:
                    Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, (uint)WaistUtils.WAIST_TYPES[0]);
                    SaveNewMetabolism((uint)WaistUtils.WAIST_TYPES[0]);
                    break;
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(300000);
        }
        
        public static void SaveNewMetabolism(uint waist)
        {
            //Next Features
        }

        [Tick]
        private async Task RadarControlHud()
        {
            if (!loaded) { return; }
            await Delay(1000);

            if (API.IsRadarHidden() && radarShow)
            {
                NUIEvents.ShowHUD(false);
                radarShow = false;
            }
            else if (!API.IsRadarHidden() && !radarShow)
            {
                NUIEvents.ShowHUD(true);
                radarShow = true;
            }

            if (API.IsPauseMenuActive() && radarShow)
            {
                NUIEvents.ShowHUD(false);
                radarShow = false;
            }
            else if (!API.IsPauseMenuActive() && !radarShow && !API.IsPlayerDead(API.PlayerId()))
            {
                NUIEvents.ShowHUD(true);
                radarShow = true;
            }
            
        }

        [Tick]
        private async Task MetabolismSaveDB()
        {
            if (!loaded) { return; }
            await Delay(60000); 
            TriggerServerEvent("vorpmetabolism:SaveLastStatus", pStatus.ToString());
        }

        [Tick]
        private async Task MetabolismUpdaters()
        {
            if (!loaded) { return; }

            await Delay(3600);

            if (pStatus["Thirst"].ToObject<int>() > 0 && pStatus["Hunger"].ToObject<int>() > 0 && !API.IsPlayerDead(API.PlayerId()))
            {
                if (API.IsPedRunning(API.PlayerPedId()))
                {
                    pStatus["Thirst"] = pStatus["Thirst"].ToObject<int>() - 3;
                    pStatus["Hunger"] = pStatus["Hunger"].ToObject<int>() - 2;
                }
                else
                {
                    pStatus["Thirst"] = pStatus["Thirst"].ToObject<int>() - 2;
                    pStatus["Hunger"] = pStatus["Hunger"].ToObject<int>() - 1;
                }

                if (pStatus["Thirst"].ToObject<int>() < 0)
                {
                    pStatus["Thirst"] = 0;
                }
                if (pStatus["Thirst"].ToObject<int>() < 0)
                {
                    pStatus["Hunger"] = 0;
                }
            }

            if (pStatus["Metabolism"].ToObject<int>() < 10000 && pStatus["Metabolism"].ToObject<int>() > -10000)
            {
                if (API.IsPedRunning(API.PlayerPedId()))
                {
                    pStatus["Metabolism"] = pStatus["Metabolism"].ToObject<int>() - 4;
                }
                else
                {
                    pStatus["Metabolism"] = pStatus["Metabolism"].ToObject<int>() - 2;
                }
                   
            }

        }

        [Tick]
        private async Task MetabolismTimers()
        {
            if (!loaded) { return; }

            await Delay(6000);

            if (pStatus["Thirst"].ToObject<int>() <= 0 && !API.IsPlayerDead(API.PlayerId()))
            {
                int newHealth = API.GetEntityHealth(API.PlayerPedId()) - 20;
                if (newHealth < 1)
                {
                    Function.Call((Hash)0x697157CED63F18D4, API.PlayerPedId(), 500000, false, true, true);
                }
                API.SetEntityHealth(API.PlayerPedId(), newHealth, 0);
            }
            if (pStatus["Hunger"].ToObject<int>() <= 0 && !API.IsPlayerDead(API.PlayerId()))
            {
                int newHealth = API.GetEntityHealth(API.PlayerPedId()) - 20;
                if (newHealth < 1)
                {
                    Function.Call((Hash)0x697157CED63F18D4, API.PlayerPedId(), 500000, false, true, true);
                }
                API.SetEntityHealth(API.PlayerPedId(), newHealth, 0);
            }

            await NUIEvents.UpdateHUD();
        }

        public uint ConvertValue(string s)
        {
            uint result;

            if (uint.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                int interesante = int.Parse(s);
                result = (uint)interesante;
                return result;
            }
        }
    }
}
