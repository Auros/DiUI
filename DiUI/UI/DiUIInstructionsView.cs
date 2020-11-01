using Zenject;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace DiUI.UI
{
    [ViewDefinition("DiUI.Views.instructions-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\instructions-view.bsml")]
    internal class DiUIInstructionsView : BSMLAutomaticViewController
    {
        private Config _config;

        [Inject]
        public void Construct(Config config)
        {
            _config = config;
        }

        [UIValue("editor-sensitivity")]
        protected float EditorSensitivity
        {
            get => _config.EditorMoveSensitivity;
            set
            {
                _config.EditorMoveSensitivity = value;
                NotifyPropertyChanged();
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            EditorSensitivity = _config.EditorMoveSensitivity;
        }
    }
}