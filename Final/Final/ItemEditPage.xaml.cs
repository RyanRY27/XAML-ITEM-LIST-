using System;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemEditPage : ContentPage
    {
        private ItemViewModel _itemViewModel;
        private ItemPageViewModel _itemPageViewModel;

        
        private string _originalName;
        private string _originalDescription;
        private string _originalPrice;
        private ImageSource _originalImage;

        public ItemEditPage(ItemViewModel itemViewModel, ItemPageViewModel itemPageViewModel)
        {
            InitializeComponent();
            _itemViewModel = itemViewModel;
            _itemPageViewModel = itemPageViewModel;

            // Store original values
            _originalName = itemViewModel.ItemName;
            _originalDescription = itemViewModel.ItemDescription;
            _originalPrice = itemViewModel.ItemPrice;
            _originalImage = itemViewModel.ItemImage;

            BindingContext = _itemViewModel;

            itemNameEntry.Text = _itemViewModel.ItemName;
            itemDescriptionEntry.Text = _itemViewModel.ItemDescription;
            itemPriceEntry.Text = _itemViewModel.ItemPrice;
            itemImage.Source = _itemViewModel.ItemImage;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _itemViewModel.ItemName = itemNameEntry.Text;
            _itemViewModel.ItemDescription = itemDescriptionEntry.Text;
            _itemViewModel.ItemPrice = itemPriceEntry.Text;

            await _itemPageViewModel?.AddItem(_itemViewModel);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            RevertChanges();
            await Navigation.PopModalAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Delete button clicked in ItemEditPage");
            await _itemPageViewModel?.DeleteItem(_itemViewModel);

            
            MessagingCenter.Send(this, "ItemDeleted");

            
            await Navigation.PopModalAsync();
        }

        private async void OnOverlayTapped(object sender, EventArgs e)
        {
            RevertChanges();
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

                    
                    _itemViewModel.ItemImage = ImageSource.FromStream(() => new MemoryStream(imageData));
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

        private void RevertChanges()
        {
            _itemViewModel.ItemName = _originalName;
            _itemViewModel.ItemDescription = _originalDescription;
            _itemViewModel.ItemPrice = _originalPrice;
            _itemViewModel.ItemImage = _originalImage;
        }
    }
}
