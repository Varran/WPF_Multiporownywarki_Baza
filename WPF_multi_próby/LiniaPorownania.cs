using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_multi_próby
{
    public class LiniaPorownania
    {
        public Kolor SkladnikKolor { get; private set; }
        public Dictionary<string, bool> Matryca;

        public LiniaPorownania(Kolor kolor)
        {
            Matryca = new Dictionary<string, bool>();
            this.SkladnikKolor = kolor;
        }

        public void DodajDoMatrycy(ZmieszanaFarba mieszanina)
        {
            string nazwaMieszaniny = mieszanina.NazwaMieszaniny;
            bool czyZawieraTenSkladnik = mieszanina.Skladniki.Any(o => (o.Nazwa == SkladnikKolor.Nazwa &&
                                                                        o.Nasycenie == SkladnikKolor.Nasycenie));
            Matryca.Add(nazwaMieszaniny, czyZawieraTenSkladnik);
        }
    }
}
