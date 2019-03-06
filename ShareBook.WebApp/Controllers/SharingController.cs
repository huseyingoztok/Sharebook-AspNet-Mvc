using ShareBook.BusinessLayer;
using ShareBook.Entities;
using ShareBook.Entities.EntityViewModels;
using ShareBook.WebApp.Filters;
using ShareBook.WebApp.Helpers;
using ShareBook.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Controllers
{
    public class SharingController : Controller
    {
        // GET: Sharing
        [AuthFilter,ExceptionFilter]
        public ActionResult Index()
        {
            using (CategoryManager categoryManager = new CategoryManager())
            {
                List<SelectListItem> categories = new List<SelectListItem>();

                categories.AddRange(categoryManager.ListQueryable().Where(x => !x.isDeleted).Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id.ToString()

                }).ToList());
                ViewBag.Categories = categories;

                return View(new SharingViewModel());
            }

        }
        [HttpGet]

        public ActionResult SharingList()
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                




                List<SharingViewModel> sharings = sharingManager.ListQueryable().Where(x => x.Owner.Id == CurrentSession.shareBookUser.Id && !x.isDelete).Select(p => new SharingViewModel
                {
                    Id = p.Id,
                    CategoryTitle = p.Category.Title,

                    Title = p.Title,
                    ShareContent = p.ShareContent,
                    LikeCount = p.Likes.Where(x => !x.likedUser.isDeleted).ToList().Count,
                    CommentCount = p.Comments.Where(x => !x.isDelete && !x.Owner.isDeleted).ToList().Count,
                    ImageUrl = p.ImageUrl,
                    isDraft = p.isDraft,
                    ModifiedOn = p.ModifiedOn,
                    CategoryId=p.CategoryId,
                    //Category=p.Category,
                    
                    
                    //CategoryId = p.CategoryId,

                    //isDelete = p.isDelete



                }).ToList();

                return Json(sharings, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public JsonResult GetShareById(int id)
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                string value = string.Empty;
                var model = sharingManager.ListQueryable().Where(x => x.Id == id&&!x.isDelete).SingleOrDefault();


                return Json(new Sharing()
                {
                   
                    Id = model.Id,
                    Title = model.Title,
                    ShareContent = model.ShareContent,
                    ImageUrl = model.ImageUrl,
                    isDraft = model.isDraft,
                    //ModifiedOn = model.ModifiedOn,
                    //Category = model.Category,
                    CategoryId = model.CategoryId,
                    //Comments = model.Comments,
                    CreatedOn = model.CreatedOn,
                    isDelete = model.isDelete,
                    //Likes = model.Likes,
                    ModifiedBy = model.ModifiedBy,
                    //Owner = model.Owner,
                    




                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveDataInDatabase(Sharing sharing, HttpPostedFileBase file)
        {
            using (SharingManager sharingManager = new SharingManager())
            {

                int result = -1;
                if (ModelState.IsValid)
                {
                    if (sharing.Id > 0)
                    {
                        var currShare = sharingManager.ListQueryable().Where(a => a.Id == sharing.Id && !a.isDelete).Include(x => x.Category).FirstOrDefault();
                        if (currShare != null)
                        {

                            string oldPhoto = string.Empty;


                            var oldPhotoRes = sharingManager.Find(x => x.Id == sharing.Id).ImageUrl;
                           // oldPhoto = HttpContext.Server.MapPath("~" + oldPhotoRes);
                            string defaultShareImage = oldPhotoRes.Split('/')[3];
                            if (defaultShareImage!="sitelogo.png")
                            {
                                oldPhoto = HttpContext.Server.MapPath("~" + oldPhotoRes);
                            }
                            if (file != null)
                            {
                                if (System.IO.File.Exists(oldPhoto))
                                {
                                    System.IO.File.Delete(oldPhoto);
                                }

                                FileHelper fileHelper = new FileHelper();
                                var imgPath = Server.MapPath("~/Images/SharingImage");

                                currShare.ImageUrl = fileHelper.SaveImage(file, imgPath, "/Images/SharingImage");

                            }
                            currShare.isDraft = sharing.isDraft;
                            currShare.CategoryId = sharing.CategoryId;
                            currShare.ShareContent = sharing.ShareContent;
                            currShare.Title = sharing.Title;
                            sharingManager.Update(currShare);
                            result = 1;
                        }


                    }
                    else
                    {
                        if (file != null)
                        {
                            FileHelper fileHelper = new FileHelper();
                            var imgPath = Server.MapPath("~/Images/SharingImage");

                            sharing.ImageUrl = fileHelper.SaveImage(file, imgPath, "/Images/SharingImage");
                        }
                        else
                        {
                            sharing.ImageUrl = "/Images/SharingImage/sitelogo.png";
                        }
                        sharing.Owner = CurrentSession.shareBookUser;
                        sharingManager.Insert(sharing);

                        result = 2;
                    }

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return View(sharing);
                }



            }


        }
        
        [AuthFilter]
        public ActionResult DeleteShare(int id)
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                var v = sharingManager.ListQueryable().Where(a => a.Id == id).FirstOrDefault();
               
                 
                    v.isDelete = true;
                var oldPhoto = string.Empty;
                var oldPhotoRes = sharingManager.Find(x => x.Id == id).ImageUrl;
                string defaultShareImage = oldPhotoRes.Split('/')[3];
                if (defaultShareImage != "sitelogo.png")
                {
                    oldPhoto = HttpContext.Server.MapPath("~" + oldPhotoRes);
                }

               
                    if (System.IO.File.Exists(oldPhoto))
                    {
                        System.IO.File.Delete(oldPhoto);
                    }

                

                if (sharingManager.Update(v) > 0)
                    {
                        return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                    }

                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]

        public ActionResult GetLiked(int?[] ids)
        {
            using (LikedManager likedManager = new LikedManager())
            {
                List<int> likedSharingId = new List<int>();
                if (ids != null)
                {

                    if (CurrentSession.shareBookUser != null)
                    {
                        likedSharingId = likedManager.List(x => x.likedUser.Id == CurrentSession.shareBookUser.Id && ids.Contains(x.Shareing.Id)).Where(x => !x.likedUser.isDeleted).Select(x => x.Shareing.Id).ToList();

                        return Json(new { Result = likedSharingId });
                    }

                    return Json(new { Result = likedSharingId });
                }
                else
                {
                    return Json(new { Result = likedSharingId });
                }
            }
        }
        [HttpPost]
        public ActionResult GetCommentCount(int? noteid)
        {
            using (CommentManager commentManager = new CommentManager())
            {
                int count = commentManager.ListQueryable().Where(x => x.Id == noteid && !x.isDelete && !x.Owner.isDeleted).ToList().Count;

                return Json(new { result = count });
            }
        }

        [HttpPost]

        public ActionResult SetLikeState(int noteid, bool liked)
        {
            using (LikedManager likedManager = new LikedManager())
            {
                using (SharingManager sharingManager = new SharingManager())
                {
                    int result = 0;
                    Liked like = likedManager.Find(x => x.Shareing.Id == noteid && x.likedUser.Id == CurrentSession.shareBookUser.Id && !x.likedUser.isDeleted && !x.Shareing.isDelete);
                    Sharing share = sharingManager.Find(x => x.Id == noteid && !x.Owner.isDeleted && !x.isDelete && !x.Owner.isDeleted);


                    if (like != null && liked == false)
                    {
                        result = likedManager.Delete(like);
                    }
                    else if (like == null && liked == true)
                    {
                        result = likedManager.Insert(new Liked()
                        {
                            likedUser = CurrentSession.shareBookUser,
                            Shareing = share
                        });
                    }

                    if (result > 0)
                    {
                        if (liked)
                        {
                            share.LikeCount++;
                        }
                        else
                        {
                            share.LikeCount--;
                        }
                        sharingManager.Update(share);

                        return Json(new { hasError = false, errorMessage = string.Empty, result = share.Likes.Where(x => !x.likedUser.isDeleted).ToList().Count });

                    }


                    return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi", result = share.Likes.Where(x => !x.likedUser.isDeleted).ToList().Count });
                }
            }
        }


        public ActionResult ShowSharingDetail(int? id)
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                Sharing share = sharingManager.Find(x => x.Id == id && !x.isDelete);

                if (share == null)
                {
                    return HttpNotFound();
                }

                return PartialView("_PartialSharingDetail", share);
            }
        }
    }
}








