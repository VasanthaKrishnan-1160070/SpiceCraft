using SpiceCraft.Server.Context;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpiceCraftContext _context;
        private IGenericRepository<Product> _products;

        public UnitOfWork(SpiceCraftContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> Products
        {
            get
            {
                return _products ??= new GenericRepository<Product>(_context);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
