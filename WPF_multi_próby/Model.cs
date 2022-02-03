using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_multi_próby
{
    public class Kolor 
    {
        public string Nazwa { get; }   
        public int Nasycenie { get; private set; }  
        public Kolor(string nazwa, int nasycenie)
        {
            this.Nazwa = nazwa;
            this.Nasycenie = nasycenie;
        }

        public void ZmienNasycenie(int noweNasycenie)
        {
            Nasycenie = noweNasycenie;
        }

        public override string ToString()
        {
            return $"Kolor:{Nasycenie.ToString().PadLeft(4, ' ')} - '{Nazwa}'";
        }
    }

    public class ZmieszanaFarba
    {
        public string NazwaMieszaniny { get; } 
        public List<Kolor> Skladniki { get; }

        public ZmieszanaFarba(string nazwa)
        {
            Skladniki = new List<Kolor>();
            this.NazwaMieszaniny= nazwa;
        }

        public ZmieszanaFarba DodajSkladnik(Kolor kolor)
        {
            bool czyDodany = false;

            foreach (var item in Skladniki)
            {
                if (item.Nazwa == kolor.Nazwa )
                {
                    item.ZmienNasycenie(item.Nasycenie + kolor.Nasycenie);
                    czyDodany = true;
                }
            }

            if (!czyDodany)
                Skladniki.Add(kolor);

            return this;
        }


    }
}
