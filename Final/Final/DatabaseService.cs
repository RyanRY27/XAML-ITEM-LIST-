using SQLite;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Final.Models;

namespace Final.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                {
                    var dbPath = Path.Combine(FileSystem.AppDataDirectory, "UserDetails.db3");
                    _database = new SQLiteAsyncConnection(dbPath);
                    _database.CreateTableAsync<UserDetails>().Wait();
                }
                return _database;
            }
        }

        public Task<int> SaveUserDetailsAsync(UserDetails userDetails)
        {
            if (userDetails.Id != 0)
            {
                return Database.UpdateAsync(userDetails);
            }
            else
            {
                return Database.InsertAsync(userDetails);
            }
        }

        public Task<UserDetails> GetUserDetailsAsync()
        {
            return Database.Table<UserDetails>().FirstOrDefaultAsync();
        }
    }
}
