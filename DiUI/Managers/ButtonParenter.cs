using System;
using Zenject;

namespace DiUI.Managers
{
    public class ButtonParenter : IInitializable, IDisposable
    {
        private readonly Config _config; 

        public ButtonParenter(Config config)
        {
            _config = config;
        }

        public void Initialize()
        {
            _config.Reloaded += Config_Reloaded;
        }

        private void Config_Reloaded(Config config)
        {
            Plugin.Log.Info("The config has reloaded!");
        }

        public void Dispose()
        {
            _config.Reloaded -= Config_Reloaded;
        }
    }
}