using System;
using RtmpSharp.Net;
using RtmpSharp.IO;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace rtmpconsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            try {
                MainAsync().Wait();
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private static async Task MainAsync()
        {
//            Socket s = new Socket(AddressFamily.Ipx, SocketType.Stream, ProtocolType.IP);
//            await s.ConnectAsync(
            const string uri = "rtmp://x.kipod.com/live/51:902b3459783c:6?signature=JJa-cs2s660ec4FEjugXHA&expires=1441112256";
            AKUtils.Trace(uri);
            var client = new RtmpClient(
                             new Uri(uri),
                             new SerializationContext(),
                             ObjectEncoding.Amf3);

            // connect to the server
            await client.ConnectAsync();
//            await client.();

            // call a remote service
//            var songs = await client.InvokeAsync<string[]>("musicalService", "findSongs");
            var streamId = await client.CreateStreamInvokeAsync();

            var play = await client.PlayInvokeAsync();//<object>("play", "51:902b3459783c:6:live");
        }
    }
}
