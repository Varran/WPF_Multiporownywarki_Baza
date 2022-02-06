using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_multi_próby
{
    //https://www.codeproject.com/Articles/37241/Displaying-a-Data-Matrix-in-WPF

    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MixedPaint> mixedPaints;
        public ObservableCollection<MixedPaint> MixedPaints { get { return mixedPaints; } }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private MixedPaint selectedMixedPaint;
        public MixedPaint SelectedMixedPaint
        {
            get { return selectedMixedPaint; }
            set
            {
                selectedMixedPaint = value;
                OnPropertyChanged(nameof(SelectedMixedPaint));
            }
        }

        private ObservableCollection<MatrixLine> comparisonMatrix;
        public ObservableCollection<MatrixLine> ComparisonMatrix { get { return comparisonMatrix; } }

        public ViewModel()
        {
            ColorBase yellowA = new ColorBase("YellowA", 110);
            ColorBase yellowB = new ColorBase("YellowB", 175);
            ColorBase blueA = new ColorBase("BlueA", 77);
            ColorBase blueB = new ColorBase("BlueB", 135);
            ColorBase redA = new ColorBase("RedA", 95);
            ColorBase redB = new ColorBase("RedB", 225);
            ColorBase whiteA = new ColorBase("WhiteA", 200);

            MixedPaint greenA = new MixedPaint("GreenLight")
                .AddIngredient(yellowA)
                .AddIngredient(blueA);
            MixedPaint greenB = new MixedPaint("GreenDark")
                .AddIngredient(yellowB)
                .AddIngredient(blueB);
            MixedPaint orangeA = new MixedPaint("OrangeLight")
                .AddIngredient(yellowA)
                .AddIngredient(redB)
                .AddIngredient(whiteA);
            MixedPaint orangeB = new MixedPaint("OrangeDark")
                .AddIngredient(yellowB)
                .AddIngredient(redB);
            MixedPaint violet = new MixedPaint("Violet")
                .AddIngredient(redA)
                .AddIngredient(blueB);

            mixedPaints = new ObservableCollection<MixedPaint>() { greenA, greenB, orangeA, orangeB, violet };
            SelectedMixedPaint = greenA;

            List<ColorBase> uniqueColorsBase = new List<ColorBase>();

            foreach (var item in mixedPaints)
                foreach (var item2 in item.Ingredients)
                    if (!uniqueColorsBase.Contains(item2))
                        uniqueColorsBase.Add(item2);

            uniqueColorsBase = uniqueColorsBase.OrderBy(o => o.Name).ThenBy(o => o.Saturation).ToList();

            comparisonMatrix = new ObservableCollection<MatrixLine>();

            foreach (var color in uniqueColorsBase)
            {
                MatrixLine line = new MatrixLine(color);
                foreach (var mixed in mixedPaints)
                    line.AddToMatrix(mixed);

                comparisonMatrix.Add(line);
            }

            values = new ObservableCollection<ObservableCollection<bool>>();
            foreach (var item in comparisonMatrix)
            {
                ObservableCollection<bool> oc = new ObservableCollection<bool>();

                foreach (var item2 in item.Matrix)
                {
                    oc.Add(item2.Value);
                }
                values.Add(oc);
            }
        }

        public ObservableCollection<MatrixLine> RowHeaders
        {
            get { return comparisonMatrix; }
        }

        public ObservableCollection<MixedPaint> ColumnHeaders
        {
            get { return mixedPaints; }
        }

        private ObservableCollection<ObservableCollection<bool>> values;
        
        public ObservableCollection<ObservableCollection<bool>> Values
        {
            get { return values; }
        }
    }
}
