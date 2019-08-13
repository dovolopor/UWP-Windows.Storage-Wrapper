using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace irregularApp.DataModel
{
    public class StorageApi
    {
        public static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        // Write data to a file
        public static async Task WriteToFile(string relativePath, string data)
        {
            StorageFile sampleFile = await localFolder.CreateFileAsync(relativePath,
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, data);
        }

        public static async void deleteTheFile(string relativePath)
        {
            try
            {
                StorageFile file = await localFolder.GetFileAsync(relativePath);
                await file.DeleteAsync();
            }

            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                // Get information from the exception, then throw
                // the info to the parent method.
                if (e.Source != null)
                {
                    Debug.WriteLine("IOException source: {0}", e.Source);
                }
                else
                {
                    Debug.WriteLine("IOException");
                }
                
            }
        }

        // Read data from a file
        public static async Task<string> ReadFromFile(string relativePath, string backupPath = "")
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync(relativePath);
                return await FileIO.ReadTextAsync(sampleFile);
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine( "Relative path: {0}, backupPath: {1}, Error str: {2}", relativePath, backupPath, e.Message);

                if (backupPath == "")
                    return "";
                else
                {
                    await CopyFile(backupPath, relativePath);
                    return await ReadFromFile(relativePath);
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }

            return "";
        }

        public static async Task CopyFile(string src, string relativeDst)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri(src));
                await file.CopyAsync(localFolder, relativeDst, NameCollisionOption.ReplaceExisting);
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
