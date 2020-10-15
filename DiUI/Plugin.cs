using DiUI.Installers;
using IPA;
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
            conf.Generated<Config>();
            zenjector.OnMenu<DiUIInstaller>();
        }

        [OnEnable, OnDisable]
        public void OnState()
        {

        }
    }
}