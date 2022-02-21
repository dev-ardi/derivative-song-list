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
    static class API_caller
    {
        public struct SongData
        {
            public string name;
            public int[] playCount;
            public DateTime creationDate;
        }
        static readonly private string API_KEY = "8c199f4051a8bfd291ba1381d44bd79a";
        static readonly private string DATABASE_PATH = "db";

        static readonly private HttpClient client = new HttpClient();
        static public List<SongData> data;


        private static async Task<string> MakeRequest(string url)
        {
            string res = await client.GetStringAsync(url);
            return res;
        }
        private static void LoadData()
        {
            FileStream fileStream;
            try{ fileStream = File.Open(DATABASE_PATH, FileMode.Open); }
            catch 
            {
                data = new List<SongData>();
                return; 
            }
            StreamReader streamReader = new StreamReader(fileStream);
            data = new List<SongData>();
            string line;
            int count = 0;
            while ((line = streamReader.ReadLine()) != null){
                SongData song = new SongData();
                data.Append(song);
                switch(count++ % 3)
                {
                    case 0: // name
                        song.name = line;
                        break;
                    case 1: // playcount
                        string[] songs = line.Split(' ');
                        int index = 0;
                        song.playCount = new int[songs.Length + 1];
                        foreach (string i in songs)                        
                            song.playCount[index++] = Int32.Parse(i);
                        break;
                    case 2: // Date
                        song.creationDate = DateTime.Parse(line);
                        break;
                }
            }
        }

        static API_caller()
        {
            LoadData();
            client.DefaultRequestHeaders.Accept.Clear();

        }
        static public async Task GetTopSongs(string country = "spain", int elements = 500 ) 
            {
            string uri = $"http://ws.audioscrobbler.com/2.0/?method=geo.gettoptracks&country={country}&api_key={API_KEY}&limit={elements}";
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(await MakeRequest(uri));
            int index = 0;
            foreach (XmlNode xmlSong in xml.SelectNodes("/lfm/tracks/track"))
            {
                string songName = xmlSong.SelectSingleNode("name").InnerText;
                SongData elem = data.Find(song => song.name == songName);
                if (elem.name == null)
                {
                    elem = new SongData()
                    {
                        creationDate = DateTime.Now,
                        name = songName,
                        playCount = new int[1]
                    };
                }
               
                int playcount = Int32.Parse(
                    xmlSong.SelectSingleNode("listeners").InnerText);
                elem.playCount.Append(playcount);
                elem.creationDate = DateTime.Now;
                data.Append(elem);
            }
        }
        static public void saveData()
        {
            FileStream fileStream = File.Open(DATABASE_PATH, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            foreach (SongData song in data)
            {
                string[] result = song.playCount.Select(x => x.ToString()).ToArray();
                string counts = string.Join(" ", result);
                string output = $"{song.name}\n{counts}\n{song.creationDate.ToString()}";
                streamWriter.WriteLine(output);
            }
        }

    }
} 

