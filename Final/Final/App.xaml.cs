using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Final
{
    public partial class App : Application
    {
        static ItemDatabase database;

        public static ItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Items.db3"));
                }
                return database;
            }
        }

        
        public static ItemPageViewModel ItemPageViewModel { get; private set; }

        public App()
        {
            InitializeComponent();

            
            ItemPageViewModel = new ItemPageViewModel();

            
            MainPage = new NavigationPage(new Home());
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
