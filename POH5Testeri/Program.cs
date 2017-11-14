using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using POH5Data;
using POH5Luokat;
using System.Configuration;

namespace POH5Testeri
{
    public class Program
    {
        static void Main(string[] args)
        {
            AsetaDataDirectory();

            ConnectionStringSettings yhteysasetukset = ConfigurationManager.ConnectionStrings["DB"];
            TuoteRepository tr = new TuoteRepository(yhteysasetukset.ConnectionString);
            TuoteRyhmaRepository trr = new TuoteRyhmaRepository(yhteysasetukset.ConnectionString);

            Console.WriteLine("Terve, ole hyvä ja valitse\n1. Demo1\n2. Demo2\n3. Demo3\n4. Demo4\n5. Demo5");
            int syote = int.Parse(Console.ReadLine());

            switch (syote)
            {
                case 1:
                    Demo1(tr);
                    break;
                case 2:
                    Demo2(trr);
                    break;
                case 3:
                    Demo3(trr);
                    break;
                case 4:
                    Demo4(trr);
                    break;
                case 5:
                    Demo5(trr);
                    break;
                default:
                    break;
            }


            //Demo1(tr);
            //Demo2(trr);
            //Demo3(trr);
            //Demo4(trr);
            //Demo5(trr);

            Console.ReadLine();
        }

        static void Demo1(TuoteRepository tr)
        {
            Console.WriteLine($"Tuotteita {tr.HaeKaikki().Count} kpl");
            int syote;
            while (true)
            {
                do
                {
                    Console.WriteLine("Anna tuotteen id: ");
                    syote = int.Parse(Console.ReadLine());
                } while (syote < 0);


                var valittuTuote = tr.HaeKaikki()
                    .Where(t => t.Id.Equals(syote));

                foreach (var tuote in valittuTuote)
                {
                    Console.WriteLine($"{tuote.Id} {tuote.Nimi} toimittaja: {tuote.ToimittajaId} {tuote.Toimittaja.Nimi} tuoteryhmä: {tuote.RyhmaId} {tuote.Ryhma.Nimi}\n");

                }
            }
        }

        static void Demo2(TuoteRyhmaRepository ttr)
        {
            var lista = ttr.HaeKaikki();
            foreach (var tuoteRyhma in lista)
            {
                if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                else
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
            }
            Console.WriteLine("Valitse ryhmä: ");
            int syote = int.Parse(Console.ReadLine());
            var valittulista = lista.Where(l => l.Id.Equals(syote));

            foreach (var tuoteRyhma in valittulista)
            {
                if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                else
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
            }
            Console.WriteLine("Tuotteet: ");
            foreach (var tuoteRyhma in valittulista)
            {
                foreach (var tuote in tuoteRyhma.Tuotteet)
                {
                    Console.WriteLine($"{tuote.Id} {tuote.Nimi}, yksikköhinta: {tuote.YksikkoHinta:0.00} / {tuote.YksikkoKuvaus}");
                }
            }
            Console.WriteLine("Paina Enter lopettaaksesi...");
        }

        static void Demo3(TuoteRyhmaRepository ttr)
        {
            Console.WriteLine("Lisätäänkö uusi <k/e>: ");
            char syotto = char.Parse(Console.ReadLine());

            if (syotto.Equals('k') || syotto.Equals('K'))
            {
                TuoteRyhma uusiTuoteRyhma = new TuoteRyhma();
                Console.WriteLine("Anna nimi: ");
                uusiTuoteRyhma.Nimi = Console.ReadLine();
                Console.WriteLine("Anna kuvaus: ");
                string tuoteryhmaKuvaus = Console.ReadLine();
                if (tuoteryhmaKuvaus.Equals(""))
                {
                    uusiTuoteRyhma.Kuvaus = null;
                }
                else
                {
                    uusiTuoteRyhma.Kuvaus = tuoteryhmaKuvaus;
                }

                ttr.Lisaa(uusiTuoteRyhma);

                var lista = ttr.HaeKaikki();
                foreach (var tuoteRyhma in lista)
                {
                    if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                        Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                    else
                        Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        static void Demo4(TuoteRyhmaRepository ttr)
        {
            var lista = ttr.HaeKaikki();
            foreach (var tuoteRyhma in lista)
            {
                if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                else
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
            }

            Console.WriteLine("Valitse muutettava: ");
            int syote = int.Parse(Console.ReadLine());
            var valittuTuoteRyhma = lista.Where(t => t.Id.Equals(syote));

            Console.WriteLine("Muuta nimi: ");
            string uusiNimi = Console.ReadLine();
            foreach (var tuoteRyhma in valittuTuoteRyhma)
            {
                if (!string.IsNullOrEmpty(uusiNimi))
                {
                    tuoteRyhma.Nimi = uusiNimi;
                }

            }

            Console.WriteLine("Muuta kuvaus: ");
            string uusiKuvaus = Console.ReadLine();
            foreach (var tuoteRyhma in valittuTuoteRyhma)
            {
                if (uusiKuvaus.Equals("NULL"))
                    tuoteRyhma.Kuvaus = null;
                else
                    tuoteRyhma.Kuvaus = uusiKuvaus;
            }

            foreach (var tuoteRyhma in valittuTuoteRyhma)
            {
                ttr.Muuta(tuoteRyhma);
            }

            var uusiLista = ttr.HaeKaikki();
            foreach (var tuoteRyhma in uusiLista)
            {
                if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                else
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
            }

            Console.WriteLine("Paina Enter lopettaaksesi...");
        }

        static void Demo5(TuoteRyhmaRepository ttr)
        {
            var lista = ttr.HaeKaikki();
            foreach (var tuoteRyhma in lista)
            {
                if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                else
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
            }

            Console.WriteLine("Valitse poistettava");
            int syote = int.Parse(Console.ReadLine());
            ttr.Poista(syote);

            var uusiLista = ttr.HaeKaikki();
            foreach (var tuoteRyhma in uusiLista)
            {
                if (string.IsNullOrEmpty(tuoteRyhma.Kuvaus))
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: NULL");
                else
                    Console.WriteLine($"{tuoteRyhma.Id} {tuoteRyhma.Nimi}: {tuoteRyhma.Kuvaus}");
            }

            Console.WriteLine("Paina Enter lopettaaksesi...");
        }

        static void AsetaDataDirectory()
        {
            // Asetetaan muuttuja DataDirectory, jota käytetään yhteysmerkkijonossa  
            // tiedostossa App.config

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\..\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }
    }
}
