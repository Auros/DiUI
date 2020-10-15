using Zenject;
using DiUI.Managers;

namespace DiUI.Installers
{
    public class DiUIInstaller : Installer<Config, DiUIInstaller>
    {
        private readonly Config _config;

        public DiUIInstaller(Config config) => _config = config;

        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();
            Container.BindInterfacesAndSelfTo<ButtonParenter>().AsSingle();
        }
    }
}