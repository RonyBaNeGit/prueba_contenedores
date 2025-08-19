namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger
{
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Clase base para la generación de ejemplos de respuesta Swagger.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class EjemploSwaggerBase
    {
        #region Constantes

        /// <summary>
        /// Indica que no se encontró información sobre el recurso solicitado.
        /// </summary>
        public const string MensajeNoExisteInformacion = "No se encontró información";

        /// <summary>
        /// Mensaje para resumen de ejemplo respuesta correcta.
        /// </summary>
        public const string SolicitudCorrecta = "Solicitud procesada correctamente";

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Genera un número entero aleatorio.
        /// </summary>
        /// <param name="maximo">Número máximo.</param>
        /// <returns>Número generado.</returns>
        public int GenerarNumeroAleatorio(int maximo = 99999999)
        {
            Random random = new Random();
            return random.Next();
        }

        /// <summary>
        /// Generar identificador único de transacción.
        /// </summary>
        /// <returns>Identificador único de transacción.</returns>
        public string GenerarIdTransaccion()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        #endregion

        #region Métodos Protegidos

        /// <summary>
        /// Generar ejemplo petición.
        /// </summary>
        /// <typeparam name="T">Tipo de petición.</typeparam>
        /// <param name="nombre">Nombre de la petición.</param>
        /// <param name="resumen">Descripción corta del ejemplo.</param>
        /// <param name="descripcion">Descripción de la petición.</param>
        /// <param name="valor">Valor de la petición.</param>
        /// <returns>Objeto de ejemplo de petición.</returns>
        protected SwaggerExample<T> CrearPeticion<T>(string nombre, string resumen, string descripcion, T valor) => SwaggerExample.Create(
                nombre,
                resumen,
                descripcion,
                valor);

        /// <summary>
        /// Generar ejemplo respuesta.
        /// </summary>
        /// <typeparam name="T">Tipo de petición.</typeparam>
        /// <param name="nombre">Nombre de la petición.</param>
        /// <param name="resumen">Descripción corta del ejemplo.</param>
        /// <param name="descripcion">Descripción de la petición.</param>
        /// <param name="valor">Valor de la petición.</param>
        /// <returns>Objeto de ejemplo de petición.</returns>
        protected SwaggerExample<T> CrearRespuesta<T>(string nombre, string resumen, string descripcion, T valor) => SwaggerExample.Create(
                nombre,
                resumen,
                descripcion,
                valor);

        #endregion
    }
}
