using Emgu.CV;
using System.Drawing.Imaging;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace StandHostService.WebSockets;


public class Camera : WebSocketBehavior
{
    protected CancellationTokenSource? cancelTokenSource;
    protected CancellationToken token;
    protected VideoCapture capture;

    protected override void OnOpen()
    {
        base.OnOpen();
        Console.WriteLine("Connecting to camera");
        try
        {
            capture = new VideoCapture();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unable to connect");
            Console.WriteLine(ex.Message);
        }
       
        cancelTokenSource = new CancellationTokenSource();
        token = cancelTokenSource.Token;

        Task task = new Task(() =>
        {
            while (true && !token.IsCancellationRequested)
            {
                var image = capture.QueryFrame().ToBitmap();
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Bmp);
                var bytes = ms.ToArray();
                Send(bytes);
                ms.Flush();
                Thread.Sleep(30);
            }
        }, token);
        task.Start();
    }

    protected override void OnClose(CloseEventArgs e)
    {
        if (cancelTokenSource != null)
        {
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();

        }
        capture.Dispose();
        base.OnClose(e);
    }
}