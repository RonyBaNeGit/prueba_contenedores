namespace CPM.ApiNotificacionesWhatsapp.Infraestructura.Constantes
{
    /// <summary>
    /// Propósito: Describa el propósito para esta clase.
    /// Fecha de creación: 14/05/2025 11:17:44.
    /// Creador: Ronaldo Barrientos Negrete (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ServicioConstantes
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ServicioConstantes"/>.
        /// </summary>
        public ServicioConstantes()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Nombre del encabezado que contendrá la llave de seguridad para autenticación al servicio de Auronix.
        /// </summary>
        public static string ApiKey { get; } = "apikey";

        /// <summary>
        /// Controlador que proporciona los métodos para el envío de notificaciones masivas por WhatsApp.
        /// </summary>
        public static string EnvioSimpleController { get; } = "transactions";

        /// <summary>
        /// Número máximo de notificaciones por petición al servicio de Auronix.
        /// </summary>
        public static int NotificacionesPeticion { get; } = 4700;
        #endregion

    }
}