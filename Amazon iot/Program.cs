using System;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;

namespace Amazon_iot
{
    class Program
    {
        static void Main(string[] _)
        {
            string iotEndpoint = "<prefix>.iot.<Region>.amazonaws.com"; //Change the <prefix> and <Region> for your respective values
            string clientID = "<Unique Client ID>"; //Change this value for your Thing Client ID specified in its connect policy

            int brokerPort = 8883; //Default AWS MQTT port
            string topic = "Testing from .NET";
            string message = "This is a test for comunicating with aws iot thing from .net core";

            var cert = X509Certificate.CreateFromCertFile("AmazonRootCA1.crt");
            var clientCert = new X509Certificate2("certificate.cert.pfx", "");

            var client = new MqttClient(iotEndpoint, brokerPort, true, cert, clientCert, MqttSslProtocols.TLSv1_2);



            try
            {
                Console.WriteLine("Trying to publish on AWS IoT");
                client.Connect(clientID);
                Console.WriteLine($"Connected to AWS IoT with client ID: {clientID}.");

                int i = 0;
                while (true)
                {
                    string newMessage = $"{message} {i++}";
                    client.Publish(topic, Encoding.UTF8.GetBytes(newMessage));
                    Console.WriteLine($"Published: " + newMessage);
                    Thread.Sleep(5000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred" + Environment.NewLine + e.Message);
            }
        }
    }
}