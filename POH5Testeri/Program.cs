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

            Demo1(tr);
            /*Demo2();
            Demo3();
            Demo4();
            Demo5();*/

            Console.ReadLine();


        }

        static void Demo1(TuoteRepository tr)
        {
            //var tuotteet = tr.HaeKaikki();
            Console.WriteLine($"Tuotteita {tr.HaeKaikki().Count} kpl");
            int syote;
            do
            {
                Console.WriteLine("Anna tuotteen id: ");
                syote = int.Parse(Console.ReadLine());
            } while (syote < 0);


            var valittuTuote = tr.HaeKaikki()
                .Where(t => t.Id.Equals(syote))
                .ToList();

            foreach (var tuote in valittuTuote)
            {
                Console.WriteLine($"{tuote.Id} {tuote.Nimi} toimittaja: {tuote.ToimittajaId} {tuote.Toimittaja.Nimi} {tuote.RyhmaId} {tuote.Ryhma.Nimi}");

                //.ForEach(a => Console.WriteLine($"{t.Id} {t.Nimi} {t.ToimittajaId} {t.Toimittaja.Nimi} {t.RyhmaId} {t.Ryhma.Nimi}"));

            }
        }

        static void Demo2(TuoteRyhmaRepository ttr)
        {

        }

        static void Demo3(TuoteRyhmaRepository ttr)
        {

        }

        static void Demo4(TuoteRyhmaRepository ttr)
        {

        }

        static void Demo5(TuoteRyhmaRepository ttr)
        {

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
