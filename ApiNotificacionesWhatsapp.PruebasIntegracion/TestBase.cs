namespace CPM.ApiNotificacionesWhatsapp.PruebasIntegracion
{
    using NUnit.Framework;

    /// <summary>
    /// Propósito: Implementa los métodos que se llaman una vez antes y después de cada prueba.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Identifica un método que se llamará inmediatamente antes de ejecutar cada prueba.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// Identifica un método que se llamará inmediatamente después de ejecutar cada prueba. 
        /// Se garantiza que se llamará al método, incluso si se produce una excepción.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }
    }
}
