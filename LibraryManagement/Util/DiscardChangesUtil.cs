using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LibraryManagement.Util
{
    public class DiscardChangesUtil
    {
        public static void UndoingChangesDbEntityLevel(DbContext context, object entity)
        {
            DbEntityEntry entry = context.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.Reload();
                    break;
                default: break;
            }
        }
    }
}