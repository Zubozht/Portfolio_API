using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class ShutterspeedToFloat
    {
        public static float shutterspeedtofloat (string sh)
                    {
                        if (float.TryParse(sh, out float intsh))
                        {
                            return intsh;
                        }
                        else if (sh.Contains('/') && (sh.Count(c => c == '/') == 1) && !sh.StartsWith('/') && !sh.EndsWith('/'))
                        {
                            if (float.TryParse(sh.Substring(0,sh.IndexOf('/')), out float dividend) && float.TryParse(sh.Substring(sh.IndexOf('/')+1), out float divisor))
                            {
                                return dividend/divisor;
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