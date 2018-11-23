using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit.Serialization.Common
{
    public class DisposableBase : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }

        ~DisposableBase()
        {
            Dispose(false);
        }
    }
}

