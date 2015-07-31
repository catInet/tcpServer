using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus.Messaging;
using System.Text;

namespace tcpServerPublish
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private AutoResetEvent connectionWaitHandle = new AutoResetEvent(false);
        private int prevMlClass = -1;
        //private EventHubClient evHubClient;
        public override void Run()
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(
                    RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["listenerPublish"].IPEndpoint);
                listener.ExclusiveAddressUse = false;
                listener.Start();
            }
            catch (SocketException)
            {
                Trace.Write("Echo server could not start.", "Error");
                return;
            }

            while (true)
            {
                IAsyncResult result = listener.BeginAcceptTcpClient(HandleAsyncConnection, listener);
                connectionWaitHandle.WaitOne();
            }

        }

        public override bool OnStart()
        {
            // Задайте максимальное число одновременных подключений
            ServicePointManager.DefaultConnectionLimit = 12;

            // Дополнительные сведения по управлению изменениями конфигурации
            // см. раздел MSDN по ссылке http://go.microsoft.com/fwlink/?LinkId=166357.
            // evHubClient = EventHubClient.CreateFromConnectionString("Endpoint=sb://kotinternet-ns.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=PN3PudxvUetJzGW6Kv1En0oGM16z2li1rZ9FWYbxpZo=",
            //                                                          "kotinternet");
            bool result = base.OnStart();

            Trace.TraceInformation("tcpServer has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("tcpServer is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("tcpServer has stopped");
        }

        private void HandleAsyncConnection(IAsyncResult result)
        {
            // Accept connection 
            TcpListener listener = (TcpListener)result.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(result);
            connectionWaitHandle.Set();

            // Accepted connection 
            Guid clientId = Guid.NewGuid();
            Trace.WriteLine("Accepted connection with ID " + clientId.ToString(), "Information");

            long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            var timestamp = ticks / 10000000;

            NetworkStream netStream = client.GetStream();
            byte[] buffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;
            // Incoming message may be larger than the buffer size.
            do
            {
                numberOfBytesRead = netStream.Read(buffer, 0, buffer.Length);
                myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));

            }
            while (netStream.DataAvailable);
            /*var textToSend = InputHandler.handleInputString(myCompleteMessage.ToString(), timestamp);
            var data = new EventData(Encoding.UTF8.GetBytes(textToSend))
            {
                PartitionKey = "0",
            };
            data.Properties.Add("Type", "Message");
            evHubClient.Send(data);*/
            var boardString = myCompleteMessage.ToString();
            //преобразовать данные
            string preprocessedData = "";
            try
            {
                preprocessedData = DataPreprocessor.generateStringToSend(boardString, timestamp);
            }
            catch (Exception)
            {
                return;
            }
            int mlClass = -1;
            //отправить в сервис машинного обучения
            //получить номер класса
            try
            {
                mlClass = MlApiWrapper.sendStringToML("tris is neutral text for ML test"); //preprocessedData);
            }
            catch (Exception)
            {
                return;
            }
            //если номер класса не совпадает с предыдущим, обнавляем предыдущий и посыдаем сообщение в вк
            if (prevMlClass != mlClass)
            { 
                //send to VK;
                try
                {   
                    var masterID = TableStorageConnector.GetMasterID();
                    var message = MessageGenerator.generateMessage(mlClass);
                    VkApiWrapper.sendToVk(message + "\n\n Плата отправила: " + preprocessedData, masterID);
                }
                catch (Exception) {
                    return;
                }

                prevMlClass = mlClass;
            }
            client.Close();
        }
    }
}
