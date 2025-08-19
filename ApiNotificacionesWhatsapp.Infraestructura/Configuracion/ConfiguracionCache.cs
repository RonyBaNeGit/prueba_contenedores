namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion
{
    /// <summary>
    /// Propósito: Contiene configuración relacionada a la administración de cache.
    /// Fecha de creación: 26/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ConfiguracionCache
    {
        #region Constantes

        /// <summary>
        /// Nombre de la sección en la configuración.
        /// </summary>
        public const string Seccion = nameof(ConfiguracionCache);

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConfiguracionCache"/>.
        /// </summary>
        public ConfiguracionCache()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Periodo de tiempo (Minutos) para almacenar en cache la información.
        /// </summary>
        public int ConfiguracionTiempoCache { get; set; }

        #endregion

    }
}
