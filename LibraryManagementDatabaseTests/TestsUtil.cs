// <copyright file="TestsUtil.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagementDatabaseTests
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// The test utility class.
    /// </summary>
    public class TestsUtil
    {
        /// <summary>
        /// Undoing the changes database entity level.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
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