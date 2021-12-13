
using Users.Application.Repositories;
using Users.Core.Entities;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationContext _dbContext;
        private RepositoryBase<User> _users;

        public UnitOfWork(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<User> Users
        {
            get
            {
                return _users ??
                    (_users = new RepositoryBase<User>(_dbContext));
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
