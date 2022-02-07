namespace MatrixLib.Matrix
{
    /// <summary>
    /// Represents a header shown in the topmost slot in a column of a matrix.
    /// </summary>
    public class MatrixColumnHeaderItem : MatrixItemBase
    {
        public MatrixColumnHeaderItem(object columnHeader)
        {
            this.ColumnHeader = columnHeader;
        }

        public object ColumnHeader { get; private set; }
    }
}