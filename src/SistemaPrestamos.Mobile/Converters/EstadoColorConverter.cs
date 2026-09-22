using System.Globalization;

namespace SistemaPrestamos.Mobile.Converters;

public class EstadoFondoConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ((value as string) switch
        {
            "Activo" => Color.FromArgb("#DCFCE7"),
            "Moroso" => Color.FromArgb("#FEE2E2"),
            "En observación" => Color.FromArgb("#FEF3C7"),
            "Pendiente" => Color.FromArgb("#FEF3C7"),
            "Cancelado" => Color.FromArgb("#F3F4F6"),
            "Administrador" => Color.FromArgb("#EDE9FE"),
            "Cobrador" => Color.FromArgb("#DCFCE7"),
            _ => Color.FromArgb("#F3F4F6")
        });
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class EstadoTextoConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ((value as string) switch
        {
            "Activo" => Color.FromArgb("#16A34A"),
            "Moroso" => Color.FromArgb("#DC2626"),
            "En observación" => Color.FromArgb("#D97706"),
            "Pendiente" => Color.FromArgb("#D97706"),
            "Cancelado" => Color.FromArgb("#6B7280"),
            "Administrador" => Color.FromArgb("#512BD4"),
            "Cobrador" => Color.FromArgb("#16A34A"),
            _ => Color.FromArgb("#6B7280")
        });
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
