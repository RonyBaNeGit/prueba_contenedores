namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Constantes
{
    /// <summary>
    /// Propósito: Proporciona valores constantes para el proceso de envío de notificaciones.
    /// Fecha de creación: 24/10/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class WhatsApp
    {
        #region Propiedades

        /// <summary>
        /// Número máximo de intentos permitidos para enviar notificaciones a Calixta cuando ocurre un error.
        /// </summary>
        public static int IntentosPermitidos { get; } = 2;

        /// <summary>
        /// Descripción de error en encabezado de plantilla HTML.
        /// </summary>
        public static string ErrorEncabezado { get; } = "Ocurrio un error durante el envío de notificaciones con el servicio de envío de notificaciones de WhatsApp.";

        /// <summary>
        /// Descripción del evento en la plantilla HTML.
        /// </summary>
        public static string Evento { get; } = "Error envío notificaciones WhatsApp.";

        #endregion
    }
}