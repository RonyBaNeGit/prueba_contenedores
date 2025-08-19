namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta para la operación GET 500 Internal Error.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class RespuestaErrorInterno : EjemploSwaggerBase, IMultipleExamplesProvider<RespuestaDTO>
    {
        #region Métodos Públicos

        /// <summary>
        /// Método que genera los posibles peticiones y sus diferentes escenarios.
        /// </summary>
        /// <returns>Lista con ejemplos para swagger.</returns>
        public IEnumerable<SwaggerExample<RespuestaDTO>> GetExamples()
        {
            yield return CrearRespuesta(
                nombre: "RespuestaErrorInterno",
                resumen: RespuestaDTOHelper.MensajeError,
                descripcion: "Se produjo un error no controlado del lado del servidor.",
                valor: RespuestaDTOHelper.RespuestaErrorInterno(this.GenerarIdTransaccion()));
        }

        #endregion
    }
}