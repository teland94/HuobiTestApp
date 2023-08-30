using Microsoft.AspNetCore.Components;

namespace HuobiTestApp.Base
{
    public abstract class RefreshablePageBase : ComponentBase
    {
        public static RefreshablePageBase Current;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected RefreshablePageBase()
        {
            Current = this;
        }
    }
}
