using System;
using System.IO;
using System.Linq;

class Osoba
{
    public string Imie { get; set; }
    public string Nazwisko { get; set; }
    public int Wiek { get; set; }
    public float Skutecznosc { get; set; }

    public Osoba(string imie, string nazwisko, int wiek, float skutecznosc)
    {
        Imie = imie;
        Nazwisko = nazwisko;
        Wiek = wiek;
        Skutecznosc = skutecznosc;
    }

    public string AnonimizujDane()
    {
        string anonimoweImie = Anonimizuj(Imie);
        string anonimoweNazwisko = Anonimizuj(Nazwisko);
        return $"{anonimoweImie} {anonimoweNazwisko}";
    }

    private string Anonimizuj(string dane)
    {
        if (dane.Length > 3)
        {
            return $"{dane[0]}{new string('*', dane.Length - 4)}{dane.Substring(dane.Length - 3)}";
        }
        return dane; // W przypadku krótkich ciągów zwracamy oryginał
    }

}

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Podaj nazwę pliku do wczytania:");
            string nazwaPliku = Console.ReadLine();

            if (!File.Exists(nazwaPliku))
            {
                Console.WriteLine("Plik nie istnieje!");
                return;
            }

            string[] linie = File.ReadAllLines(nazwaPliku);
            Osoba[] osoby = new Osoba[linie.Length];

            for (int i = 0; i < linie.Length; i++)
            {
                string[] czesci = linie[i].Split(',');
                string imie = czesci[0].Trim();
                string nazwisko = czesci[1].Trim();
                if (int.TryParse(czesci[2].Trim(), out int wiek) && float.TryParse(czesci[3].Trim(), out float skutecznosc))
                {
                    osoby[i] = new Osoba(imie, nazwisko, wiek, skutecznosc);
                }
                else
                {
                    Console.WriteLine($"Nie można przetworzyć danych w linii: {linie[i]}");
                }
            }

            float maksymalnaSkutecznosc = osoby.Max(o => o.Skutecznosc);
            var wybraneOsoby = osoby.Where(o => o != null && o.Imie.Length > 3 && o.Nazwisko.EndsWith("ski") && o.Skutecznosc == maksymalnaSkutecznosc);

            string nazwaNowegoPliku = Path.GetFileNameWithoutExtension(nazwaPliku) + ".max";
            using (StreamWriter sw = File.CreateText(nazwaNowegoPliku))
            {
                if (wybraneOsoby.Any())
                {
                    foreach (var osoba in wybraneOsoby)
                    {
                        sw.WriteLine($"{osoba.AnonimizujDane()}, {osoba.Wiek}");
                    }
                }
                else
                {
                    sw.WriteLine("");
                }
            }
            Console.WriteLine($"Plik {nazwaNowegoPliku} został utworzony.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        }
    }
}

