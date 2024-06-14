using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        private ItemViewModel _itemViewModel;
        private ItemPageViewModel _itemPageViewModel;

        public ItemDetailPage(ItemViewModel itemViewModel, ItemPageViewModel itemPageViewModel)
        {
            InitializeComponent();
            _itemViewModel = itemViewModel;
            _itemPageViewModel = itemPageViewModel;
            BindingContext = _itemViewModel;

           
            MessagingCenter.Subscribe<ItemEditPage>(this, "ItemDeleted", async (sender) =>
            {
                System.Diagnostics.Debug.WriteLine("Received ItemDeleted message in ItemDetailPage");
                await ClosePage();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            System.Diagnostics.Debug.WriteLine("Unsubscribing from ItemDeleted message");
            MessagingCenter.Unsubscribe<ItemEditPage>(this, "ItemDeleted");
        }

        private async Task ClosePage()
        {
            
            System.Diagnostics.Debug.WriteLine("Closing ItemDetailPage");
            await Navigation.PopModalAsync();
        }

        private async void OnOverlayTapped(object sender, EventArgs e)
        {
            await ClosePage();
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ItemEditPage(_itemViewModel, _itemPageViewModel));
        }
    }
}
