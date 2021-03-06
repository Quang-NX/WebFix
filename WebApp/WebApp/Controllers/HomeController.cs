﻿using AutoMapper;
using Domain;
using Domain.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models.ViewModels;
using WebApp.Models.ViewDto;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private EcommerceDbContext db = new EcommerceDbContext();

        public ActionResult Index()
        {

            var productList = db.Products.OrderByDescending(p => p.CreatedDate).Select(s => new ProductViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                UrlImage = s.UrlImage,
                CategoryName = s.Category.Name
            }).Take(5).ToList();
            ViewBag.ProductList = db.Products.OrderBy(p => p.CreatedDate).Select(s => new ProductViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                UrlImage = s.UrlImage,
                CategoryName = s.Category.Name
            }).ToList();

            ViewBag.ListCate = db.Categories.ToList();

            return View(productList);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUserDto loginDto)
        {
            User user = new User();
            if (ModelState.IsValid)
            {
                var username = db.Users.Select(s => s.UserName);
                if (username.Contains(loginDto.UserName))
                {
                    ViewBag.Notify = "Tên tài khoản đã được sử dụng.Vui lòng chọn tên khác !";
                    return View("CreateUser", loginDto);
                }
                else
                {
                    if (loginDto.Password.Equals(loginDto.ConfirmPassword))
                    {
                        user.UserName = loginDto.UserName;
                        user.Password = loginDto.Password;
                        user.Email = loginDto.Email;
                        user.PhoneNumber = loginDto.PhoneNumber;
                        user.FirstName = loginDto.FirstName;
                        user.LastName = loginDto.LastName;
                        user.Address = loginDto.Address;
                        db.Users.Add(user);
                        db.SaveChanges();
                        Session["username"] = user;
                        Session["GioHang"] = null;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("CreateUser", "Home", loginDto);
                    }
                }
            }

            return View(loginDto);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDto loginDto)
        {
            var sTaikhoan = loginDto.UserName;
            var sMatkhau = loginDto.Password;
            User user = db.Users.FirstOrDefault(n => n.UserName.Equals(sTaikhoan) && n.Password.Equals(sMatkhau));
            if (user != null)
            {
                Session["username"] = user;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Thông tin tài khoản hoặc mật khẩu không chính xác !";
            return View(loginDto);
        }

        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Login", "Home");
        }

        //list name product
        public ActionResult ListName(string q)
        {
            var lstName = db.Products.Where(m => m.Name.Contains(q) && m.IsDeleted == false).Select(s => s.Name).ToList();

            return Json(new
            {
                data = lstName,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchPatialView()
        {
            var lstCategory = db.Categories.ToList();
            TempData["CategoryLstSearch"] = new SelectList(lstCategory, "Id", "Name");
            return PartialView(lstCategory);
        }

        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var productViewModel = Mapper.Map<ProductViewModel>(product);

            return View(productViewModel);
        }

        //phân trang
        public ActionResult PageProductDetail(int? page, int Id, double? price)
        {

            var listProduct = db.Products.Where(p => p.Category.Id.Equals(Id)).ToList();
            var lstProductViewModel = Mapper.Map<IEnumerable<ProductViewModel>>(listProduct);
            int pageSize = 2;
            //toán tử tương đương với nếu page!=null thì pageNumber =1 ngược lại =page
            int pageNumber = (page ?? 1);

            ViewBag.ListCate = db.Categories.ToList();
            ViewBag.CategoryId = Id;

            return View(lstProductViewModel.ToPagedList(pageNumber, pageSize));
        }


        //phân trang
        public ActionResult Search(string keyword, int? page/*, FormCollection formCollection*/)
        {
            //IEnumerable<Product> listProduct = null;
            ////ViewBag.ListManu = new List<ManufactureViewModel>(db.Manufacturers.Select(s => new ManufactureViewModel { Name = s.Name }));
            //int categoryId = int.Newint();

            //if (formCollection.Count!= 0)
            //{
            //    categoryId = int.Parse(formCollection["CategoryLstSearch"].ToString());
            //    Session["keysearch"] = categoryId;
            //    //nếu khách hàng chọn thể loại để tìm kiếm thì tìm kiếm theo từ khóa và thể loại
            //    listProduct = db.Products.Where(m => m.Name.Contains(keyword) && m.CategoryId == categoryId).ToList();
            //}
            //else
            //{
            //    //ngược lại chỉ tìm kiếm theo từ khóa

            //    listProduct = db.Products.Where(m => m.Name.Contains(keyword)).ToList();
            //}


            ViewBag.ListCate = db.Categories.ToList();

            var listProduct = db.Products.Where(m => m.Name.Contains(keyword)).ToList();
            var lstProductViewModel = Mapper.Map<IEnumerable<ProductViewModel>>(listProduct);
            int pageSize = 2;
            //toán tử tương đương với nếu page!=null thì pageNumber =1 ngược lại =page
            int pageNumber = (page ?? 1);
            ////get Id Category
            //ViewBag.CategoryId = categoryId;
            //lưu lại từ khóa để search
            ViewBag.KeyWord = keyword;
            return View(lstProductViewModel.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult GetFormEmail(FormCollection form, string email)
        {
            string Email = "";
            if (email != null)
            {
                Email = email;
            }
            else
            {
                Email = form["email"].ToString();
            }
            if (!string.IsNullOrEmpty(Email))
            {
                string emailAddress = Email;
                /*Cảm ơn bạn đã để lại thông tin gmail.Chúng tôi sẽ cập nhật cho bạn những thông tin mới nhất từ trang web.*/
                string content = "<h1>UETShop xin chào quý khách !</h1></br>";
                content += "<p>Cảm ơn bạn đã đăng kí nhận thông báo. !</p>";
                content += "<a href=" + "http://localhost:55666/Home/Index" + ">Quay lại trang chủ</a>";
                GuiEmail("Thư xác nhận Email từ UETShop !", emailAddress, "uetshop99@gmail.com",
                    "123456a@A", content);
                ViewBag.Notify = "Đăng kí email thành công";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //gửi email
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            //check gửi mail
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title; // tiêu đề gửi
            mail.Body = Content; // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587; //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail); //Gửi mail đi
        }
    }
}