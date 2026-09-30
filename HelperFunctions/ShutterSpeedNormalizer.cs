using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.HelperFunctions
{
    public class ShutterSpeedNormalizer
    {
        public static string shutterspeednormalizer (float shutterspeedvalue)
                    {
                        float exposuretime = (float)Math.Pow(2, -shutterspeedvalue);

                        if (exposuretime >= 1)
                        {
                            return exposuretime.ToString("0.#");
                        }
                        else
                        {
                            //check
                            return $"1/{(int)Math.Round(1/exposuretime)}";
                        }
                    }
    }
}