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
            set { matryca = value;
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

    public class MatrycaViewModel : MatrixBase<ColorBase, MixedPaint>
    {
        private ObservableCollection<MixedPaint> mixedPaints;

        private ObservableCollection<ColorBase> baseColors;
        public MatrycaViewModel(ObservableCollection<MixedPaint> farbki, ObservableCollection<ColorBase> kolory)
        {
            this.mixedPaints = farbki;
            this.baseColors = kolory;
        }
        public override ObservableCollection<MixedPaint> GetColumnHeaderValues { get { return mixedPaints; } }
        public override ObservableCollection<ColorBase> GetRowHeaderValues { get { return baseColors; } }
        public override object GetCellValue(ColorBase rowHeaderValue, MixedPaint columnHeaderValue) { return columnHeaderValue.Ingredients.Contains(rowHeaderValue); }
    }
}
