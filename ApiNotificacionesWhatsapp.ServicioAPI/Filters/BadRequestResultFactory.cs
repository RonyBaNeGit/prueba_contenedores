namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

    /// <summary>
    /// Propósito: Proporciona una estructura de respuesta estándar para el cliente que consume el servicio 
    /// cuándo la petición no cumple con la estructura esperada.
    /// Fecha de creación: 17/10/2023.
    /// Creador: Juan Carlos Miranda Méndez (MIMJ22037).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class BadRequestResultFactory : IFluentValidationAutoValidationResultFactory
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BadRequestResultFactory"/>.
        /// </summary>
        /// <param name="context">Un contexto para filtros de acción, específicamente Microsoft.AspNetCore.Mvc.Filters.IActionFilter.OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext) y Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter.OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters. ActionExecutingContext, Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate).</param>
        /// <param name="validationProblemDetails">Un Microsoft.AspNetCore.Mvc.ProblemDetails para errores de validación.</param>
        /// <returns>Define un contrato que representa el resultado de un método de acción.</returns>
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            string idTransaccion = context.HttpContext.TraceIdentifier;
            List<string> errores = new List<string>();
            foreach (KeyValuePair<string, string[]> error in validationProblemDetails!.Errors)
            {
                errores.Add($"{string.Join(",", error.Value)}");
            }

            RespuestaDTO<IEnumerable<string>> respuesta = RespuestaDTOHelper.RespuestaInvalida<IEnumerable<string>>(idTransaccion);
            respuesta.ObjetoInformacion = errores;
            return new BadRequestObjectResult(respuesta);
        }
    }
}
