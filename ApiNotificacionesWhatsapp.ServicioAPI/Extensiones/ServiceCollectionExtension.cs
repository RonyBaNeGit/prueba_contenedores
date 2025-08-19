namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Extensiones
{
    using System.Reflection;
    using System.Text.Json.Serialization;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Extensiones;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters;
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Helpers;
    using CPM.AspNetCore.Api.Seguridad.Controllers;
    using CPM.AspNetCore.Api.Seguridad.Middlewares;
    using CPM.AspNetCore.OpenApi.Extensiones;
    using CPM.AspNetCore.OpenApi.Interfaces;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Extensiones.Configuracion.Core.Configuracion;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

    /// <summary>
    /// Propósito: Registra la colección de servicios de la capa de presentación CPM.ApiNotificacionesWhatsapp.ServicioAPI.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Registra la lista de servicios de la capa Servicio API a la colección de servicios de la aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <param name="config">Representa un conjunto de propiedades de configuración de aplicación de clave/valor.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarServiciosAPI(this IServiceCollection services, IConfiguration config)
        {
            AplicacionBase configuracionAplicacion = config.GetSection(Aplicacion.Seccion).Get<AplicacionBase>()!;
            services.AddResponseCompression();
            services.AddHttpContextAccessor();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Registrar una instancia de la clase TrazabilidadDTO para cada petición.
            services.AddScoped(sp =>
            {
                IHttpContextAccessor httpContextAccesor = sp.GetService<IHttpContextAccessor>()!;
                TrazabilidadDTO parametros = DataCollectionHelper.DictionaryToObject<TrazabilidadDTO>(httpContextAccesor.HttpContext!.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
                parametros.IdTransaccion = httpContextAccesor.HttpContext!.TraceIdentifier.ToUpper() ?? string.Empty;
                string usuario = httpContextAccesor.HttpContext!.User!.Identity!.Name ?? string.Empty;
                parametros.Usuario = usuario.ToUpper();
                return parametros;
            });

            services.AddControllers(options =>
            {
                options.Conventions.Insert(0, new ModelRouteConvention(configuracionAplicacion.Prefijo));
            })
            .AddApplicationPart(typeof(SeguridadController).Assembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddAuthorization();

            // Registrar políticas CORS.
            services.RegistrarCORS(config);

            // Registrar filtro para inicio y termino de petición.
            services.AddScoped<ActionFilterLogger>();

            // Registrar Open Api Definition, Swagger UI, RapidDoc UI.
            services.AddEndpointsApiExplorer();
            services.AddSingleton<IOperationRulesFilter, OperationRulesFilter>();
            OpenApiInfo openApiInfo = OpenApiExtension.CrearDocumentoOpenApi(config);
            services.ConfigurarSwagger(config, openApiInfo);

            // Agregar Fluent Validation (Validación estructura de petición Header y Body).
            services.AddMvc();
            services.AddFluentValidationAutoValidation(config =>
            {
                // Habilitar la validación de parámetros vinculados desde fuentes de enlace `BindingSource.Query`.
                config.EnableQueryBindingSourceAutomaticValidation = true;

                // Habilite la validación de parámetros vinculados desde fuentes de enlace `BindingSource.Path`.
                config.EnablePathBindingSourceAutomaticValidation = true;

                // Reemplace la fábrica de resultados predeterminada con una implementación personalizada.
                config.OverrideDefaultResultFactoryWith<BadRequestResultFactory>();
            });
            services.AddFluentValidationClientsideAdapters();

            // Registrar validadores Fluent Validation.
            services.RegistrarValidadoresAplicacion();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Agregar reglas Fluent Validation al Swagger.
            services.AddFluentValidationRulesToSwagger();
            return services;
        }

        #endregion
    }
}
