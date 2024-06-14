using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Final
{
    public partial class ItemAddPage : ContentPage
    {
        private readonly ItemPageViewModel _itemPageViewModel;

        public ItemAddPage(ItemPageViewModel itemPageViewModel)
        {
            InitializeComponent();
            _itemPageViewModel = itemPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void OnOverlayTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var itemViewModel = new ItemViewModel
            {
                ItemName = itemNameEntry.Text,
                ItemDescription = itemDescriptionEntry.Text,
                ItemPrice = itemPriceEntry.Text,
                ItemImage = itemImage.Source
            };

            await _itemPageViewModel.AddItem(itemViewModel);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var imageData = new byte[stream.Length];
                    await stream.ReadAsync(imageData, 0, (int)stream.Length);
                    itemImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
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
    }
}
