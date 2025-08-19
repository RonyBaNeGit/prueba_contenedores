namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Modelos
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Propósito: Encabezados de petición para los métodos GET y DELETE.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class HeadersDTO : HeaderBaseDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="HeadersDTO"/>.
        /// </summary>
        public HeadersDTO()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador del registro.
        /// </summary>
        [FromRoute]
        public string Id { get; set; } = string.Empty;

        #endregion
    }
}