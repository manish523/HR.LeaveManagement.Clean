using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Providers;
using HR.LeaveManagement.BlazorUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace HR.LeaveManagement.BlazorUI.Pages
{
    public partial class Index
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ((ApiAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticationStateAsync();
        }
        protected void GoToLogin()
        {
            NavigationManager.NavigateTo("login/");
        }

        protected void GoToRegister()
        {
            NavigationManager.NavigateTo("register/");
        }
        protected async void Logout()
        {
            await AuthenticationService.Logout();
        }
    }
}