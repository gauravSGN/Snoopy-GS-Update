using System;

namespace Util
{
    public static class DateTimeUtil
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public static long GetUnixTime()
        {
            return (long)(DateTime.UtcNow.Subtract(Epoch)).TotalSeconds;
        }
    }
}