using FeeSystem.Debugging;

namespace FeeSystem
{
    public class FeeSystemConsts
    {
        public const string LocalizationSourceName = "FeeSystem";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "691a7d6262194686a3091a7b4edab617";
    }
}
