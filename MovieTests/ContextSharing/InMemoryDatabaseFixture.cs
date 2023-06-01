using Microsoft.EntityFrameworkCore;
using Movie.Repository;
using System;

namespace MovieTests
{
    public class InMemoryDatabaseFixture : IDisposable
    {
        public APIContext _database { get; private set; }

        public InMemoryDatabaseFixture()
        {
            DbContextOptions options = new DbContextOptionsBuilder<APIContext>()
                            .UseInMemoryDatabase("MovieDatabase").Options;
            _database = new APIContext(options);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~InMemoryDatabaseFixture()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
