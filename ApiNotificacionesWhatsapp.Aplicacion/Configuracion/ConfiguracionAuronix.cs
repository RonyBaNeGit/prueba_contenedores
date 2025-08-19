namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion
{
    using CPM.Mensajeria.Auronix.Infraestructura.Configuracion;

    /// <summary>
    /// Propósito: Parámetros de configuración en base de datos para el consumo del servicio de mensajeria de Auronix.
    /// Fecha de creación: 04/11/2024.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ConfiguracionAuronix : ConfiguracionMensajeriaWhatsApp
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConfiguracionAuronix"/>.
        /// </summary>
        public ConfiguracionAuronix()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Línea telefónica de la cual se envían los WhatsApp.
        /// </summary>
        public string Canal { get; set; } = string.Empty;

        /// <summary>
        /// Indica si las notificaciones se distribuyen uniformemente o no.
        /// </summary>
        public bool DistribucionUniforme { get; set; }

        /// <summary>
        /// Indica si se incluyen los parámetros para envío programado o no.
        /// </summary>
        public bool EnvioEnLinea { get; set; }

        /// <summary>
        /// Límite máximo de notificaciones de WhatsApp por día hacia META.
        /// </summary>
        public short LimiteNotificacionesMeta { get; set; }

        #endregion
    }
}