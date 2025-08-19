namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp
{
    using System.Text.Json.Serialization;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Peticion;

    /// <summary>
    /// Propósito: Clase que contiene la información de la petición que se envía al servicio de Auronix.
    /// </summary>
    /// <remarks>
    /// Propósito: Clase que contiene la información de la petición que se envía al servicio de Auronix.
    /// Fecha de creación: 14/05/2025 11:39:08.
    /// Creador: Ronaldo Barrientos Negrete (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </remarks>
    public class PeticionWhatsAppSimpleDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PeticionWhatsAppSimpleDTO"/>.
        /// </summary>
        public PeticionWhatsAppSimpleDTO()
        {
            Plantilla = new PlantillaDTO();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Número de teléfono del cual se envía el WhatsApp.
        /// </summary>
        [JsonPropertyName("channel")]
        public string Canal { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la transacción por parte del cliente.
        /// </summary>
        [JsonPropertyName("customerTransactionId")]
        public string IdTransaccionCliente { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de usuario quién realiza el envío de notificación.
        /// </summary>
        [JsonPropertyName("customerDestinationUserId")]
        public string IdUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Número de telefono del socio a quien se le enviará la notificación.
        /// </summary>
        [JsonPropertyName("destination")]
        public string TelefonoDestinatario { get; set; } = string.Empty;

        /// <summary>
        /// Contiene los datos propios de la plantilla.
        /// </summary>
        [JsonPropertyName("template")]
        public PlantillaDTO Plantilla { get; set; }

        /// <summary>
        /// Lista de metada enviada al realizarse el envío del template.
        /// </summary>
        [JsonPropertyName("messageMetadata")]
        public List<string> ListaMensajeDatosMeta { get; set; } = new List<string>();

        /// <summary>
        /// Descripción de la transacción.
        /// </summary>
        [JsonPropertyName("description")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Números de teléfono en lista negra.
        /// </summary>
        [JsonPropertyName("blackListIds")]
        public List<string> ListaNegra { get; set; } = new List<string>();

        /// <summary>
        /// Lista de metadatos de la transacción.
        /// </summary>
        [JsonPropertyName("transactionMetadata")]
        public List<string> ListaMetaDatos { get; set; } = new List<string>();

        #endregion
    }
}