namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Validadores
{
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Modelos;
    using FluentValidation;

    /// <summary>
    /// Propósito: Validador de reglas para la entidad <see cref="HeadersDTO"/>.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class HeadersValidador : AbstractValidator<HeadersDTO>
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de clase <see cref="HeadersValidador"/>.
        /// </summary>
        public HeadersValidador()
        {
            Include(new HeaderBaseValidador());
            RuleFor(dto => dto.Id)
           .NotNull().WithMessage("{PropertyName} es requerido")
           .NotEmpty().WithMessage("{PropertyName} es requerido")
           .Length(36).WithMessage("{PropertyName} debe contar con una longitud fija de {TotalLength}");
        }

        #endregion
    }
}
