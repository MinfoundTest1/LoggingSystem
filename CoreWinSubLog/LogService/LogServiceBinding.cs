using System;
using System.ServiceModel;

namespace CoreWinSubLog
{
    /// <summary>
    /// WCF binding for log service.
    /// </summary>
    public static class LogServiceBinding
    {
        /// <summary>
        /// Tcp binding for WCF service.
        /// </summary>
        /// <returns></returns>
        public static NetTcpBinding TcpBinding()
        {
            NetTcpBinding myBinding = new NetTcpBinding(SecurityMode.None);

            myBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            myBinding.ReceiveTimeout = TimeSpan.FromDays(14);
            myBinding.ReliableSession.InactivityTimeout = TimeSpan.FromDays(14);
            myBinding.SendTimeout = TimeSpan.FromMinutes(1);
            myBinding.MaxReceivedMessageSize = int.MaxValue;
            myBinding.MaxBufferSize = int.MaxValue;
            myBinding.TransferMode = TransferMode.Buffered;

            myBinding.ReaderQuotas.MaxDepth = int.MaxValue;
            myBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            myBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            myBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;

            return myBinding;
        }

        ///// <summary>
        ///// Http binding for WCF service.
        ///// </summary>
        ///// <returns></returns>
        //public static NetHttpBinding HttpBinding()
        //{
        //    NetHttpBinding myBinding = new NetHttpBinding(BasicHttpSecurityMode.None);

        //    myBinding.ReceiveTimeout = TimeSpan.FromDays(14);
        //    myBinding.ReliableSession.InactivityTimeout = TimeSpan.FromDays(14);
        //    myBinding.SendTimeout = TimeSpan.FromMinutes(1);
        //    myBinding.MaxReceivedMessageSize = int.MaxValue;
        //    myBinding.MaxBufferSize = int.MaxValue;
        //    myBinding.TransferMode = TransferMode.Buffered;

        //    myBinding.ReaderQuotas.MaxDepth = int.MaxValue;
        //    myBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
        //    myBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
        //    myBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;

        //    return myBinding;
        //}

        /// <summary>
        /// WCF service remote address which is tcp connection.
        /// </summary>
        /// <param name="ipAddress">Host ip address</param>
        /// <returns>Tcp Uri</returns>
        public static Uri TcpUri(string ipAddress)
        {
            return new Uri("net.tcp://" + ipAddress + ":8202/LogbookService"); 
        }

        /// <summary>
        /// WCF service remote address which is http connection.
        /// </summary>
        /// <param name="ipAddress">Host ip address</param>
        /// <returns>Http uri</returns>
        public static Uri HttpUri(string ipAddress)
        {
            return new Uri("http://" + ipAddress + ":8200/LogbookService");
        }
    }
}
