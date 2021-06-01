using System;
using System.Globalization;
using System.Security;
using System.Windows;
using System.Windows.Data;

namespace MemeFolder.Converters
{
    public class StrLenghtToVisibilityConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsFocused = false, IsLenghtNotZero = true;


            if (values[1] != null)
            {
                if ((string)parameter == "Password")
                {
                    if (values[1] is SecureString temp)
                    {
                        if (temp.Length == 0)
                            IsLenghtNotZero = false;
                        else IsLenghtNotZero = true;
                    }
                }
                else
                {
                    if (values[1] is string temp)
                    {
                        if (temp == "")
                            IsLenghtNotZero = false;
                        else IsLenghtNotZero = true;
                    }
                }
            }
            else return Visibility.Visible;

            if (values[0] != null)
            {
                if (values[0] is bool value)
                {
                    if (value == false)
                        IsFocused = false;
                    else IsFocused = true;
                }
            }
            else return Visibility.Visible;

            if (IsFocused == true || IsLenghtNotZero == true)
                return Visibility.Hidden;
            else return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
