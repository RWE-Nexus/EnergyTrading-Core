namespace EnergyTrading
{
    using System.Net;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Web;

    public static class ContextInfoProvider
    {
         public static string GetUserName()
         {
             // Service context
             var operationContext = OperationContext.Current;
             if (operationContext != null && operationContext.ServiceSecurityContext != null &&
                 !string.IsNullOrWhiteSpace(operationContext.ServiceSecurityContext.WindowsIdentity.Name))
             {
                 return operationContext.ServiceSecurityContext.WindowsIdentity.Name;
             }

            // Web context
            var httpContext = HttpContext.Current;
            if (httpContext != null && httpContext.User != null && !string.IsNullOrWhiteSpace(httpContext.User.Identity.Name))
            {
                return httpContext.User.Identity.Name;
            }

            // If not, window's context user
            var windowsIdentity = WindowsIdentity.GetCurrent();

            return windowsIdentity != null ? windowsIdentity.Name : "Unknown";
         }

         public static string GetClientMachineName()
         {
             // Service context
             var operationContext = OperationContext.Current;
             if (operationContext != null && operationContext.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] != null)
             {
                 var clientMachineIP = ((RemoteEndpointMessageProperty)operationContext.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address;
                 if (!string.IsNullOrWhiteSpace(clientMachineIP))
                 {
                     var hostEntry = Dns.GetHostEntry(clientMachineIP);
                     if (hostEntry != null && !string.IsNullOrWhiteSpace(hostEntry.HostName))
                     {
                         return hostEntry.HostName;
                     }
                 }
             }

             // Web context
             var httpContext = HttpContext.Current;
             if (httpContext != null && httpContext.User != null &&
                !string.IsNullOrWhiteSpace(httpContext.Request.UserHostName))
             {
                 return httpContext.Request.UserHostName;
             }

             return "Unknown";
         }
    }
}