using IBM.WatsonDeveloperCloud.SpeechToText.v1;
using IBM.WatsonDeveloperCloud.SpeechToText.v1.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCleaner
{
    public class SpeechToTextCleaner
    {
        private SpeechToTextService _speechToText;
        private List<string> customizationNamesToDelete = new List<string>() { "model_test", "unity-test-customization", "java-sdk-temporary" };

        public SpeechToTextCleaner(string username, string password)
        {
            _speechToText = new SpeechToTextService(username, password);
        }

        public void Clean()
        {
            Console.WriteLine(string.Format("\nCleaning SpeechToText()..."));

            List<string> customizationsToDelete = GetCustomizations();

            if (customizationsToDelete != null && customizationsToDelete.Count > 0)
            {
                for(int i = customizationsToDelete.Count - 1; i >= 0; i--)
                {
                    string customizationToDelete = customizationsToDelete[i];
                    if (DeleteCustomization(customizationToDelete))
                        customizationNamesToDelete.Remove(customizationToDelete);
                }
            }
            else
            {
                Console.WriteLine("\nThere are no customizations to delete!");
            }

            Console.WriteLine(string.Format("\nClean Speech to Text complete"));
        }

        private List<string> GetCustomizations()
        {
            var result = _speechToText.ListCustomModels();
            var customizationsToDelete = new List<string>();

            if (result != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                foreach (Customization customization in result.Customization)
                {
                    if(shouldDelete(customization))
                    {
                        Console.WriteLine(string.Format("Setting customization {0} with name {1} to delete.", customization.CustomizationId, customization.Name));
                        customizationsToDelete.Add(customization.CustomizationId);
                    }
                }
            }
            else
            {
                Console.WriteLine("result is null.");
            }

            return customizationsToDelete;
        }

        private bool DeleteCustomization(string customizationId)
        {
            if(!string.IsNullOrEmpty(customizationId))
            {
                var result = _speechToText.DeleteCustomModel(customizationId);

                if (result != null)
                {
                    Console.WriteLine(string.Format("Deleted {0}.", customizationId));
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private bool shouldDelete(Customization customization)
        {
            return customizationNamesToDelete.Contains(customization.Name) && customization.Status != "pending";
        }
    }
}
