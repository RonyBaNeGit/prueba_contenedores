namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Mappers
{
    using AutoMapper;

    /// <summary>
    /// Propósito: Registra el mapeo de entidades modelo a entidades DTO y viceversa de la capa de Aplicación.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class AplicacionProfile : Profile
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AplicacionProfile"/>.
        /// </summary>
        public AplicacionProfile()
        {
            ////CreateMap<EntidadDTO, Entidad>()
            ////.ForMember(destino => destino.Id, opt => opt.MapFrom(origen => origen.Id))
            ////.ReverseMap();
        }

        #endregion
    }
}
