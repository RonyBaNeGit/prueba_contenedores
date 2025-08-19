namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters
{
    using System.Reflection;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Propósito: Excluye los campos que contienen el atributo SwaggerIgnore..
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class SwaggerSkipPropertyFilter : ISchemaFilter
    {
        #region Métodos Públicos

        /// <summary>
        /// Se recorren las propiedades de las clases y se ignorar las propiedades con el atributo SwaggerIgnore.
        /// </summary>
        /// <param name="schema">Schema OpenApi.</param>
        /// <param name="context">Contexto de filtro swagger.</param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var skipProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerIgnore>() != null);
            foreach (var skipProperty in skipProperties)
            {
                var propertyToSkip = schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, skipProperty.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyToSkip != null)
                {
                    schema.Properties.Remove(propertyToSkip);
                }
            }
        }

        #endregion
    }
}