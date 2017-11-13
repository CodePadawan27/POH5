using System.Collections.Generic;

namespace POH5Luokat
{
    public class Toimittaja : IId, INimi
    {
        public int Id { get; }
        public string Nimi { get; set; }
        public string YhteysHenkilo { get; set; }
        public string YhteysTitteli { get; set; }
        public string Katuosoite { get; set; }
        public string PostiKoodi { get; set; }
        public string Kaupunki { get; set; }
        public string Maa { get; set; }
        public virtual List<Tuote> Tuotteet { get; set; }
        public Toimittaja()
        {
            Tuotteet = new List<Tuote>();
        }
        public Toimittaja(int id, string nimi)
            :this()
        {
            Id = id;
            Nimi = nimi;
        }
        public override string ToString() => $"{Id} {Nimi} ({Tuotteet.Count})";
    }
}
