using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace DiUI.UI
{
    [ViewDefinition("DiUI.Views.editor-controller.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\editor-controller.bsml")]
    internal class DiUIEditorController : BSMLAutomaticViewController
    {
        internal event Action ExitEditRequested;

        [UIAction("exit-edit-mode")]
        protected void ExitEditMode()
        {
            ExitEditRequested?.Invoke();
        }
    }
}