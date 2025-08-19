namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Entidad
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta correcta de la operación GET.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ConsultarRegistrosRespuestaOK : EjemploSwaggerBase, IMultipleExamplesProvider<RespuestaDTO<List<EntidadDTO>>>
    {
        #region Métodos Públicos

        /// <summary>
        /// Método que genera los posibles peticiones y sus diferentes escenarios.
        /// </summary>
        /// <returns>lista con ejemplos para swagger.</returns>
        public IEnumerable<SwaggerExample<RespuestaDTO<List<EntidadDTO>>>> GetExamples()
        {
            yield return this.CrearRespuesta(
                nombre: "GetCorrecto",
                resumen: "Solicitud procesada correctamente",
                descripcion: "Solicitud procesada correctamente",
                valor: RespuestaDTOHelper.RespuestaCorrecta(
                    new List<EntidadDTO>()
                    {
                        new EntidadDTO(),
                        new EntidadDTO(),
                    },
                    this.GenerarIdTransaccion()));
        }

        #endregion
    }
}
