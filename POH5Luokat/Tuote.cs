namespace POH5Luokat
{
    public class Tuote : IId, INimi
    {
        public int Id { get; }
        public string Nimi { get; set; }
        public int? ToimittajaId { get; set; }
        public int? RyhmaId { get; set; }
        public string YksikkoKuvaus { get; set; }
        public double? YksikkoHinta { get; set; }
        public int? VarastoSaldo { get; set; }
        public int? TilausSaldo { get; set; }
        public int? HalytysRaja { get; set; }
        public bool EiKaytossa { get; set; }
        public virtual Toimittaja Toimittaja { get; set; }
        public virtual TuoteRyhma Ryhma { get; set; }
        public Tuote()
        {
            Toimittaja = null;
            Ryhma = null;
        }
        public Tuote(int id, string nimi)
            : this()
        {
            Id = id;
            Nimi = nimi;
        }
        public override string ToString() => $"{Id} {Nimi}";
    }
}
