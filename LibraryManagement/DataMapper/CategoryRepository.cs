// <copyright file="CategoryRepository.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>

using System.Collections.Generic;
using System.Data.Entity;
using LibraryManagement.Util;

namespace LibraryManagement.DataMapper
{
    using System.Linq;
    using System.Reflection;
    using LibraryManagement.DomainModel;

    /// <summary>
    /// The Category repository.
    /// </summary>
    public class CategoryRepository
    {
        private readonly LibraryDbContext libraryContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="libraryContext">The library database.</param>
        public CategoryRepository(LibraryDbContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        /// <summary>
        /// Get Category by category name.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>A role.</returns>
        public Category GetCategoryByName(string categoryName)
        {
            var entity = this.libraryContext.Categories.FirstOrDefault(
                a => a.Name.Equals(categoryName));
            DiscardChangesUtil.UndoingChangesDbEntityLevel(this.libraryContext, entity);
            return this.libraryContext.Categories.FirstOrDefault(
                a => a.Name.Equals(categoryName));
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
                LoggerUtil.LogInfo($"Category added successfully : {category.Name} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Category failed to add to db : {category.Name}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Get All categories.
        /// </summary>
        /// <returns>All categories.</returns>
        public IEnumerable<Category> GetCategories()
        {
            return this.libraryContext.Categories.ToList();
        }

        /// <summary>
        /// Get category by category id.
        /// </summary>
        /// <param name="id">The category id.</param>
        /// <returns>A category.</returns>
        public Category GetCategoryById(int id)
        {
            var entity = this.libraryContext.Categories.Find(id);
            DiscardChangesUtil.UndoingChangesDbEntityLevel(this.libraryContext, entity);
            return this.libraryContext.Categories.Find(id);
        }

        /// <summary>
        /// Add a new category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>If category was updated.</returns>
        public bool UpdateCategory(Category category)
        {
            this.libraryContext.Entry(category).State = EntityState.Modified;
            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Category updated successfully : {category.Name} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Category failed to update to db : {category.Name}",
                    MethodBase.GetCurrentMethod());
            }

            return successful;
        }

        /// <summary>
        /// Delete a category.
        /// </summary>
        /// <param name="id">The category id.</param>
        /// <returns>If category was deleted.</returns>
        public bool DeleteCategory(int id)
        {
            var category = this.libraryContext.Categories.Find(id);
            if (category != null)
            {
                this.libraryContext.Categories.Remove(category);
            }
            else
            {
                LoggerUtil.LogError($"Category failed to delete from db. We don't found a category with id: {id}",
                    MethodBase.GetCurrentMethod());
                return false;
            }

            var successful = this.libraryContext.SaveChanges() != 0;
            if (successful)
            {
                LoggerUtil.LogInfo($"Category was deleted successfully : {id} ", MethodBase.GetCurrentMethod());
            }
            else
            {
                LoggerUtil.LogError($"Category failed to delete from db : {id}", MethodBase.GetCurrentMethod());
            }

            return successful;
        }
    }
}