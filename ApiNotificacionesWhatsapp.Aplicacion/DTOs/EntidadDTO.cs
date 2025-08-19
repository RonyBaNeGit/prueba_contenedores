namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs
{
    /// <summary>
    /// Describa el propósito para esta clase.
    /// </summary>
    /// <remarks>
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </remarks>
    public class EntidadDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EntidadDTO"/>.
        /// </summary>
        public EntidadDTO()
        {
            this.Id = Guid.NewGuid().ToString().ToUpper();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador del registro.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        #endregion
    }
}
