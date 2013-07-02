using System;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Hircine.Core.Connectivity
{
	 public class DefaultRavenInstanceFactory : IRavenInstanceFactory
	 {
		  #region Implementation of IRavenInstanceFactory

		  public IDocumentStore GetRavenConnection(string connectionString)
		  {
				//If RavenDB finds any connection string errors it will throw them here, and we will pass that back to the client as is.
				var connectionStringOptions = RavenConnectionStringParser.ParseNetworkedDbOptions(connectionString);

				if (string.IsNullOrWhiteSpace(connectionStringOptions.DefaultDatabase))
					throw new ArgumentException("Database", "Connection string does not contain a database name.");

				//create a new document store from the connection string
				var docStore = new DocumentStore()
							  {
									DefaultDatabase = connectionStringOptions.DefaultDatabase,
									Url = connectionStringOptions.Url,
									ApiKey = connectionStringOptions.ApiKey,
									EnlistInDistributedTransactions = connectionStringOptions.EnlistInDistributedTransactions,
									ResourceManagerId = connectionStringOptions.ResourceManagerId
							  };

			  if (connectionStringOptions.Credentials != null)
				  docStore.Credentials = connectionStringOptions.Credentials;

			  return docStore;
		  }

		  public IDocumentStore GetEmbeddedInstance(bool runInMemory = true)
		  {
				return new EmbeddableDocumentStore(){RunInMemory = runInMemory};
		  }

		  #endregion
	 }
}