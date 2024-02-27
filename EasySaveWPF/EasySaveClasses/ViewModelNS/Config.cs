using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveClasses.ViewModelNS
{
    public class Config
    {
        private readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");


        /// <summary>
        /// Read the extensions to encrypt from the json configuration file and return the list of extensions.
        /// </summary>
        /// <returns>The list of extensions</returns>
        public ObservableCollection<string> ReadExtensionsForEncryptionFromJson(bool IsPriority)
        {
            ObservableCollection<string> listExt = [];
            if (File.Exists(configPath))
            {

                string whichExtension = convertBool2String(IsPriority);
                string jsonContent = File.ReadAllText(configPath);

                dynamic jsonObject = JsonConvert.DeserializeObject(jsonContent);
                dynamic firstConfig = jsonObject[whichExtension];

                if (firstConfig != null)
                {
                    foreach (var extension in firstConfig)
                    {
                        string extensionWithoutFirstChar = extension.ToString().Substring(1);
                        listExt.Add(extensionWithoutFirstChar);
                    }
                }
            }
            return listExt;
        }

        public string convertBool2String(bool IsPriority)
        {
            if (IsPriority)
            {
                return "ExtensionPriority";
            }
            return "ExtensionCryptage";
        }

        public void InsertExtensions(string newExt, bool IsPriority)
        {
            // Load the JSON
            string json = File.ReadAllText(configPath);
            string whichExtension = convertBool2String(IsPriority);

            dynamic configuration = JsonConvert.DeserializeObject(json);

            // Verify if "ExtensionCryptage" exist in the Json
            if (configuration[whichExtension] == null)
            {
                configuration[whichExtension] = new JArray();
            }

            // Add the new extension
            configuration[whichExtension].Add(newExt);

            string nouveauJson = JsonConvert.SerializeObject(configuration, Formatting.Indented);

            // Write the new Json file with the new changements
            File.WriteAllText(configPath, nouveauJson);
        }

        public void RemoveExtension(string itemToRemove, bool IsPriority)
        {
            string whichExtension = convertBool2String(IsPriority);
            // Load the JSON file content
            string jsonString = File.ReadAllText(configPath);

            // Parse the JSON content into a JArray since the root is an array
            var jsonArray = JArray.Parse(jsonString);

            // Access the first object in the array and then the "ExtensionCryptage" property within that object
            JArray extensionsArray = (JArray)jsonArray[whichExtension];

            // Remove the specified item from the array
            var itemToRemoveToken = extensionsArray.FirstOrDefault(x => x.ToString() == itemToRemove);
            if (itemToRemoveToken != null)
            {
                extensionsArray.Remove(itemToRemoveToken);
            }

            // Convert the modified JArray back to a string
            string updatedJsonString = jsonArray.ToString();

            // Write the updated JSON string back to the file, overwriting the original content
            File.WriteAllText(configPath, updatedJsonString);
        }

    }
}
