namespace CPM.ApiNotificacionesWhatsapp.PruebasUnitarias
{
    using AutoMapper;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Extensiones;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    /// <summary>
    /// Propósito: Implementa los métodos que se llaman una vez antes y después de ejecutar cualquier prueba secundaria.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [SetUpFixture]
    public class Testing
    {
        #region Variables

        /// <summary>
        /// Una fábrica para crear instancias de Microsoft.Extensions.DependencyInjection.IServiceScope,
        /// que se utiliza para crear servicios dentro de un alcance.
        /// </summary>
        private static IServiceScopeFactory? scopeFactory;

        #endregion

        #region Métodos Estáticos Públicos

        /// <summary>
        /// Crea una instancia del tipo <see cref="IMapper"/>.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="IMapper"/>.</returns>
        public static IMapper CrearMapper()
        {
            using var scope = scopeFactory!.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMapper>();
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método que se llama una vez para realizar la configuración antes de ejecutar cualquier prueba secundaria.
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            ServiceCollection services = new ServiceCollection();
            services.RegistrarMapeosAplicacion();
            services.RegistrarValidadoresAplicacion();
            scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        }

        #endregion
    }
}
