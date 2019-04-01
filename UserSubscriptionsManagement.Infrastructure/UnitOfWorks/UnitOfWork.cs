using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Infrastructure.Repositories;

namespace UserSubscriptionsManagement.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext Context { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var repo = new Repository<T, ApplicationDbContext>(Context);
            return repo;
        }

        public TRepository GenericRepository<TRepository>() where TRepository : class
        {
            var repo = (TRepository)Activator.CreateInstance(typeof(TRepository), Context);

            //repo.DataChanged += OnDataHistoryCreated;
            return repo;
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return Context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!this._disposed)
                {
                    if (disposing)
                    {
                        Context?.Dispose();
                    }
                }
            }
            catch
            {
                // Ignore.
            }

            this._disposed = true;
        }

        /// <summary>
        /// Disposing
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
