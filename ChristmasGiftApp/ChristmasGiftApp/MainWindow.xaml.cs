using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ChristmasGiftApp.Models;

namespace ChristmasGiftApp
{
    public partial class MainWindow : Window
    {
        private Random random = new Random(); // Pro generování barev
        private DispatcherTimer timer; // Pro časový odpočet

        public MainWindow()
        {
            InitializeComponent();
            InitializeCountdown(); // Nastavení časového odpočtu
        }

        // Inicializace časového odpočtu
        private void InitializeCountdown()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Aktualizace každou sekundu
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Aktualizace odpočítávání
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime christmas = new DateTime(now.Year, 12, 24, 0, 0, 0);

            // Pokud jsou už Vánoce pryč, nastav příští rok
            if (now > christmas)
            {
                christmas = christmas.AddYears(1);
            }

            TimeSpan timeLeft = christmas - now;
            CountdownTextBlock.Text = $"Zbývá {timeLeft.Days} dní, {timeLeft.Hours} hodin, {timeLeft.Minutes} minut, {timeLeft.Seconds} sekund do Vánoc!";
        }

        // Přidání osoby
        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PersonNameTextBox.Text) && PersonNameTextBox.Text != "Zadejte jméno osoby...")
            {
                var person = new Person
                {
                    Name = PersonNameTextBox.Text,
                    Color = GetRandomBrush() // Přiřadí náhodnou barvu
                };

                PersonsListBox.Items.Add(person);
                PersonNameTextBox.Text = "Zadejte jméno osoby...";
                PersonNameTextBox.Foreground = Brushes.Gray;
            }
            else
            {
                MessageBox.Show("Prosím, zadejte jméno osoby.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Přidání dárku
        private void AddGift_Click(object sender, RoutedEventArgs e)
        {
            if (PersonsListBox.SelectedItem is Person selectedPerson)
            {
                if (!string.IsNullOrWhiteSpace(GiftTextBox.Text) && GiftTextBox.Text != "Zadejte dárek...")
                {
                    selectedPerson.Gifts.Add(GiftTextBox.Text);
                    RefreshGiftsList(selectedPerson);
                    GiftTextBox.Text = "Zadejte dárek...";
                    GiftTextBox.Foreground = Brushes.Gray;
                }
                else
                {
                    MessageBox.Show("Prosím, zadejte dárek.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Prosím, vyberte osobu před přidáním dárku.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Odebrání dárku
        private void RemoveGift_Click(object sender, RoutedEventArgs e)
        {
            if (PersonsListBox.SelectedItem is Person selectedPerson && GiftsListBox.SelectedItem is string selectedGift)
            {
                selectedPerson.Gifts.Remove(selectedGift);
                RefreshGiftsList(selectedPerson);
            }
        }

        // Aktualizace seznamu dárků pro vybranou osobu
        private void PersonsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PersonsListBox.SelectedItem is Person selectedPerson)
            {
                RefreshGiftsList(selectedPerson);
                GiftsListBox.Foreground = selectedPerson.Color; // Nastavení barvy dárků podle osoby
            }
        }

        private void RefreshGiftsList(Person person)
        {
            GiftsListBox.Items.Clear();
            foreach (var gift in person.Gifts)
            {
                GiftsListBox.Items.Add(gift);
            }
        }

        // Generování náhodné barvy
        private Brush GetRandomBrush()
        {
            var color = Color.FromRgb(
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256));
            return new SolidColorBrush(color);
        }

        // Placeholder logika pro PersonNameTextBox
        private void PersonNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PersonNameTextBox.Text == "Zadejte jméno osoby...")
            {
                PersonNameTextBox.Text = string.Empty;
                PersonNameTextBox.Foreground = Brushes.Black;
            }
        }

        private void PersonNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PersonNameTextBox.Text))
            {
                PersonNameTextBox.Text = "Zadejte jméno osoby...";
                PersonNameTextBox.Foreground = Brushes.Gray;
            }
        }

        // Placeholder logika pro GiftTextBox
        private void GiftTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GiftTextBox.Text == "Zadejte dárek...")
            {
                GiftTextBox.Text = string.Empty;
                GiftTextBox.Foreground = Brushes.Black;
            }
        }

        private void GiftTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GiftTextBox.Text))
            {
                GiftTextBox.Text = "Zadejte dárek...";
                GiftTextBox.Foreground = Brushes.Gray;
            }
        }
    }
}
