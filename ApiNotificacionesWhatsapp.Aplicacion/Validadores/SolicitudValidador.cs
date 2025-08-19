namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.Validadores
{
    using FluentValidation;
    using global::CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;

    /// <summary>
    /// Propósito: Validador de reglas para la entidad <see cref="SolicitudDTO"/>.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// </summary>
    public class SolicitudValidador : AbstractValidator<SolicitudDTO>
        {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SolicitudValidador"/>.
        /// </summary>
        public SolicitudValidador()
        {
            RuleFor(dto => dto.FolioSolicitud)
                .Length(10, 20).WithMessage("El folio de solicitud debe tener entre 10 y 20 dígitos.")
                .Matches(@"^\d*$").WithMessage("El folio de solicitud solo puede contener números.")
                .When(dto => !string.IsNullOrEmpty(dto.FolioSolicitud));

            RuleFor(dto => dto.NumeroSocio)
            .NotNull().WithMessage("El número de socio es obligatorio.")
            .NotEmpty().WithMessage("El número de socio no puede estar vacío.")
            .Length(1, 10).WithMessage("El número de socio debe tener entre 1 y 10 dígitos.")
            .Matches(@"^\d+$").WithMessage("El número de socio solo puede contener números.");

            RuleFor(dto => dto.Telefono)
                .NotNull().WithMessage("El número de teléfono es obligatorio.")
                .NotEmpty().WithMessage("El número de teléfono no puede estar vacío.")
                .Length(10).WithMessage("El número de teléfono debe tener exactamente 10 dígitos.")
                .Matches(@"^\d{10}$").WithMessage("El número de teléfono solo puede contener números.");

            RuleFor(dto => dto.IdPlantilla)
                .NotNull().WithMessage("El identificador de plantilla es obligatorio.")
                .NotEmpty().WithMessage("El identificador de plantilla no puede estar vacío.");

            RuleFor(x => x.Parametros)
            .NotNull().WithMessage("Parametros no puede ser nulo.")
            .Must(x => x.Any()).WithMessage("Debe proporcionar al menos un parámetro.")
            .ForEach(param => param.NotEmpty().WithMessage("Cada parámetro no puede estar vacío."));
        }
    }   
}
