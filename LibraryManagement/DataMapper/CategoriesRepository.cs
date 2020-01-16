// <copyright file="CategoriesRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

namespace LibraryManagement.DataMapper
{
    using System.Linq;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The categories repository.
    /// </summary>
    public class CategoriesRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library database.</param>
        public CategoriesRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Add a new category.
        /// </summary>
        /// <param name="category">The new category.</param>
        /// <returns>If category was added.</returns>
        public bool AddCategory(Category category)
        {
            this.libraryContext.Categories.Add(category);
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Category added successfully : {category.Name} ");
            }
            else
            {
                LoggerUtil.LogError($"Category failed to add to db : {category.Name}");
            }

            return successful;
        }

        /// <summary>
        /// Get category by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A category.</returns>
        public Category GetCategory(string name)
        {
            return this.libraryContext.Categories.FirstOrDefault(c => c.Name.Equals(name));
        }
    }
}