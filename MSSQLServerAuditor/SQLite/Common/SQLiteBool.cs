namespace MSSQLServerAuditor.SQLite.Common
{
    using System;

    public class SqLiteBool
    {
        public const int True = 1;
        public const int False = 0;

        public static bool FromString(string value)
        {
            return FromBit(int.Parse(value));
        }

        public static bool FromBit(int value)
        {
            switch (value)
            {
                case True:
                    return true;
                case False:
                    return false;
                default:
                    throw new ArgumentException(value + " is a bit value (0 or 1)");
            }
        }

        public static int ToBit(bool value)
        {
            return value ? True : False;
        }
    }
}