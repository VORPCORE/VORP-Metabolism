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
    class UseItemsActions : BaseScript
    {
        public UseItemsActions()
        {
            EventHandlers["vorpmetabolism:useItem"] += new Action<int, string>(ConsumeItems);
        }

        private void ConsumeItems(int index, string label)
        {
            API.PlaySoundFrontend("Core_Fill_Up", "Consumption_Sounds", true, 0);

            if (GetConfig.Config["ItemsToUse"][index]["Thirst"].ToObject<int>() != 0)
            {
                int newThirst = vorpmetabolism_init.pStatus["Thirst"].ToObject<int>() + GetConfig.Config["ItemsToUse"][index]["Thirst"].ToObject<int>();

                if (newThirst > 1000)
                    newThirst = 1000;

                if (newThirst < 0)
                    newThirst = 0;

                vorpmetabolism_init.pStatus["Thirst"] = newThirst;
            }
            if (GetConfig.Config["ItemsToUse"][index]["Hunger"].ToObject<int>() != 0)
            {
                int newHunger = vorpmetabolism_init.pStatus["Hunger"].ToObject<int>() + GetConfig.Config["ItemsToUse"][index]["Hunger"].ToObject<int>();

                if (newHunger > 1000)
                    newHunger = 1000;

                if (newHunger < 0)
                    newHunger = 0;

                vorpmetabolism_init.pStatus["Hunger"] = newHunger;
            }
            if (GetConfig.Config["ItemsToUse"][index]["Metabolism"].ToObject<int>() != 0)
            {
                int newMetabolism = vorpmetabolism_init.pStatus["Metabolism"].ToObject<int>() + GetConfig.Config["ItemsToUse"][index]["Metabolism"].ToObject<int>();

                if (newMetabolism > 10000)
                    newMetabolism = 10000;

                if (newMetabolism < -10000)
                    newMetabolism = -10000;

                vorpmetabolism_init.pStatus["Metabolism"] = newMetabolism;
            }
            if (GetConfig.Config["ItemsToUse"][index]["Stamina"].ToObject<int>() != 0)
            {
                int stamina = Function.Call<int>((Hash)0x36731AC041289BB1, API.PlayerPedId(), 1);
                int newStamina = stamina + GetConfig.Config["ItemsToUse"][index]["Stamina"].ToObject<int>();

                if (newStamina > 100)
                    newStamina = 100;

                Function.Call((Hash)0xC6258F41D86676E0, API.PlayerPedId(), 1, newStamina);
            }
            if (GetConfig.Config["ItemsToUse"][index]["InnerCoreHealth"].ToObject<int>() != 0)
            {
                int health = Function.Call<int>((Hash)0x36731AC041289BB1, API.PlayerPedId(), 0);
                int newhealth = health + GetConfig.Config["ItemsToUse"][index]["InnerCoreHealth"].ToObject<int>();

                if (newhealth > 100)
                    newhealth = 100;

                Function.Call((Hash)0xC6258F41D86676E0, API.PlayerPedId(), 1, newhealth);
            }
            if (GetConfig.Config["ItemsToUse"][index]["OuterCoreHealth"].ToObject<int>() != 0)
            {
                int health = Function.Call<int>((Hash)0x82368787EA73C0F7, API.PlayerPedId(), 0);
                int newhealth = health + GetConfig.Config["ItemsToUse"][index]["OuterCoreHealth"].ToObject<int>();

                if (newhealth > 100)
                    newhealth = 100;

                Function.Call((Hash)0xAC2767ED8BDFAB15, API.PlayerPedId(), newhealth, 0);
            }
            //Golds
            if (GetConfig.Config["ItemsToUse"][index]["OuterCoreHealthGold"].ToObject<float>() != 0f)
            {
                Function.Call((Hash)0xF6A7C08DF2E28B28, API.PlayerPedId(), 0, GetConfig.Config["ItemsToUse"][index]["OuterCoreHealthGold"].ToObject<float>(), true);
            }
            if (GetConfig.Config["ItemsToUse"][index]["InnerCoreHealthGold"].ToObject<float>() != 0f)
            {
                Function.Call((Hash)0xF6A7C08DF2E28B28, API.PlayerPedId(), 0, GetConfig.Config["ItemsToUse"][index]["InnerCoreHealthGold"].ToObject<float>(), true);
            }

            if (GetConfig.Config["ItemsToUse"][index]["OuterCoreStaminaGold"].ToObject<float>() != 0f)
            {
                Function.Call((Hash)0xF6A7C08DF2E28B28, API.PlayerPedId(), 1, GetConfig.Config["ItemsToUse"][index]["OuterCoreStaminaGold"].ToObject<float>(), true);
            }
            if (GetConfig.Config["ItemsToUse"][index]["InnerCoreStaminaGold"].ToObject<float>() != 0f)
            {
                Function.Call((Hash)0xF6A7C08DF2E28B28, API.PlayerPedId(), 1, GetConfig.Config["ItemsToUse"][index]["InnerCoreStaminaGold"].ToObject<float>(), true);
            }

            if (GetConfig.Config["ItemsToUse"][index]["Animation"].ToString().ToLower().Contains("eat"))
            {
                PlayAnimEat(GetConfig.Config["ItemsToUse"][index]["PropName"].ToString().ToLower());
            }
            else
            {
                PlayAnimDrink(GetConfig.Config["ItemsToUse"][index]["PropName"].ToString().ToLower());
            }

            TriggerEvent("vorp:Tip", string.Format(GetConfig.Langs["OnUseItem"], label), 3000);
        }

        public async Task PlayAnimDrink(string propName)
        {
            Vector3 playerCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            string dict = "amb_rest_drunk@world_human_drinking@male_a@idle_a";
            string anim = "idle_a";

            if (!API.IsPedMale(API.PlayerPedId()))
            {
                dict = "amb_rest_drunk@world_human_drinking@female_a@idle_b";
                anim = "idle_b";
            }
            
            API.RequestAnimDict(dict);
            while (!API.HasAnimDictLoaded(dict))
            {
                await Delay(100);
            }

            uint hashItem = (uint)API.GetHashKey(propName);

            int prop = API.CreateObject(hashItem, playerCoords.X, playerCoords.Y, playerCoords.Z + 0.2f, true, true, false, false, true);
            int boneIndex = API.GetEntityBoneIndexByName(API.PlayerPedId(), "SKEL_R_Finger12");

            await Delay(1000);

            Function.Call(Hash.TASK_PLAY_ANIM, API.PlayerPedId(), dict, anim, 1.0f, 8.0f, 5000, 31, 0.0f, false, false, false);
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, API.PlayerPedId(), boneIndex, 0.02f, 0.028f, 0.001f, 15.0f, 175.0f, 0.0f, true, true, false, true, 1, true);
            await Delay(6000);

            API.DeleteObject(ref prop);
            API.ClearPedSecondaryTask(API.PlayerPedId());
        }

        public async Task PlayAnimEat(string propName)
        {
            Vector3 playerCoords = API.GetEntityCoords(API.PlayerPedId(), true, true);
            string dict = "mech_inventory@clothing@bandana";
            string anim = "NECK_2_FACE_RH";

            //if (!API.IsPedMale(API.PlayerPedId()))
            //{
            //    dict = "amb_rest_drunk@world_human_drinking@female_a@idle_b";
            //    anim = "idle_b";
            //}

            API.RequestAnimDict(dict);
            while (!API.HasAnimDictLoaded(dict))
            {
                await Delay(100);
            }

            uint hashItem = (uint)API.GetHashKey(propName);

            int prop = API.CreateObject(hashItem, playerCoords.X, playerCoords.Y, playerCoords.Z + 0.2f, true, true, false, false, true);
            int boneIndex = API.GetEntityBoneIndexByName(API.PlayerPedId(), "SKEL_R_Finger12");

            await Delay(1000);

            Function.Call(Hash.TASK_PLAY_ANIM, API.PlayerPedId(), dict, anim, 1.0f, 8.0f, 5000, 31, 0.0f, false, false, false);
            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, API.PlayerPedId(), boneIndex, 0.02f, 0.028f, 0.001f, 15.0f, 175.0f, 0.0f, true, true, false, true, 1, true);
            await Delay(6000);

            API.DeleteObject(ref prop);
            API.ClearPedSecondaryTask(API.PlayerPedId());
        }
    }
}
