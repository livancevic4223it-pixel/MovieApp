using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Views
{
    public partial class FavoritesPage : ContentPage
    {
        private readonly DatabaseService _db;

        public FavoritesPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadFavorites();
        }

        private async Task LoadFavorites()
        {
            var favorites = await _db.GetFavoriteMoviesAsync();
            FavoritesCollection.ItemsSource = favorites;
        }

        private async void OnRemoveFavoriteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Movie movie)
            {
                movie.IsFavorite = false;
                await _db.SaveMovieAsync(movie);
                await LoadFavorites();
            }
        }

        private async void OnMovieTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Movie movie)
                await Shell.Current.GoToAsync($"moviedetail?id={movie.Id}");
        }
    }
}