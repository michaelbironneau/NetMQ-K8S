using System.Data;
using MySqlConnector;
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

string FailToMakeDbRequest()
// This should not work - NetworkPolicy should prevent it
    {
        string strData = "";
        string connectionString = "server=database;userid=root;password=thisisademo";

        try
        {                

            using (var mysqlconnection = new MySqlConnection(connectionString))
            {
                mysqlconnection.Open();
                using (MySqlCommand cmd = mysqlconnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 10;
                    cmd.CommandText = "SELECT 'This should have failed'";

                    object objValue = cmd.ExecuteScalar();
                    if (objValue == null)
                    {
                        cmd.Dispose();
                        return string.Empty;
                    }
                    else
                    {
                        strData = (string)cmd.ExecuteScalar();
                        cmd.Dispose();
                    }

                    mysqlconnection.Close();

                    if (strData == null)
                        return string.Empty;
                    else
                        return strData;                        
                }                    
            }                                
        }
        catch (MySqlException ex)
        {
            return "MySqlException connecting to database: " + ex.ToString();
        }
        catch (Exception ex)
        {
            return "Other exception connecting to database: " + ex.ToString();
        }
        finally
        {

        }
    }
using (var greetingClient = new RequestSocket())
{
    app.MapGet("/", () => "API up and running");
    app.MapGet("/database", () => FailToMakeDbRequest());
    app.MapGet("/greet/{name}", (string name) => GetGreetingBlocking(greetingClient, name));
    greetingClient.Connect("tcp://greetings-service:5555");
    app.Run();
}
