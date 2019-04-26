using System;

namespace ActiveSync.Core.Helper
{
    public static class ConvertHelper
    {
        public static string ToBitString(this bool input)
        {
            var byteValue = Convert.ToByte(input);

            return byteValue.ToString();
        }
    }
}
