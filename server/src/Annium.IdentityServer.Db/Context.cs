using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Annium.IdentityServer.Db
{
    internal class Context : DbContext, IContext
    {
        public virtual DbSet<Entities.App> AppsSet { get; set; }

        public ITable<Entities.App> Apps => AppsSet.ToLinqToDBTable();

        public Context(DbContextOptions contextOptions) : base(contextOptions) { }

        public DataConnection GetDataConnection() => this.CreateLinqToDbConnection();

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}