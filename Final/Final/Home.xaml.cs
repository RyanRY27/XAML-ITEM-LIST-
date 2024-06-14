using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Final
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        private readonly ItemPageViewModel _viewModel;

        public Home()
        {
            InitializeComponent();
            _viewModel = new ItemPageViewModel();
            BindingContext = _viewModel;

            
            MessagingCenter.Subscribe<ItemEditPage>(this, "ItemDeleted", async (sender) =>
            {
                System.Diagnostics.Debug.WriteLine("Received ItemDeleted message in Home");

                
                while (Navigation.ModalStack.Count > 0)
                {
                    await Navigation.PopModalAsync();
                }
            });
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.FilterItems(e.NewTextValue);
            }
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.FilterItems(((SearchBar)sender).Text);
            }
        }

        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserDetailsPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null && !_viewModel.Items.Any())
            {
                _viewModel.LoadItemsCommand.Execute(null);
            }
        }

        private async void OnItemTapped(ItemViewModel item)
        {
            if (item == null)
                return;

            await Navigation.PushModalAsync(new ItemDetailPage(item, _viewModel)); 
        }

    }
}
