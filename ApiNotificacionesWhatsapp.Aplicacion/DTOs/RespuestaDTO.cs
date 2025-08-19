namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Enumerados;

    /// <summary>
    /// Representa la estructura de respuesta estándar para las operaciones del servicio.
    /// </summary>
    /// <remarks>
    /// Propósito: Proporciona una estructura de respuesta estándar para los servicios.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </remarks>
    public class RespuestaDTO
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RespuestaDTO"/>.
        /// </summary>
        public RespuestaDTO()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Código de respuesta de la aplicación: 
        /// 0: Correcto. 
        /// 1: Petición incorrecta por parte del cliente.
        /// 2: Error interno de la aplicación.
        /// n: Error de la aplicación durante la ejecución de alguno de los proceso internos.
        /// </summary>
        public CodigoRespuesta Codigo { get; set; } = CodigoRespuesta.Correcto;

        /// <summary>
        /// Identificador único de la transacción.
        /// </summary>
        public string IdTransaccion { get; set; } = string.Empty;

        /// <summary>
        /// Describe el resultado de la petición.
        /// </summary>
        public string Mensaje { get; set; } = string.Empty;

        #endregion
    }
}
