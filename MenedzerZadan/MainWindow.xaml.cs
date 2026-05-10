using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Linq;

namespace MenedzerZadan
{
    public partial class MainWindow : Window
    {
        private string sciezkaPliku = "zadania.txt";

        public MainWindow()
        {
            InitializeComponent();
            WczytajZPliku();
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrWhiteSpace(TxtZadanie.Text))
            {
                ListaZadan.Items.Add(TxtZadanie.Text);
                TxtZadanie.Clear();
                ZapiszDoPliku();
            }
        }
        private void TxtZadanie_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnDodaj_Click(this, new RoutedEventArgs());
            }
        }
        private void ListaZadan_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Sprawdzamy, czy naciśnięty klawisz to Delete
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                // Wywołujemy logikę przycisku Usuń
                BtnUsun_Click(this, new RoutedEventArgs());
            }
        }
        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (ListaZadan.SelectedItem != null)
            {
                ListaZadan.Items.Remove(ListaZadan.SelectedItem);
                ZapiszDoPliku();
            }
        }
        private void ZapiszDoPliku()
        {
            var zadania = ListaZadan.Items.Cast<string>();
            File.WriteAllLines(sciezkaPliku, zadania);
        }

        private void WczytajZPliku()
        {
            if (File.Exists(sciezkaPliku))
            {
                var zadania = File.ReadAllLines(sciezkaPliku);
                foreach (var zadanie in zadania)
                {
                    ListaZadan.Items.Add(zadanie);
                }
            }
        }
    }
}