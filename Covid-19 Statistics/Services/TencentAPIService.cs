using Covid_19_Statistics.Models;
using Lepton_Library.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Covid_19_Statistics.Services
{
    public class TencentAPIService
    {
        public static Windows.Storage.StorageFolder storageFolder;
        public static Windows.Storage.StorageFile TencentDataTestFile;
        public static TencentAPIModel TencentAPIModel { get; set; }

        public static async Task<bool> Init()
        {
            storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                TencentDataTestFile = await storageFolder.GetFileAsync("TencentDataTest.json");
                string text = await Windows.Storage.FileIO.ReadTextAsync(TencentDataTestFile);
                if(text == "") throw new Exception();
                TencentAPIModel = Json.Deserialize<TencentAPIModel>(text);
            }
            catch (Exception)
            {
                await UpdateAsync();
            }
            return true;
        }

        public static async Task Write(string jsontext)
        {
            var stream = await TencentDataTestFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                {
                    dataWriter.WriteString(jsontext);
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a using statement as well.
        }


        public static async Task UpdateAsync()
        {
            try
            {
                TencentAPIModel result = null;
                WebRequest webRequest = HttpWebRequest.Create("https://view.inews.qq.com/g2/getOnsInfo?name=disease_h5");
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                var jsonstring = sr.ReadToEnd();
                try
                {
                    var temp = Json.Deserialize<TencentAPIDataContainer>(jsonstring);
                    result = Json.Deserialize<TencentAPIModel>(temp.data);
                    TencentDataTestFile = await storageFolder.CreateFileAsync("TencentDataTest.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    await Write(temp.data);
                }
                catch (Exception ex)
                {
                    //HandleException(DictionaryServiceException.Unsupport_Sentence, jsonstring);
                    Console.WriteLine("失败");
                }
                TencentAPIModel = result;
            }
            catch (Exception)
            {
                
            }
        }


        public static CovidDataModel GetChinaData()
        {
            return TencentAPIModel.ToStandardCovidDataModel();
        }
    }
}
