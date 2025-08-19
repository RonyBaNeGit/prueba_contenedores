namespace CPM.ApiNotificacionesWhatsapp.Dominio.Parametros
{
    /// <summary>
    /// Propósito: Información de relación entre un evento de notificacion WA y los parámetros utilizados en la transacción.
    /// Fecha de creación: 21/05/2025 9:36:06.
    /// Creador: Ronaldo Barrientos Negrete (banr25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class EventoParametro
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="EventoParametro"/>.
        /// </summary>
        public EventoParametro()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Identificador único de la transacción de la notificación e enviar a Calixta.
        /// </summary>
        public string IdTransaccionRegistro { get; set; } = string.Empty;

        /// <summary>
        /// Identificador único del parámetro.
        /// </summary>
        public int IdParametro { get; set; }

        /// <summary>
        /// Valor por default del parámetro.
        /// </summary>
        public string Valor { get; set; } = string.Empty;

        /// <summary>
        /// Indica el orden en el que cada parámetro se insertará en la plantilla de la notificación.
        /// </summary>
        public int Orden { get; set; }

        #endregion
    }
}