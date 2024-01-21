using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Library
{
    internal class SaveLoadSystem
    {
        public T LoadData<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        return (T)formatter.Deserialize(fileStream);
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from {filePath}: {ex.Message}");
                return default(T);
            }
        }

        public void SaveData<T>(T data, string filePath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data to {filePath}: {ex.Message}");
            }
        }
    }
}
