using SysProgLAB1_18240_18450;
using System.Net;


const string FAVICON = "favicon.ico";
HttpListener listener = new HttpListener();
string[] prefixes = new string[] { "http://localhost:5050/","http://127.0.0.1:5050/" };

foreach(string prefix in prefixes)
{
    listener.Prefixes.Add(prefix);
}

listener.Start();

while(true)
{
    Console.WriteLine("Server slusa...");
    ThreadPool.QueueUserWorkItem(ProcessRequest,listener.GetContext());
}

void ProcessRequest(object? state)
{
    try
    {
        var context = state as HttpListenerContext;
        if (context == null)
            throw new Exception("Zahtev nije primljen.");
        var request = context.Request;
        var response = context.Response;

        string requestUrl = request.RawUrl ?? "/";
        
        requestUrl = requestUrl.Replace("/", string.Empty);

        if(requestUrl.Contains("%20"))
            requestUrl = requestUrl.Replace("%20", " ");

        if (requestUrl == FAVICON)
            return;

        if (string.IsNullOrEmpty(Path.GetExtension(requestUrl)))
            ServerBusinessLogic.ZahtevPrikazivanjaListeFajlova(requestUrl, response);
        else
            ServerBusinessLogic.ZahtevPreuzimanjaFajla(requestUrl, response);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Zahtev nije uspesno obradjen zbog: {e.Message}");
    }
}




