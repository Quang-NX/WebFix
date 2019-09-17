﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("USER")]
    public class User : BaseEntity
    {
        #region Filed
        public string UserName { get; set; }
        [MaxLength(250)]
        public string FirstName { get; set; }
        [MaxLength(250)]
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? Age { get; set; }
        public string PhoneNumber { get; set; }

        #endregion


        #region Relationship
        public Guid? RoleId { get; set; }
        public virtual Role Role { get; set; }
        #endregion
    }
}
