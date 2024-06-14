using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Final
{
    public class FlyoutMenuDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MenuTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var flyoutItem = item as FlyoutItemPage;
            if (flyoutItem?.Title == "MENU")
            {
                return MenuTemplate;
            }
            return ItemTemplate;
        }
    }
}