using MatrixLib.Matrix;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_multi_próby
{
    //https://www.codeproject.com/Articles/37241/Displaying-a-Data-Matrix-in-WPF

    public class ViewModel : MatrixBase<MatrixLine, MixedPaint>, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Zrodla mojej kontrolki
        private ObservableCollection<MatrixLine> comparisonMatrix;
        public ObservableCollection<MatrixLine> ComparisonMatrix { get { return comparisonMatrix; } }
        #endregion

        #region zrodla kontrolki StackOverFlow
        private ObservableCollection<ObservableCollection<bool>> values;
        public ObservableCollection<ObservableCollection<bool>> Values { get { return values; } }
        public ObservableCollection<MatrixLine> RowHeaders { get { return comparisonMatrix; } }
        public ObservableCollection<MixedPaint> ColumnHeaders { get { return mixedPaints; } }

        #endregion

        #region zrodla kontrolki CodeProject
        //readonly MatrixLine[] matrixLines;
        //readonly MixedPaint[] mixedPaints2;
        //readonly ColorBase[] colorBases;
        protected override ObservableCollection<MixedPaint> GetColumnHeaderValues { get { return mixedPaints; } }
        protected override ObservableCollection<MatrixLine> GetRowHeaderValues { get { return comparisonMatrix; } }
        protected override object GetCellValue(MatrixLine rowHeaderValue, MixedPaint columnHeaderValue) { return rowHeaderValue.Matrix[columnHeaderValue.PaintName]; }
        #endregion

        #region baza kolorow i mieszanin
        private ObservableCollection<ColorBase> baseColors;
        public ObservableCollection<ColorBase> BaseColors { get { return baseColors; } }

        private ObservableCollection<MixedPaint> mixedPaints;
        public ObservableCollection<MixedPaint> MixedPaints { get { return mixedPaints; } }

        private MixedPaint? selectedMixedPaint;
        public MixedPaint? SelectedMixedPaint
        {
            get { return selectedMixedPaint; }
            set
            {
                selectedMixedPaint = value;
                OnPropertyChanged(nameof(SelectedMixedPaint));
            }
        }
        #endregion

        public ViewModel()
        {
            #region zrodla danych dla bazy kolorow i mieszanin
            ColorBase yellowA = new ColorBase("Yellow A", 110);
            ColorBase yellowB = new ColorBase("Yellow B", 175);
            ColorBase blueA = new ColorBase("Blue A", 77);
            ColorBase blueB = new ColorBase("Blue B", 135);
            ColorBase redA = new ColorBase("Red A", 95);
            ColorBase redB = new ColorBase("Red B", 225);
            ColorBase whiteA = new ColorBase("White A", 200);

            baseColors = new ObservableCollection<ColorBase>() { yellowA, yellowB, blueA, blueB, redA, redB, whiteA };

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


            newIgredients = new ObservableCollection<ColorBase>();
            #endregion

            #region zrodla danych mojej kontrolki oraz z stackoverflow
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
            #endregion

            #region zrodla danych kontrolki stackoverflow
            values = new ObservableCollection<ObservableCollection<bool>>();
            foreach (var item in comparisonMatrix)
            {
                ObservableCollection<bool> oc = new ObservableCollection<bool>();

                foreach (var item2 in item.Matrix)
                    oc.Add(item2.Value);

                values.Add(oc);
            }
            #endregion

            #region zrodla danych kontrolki CodeProject
            //colorBases = new ColorBase[7];
            //colorBases[0] = yellowA;
            //colorBases[1] = yellowB;
            //colorBases[2] = blueA;
            //colorBases[3] = blueB;
            //colorBases[4] = redA;
            //colorBases[5] = redB;
            //colorBases[6] = whiteA;

            //mixedPaints2 = new MixedPaint[5];
            //mixedPaints2[0] = violet;
            //mixedPaints2[1] = greenA;
            //mixedPaints2[2] = greenB;
            //mixedPaints2[3] = orangeA;
            //mixedPaints2[4] = orangeB;

            //matrixLines = new MatrixLine[7];
            //for (int i = 0; i < colorBases.Length; i++)
            //{
            //    MatrixLine line = new MatrixLine(colorBases[i]);
            //    foreach (var mixed in mixedPaints)
            //        line.AddToMatrix(mixed);

            //    matrixLines[i] = line;
            //}
            #endregion
        }

        #region  dodawanie nowego koloru
        private ICommand addNewColorCommand;
        public ICommand AddNewColorCommand
        {
            get
            {
                if (addNewColorCommand == null)
                    addNewColorCommand = new RelayCommand(AddNewColor, o => !String.IsNullOrEmpty(NewColorName) && !String.IsNullOrEmpty(NewColorSaturation));
                return addNewColorCommand;
            }
        }

        private void AddNewColor(object color)
        {
            ViewModel vm = color as ViewModel;

            baseColors.Add(new ColorBase(vm.NewColorName, Int32.Parse(vm.NewColorSaturation)));
        }
        public string NewColorName { get; set; }
        public string NewColorSaturation { get; set; }
        #endregion

        #region dodawanie nowej farbki
        public string NewMixedPaintName { get; set; }
        private ObservableCollection<ColorBase> newIgredients;
        public ObservableCollection<ColorBase> NewIgredients { get { return newIgredients; } }
        private ColorBase selectedColorToAdd;
        public ColorBase SelectedColorToAdd
        {
            get { return selectedColorToAdd; }
            set
            {
                selectedColorToAdd = value;
                OnPropertyChanged(nameof(SelectedColorToAdd));
            }
        }
        private ICommand addSelectedColorToNewMixedPaint;
        public ICommand AddSelectedColorToNewMixedPaint
        {
            get
            {
                if (addSelectedColorToNewMixedPaint == null)
                    addSelectedColorToNewMixedPaint = new RelayCommand(AddNewIgredientToNewMixedPaint, o => selectedColorToAdd != null);
                return addSelectedColorToNewMixedPaint;
            }
        }
        private void AddNewIgredientToNewMixedPaint(object o)
        {
            if (SelectedColorToAdd != null)
                newIgredients.Add(selectedColorToAdd);
        }

        private ICommand addNewMixedPaintToListCommand;
        public ICommand AddNewMixedPaintToListCommand
        {
            get
            {
                if (addNewMixedPaintToListCommand == null)
                    addNewMixedPaintToListCommand = new RelayCommand(AddNewMixedPaintToList, o => !String.IsNullOrEmpty(NewMixedPaintName) && newIgredients.Count > 0);
                return addNewMixedPaintToListCommand;
            }
        }
        private void AddNewMixedPaintToList(object o)
        {
            if (o != null)
            {
                ViewModel vm = o as ViewModel;

                MixedPaint mixed = new MixedPaint(vm.NewMixedPaintName);
                foreach (var item in newIgredients)
                    mixed.AddIngredient(item);

                mixedPaints.Add(mixed);
            }
            comparisonMatrix.Clear();

            foreach (var item in baseColors)
            {
                MatrixLine ml = new MatrixLine(item);
                foreach (var item2 in mixedPaints)
                {
                    ml.AddToMatrix(item2);
                }
                comparisonMatrix.Add(ml);
            }
            
        }
        #endregion
    }
}
