using System.Text.Json;

namespace SistemaPrestamos.Mobile.Services;

/// <summary>
/// El backend responde errores como {"mensaje":"..."}.
/// Esto extrae el texto limpio y lo recorta para diálogos.
/// </summary>
public static class TextoError
{
    public static string Limpiar(string? error, string defecto = "Ocurrió un error.")
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return defecto;
        }

        var texto = error.Trim();

        if (texto.StartsWith("{"))
        {
            try
            {
                using var doc = JsonDocument.Parse(texto);

                if (doc.RootElement.TryGetProperty("mensaje", out var mensaje))
                {
                    var valor = mensaje.GetString();

                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        texto = valor.Trim();
                    }
                }
            }
            catch
            {
                // Si no es JSON válido, se muestra recortado.
            }
        }

        return texto.Length > 220
            ? texto.Substring(0, 220) + "..."
            : texto;
    }
}
