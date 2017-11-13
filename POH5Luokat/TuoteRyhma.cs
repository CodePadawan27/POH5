using System.Collections.Generic;

namespace POH5Luokat
{
    public class TuoteRyhma : IId, INimi
    {
        public int Id { get; }
        public string Nimi { get; set; }
        public string Kuvaus { get; set; }
        public virtual List<Tuote> Tuotteet { get; set; }
        public TuoteRyhma()
        {
            Tuotteet = new List<Tuote>();
        }
        public TuoteRyhma(int id, string nimi)
            : this()
        {
            Id = id;
            Nimi = nimi;
        }
        public override string ToString() => $"{Id} {Nimi} ({Tuotteet.Count})";
    }
}
