namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger
{
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta para una petición cuyo token de seguridad es incorrecto.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó: No aplica.
    /// Dependencias de conexiones e interfaces: IMultipleExamplesProvider.
    /// </summary>
    public class RespuestaUnauthorized : IMultipleExamplesProvider<string>
    {
        #region Métodos Públicos

        /// <summary>
        /// Proporciona ejemplos de respuesta para los diferentes casos de uso.
        /// </summary>
        /// <returns>Lista de ejemplos.</returns>
        public IEnumerable<SwaggerExample<string>> GetExamples()
        {
            yield return SwaggerExample.Create(
            name: "No autorizado",
            summary: "Credenciales de autenticación incorrectas",
            description: "El cliente no ha proporcionado un token de autenticación válido",
            value: "Credenciales de autenticación incorrectas.");
        }

        #endregion
    }
}
