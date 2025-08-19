namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Entidad
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta correcta de la operación GET/{id}.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ConsultarRegistroRespuestaOK : EjemploSwaggerBase, IMultipleExamplesProvider<RespuestaDTO<EntidadDTO>>
    {
        #region Métodos Públicos

        /// <summary>
        /// Método que genera los posibles peticiones y sus diferentes escenarios.
        /// </summary>
        /// <returns>lista con ejemplos para swagger.</returns>
        public IEnumerable<SwaggerExample<RespuestaDTO<EntidadDTO>>> GetExamples()
        {
            yield return this.CrearRespuesta(
                nombre: "Consultar detalle - Respuesta correcta",
                resumen: "Solicitud procesada correctamente",
                descripcion: "Solicitud procesada correctamente",
                valor: RespuestaDTOHelper.RespuestaCorrecta(new EntidadDTO(), this.GenerarIdTransaccion()));
        }

        #endregion
    }
}
