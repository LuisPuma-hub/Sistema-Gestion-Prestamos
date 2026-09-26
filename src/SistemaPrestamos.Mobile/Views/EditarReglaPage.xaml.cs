using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(ReglaId), "reglaId")]
public partial class EditarReglaPage : ContentPage
{
    private static readonly (string Clave, string Texto)[] EventosWhatsapp =
    [
        ("VenceHoy", "Vence hoy"),
        ("VenceManana", "Vence mañana"),
        ("MoraNueva", "Mora nueva"),
        ("MoraPersistente", "Mora persistente")
    ];

    private static readonly (string Clave, string Texto)[] EventosPush =
    [
        ("ResumenDiario", "Resumen diario"),
        ("MoraCobrador", "Mora al cobrador"),
        ("CobradoDia", "Cobrado del día"),
        ("PrestamoPorAprobar", "Préstamo por aprobar"),
        ("ResumenVencimientos", "Vencen hoy (resumen)")
    ];

    private static readonly string[] Plantillas =
    [
        "recordatorio_pago_v2",
        "aviso_mora",
        "confirmacion_pago",
        "prestamo_aprobado"
    ];

    private static readonly Color ColorActivo = Color.FromArgb("#15307A");
    private static readonly Color ColorInactivo = Color.FromArgb("#F3F4F6");
    private static readonly Color TextoActivo = Colors.White;
    private static readonly Color TextoInactivo = Color.FromArgb("#374151");

    private readonly NotificacionesService _notificacionesService;
    private readonly bool[] _dias = [true, true, true, true, true, true, true];
    private Guid? _id;

    public string ReglaId
    {
        set
        {
            if (Guid.TryParse(value, out var id))
            {
                _id = id;
            }
        }
    }

    public EditarReglaPage(
        NotificacionesService notificacionesService)
    {
        InitializeComponent();
        _notificacionesService = notificacionesService;

        foreach (var plantilla in Plantillas)
        {
            PlantillaPicker.Items.Add(plantilla);
        }

        RadioWhatsapp.IsChecked = true;
        CargarEventos(false);

        PintarDias();
    }

    private (string Clave, string Texto)[] EventosActuales()
    {
        return RadioPush.IsChecked ? EventosPush : EventosWhatsapp;
    }

    private void CargarEventos(bool esPush)
    {
        var lista = esPush ? EventosPush : EventosWhatsapp;

        EventoPicker.Items.Clear();

        foreach (var (_, texto) in lista)
        {
            EventoPicker.Items.Add(texto);
        }

        EventoPicker.SelectedIndex = -1;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        HoraLimaLabel.Text = "Hora actual en Lima: ...";

        try
        {
            var diagnostico = await _notificacionesService
                .ObtenerDiagnosticoAsync();

            HoraLimaLabel.Text = diagnostico is null
                ? $"Hora actual en Lima: {DateTime.UtcNow.AddHours(-5):HH:mm} (aprox.)"
                : $"Hora actual en Lima: {diagnostico.HoraLima} (servidor)";
        }
        catch
        {
            HoraLimaLabel.Text =
                $"Hora actual en Lima: {DateTime.UtcNow.AddHours(-5):HH:mm} (aprox.)";
        }

        if (!_id.HasValue)
        {
            return;
        }

        TituloLabel.Text = "Editar regla";
        EliminarButton.IsVisible = true;

        var regla = await _notificacionesService.ObtenerReglaAsync(
            _id.Value);

        if (regla is null)
        {
            MostrarError("La regla no existe.");
            return;
        }

        NombreEntry.Text = regla.Nombre;

        var esPush = regla.Canal == "Push";

        RadioPush.IsChecked = esPush;
        RadioWhatsapp.IsChecked = !esPush;

        CargarEventos(esPush);
        EventoPicker.SelectedIndex = IndiceDe(EventosActuales(), regla.Evento);

        var partes = regla.Hora.Split(':');

        if (partes.Length == 2 &&
            int.TryParse(partes[0], out var h) &&
            int.TryParse(partes[1], out var m))
        {
            HoraPicker.Time = new TimeSpan(h, m, 0);
        }

        for (var i = 0; i < 7; i++)
        {
            _dias[i] = (regla.DiasSemana & (1 << i)) != 0;
        }

        PintarDias();
        ActualizarPlantilla();

        if (!string.IsNullOrWhiteSpace(regla.Plantilla))
        {
            PlantillaPicker.SelectedIndex = Array.IndexOf(
                Plantillas,
                regla.Plantilla);
        }
    }

