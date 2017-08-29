using IBM.WatsonDeveloperCloud.VisualRecognition.v3;
using IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model;
using System;
using System.Collections.Generic;

namespace ServiceCleaner
{
    public class VisualRecognitionCleaner
    {
        private VisualRecognitionService _visualRecognition;
        public VisualRecognitionCleaner(string apikey, string url)
        {
            _visualRecognition = new VisualRecognitionService(apikey, url);
        }

        public void Clean()
        {
            Console.WriteLine(string.Format("\nCleaning Visual Recognition..."));

            List<string> classifiersToDelete = GetClassifiers();
            List<string> classifiersRef = new List<string>(classifiersToDelete);
            foreach (string classifierId in classifiersToDelete)
            {
                try
                {
                    if (DeleteClassifier(classifierId))
                    {
                        Console.WriteLine(string.Format("{0} deleted.", classifierId));
                        classifiersRef.Remove(classifierId);
                    }
                }
                catch
                {
                    break;
                }
            }

            if (classifiersRef.Count > 0)
                Clean();
            else
                Console.WriteLine(string.Format("\nClean Visual Recognition complete"));
        }

        public List<string> GetClassifiers()
        {
            List<string> classifiers = new List<string>();
            var result = _visualRecognition.GetClassifiersBrief();

            foreach (GetClassifiersPerClassifierBrief classifier in result.Classifiers)
            {
                classifiers.Add(classifier.ClassifierId);
            }

            Console.WriteLine(string.Format("{0} classifiers found.", classifiers.Count));
            return classifiers;
        }

        public bool DeleteClassifier(string classifierId)
        {
            Console.WriteLine(string.Format("Deleting classifier {0}.", classifierId));

            var result = _visualRecognition.DeleteClassifier(classifierId);
            return result != null;
        }
    }
}
