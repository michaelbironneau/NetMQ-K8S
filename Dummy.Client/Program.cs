using NetMQ;
using NetMQ.Sockets;

/**
---------------------
Greeting Client
---------------------
Requests a greeting for a given name (eg Ben) and receives the response.
**/
const string GreetedName = "Ben";

using (var greetingClient = new RequestSocket())
{
    
    greetingClient.Connect("tcp://localhost:5555");
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine("Sending '{0}'", GreetedName);
        greetingClient.SendFrame(GreetedName);
        var greeting = greetingClient.ReceiveFrameString();
        Console.WriteLine("Received greeting: {0}", greeting);
    }
}
