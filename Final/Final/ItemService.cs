using System.Threading.Tasks;
using Xamarin.Forms;
using Final.Models;

namespace Final.Services
{
    public class ItemService
    {
        private static ItemService _instance;
        public static ItemService Instance => _instance ?? (_instance = new ItemService());

        private ItemDetails _currentItemDetails;
        public ImageSource ItemImage { get; set; } 

        private ItemService()
        {
            _currentItemDetails = new ItemDetails();
        }

        public ItemDetails GetItemDetails()
        {
            return _currentItemDetails;
        }

        public async Task SaveItemDetailsAsync(ItemDetails itemDetails)
        {
            
            await Task.Delay(500);

            
            _currentItemDetails = itemDetails;
        }
    }
}
