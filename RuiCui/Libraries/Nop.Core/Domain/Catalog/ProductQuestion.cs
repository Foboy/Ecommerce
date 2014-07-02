using System;
using System.Collections.Generic;
using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product review
    /// </summary>
    public partial class ProductQuestion : BaseEntity
    {


        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the question text
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// OrderNumË³Ðò
        /// </summary>
        public int OrderNum { get; set; }


      
    }
}
