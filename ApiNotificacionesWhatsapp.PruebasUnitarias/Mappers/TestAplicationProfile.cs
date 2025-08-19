namespace CPM.ApiNotificacionesWhatsapp.PruebasUnitarias.Mappers
{
    using AutoMapper;
    using NUnit.Framework;
    using static Testing;

    /// <summary>
    /// Propósito: Describa el propósito para esta clase.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [TestFixture]
    [Category(nameof(TestAplicationProfile))]
    public class TestAplicationProfile
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="TestAplicationProfile"/>.
        /// </summary>
        public TestAplicationProfile()
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Validar el mapeo de Modelo a DTO.
        /// </summary>
        [Test]
        public void MapearModeloADTOCorrecto()
        {
            IMapper mapper = CrearMapper();
            ////Modelo modelo = new Modelo();
            ////EntidadDTO dto = mapper.Map<Modelo, EntidadDTO>(modelo);
            ////Assert.That(dto, Is.Not.Null);
        }

        #endregion
    }
}
