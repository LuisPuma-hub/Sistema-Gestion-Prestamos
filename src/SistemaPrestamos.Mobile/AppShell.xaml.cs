namespace SistemaPrestamos.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(
            nameof(Views.RegistrarClientePage),
            typeof(Views.RegistrarClientePage));

        Routing.RegisterRoute(
            nameof(Views.DetalleClientePage),
            typeof(Views.DetalleClientePage));

        Routing.RegisterRoute(
            nameof(Views.RegistrarPrestamoPage),
            typeof(Views.RegistrarPrestamoPage));

        Routing.RegisterRoute(
            nameof(Views.DetallePrestamoPage),
            typeof(Views.DetallePrestamoPage));

        Routing.RegisterRoute(
            nameof(Views.RegistrarPagoPage),
            typeof(Views.RegistrarPagoPage));

        Routing.RegisterRoute(
            nameof(Views.DetallePagoPage),
            typeof(Views.DetallePagoPage));

        Routing.RegisterRoute(
            nameof(Views.DetalleMorosidadPage),
            typeof(Views.DetalleMorosidadPage));

        Routing.RegisterRoute(
            nameof(Views.HistorialWhatsappPage),
            typeof(Views.HistorialWhatsappPage));

        Routing.RegisterRoute(
            nameof(Views.PerfilPage),
            typeof(Views.PerfilPage));

        Routing.RegisterRoute(
            nameof(Views.UsuariosPage),
            typeof(Views.UsuariosPage));

        Routing.RegisterRoute(
            nameof(Views.RegistrarUsuarioPage),
            typeof(Views.RegistrarUsuarioPage));

        Routing.RegisterRoute(
            nameof(Views.NotificacionesPage),
            typeof(Views.NotificacionesPage));

        Routing.RegisterRoute(
            nameof(Views.EditarReglaPage),
            typeof(Views.EditarReglaPage));

        Routing.RegisterRoute(
            nameof(Views.HistorialEnviosPage),
            typeof(Views.HistorialEnviosPage));
    }
}
