using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Views
{
    [QueryProperty(nameof(MovieId), "id")]
    public partial class MovieDetailPage : ContentPage
    {
        private readonly DatabaseService _db;
        private Movie? _movie;

        public string MovieId
        {
            set
            {
                if (int.TryParse(value, out int id))
                    LoadMovie(id);
            }
        }

        public MovieDetailPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        private async void LoadMovie(int id)
        {
            _movie = await _db.GetMovieAsync(id);
            if (_movie == null) return;

            TitleLabel.Text = _movie.Title;
            GenreYearLabel.Text = $"{_movie.Genre} • {_movie.Year}";
            DescriptionLabel.Text = _movie.Description;
            CastLabel.Text = _movie.Cast;
            RatingLabel.Text = $"Average rating: {_movie.AverageRating:F1} / 5";

            if (!string.IsNullOrWhiteSpace(_movie.ImageUrl))
            {
                MovieImage.Source = _movie.ImageUrl;
                MovieImage.IsVisible = true;
            }

            FavoriteButton.Text = _movie.IsFavorite ? "Remove from Favorites" : "Add to Favorites";
            UpdateStars(_movie.AverageRating);
        }

        private void UpdateStars(double rating)
        {
            var stars = new[] { Star1, Star2, Star3, Star4, Star5 };
            for (int i = 0; i < stars.Length; i++)
                stars[i].TextColor = i < rating ? Colors.Gold : Colors.Gray;
        }

        private async void OnStarTapped(object sender, TappedEventArgs e)
        {
            if (_movie == null) return;

            if (e.Parameter is string param && int.TryParse(param, out int rating))
            {
                _movie.AverageRating = rating;
                await _db.SaveMovieAsync(_movie);
                RatingLabel.Text = $"Average rating: {_movie.AverageRating:F1} / 5";
                UpdateStars(rating);
            }
        }

        private async void OnFavoriteClicked(object sender, EventArgs e)
        {
            if (_movie == null) return;

            _movie.IsFavorite = !_movie.IsFavorite;
            await _db.SaveMovieAsync(_movie);
            FavoriteButton.Text = _movie.IsFavorite ? "Remove from Favorites" : "Add to Favorites";
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (_movie == null) return;
            await Shell.Current.GoToAsync($"addedit?id={_movie.Id}");
        }
    }
}