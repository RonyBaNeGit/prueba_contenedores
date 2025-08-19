namespace CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion
{
    /// <summary>
    /// Propósito: Parámetros de la sección ConnectionStrings del appsettings.json.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class CadenasConexion
    {
        #region Constantes

        /// <summary>
        /// Nombre de la sección en la configuración.
        /// </summary>
        public const string Seccion = "ConnectionStrings";

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CadenasConexion"/>.
        /// </summary>
        public CadenasConexion()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Cadena de conexión a la base de datos.
        /// </summary>
        public string CadenaConexion { get; set; } = string.Empty;

        #endregion
    }
}
