namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosOrquestacion
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;

    /// <summary>
    /// Propósito :Interfaz que define los métodos necesarios para realizar el envío de whatsapp simple.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public interface IOrquestadorServicioWhatsapp
    {
        #region Métodos

        /// <summary>
        /// Envía WhatsApp de forma individual.
        /// </summary>
        /// <param name="solicitud">sadsadd.</param>
        /// <returns>Representa una operación asincrona.</returns>
        Task<RespuestaDTO> EnviarWhatsAppIndividual(SolicitudDTO solicitud);
        #endregion
    }
}
