using ShareBook.BusinessLayer;
using ShareBook.BusinessLayer.Results;
using ShareBook.Entities;
using ShareBook.Entities.EntityViewModels;
using ShareBook.WebApp.Filters;
using ShareBook.WebApp.Helpers;
using ShareBook.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class SliderController : Controller
    {

        public ActionResult Index()
        {

            return View();
        }



        public JsonResult GetSliderList()
        {
            using (SliderManager sliderManager = new SliderManager())
            {
                List<SliderViewModel> sliderList = sliderManager.ListQueryable().Where(x => !x.isDeleted).Select(x => new SliderViewModel
                {
                    Id = x.Id,
                    imageUrl = x.imageUrl,
                    isActive = x.isActive,
                    isDeleted = x.isDeleted,
                    Name = x.Name,
                    Priority = x.Priority,
                    Url = x.Url


                }).ToList();


                return Json(sliderList, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpGet]
        public JsonResult GetSliderById(int id)
        {
            using (SliderManager sliderManager = new SliderManager())
            {
                var model = sliderManager.Find(x => x.Id == id);


                return Json(new Slider()
                {
                    Id = model.Id,
                    imageUrl = model.imageUrl,
                    Url = model.Url,
                    Priority = model.Priority,
                    Name = model.Name,
                    isDeleted = model.isDeleted,
                    isActive = model.isActive,



                }, JsonRequestBehavior.AllowGet);
            }

        }



        [HttpPost]
        public ActionResult SaveDataInDatabase(Slider slider, HttpPostedFileBase file)
        {
            using (SliderManager sliderManager = new SliderManager())
            {




                int result = -1;
                if (ModelState.IsValid)
                {
                    if (slider.Id > 0)
                    {

                        var sliderItems = sliderManager.Find(x => x.Id == slider.Id);

                        BusinessLayerResult<Slider> res = sliderManager.priorityControlUpdate(slider);
                        if (res.Errors.Count > 0)
                        {
                            ErrorViewModel errorObj = new ErrorViewModel()
                            {
                                Items = res.Errors,
                                Tittle = "Slider Güncellenemedi...",
                                RedirectingUrl = "/Admin/Slider/Index/" + slider.Id,


                            };
                            return View("AdminError", errorObj);
                        }

                        var deleted = HttpContext.Server.MapPath("~" + sliderItems.imageUrl);
                        sliderItems.Name = slider.Name;
                        sliderItems.Url = slider.Url;
                        if (file != null)
                        {
                            if (System.IO.File.Exists(deleted))
                            {
                                System.IO.File.Delete(deleted);
                            }

                            FileHelper filehelper = new FileHelper();
                            var mpath = Server.MapPath("~/Images/Slider");

                            sliderItems.imageUrl = filehelper.SaveImage(file, mpath, "/Images/Slider");
                        }
                        //editUrun.UrunResimUrl = @"/Images/Product/" + editUrun.UrunID;
                        sliderItems.isActive = slider.isActive;
                        sliderItems.Priority = slider.Priority;
                        if (sliderManager.Update(sliderItems) > 0)
                        {
                            result = 1;
                        }
                    }
                    else
                    {

                        BusinessLayerResult<Slider> res = sliderManager.priorityControlInsert(slider);


                        if (res.Errors.Count > 0)
                        {
                            ErrorViewModel errorObj = new ErrorViewModel()
                            {
                                Items = res.Errors,
                                Tittle = "Slider Eklenemedi...",
                                RedirectingUrl = "/Admin/Slider/Index",


                            };
                            return View("AdminError", errorObj);
                        }

                        if (file != null)
                        {
                            FileHelper filehelper = new FileHelper();
                            var mpath = Server.MapPath("~/Images/Slider");

                            slider.imageUrl = filehelper.SaveImage(file, mpath, "/Images/Slider");
                        }



                        sliderManager.Insert(slider);

                        //var model = sliderManager.ListQueryable().Where(x => !x.isDeleted).OrderBy(x => x.Priority).ToList();


                        result = 2;

                    }

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View(slider);
                }



            }


        }




        public ActionResult Delete(int id)
        {
            using (SliderManager sliderManager = new SliderManager())
            {
                Slider slider = sliderManager.Find(x => x.Id == id);

                if (slider == null)
                {
                    return new HttpNotFoundResult();
                }

                slider.isDeleted = true;

                if (sliderManager.Update(slider) > 0)
                {
                    return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }

        }



        //[HttpGet]
        //public JsonResult GetCategoryById(int id)
        //{
        //    using (CategoryManager categoryManager = new CategoryManager())
        //    {
        //        string value = string.Empty;
        //        var model = categoryManager.ListQueryable().Where(x => x.Id == id).SingleOrDefault();


        //        return Json(new Category()
        //        {
        //            Id = model.Id,
        //            Description = model.Description,
        //            Title = model.Title,
        //            Icon = model.Icon,
        //            isActive = model.isActive,



        //        }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //[HttpPost]
        //public JsonResult SaveDataInDatabase(Category category)
        //{
        //    using (CategoryManager categoryManager = new CategoryManager())
        //    {
        //        int result = -1;

        //        if (category.Id > 0)
        //        {
        //            Category cat = categoryManager.Find(x => x.Id == category.Id);

        //            cat.Title = category.Title;
        //            cat.Description = category.Description;
        //            cat.isActive = category.isActive;
        //            cat.Icon = category.Icon;
        //            if (categoryManager.Update(cat) > 0)
        //            {
        //                result = 1;
        //            }



        //        }
        //        else
        //        {
        //            categoryManager.Insert(category);
        //            result = 2;
        //        }


        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //public ActionResult Delete(int id)
        //{

        //    using (CategoryManager categoryManager = new CategoryManager())
        //    {
        //        Category category = categoryManager.Find(x => x.Id == id);

        //        if (category == null)
        //        {
        //            return new HttpNotFoundResult();
        //        }

        //        category.isDeleted = true;

        //        if (categoryManager.Update(category) > 0)
        //        {
        //            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        //        }


        //        return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
        //    }

        //}


        //private SliderManager sliderManager = new SliderManager();
        //public ActionResult Index()
        //{
        //    var sliderListesi = sliderManager.ListQueryable().Where(x => !x.isDeleted).OrderBy(x=>x.Priority).ToList();
        //    return View(sliderListesi);
        //}

        //public ActionResult Insert()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Insert(Slider sld, HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        BusinessLayerResult<Slider> res = sliderManager.priorityControlInsert(sld);


        //        if (res.Errors.Count > 0)
        //        {
        //            ErrorViewModel errorObj = new ErrorViewModel()
        //            {
        //                Items = res.Errors,
        //                Tittle = "Slider Eklenemedi...",
        //                RedirectingUrl = "/Admin/Slider/Insert",


        //            };
        //            return View("AdminError", errorObj);
        //        }

        //        if (file != null)
        //        {
        //            FileHelper filehelper = new FileHelper();
        //            var mpath = Server.MapPath("~/Images/Slider");

        //            sld.imageUrl = filehelper.SaveImage(file, mpath, "/Images/Slider");
        //        }



        //        sliderManager.Insert(sld);

        //        var model = sliderManager.ListQueryable().Where(x => !x.isDeleted).OrderBy(x => x.Priority).ToList();
        //        return View("Index", model);



        //    }
        //    else
        //    {
        //        return View(sld);
        //    }
        //}
        //public ActionResult Edit(int id)
        //{
        //    var editSlider = sliderManager.Find(x => x.Id == id);
        //    return View(editSlider);
        //}

        //[HttpPost]
        //public ActionResult Edit(Slider slditem, HttpPostedFileBase file)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var sliderItems = sliderManager.Find(x => x.Id == slditem.Id);

        //        BusinessLayerResult<Slider> res = sliderManager.priorityControlUpdate(slditem);
        //        if (res.Errors.Count > 0)
        //        {
        //            ErrorViewModel errorObj = new ErrorViewModel()
        //            {
        //                Items = res.Errors,
        //                Tittle = "Slider Güncellenemedi...",
        //                RedirectingUrl = "/Admin/Slider/Edit/"+slditem.Id,


        //            };
        //            return View("AdminError", errorObj);
        //        }

        //        var deleted = HttpContext.Server.MapPath("~" + sliderItems.imageUrl);
        //        sliderItems.Name = slditem.Name;
        //        sliderItems.Url = slditem.Url;
        //        if (file != null)
        //        {
        //            if (System.IO.File.Exists(deleted))
        //            {
        //                System.IO.File.Delete(deleted);
        //            }

        //            FileHelper filehelper = new FileHelper();
        //            var mpath = Server.MapPath("~/Images/Slider");

        //            sliderItems.imageUrl = filehelper.SaveImage(file, mpath, "/Images/Slider");
        //        }
        //        //editUrun.UrunResimUrl = @"/Images/Product/" + editUrun.UrunID;
        //        sliderItems.isActive = slditem.isActive;
        //        sliderItems.Priority = slditem.Priority;
        //        sliderManager.Update(sliderItems);

        //        var model = sliderManager.ListQueryable().Where(x => !x.isDeleted).OrderBy(x => x.Priority).ToList();


        //        return View("Index", model);
        //    }
        //    else
        //    {
        //        return View(slditem);
        //    }

        //}

    }




}