
using Users.Core.Entities;

namespace Users.Application.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        void Commit();
    }
}