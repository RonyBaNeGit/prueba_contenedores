namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp.EnviarWhatsAppSimple.Respuesta
{
    using System.Text.Json.Serialization;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp;

    /// <summary>
    /// Propósito:Clase con información de respuesta del servicio de Auronix para una solicitud.
    /// </summary>
    /// <remarks>
    /// Propósito:Clase con información de respuesta del servicio de Auronix para una solicitud.
    /// Fecha de creación: 14/05/2025 16:03:38.
    /// Creador: Ronaldo Barrrientos Negrete (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </remarks>
    public class RespuestaServicioAuronixWASimpleDTO : RespuestaErrorServicioAuronixDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RespuestaServicioAuronixWASimpleDTO"/>.
        /// </summary>
        public RespuestaServicioAuronixWASimpleDTO()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador de la transacción.
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string IdTransaccion { get; set; } = string.Empty;
        #endregion
    }
}