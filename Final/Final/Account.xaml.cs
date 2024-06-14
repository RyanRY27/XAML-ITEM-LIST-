using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Final.Services;
using System;

namespace Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        public Account()
        {
            InitializeComponent();
            BindingContext = UserDetailsService.Instance;
        }

        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ProfileEditPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UserDetailsService.Instance.GetUserDetailsAsync();
        }
    }
}
