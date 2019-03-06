using Newtonsoft.Json;
using ShareBook.BusinessLayer;
using ShareBook.Entities;
using ShareBook.Entities.EntityViewModels;
using ShareBook.Entities.ValueObjects;
using ShareBook.WebApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class CategoryController : Controller
    {
        // GET: Admin/Category

        public ActionResult Index()
        {
            return View();
           
        }

        public JsonResult GetCategoryList()
        {
            using (CategoryManager categoryManager = new CategoryManager())
            {
                List<CategoryViewModel> categoryList = categoryManager.ListQueryable().Where(x => !x.isDeleted).Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Icon = x.Icon,
                    isActive = x.isActive,
                    CreatedOn = x.CreatedOn.ToString(),
                    ModifiedOn = x.ModifiedOn.ToString(),
                    ModifiedBy = x.ModifiedBy,
                    isDeleted = x.isDeleted,

                }).ToList();


                return Json(categoryList, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpGet]
        public JsonResult GetCategoryById(int id)
        {
            using (CategoryManager categoryManager = new CategoryManager())
            {
               
                var model = categoryManager.ListQueryable().Where(x => x.Id == id).SingleOrDefault();


                return Json(new Category()
                {
                    Id = model.Id,
                    Description = model.Description,
                    Title = model.Title,
                    Icon = model.Icon,
                    isActive = model.isActive,



                }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult SaveDataInDatabase(Category category)
        {
            using (CategoryManager categoryManager = new CategoryManager())
            {
                int result = -1;

                if (category.Id > 0)
                {
                    Category cat = categoryManager.Find(x => x.Id == category.Id);

                    cat.Title = category.Title;
                    cat.Description = category.Description;
                    cat.isActive = category.isActive;
                    cat.Icon = category.Icon;
                    if (categoryManager.Update(cat) > 0)
                    {
                        result = 1;
                    }



                }
                else
                {
                    categoryManager.Insert(category);
                    result = 2;
                }


                return Json(result, JsonRequestBehavior.AllowGet);
            }
            
        }







        public ActionResult Delete(int id)
        {

            using (CategoryManager categoryManager = new CategoryManager())
            {
                Category category = categoryManager.Find(x => x.Id == id);

                if (category == null)
                {
                    return new HttpNotFoundResult();
                }

                category.isDeleted = true;

                if (categoryManager.Update(category) > 0)
                {
                    return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}