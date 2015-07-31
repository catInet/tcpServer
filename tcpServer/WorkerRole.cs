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

namespace tcpServer
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private AutoResetEvent connectionWaitHandle = new AutoResetEvent(false);
        private EventHubClient evHubClient;
        public override void Run() 
        { 
             TcpListener listener = null; 
             try 
             { 
                  listener = new TcpListener(
                      RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["listener"].IPEndpoint); 
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
            evHubClient = EventHubClient.CreateFromConnectionString("Endpoint=sb://kotinternet-ns.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=PN3PudxvUetJzGW6Kv1En0oGM16z2li1rZ9FWYbxpZo=",
                                                                     "kotinternet");
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
                var timestamp = ticks.ToString();

                NetworkStream netStream = client.GetStream();
                byte[] buffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;
                // Incoming message may be larger than the buffer size.
                try {
                    do
                    {
                        numberOfBytesRead = netStream.Read(buffer, 0, buffer.Length);
                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
                    }
                    while (netStream.DataAvailable);
                    //StreamReader reader = new StreamReader(netStream);
                    //StreamWriter writer = new StreamWriter(netStream);
                    //writer.AutoFlush = true;
                    var dataPackages = myCompleteMessage.ToString().Split(';');
                    string acc_x = "", acc_y = "", acc_z = "",
                           gyr_x = "", gyr_y = "", gyr_z = "";
                    foreach (string package in dataPackages)
                    {
                        if (!(String.IsNullOrEmpty(package)))
                        {
                            var singleReadings = package.Split(',');
                            int testInt = 0;
                            try
                            {
                                acc_x += singleReadings[0] + "_";
                                testInt = Convert.ToInt32(singleReadings[0]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                acc_x += "0_";
                            }

                            try
                            {
                                acc_y += singleReadings[1] + "_";
                                testInt = Convert.ToInt32(singleReadings[1]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                acc_y += "0_";
                            }

                            try
                            {
                                acc_z += singleReadings[2] + "_";
                                testInt = Convert.ToInt32(singleReadings[2]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                acc_z += "0_";

                            }

                            try
                            {
                                gyr_x += singleReadings[3] + "_";
                                testInt = Convert.ToInt32(singleReadings[3]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                gyr_x += "0_";
                            }

                            try    
                            {
                                gyr_y += singleReadings[4] + "_";
                                testInt = Convert.ToInt32(singleReadings[4]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                gyr_y += "0_";
                            }

                            try
                            {
                                gyr_z += singleReadings[5] + "_";
                                testInt = Convert.ToInt32(singleReadings[5]);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                gyr_z += "0_";
                            }
                        }
                    }

                    var textToSend = "{\"acc_x\": \"" + acc_x.Substring(0, acc_x.Length - 1) + "\", " +
                                     "\"acc_y\": \"" + acc_y.Substring(0, acc_y.Length - 1) + "\", " +
                                     "\"acc_z\": \"" + acc_z.Substring(0, acc_z.Length - 1) + "\", " +
                                     "\"gyr_x\": \"" + gyr_x.Substring(0, gyr_x.Length - 1) + "\", " +
                                     "\"gyr_y\": \"" + gyr_y.Substring(0, gyr_y.Length - 1) + "\", " +
                                     "\"gyr_z\": \"" + gyr_z.Substring(0, gyr_z.Length - 1) + "\", " +
                                     "\"timestamp\" :" + timestamp +
                                     "}";
                    // Show application 
                    var data = new EventData(Encoding.UTF8.GetBytes(textToSend))
                    {
                        PartitionKey = "0",
                    };
                    data.Properties.Add("Type", "Message");
                    evHubClient.Send(data);

                    // Done! 
                    client.Close();
                }
                catch(Exception) {
                    client.Close();
                }
            }
        }
    }


