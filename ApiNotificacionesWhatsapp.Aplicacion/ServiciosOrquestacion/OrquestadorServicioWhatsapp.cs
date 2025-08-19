namespace CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosOrquestacion
{
    using AutoMapper;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.DTOs.WhatsApp.EnviarWhatsAppSimple.Respuesta;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Helpers;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.ServiciosExternos;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Mensajeria.Auronix.Aplicacion.DTOs.WhatsApp.EnviarCampania.Peticion;
    using CPM.Mensajeria.Auronix.Aplicacion.Enumerados;
    using CPM.Mensajeria.Auronix.Aplicacion.Repositorios;
    using CPM.Mensajeria.Auronix.Dominio;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Propósito: Envío de notificaciones simples de WhatsApp.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class OrquestadorServicioWhatsapp : IOrquestadorServicioWhatsapp
    {
        #region Constantes

        /// <summary>
        /// Identificador de parámetros de para límite de notificaciones en caché.
        /// </summary>
        public const string ClaveLimiteNotificacionesCache = $"LimiteNotificaciones";

        #endregion

        #region Variables

        /// <summary>
        /// Proporciona información de los parámetros de la petición actual.
        /// </summary>
        private readonly TrazabilidadDTO trazabilidadDTO;

        /// <summary>
        /// Proveedor de configuración para realizar mapas.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Proporciona los métodos necesarios para el registro de trazabilidad..
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Define los métodos necesarios para la administración de información para envío de notificaciones por WhatsApp.
        /// </summary>
        private readonly IRepositorioMensajeriaWhatsApp repositorioMensajeriaWhatsApp;

        /// <summary>
        /// Define los métodos necesarios para el envío de notificaciones por WhatsApp a través del servicio de Auronix.
        /// </summary>
        private readonly IServicioMensajeriaWhatsAppSimpleTransacciones servicioMensajeriaWhatsAppSimpleTransacciones;

        /// <summary>
        ///  Interfaz que define los métodos necesarios para la consulta de configuración para envió de notificaciones de WhatsApp.
        /// </summary>
        private readonly IServicioConfiguracionWhatsapp servicioConfiguracionWhatsapp;

        /// <summary>
        /// Proporciona los métodos necesarios para la consulta de información de plantillas y parámetros.
        /// </summary>
        private readonly IRepositorioWhatsApp repositorioWhatsApp;

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OrquestadorServicioWhatsapp" />.
        /// </summary>
        /// <param name="trazabilidadDTO">Proporciona información de los parámetros de la petición actual.</param>
        /// <param name="mapper">Proveedor de configuración para realizar mapas.</param>
        /// <param name="servicioMensajeriaWhatsAppSimpleTransacciones">Servicio responsable del envío de mensajes para whatsapp simple, utilizado en la orquestación.</param>
        /// <param name="servicioConfiguracionWhatsapp"> Interfaz que define los métodos necesarios para la consulta de configuración para envió de notificaciones de WhatsApp.</param>
        /// <param name="repositorioWhatsApp">Repositorio que maneja las operaciones específicas de consulta de plantillas y parámetros.</param>        
        /// <param name="logger">Proporciona los métodos necesarios para el registro de trazabilidad.</param>
        /// <param name="repositorioMensajeriaWhatsApp">Define los métodos necesarios para la administración de información para envío de notificaciones por WhatsApp.</param> 
        public OrquestadorServicioWhatsapp(
      TrazabilidadDTO trazabilidadDTO,
      IMapper mapper,
      IRepositorioWhatsApp repositorioWhatsApp,
      IServicioMensajeriaWhatsAppSimpleTransacciones servicioMensajeriaWhatsAppSimpleTransacciones,  
      IServicioConfiguracionWhatsapp servicioConfiguracionWhatsapp,
      ILogger<OrquestadorServicioWhatsapp> logger,
      IRepositorioMensajeriaWhatsApp repositorioMensajeriaWhatsApp)
        {
            this.trazabilidadDTO = trazabilidadDTO;
            this.mapper = mapper;           
            this.logger = logger;
            this.repositorioWhatsApp = repositorioWhatsApp;
            this.servicioMensajeriaWhatsAppSimpleTransacciones = servicioMensajeriaWhatsAppSimpleTransacciones;
            this.servicioConfiguracionWhatsapp = servicioConfiguracionWhatsapp;
            this.logger.BeginScope(new { IdTransaccion = trazabilidadDTO.IdTransaccion, Usuario = this.trazabilidadDTO.Usuario });
            this.repositorioMensajeriaWhatsApp = repositorioMensajeriaWhatsApp;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Envía WhatsApp de forma individual.
        /// </summary>
        /// <param name="solicitud">Información de la solicitud para el envío simple.</param>
        /// <returns>Representa una operación asincrona.</returns>
        public async Task<RespuestaDTO> EnviarWhatsAppIndividual(SolicitudDTO solicitud)
        {
            // Generar identificadores únicos para la transacción de Auronix.
            string idTransaccionRegistroCliente = Guid.NewGuid().ToString().ToUpper();
            string idTransaccionRegistroUsuario = Guid.NewGuid().ToString().ToUpper();

            // Obtener la configuración de WhatsApp
            ConfiguracionWhatsApp? configuracionWhatsApp = await this.servicioConfiguracionWhatsapp.ObtenerConfiguracionWhatsappAsync();
            if (configuracionWhatsApp == null)
            {
                return RespuestaDTOHelper.RespuestaErrorInterno(trazabilidadDTO.IdTransaccion, Enumerados.CodigoRespuesta.ErrorInterno, "Ocurrió un error al obtener la configuración para el envío del WhatsApp.");
            }

            // Consultar las notificaciones procesadas
            IEnumerable<NotificacionWhatsApp>? notificacionesProcesadas = await this.repositorioMensajeriaWhatsApp.ConsultarNotificacionesProcesadas(configuracionWhatsApp.LimiteNotificacionesMeta);
            if (notificacionesProcesadas is null)
            {
                return RespuestaDTOHelper.RespuestaErrorInterno(trazabilidadDTO.IdTransaccion, Enumerados.CodigoRespuesta.ErrorInterno, "Ocurrió un error al consultar las notificaciones procesadas correctamente.");
            }

            // Validar el límite de notificaciones
            if (!ValidarNotificacionesProcesadas(notificacionesProcesadas, configuracionWhatsApp))
            {
                return RespuestaDTOHelper.RespuestaInvalida(trazabilidadDTO.IdTransaccion, "Se alcanzó el límite de envío de notificaciones.");
            }

            // Obtener las plantillas de WhatsApp
            PlantillaWhatsApp? plantilla = await ObtenerPlantilla(solicitud.IdPlantilla);
            if (plantilla == null)
            {
                return RespuestaDTOHelper.RespuestaErrorInterno(trazabilidadDTO.IdTransaccion, Enumerados.CodigoRespuesta.ErrorInterno, "Error al consultar el registro de la plantilla solicitada.");
            }

            if (string.IsNullOrEmpty(plantilla!.Id))
            {
                return RespuestaDTOHelper.RespuestaInvalida(trazabilidadDTO.IdTransaccion, "No existe la plantilla proporcionada.");
            }
             
            // Obtener los parámetros de la plantilla
            var parametrosPlantilla = await ObtenerParametrosPlantilla(solicitud.IdPlantilla);
            if (parametrosPlantilla == null)
            {
                return RespuestaDTOHelper.RespuestaErrorInterno(trazabilidadDTO.IdTransaccion, Enumerados.CodigoRespuesta.ErrorInterno, "Error al obtener parámetros de la plantilla.");
            }

            if (!parametrosPlantilla.Any())
            {
                return RespuestaDTOHelper.RespuestaInvalida(trazabilidadDTO.IdTransaccion, "La plantilla solicitada no cuenta con parámetros configurados.");
            }

            // Validar el número de parámetros
            if (solicitud.Parametros.Count != parametrosPlantilla.Count())
            {
                return RespuestaDTOHelper.RespuestaInvalida(trazabilidadDTO.IdTransaccion, "La cantidad de parámetros proporcionados no coincide con la cantidad requerida para la plantilla.");
            }

            // Crear y enviar solicitud de WhatsApp
            RespuestaServicioAuronixWASimpleDTO? resultado = await EnviarWhatsApp(solicitud, idTransaccionRegistroCliente, idTransaccionRegistroUsuario, configuracionWhatsApp, plantilla, solicitud.Parametros);

            // Registrar notificación en base de datos
            if (!await RegistrarNotificacion(solicitud, resultado!, trazabilidadDTO))
            {
                this.logger.LogError("Ocurrió un error al registrar la notificación en base de datos. Solicitud:{@@solicitud}", solicitud);
            }

            // Insertar parámetros
            if (!await InsertarParametrosEvento(parametrosPlantilla, trazabilidadDTO.IdTransaccion, solicitud.Parametros))
            {
                this.logger.LogError("Ocurrió un error al registrar el parámetro-Evento en base de datos. Solicitud:{@@solicitud}", solicitud);
            }

            if (resultado == null)
            {
                return RespuestaDTOHelper.RespuestaErrorInterno(trazabilidadDTO.IdTransaccion, Enumerados.CodigoRespuesta.ErrorInterno, "Ocurrió un error al intentar establecer comunicación con el servicio de Auronix.");
            }

            if (!string.IsNullOrEmpty(resultado.Descripcion))
            {
                return RespuestaDTOHelper.RespuestaInvalida(trazabilidadDTO.IdTransaccion, resultado.Descripcion);
            }

            return RespuestaDTOHelper.RespuestaCorrecta(trazabilidadDTO.IdTransaccion, "Solicitud procesada correctamente.");
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Convierte un nombre de parámetro en formato de texto plano a formato PascalCase.
        /// </summary>
        /// <param name="nombre">El nombre del parámetro en formato de texto plano.</param>
        /// <returns>El nombre del parámetro en formato PascalCase.</returns>
        private string ConvertirNombreParametro(string nombre)
        {
            return string.Join(string.Empty, nombre
                .ToLower()
                .Split(' ')
                .Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }

        /// <summary>
        /// Valida el número de socio y lo completa con ceros a la izquierda hasta tener 10 dígitos.
        /// </summary>
        /// <param name="numeroSocio">El número de socio como string.</param>
        /// <returns>El número de socio completo con ceros a la izquierda.</returns>
        private string FormatearNumeroSocio(string numeroSocio)
        {
            // Elimina espacios y verifica la longitud
            return numeroSocio.Trim().PadLeft(10, '0');
        }

        /// <summary>
        /// Agrega el prefijo de clave de México (+52) al número de teléfono si aún no lo contiene.
        /// </summary>
        /// <param name="numeroTelefono">Número de teléfono destinatario.</param>
        /// <returns>El número de teléfono con el prefijo "52" (código de país México).</returns>
        private string AgregarPrefijoClaveMX(string numeroTelefono)
        {
            // Limpia espacios en blanco.
            string telefonoLimpio = numeroTelefono!.Trim();

            // Agrega el prefijo "52" antes del número
            return $"52{telefonoLimpio}";
        }

        /// <summary>
        /// Valida si se han procesado las notificaciones y verifica el límite de envío.
        /// </summary>
        /// <param name="notificacionesProcesadas">Listado de notificaciones procesadas correctamente.</param>
        /// <param name="configuracionWhatsApp">Configuración de WhatsApp que contiene el límite de notificaciones.</param>
        /// <returns>True si las notificaciones no superan el límite; de lo contrario, false.</returns>
        private bool ValidarNotificacionesProcesadas(IEnumerable<NotificacionWhatsApp>? notificacionesProcesadas, ConfiguracionWhatsApp configuracionWhatsApp)
        {
            if (notificacionesProcesadas!.Count() > configuracionWhatsApp.LimiteNotificacionesMeta)
            {
                this.logger.LogDebug("Se alcanzó el límite de envío de notificaciones.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Obtiene una plantilla de WhatsApp por su ID.
        /// </summary>
        /// <param name="idPlantilla">El ID de la plantilla a buscar.</param>
        /// <returns>Un objeto de tipo <see cref="PlantillaWhatsApp"/> o null si no se encuentra.</returns>
        private async Task<PlantillaWhatsApp?> ObtenerPlantilla(string idPlantilla)
        {
            var listaPlantillaWhatsApp = await this.repositorioMensajeriaWhatsApp.ConsultarPlantillas();

            if (listaPlantillaWhatsApp == null)
            {
                return null;
            }

            return listaPlantillaWhatsApp.FirstOrDefault(x => x.Id == idPlantilla) ?? new PlantillaWhatsApp();
        }

        /// <summary>
        /// Obtiene los parámetros de una plantilla de WhatsApp.
        /// </summary>
        /// <param name="idPlantilla">El ID de la plantilla cuyas parámetros se desean obtener.</param>
        /// <returns>Una colección de objetos de tipo <see cref="Parametro"/> o null si no se encuentran.</returns>
        private async Task<IEnumerable<Parametro>?> ObtenerParametrosPlantilla(string idPlantilla)
        {
            IEnumerable<Parametro>? parametros = await this.repositorioWhatsApp.ConsultarParametrosPlantilla(idPlantilla);
            if (parametros == null)
            {
                return null;
            }

            if (!parametros.Any())
            {
                return Enumerable.Empty<Parametro>();
            }

            return parametros;
        }

        /// <summary>
        /// Envía un mensaje de WhatsApp utilizando la configuración y plantilla especificadas.
        /// </summary>
        /// <param name="solicitud">La solicitud de envío de WhatsApp que contiene los datos del destinatario.</param>
        /// <param name="idTransaccionRegistroCliente">El ID de transacción del cliente.</param>
        /// <param name="idTransaccionRegistroUsuario">El ID de transacción del usuario.</param>
        /// <param name="configuracionWhatsApp">La configuración de WhatsApp a utilizar para el envío.</param>
        /// <param name="plantilla">La plantilla de WhatsApp a utilizar.</param>
        /// <param name="parametrosLista">Una lista de parámetros a incluir en el mensaje.</param>
        /// <returns>Un objeto de tipo <see cref="RespuestaServicioAuronixWASimpleDTO"/> que contiene la respuesta del servicio de Auronix.</returns>
        private async Task<RespuestaServicioAuronixWASimpleDTO?> EnviarWhatsApp(SolicitudDTO solicitud, string idTransaccionRegistroCliente, string idTransaccionRegistroUsuario, ConfiguracionWhatsApp configuracionWhatsApp, PlantillaWhatsApp plantilla, List<string> parametrosLista)
        {
            var plantillaDTO = new PlantillaDTO
            {
                IdPlantilla = solicitud.IdPlantilla.ToString(),
                Lenguaje = plantilla.Lenguaje.ToString(),
                Url = plantilla.Url.ToString(),
                Parametros = parametrosLista,
                TipoFormato = plantilla.Mime.ToString(),
            };

            var solicitudWhatsAppSimple = new PeticionWhatsAppSimpleDTO
            {
                Canal = configuracionWhatsApp.Canal,
                IdTransaccionCliente = idTransaccionRegistroCliente,
                IdUsuario = idTransaccionRegistroUsuario,
                TelefonoDestinatario = AgregarPrefijoClaveMX(solicitud.Telefono.ToString()),
                Plantilla = plantillaDTO,
                ListaMensajeDatosMeta = Array.Empty<string>().ToList(),
                Descripcion = plantilla.DescripcionCampania,
                ListaNegra = Array.Empty<string>().ToList(),
                ListaMetaDatos = Array.Empty<string>().ToList(),
            };

            RespuestaServicioAuronixWASimpleDTO? resultado = await this.servicioMensajeriaWhatsAppSimpleTransacciones.EnviarWhatsAppSimple(solicitudWhatsAppSimple);

            return resultado;
        }

        /// <summary>
        /// Registra la notificación de WhatsApp en la base de datos.
        /// </summary>
        /// <param name="solicitud">La solicitud de envío de WhatsApp.</param>
        /// <param name="resultado">La respuesta del servicio de Auronix.</param>
        /// <param name="trazabilidadDTO">Información de trazabilidad de la solicitud.</param>
        /// <returns>El objeto <see cref="NotificacionWhatsApp"/> registrado.</returns>
        private async Task<bool> RegistrarNotificacion(SolicitudDTO solicitud, RespuestaServicioAuronixWASimpleDTO? resultado, TrazabilidadDTO trazabilidadDTO)
        {
            if (resultado == null)
            {
                resultado = new RespuestaServicioAuronixWASimpleDTO()
                {
                    Descripcion = "Error",   
                };
            }

            var notificacionWA = new NotificacionWhatsApp
            {
                IdTransaccionRegistro = trazabilidadDTO.IdTransaccion,
                TelefonoDestinatario = AgregarPrefijoClaveMX(solicitud.Telefono),
                FechaInicio = DateTime.Now.ToString("yyyy-MM-dd"),
                FechaFinal = DateTime.Now.ToString("yyyy-MM-dd"),
                Estatus = string.IsNullOrEmpty(resultado.Descripcion) ? EstatusNotificacion.EnviadoConExito : EstatusNotificacion.EnviadoConError,
                IdCanalCPM = trazabilidadDTO.IdCanal,
                NombreAplicacion = trazabilidadDTO.NombreAplicacion,
                FolioSolicitud = solicitud.FolioSolicitud == null ? string.Empty : solicitud.FolioSolicitud,
                IdPlantilla = solicitud.IdPlantilla,
                Reintentos = (byte)((string.IsNullOrEmpty(resultado.IdRelacionado) && string.IsNullOrEmpty(resultado.IdTransaccion)) ? 0 : 1),
                IdCampania = resultado!.IdTransaccion,
                IdTransaccionProceso = string.Empty,
                NumeroSocio = FormatearNumeroSocio(solicitud.NumeroSocio),
            };

            if (!await this.repositorioWhatsApp.InsertarNotificacionWA(notificacionWA))
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Inserta los parámetros del evento en la base de datos.
        /// </summary>
        /// <param name="parametrosPlantilla">La colección de parámetros de la plantilla.</param>
        /// <param name="idTransaccionRegistro">El ID de la transacción de la notificación.</param>
        /// <param name="parametrosRecibidos">Los parámetros proporcionados en la solicitud para insertar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        private async Task<bool> InsertarParametrosEvento(IEnumerable<Parametro> parametrosPlantilla, string idTransaccionRegistro, List<string> parametrosRecibidos)
        {
            int index = 0;
            foreach (var param in parametrosPlantilla)
            {
                var eventoParametro = new EventoParametro
                {
                    IdTransaccionRegistro = idTransaccionRegistro,
                    IdParametro = param.IdParametro,
                    Valor = parametrosRecibidos.ElementAt(index), // Usar el valor de la lista de cadenas
                    Orden = param.Orden,
                };

                // Intenta insertar el parámetro y devuelve false si falla
                if (!await this.repositorioWhatsApp.InsertarEventoParametro(eventoParametro))
                {
                    return false; // Retorna false si la inserción falla
                }

                index++;
            }

            return true; // Retorna true si todas las inserciones son exitosas
        }
        #endregion
    }
}