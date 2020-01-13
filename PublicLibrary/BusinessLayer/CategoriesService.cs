// <copyright file="CategoriesService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.BusinessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Core.Internal;
    using PublicLibrary.Data_Mapper;
    using PublicLibrary.Domain_Model;

    /// <summary>
    /// The category service.
    /// </summary>
    public class CategoriesService
    {
        private CategoriesRepository categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesService"/> class.
        /// </summary>
        /// <param name="categoryRepository"> The category repository.</param>
        public CategoriesService(CategoriesRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>If category was created.</returns>
        public bool AddCategory(Category category)
        {
            if (category == null)
            {
                LoggerUtil.LogInfo($"The category is null.");
                return false;
            }

            if (category.Name.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"The category name is null or empty.");
                return false;
            }

            if (category.Name.Length < 3 || category.Name.Length > 80)
            {
                LoggerUtil.LogInfo($"The category name is invalid.");
                return false;
            }

            if (category.Name.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
            {
                LoggerUtil.LogInfo($"The category name is invalid.");
                return false;
            }

            return this.categoryRepository.AddCategory(category);
        }

        /// <summary>
        /// Check if a book is in category.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="category">The category.</param>
        /// <returns>If book is in category.</returns>
        public bool IsPartOfCategory(Book book, Category category)
        {
            return this.CategoryIsPartOfCategories(category, book.Categories.ToList());
        }

        /// <summary>
        /// Check if category is in category list.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="categories">The categories list.</param>
        /// <returns>If category is in categories.</returns>
        public bool CategoryIsPartOfCategories(Category category, List<Category> categories)
        {
            while (category != null)
            {
                if (this.CheckCategories(category, categories))
                {
                    return true;
                }

                category = category.ParentCategory;
            }

            return false;
        }

        private bool CheckCategories(Category category, List<Category> categories)
        {
            foreach (var bookCategory in categories)
            {
                var currentCategory = bookCategory;

                while (currentCategory != null)
                {
                    if (category.Name.Equals(currentCategory.Name))
                    {
                        return true;
                    }

                    currentCategory = currentCategory.ParentCategory;
                }
            }

            return false;
        }
    }
}
