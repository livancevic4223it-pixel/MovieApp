using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp.Views
{
    [QueryProperty(nameof(MovieId), "id")]
    public partial class AddEditMoviePage : ContentPage
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

        public AddEditMoviePage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        private async void LoadMovie(int id)
        {
            _movie = await _db.GetMovieAsync(id);
            if (_movie != null)
            {
                TitleEntry.Text = _movie.Title;
                GenrePicker.SelectedItem = _movie.Genre;
                YearEntry.Text = _movie.Year.ToString();
                DescriptionEditor.Text = _movie.Description;
                CastEntry.Text = _movie.Cast;
                ImageUrlEntry.Text = _movie.ImageUrl;
                Title = "Edit Movie";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Error", "Title is required.", "OK");
                return;
            }

            var movie = _movie ?? new Movie();
            movie.Title = TitleEntry.Text;
            movie.Genre = GenrePicker.SelectedItem?.ToString() ?? "Drama";
            movie.Year = int.TryParse(YearEntry.Text, out int year) ? year : 0;
            movie.Description = DescriptionEditor.Text ?? string.Empty;
            movie.Cast = CastEntry.Text ?? string.Empty;
            movie.ImageUrl = ImageUrlEntry.Text ?? string.Empty;

            await _db.SaveMovieAsync(movie);
            await Shell.Current.GoToAsync("..");
        }
    }
}