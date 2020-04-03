using Newtonsoft.Json;
using System.IO;
using System;

namespace laba3._2
{
    public class Dictionary_objects
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool success { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary_objects obj1 = new Dictionary_objects() { id = 1, success = true, name = "Lary" };
            Dictionary_objects obj2 = new Dictionary_objects() { id = 2, success = false, name = "Rabi" };
            Dictionary_objects obj3 = new Dictionary_objects() { id = 3, success = true, name = "Alex" };
            Dictionary_objects[] dic1 = new Dictionary_objects[] { obj1, obj2, obj3 };
            string path = @"dict2.json";
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                string json = JsonConvert.SerializeObject(dic1, Formatting.Indented);
                sw.Write(json);
            }
            var obj = JsonConvert.DeserializeObject<Dictionary_objects[]>(File.ReadAllText(path));
            int count = 0;
            foreach (Dictionary_objects dic in obj)
            {
                if (dic.success == true)
                    count++;
            }
            Console.WriteLine($"Count of success == true equals {count}");
            Console.ReadKey();
        }
    }
}

