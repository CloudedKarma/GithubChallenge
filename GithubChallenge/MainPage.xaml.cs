using System.Collections.Specialized;
using System.Reflection.Metadata;

namespace GithubChallenge
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }

        private async void ButtonReserveSeat(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Reserverd", "Your seat was reserverd successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }

        // 1. Reserving a range of seats
        private async void ButtonReserveRange(object sender, EventArgs e)
        {
            var startSeat = await DisplayPromptAsync("Reserve Seat Range", "Enter starting seat number (e.g., A1): ");
            var endSeat = await DisplayPromptAsync("Reserve Seat Range", "Enter ending seat number (e.g., A5): ");

            if (startSeat != null && endSeat != null)
            {
                bool rangeFound = false;
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == startSeat)
                        {
                            rangeFound = true;
                        }

                        if (rangeFound)
                        {
                            seatingChart[i, j].Reserved = true;

                            if (seatingChart[i, j].Name == endSeat)
                            {
                                await DisplayAlert("Success", "The seat range was successfully reserved!", "Ok");
                                RefreshSeating();
                                return;
                            }
                        }
                    }
                }

                if (!rangeFound)
                {
                    await DisplayAlert("Error", "Seat range was not found.", "Ok");
                }
            }
        }

        // 2. Cancel a specific seat reservation
        private async void ButtonCancelReservation(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Cancel Reservation", "Enter seat number to cancel: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat && seatingChart[i, j].Reserved)
                        {
                            seatingChart[i, j].Reserved = false;
                            await DisplayAlert("Cancelled", "Reservation has been canceled.", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found or is not reserved.", "Ok");
            }
        }

        // 3. Cancel a range of seat reservations
        private async void ButtonCancelReservationRange(object sender, EventArgs e)
        {
            var startSeat = await DisplayPromptAsync("Cancel Reservation Range", "Enter starting seat number: ");
            var endSeat = await DisplayPromptAsync("Cancel Reservation Range", "Enter ending seat number: ");

            if (startSeat != null && endSeat != null)
            {
                bool rangeFound = false;
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == startSeat)
                        {
                            rangeFound = true;
                        }

                        if (rangeFound)
                        {
                            seatingChart[i, j].Reserved = false;

                            if (seatingChart[i, j].Name == endSeat)
                            {
                                await DisplayAlert("Success", "The reservation range was successfully canceled!", "Ok");
                                RefreshSeating();
                                return;
                            }
                        }
                    }
                }

                if (!rangeFound)
                {
                    await DisplayAlert("Error", "Seat range was not found.", "Ok");
                }
            }
        }

        // 4. Reset the entire seating chart
        private void ButtonResetSeatingChart(object sender, EventArgs e)
        {
            for (int i = 0; i < seatingChart.GetLength(0); i++)
            {
                for (int j = 0; j < seatingChart.GetLength(1); j++)
                {
                    seatingChart[i, j].Reserved = false;
                }
            }

            DisplayAlert("Success", "The seating chart has been reset.", "Ok");
            RefreshSeating();
        }

    }

}

