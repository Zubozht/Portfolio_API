using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.HelperFunctions
{
    public class ShutterSpeedNormalizers
    {
        public static string ShutterSpeedNormalizer (float shutterspeedvalue)
                    {
                        float exposuretime = (float)Math.Pow(2, -shutterspeedvalue);

                        if (exposuretime >= 1)
                        {
                            return exposuretime.ToString("0.#");
                        }
                        else
                        {
                            return $"1/{(int)Math.Round(1/exposuretime)}";
                        }
                    }

        public static float ShutterspeedToFloat(string sh)
        {
            if (float.TryParse(sh, out float intsh))
            {
                return intsh;
            }
            else if (sh.Contains('/') && (sh.Count(c => c == '/') == 1) && !sh.StartsWith('/') && !sh.EndsWith('/'))
            {
                if (float.TryParse(sh.Substring(0, sh.IndexOf('/')), out float dividend) && float.TryParse(sh.Substring(sh.IndexOf('/') + 1), out float divisor))
                {
                    return dividend / divisor;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}