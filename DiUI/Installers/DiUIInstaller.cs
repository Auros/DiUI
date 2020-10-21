using DiUI.UI;
using Zenject;
using SiraUtil;
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
            Container.BindInterfacesTo<MenuButtonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<EditModeManager>().AsSingle();

            Container.BindViewController<DiUIManagerView>();
            Container.BindViewController<DiUIEditorController>();
            Container.BindFlowCoordinator<DiUIFlowCoordinator>();
        }
    }
}