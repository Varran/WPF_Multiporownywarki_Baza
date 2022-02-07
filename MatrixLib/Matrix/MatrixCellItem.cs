namespace MatrixLib.Matrix
{
    /// <summary>
    /// Represents a row-column intersection in a matrix.
    /// </summary>
    public class MatrixCellItem : MatrixItemBase
    {
        public MatrixCellItem(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }
    }
}