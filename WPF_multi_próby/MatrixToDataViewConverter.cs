using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF_multi_próby
{
    public class MatrixToDataViewConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var myDataTable = new DataTable();
            var colums = values[0] as ObservableCollection<MixedPaint>;
            var rows = values[1] as ObservableCollection<MatrixLine>;
            var vals = values[2] as ObservableCollection<ObservableCollection<bool>>;

            myDataTable.Columns.Add("---");    

            if (colums != null)
                foreach (var value in colums)
                    myDataTable.Columns.Add(value.PaintName);

            int index = 0;

            if (rows != null)
                foreach (MatrixLine row in rows)
                {
                    var tmp = new string[1 + vals[index].Count()];
                    var tmpboolean = new bool[1 + vals[index].Count()];
                    vals[index].CopyTo(tmpboolean, 1);
                    tmp[0] = row.ColorIngredient.Name;
                    myDataTable.Rows.Add(tmp);
                    index++;
                }
            
            return myDataTable.DefaultView;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
