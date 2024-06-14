using System;
using System.Linq;
using Xamarin.Forms;

namespace Final
{
    public partial class ItemPage : ContentPage
    {
        private readonly ItemPageViewModel _viewModel;

        public ItemPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemPageViewModel();
        }

        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ItemAddPage(_viewModel));
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.FilterItems(e.NewTextValue);
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            _viewModel.FilterItems(((SearchBar)sender).Text);
        }

        private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            if (swipeItem?.BindingContext is ItemViewModel itemViewModel)
            {
                await Navigation.PushModalAsync(new ItemEditPage(itemViewModel, _viewModel));
            }
        }

        private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            if (swipeItem?.BindingContext is ItemViewModel itemViewModel)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this item?", "Yes", "No");
                if (confirmDelete)
                {
                    await _viewModel.DeleteItem(itemViewModel);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_viewModel.Items.Any())
            {
                _viewModel.LoadItemsCommand.Execute(null);
            }
        }

    }
}