    private void OnDiaClicked(object? sender, EventArgs e)
    {
        if (sender is not Button boton)
        {
            return;
        }

        var indice = Grid.GetColumn(boton);

        _dias[indice] = !_dias[indice];

        PintarDias();
    }

    private void PintarDias()
    {
        var botones = new[]
        {
            Dia0, Dia1, Dia2, Dia3, Dia4, Dia5, Dia6
        };

        for (var i = 0; i < 7; i++)
        {
            botones[i].BackgroundColor = _dias[i] ? ColorActivo : ColorInactivo;
            botones[i].TextColor = _dias[i] ? TextoActivo : TextoInactivo;
            botones[i].CornerRadius = 10;
            botones[i].HeightRequest = 44;
            botones[i].FontAttributes = FontAttributes.Bold;
        }
    }

    private void OnCanalRadioChanged(object? sender, CheckedChangedEventArgs e)
    {
        CargarEventos(RadioPush.IsChecked);
        ActualizarPlantilla();
    }

    private void ActualizarPlantilla()
    {
        PlantillaCard.IsVisible = !RadioPush.IsChecked;
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (EventoPicker.SelectedIndex < 0)
        {
            MostrarError("Selecciona el evento.");
            return;
        }

        var eventos = EventosActuales();

        if (EventoPicker.SelectedIndex < 0 ||
            EventoPicker.SelectedIndex >= eventos.Length)
        {
            MostrarError("Selecciona el evento.");
            return;
        }

        var evento = eventos[EventoPicker.SelectedIndex].Clave;
        var canal = RadioPush.IsChecked ? "Push" : "Whatsapp";

        var dias = 0;

        for (var i = 0; i < 7; i++)
        {
            if (_dias[i])
            {
                dias |= 1 << i;
            }
        }

        if (dias == 0)
        {
            MostrarError("Selecciona al menos un día.");
            return;
        }

        var tiempo = HoraPicker.Time ?? TimeSpan.Zero;
        var hora = $"{tiempo.Hours:00}:{tiempo.Minutes:00}";
        var plantilla = PlantillaCard.IsVisible &&
            PlantillaPicker.SelectedIndex >= 0
            ? Plantillas[PlantillaPicker.SelectedIndex]
            : null;

        (bool Exito, string? Error) resultado;

        if (_id.HasValue)
        {
            resultado = await _notificacionesService.ActualizarReglaAsync(
                _id.Value,
                NombreEntry.Text?.Trim() ?? string.Empty,
                evento,
                canal,
                hora,
                dias,
                plantilla,
                true);
        }
        else
        {
            resultado = await _notificacionesService.CrearReglaAsync(
                NombreEntry.Text?.Trim() ?? string.Empty,
                evento,
                canal,
                hora,
                dias,
                plantilla);
        }

        if (!resultado.Exito)
        {
            MostrarError(resultado.Error ?? "No se pudo guardar.");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnEliminarClicked(object? sender, EventArgs e)
    {
        if (!_id.HasValue)
        {
            return;
        }

        var confirma = await DisplayAlertAsync(
            "Eliminar",
            "¿Eliminar esta regla?",
            "Sí",
            "No");

        if (!confirma)
        {
            return;
        }

        var (exito, error) = await _notificacionesService.EliminarReglaAsync(
            _id.Value);

        if (!exito)
        {
            MostrarError(error ?? "No se pudo eliminar.");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnVolverClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private static int IndiceDe(
        (string Clave, string Texto)[] lista,
        string clave)
    {
        for (var i = 0; i < lista.Length; i++)
        {
            if (lista[i].Clave == clave)
            {
                return i;
            }
        }

        return -1;
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }
}
