
using System;

public static class RandomLong
{
    public static ulong RandomULong(ulong min, ulong max)
    {
        Random rand = new Random();
        var hight = rand.Next((int)(min >> 32), (int)(max >> 32));
        var minLow = Math.Min((int)min, (int)max);
        var maxLow = Math.Max((int)min, (int)max);
        var low = (uint)rand.Next(minLow, maxLow);
        ulong result = (ulong)hight;
        result <<= 32;
        result |= (ulong)low;
        return result;
    }
}
