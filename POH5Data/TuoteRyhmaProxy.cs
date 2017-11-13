using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POH5Luokat;

namespace POH5Data
{
    public class TuoteRyhmaProxy : TuoteRyhma
    {
        List<Tuote> _tuotteet;
        bool TuotteetHaettu = false;
        public TuoteRepository TuoteRepository { get; set; }
        public override List<Tuote> Tuotteet
        {
            get
            {
                if (!TuotteetHaettu)
                {
                    _tuotteet = TuoteRepository.HaeRyhmanKaikki(base.Id);
                    TuotteetHaettu = true;

                }
                return _tuotteet;

                //get => base.ToimittajaId.HasValue ? _toimittaja : null;
            }
            set => base.Tuotteet = value;
        }

        public TuoteRyhmaProxy( int id, string nimi) : base(id, nimi)
        {

        }


    }
}
