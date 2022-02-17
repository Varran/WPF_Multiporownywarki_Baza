using System;
using System.Collections.Generic;

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

        private object _value;

        protected EventHandler _NotifyChange;
        public event EventHandler NotifyChange
        {
            add { _NotifyChange += value; }
            remove { _NotifyChange -= value; }
        }

        public object Value { 
            get { return _value; } 
            set { _value = value; _NotifyChange?.Invoke(this, EventArgs.Empty); } 
        }
    }
}