using MovieApp.Services;

namespace MovieApp.Views
{
    public partial class TopListPage : ContentPage
    {
        private readonly DatabaseService _db;

        public TopListPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var movies = await _db.GetTopMoviesAsync();
            TopMoviesCollection.ItemsSource = movies;
        }
    }
}