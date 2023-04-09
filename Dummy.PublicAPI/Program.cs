using NetMQ;
using NetMQ.Sockets;

/**
---------------------
Greeting Public API
---------------------
Requests a greeting for the given name (eg Ben) from the backend and receives the response.
**/

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000");
var app = builder.Build();

String GetGreetingBlocking(RequestSocket greetingClient, string name){
    Console.WriteLine("Sending '{0}'", name);
    greetingClient.SendFrame(name);
    var greeting = greetingClient.ReceiveFrameString();
    Console.WriteLine("Received greeting: {0}", greeting);
    return greeting;
}

using (var greetingClient = new RequestSocket())
{
    app.MapGet("/", () => "API up and running");
    app.MapGet("/greet/{name}", (string name) => GetGreetingBlocking(greetingClient, name));
    greetingClient.Connect("tcp://greetings-service:5555");
    app.Run();
}
