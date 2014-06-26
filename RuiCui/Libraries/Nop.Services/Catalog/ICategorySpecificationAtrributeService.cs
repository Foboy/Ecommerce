using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
    public partial interface ICategorySpecificationAtrributeService
    {
        /// <summary>
        /// 获取CategorySpecificationAtrribute表所有数据
        /// </summary>
        /// <returns></returns>
        IList<CategorySpecificationAtrribute> LoadAllCategorySpecificationAtrribute();
        /// <summary>
        /// 获取某个类别的所有规格规格参数集合
        /// </summary>
        /// <param name="categoryId">类别id</param>
        /// <returns></returns>
        IList<CategorySpecificationAtrribute> LoadCategorySpecificationAtrributeById(int categoryId);
        /// <summary>
        /// Inserts CategorySpecificationAtrribute
        /// </summary>
        /// <param name="categorySpecificationAtrribute">CategorySpecificationAtrribute</param>
        void InsertCategorySpecificationAtrribute(CategorySpecificationAtrribute categorySpecificationAtrribute);

        /// <summary>
        /// Updates the CategorySpecificationAtrribute
        /// </summary>
        /// <param name="categorySpecificationAtrribute">CategorySpecificationAtrribute</param>
        void UpdateCategorySpecificationAtrribute(CategorySpecificationAtrribute categorySpecificationAtrribute);

        /// <summary>
        /// 根据规格参数id获取CategorySpecificationAtrribute
        /// </summary>
        /// <param name="specificationAttributeId">specificationAttributeId</param>
        /// <returns></returns>
        CategorySpecificationAtrribute GetCategorySpecificationAtrributeBySid( int specificationAttributeId);
        /// <summary>
        /// Deletes the CategorySpecificationAtrribute
        /// </summary>
        /// <param name="categorySpecificationAtrribute">Product category</param>
        void DeleteCategorySpecificationAtrribute(CategorySpecificationAtrribute categorySpecificationAtrribute);



    }
}
