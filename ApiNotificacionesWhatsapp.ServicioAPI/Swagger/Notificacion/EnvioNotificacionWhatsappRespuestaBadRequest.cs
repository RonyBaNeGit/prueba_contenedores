namespace CPM.TellerCentral.Aplicacion.Swagger.EstacionesTrabajo.Post
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta la operación POST - 400 - BadRequest.
    /// Fecha de creación: 05/01/2024.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class EnvioNotificacionWhatsappRespuestaBadRequest : EjemploSwaggerBase, IMultipleExamplesProvider<RespuestaDTO<string[]>>
    {
        #region Métodos Públicos

        /// <summary>
        /// Proporciona ejemplos de petición para los diferentes casos de uso.
        /// </summary>
        /// <returns>Lista de ejemplos.</returns>
        public IEnumerable<SwaggerExample<RespuestaDTO<string[]>>> GetExamples()
        {
            yield return CrearRespuesta(
            nombre: "Envío de notificación simple de WhatsApp - Estructura petición incorrecta",
            resumen: "Encabezados o cuerpo de petición incorrectos",
            descripcion: "Los encabezados o cuerpo de petición no cumplen las reglas de estructura básicas:\n\n1. Campos requeridos \n2. El tipo de dato no corresponde \n3. Rangos mínimos y máximos \n4. Longitud minima, máxima o fija \n5. Formato",
            valor: RespuestaDTOHelper.RespuestaInvalida(
                this.GenerarIdTransaccion(),
                RespuestaDTOHelper.MensajePeticionIncorrecta,
                new string[] { "El Id Canal debe ser mayor a 0.", "El número de socio es obligatorio y no puede estar vacío.", "El número de socio debe tener entre 1 y 10 dígitos.", "El número de teléfono es obligatorio y debe tener exactamente 10 dígitos.", "El identificador de plantilla es obligatorio y no puede estar vacío.", "Los parámetros de la plantilla son obligatorios." }));
        }

        #endregion
    }
}
