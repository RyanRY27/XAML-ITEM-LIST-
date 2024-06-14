using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Final.Services;
using System.Threading.Tasks;
using System;

namespace Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDetailsPage : ContentPage
    {
        private bool isFirstAppearance = true;

        public UserDetailsPage()
        {
            InitializeComponent();
            BindingContext = UserDetailsService.Instance;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (isFirstAppearance)
            {
                ModalFrame.Opacity = 0;
                ModalFrame.Scale = 0.8;
                ModalFrame.TranslationY = 50;

                await Task.WhenAll(
                    OverlayGrid.FadeTo(1, 250),
                    ModalFrame.FadeTo(1, 250),
                    ModalFrame.ScaleTo(1, 250),
                    ModalFrame.TranslateTo(0, 0, 250, Easing.CubicOut)
                );

                isFirstAppearance = false;
            }

            
            await UserDetailsService.Instance.GetUserDetailsAsync();
        }

        private async void OnOverlayTapped(object sender, EventArgs e)
        {
            await Task.WhenAll(
                ModalFrame.ScaleTo(0.8, 250),
                ModalFrame.FadeTo(0, 250),
                OverlayGrid.FadeTo(0, 250),
                ModalFrame.TranslateTo(0, 50, 250, Easing.CubicIn)
            );
            await Navigation.PopModalAsync();
        }

        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ProfileEditPage());
        }
    }
}
