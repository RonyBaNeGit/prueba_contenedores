namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Configuracion.Cors
{
    /// <summary>
    /// Propósito: Parámetros de la sección CORS del appsettings.json.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ConfiguracionCORS
    {
        #region Constantes

        /// <summary>
        /// Nombre de la sección en el archivo de configuración.
        /// </summary>
        public const string Seccion = "CORS";

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConfiguracionCORS"/>.
        /// </summary>
        public ConfiguracionCORS()
        {
            Politicas = new List<Politica>();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si se permite el procesamiento de peticiones desde cualquier origen.
        /// </summary>
        public bool PermitirCualquierOrigen { get; set; } = true;

        /// <summary>
        /// Lista de politica para habilitar el enrutamiento de puntos de conexión.
        /// </summary>
        public List<Politica> Politicas { get; set; }

        #endregion
    }
}
