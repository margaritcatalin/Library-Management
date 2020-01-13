// <copyright file="CategoriesRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Data_Mapper
{
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The category repository.
    /// </summary>
    public class CategoriesRepository
    {
        private LibraryDb libraryDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="libraryDb">The database connection.</param>
        public CategoriesRepository(LibraryDb libraryDb)
        {
            this.libraryDb = libraryDb;
        }

        /// <summary>
        /// Add a new category.
        /// </summary>
        /// <param name="category">The category.</param>
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
    }
}