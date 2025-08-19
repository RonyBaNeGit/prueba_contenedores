namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Modelos;
    using CPM.AspNetCore.Api.Seguridad.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Propósito: Filtro para registro de inicio de petición y respuesta del servicio.
    /// Fecha de creación: 21/05/2024.
    /// Creador: Juan Carlos Miranda Méndez (MIMJ22037).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class ActionFilterLogger : IActionFilter
    {
        #region Variables

        /// <summary>
        /// Proporciona los métodos para el registro de trazabilidad.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Parámetros de la sección Aplicacion en la configuración.
        /// </summary>
        private readonly Aplicacion aplicacion;

        /// <summary>
        /// URL base.
        /// </summary>
        private string pathBase = string.Empty;

        /// <summary>
        /// Nombre del controlador.
        /// </summary>
        private string path = string.Empty;

        /// <summary>
        /// Método u operación del servicio.
        /// </summary>
        private string operation = string.Empty;

        /// <summary>
        /// Método HTTP.
        /// </summary>
        private string metodo = string.Empty;

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ActionFilterLogger"/>.
        /// </summary>
        /// <param name="logger">Proporciona los métodos para el registro de trazabilidad.</param>
        /// <param name="options">Parámetros de la sección Aplicacion en la configuración.</param>
        public ActionFilterLogger(ILogger<ActionFilterLogger> logger, IOptions<Aplicacion> options)
        {
            this.logger = logger;
            this.aplicacion = options.Value;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Se llama antes de que se ejecute la acción, después de que se complete el enlace del modelo.
        /// </summary>
        /// <param name="context">El Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            this.operation = $"{actionDescriptor.ControllerName}/{actionDescriptor.ActionName}";
            this.path = context.HttpContext.Request.Path;
            this.pathBase = context.HttpContext.Request.PathBase;
            this.metodo = context.HttpContext.Request.Method;
            string usuario = context!.HttpContext.User.Identity!.Name ?? string.Empty;
            HeaderBaseDTO headers = (HeaderBaseDTO)context.ActionArguments["headers"] !;
            if (string.IsNullOrEmpty(headers.IdTransaccion))
            {
                headers.IdTransaccion = context.HttpContext.TraceIdentifier.ToUpper();
            }

            this.logger.BeginScope(new { IdTransaccion = headers.IdTransaccion!, Usuario = usuario });
        }

        /// <summary>
        /// Se llama después de que se ejecuta la acción, antes del resultado de la acción.
        /// </summary>
        /// <param name="context">El Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Verificar si la ejecución resultó en un resultado de tipo ObjectResult (por ejemplo, JsonResult, OkObjectResult)
            if (context.Result is ObjectResult)
            {
                // Obtener la respuesta HTTP
                HttpResponse response = context.HttpContext.Response;

                // Obtener el código de estado HTTP
                int estatusHttp = response.StatusCode;

                // Obtener la frase de motivo (reason phrase)
                string mensaje = ReasonPhrases.GetReasonPhrase(estatusHttp);
                string codigoError = string.Empty;
                if ((context.Result as ObjectResult)?.Value is RespuestaDTO respuestaDTO)
                {
                    codigoError = respuestaDTO.Codigo.ToString();
                    mensaje = respuestaDTO.Mensaje;
                }

                this.logger.LogWarning($"Terminó petición de la firma {this.operation} con Estatus HTTP: {estatusHttp}. Codigo: {codigoError}. Mensaje: {mensaje}.");
            }
        }

        #endregion
    }
}
