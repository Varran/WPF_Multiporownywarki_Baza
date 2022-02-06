using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib.Matrix;

namespace WPF_multi_próby
{
    public class PaintMatrix : MatrixBase<MatrixLine, MixedPaint>
    {
        readonly MatrixLine[] matrixLines;
        readonly MixedPaint[] mixedPaints;
        readonly ColorBase[] colorBases;

        public PaintMatrix()
        {
            #region Tworzenie colorBases;
            ColorBase yellowA = new ColorBase("YellowA", 110);
            ColorBase yellowB = new ColorBase("YellowB", 175);
            ColorBase blueA = new ColorBase("BlueA", 77);
            ColorBase blueB = new ColorBase("BlueB", 135);
            ColorBase redA = new ColorBase("RedA", 95);
            ColorBase redB = new ColorBase("RedB", 225);
            ColorBase whiteA = new ColorBase("WhiteA", 200);

            colorBases = new ColorBase[7];
            colorBases[0] = yellowA;
            colorBases[1] = yellowB;
            colorBases[2] = blueA;
            colorBases[3] = blueB;
            colorBases[4] = redA;
            colorBases[5] = redB;
            colorBases[6] = whiteA;
            #endregion
            #region Tworzenie mixedPaints
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

            mixedPaints = new MixedPaint[5];
            mixedPaints[0] = violet;
            mixedPaints[1] = greenA;
            mixedPaints[2] = greenB;
            mixedPaints[3] = orangeA;
            mixedPaints[4] = orangeB;
            #endregion
            #region Tworzenie matrixLines

            matrixLines = new MatrixLine[7];
            for (int i = 0; i < colorBases.Length; i++)
            {
                MatrixLine line = new MatrixLine(colorBases[i]);
                foreach (var mixed in mixedPaints)
                    line.AddToMatrix(mixed);

                matrixLines[i] = line;
            }
            #endregion
        }
        protected override object GetCellValue(MatrixLine rowHeaderValue, MixedPaint columnHeaderValue)
        {
            return rowHeaderValue.Matrix[columnHeaderValue.PaintName];
        }

        protected override IEnumerable<MixedPaint> GetColumnHeaderValues()
        {
            return mixedPaints;
        }

        protected override IEnumerable<MatrixLine> GetRowHeaderValues()
        {
            return matrixLines;
        }
    }
}
