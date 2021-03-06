﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using System.Diagnostics;
using ServiceHelpers;
using Newtonsoft.Json;

namespace IntelligentKioskSample.Controls
{
    class IoTClient
    {
        private static int MESSAGE_COUNT = 5;
        private IEnumerable<SimilarFaceMatch> lastSimilarPersistedFaceSample;
        // String containing Hostname, Device Id & Device Key in one of the following formats:
        //  "HostName=<iothub_host_name>;DeviceId=<device_id>;SharedAccessKey=<device_key>"
        //  "HostName=<iothub_host_name>;CredentialType=SharedAccessSignature;DeviceId=<device_id>;SharedAccessSignature=SharedAccessSignature sr=<iot_host>/devices/<device_id>&sig=<token>&se=<expiry_time>";
        private const string DeviceConnectionString = "HostName=kioskreal.azure-devices.net;DeviceId=realtime;SharedAccessKey=DL1ltOIl9z8ZzVLXSeaEZdYRZFHt0Scche2d5mDXdDI=";

        public async static Task Start(SimilarFaceMatch item)
        {
            try
            {
               // DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Http1);

                System.Diagnostics.Debug.WriteLine("enter iot");
                Random rand = new Random();
                DeviceClient serviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, Microsoft.Azure.Devices.Client.TransportType.Http1);
                // var serviceClient = Microsoft.Azure.Devices.ServiceClient.CreateFromConnectionString(connectionString, TransportType.Amqp);
                var str = "Hello";
                var dict = new Dictionary<string, string>();
                dict.Add("id", item.Face.FaceId.ToString());
                dict.Add("gender", item.Face.FaceAttributes.Gender);
                dict.Add("age", item.Face.FaceAttributes.Age.ToString());
                dict.Add("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                dict.Add("smile", item.Face.FaceAttributes.Smile.ToString());
                dict.Add("glasses", item.Face.FaceAttributes.Glasses.ToString());
                dict.Add("avgs", rand.Next(5, 8).ToString());
                dict.Add("avgrank", (3 + rand.NextDouble() * 1.5).ToString());
                dict.Add("unique", item.Unique);
                dict.Add("anger", item.Anger);
                dict.Add("contempt", item.Contempt);
                dict.Add("disgust", item.Disgust);
                dict.Add("fear", item.Fear);
                dict.Add("happiness", item.Happiness);
                dict.Add("neutral", item.Neutral);
                dict.Add("sadness", item.Sadness);
                dict.Add("surprise", item.Surprise);
                var message = new Microsoft.Azure.Devices.Message(System.Text.Encoding.ASCII.GetBytes(str));
                string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
                await serviceClient.SendEventAsync(new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(json)));
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in sample: {0}", ex.Message);
            }
        }
    }
}
