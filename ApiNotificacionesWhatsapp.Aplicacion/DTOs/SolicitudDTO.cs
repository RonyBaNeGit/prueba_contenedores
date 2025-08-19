namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs
{
    using System.Collections.Generic;

    /// <summary>
    /// Clase que contiene la información de la solicitud para envió de mensaje de Whatsapp simple.
    /// </summary>
    /// <remarks>
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó: -
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </remarks>
    public class SolicitudDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SolicitudDTO"/>.
        /// </summary>
        public SolicitudDTO()
        {
            Parametros = new List<string>();
        }

        #endregion

        /// <summary>
        /// Folio de la solicitud.
        /// </summary>
        public string? FolioSolicitud { get; set; } = string.Empty;

        /// <summary>
        /// Número de identificación del socio asociado a la solicitud.
        /// </summary>
        public string NumeroSocio { get; set; } = string.Empty;

        /// <summary>
        /// Número de teléfono relacionado con la solicitud.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la plantilla utilizada en la solicitud.
        /// </summary>
        public string IdPlantilla { get; set; } = string.Empty;

        /// <summary>
        /// Establece el listado de parámetros asociados a la plantilla.
        /// </summary>
        public List<string> Parametros { get; set; } 
    }
}
