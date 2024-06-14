using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Final
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            flyout.listview.ItemSelected += OnSelectedItem;

            
            MessagingCenter.Subscribe<ItemEditPage, ItemViewModel>(this, "ItemDeleted", async (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("MainPage received ItemDeleted message");
                await CloseItemDetailPage();
            });
        }

        private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var Item = e.SelectedItem as FlyoutItemPage;
            if (Item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(Item.TargetPage));
                flyout.listview.SelectedItem = null;
                IsPresented = false;
            }
        }

        private async Task CloseItemDetailPage()
        {
            if (Navigation.ModalStack.Count > 0 && Navigation.ModalStack.Last() is ItemDetailPage)
            {
                System.Diagnostics.Debug.WriteLine("MainPage Closing ItemDetailPage");
                await Navigation.PopModalAsync();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            System.Diagnostics.Debug.WriteLine("MainPage unsubscribing from ItemDeleted message");
            MessagingCenter.Unsubscribe<ItemEditPage, ItemViewModel>(this, "ItemDeleted");
        }
    }
}
