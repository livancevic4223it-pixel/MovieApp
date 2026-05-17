using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _db;
        private List<Movie> _allMovies = new();
        private string _selectedGenre = "All";

        public MainPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadMovies();
        }

        private async Task LoadMovies()
        {
            _allMovies = await _db.GetMoviesAsync();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _allMovies;

            if (_selectedGenre != "All")
                filtered = filtered.Where(m => m.Genre == _selectedGenre).ToList();

            var searchText = SearchBar.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
                filtered = filtered.Where(m => m.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

            MoviesCollection.ItemsSource = filtered;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void OnGenreFilterClicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                _selectedGenre = btn.Text;
                ApplyFilters();
            }
        }

        private async void OnMovieTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Movie movie)
                await Shell.Current.GoToAsync($"moviedetail?id={movie.Id}");
        }

        private async void OnAddMovieClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("addedit");
        }
    }
}