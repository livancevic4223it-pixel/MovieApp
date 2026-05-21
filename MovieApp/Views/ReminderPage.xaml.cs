using MovieApp.Models;
using MovieApp.Services;
using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;

namespace MovieApp.Views
{
    public partial class ReminderPage : ContentPage
    {
        private readonly DatabaseService _db;
        private List<Movie> _movies = new();

        public ReminderPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _movies = await _db.GetMoviesAsync();
            MoviePicker.ItemsSource = _movies.Select(m => m.Title).ToList();
        }

        private async void OnSetReminderClicked(object sender, EventArgs e)
        {
            if (MoviePicker.SelectedIndex < 0)
            {
                await DisplayAlertAsync("Error", "Please select a movie.", "OK");
                return;
            }

            var movie = _movies[MoviePicker.SelectedIndex];
            var reminderDateTime = ReminderDate.Date + ReminderTime.Time;

            if (reminderDateTime <= DateTime.Now)
            {
                await DisplayAlertAsync("Error", "Please select a future date and time.", "OK");
                return;
            }

            var notification = new NotificationRequest
            {
                NotificationId = movie.Id,
                Title = "Movie Reminder",
                Description = $"Time to watch: {movie.Title}!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = reminderDateTime
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
            await DisplayAlertAsync("Success", $"Reminder set for {movie.Title} at {reminderDateTime:g}", "OK");
        }
    }
}