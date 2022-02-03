using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_multi_próby
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ZmieszanaFarba> mieszaniny;
        public ObservableCollection<ZmieszanaFarba> Mieszaniny { get { return mieszaniny; } }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string nazwa)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nazwa));
        }

        private ZmieszanaFarba zaznaczonaFarba;
        public ZmieszanaFarba ZaznaczonaFarba {
            get { return zaznaczonaFarba; }
            set { zaznaczonaFarba = value;
                OnPropertyChanged(nameof(ZaznaczonaFarba)); } }

        private ObservableCollection<LiniaPorownania> matrycaPorownan;
        public ObservableCollection<LiniaPorownania> MatrycaPorownan  { get { return matrycaPorownan; } }

        public ViewModel()
        {
            Kolor zoltyA = new Kolor("ŻółtyA", 110);
            Kolor zoltyB = new Kolor("ŻółtyB", 175);
            Kolor niebieskiA = new Kolor("NiebieskiA", 77);
            Kolor niebieskiB = new Kolor("NiebieskiB", 135);
            Kolor czerwonyA = new Kolor("CzerwonyA", 95);
            Kolor czerwonyB = new Kolor("CzerwonyB", 225);
            Kolor bialyA = new Kolor("BiałyA", 200);

            ZmieszanaFarba zielonyA = new ZmieszanaFarba("ZielonyJasny")
                .DodajSkladnik(zoltyA)
                .DodajSkladnik(niebieskiA);
            ZmieszanaFarba zielonyB = new ZmieszanaFarba("ZielonyCiemny")
                .DodajSkladnik(zoltyB)
                .DodajSkladnik(niebieskiB);
            ZmieszanaFarba pomaranczA = new ZmieszanaFarba("PomaranczJasny")
                .DodajSkladnik(zoltyA)
                .DodajSkladnik(czerwonyB)
                .DodajSkladnik(bialyA);
            ZmieszanaFarba pomaranczB = new ZmieszanaFarba("PomaranczCiemny")
                .DodajSkladnik(zoltyB)
                .DodajSkladnik(czerwonyB);
            ZmieszanaFarba fiolet = new ZmieszanaFarba("Fioler")
                .DodajSkladnik(czerwonyA)
                .DodajSkladnik(niebieskiB);

            mieszaniny = new ObservableCollection<ZmieszanaFarba>() { zielonyA, zielonyB, pomaranczA, pomaranczB, fiolet };
            ZaznaczonaFarba = zielonyA;

            List<Kolor> unikalneKolory = new List<Kolor>();

            foreach (var item in mieszaniny)
                foreach (var item2 in item.Skladniki)
                    if (!unikalneKolory.Contains(item2))
                        unikalneKolory.Add(item2);

            unikalneKolory = unikalneKolory.OrderBy(o => o.Nazwa).ThenBy(o => o.Nasycenie).ToList();

            matrycaPorownan = new ObservableCollection<LiniaPorownania>();

            foreach (var kolor in unikalneKolory)
            {
                LiniaPorownania linia = new LiniaPorownania(kolor);
                foreach (var mieszanina in mieszaniny)
                    linia.DodajDoMatrycy(mieszanina);

                matrycaPorownan.Add(linia);
            }
        }
    }
}
