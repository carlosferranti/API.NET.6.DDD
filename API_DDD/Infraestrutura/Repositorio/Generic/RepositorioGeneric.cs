using Dominio.Interfaces.Generics;
using Infraestrutura.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio.Generic
{
    public class RepositorioGeneric<T> : IGeneric<T>, IDisposable where T : class
    {

        private readonly DbContextOptions<ContextoBase> _OptionsBuilder;

        public RepositorioGeneric()
        {
            _OptionsBuilder = new DbContextOptions<ContextoBase>();
        }

        public async Task Add(T Object)
        {
            using (var data = new ContextoBase(_OptionsBuilder))
            {
                await data.Set<T>().AddAsync(Object);
                await data.SaveChangesAsync();
            }
        }

        public async Task Delete(T Object)
        {
            using (ContextoBase data = new ContextoBase(_OptionsBuilder))
            {
                data.Set<T>().Remove(Object);
                await data.SaveChangesAsync();
            }
        }

        public async Task<T> GetEntityById(int Id)
        {
            using (ContextoBase data = new ContextoBase(_OptionsBuilder))
            {
                return await data.Set<T>().FindAsync(Id);
            }
        }

        public async Task<List<T>> List()
        {
            using (ContextoBase data = new ContextoBase(_OptionsBuilder))
            {
              return   await data.Set<T>().ToListAsync();                
            }
        }

        public async Task Update(T Object)
        {
            using (ContextoBase data = new ContextoBase(_OptionsBuilder))
            {
                data.Set<T>().Update(Object);
                await data.SaveChangesAsync();
            }
        }

        #region Disposed https://learn.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose#implement-the-dispose-pattern

        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public RepositorioGeneric(bool disposed, SafeHandle safeHandle)
        {
            this.disposed = disposed;
            _safeHandle = safeHandle;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                _safeHandle.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
