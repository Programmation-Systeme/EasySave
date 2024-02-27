namespace EasySaveClasses.ViewModelNS
{
    public sealed class LogManager
    {
        private static readonly Lazy<LogManager> instance = new Lazy<LogManager>(() => new LogManager());

        public static LogManager Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private ILog logStrategy;

        private LogManager()
        {
            // Initialisation privée pour empêcher l'instanciation externe.
            // Ici, vous pouvez initialiser avec une stratégie de log par défaut, par exemple JsonLog.
            logStrategy = new JsonLog(); // Ou new XmlLog(), selon la configuration ou la préférence par défaut.
        }

        public string LogStrategyType
        {
            set
            {
                string a = value.ToLower();   
                Console.WriteLine($"Setting log strategy to: {value.ToLower()}");
                switch (value.ToLower())
                {
                    case "json":
                        logStrategy = new JsonLog();
                        break;
                    case "xml":
                        logStrategy = new XmlLog();
                        break;
                    default:
                        throw new ArgumentException("Unsupported log strategy type");
                }
            }
        }

        public string AddLog(string sourcePath, string targetPath, float transferTime)
        {
            return logStrategy.AddLog(sourcePath, targetPath, transferTime);
        }
    }
}