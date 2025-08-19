namespace CPM.ApiNotificacionesWhatsapp.Persistencia.Repositorios
{
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using CPM.ApiNotificacionesWhatsapp.Aplicacion.Repositorios;
    using CPM.ApiNotificacionesWhatsapp.Dominio.Parametros;
    using CPM.ApiNotificacionesWhatsapp.Persistencia.Configuracion;
    using CPM.Comun.Arquitectura.Configuracion;
    using CPM.Comun.Arquitectura.DTOs;
    using CPM.Datos.SqlServer;
    using CPM.Mensajeria.Auronix.Dominio;
    using CPM.Mensajeria.Auronix.Persistencia.Configuracion;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Polly;
    using Polly.Retry;

    /// <summary>
    /// Propósito: Implementa los métodos de la interfaz <see cref="IRepositorioWhatsApp"/>.
    /// Fecha de creación: 07/05/2025.
    /// Creador: Ronaldo Barrientos Negrete (BANR25734).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    public class RepositorioWhatsApp : RepositorioSqlServer, IRepositorioWhatsApp
    {
        #region Variables

        /// <summary>
        /// Listado de procedimientos almacenados.
        /// </summary>
        private readonly ProcedimientosAlmacenados procedimientos;

        /// <summary>
        /// Proporciona los métodos para el registro de trazabilidad.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Proporciona información de los parámetros de la petición actual.
        /// </summary>
        private readonly TrazabilidadDTO trazabilidad;

        /// <summary>
        /// Una política de reintento que se puede aplicar a delegados sincrónicos.
        /// </summary>
        private AsyncRetryPolicy? retryPolicy;

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RepositorioWhatsApp"/>.
        /// </summary>
        /// <param name="trazabilidadDTO">Proporciona información de los parámetros de la petición actual.</param>
        /// <param name="logger">Proporciona los métodos para el registro de trazabilidad.</param>
        /// <param name="politicasReintento">Proporciona configuración para la implementación de reintentos.</param>
        public RepositorioWhatsApp(TrazabilidadDTO trazabilidadDTO,IOptions<DbConfiguracion> configuracion, ILogger<RepositorioWhatsApp> logger, IOptions<PoliticasReintento> politicasReintento)
        : base(configuracion.Value.Cadenas.CadenaConexion)
        {
            this.procedimientos = configuracion.Value.Procedimientos;
            this.logger = logger;
            this.logger.BeginScope(new
            {
                IdTransaccion = trazabilidadDTO.IdTransaccion,
                Usuario = trazabilidadDTO.Usuario,
            });
            trazabilidad = trazabilidadDTO;
            this.ConfigurarPoliticasReintento(politicasReintento.Value.Reintento);
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Consulta los parámetros asociados a una plantilla específica.
        /// </summary>
        /// <param name="idPlantilla">Identificador de la plantilla para la cual se consultan los parámetros.</param>
        /// <returns>Una colección de parámetros asociados a la plantilla, o null si ocurre un error.</returns>
        public async Task<IEnumerable<Parametro>?> ConsultarParametrosPlantilla(string IdPlantilla)
        {
            IEnumerable<Parametro>? listaParametrosPlantilla = Enumerable.Empty<Parametro>();
            List<SqlParameter> listaParametros = new()
            {
                new SqlParameter("@IdPlantilla", IdPlantilla),
            };
            try
            {
                listaParametrosPlantilla = await this.ConsultarProcedimientoAlmacenadoAsync(
               this.procedimientos.SPConsultarParametrosPlantilla,
               reader =>
               {
                   return new Parametro()
                   {
                       Id = reader["IdPlantilla"].ToString()!,
                       NombrePlantilla = reader["NombrePlantilla"].ToString()!,
                       IdParametro = int.Parse(reader["IdParametro"].ToString()!),
                       NombreParametro = reader["NombreParametro"].ToString()!,
                       Orden = int.Parse(reader["Orden"].ToString()!),
                       Valor = reader["ValorDefault"].ToString()!,
                   };
               },
               listaParametros);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, "Ocurrió un error al consultar los parámetros de la plantilla solicitada.");
                return null;
            }

            this.logger.LogInformation("Consulta de información de parámetros de la plantilla realizada correctamente.");
            return listaParametrosPlantilla ?? Enumerable.Empty<Parametro>();
        }

        /// <summary>
        /// Inserta una notificación de WhatsApp en la base de datos.
        /// </summary>
        /// <param name="notificacionWhatsApp">Objeto que contiene la información de la notificación a insertar.</param>
        /// <returns>Devuelve true si la inserción fue exitosa; de lo contrario, false.</returns>
        public async Task<bool> InsertarNotificacionWA(NotificacionWhatsApp notificacionWhatsApp)
        {
            try
            {
                await this.retryPolicy!.ExecuteAsync(async () =>
                {
                    List<SqlParameter> listaParametros = new()
                {
                    new SqlParameter("@IdTransaccionRegistro", notificacionWhatsApp.IdTransaccionRegistro),
                    new SqlParameter("@TelefonoDestinatario", notificacionWhatsApp.TelefonoDestinatario),
                    new SqlParameter("@FechaInicio", notificacionWhatsApp.FechaInicio),
                    new SqlParameter("@FechaFinal", notificacionWhatsApp.FechaFinal),
                    new SqlParameter("@Estatus", notificacionWhatsApp.Estatus),
                    new SqlParameter("@IdCanalCPM", notificacionWhatsApp.IdCanalCPM),
                    new SqlParameter("@NombreAplicacion", notificacionWhatsApp.NombreAplicacion),
                    new SqlParameter("@UsuarioRegistro", this.trazabilidad.Usuario),
                    new SqlParameter("@FolioSolicitud", string.IsNullOrEmpty(notificacionWhatsApp.FolioSolicitud) ? (object)DBNull.Value : notificacionWhatsApp.FolioSolicitud),
                    new SqlParameter("@IdPlantilla", notificacionWhatsApp.IdPlantilla ?? (object)DBNull.Value),
                    new SqlParameter("@Reintentos", notificacionWhatsApp.Reintentos),
                    new SqlParameter("@IdCampania", string.IsNullOrEmpty(notificacionWhatsApp.IdCampania) ? (object)DBNull.Value : notificacionWhatsApp.IdCampania),
                    new SqlParameter("@IdTransaccionProceso", string.IsNullOrEmpty(notificacionWhatsApp.IdTransaccionProceso) ? (object)DBNull.Value : notificacionWhatsApp.IdTransaccionProceso),
                    new SqlParameter("@NumeroSocio", notificacionWhatsApp.NumeroSocio)
                };
                    await this.EjecutarProcedimientoAlmacenadoAsync(this.procedimientos.SPInsertarNotificacionWAEvento, listaParametros);
                });
                this.logger.LogInformation("Notificación de WhatsApp insertada correctamente.");
                return true;
            }

            catch (Exception ex)
            {
                this.logger.LogCritical(ex, "Ocurrió un error al insertar la notificación de WhatsApp.");
                return false;
            }
        }

        /// <summary>
        /// Inserta un parámetro de evento en la base de datos.
        /// </summary>
        /// <param name="eventoParametro">Objeto que contiene la información del parámetro a insertar.</param>
        /// <returns>Devuelve true si la inserción fue exitosa; de lo contrario, false.</returns>
        public async Task<bool> InsertarEventoParametro(EventoParametro eventoParametro)
        {      
            try
            {
                await this.retryPolicy!.ExecuteAsync(async () =>
                {
                    List<SqlParameter> listaParametros = new()
                  {
                      new SqlParameter("@IdTransaccionRegistro", eventoParametro.IdTransaccionRegistro),
                      new SqlParameter("@IdParametro", eventoParametro.IdParametro),
                      new SqlParameter("@Valor", eventoParametro.Valor),
                      new SqlParameter("@Orden", eventoParametro.Orden)
                  };

                    await this.EjecutarProcedimientoAlmacenadoAsync(this.procedimientos.SPInsertarNotificacionWAEventoParametro, listaParametros);
                });
                this.logger.LogInformation("Parámetro de evento insertado correctamente.");
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, "Ocurrió un error al insertar el parámetro de evento.");
                return false;
            }
        }
        #endregion

        #region Métodos privados

        /// <summary>
        /// Configura las politicas para la implementación de un patrón de reintentos a un proceso o tarea.
        /// </summary>
        /// <param name="reintento">Cantidad y tiempo de reintento.</param>
        private void ConfigurarPoliticasReintento(Reintento reintento)
        {
            // Definir la política de reintento con un máximo de 3 intentos.
            this.retryPolicy = Policy.Handle<Exception>().Or<TimeoutException>().Or<SqlNullValueException>() // Manejar excepciones específicas, en este caso Exception
                .WaitAndRetryAsync(reintento.Cantidad, retryAttempt => TimeSpan.FromSeconds(reintento.TiempoEspera), (exception, timeSpan, retryCount, context) =>
                {
                    // Acción opcional en cada reintento, por ejemplo, loguear el error.
                    this.logger.LogCritical(exception, "Intento {retryCount}/{count}: Falló con la excepción {mensaje}. Reintentando en {segundos} segundos", retryCount, reintento.Cantidad, exception.Message, timeSpan.Seconds);
                });
        }

        #endregion
    }
}
