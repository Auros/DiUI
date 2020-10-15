using IPA;
using DiUI.Installers;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using Conf = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace DiUI
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(Conf conf, IPALogger logger, Zenjector zenjector)
        {
            Log = logger;
            var config = conf.Generated<Config>();
            zenjector.OnMenu<DiUIInstaller>().WithParameters(config);
        }

        [OnEnable, OnDisable]
        public void OnState()
        {

        }
    }
}