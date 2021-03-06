﻿using IBM.WatsonDeveloperCloud.Discovery.v1;
using IBM.WatsonDeveloperCloud.Discovery.v1.Model;
using Newtonsoft.Json;
using System;

namespace ServiceCleaner
{
    public class DiscoveryCleaner
    {
        private DiscoveryService _discovery;
        private string _environmentIdToDelete;

        public DiscoveryCleaner(string username, string password)
        {
            _discovery = new DiscoveryService(username, password, "2017-09-01");
        }

        public void Clean()
        {
            Console.WriteLine(string.Format("\nCleaning Discovery()..."));

            GetEnvironments();
            DeleteEnvironment();
        }

        private void GetEnvironments()
        {
            Console.WriteLine(string.Format("\nGetting environments..."));
            var result = _discovery.ListEnvironments();

            if (result != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                foreach(ModelEnvironment environment in result.Environments)
                {
                    if(!(bool)environment._ReadOnly)
                    {
                        _environmentIdToDelete = environment.EnvironmentId;
                        Console.WriteLine(string.Format("\nEnvironemnt found, Setting environment {0} to delete", environment.Name));
                    }
                }
            }
            else
            {
                Console.WriteLine("result is null.");
            }
        }

        private void DeleteEnvironment()
        {
            Console.WriteLine(string.Format("\nDeleting {0}...", _environmentIdToDelete));
            if(!string.IsNullOrEmpty(_environmentIdToDelete))
            {
                var result = _discovery.DeleteEnvironment(_environmentIdToDelete);

                if(result != null)
                    Console.WriteLine(string.Format("\n{0}: {1}", result.EnvironmentId, result.Status));
            }
            else
            {
                Console.WriteLine(string.Format("\nOnly readonly environment found"));
            }

            Console.WriteLine(string.Format("\nClean Discovery complete"));
        }
    }
}
