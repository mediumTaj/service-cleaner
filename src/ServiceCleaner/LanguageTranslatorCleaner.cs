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
        private List<string> _modelNamesToDelete = new List<string>() { "Texan" };
        private List<string> _modelStatusesToDelete = new List<string>() { "error" };

        public LanguageTranslatorCleaner(string username, string password)
        {
            _languageTranslator = new LanguageTranslatorService(username, password);
        }

        public void Clean()
        {
            Console.WriteLine(string.Format("\nCleaning LanguageTranslator()..."));

            List<string> modelsToDelete = ListModelsToDelete();

            if(modelsToDelete != null  && modelsToDelete.Count > 0)
            {
                for (int i = modelsToDelete.Count - 1; i >= 0; i--)
                {
                    string modelToDelete = modelsToDelete[i];
                    if (DeleteModel(modelToDelete))
                        modelsToDelete.Remove(modelToDelete);
                }
            }
            else
            {
                Console.WriteLine("\nThere are no models to delete!");
            }

            Console.WriteLine("\nClean Language Translator complete");
        }

        private List<string> ListModelsToDelete()
        {
            var result = _languageTranslator.ListModels();
            var modelsToDelete = new List<string>();
            if (result != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                foreach (ModelPayload model in result.Models)
                {
                    if (shouldDelete(model))
                    {
                        modelsToDelete.Add(model.ModelId);
                        Console.WriteLine($"\nInvalid model found, Setting model {0} ({1}) to delete", model.Name, model.ModelId);
                    }
                }
            }
            else
            {
                Console.WriteLine("result is null.");
            }

            return modelsToDelete;
        }

        private bool DeleteModel(string modelId)
        {
            if(!string.IsNullOrEmpty(modelId))
            {
                var result = _languageTranslator.DeleteModel(modelId);
                if(result != null)
                {
                    Console.WriteLine(string.Format("Deleted {0}.", modelId));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(modelId));
            }
        }

        private bool shouldDelete(ModelPayload model)
        {
            return _modelNamesToDelete.Contains(model.Name) || _modelStatusesToDelete.Contains(model.Status);
        }
    }
}
