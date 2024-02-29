using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasySaveClasses.ViewModelNS
{
    public class Config
    {
        private readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../EasySaveClasses/ViewModelNS/Config.json");


        /// <summary>
        /// Read the extensions to encrypt from the json configuration file and return the list of extensions.
        /// </summary>
        /// <param name="IsPriority">True if we want to get priority extensions, False if we want to get encryption extensions</param>
        /// <returns>The list of extensions</returns>
        public ObservableCollection<string> ReadExtensionsFromJson(bool IsPriority)
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

            // Verify if it exists in the Json
            if (configuration[whichExtension] == null)
            {
                configuration[whichExtension] = new JArray();
            }

            // Add the new extension
            configuration[whichExtension].Add(newExt);

            string newJson = JsonConvert.SerializeObject(configuration, Formatting.Indented);

            // Write the new Json file with the new changements
            File.WriteAllText(configPath, newJson);
        }

        public void RemoveExtension(string itemToRemove, bool IsPriority)
        {
            string whichExtension = convertBool2String(IsPriority);

            // Load the JSON file content
            string jsonString = File.ReadAllText(configPath);

            // Parse the JSON content into a JObject since the root is an object
            var jsonObject = JObject.Parse(jsonString);

            // Access the specific extension array within the object
            JArray extensionsArray = (JArray)jsonObject[whichExtension];

            // Remove the specified item from the array
            var itemToRemoveToken = extensionsArray.FirstOrDefault(x => x.ToString() == itemToRemove);
            if (itemToRemoveToken != null)
            {
                extensionsArray.Remove(itemToRemoveToken);
            }

            // Convert the modified JObject back to a string
            string updatedJsonString = jsonObject.ToString();

            // Write the updated JSON string back to the file, overwriting the original content
            File.WriteAllText(configPath, updatedJsonString);
        }

        public void ChangeMaxFileSize(int newMaxFileSize)
        {
            string configJsonContent = File.ReadAllText(configPath);
            // Deserializing the existing JSON in a JObject object
            dynamic configuration = JsonConvert.DeserializeObject(configJsonContent);

            configuration["MaxFileSize"] = newMaxFileSize;

            // Serialization of the JObject object in JSON
            string updatedJson = JsonConvert.SerializeObject(configuration, Formatting.Indented);

            // Writing the updated JSON to the configuration file
            File.WriteAllText(configPath, updatedJson);
        }

        public int GetMaxFileSizeFromJson()
        {
            if (File.Exists(configPath))
            {
                string configJsonContent = File.ReadAllText(configPath);

                dynamic configuration = JsonConvert.DeserializeObject(configJsonContent);
                string maxFileSizeFromJson = configuration["MaxFileSize"];
                int parsedMaxFileSizeFromJson = 0;
                int.TryParse(maxFileSizeFromJson, out parsedMaxFileSizeFromJson);
                return parsedMaxFileSizeFromJson;
            }
            return 0;
        }
    }
}
