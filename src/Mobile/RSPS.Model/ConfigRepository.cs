namespace RSPS.Model
{
    public class ConfigRepository
    {
        private const string ConfigFileName = @"config.json";
        private readonly ILocalStorage _localStorage;

        public ConfigRepository(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public Config GetConfig()
        {
            var localFile = _localStorage.LoadFile(ConfigFileName);
            if (localFile == null) return new Config() {LoginAttempts = 0, Name = "Jan Banan"};

            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(localFile);
            return config;
        }

        public void SaveConfig(Config config)
        {
            var serializeObject = Newtonsoft.Json.JsonConvert.SerializeObject(config);
            _localStorage.StoreFile(ConfigFileName, serializeObject);
        }

    }
}