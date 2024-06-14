using System;
using Xamarin.Forms;

namespace Final
{
    public class ItemEditPageViewModel : BaseViewModel
    {
        private readonly ItemViewModel _item;
        private readonly ItemPageViewModel _viewModel;

        public string ItemName
        {
            get => _item.ItemName;
            set
            {
                _item.ItemName = value;
                OnPropertyChanged();
            }
        }

        public string ItemDescription
        {
            get => _item.ItemDescription;
            set
            {
                _item.ItemDescription = value;
                OnPropertyChanged();
            }
        }

        public string ItemPrice
        {
            get => _item.ItemPrice;
            set
            {
                _item.ItemPrice = value;
                OnPropertyChanged();
            }
        }

        public ImageSource ItemImage
        {
            get => _item.ItemImage;
            set
            {
                _item.ItemImage = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public ItemEditPageViewModel(ItemViewModel item, ItemPageViewModel viewModel)
        {
            _item = item;
            _viewModel = viewModel;

            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnSave()
        {
            await _viewModel.AddItem(_item);
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void OnCancel()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
