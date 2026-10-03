namespace InfoBoxNET.Core
{
    public static class CommandHelper
    {
        /// <summary>
        /// Prüft, ob ein Objekt einem bestimmten Enum-Wert entspricht und gibt den gecasteten Wert optional aus.
        /// </summary>
        public static bool IsCommand<TEnum>(object commandParam, TEnum expectedValue, out TEnum typedButton) where TEnum : struct, Enum
        {
            if (commandParam is TEnum actualValue && actualValue.Equals(expectedValue))
            {
                typedButton = actualValue;
                return true;
            }

            typedButton = default;
            return false;
        }
    }
}
