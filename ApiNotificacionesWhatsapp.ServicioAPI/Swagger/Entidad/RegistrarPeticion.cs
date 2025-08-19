namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Entidad
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta correcta de la operación PATCH, PUT, POST 200 - OK.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class RegistrarPeticion : EjemploSwaggerBase, IMultipleExamplesProvider<EntidadDTO>
    {
        #region Métodos Públicos

        /// <summary>
        /// Método que genera los posibles peticiones y sus diferentes escenarios.
        /// </summary>
        /// <returns>lista con ejemplos para swagger.</returns>
        public IEnumerable<SwaggerExample<EntidadDTO>> GetExamples()
        {
            yield return CrearPeticion(
                nombre: "Registrar nuevo - Petición correcta",
                resumen: "Información correcta",
                descripcion: "Petición correcta para un nuevo registro",
                valor: new EntidadDTO()
                {
                    Id = GenerarIdTransaccion(),
                });
        }

        #endregion
    }
}
