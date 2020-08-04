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
    class ApiCalls : BaseScript
    {
        public ApiCalls()
        {
            EventHandlers["vorpmetabolism:changeValue"] += new Action<string, int>(changeValue);
            EventHandlers["vorpmetabolism:setValue"] += new Action<string, int>(setValue);
            EventHandlers["vorpmetabolism:getValue"] += new Action<string, dynamic>(getValue);

            EventHandlers["vorpmetabolism:setHud"] += new Action<bool>(setHud);
        }

        public static bool APIShowOn = true;

        private void setHud(bool enable)
        {
            APIShowOn = enable;
        }

        private void getValue(string key, dynamic cb)
        {
            string newKey = key.First().ToString().ToUpper() + key.Substring(1); //Fixed first char to upper case

            if (vorpmetabolism_init.pStatus.ContainsKey(newKey))
            {
                cb.Invoke(vorpmetabolism_init.pStatus[newKey].ToObject<int>());
            }
            else
            {
                cb.Invoke(null);
            }

        }

        private void changeValue(string key, int value)
        {
            string newKey = key.First().ToString().ToUpper() + key.Substring(1); //Fixed first char to upper case
            if (vorpmetabolism_init.pStatus.ContainsKey(newKey))
            {
                int newValue = vorpmetabolism_init.pStatus[newKey].ToObject<int>() + value;
                if (newKey.Equals("Metabolism"))
                {
                    if (newValue > 10000)
                    {
                        newValue = 10000;
                    }
                    else if (newValue < -10000)
                    {
                        newValue = -10000;
                    }
                }
                else
                {
                    if (newValue > 1000)
                    {
                        newValue = 1000;
                    }
                    else if (newValue < 0)
                    {
                        newValue = 0;
                    }
                }

                vorpmetabolism_init.pStatus[newKey] = newValue;

            }
        }

        private void setValue(string key, int value)
        {
            string newKey = key.First().ToString().ToUpper() + key.Substring(1); //Fixed first char to upper case
            if (vorpmetabolism_init.pStatus.ContainsKey(newKey))
            {
                int newValue = value;
                if (newKey.Equals("Metabolism"))
                {
                    if (newValue > 10000)
                    {
                        newValue = 10000;
                    }
                    else if (newValue < -10000)
                    {
                        newValue = -10000;
                    }
                }
                else
                {
                    if (newValue > 1000)
                    {
                        newValue = 1000;
                    }
                    else if (newValue < -1000)
                    {
                        newValue = -1000;
                    }
                }

                vorpmetabolism_init.pStatus[newKey] = newValue;

            }
        }
    }
}
