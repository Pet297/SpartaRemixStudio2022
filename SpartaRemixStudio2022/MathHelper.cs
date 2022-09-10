using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaRemixStudio2022
{
    public static class MathHelper
    {
        public static float LinearMap(float srcFrom, float srcTo, float src, float dstFrom, float dstTo)
        {
            float p = (src - srcFrom) / (srcTo - srcFrom);
            return (1 - p) * dstFrom + p * dstTo;
        }
    }
}
