namespace CPM.ApiNotificacionesWhatsapp.Dominio.Extensiones
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Propósito: Registra la colección de servicios de la capa de Core\Dominio.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        #region Métodos Estáticos Públicos

        /// <summary>
        /// Agregar la lista de mapeos de entidades de Evento y entidades Modelo de la capa de dominio a la colección de servicios de la capa de aplicación.
        /// </summary>
        /// <param name="services">Especifica el contrato para una colección de descriptores de servicios.</param>
        /// <returns>Colección de descriptores de servicios.</returns>
        public static IServiceCollection RegistrarMapeosDominio(this IServiceCollection services)
        {
            ////services.AddAutoMapper(typeof(DominioProfile));
            return services;
        }

        #endregion
    }
}
