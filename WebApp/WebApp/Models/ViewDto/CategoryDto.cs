﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.ViewDto
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public int QuantityCate { get; set; }
        public int Id { get; set; }
        public string DisplayImage { get; set; }
    }
}