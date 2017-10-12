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

        public static NetHttpBinding HttpBinding()
        {
            NetHttpBinding myBinding = new NetHttpBinding(BasicHttpSecurityMode.None);

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

        /// <summary>
        /// WCF service remote address.
        /// </summary>
        /// <param name="ipAddress">Host ip address</param>
        /// <returns></returns>
        public static Uri TcpUri(string ipAddress)
        {
            return new Uri("net.tcp://" + ipAddress + ":8202/LogbookService"); 
        }

        public static Uri HttpUri(string ipAddress)
        {
            return new Uri("http://" + ipAddress + ":8200/LogbookService");
        }
    }
}
