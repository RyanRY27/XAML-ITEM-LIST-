using System.ComponentModel;
using Xamarin.Forms;

public class ItemViewModel : INotifyPropertyChanged
{
    private string _itemName;
    private string _itemDescription;
    private string _itemPrice;
    private ImageSource _itemImage;

    public string ItemName
    {
        get => _itemName;
        set
        {
            _itemName = value;
            OnPropertyChanged(nameof(ItemName));
        }
    }

    public string ItemDescription
    {
        get => _itemDescription;
        set
        {
            _itemDescription = value;
            OnPropertyChanged(nameof(ItemDescription));
        }
    }

    public string ItemPrice
    {
        get => _itemPrice;
        set
        {
            _itemPrice = value;
            OnPropertyChanged(nameof(ItemPrice));
        }
    }

    public ImageSource ItemImage
    {
        get => _itemImage;
        set
        {
            _itemImage = value;
            OnPropertyChanged(nameof(ItemImage));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
