using PublicLibrary.Domain_Model;

namespace PublicLibrary.Data_Mapper
{
    public class CategoriesRepository
    {
        private LibraryDb _libraryDb;

        public CategoriesRepository(LibraryDb libraryDb)
        {
            _libraryDb = libraryDb;
        }

        public bool AddCategory(Category category)
        {
            _libraryDb.Categories.Add(category);
            var successful = _libraryDb.SaveChanges() != 0;
            if (successful) LoggerUtil.LogInfo($"Category added successfully : {category.Name} ");
            else LoggerUtil.LogError($"Category failed to add to db : {category.Name}");
            return successful; 
        }
    }
}