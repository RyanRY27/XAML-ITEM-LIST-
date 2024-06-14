using Xamarin.Forms;

namespace Final
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate ButtonTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ItemViewModel)
                return ItemTemplate;
            else
                return ButtonTemplate;
        }
    }
}
