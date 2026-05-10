using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MenedzerZadan
{
  public class Zadanie
    {
        public string Tresc { get; set; }
        public bool CzyZrobione { get; set; }
    }

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
                var noweZadanie = new Zadanie { Tresc = TxtZadanie.Text, CzyZrobione = false };
                ListaZadan.Items.Add(noweZadanie);
                TxtZadanie.Clear();
                ZapiszDoPliku();
            }
        }

        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            // 1. Tworzymy tymczasową "listę śmieci"
            var doUsuniecia = new List<Zadanie>();

            // 2. Szukamy wszystkich zadań, które mają zaznaczonego ptaszka
            foreach (Zadanie z in ListaZadan.Items)
            {
                if (z.CzyZrobione == true)
                {
                    doUsuniecia.Add(z);
                }
            }

            // 3. Usuwamy znalezione zadania z naszej głównej listy w oknie
            foreach (var z in doUsuniecia)
            {
                ListaZadan.Items.Remove(z);
            }

            // 4. Zapisujemy porządek do pliku tekstowego
            ZapiszDoPliku();
        }

        private void TxtZadanie_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnDodaj_Click(this, new RoutedEventArgs());
            }
        }

        private void ListaZadan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                BtnUsun_Click(this, new RoutedEventArgs());
            }
        }

       private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ZapiszDoPliku();
        }

        private void ZapiszDoPliku()
        {
            var linie = new List<string>();
            foreach (Zadanie z in ListaZadan.Items)
            {
                linie.Add($"{z.CzyZrobione}|{z.Tresc}");
            }
            File.WriteAllLines(sciezkaPliku, linie);
        }

        private void WczytajZPliku()
        {
           try
            {
                if (File.Exists(sciezkaPliku))
                {
                    var linie = File.ReadAllLines(sciezkaPliku);
                    foreach (var linia in linie)
                    {
                        // Rozcinamy tekst na dwie części używając znaku '|'
                        var czesci = linia.Split('|');
                        if (czesci.Length == 2)
                        {
                            var wczytaneZadanie = new Zadanie
                            {
                                CzyZrobione = bool.Parse(czesci[0]),
                                Tresc = czesci[1]
                            };
                            ListaZadan.Items.Add(wczytaneZadanie);
                        }
                    }
                }
            }
            catch
            {
               File.Delete(sciezkaPliku);
            }
        }
    }
}