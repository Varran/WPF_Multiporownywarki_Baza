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

    public class MatrycaViewModel : MatrixBase<ColorBase, MixedPaint>
    {
        public MatrycaViewModel(ObservableCollection<MixedPaint> farbki, ObservableCollection<ColorBase> kolory)
        {
            this.mixedPaints = farbki;
            this.baseColors = kolory;
        }


        private ObservableCollection<MixedPaint> mixedPaints;
        public override ObservableCollection<MixedPaint> GetColumnHeaderValues { get { return mixedPaints; } }


        private ObservableCollection<ColorBase> baseColors;
        public override ObservableCollection<ColorBase> GetRowHeaderValues { get { return baseColors; } }

        public override object GetCellValue(ColorBase rowHeaderValue, MixedPaint columnHeaderValue) { return columnHeaderValue.Ingredients.Contains(rowHeaderValue); }

        public ViewModel Viewmodel { get; internal set; }

        public new ReadOnlyCollection<MatrixItemBase> MatrixItems
        {
            get
            {
                //if (_matrixItems == null)
                base.MatrixItems = new ReadOnlyCollection<MatrixItemBase>(BuildMatrix());

                return base.MatrixItems;
            }
        }

        protected override List<MatrixItemBase> BuildMatrix()
        {
            List<MatrixItemBase> matrixItems = new List<MatrixItemBase>();

            // Get the column and row header values from the child class.
            List<ColorBase> columnHeaderValues = GetRowHeaderValues.ToList();
            List<MixedPaint> rowHeaderValues = GetColumnHeaderValues.ToList();

            base.CreateEmptyHeader(matrixItems);
            base.CreateColumnHeaders(matrixItems, rowHeaderValues);
            base.CreateRowHeaders(matrixItems, columnHeaderValues);
            CreateCells(matrixItems, columnHeaderValues, rowHeaderValues);

            return matrixItems;
        }

        protected new void CreateCells(List<MatrixItemBase> matrixItems, List<ColorBase> rowHeaderValues, List<MixedPaint> columnHeaderValues)
        {
            // Insert a cell item for each row/column intersection.
            for (int row = 1; row <= rowHeaderValues.Count; ++row)
            {
                ColorBase rowHeaderValue = rowHeaderValues[row - 1];

                for (int column = 1; column <= columnHeaderValues.Count; ++column)
                {
                    // Ask the child class for the cell's value.
                    object cellValue = this.GetCellValue(rowHeaderValue, columnHeaderValues[column - 1]);
                    MatrixCellItem cell = (new MatrixCellItem(cellValue)
                    {
                        GridRow = row,
                        GridColumn = column
                    });
                    cell.NotifyChange += Cell_Change;
                    matrixItems.Add(cell);
                }
            }
        }

        private void Cell_Change(object sender, EventArgs e)
        {
            MatrixCellItem cell = (MatrixCellItem)sender;
            ColorBase color = GetRowHeaderValues[cell.GridRow - 1];
            if (GetColumnHeaderValues[cell.GridColumn - 1].Ingredients.Contains(color))
            {
                GetColumnHeaderValues[cell.GridColumn - 1].Ingredients.Remove(color);
            }
            else
            {
                GetColumnHeaderValues[cell.GridColumn - 1].Ingredients.Add(color);
            }
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private MatrycaViewModel matryca;
        public MatrycaViewModel Matryca
        {
            get { return matryca; }
            set { matryca = value; matryca.Viewmodel = this;
                OnPropertyChanged(nameof(Matryca)); }
        }

        private ObservableCollection<MixedPaint> mixedPaints;

        private ObservableCollection<ColorBase> baseColors;


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

            Matryca = new MatrycaViewModel(mixedPaints, baseColors);
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
            NewColorSaturation = "";
            NewColorName = "";
            Matryca = new MatrycaViewModel(mixedPaints, baseColors);

            OnPropertyChanged(nameof(Matryca));
        }
        private string newColorName;
        public string NewColorName
        {
            get { return newColorName; }
            set { newColorName = value;
                OnPropertyChanged(nameof(NewColorName)); }
        }

        private string newColorSaturation;
        public string NewColorSaturation 
        {
            get { return newColorSaturation; }
            set { newColorSaturation = value;
                OnPropertyChanged(nameof(NewColorSaturation)); } 
        }
        #endregion

        #region dodawanie nowej farbki
        private string newMixedPaintNaame;
        public string NewMixedPaintName 
        { 
            get { return newMixedPaintNaame; }
            set { newMixedPaintNaame = value;
                OnPropertyChanged(nameof(NewMixedPaintName)); }
        }
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

            OnPropertyChanged(nameof(Matryca));
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
            newIgredients.Clear();
            NewMixedPaintName = "";
            matryca = new MatrycaViewModel(mixedPaints, baseColors);
            OnPropertyChanged(nameof(Matryca));
        }
        #endregion
    }


}
