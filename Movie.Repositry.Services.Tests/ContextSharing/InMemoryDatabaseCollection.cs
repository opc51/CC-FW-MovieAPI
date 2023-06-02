using Xunit;

namespace Movie.Repository.Services.Tests.ContextSharing
{
    ///<inheritdoc/>
    [CollectionDefinition("In Memory Database Collection")]
    public class InMemoryDatabaseCollection : ICollectionFixture<InMemoryDatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
