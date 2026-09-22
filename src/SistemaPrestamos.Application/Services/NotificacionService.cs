using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.Application.Services;

public class NotificacionService : INotificacionService
{
    private readonly IDispositivoRepository _dispositivoRepository;
    private readonly object _bloqueo = new();
    private bool _inicializado;

    public NotificacionService(
        IDispositivoRepository dispositivoRepository,
        IConfiguration configuration)
    {
        _dispositivoRepository = dispositivoRepository;

        var ruta = configuration["Firebase:ServiceAccountPath"];
        var proyecto = configuration["Firebase:ProjectId"];

        if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
        {
            throw new InvalidOperationException(
                "Firebase:ServiceAccountPath no configurado o archivo inexistente.");
        }

        lock (_bloqueo)
        {
            if (!_inicializado)
            {
                if (FirebaseApp.DefaultInstance is null)
                {
                    var credential = CredentialFactory
                        .FromFile<ServiceAccountCredential>(ruta)
                        .ToGoogleCredential();

                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = credential,
                        ProjectId = proyecto
                    });
                }

                _inicializado = true;
            }
        }
    }

    public async Task<int> EnviarAUsuarioAsync(
        Guid usuarioId,
        string titulo,
        string cuerpo)
    {
        var dispositivos = await _dispositivoRepository
            .ObtenerPorUsuarioAsync(usuarioId);

        var enviados = 0;

        foreach (var dispositivo in dispositivos)
        {
            try
            {
                await EnviarATokenAsync(
                    dispositivo.Token,
                    titulo,
                    cuerpo);

                enviados++;
            }
            catch (FirebaseAdmin.Messaging.FirebaseMessagingException ex)
                when (ex.MessagingErrorCode
                    == FirebaseAdmin.Messaging.MessagingErrorCode.Unregistered)
            {
                // Token muerto (app desinstalada): se elimina
                // para no intentarlo nunca más.
                await _dispositivoRepository.EliminarAsync(dispositivo);
                await _dispositivoRepository.GuardarCambiosAsync();
            }
            catch
            {
                // Otro error: se omite sin tumbar el resto.
            }
        }

        return enviados;
    }

    public async Task EnviarATokenAsync(
        string token,
        string titulo,
        string cuerpo)
    {
        var mensaje = new Message
        {
            // Message.Token está obsoleto (sugiere Fid), pero seguimos
            // usando registration tokens FCM: migrar a Fid implicaría
            // cambiar el flujo de tokens en el móvil. Pendiente futuro.
#pragma warning disable CS0618
            Token = token,
#pragma warning restore CS0618
            Notification = new Notification
            {
                Title = titulo,
                Body = cuerpo
            },
            Android = new AndroidConfig
            {
                Notification = new AndroidNotification
                {
                    ChannelId = "general"
                }
            }
        };

        await FirebaseMessaging.DefaultInstance.SendAsync(mensaje);
    }
}
