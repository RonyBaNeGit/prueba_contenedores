namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Estructura de respuesta estándar para las operaciones del servicio.
    /// </summary>
    /// <summary>
    /// Propósito: Proporciona una estructura de respuesta estándar para los servicios de Caja Pop Mex.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que se regresará como objeto de información.</typeparam>
    public class RespuestaDTO<T> : RespuestaDTO
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
        /// Propiedad para cuando se requiera regresar una entidad de tipo T.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T ObjetoInformacion { get; set; } = default!;

        #endregion
    }
}