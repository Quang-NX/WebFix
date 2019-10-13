﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.ViewDto
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Nhập tên đăng nhập !")]
        [DisplayName("Tên đăng nhập")]
        public string UserName { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Nhập mật khẩu !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Xác nhận mật khẩu !")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

        [MaxLength(250)]
        [DisplayName("Họ")]
        public string FirstName { get; set; }

        [MaxLength(250)]
        [DisplayName("Tên")]
        public string LastName { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
    }
}