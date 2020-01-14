// <copyright file="CategoriesRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using System.Linq;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The categories repository.
    /// </summary>
    public class CategoriesRepository
    {
        private LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">The library database.</param>
        public CategoriesRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Add a new category.
        /// </summary>
        /// <param name="category">The new category.</param>
        /// <returns>If category was added.</returns>
        public bool AddCategory(Category category)
        {
            this.libraryDb.Categories.Add(category);
            var successful = this.libraryDb.SaveChanges() != 0;
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
            return this.libraryDb.Categories.FirstOrDefault(c => c.Name.Equals(name));
        }
    }
}