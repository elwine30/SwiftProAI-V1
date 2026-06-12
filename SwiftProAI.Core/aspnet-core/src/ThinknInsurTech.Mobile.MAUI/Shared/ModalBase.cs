using ThinknInsurTech.Core.Dependency;
using ThinknInsurTech.Mobile.MAUI.Services.UI;

namespace ThinknInsurTech.Mobile.MAUI.Shared
{
    public abstract class ModalBase : ThinknInsurTechComponentBase
    {
        protected ModalManagerService ModalManager { get; set; }

        public abstract string ModalId { get; }

        public ModalBase()
        {
            ModalManager = DependencyResolver.Resolve<ModalManagerService>();
        }

        public virtual async Task Show()
        {
            await ModalManager.Show(JS, ModalId);
            StateHasChanged();
        }

        public virtual async Task Hide()
        {
            await ModalManager.Hide(JS, ModalId);
        }
    }
}
