namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Validadores
{
    using CPM.ApiNotificacionesWhatsapp.ServicioAPI.Modelos;
    using FluentValidation;

    /// <summary>
    /// Propósito: Validador de reglas para la entidad <see cref="HeaderBaseDTO"/>.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class HeaderBaseValidador : AbstractValidator<HeaderBaseDTO>
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de clase <see cref="HeaderBaseValidador"/>.
        /// </summary>
        public HeaderBaseValidador()
        {
            RuleFor(m => m.IdCanal)
                .NotNull().WithMessage("{PropertyName} es requerido")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor a {PropertyValue}")
                .LessThan(20).WithMessage("{PropertyName} debe ser menor a {PropertyValue}");
            RuleFor(m => m.NombreAplicacion)
                .NotNull().WithMessage("{PropertyName} es requerido")
                .NotEmpty().WithMessage("{PropertyName} es requerido")
                .MaximumLength(50).WithMessage("{PropertyName} debe tener una longitud máxima de {MaxLength} caracteres alfanuméricos");
        }

        #endregion
    }
}