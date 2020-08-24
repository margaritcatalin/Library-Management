// <copyright file="CategoryService.cs" company="Transilvania University of Brasov">
// Margarit Marian Catalin
// </copyright>
// <summary>This is the Category service class.</summary>

namespace LibraryManagement.BusinessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.Core.Internal;
    using LibraryManagement.DataMapper;
    using LibraryManagement.DomainModel;
    using LibraryManagement.Util;

    /// <summary>
    /// The Category service.
    /// </summary>
    public class CategoryService
    {
        /// <summary>
        /// Defines the categoryRepository.
        /// </summary>
        private readonly CategoryRepository categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The Category repository.</param>
        public CategoryService(CategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Add a new Category.
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <returns>If Category was added.</returns>
        public bool AddCategory(Category category)
        {
            if (this.ValidateCategory(category))
            {
                return this.categoryRepository.AddCategory(category);
            }

            return false;
        }

        /// <summary>
        /// Get All AuctionUsers.
        /// </summary>
        /// <returns>All AuctionUsers.</returns>
        public IEnumerable<Category> GetCategories()
        {
            return this.categoryRepository.GetCategories();
        }

        /// <summary>
        /// Get Category by Category id.
        /// </summary>
        /// <param name="id">The Category id.</param>
        /// <returns>An Category.</returns>
        public Category GetCategoryById(int id)
        {
            return this.categoryRepository.GetCategoryById(id);
        }

        /// <summary>
        /// Update an Category.
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <returns>If Category was updated.</returns>
        public bool UpdateCategory(Category category)
        {
            if (this.ValidateCategory(category))
            {
                return this.categoryRepository.UpdateCategory(category);
            }

            return false;
        }

        /// <summary>
        /// Delete Category.
        /// </summary>
        /// <param name="id">The Category id.</param>
        /// <returns>If Category was deleted.</returns>
        public bool DeleteCategory(int id)
        {
            return this.categoryRepository.DeleteCategory(id);
        }

        /// <summary>
        /// Get All Categories For Product.
        /// </summary>
        /// <param name="product">The Product.</param>
        /// <returns>All Categories For Product.</returns>
        public IEnumerable<Category> GetAllCategoriesProduct(Product product)
        {
            List<Category> allProductCategories = new List<Category>();
            var productCategories = product.Category;
            foreach (var category in productCategories)
            {
                var currentCategory = category;
                while (currentCategory != null)
                {
                    if (!allProductCategories.Contains(currentCategory))
                    {
                        allProductCategories.Add(currentCategory);
                    }

                    currentCategory = currentCategory.ParentCategory;
                }
            }

            return allProductCategories;
        }

        /// <summary>
        /// Get Category by category name.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>The category with the specific category name.</returns>
        public Category GetCategoryByName(string categoryName)
        {
            if (this.ValidateCategoryName(categoryName))
            {
                return this.categoryRepository.GetCategoryByName(categoryName);
            }

            return null;
        }

        /// <summary>
        /// Validation for Category user.
        /// </summary>
        /// <param name="category">The Category.</param>
        /// <returns>If Category is valid or not.</returns>
        private bool ValidateCategory(Category category)
        {
            if (category == null)
            {
                LoggerUtil.LogInfo($"Category is invalid. You tried to add an null Category.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!this.ValidateCategoryName(category.Name))
            {
                LoggerUtil.LogInfo($"Category is invalid. You tried to add an Category with a wrong Name.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (category.ParentCategory_Id != null && category.Id == category.ParentCategory_Id)
            {
                LoggerUtil.LogInfo($"Your Category is invalid. Category is the same with parent category.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validation for categoryName.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>If Category is valid or not.</returns>
        private bool ValidateCategoryName(string categoryName)
        {
            if (categoryName.IsNullOrEmpty())
            {
                LoggerUtil.LogInfo($"Category is invalid. You tried to add an Category with null empty name.", MethodBase.GetCurrentMethod());
                return false;
            }

            if ((categoryName.Length < 3) || (categoryName.Length > 100))
            {
                LoggerUtil.LogInfo($"Category is invalid. You tried to add an Category with invalid lenght name.", MethodBase.GetCurrentMethod());
                return false;
            }

            if (char.IsLower(categoryName.First()))
            {
                LoggerUtil.LogInfo($"Category is invalid. You tried to add an Category with name with lower case.", MethodBase.GetCurrentMethod());
                return false;
            }

            return true;
        }
    }
}