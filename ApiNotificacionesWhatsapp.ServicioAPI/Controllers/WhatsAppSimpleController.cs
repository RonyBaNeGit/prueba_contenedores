namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Controllers
{
    using System.Threading.Tasks;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Enumerados;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosOrquestacion;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Modelos;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Entidad;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger.Notificacion;
    using CPM.AspNetCore.Api.Seguridad.Controllers;
    using CPM.TellerCentral.Aplicacion.Swagger.EstacionesTrabajo.Post;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// Propósito: Contiene las definiciones del controlador de WhatsAppSimple.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [Route("[controller]")]
    public class WhatsAppSimpleController : CPMController
    {
        #region Variables

        /// <summary>
        /// Proporciona los métodos para la implementar la lógica de orquestación a llamadas a servicios de terceros y bases de datos.
        /// </summary>
        private readonly IOrquestadorServicioWhatsapp orquestadorServicioWhatsapp;

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WhatsAppSimpleController"/>.
        /// </summary>
        /// <param name="orquestadorServicioWhatsapp">Proporciona los métodos para la implementar la lógica de orquestación a llamadas a servicios de terceros y bases de datos.</param>
        public WhatsAppSimpleController(IOrquestadorServicioWhatsapp orquestadorServicioWhatsapp)
        {
            this.orquestadorServicioWhatsapp = orquestadorServicioWhatsapp;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Envía un mensaje vía WhatsApp.
        /// </summary>
        /// <param name="headers">Encabezados de la solicitud.</param>
        /// <param name="solicitud">Datos de la solicitud para el envío del mensaje.</param>
        /// <returns>Una tarea que, al ser resuelta devuelve el resultado del proceso.</returns>
        [HttpPost("EnvioSimpleWhatsapp")]
        [ServiceFilter(typeof(ActionFilterLogger))]
        [SwaggerOperation(Summary = "Envío de un mensaje vía whatsapp.", OperationId = "Simple")]
        [SwaggerResponse(200, "Correcto", typeof(RespuestaDTO), "application/json")]
        [SwaggerRequestExample(typeof(RespuestaDTO), typeof(InsertarSolicitudWAPeticion))]
        [SwaggerResponseExample(200, typeof(RespuestaOK))]
        [SwaggerResponseExample(400, typeof(EnvioNotificacionWhatsappRespuestaBadRequest))]
        [SwaggerResponseExample(401, typeof(RespuestaUnauthorized))]
        [SwaggerResponseExample(500, typeof(RespuestaErrorInterno))]
        public async Task<ActionResult<RespuestaDTO>> Post([FromQuery] HeaderBaseDTO headers, [FromBody] SolicitudDTO solicitud)
        {
            RespuestaDTO respuesta = await this.orquestadorServicioWhatsapp.EnviarWhatsAppIndividual(solicitud);
            switch (respuesta.Codigo)
            {
                case CodigoRespuesta.Correcto:
                    return Ok(respuesta);
                case CodigoRespuesta.Invalido:
                    return BadRequest(respuesta);
                case CodigoRespuesta.ErrorInterno:
                default:
                    return StatusCode(500, respuesta);
            }
        }

        #endregion
    }
}
