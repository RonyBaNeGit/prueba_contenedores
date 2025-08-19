namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger
{
    using System.Collections.Generic;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta para una petición incorrecta 400 Bad Request.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class RespuestaBadRequest : EjemploSwaggerBase, IMultipleExamplesProvider<RespuestaDTO<string[]>>
    {
        #region Métodos Públicos

        /// <summary>
        /// Proporciona ejemplos de petición para los diferentes casos de uso.
        /// </summary>
        /// <returns>Lista de ejemplos.</returns>
        public IEnumerable<SwaggerExample<RespuestaDTO<string[]>>> GetExamples()
        {
            yield return this.CrearRespuesta(
            nombre: "Petición incorrecta",
            resumen: "Encabezados o cuerpo de petición incorrectos",
            descripcion: "Los encabezados o cuerpo de petición no cumplen las reglas de estructura básicas:\n\n1. Campos requeridos \n2. El tipo de dato no corresponde \n3. Rangos mínimos y máximos \n4. Longitud minima, máxima o fija \n5. Formato",
            valor: RespuestaDTOHelper.RespuestaInvalida(
                this.GenerarIdTransaccion(),
                RespuestaDTOHelper.MensajePeticionIncorrecta,
                new string[] { "Id Canal debe ser mayor a 0", "Nombre Aplicacion es requerido" }));
        }

        #endregion
    }
}
