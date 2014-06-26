using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Services.Stores;

namespace Nop.Services.Catalog
{
    public partial class CategorySpecificationAtrributeService : ICategorySpecificationAtrributeService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : categorySpecificationAtrribute ID
        /// </remarks>
        private const string CATEGORYSPECIFICATIONATTRIBUTE_BY_ID_KEY = "Nop.categoryspecificationatrribute.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : categorySpecificationAtrribute ID
        /// {1} : store ID
        /// </remarks>
        private const string CATEGORYSPECIFICATIONATTRIBUTE_BY_CATEGORY_ID_KEY = "Nop.categoryspecificationatrribute.category-{0}-{1}";
       
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CATEGORYSPECIFICATIONATTRIBUTE_PATTERN_KEY = "Nop.categoryspecificationatrribute.";
      

        #endregion

        #region Fields

        private readonly IRepository<CategorySpecificationAtrribute> _categorySpecificationAtrributeRepository;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IAclService _aclService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="categorySpecificationAtrributeRepository">CategorySpecificationAtrribute repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="aclService">ACL service</param>
        public CategorySpecificationAtrributeService(ICacheManager cacheManager,
            IRepository<CategorySpecificationAtrribute> categorySpecificationAtrributeRepository,
            IRepository<AclRecord> aclRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IWorkContext workContext,
            IStoreContext storeContext,
            IEventPublisher eventPublisher,
            IStoreMappingService storeMappingService,
            IAclService aclService)
        {
            this._cacheManager = cacheManager;
            this._categorySpecificationAtrributeRepository = categorySpecificationAtrributeRepository;
            this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._eventPublisher = eventPublisher;
            this._storeMappingService = storeMappingService;
            this._aclService = aclService;
        }

        #endregion

        public virtual IList<CategorySpecificationAtrribute> LoadCategorySpecificationAtrributeById(int categoryId)
        {
            string key = string.Format(CATEGORYSPECIFICATIONATTRIBUTE_BY_CATEGORY_ID_KEY, categoryId, _storeContext.CurrentStore.Id);
            return _cacheManager.Get(key, () =>
            {
                var query = _categorySpecificationAtrributeRepository.Table;
                query = query.Where(c => c.CategoryId == categoryId);
                query = query.Where(c => !c.Deleted);
                    //only distinct categories (group by ID)
                    //query = from c in query
                    //        group c by c.Id
                    //            into cGroup
                    //            orderby cGroup.Key
                    //            select cGroup.FirstOrDefault();
                    //query = query.OrderBy(c => c.CategoryId);
                
                var categories = query.ToList();
                return categories;
            });
        }

        public virtual void InsertCategorySpecificationAtrribute(CategorySpecificationAtrribute categorySpecificationAtrribute)
        {
            if (categorySpecificationAtrribute == null)
                throw new ArgumentNullException("categorySpecificationAtrribute");

            _categorySpecificationAtrributeRepository.Insert(categorySpecificationAtrribute);

            //cache
            _cacheManager.RemoveByPattern(CATEGORYSPECIFICATIONATTRIBUTE_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(categorySpecificationAtrribute);
        }

        public virtual void UpdateCategorySpecificationAtrribute(CategorySpecificationAtrribute categorySpecificationAtrribute)
        {

            if (categorySpecificationAtrribute == null)
                throw new ArgumentNullException("categorySpecificationAtrribute");

            //validate category hierarchy
            var CategorySpecificationAtrribute = GetCategorySpecificationAtrributeBySid(categorySpecificationAtrribute.SpecificationAttributeId);

            if (CategorySpecificationAtrribute != null)
            {
                CategorySpecificationAtrribute.AllowFiltering = categorySpecificationAtrribute.AllowFiltering;
                CategorySpecificationAtrribute.Deleted = categorySpecificationAtrribute.Deleted;
                CategorySpecificationAtrribute.CategoryId = categorySpecificationAtrribute.CategoryId;
                CategorySpecificationAtrribute.SpecificationAttributeId = categorySpecificationAtrribute.SpecificationAttributeId;
                _categorySpecificationAtrributeRepository.Update(CategorySpecificationAtrribute);

            }
            //cache
            _cacheManager.RemoveByPattern(CATEGORYSPECIFICATIONATTRIBUTE_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(categorySpecificationAtrribute);
        }


        public virtual CategorySpecificationAtrribute GetCategorySpecificationAtrributeBySid(int specificationAttributeId)
        {
            var query = _categorySpecificationAtrributeRepository.Table;
            return query.FirstOrDefault(x => x.SpecificationAttributeId == specificationAttributeId);
        }


        public virtual void DeleteCategorySpecificationAtrribute(CategorySpecificationAtrribute categorySpecificationAtrribute)
        {
            if (categorySpecificationAtrribute == null)
                throw new ArgumentNullException("categorySpecificationAtrribute");

            categorySpecificationAtrribute.Deleted = true; 
            UpdateCategorySpecificationAtrribute(categorySpecificationAtrribute);
        }

        /// <summary>
        /// 获取CategorySpecificationAtrribute表所有数据
        /// </summary>
        /// <returns></returns>
        public virtual IList<CategorySpecificationAtrribute> LoadAllCategorySpecificationAtrribute()
        {
            var query = _categorySpecificationAtrributeRepository.Table;
            return query.ToList();
        }
    }
}
