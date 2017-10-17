using IBM.WatsonDeveloperCloud.LanguageTranslator.v2;
using IBM.WatsonDeveloperCloud.LanguageTranslator.v2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCleaner
{
    public class LanguageTranslatorCleaner
    {
        private LanguageTranslatorService _languageTranslator;
        private List<string> _customModelIdsToDelete = new List<string>();

        public LanguageTranslatorCleaner(string username, string password)
        {
            _languageTranslator = new LanguageTranslatorService(username, password);
        }

        public void Clean()
        {
            Console.WriteLine(string.Format("\nCleaning LanguageTranslator()..."));

            GetEnvironments();
            DeleteModels();
        }

        private void GetEnvironments()
        {
            var result = _languageTranslator.ListModels();

            if (result != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                foreach (ModelPayload model in result.Models)
                {
                    if (model.Status == "error" || model.Name == "Texan")
                    {
                        _customModelIdsToDelete.Add(model.ModelId);
                        Console.WriteLine($"\nInvalid model found, Setting model {0} ({1}) to delete", model.Name, model.ModelId);
                    }
                }
            }
            else
            {
                Console.WriteLine("result is null.");
            }
        }

        private void DeleteModels()
        {
            if(_customModelIdsToDelete != null && _customModelIdsToDelete.Count > 0)
            {
                foreach(string customModelIdToDelete in _customModelIdsToDelete)
                {
                    var result = _languageTranslator.DeleteModel(customModelIdToDelete);

                    Console.WriteLine($"\nSuccess: {0}", result?.Deleted);
                }
            }

            Console.WriteLine("\nClean Language Translator complete");
        }
    }
}
