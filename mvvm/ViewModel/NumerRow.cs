using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;


namespace mvvm
{
    public class NumerRow : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridRow row = value as DataGridRow;
            if (row.DataContext?.GetType().FullName == "MS.Internal.NameObject") return null;
            return row.GetIndex() + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}
