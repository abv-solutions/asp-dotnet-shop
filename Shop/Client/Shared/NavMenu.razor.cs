using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shop.Client.Services;

namespace Shop.Client.Shared
{
    public partial class NavMenu : IDisposable
    {
        [Inject]
        private State state { get; set; }

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private bool collapseNavMenu = true;

        protected override void OnInitialized()
        {
            state.OnChange += StateHasChanged;

        }
        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        public void Dispose()
        {
            state.OnChange -= StateHasChanged;
        }
    }
}
