namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.ServiciosExternos
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.Mensajeria.Auronix.Dominio;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Clase de pruebas para los métodos definidos en la interfaz <see cref="IServicioConfiguracionWhatsapp"/>.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestObtenerConfiguracion))]
    public class TestObtenerConfiguracion
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestObtenerConfiguracion"/>.
        /// </summary>
        public TestObtenerConfiguracion()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Caso de prueba para obtener la configuración necesaria para el envío de una notificación de whatsapp.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task ObtenerConfiguracionWA()
        {
            IServicioConfiguracionWhatsapp servicioConfiguracionWhatsapp = CrearInstanciaObtenerConfiguracion();
            ConfiguracionWhatsApp? configuracionWhatsApp = await servicioConfiguracionWhatsapp.ObtenerConfiguracionWhatsappAsync();
            Assert.That(configuracionWhatsApp, Is.Not.Null);
        }

        /// <summary>
        /// Simula un caso de prueba incorrecto obtener la configuración necesaria para el envío de una notificación de whatsapp, siendo el escenario que el procedimiento almacenado no exista o sea incorrecto.
        /// </summary>
        /// <returns>Representa una operación asíncrona.</returns>
        [TestCase]
        public async Task InsertarNotificacionSPIncorrecto()
        {
            ObtenerConfiguracionCadenasConexion().CadenaConexion = "CadenaConexionIncorrecta";
            IServicioConfiguracionWhatsapp servicioConfiguracionWhatsapp = CrearInstanciaObtenerConfiguracion();
            ConfiguracionWhatsApp? configuracionWhatsApp = await servicioConfiguracionWhatsapp.ObtenerConfiguracionWhatsappAsync();
            Assert.That(configuracionWhatsApp, Is.Null);
        }

        #endregion
    }
}
