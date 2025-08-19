namespace CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion
{
    /// <summary>
    /// Propósito: Lista de procedimientos almacenados.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ProcedimientosAlmacenados
    {
        #region Constantes

        /// <summary>
        /// Nombre de la sección en la configuración.
        /// </summary>
        public const string Seccion = nameof(ProcedimientosAlmacenados);

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ProcedimientosAlmacenados"/>.
        /// </summary>
        public ProcedimientosAlmacenados()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Límite de registros de notificaciones pendientes de procesar.
        /// </summary>
        public int LimiteRegistros { get; set; }

        /// <summary>
        /// Procedimiento para obtener la información de los parámetros relacionados a una plantilla.
        /// </summary>
        public string SPConsultarParametrosPlantilla { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la plantilla para enviar mensajes de WhatsApp.
        /// </summary>
        public string SPConsultarPlantillasWhastapp { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la configuración para la integración de WhatsApp.
        /// </summary>
        public string SPConsultarConfiguracionWhastapp { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la notificación para un evento en WhatsApp.
        /// </summary>
        public string SPInsertarNotificacionWAEvento { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el parámetro de la notificación para un evento en WhatsApp.
        /// </summary>
        public string SPInsertarNotificacionWAEventoParametro { get; set; } = string.Empty;
        #endregion
    }
}