using SQLite;
using MovieApp.Models;

namespace MovieApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;
        private bool _initialized = false;

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "movies.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Movie>();

            if (await _database.Table<Movie>().CountAsync() == 0)
                await SeedDataAsync();

            _initialized = true;
        }

        private async Task SeedDataAsync()
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Shawshank Redemption", Genre = "Drama", Year = 1994, Description = "Two imprisoned men bond over years.", Cast = "Tim Robbins, Morgan Freeman", AverageRating = 5 },
                new Movie { Title = "The Godfather", Genre = "Crime", Year = 1972, Description = "The aging patriarch of an organized crime dynasty.", Cast = "Marlon Brando, Al Pacino", AverageRating = 5 },
                new Movie { Title = "The Dark Knight", Genre = "Action", Year = 2008, Description = "Batman faces the Joker.", Cast = "Christian Bale, Heath Ledger", AverageRating = 5 },
                new Movie { Title = "Pulp Fiction", Genre = "Crime", Year = 1994, Description = "Lives of two mob hitmen intertwine.", Cast = "John Travolta, Samuel L. Jackson", AverageRating = 4 },
                new Movie { Title = "Forrest Gump", Genre = "Drama", Year = 1994, Description = "Life story of Forrest Gump.", Cast = "Tom Hanks, Robin Wright", AverageRating = 4 },
                new Movie { Title = "Inception", Genre = "Sci-Fi", Year = 2010, Description = "A thief who steals corporate secrets through dreams.", Cast = "Leonardo DiCaprio, Joseph Gordon-Levitt", AverageRating = 4 },
                new Movie { Title = "Interstellar", Genre = "Sci-Fi", Year = 2014, Description = "Explorers travel through a wormhole in space.", Cast = "Matthew McConaughey, Anne Hathaway", AverageRating = 4 },
                new Movie { Title = "The Matrix", Genre = "Sci-Fi", Year = 1999, Description = "A hacker discovers reality is a simulation.", Cast = "Keanu Reeves, Laurence Fishburne", AverageRating = 4 },
                new Movie { Title = "Goodfellas", Genre = "Crime", Year = 1990, Description = "The story of Henry Hill and his life in the mob.", Cast = "Ray Liotta, Robert De Niro", AverageRating = 4 },
                new Movie { Title = "Fight Club", Genre = "Drama", Year = 1999, Description = "An insomniac forms an underground fight club.", Cast = "Brad Pitt, Edward Norton", AverageRating = 4 }
            };

            await _database!.InsertAllAsync(movies);
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Movie>().ToListAsync();
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            await InitializeAsync();
            return await _database!.Table<Movie>().Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Movie>> GetFavoriteMoviesAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Movie>().Where(m => m.IsFavorite).ToListAsync();
        }

        public async Task<List<Movie>> GetMoviesByGenreAsync(string genre)
        {
            await InitializeAsync();
            return await _database!.Table<Movie>().Where(m => m.Genre == genre).ToListAsync();
        }

        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            await InitializeAsync();
            return await _database!.Table<Movie>().Where(m => m.Title.Contains(query)).ToListAsync();
        }

        public async Task<List<Movie>> GetTopMoviesAsync()
        {
            await InitializeAsync();
            var movies = await _database!.Table<Movie>().ToListAsync();
            return movies.OrderByDescending(m => m.AverageRating).ToList();
        }

        public async Task SaveMovieAsync(Movie movie)
        {
            await InitializeAsync();
            if (movie.Id == 0)
                await _database!.InsertAsync(movie);
            else
                await _database!.UpdateAsync(movie);
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            await InitializeAsync();
            await _database!.DeleteAsync(movie);
        }
    }
}