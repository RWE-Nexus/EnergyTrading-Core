namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Net.Security;
    using System.ServiceModel;

    public static class TcpBindingUtility
    {
        /// <summary>
        /// Client: maximum connections to be pooled for reuse.
        /// Server: maximum connections allowed to be pending dispatch.
        /// </summary>
        public static int MaxConnections { get; private set; }

        /// <summary>
        /// The default is 65,536 bytes.
        /// </summary>
        public static long MaxReceivedMessageSize { get; private set; }

        /// <summary>
        /// The default is 65,536 bytes
        /// </summary>
        public static long MaxBufferPoolSize { get; private set; }

        public static int MaxArrayLength { get; private set; }
        public static int MaxBytesPerRead { get; private set; }
        public static int MaxDepth { get; private set; }

        public static TimeSpan OpenTimeout { get; private set; }
        public static TimeSpan CloseTimeout { get; private set; }
        public static TimeSpan ReceiveTimeout { get; private set; }
        public static TimeSpan SendTimeout { get; private set; }


        /// <summary>
        /// Static initializer for default values in each build configuration.
        /// </summary>
        static TcpBindingUtility()
        {
#if(DEBUG)
            MaxConnections = 10;
            MaxReceivedMessageSize = 4194304;           // 4MB The default is 65,536 bytes
            MaxBufferPoolSize = 1048576;                // 1MB default is 65,536 bytes
            OpenTimeout = new TimeSpan(0, 20, 0);
            CloseTimeout = new TimeSpan(0, 20, 0);
            ReceiveTimeout = new TimeSpan(0, 20, 0);
            SendTimeout = new TimeSpan(0, 20, 0);
            MaxArrayLength = 524288;                    // 512KB default is 16,384 bytes
            MaxBytesPerRead = 16384;                    // 16KB default is 4,096 bytes
            MaxDepth = 96;                              // 96 default is 32
#else
            MaxConnections = 10;
            MaxReceivedMessageSize = 4194304;           // 4MB The default is 65,536 bytes
            MaxBufferPoolSize = 1048576;                // 1MB default is 65,536 bytes
            OpenTimeout = new TimeSpan(0, 1, 30);
            CloseTimeout = new TimeSpan(0, 1, 30);
            ReceiveTimeout = new TimeSpan(0, 10, 0);
            SendTimeout = new TimeSpan(0, 1, 30);
            MaxArrayLength = 524288;                    // 512KB default is 16,384 bytes
            MaxBytesPerRead = 16384;                    // 16KB default is 4,096 bytes
            MaxDepth = 96;                              // 96 default is 32
#endif
        }

        public static NetTcpBinding CreateNetTcpBinding()
        {
            var tcpBinding = new NetTcpBinding(SecurityMode.Transport, false);

            tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            tcpBinding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;

            tcpBinding.MaxConnections = MaxConnections;
            tcpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            tcpBinding.MaxBufferPoolSize = MaxBufferPoolSize;

            tcpBinding.ReaderQuotas.MaxArrayLength = MaxArrayLength;
            tcpBinding.ReaderQuotas.MaxBytesPerRead = MaxBytesPerRead;
            tcpBinding.ReaderQuotas.MaxDepth = MaxDepth;
            
            //not allowed by partially trusted 
            //tcpBinding.MaxBufferSize = 262144;
            //256KB default is 65,536 bytes

            tcpBinding.OpenTimeout = OpenTimeout;
            tcpBinding.CloseTimeout = CloseTimeout;
            tcpBinding.ReceiveTimeout = ReceiveTimeout;
            tcpBinding.SendTimeout = SendTimeout;

            return tcpBinding;
        }

        public static EndpointAddress CreateEndpointAddress(string serviceAddress)
        {
            return new EndpointAddress(string.Format("net.tcp://{0}", serviceAddress));
        }
    }
}