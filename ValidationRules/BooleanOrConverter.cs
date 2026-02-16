using System.Windows;
using System.Windows.Data;

namespace GenshinNamecardINICreator.ValidationRules
{
    public class BooleanOrConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object value in values)
            {
                if (value != null && value != DependencyProperty.UnsetValue)
                {
                    if ((bool)value == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
