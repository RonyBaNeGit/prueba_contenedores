namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Extensiones
{
    using System.Reflection;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Configuracion;
    using CPM.AspNetCore.OpenApi.Extensiones;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Propósito: Configura la canalización HTTP (pipeline) y las rutas.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ApplicationBuilderExtension
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Configura el pipeline y rutas de la aplicación.
        /// </summary>
        /// <param name="builder">El Microsoft.AspNetCore.Builder.WebApplicationBuilder.</param>
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        /// <returns>La aplicación web utilizada para configurar la canalización HTTP y las rutas.</returns>
        public static WebApplication ConstruirAplicacion(this WebApplicationBuilder builder, ILogger logger)
        {
            WebApplication app = builder.Build();
            Aplicacion configuracionAplicacion = app.Services.GetRequiredService<IOptions<Aplicacion>>().Value!;
            if (configuracionAplicacion.ModoDesarrollo)
            {
                app.Use((context, next) =>
                {
                    context.Request.EnableBuffering();
                    return next();
                });

                app.UseDeveloperExceptionPage();
            }

            // Habilitar Open Api Definition, Swagger UI.
            app.HabilitarSwaggerUI();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseRouting();

            // Habilitar políticas CORS.
            app.ConfigurarCORS();
            app.UseAuthorization();
            app.MapControllers();
            string mensaje = $"{configuracionAplicacion.Titulo} v{configuracionAplicacion.Version} iniciado correctamente.";
            if (!configuracionAplicacion.ModoDesarrollo)
            {
                AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync(mensaje);
                });
            }

            logger.LogInformation(mensaje);
            return app;
        }

        #endregion
    }
}
