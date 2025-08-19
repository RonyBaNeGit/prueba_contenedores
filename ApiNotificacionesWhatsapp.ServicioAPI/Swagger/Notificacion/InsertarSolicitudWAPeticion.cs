namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Notificacion
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Ejemplo de respuesta correcta de la operación POST 200 - OK, para envío de notificación.
    /// Fecha de creación: 18/06/2024.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class InsertarSolicitudWAPeticion : EjemploSwaggerBase, IMultipleExamplesProvider<SolicitudDTO>
    {
        #region Métodos Públicos

        /// <summary>
        /// Genera los posibles peticiones y sus diferentes escenarios.
        /// </summary>
        /// <returns>lista con ejemplos para swagger.</returns>
        public IEnumerable<SwaggerExample<SolicitudDTO>> GetExamples()
        {
            yield return this.CrearPeticion(
                nombre: "Envío de mensaje simple WhatsApp - Petición correcta",
                resumen: SolicitudCorrecta,
                descripcion: "Petición correcta para el envío de notificación de WhatsApp.",
                valor: new SolicitudDTO()
                {
                    FolioSolicitud = "1234567890",
                    NumeroSocio = "0000123456",
                    Telefono = "4771234567",
                    IdPlantilla = "string",
                    Parametros = new List<string>()
                    {
                        "Juan",
                        "Perez",
                        "10/12/2024",
                        "4771234567",
                    },
                });
        }

        #endregion
    }
}
