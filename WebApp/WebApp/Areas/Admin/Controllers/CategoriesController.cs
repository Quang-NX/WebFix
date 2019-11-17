﻿using AutoMapper;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoriesController : Controller
    {
        EcommerceDbContext db = new EcommerceDbContext();
        // GET: Categories
        public ActionResult Index()
        {
            var category = db.Categories.ToList();
            var categoryViewModel = Mapper.Map<IEnumerable<CategoryViewModel>>(category);
            return View(categoryViewModel);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var categoryViewModel = Mapper.Map<CategoryViewModel>(category);
            return View(categoryViewModel);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        public ActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = Mapper.Map<Category>(categoryViewModel);
                category.Status = Domain.Enum.CommonStatus.Active;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var categoryViewModel = Mapper.Map<CategoryViewModel>(category);
            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public ActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = db.Categories.Find(categoryViewModel.Id);
                if (category == null)
                {
                    return HttpNotFound();
                }

                Mapper.Map(categoryViewModel, category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            var categoryViewModel = Mapper.Map<CategoryViewModel>(category);
            return View(categoryViewModel);
        }

        // POST: Categories/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            List<Product> product = db.Products.Where(p => p.CategoryId == id).ToList();

            foreach (var item in product)
            {
                db.Products.Remove(item);
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}