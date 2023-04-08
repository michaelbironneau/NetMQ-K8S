
using NetMQ;
using NetMQ.Sockets;

/**
---------------------
Greeting Service
---------------------
Receives a request containing <name> and replies with "Hello, <name>"
**/
using (var server = new ResponseSocket())
{
    server.Bind("tcp://*:5555");
    while (true)
    {
        var name = server.ReceiveFrameString();
        Console.WriteLine("Received name: {0}", name);
        // simulate some processing time
        Thread.Sleep(100);
        Console.WriteLine("Sending 'Hello, {0}'", name);
        server.SendFrame("Hello, " + name);
    }
}