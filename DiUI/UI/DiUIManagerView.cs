using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace DiUI.UI
{
    [ViewDefinition("DiUI.Views.manager-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\manager-view.bsml")]
    internal class DiUIManagerView : BSMLAutomaticViewController
    {
        internal event Action EditModeRequested;

        [UIAction("enter-edit-mode")]
        protected void EnterEditMode()
        {
            EditModeRequested?.Invoke();
        }
    }
}