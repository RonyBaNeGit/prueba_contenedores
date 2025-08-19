namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.Helpers
{
    using System.Text;
    using System.Text.Json;

    /// <summary>
    /// Propósito: Describa el propósito para esta clase.
    /// Fecha de creación: 14/05/2025 11:21:49.
    /// Creador: CAPOME (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Crear una nueva instancia del tipo <see cref="StringContent"/> que contiene el cuerpo de petición como JSON. 
        /// </summary>
        /// <typeparam name="TRequest">El tipo de valor a serializar.</typeparam>
        /// <param name="contenido">El valor a serializar.</param>
        /// <returns>Proporciona contenido HTTP basado en una cadena.</returns>
        public static StringContent GetHttpJsonContent<TRequest>(TRequest contenido)
        {
            string json = string.Empty;
            if (contenido != null)
            {
                json = JsonSerializer.Serialize(contenido);
            }

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Procesa el cuerpo de respuesta de la solicitud y devuelve el valor serializado como JSON.
        /// </summary>
        /// <typeparam name="TResult">El tipo de valor de la respuesta.</typeparam>
        /// <param name="respuestaHttp">Representa un mensaje de respuesta HTTP que incluye el código de estado y los datos.</param>
        /// <returns>Una tarea que, al ser resulta devuelve una instancia de la clase del tipo {TResult} con el resultado de la petición.</returns>
        public static async Task<TResult> GetHttpResponseContentAsJson<TResult>(HttpResponseMessage respuestaHttp)
        {
            var contenido = await respuestaHttp.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(contenido))
            {
                return default!;
            }

            return JsonSerializer.Deserialize<TResult>(contenido)!;
        }
    }
}