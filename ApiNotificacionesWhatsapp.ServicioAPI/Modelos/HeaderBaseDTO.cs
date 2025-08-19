namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Modelos
{
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Propósito: Encabezados de petición (Header, Query, Path).
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class HeaderBaseDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HeaderBaseDTO"/>.
        /// </summary>
        public HeaderBaseDTO()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador del canal que origina la solicitud.
        /// </summary>
        [FromHeader]
        public int IdCanal { get; set; }

        /// <summary>
        /// Nombre de la aplicación que realiza la solicitud.
        /// </summary>
        [FromHeader]
        public string NombreAplicacion { get; set; } = string.Empty;

        /// <summary>
        /// Identificador único de transacción.
        /// </summary>
        [FromHeader]
        [SwaggerIgnore]
        public string IdTransaccion { get; set; } = string.Empty;

        #endregion
    }
}
