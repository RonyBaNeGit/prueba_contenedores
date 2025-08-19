namespace CPM.ApiNotificacionesWhatsapp.ServicioAPI.Filters
{
    using System;

    /// <summary>
    /// Propósito: Excluye los campos que contienen el atributo SwaggerIgnore.
    /// Fecha de creación: 03/10/2023.
    /// Creador: Juan Carlos Miranda Méndez (MIMJ22037).
    /// Modificó:
    /// Dependencias de conexiones e interfaces: No Aplica.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SwaggerIgnore : Attribute
    {
    }
}
