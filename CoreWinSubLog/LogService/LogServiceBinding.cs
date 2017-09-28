using System;
using System.ServiceModel;

namespace CoreWinSubLog
{
    public static class LogServiceBinding
    {
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

        public static Uri Uri(string ipAddress)
        {
            return new Uri("net.tcp://" + ipAddress + ":8200/LogbookService"); 
        }
    }
}
