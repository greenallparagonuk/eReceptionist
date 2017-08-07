using System;
using eReceptionist.Models;
using LiteDB;
using Microsoft.AspNetCore.Hosting;

namespace eReceptionist.Data
{
    public class DbContext : IDisposable
    {
        private readonly LiteDatabase _database;
        private IHostingEnvironment _env;
        public DbContext(IHostingEnvironment env)
        {
            _env = env;
            _database = new LiteDatabase(System.IO.Path.Combine(_env.WebRootPath, "RoySecure.db"));            
        }
        public LiteCollection<CompanyDetails> Company => _database.GetCollection<CompanyDetails>("companydetails");
        public LiteCollection<stafflog> StaffLog => _database.GetCollection<stafflog>("stafflog");
        public LiteCollection<VisitorCompany> VisitorCompany => _database.GetCollection<VisitorCompany>("visitorcompany");
        public LiteCollection<visitorlog> VisitorLog => _database.GetCollection<visitorlog>("visitorlog");
        public LiteCollection<Meetings> meetings => _database.GetCollection<Meetings>("meetings");
        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _database.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DbContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}