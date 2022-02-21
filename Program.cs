using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.IO;

namespace derivative_song_app
{
    class Program
    {
        static void Main(string[] args)
        {
            API_caller.GetTopSongs();
            Console.ReadKey();
            API_caller.saveData();
        }
        
    }
}
