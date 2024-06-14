using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using System;

namespace Final.Services
{
    public class UserDetailsService : INotifyPropertyChanged
    {
        private static readonly Lazy<UserDetailsService> lazy =
            new Lazy<UserDetailsService>(() => new UserDetailsService());

        public static UserDetailsService Instance => lazy.Value;

        private readonly SQLiteAsyncConnection _database;
        private UserDetails _userDetails;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Username
        {
            get => _userDetails?.Username;
            set
            {
                if (_userDetails != null)
                {
                    _userDetails.Username = value;
                    SaveUserDetailsAsync(_userDetails);
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _userDetails?.Email;
            set
            {
                if (_userDetails != null)
                {
                    _userDetails.Email = value;
                    SaveUserDetailsAsync(_userDetails);
                    OnPropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _userDetails?.Phone;
            set
            {
                if (_userDetails != null)
                {
                    _userDetails.Phone = value;
                    SaveUserDetailsAsync(_userDetails);
                    OnPropertyChanged();
                }
            }
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get => _profileImage;
            set
            {
                _profileImage = value;
                OnPropertyChanged();
            }
        }

        private UserDetailsService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userdetails.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UserDetails>().Wait();
            Task.Run(async () => await LoadUserDetailsAsync()).Wait();
        }

        public async Task LoadUserDetailsAsync()
        {
            _userDetails = await GetUserDetailsAsync();
            if (_userDetails == null)
            {
                _userDetails = new UserDetails();
                await SaveUserDetailsAsync(_userDetails);
            }
            ProfileImage = !string.IsNullOrEmpty(_userDetails?.ProfileImagePath)
                ? ImageSource.FromFile(_userDetails.ProfileImagePath)
                : null;
        }

        public async Task<UserDetails> GetUserDetailsAsync()
        {
            return await _database.Table<UserDetails>().FirstOrDefaultAsync();
        }

        public async Task SaveUserDetailsAsync(UserDetails details)
        {
            await _database.InsertOrReplaceAsync(details);
        }

        public UserDetails GetUserDetails()
        {
            return _userDetails;
        }

        public void UpdateProfileImage(string imagePath)
        {
            if (_userDetails != null)
            {
                _userDetails.ProfileImagePath = imagePath;
                SaveUserDetailsAsync(_userDetails);
                ProfileImage = ImageSource.FromFile(imagePath);
            }
        }
    }

    public class UserDetails
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
