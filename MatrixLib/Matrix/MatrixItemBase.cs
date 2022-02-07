﻿namespace MatrixLib.Matrix
{
    /// <summary>
    /// Represents an entity that can occupy a slot/cell in a matrix.
    /// </summary>
    public abstract class MatrixItemBase
    {
        public int GridRow { get; internal set; }
        public int GridColumn { get; internal set; }
    }
}