using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using PublicLibrary.Data_Mapper;
using PublicLibrary.Domain_Model;

namespace PublicLibrary.BusinessLayer
{
    public class CategoriesService
    {
        private CategoriesRepository _categoryRepository;

        public CategoriesService(CategoriesRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public bool AddCategory(Category category)
        {
            if (category == null) return false;
            if (category.Name.IsNullOrEmpty()) return false;
            if (category.Name.Length < 3 || category.Name.Length > 80) return false;
            if (category.Name.Any(c=> !(char.IsLetter(c) || char.IsWhiteSpace(c)) )) return false;

            return _categoryRepository.AddCategory(category);
        }

        public bool IsPartOfCategory(Book book,Category category)
        {
            return CategoryIsPartOfCategories(category, book.Categories.ToList());
        }

        public bool CategoryIsPartOfCategories(Category category,List<Category> categories)
        {
            while (category !=null)
            {
                if (CheckCategories(category, categories))
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
