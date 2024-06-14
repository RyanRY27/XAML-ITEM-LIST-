using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Final
{
    public class ItemPageViewModel : BaseViewModel
    {
        private ObservableCollection<object> _filteredItems;
        private ObservableCollection<ItemViewModel> _items;
        private string _filterText;
        private ImageSource _profileImage;
        private bool _isRefreshing;

        public ObservableCollection<ItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ObservableCollection<object> FilteredItems
        {
            get => _filteredItems;
            set
            {
                if (SetProperty(ref _filteredItems, value))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        OnPropertyChanged(nameof(FilteredItems));
                    });
                }
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    FilterItems(_filterText);
                }
            }
        }

        public ImageSource ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand LoadItemsCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand EditItemCommand { get; }
        public ICommand ItemTappedCommand { get; }

        public ItemPageViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            FilteredItems = new ObservableCollection<object>();
            FilterText = string.Empty;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            RefreshCommand = new Command(async () => await RefreshItems());
            DeleteItemCommand = new Command<ItemViewModel>(async (itemViewModel) => await DeleteItem(itemViewModel));
            AddItemCommand = new Command(async () => await ExecuteAddItemCommand());
            EditItemCommand = new Command<ItemViewModel>(async (itemViewModel) => await ExecuteEditItemCommand(itemViewModel));
            ItemTappedCommand = new Command<ItemViewModel>(OnItemTapped);

            LoadItemsCommand.Execute(null);
            ProfileImage = ImageSource.FromFile("profile_placeholder.png");
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await App.Database.GetItemsAsync();

                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(new ItemViewModel
                    {
                        Id = item.Id,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        ItemPrice = item.ItemPrice,
                        ItemImage = ImageSource.FromStream(() => new MemoryStream(item.ItemImage))
                    });
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {Items.Count} items.");
                FilterItems(FilterText);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task RefreshItems()
        {
            IsRefreshing = true;

            await ExecuteLoadItemsCommand();

            IsRefreshing = false;
        }

        public async Task AddItem(ItemViewModel itemViewModel)
        {
            var item = new Item
            {
                Id = itemViewModel.Id,
                ItemName = itemViewModel.ItemName,
                ItemDescription = itemViewModel.ItemDescription,
                ItemPrice = itemViewModel.ItemPrice,
                ItemImage = Final.Extensions.ToByteArray(((StreamImageSource)itemViewModel.ItemImage).Stream.Invoke(new System.Threading.CancellationToken()).Result)
            };

            await App.Database.SaveItemAsync(item);
            await ExecuteLoadItemsCommand();
        }

        public async Task DeleteItem(ItemViewModel itemViewModel)
        {
            var item = new Item
            {
                Id = itemViewModel.Id,
                ItemName = itemViewModel.ItemName,
                ItemDescription = itemViewModel.ItemDescription,
                ItemPrice = itemViewModel.ItemPrice,
                ItemImage = Final.Extensions.ToByteArray(((StreamImageSource)itemViewModel.ItemImage).Stream.Invoke(new System.Threading.CancellationToken()).Result)
            };

            await App.Database.DeleteItemAsync(item);
            Items.Remove(itemViewModel);
            FilterItems(FilterText);
            OnPropertyChanged(nameof(FilteredItems));
        }

        private async Task ExecuteEditItemCommand(ItemViewModel itemViewModel)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ItemEditPage(itemViewModel, this));
        }

        private async Task ExecuteAddItemCommand()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ItemAddPage(this));
        }

        private async void OnItemTapped(ItemViewModel item)
        {
            if (item == null)
                return;

            await Application.Current.MainPage.Navigation.PushModalAsync(new ItemDetailPage(item, this));
        }

        public void FilterItems(string filterText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    var allItems = Items.Cast<object>().ToList();
                    allItems.Add(new object());
                    FilteredItems = new ObservableCollection<object>(allItems);
                    System.Diagnostics.Debug.WriteLine("Filter applied with no text, all items shown.");
                }
                else
                {
                    var filteredList = Items.Where(item =>
                        (item.ItemName?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                        (item.ItemDescription?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                        (item.ItemPrice?.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0).Cast<object>().ToList();

                    filteredList.Add(new object());
                    FilteredItems = new ObservableCollection<object>(filteredList);
                    System.Diagnostics.Debug.WriteLine($"Filter applied, {FilteredItems.Count} items shown.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error filtering items: {ex.Message}");
            }
        }

        public async Task NavigateToItemDetailPage(ItemViewModel itemViewModel)
        {
            if (itemViewModel == null)
                return;

            await Application.Current.MainPage.Navigation.PushModalAsync(new ItemDetailPage(itemViewModel, this));
        }
    }

    public class ItemViewModel : BaseViewModel
    {
        private string _itemName;
        private string _itemDescription;
        private string _itemPrice;
        private ImageSource _itemImage;

        public int Id { get; set; }

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        public string ItemDescription
        {
            get => _itemDescription;
            set => SetProperty(ref _itemDescription, value);
        }

        public string ItemPrice
        {
            get => _itemPrice;
            set => SetProperty(ref _itemPrice, value);
        }

        public ImageSource ItemImage
        {
            get => _itemImage;
            set => SetProperty(ref _itemImage, value);
        }
    }

    public static class Extensions
    {
        public static byte[] ToByteArray(this Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
