namespace LibraryManagement.Util
{
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity;

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