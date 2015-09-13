using System;

namespace SSExec.Button.Core
{
    public abstract class DisposeObject : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposeObject()
        {
            Dispose(false);
        }


        private bool _disposed;


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free magaged resorces
                //

                DisposeManagedResources();
            }

            // free unmanaged resources
            //

            DisposeUnManagedResources();
            _disposed = true;
        }

        protected virtual void DisposeManagedResources()
        {
        }

        protected virtual void DisposeUnManagedResources()
        {
        }
    }
}