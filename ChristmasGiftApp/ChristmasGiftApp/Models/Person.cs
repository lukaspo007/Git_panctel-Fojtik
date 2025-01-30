using System.Collections.Generic;
using System.Windows.Media;

namespace ChristmasGiftApp.Models
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Gifts { get; set; } = new List<string>();
        public Brush Color { get; set; } = Brushes.Black; // Výchozí barva

        public override string ToString()
        {
            return Name; // Zobrazí jméno v ListBoxu
        }
    }
}
