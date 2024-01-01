using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Repository
{
    // public class GenericRepository<T> : IGenericRepository<T> where T : class
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        //private EmployeeDbEntities _context = null;
        //private DbSet<T> table = null;

        // Đối tượng Entity Framework context.
        private IDbSet<T> _entities;

        // Chuỗi lưu trữ thông báo lỗi (nếu có).
        private string _errorMessage = string.Empty;

        // Biến xác định xem lớp này đã được giải phóng (disposed) chưa.
        private bool _isDisposed;

        // Constructor nhận một đối tượng Unit of Work thông qua parameter.
        // Unit of Work thường được sử dụng để quản lý tác vụ ghi và đọc từ cơ sở dữ liệu.
        public GenericRepository(IUnitOfWork<QuanLyThuVienEntities> unitOfWork)
            : this(unitOfWork.Context)
        {
        }

        // Constructor nhận một đối tượng context.
        public GenericRepository(QuanLyThuVienEntities context)
        {
            _isDisposed = false;
            Context = context;
        }

        // Property Context lưu trữ đối tượng context.
        public QuanLyThuVienEntities Context { get; set; }

        //public GenericRepository()
        //{
        //    this._context = new EmployeeDbEntities();
        //    table = _context.Set<T>();
        //}
        //public GenericRepository(EmployeeDbEntities _context)
        //{
        //    this._context = _context;
        //    table = _context.Set<T>();
        //}

        // Phương thức Table trả về một IQueryable cho phép truy vấn dữ liệu.
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        // Property Entities trả về tập hợp (DbSet) của đối tượng T.
        private IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = Context.Set<T>();
                }
                return _entities;
            }
        }

        // Phương thức Dispose giải phóng các tài nguyên.
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }

        // Phương thức GetAll trả về tất cả các đối tượng T từ cơ sở dữ liệu.
        public IEnumerable<T> GetAll()
        {
            // return table.ToList();
            return _entities.ToList();
        }

        // Phương thức GetById trả về một đối tượng T dựa trên khóa chính (ID) được chỉ định.
        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public virtual T Login(object username, object password)
        {
            return Entities.Find(username, password);
        }

        // Phương thức Insert thực hiện việc thêm một đối tượng T mới vào cơ sở dữ liệu.
        public virtual int Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                Entities.Add(entity);
                if (Context == null || _isDisposed)
                    Context = new QuanLyThuVienEntities();
                return Context.SaveChanges(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }

        // Phương thức BulkInsert thực hiện việc thêm nhiều đối tượng T vào cơ sở dữ liệu.
        public void BulkInsert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Set<T>().AddRange(entities);
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                             validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        // Phương thức Update thực hiện việc cập nhật một đối tượng T đã tồn tại trong cơ sở dữ liệu.
        public virtual int Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new QuanLyThuVienEntities();
                SetEntryModified(entity);
                return Context.SaveChanges(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        // Phương thức Delete thực hiện việc xóa một đối tượng T khỏi cơ sở dữ liệu.
        public virtual int Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                if (Context == null || _isDisposed)
                    Context = new QuanLyThuVienEntities();
                Entities.Remove(entity);
                return Context.SaveChanges(); //commented out call to SaveChanges as Context save changes will be called with Unit of work
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        // Phương thức SetEntryModified được sử dụng để đánh dấu một đối tượng T
        // là đã thay đổi để sau đó có thể lưu các thay đổi này vào cơ sở dữ liệu.
        public virtual void SetEntryModified(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}