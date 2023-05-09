using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgLAB1_18240_18450
{
    internal class HTMLGenerator
    {
        private static string HTMLAElement(string fileName)
        {
            return $"<li><a href=\"http://localhost:5050/{fileName}\" target=\"_blank\">{fileName}</a></li>";
        }

        public static string KreirajResponseBody(List<string> listOfFiles)
        {
            string aElements =
                listOfFiles.Count > 0 ?
                listOfFiles.Aggregate("", (accumulator, current) =>
                {
                    return accumulator += HTMLAElement(current);
                })
                : "<h3>Nema fajlova koji zadovoljavaju zadate kriterijume.</h3>";

            string responseBody = "<HTML>" +
                                  "<BODY>" +
                                  "<ul>" +
                                  aElements +
                                  "</ul>" +
                                  "</BODY>" +
                                  "</HTML>";

            return responseBody;
        }
    }
}
