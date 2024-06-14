using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Final.Services;

namespace Final
{
    public partial class ProfileEditPage : ContentPage
    {
        private string originalUsername;
        private string originalEmail;
        private string originalPhone;
        private ImageSource originalProfileImage;
        private string originalProfileImagePath;
        private string newProfileImagePath;

        public ProfileEditPage()
        {
            InitializeComponent();
            BindingContext = UserDetailsService.Instance;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var userDetails = UserDetailsService.Instance.GetUserDetails();
            if (userDetails != null)
            {
                originalUsername = userDetails.Username;
                originalEmail = userDetails.Email;
                originalPhone = userDetails.Phone;
                originalProfileImage = UserDetailsService.Instance.ProfileImage;
                originalProfileImagePath = userDetails.ProfileImagePath;
            }

            OverlayGrid.Opacity = 1;
            ModalFrame.Opacity = 1;
            ModalFrame.Scale = 1;
            ModalFrame.TranslationY = 0;
        }

        private async void OnOverlayTapped(object sender, EventArgs e)
        {
            ResetEntries();
            await Navigation.PopModalAsync();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            await SaveDetails();
            UserDetailsService.Instance.ProfileImage = ImageSource.FromFile(newProfileImagePath ?? originalProfileImagePath);
            await Navigation.PopModalAsync();
        }

        private async Task SaveDetails()
        {
            var userService = UserDetailsService.Instance;
            var userDetails = userService.GetUserDetails();
            userDetails.Username = usernameEntry.Text;
            userDetails.Email = emailEntry.Text;
            userDetails.Phone = phoneEntry.Text;
            userDetails.ProfileImagePath = newProfileImagePath ?? originalProfileImagePath; 
            await userService.SaveUserDetailsAsync(userDetails);
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            ResetEntries();
            await Navigation.PopModalAsync();
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    var newFilePath = await SaveProfileImageAsync(result.FullPath);
                    newProfileImagePath = newFilePath; 
                    uploadedImage.Source = ImageSource.FromFile(newFilePath); 
                }
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "Permissions not granted.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task<string> SaveProfileImageAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            var destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.GetFileName(filePath));
            File.Copy(filePath, destinationPath, true);
            return destinationPath;
        }

        private void ResetEntries()
        {
            usernameEntry.Text = originalUsername;
            emailEntry.Text = originalEmail;
            phoneEntry.Text = originalPhone;
            uploadedImage.Source = originalProfileImage;
            newProfileImagePath = null; 
            UserDetailsService.Instance.ProfileImage = originalProfileImage; 
        }
    }
}
