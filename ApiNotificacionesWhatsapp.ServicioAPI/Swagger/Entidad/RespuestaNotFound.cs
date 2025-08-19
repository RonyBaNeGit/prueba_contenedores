namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Entidad
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta para la operación GET/{id} 200 Not Found.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class RespuestaNotFound : EjemploSwaggerBase, IMultipleExamplesProvider<RespuestaDTO>
    {
        #region Métodos Públicos

        /// <summary>
        /// Método que genera los posibles peticiones y sus diferentes escenarios.
        /// </summary>
        /// <returns>lista con ejemplos para swagger.</returns>
        public IEnumerable<SwaggerExample<RespuestaDTO>> GetExamples()
        {
            yield return this.CrearRespuesta(
                nombre: "No se encontró información",
                resumen: "El recurso solicitado no existe",
                descripcion: "La solicitud fue procesada correctamente pero no se encontró información del recurso solicitado.",
                valor: RespuestaDTOHelper.RespuestaInvalida(this.GenerarIdTransaccion(), MensajeNoExisteInformacion));
        }

        #endregion
    }
}