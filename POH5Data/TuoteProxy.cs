using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POH5Luokat;

namespace POH5Data
{
    public class TuoteProxy : Tuote
    {
        Toimittaja _toimittaja;
        TuoteRyhma _ryhma;
        bool ToimittajaHaettu = false;
        bool RyhmaHaettu = false;


        public ToimittajaRepository ToimittajaRepository { get; set; }
        public TuoteRyhmaRepository TuoteRyhmaRepository { get; set; }

        public override Toimittaja Toimittaja
        {
            get
            {
                if (base.ToimittajaId.HasValue)
                {
                    return _toimittaja;
                }
                else if (!ToimittajaHaettu)
                {
                    _toimittaja = ToimittajaRepository.Hae(base.ToimittajaId.Value);
                    ToimittajaHaettu = true;
                    return _toimittaja;
                }
                else
                {
                    return null;
                }

                //get => base.ToimittajaId.HasValue ? _toimittaja : null;
            }
            set => base.Toimittaja = value;
        }

        public override TuoteRyhma Ryhma
        {
            get
            {
                if (base.RyhmaId.HasValue)
                {
                    return _ryhma;
                }
                else if (!RyhmaHaettu)
                {
                    _ryhma = TuoteRyhmaRepository.Hae(base.RyhmaId.Value);
                    RyhmaHaettu = true;
                    return _ryhma;
                }
                else
                {
                    return null;
                }

                //get => base.ToimittajaId.HasValue ? _toimittaja : null;
            }
            set => base.Ryhma = value;
        }

        public TuoteProxy(int id, string nimi) : base(id, nimi)
        {

        }

    }
}
