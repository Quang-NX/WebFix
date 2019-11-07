﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Domain.Entities
{
    [Table("PRODUCT")]
    public class Product : BaseEntity
    {
        #region Field
        [StringLength(256)]
        public string Name { get; set; }
        public DateTime? PublicationDate { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public string UrlImage { get; set; }
        public int ProductInStock { get; set; }
        public int? View { get; set; }
        public int? QuantityBuy { get; set; }

        #endregion

        #region Relation

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public int? ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
        #endregion
    }
}
