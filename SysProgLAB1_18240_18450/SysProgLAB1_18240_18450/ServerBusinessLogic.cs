using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysProgLAB1_18240_18450
{
    internal class ServerBusinessLogic
    {
        private static Cache _cache = new Cache(3);
        private static string CURRENT_DIRECTORY = Directory.GetCurrentDirectory();
        private static List<string> PretraziKljucnuRec(string path, string keyword)
        {
            List<string> returnLista = new List<string>();
            if (!string.IsNullOrEmpty(keyword))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.Contains(keyword))
                    {
                        returnLista.Add(fileName);
                    }
                }
                /*
                 * Za slucaj da zelimo pretragu po svim poddirektorijumima rut direktorijuma
                 * string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    returnLista.Concat(searchDirectoryForKeyword(directory, keyword));
                }*/
            }
            return returnLista;
        }
        public static void ZahtevPrikazivanjaListeFajlova(string requestUrl, HttpListenerResponse response)
        {
            Console.WriteLine($"Pretrazuju se fajlovi sa kljucnom recju: {requestUrl}");
            string responseBody;

            if (_cache.SadrziKljuc(requestUrl))
            {
                responseBody = _cache.CitajIzKesa(requestUrl);
            }
            else
            {
                responseBody = HTMLGenerator.KreirajResponseBody(PretraziKljucnuRec(CURRENT_DIRECTORY, requestUrl));
                _cache.UpisiUKes(requestUrl, responseBody);
            }
            
            byte[] responseBodyBinary = Encoding.UTF8.GetBytes(responseBody);
            response.ContentLength64 = responseBodyBinary.Length;
            response.OutputStream.Write(responseBodyBinary, 0, responseBodyBinary.Length);
            response.OutputStream.Close();
            Console.WriteLine($"Fajlovi sa kljucnom recju {requestUrl} uspesno dostavljeni.");
        }
        public static void ZahtevPreuzimanjaFajla(string requestUrl, HttpListenerResponse response)
        {
            Console.WriteLine($"Zahtevan download fajla: {requestUrl}");
            using (FileStream fs = new FileStream(requestUrl, FileMode.Open))
            {
                byte[] byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                response.ContentLength64 = byteArray.Length;
                response.AppendHeader("Content-Disposition", "attachment");
                response.OutputStream.Write(byteArray, 0, byteArray.Length);
                response.OutputStream.Close();
                Console.WriteLine($"Fajl \"{requestUrl}\" downloadovan sa servera.");
            }
        }
    }
}
