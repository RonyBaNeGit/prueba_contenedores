namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters
{
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Swagger;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Propósito: Establece los estatus HTTP de respuesta y los encabezados de petición requeridos
    /// para cada operación del servicio api que se visualizaran en el Swagger.
    /// Fecha de creación: 03/06/2024.
    /// Creador: Adrian Velazquez Rocha (VERA26024).
    /// Modificó: No aplica.
    /// Dependencias de conexiones e interfaces: IOperationFilter.
    /// </summary>
    public class SwaggerOperationFilter : IOperationFilter
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase  <see cref="SwaggerOperationFilter"/>.
        /// </summary>
        public SwaggerOperationFilter()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Aplica el filtro a la operación del servicio API.
        /// </summary>
        /// <param name="operation">Representa una operación de un servicio API.</param>
        /// <param name="context">Contexto de la operación.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            IEnumerable<KeyValuePair<string, OpenApiMediaType>> contents;
            operation.Parameters ??= new List<OpenApiParameter>();
            if (operation.RequestBody is not null)
            {
                contents = operation.RequestBody.Content.Where(c => c.Key != "application/json");
                foreach (var item in contents)
                {
                    operation.RequestBody.Content.Remove(item);
                }
            }

            if (operation.Responses == null)
            {
                return;
            }

            operation.Responses.Add("400", this.CrearOpenApiResponse("application/json", "Petición incorrecta"));
            operation.Responses.Add("401", this.CrearOpenApiResponse("text/plain", "No autorizado"));
            operation.Responses.Add("500", this.CrearOpenApiResponse("application/json", "Error interno del servidor"));

            // Ejemplo de valores para encabezados de petición.
            OpenApiParameter? parametro = operation.Parameters.FirstOrDefault(x => x.Name == "IdCanal");
            if (parametro is not null)
            {
                parametro.Example = new OpenApiString("6");
            }

            parametro = operation.Parameters.FirstOrDefault(x => x.Name == "NombreAplicacion");
            if (parametro is not null)
            {
                parametro.Example = new OpenApiString("Swagger");
            }

            parametro = operation.Parameters.FirstOrDefault(x => x.Name == "IdSucursal");
            if (parametro is not null)
            {
                parametro.Examples.Add("Identificador sucursal correcto", this.GetOpenApiExample("1"));
                parametro.Examples.Add("Identificador sucursal incorrecto", this.GetOpenApiExample("0"));
            }

            parametro = operation.Parameters.FirstOrDefault(x => x.Name == "IdBanco");
            if (parametro is not null)
            {
                parametro.Example = new OpenApiString("0");
            }

            parametro = operation.Parameters.FirstOrDefault(x => x.Name == "FechaInicial");
            if (parametro is not null)
            {
                parametro.Example = new OpenApiString(DateTime.Now.Date.AddMonths(-1).ToString("yyyy/MM/dd"));
            }

            parametro = operation.Parameters.FirstOrDefault(x => x.Name == "FechaFinal");
            if (parametro is not null)
            {
                parametro.Example = new OpenApiString(DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Crea un objeto de respuesta para Open API.
        /// </summary>
        /// <param name="tipo">Tipo de respuesta.</param>
        /// <param name="descripcion">Descripción del objeto de respuesta.</param>
        /// <returns>Objeto de respuesta JSON.</returns>
        private OpenApiResponse CrearOpenApiResponse(string tipo, string descripcion)
        {
            return new OpenApiResponse()
            {
                Description = descripcion,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        tipo,
                        new OpenApiMediaType()
                        {
                            Schema = new OpenApiSchema(),
                            Example = new OpenApiObject(),
                        }
                    },
                },
            };
        }

        /// <summary>
        /// Inicializa una copia del objeto <see cref="OpenApiExample"/>.
        /// </summary>
        /// <param name="valor">Valor de ejemplo.</param>
        /// <returns>Un objeto que representa un objeto de la clase <see cref="OpenApiExample"/>.</returns>
        private OpenApiExample GetOpenApiExample(string valor)
        {
            return new OpenApiExample()
            {
                Value = new OpenApiString(valor),
            };
        }

        #endregion
    }
}
