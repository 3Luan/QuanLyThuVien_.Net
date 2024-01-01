using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using WebQuanLyThuVien.Interfaces;

namespace WebQuanLyThuVien.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
           where TContext : DbContext, new()
    {
        //Here TContext is nothing but your DBContext class
        //In our example it is EmployeeDBContext class
        private readonly TContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private DbContextTransaction _objTran;
        private Dictionary<string, object> _repositories;
        //Using the Constructor we are initializing the _context variable is nothing but
        //we are storing the DBContext (EmployeeDBContext) object in _context variable
        
        public UnitOfWork()
        {
            _context = new TContext();
        }

        //The Dispose() method is used to free unmanaged resources like files, 
        //database connections etc. at any time.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //This Context property will return the DBContext object i.e. (EmployeeDBContext) object
        public TContext Context
        {
            get { return _context; }
        }

        // Khi cần thực hiện song song 2 hoặc nhiều xử lý mà đồng thời tất cả phải thành công thì tạo Transaction
        public void CreateTransaction()
        {
            _objTran = _context.Database.BeginTransaction();
        }

        // Tất cả xử lý ok thì commit (lưu database)
        public void Commit()
        {
            _objTran.Commit();
        }

        // Nếu ít nhất một trong các XỬ lý không thành công thì chúng ta cần gọi Rollback() này
        // phương thức khôi phục các thay đổi của cơ sở dữ liệu về trạng thái trước đó
        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        // Thực hiện sao lưu data
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public GenericRepository<T> GenericRepository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<T>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)_repositories[type];
        }
    }
}
