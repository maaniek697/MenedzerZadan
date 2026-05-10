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
            AktualizujLicznik();
        }

        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtZadanie.Text))
            {
                var noweZadanie = new Zadanie { Tresc = TxtZadanie.Text, CzyZrobione = false };
                ListaZadan.Items.Add(noweZadanie);
                TxtZadanie.Clear();
                
                ZapiszDoPliku();
                AktualizujLicznik(); 
            }
        }

        private void BtnUsun_Click(object sender, RoutedEventArgs e)
        {
            var doUsuniecia = new List<Zadanie>();
            foreach (Zadanie z in ListaZadan.Items)
            {
                if (z.CzyZrobione)
                {
                    doUsuniecia.Add(z);
                }
            }

            foreach (var z in doUsuniecia)
            {
                ListaZadan.Items.Remove(z);
            }

            ZapiszDoPliku();
            AktualizujLicznik(); 
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
            AktualizujLicznik(); 
        }

        private void EdycjaZadania_Zapisz(object sender, RoutedEventArgs e)
        {
            ZapiszDoPliku();
        }

        private void EdycjaZadania_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus(); // Odznaczamy pole tekstowe
                ZapiszDoPliku();
            }
        }

        
        private void AktualizujLicznik()
        {
            int wszystkie = ListaZadan.Items.Count;
            int zrobione = 0;

            foreach (Zadanie z in ListaZadan.Items)
            {
                if (z.CzyZrobione)
                {
                    zrobione++;
                }
            }

            TxtLicznik.Text = $"Zrobione: {zrobione} / {wszystkie}";
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