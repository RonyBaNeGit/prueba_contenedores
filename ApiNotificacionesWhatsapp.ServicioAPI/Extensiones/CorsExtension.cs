namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Extensiones
{
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Configuracion.Cors;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Propósito: Registra y configura la colección de servicios para la habilitación de CORS.
    /// Fecha de creación: 04/10/2023.
    /// Creador: Juan Carlos Miranda Méndez (MIMJ22037).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class CorsExtension
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Registra las políticas para el enrutamiento de puntos de conexión (CORS).
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <param name="config">Representa un conjunto de propiedades de configuración de aplicación de clave/valor.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarCORS(this IServiceCollection services, IConfiguration config)
        {
            IConfigurationSection seccion = config.GetSection(ConfiguracionCORS.Seccion);
            ConfiguracionCORS configuracion = seccion.Get<ConfiguracionCORS>() ?? new ConfiguracionCORS();
            services.Configure<ConfiguracionCORS>(seccion);
            services.AddAuthorization();
            services.AddSingleton(configuracion);
            if (configuracion.PermitirCualquierOrigen)
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder
                        .SetIsOriginAllowed(origen => new Uri(origen).Host == "localhost")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });

                return services;
            }

            foreach (Politica politica in configuracion.Politicas)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(politica.Nombre, builder =>
                    {
                        builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins(politica.Origenes.ToArray()).
                        SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
                });
            }

            return services;
        }

        /// <summary>
        /// Configura el uso de las políticas previamente registrar para la habilitación y uso de CORS.
        /// </summary>
        /// <param name="app">Un constructor de aplicaciones y servicios web.</param>
        /// <returns>La aplicación web utilizada para configurar la canalización HTTP y las rutas.</returns>
        public static WebApplication ConfigurarCORS(this WebApplication app)
        {
            ConfiguracionCORS configuracionCORS = app.Services.GetRequiredService<IOptions<ConfiguracionCORS>>().Value!;
            if (configuracionCORS.PermitirCualquierOrigen)
            {
                app.UseCors();
                return app;
            }

            foreach (Politica politica in configuracionCORS.Politicas)
            {
                app.UseCors(politica.Nombre);
            }

            return app;
        }

        #endregion
    }
}
