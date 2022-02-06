using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_multi_próby
{
    public class ColorBase 
    {
        public string Name { get; }   
        public int Saturation { get; private set; }  
        public ColorBase(string name, int saturation)
        {
            this.Name = name;
            this.Saturation = saturation;
        }

        public void ChangeSaturation(int newSaturation)
        {
            Saturation = newSaturation;
        }

        public override string ToString()
        {
            return $"ColorBase: {Saturation.ToString().PadLeft(4, ' ')} - '{Name}'";
        }
    }

    public class MixedPaint
    {
        public string PaintName { get; } 
        public List<ColorBase> Ingredients { get; }

        public MixedPaint(string name)
        {
            Ingredients = new List<ColorBase>();
            this.PaintName= name;
        }

        public MixedPaint AddIngredient(ColorBase color)
        {
            bool added = false;

            foreach (var item in Ingredients)
                if (item.Name == color.Name )
                {
                    item.ChangeSaturation(item.Saturation + color.Saturation);
                    added = true;
                }

            if (!added)
                Ingredients.Add(color);

            return this;
        }

        public override string ToString()
        {
            return $"{PaintName} ({Ingredients.Count})";
        }
    }

    public class MatrixLine
    {
        public ColorBase ColorIngredient { get; private set; }
        public Dictionary<string, bool> Matrix;

        public MatrixLine(ColorBase color)
        {
            Matrix = new Dictionary<string, bool>();
            this.ColorIngredient = color;
        }

        public void AddToMatrix(MixedPaint mixedPaint)
        {
            string paintName = mixedPaint.PaintName;
            bool doesItContainIgredient = mixedPaint.Ingredients.Any(o => (o.Name == ColorIngredient.Name &&
                                                                        o.Saturation == ColorIngredient.Saturation));
            Matrix.Add(paintName, doesItContainIgredient);
        }

        public override string ToString()
        {
            return $"{ColorIngredient.Name}";
        }
    }
}
