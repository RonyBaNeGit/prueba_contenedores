namespace CPM.ApiNotificacionesWhatsapp.Dominio.Parametros
{
    /// <summary>
    /// Propósito: Información de la relación parámetro-plantilla.
    /// Fecha de creación: 12/05/2025 17:03:42.
    /// Creador: Ronaldo Barrientos Negrete (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class Parametro
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Parametro"/>.
        /// </summary>
        public Parametro()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador de la plantilla.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la plantilla.
        /// </summary>
        public string NombrePlantilla { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del parámetro.
        /// </summary>
        public int IdParametro { get; set; }

        /// <summary>
        /// Nombre del parámetro.
        /// </summary>
        public string NombreParametro { get; set; } = string.Empty;

        /// <summary>
        /// Orden del parámetro.
        /// </summary>
        public int Orden { get; set; }

        /// <summary>
        /// Valor del parámetro.
        /// </summary>
        public string? Valor { get; set; }

        #endregion
    }
}