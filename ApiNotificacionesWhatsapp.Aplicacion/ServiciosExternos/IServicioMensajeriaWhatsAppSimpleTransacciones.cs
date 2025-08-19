namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp.EnviarWhatsAppSimple.Respuesta;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Peticion;

    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Respuesta;

    /// <summary>
    /// Propósito: Interfaz que define los métodos necesarios para la solicitud al servicio de Auronix.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public interface IServicioMensajeriaWhatsAppSimpleTransacciones
    {
        #region Métodos

        /// <summary>
        /// Realiza la petición de envío al servicio de Auronix para realizar un mensaje vía whatsapp.
        /// </summary>
        /// <param name="plantillaWhatsAppDTO">Solicitud de la petición a Auronix.</param>
        /// <returns>Representa una operación asíncrona.</returns>
        Task<RespuestaServicioAuronixWASimpleDTO?> EnviarWhatsAppSimple(PeticionWhatsAppSimpleDTO plantillaWhatsAppDTO);

        #endregion
    }
}
