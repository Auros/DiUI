using DiUI.UI;
using Zenject;
using SiraUtil;
using DiUI.Managers;

namespace DiUI.Installers
{
    internal class DiUIInstaller : Installer<Config, DiUIInstaller>
    {
        private readonly Config _config;

        public DiUIInstaller(Config config) => _config = config;

        public override void InstallBindings()
        {
            Container.BindInstance(_config).AsSingle();
            Container.Bind<MenuChildManager>().AsSingle();
            Container.BindInterfacesTo<MenuButtonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<EditModeManager>().AsSingle();

            Container.BindViewController<DiUIChildView>();
            Container.BindViewController<DiUIManagerView>();
            Container.BindViewController<DiUIEditorController>();
            Container.BindViewController<DiUIInstructionsView>();
            Container.BindFlowCoordinator<DiUIFlowCoordinator>();
        }
    }
}