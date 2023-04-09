
using System.Data;
using MySqlConnector;
using NetMQ;
using NetMQ.Sockets;

/**
---------------------
Greeting Service
---------------------
Receives a request containing <name> and replies with "Hello, <name>"
**/

const string DatabaseConnectionString = "server=database;uid=root;pwd=thisisademo";

string MakeDBRequest(MySqlConnection connection){
    using (MySqlCommand cmd = connection.CreateCommand())
                {
                    string result = "";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 300;
                    cmd.CommandText = "SELECT 'Special greetings from the database!'";

                    object objValue = cmd.ExecuteScalar();
                    if (objValue == null)
                    {
                        cmd.Dispose();
                        return string.Empty;
                    }
                    else
                    {
                        result = (string)cmd.ExecuteScalar();
                        cmd.Dispose();
                    }

                    if (result == null)
                        return string.Empty;
                    else
                        return result;                        
                }                    
}

using (var mysqlconnection = new MySqlConnection(DatabaseConnectionString)){
    using (var server = new ResponseSocket())
    {
        mysqlconnection.Open();
        server.Bind("tcp://*:5555");
        while (true)
        {
            var name = server.ReceiveFrameString();
            Console.WriteLine("Received name: {0}", name);

            if (name.ToUpper() == "DATABASE"){
                // magic string to demonstrate db connectivity
                string response = MakeDBRequest(mysqlconnection);
                server.SendFrame(response);
                Console.WriteLine("Returned database greeting: '{0}'", response);
            } else {
                Console.WriteLine("Sending 'Hello, {0}'", name);
                server.SendFrame("Hello, " + name);
            }
        }
    }
}
