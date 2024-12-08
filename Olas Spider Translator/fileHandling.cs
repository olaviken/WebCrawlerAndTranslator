using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Olas_Spider_Translator
{
    internal class fileHandling
    {
        /*Var = PascalCase*/
        /*Object and functions = camelCase*/
        public string getCSVFilePath()
        /*Finds the filepath for CSV file where all the URL are stored*/
        {
            string FilePath = string.Empty;

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = " CSV files (*.csv)|*.csv";
            openFile.Multiselect = false;
            openFile.Title = "Select a CSV File";
            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.InitialDirectory = Environment.CurrentDirectory;
            if (openFile.ShowDialog() == true)
            {
                FilePath = openFile.FileName;
            }
            return FilePath;
        }

        public List<string> getURL(string FilePath)
        {
            /*Extracts the URL from the CSV file*/
            List<string> URL = new List<string>();
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                while (csv.Read())
                {
                    urlInfo urlInfo = new urlInfo();
                    string url = csv.GetField<string>(0);
                    if (url != null)
                    {
                        urlInfo.URL = url;
                    }
                    URL.Add(url);
                }
            }
            return URL;
        }
        public void saveJsonFile(List<urlInfo> datalist)
        {
            /*Saves the URL, original text, translated text and language into a JSON file.*/
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "OversatteSider";
            saveFile.Filter = "JSON files (*.json)|*.json";
            saveFile.DefaultExt = ".json";
            saveFile.AddExtension = true;
            if (saveFile.ShowDialog() == true)
            {
                string Filepath = @saveFile.FileName;
                if (!string.IsNullOrEmpty(Filepath))
                {
                    string JsonString = JsonSerializer.Serialize(datalist, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(Filepath, JsonString);
                }
                else
                {
                    return;
                }
                
            }
        }
    }
}
