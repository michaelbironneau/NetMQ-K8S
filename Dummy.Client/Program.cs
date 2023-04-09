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
    
    greetingClient.Connect("tcp://greetings-service:5555");
    while (true)
    {
        Console.WriteLine("Sending '{0}'", GreetedName);
        greetingClient.SendFrame(GreetedName);
        var greeting = greetingClient.ReceiveFrameString();
        Console.WriteLine("Received greeting: {0}", greeting);
        Thread.Sleep(2500); // wait until next request
    }
}
