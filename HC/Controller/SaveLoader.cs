using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HC.Controller
{
    public class SaveLoader
    {
        public static List<T> Load<T>(string path) where T: class
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                if (fs.Length > 0 && formatter.Deserialize(fs) is List<T> items)
                {
                    return items;
                }
                else
                {
                    return default;
                }
            }
        }
        public static void Save<T>(string path,List<T> item) where T: class
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(fs, item);
            }
        }
    }
}
