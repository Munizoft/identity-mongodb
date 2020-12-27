
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace Munizoft.Identity.Persistence.MongoDB
{
    public class IdentityContext : MongoDbContext
    {

        public IdentityContext(IMongoDatabase mongoDatabase)
            : base(mongoDatabase)
        {
        }

        public IdentityContext(string connectionString)
          : base(connectionString)
        {
        }

        public IdentityContext(string connectionString, string databaseName)
           : base(connectionString, databaseName)
        {
        }
    }
}