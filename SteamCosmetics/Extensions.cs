using Newtonsoft.Json.Linq;

namespace SteamCosmetics
{
    public static class Extensions
    {
        public static bool IsJson(this string input)
        {
            try
            {
                JContainer.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
