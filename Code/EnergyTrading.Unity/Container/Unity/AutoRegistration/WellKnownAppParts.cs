namespace EnergyTrading.Container.Unity.AutoRegistration
{
    /// <summary>
    /// Well known application part names: 
    /// UI composition parts, like Controller or View;
    /// design patterns, like Visitor or Proxy;
    /// general app parts, like Service or Validator
    /// </summary>
    public static class WellKnownAppParts
    {
        // UI
        public static readonly string Controller = "Controller";
        public static readonly string View = "View";
        public static readonly string ViewModel = "ViewModel";
        public static readonly string Presenter = "Presenter";

        // General
        public static readonly string Service = "Service";
        public static readonly string Task = "Task";
        public static readonly string Factory = "Factory";
        public static readonly string Validator = "Validator";
        public static readonly string Manager = "Manager";
        public static readonly string Extension = "Extension";
        public static readonly string Handler = "Handler";
        public static readonly string Event = "Event";
        public static readonly string DomainEvent = "DomainEvent";
        public static readonly string Provider = "Provider";
        public static readonly string Policy = "Policy";
        public static readonly string Config = "Config";
        public static readonly string Driver = "Driver";
        public static readonly string Builder = "Builder";
        public static readonly string Request = "Request";
        public static readonly string Reply = "Reply";
        public static readonly string Response = "Response";
        public static readonly string Info = "Info";
        public static readonly string Filter = "Filter";
        public static readonly string Element = "Element";
        public static readonly string Description = "Description";
        public static readonly string Message = "Message";
        public static readonly string Logger = "Logger";

        // Design patterns
        public static readonly string Strategy = "Strategy";
        public static readonly string Decorator = "Decorator";
        public static readonly string Visitor = "Visitor";
        public static readonly string Adapter = "Adapter";
        public static readonly string Wrapper = "Wrapper";
        public static readonly string Singleton = "Singleton";
        public static readonly string Bridge = "Bridge";
        public static readonly string Facade = "Facade";
        public static readonly string Proxy = "Proxy";
        public static readonly string Command = "Command";
        public static readonly string Repository = "Repository";
        public static readonly string Specification = "Specification";
    }
}