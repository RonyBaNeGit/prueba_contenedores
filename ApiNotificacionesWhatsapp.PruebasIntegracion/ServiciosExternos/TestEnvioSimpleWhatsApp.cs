namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion.ServiciosExternos
{
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp.EnviarWhatsAppSimple.Respuesta;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Clase de pruebas integrales para consultar los parámetros correspondientes de una plantilla en específico.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestEnvioSimpleWhatsApp))]
    public class TestEnvioSimpleWhatsApp
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestEnvioSimpleWhatsApp"/>.
        /// </summary>
        public TestEnvioSimpleWhatsApp()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Caso de prueba para realizar el envío de una notificación de whatsapp haciendo la solicitud al servicio de Auronix.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioSimpleWhatsappAuronix()
        {
            IServicioMensajeriaWhatsAppSimpleTransacciones servicioMensajeriaWhatsAppSimpleTransacciones = CrearInstanciaRepositorioWhatsAppSimpleTransaccione();
            PeticionWhatsAppSimpleDTO peticionWhatsAppSimpleDTO = CrearInstanciaPeticionWhatsAppSimple();
            RespuestaServicioAuronixWASimpleDTO? resultado = await servicioMensajeriaWhatsAppSimpleTransacciones.EnviarWhatsAppSimple(peticionWhatsAppSimpleDTO);
            Assert.That(resultado, Is.Not.Null);
        }

        /// <summary>
        /// Caso de prueba incorrecto para realizar el envío de una notificación de whatsapp haciendo la solicitud al servicio de Auronix
        /// simulando el escenario donde la ruta al servicio de Auronix sea incorrecta.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioSimpleWhatsappAuronixRutaIncorrecta()
        {
            Infraestructura.Configuracion.ServiciosExternos serviciosExternos = ObtenerConfiguracionServiciosExternos();
            string url = serviciosExternos.UrlApiAuronixTransaccion;
            serviciosExternos.UrlApiAuronixTransaccion = "http://fakeUrl";
            IServicioMensajeriaWhatsAppSimpleTransacciones servicioMensajeriaWhatsAppSimpleTransacciones = CrearInstanciaRepositorioWhatsAppSimpleTransaccione();
            PeticionWhatsAppSimpleDTO peticionWhatsAppSimpleDTO = CrearInstanciaPeticionWhatsAppSimple();
            RespuestaServicioAuronixWASimpleDTO? resultado = await servicioMensajeriaWhatsAppSimpleTransacciones.EnviarWhatsAppSimple(peticionWhatsAppSimpleDTO);
            Assert.That(resultado, Is.Not.Null);    
        }

        /// <summary>
        /// Caso de prueba incorrecto para realizar el envío de una notificación de whatsapp haciendo la solicitud al servicio de Auronix
        /// simulando el escenario donde se envíe un objeto de petición incorrecto.
        /// </summary>
        /// <returns>Representa una operación asincrona.</returns>
        [Test]
        public async Task EnvioSimpleWhatsappAuronixObjetoIncorrecto()
        {
            IServicioMensajeriaWhatsAppSimpleTransacciones servicioMensajeriaWhatsAppSimpleTransacciones = CrearInstanciaRepositorioWhatsAppSimpleTransaccione();
            PeticionWhatsAppSimpleDTO peticionWhatsAppSimpleDTO = CrearInstanciaPeticionWhatsAppSimple();
            peticionWhatsAppSimpleDTO.ListaNegra = new List<string>()
            {
                "ff",
            };
            RespuestaServicioAuronixWASimpleDTO? resultado = await servicioMensajeriaWhatsAppSimpleTransacciones.EnviarWhatsAppSimple(peticionWhatsAppSimpleDTO);
            Assert.That(resultado, Is.Not.Null);
        }

        #endregion
    }
}
