# ELIMINAR REFERENCIAS DE PROYECTOS DE LA SOLUCIÓN
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.PruebasIntegracion\ApiNotificacionWhatsapp.PruebasIntegracion.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.PruebasUnitarias\ApiNotificacionWhatsapp.PruebasUnitarias.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.Dominio\ApiNotificacionWhatsapp.Dominio.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.Infraestructura\ApiNotificacionWhatsapp.Infraestructura.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.Persistencia\ApiNotificacionWhatsapp.Persistencia.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.Aplicacion\ApiNotificacionWhatsapp.Aplicacion.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.BlazorWASM\ApiNotificacionWhatsapp.BlazorWASM.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.BlazorServer\ApiNotificacionWhatsapp.BlazorServer.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.ServicioAPI\ApiNotificacionWhatsapp.ServicioAPI.csproj
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\ApiNotificacionWhatsapp.Wst\ApiNotificacionWhatsapp.Wst.csproj

# ELIMINAR REFERENCIAS DOCUMENTACIÓN
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\Documentacion\Archivos\BaseDatos\01-Tablas\01-Notificacion_Email_Cat.sql --solution-folder "Documentacion"
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\Documentacion\Archivos\BaseDatos\02-Procedimientos\01-pa_s_Consultar_Notificacion_Email_Cat.sql --solution-folder "Documentacion"
dotnet sln ..\ApiNotificacionWhatsapp.sln remove ..\Documentacion\Archivos\BaseDatos\03-Inserciones\01-Notificacion_Email_Cat.sql --solution-folder "Documentacion"

# AGREGAR REFERENCIAS DE PROYECTOS DE LA SOLUCIÓN
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.PruebasIntegracion\ApiNotificacionWhatsapp.PruebasIntegracion.csproj --solution-folder "00 Pruebas"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.PruebasUnitarias\ApiNotificacionWhatsapp.PruebasUnitarias.csproj --solution-folder "00 Pruebas"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.Dominio\ApiNotificacionWhatsapp.Dominio.csproj --solution-folder "04 Core"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.Infraestructura\ApiNotificacionWhatsapp.Infraestructura.csproj --solution-folder "03 Infraestructura"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.Persistencia\ApiNotificacionWhatsapp.Persistencia.csproj --solution-folder "03 Infraestructura"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.Aplicacion\ApiNotificacionWhatsapp.Aplicacion.csproj --solution-folder "02 Aplicacion"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.BlazorServer\ApiNotificacionWhatsapp.BlazorServer.csproj --solution-folder "01 Presentacion"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.BlazorWASM\ApiNotificacionWhatsapp.BlazorWASM.csproj --solution-folder "01 Presentacion"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.ServicioAPI\ApiNotificacionWhatsapp.ServicioAPI.csproj --solution-folder "01 Presentacion"
dotnet sln ..\ApiNotificacionWhatsapp.sln add ..\ApiNotificacionWhatsapp.Wst\ApiNotificacionWhatsapp.Wst.csproj --solution-folder "01 Presentacion"

# AGREGAR REFERENCIAS DOCUMENTACIÓN
dotnet sln ApiNotificacionWhatsapp.sln add ..\Documentacion\Archivos\BaseDatos\01-Tablas\01-Notificacion_Email_Cat.sql --solution-folder "Documentacion"
dotnet sln ApiNotificacionWhatsapp.sln add ..\Documentacion\Archivos\BaseDatos\02-Procedimientos\01-pa_s_Consultar_Notificacion_Email_Cat.sql --solution-folder "Documentacion"
dotnet sln ApiNotificacionWhatsapp.sln add ..\Documentacion\Archivos\BaseDatos\03-Inserciones\01-Notificacion_Email_Cat.sql --solution-folder "Documentacion"