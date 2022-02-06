using MatrixLib.Matrix;
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

    public class ViewModel : MatrixBase<MatrixLine, MixedPaint>, INotifyPropertyChanged
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

        private ObservableCollection<ObservableCollection<bool>> values;
        public ObservableCollection<ObservableCollection<bool>> Values { get { return values; } }
        public ObservableCollection<MatrixLine> RowHeaders { get { return comparisonMatrix; } }
        public ObservableCollection<MixedPaint> ColumnHeaders { get { return mixedPaints; } }

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

            MixedPaint greenA = new MixedPaint("Green Light")
                .AddIngredient(yellowA)
                .AddIngredient(blueA);
            MixedPaint greenB = new MixedPaint("Green Dark")
                .AddIngredient(yellowB)
                .AddIngredient(blueB);
            MixedPaint orangeA = new MixedPaint("Orange Light")
                .AddIngredient(yellowA)
                .AddIngredient(redB)
                .AddIngredient(whiteA);
            MixedPaint orangeB = new MixedPaint("Orange Dark")
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

            colorBases = new ColorBase[7];
            colorBases[0] = yellowA;
            colorBases[1] = yellowB;
            colorBases[2] = blueA;
            colorBases[3] = blueB;
            colorBases[4] = redA;
            colorBases[5] = redB;
            colorBases[6] = whiteA;

            mixedPaints2 = new MixedPaint[5];
            mixedPaints2[0] = violet;
            mixedPaints2[1] = greenA;
            mixedPaints2[2] = greenB;
            mixedPaints2[3] = orangeA;
            mixedPaints2[4] = orangeB;

            matrixLines = new MatrixLine[7];
            for (int i = 0; i < colorBases.Length; i++)
            {
                MatrixLine line = new MatrixLine(colorBases[i]);
                foreach (var mixed in mixedPaints)
                    line.AddToMatrix(mixed);

                matrixLines[i] = line;
            }
        }

        readonly MatrixLine[] matrixLines;
        readonly MixedPaint[] mixedPaints2;
        readonly ColorBase[] colorBases;

        protected override object GetCellValue(MatrixLine rowHeaderValue, MixedPaint columnHeaderValue) {  return rowHeaderValue.Matrix[columnHeaderValue.PaintName]; }
        protected override IEnumerable<MixedPaint> GetColumnHeaderValues() { return mixedPaints; }
        protected override IEnumerable<MatrixLine> GetRowHeaderValues() { return matrixLines;  }        
    }
}
