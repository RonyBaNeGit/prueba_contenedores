namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos
{
    using CPM.Mensajeria.Auronix.Dominio;

    /// <summary>
    /// Propósito: Interfaz que define los métodos necesarios para la consulta de configuración para envió de notificaciones de WhatsApp.
    /// Fecha de creación: 26/05/2025 10:41:14.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public interface IServicioConfiguracionWhatsapp
    {
        #region Métodos

        /// <summary>
        /// Obtiene la información de configuración para la solicitud de mensajeria de whatsapp.
        /// </summary>
        /// <returns>Objeto con la información de la configuración..</returns>
        Task<ConfiguracionWhatsApp?> ObtenerConfiguracionWhatsappAsync();
        #endregion
    }
}
