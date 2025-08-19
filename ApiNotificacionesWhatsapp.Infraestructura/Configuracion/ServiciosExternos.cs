namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.Configuracion
{
    using CPM.Extensiones.Configuracion.Core.Configuracion;

    /// <summary>
    /// Propósito: Parámetros de la sección ServiciosExternos en la Configuración que contiene el listado url de servicios externos y tokens
    /// definidos en el appsettings o variables de entorno del sistema.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ServiciosExternos : ServiciosExternosBase
    {
        #region Constantes

        /// <summary>
        /// Nombre de la sección en el archivo de configuración.
        /// </summary>
        public const string Seccion = nameof(ServiciosExternos);

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ServiciosExternos"/>.
        /// </summary>
        public ServiciosExternos()
            : base()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// URL base del Servicio de Mensajeria.
        /// </summary>
        public string UrlApiAuronixTransaccion { get; set; } = string.Empty;

        /// <summary>
        /// URL base del servicio de Auronix para consultas. 
        /// </summary>
        public string UrlApiAuronixConsultas { get; set; } = string.Empty;

        /// <summary>
        /// Llave de seguridad para autenticación del servicio Auronix.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        #endregion
    }
}
