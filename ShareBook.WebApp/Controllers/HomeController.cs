using ShareBook.BusinessLayer;
using ShareBook.BusinessLayer.Results;
using ShareBook.Entities;
using ShareBook.Entities.EntityViewModels;
using ShareBook.Entities.Messages;
using ShareBook.Entities.ValueObjects;
using ShareBook.WebApp.Filters;
using ShareBook.WebApp.Helpers;
using ShareBook.WebApp.Models;
using ShareBook.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShareBook.WebApp.Controllers
{
    [ExceptionFilter]
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            using (SharebookUserManager sharebookUserManager=new SharebookUserManager())
            {
                if (ModelState.IsValid)
                {

                    BusinessLayerResult<ShareBookUser> result = sharebookUserManager.RegisterUser(model);

                    if (result.Errors.Count > 0)
                    {
                        result.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                        return View(model);
                    }



                    InfoViewModel infoViewModel = new InfoViewModel()
                    {
                        Tittle = "Kayıt Başarılı...",
                        RedirectingUrl = "/Home/Login",


                    };
                    infoViewModel.Items.Add(" E-posta adresinize gönderilen mail ile hesabınızı aktive ediniz. Aksi halde not yazamaz, yorum yapamaz ve notları beğenme işlemini gerçekleştiremezsiniz!");


                    return View("Info", infoViewModel);

                }
                return View(model);
            }
            
        }

        public ActionResult ActivateUser(Guid id)
        {

            using (SharebookUserManager sharebookUserManager=new SharebookUserManager())
            {
                BusinessLayerResult<ShareBookUser> res = sharebookUserManager.ActivateUser(id);

                if (res.Result != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        ErrorViewModel errorNotifyObj = new ErrorViewModel()
                        {
                            Tittle = "Geçersiz İşlem",
                            Items = res.Errors

                        };
                        return View("Error", errorNotifyObj);
                        //TempData["errors"] = res.Errors;
                        //return RedirectToAction("ActivateUserCancel", "Home");
                    }
                    OkViewModel okNotifyObj = new OkViewModel()
                    {
                        Tittle = "Hesabınızın aktivasyon işlemi tamamlanmıştır...",
                        RedirectingUrl = "/Home/Login"

                    };
                    okNotifyObj.Items.Add("Paylaşım yapmak için ne bekliyorsunuz!");
                    return View("Ok", okNotifyObj);
                }
                else
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Tittle = "Geçersiz İşlem",


                    };
                    errorNotifyObj.Items.Add(new ErrorMessageObj()
                    {
                        Code = ErrorMessagesCode.MailHasTimedOut,
                        Message = "Mail zaman aşımına uğramış."
                    });
                    return View("Error", errorNotifyObj);
                }
            }
        }
        // GET: Home
        public ActionResult Index()
        {

            using (SharingManager sharingManager = new SharingManager())
            {
                List<SharingViewModel> model = sharingManager.ListQueryable().Where(x => !x.isDelete && !x.isDraft && !x.Owner.isDeleted).Select(x => new SharingViewModel()
                {
                    Category = x.Category,
                    Comments = x.Comments,
                    Likes = x.Likes,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    ShareContent = x.ShareContent,
                    Owner = x.Owner,
                    ModifiedOn = x.ModifiedOn,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    isDraft = x.isDraft,
                    isDelete = x.isDelete,
                    LikeCount = x.LikeCount,





                }).OrderByDescending(x => x.ModifiedOn).ToList();
                return View(model);
            }
            
        }
        [AuthFilter]
        public ActionResult MyLikedNotes()
        {
            using (LikedManager likedManager = new LikedManager())
            {
                var myLikedNotes = likedManager.ListQueryable().Include(x => x.likedUser).Include(x => x.Shareing).Where(x => x.likedUser.Id == CurrentSession.shareBookUser.Id).Select(x => x.Shareing).Include(x => x.Category).Include(x => x.Owner).Select(x => new SharingViewModel()
                {
                    Category = x.Category,
                    Comments = x.Comments,
                    Likes = x.Likes,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    ShareContent = x.ShareContent,
                    Owner = x.Owner,
                    ModifiedOn = x.ModifiedOn,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    isDraft = x.isDraft,
                    isDelete = x.isDelete,
                    LikeCount = x.LikeCount,



                }).OrderByDescending(x => x.ModifiedOn).ToList();

                return View("Index", myLikedNotes);
            }
        }

        [AuthFilter]
        public ActionResult MyCommentNotes()
        {
            using (CommentManager commentManager = new CommentManager())
            {
                List<SharingViewModel> myCommentNotes = commentManager.ListQueryable().Include(x => x.Owner).Include(x => x.Sharing).Where(x => x.Owner.Id == CurrentSession.shareBookUser.Id && !x.isDelete).Select(x => x.Sharing).Distinct().Include(x => x.Category).Include(x => x.Owner).Select(x => new SharingViewModel()
                {
                    Category = x.Category,
                    Comments = x.Comments,
                    Likes = x.Likes,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    ShareContent = x.ShareContent,
                    Owner = x.Owner,
                    ModifiedOn = x.ModifiedOn,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    isDraft = x.isDraft,
                    isDelete = x.isDelete,
                    LikeCount = x.LikeCount,




                }).OrderByDescending(x => x.ModifiedOn).ToList();

                return View("Index", myCommentNotes);
            }
        }
        public ActionResult SelectCategory(int? id)
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                List<SharingViewModel> sharing = sharingManager.ListQueryable().Where(x => x.Category.Id == id && !x.isDelete && !x.isDraft && !x.Owner.isDeleted).Select(x => new SharingViewModel()
                {
                    Category = x.Category,
                    Comments = x.Comments,
                    Likes = x.Likes,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    ShareContent = x.ShareContent,
                    Owner = x.Owner,
                    ModifiedOn = x.ModifiedOn,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    isDraft = x.isDraft,
                    isDelete = x.isDelete,
                    LikeCount = x.LikeCount,



                }).OrderByDescending(x => x.ModifiedOn).ToList();

                return View("Index", sharing);
            }
        }


        public ActionResult Search(string search)
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (search.Length >= 3)
                        {
                            List<SharingViewModel> share = sharingManager.ListQueryable().Where(x => (x.ShareContent.Contains(search) || x.Title.Contains(search)) && !x.isDelete && !x.isDraft && !x.Owner.isDeleted || x.Owner.Username.Contains(search) || x.Owner.Name.Contains(search) || x.Owner.Surname.Contains(search)).Select(x => new SharingViewModel()
                            {
                                Title = x.Title,
                                ShareContent = x.ShareContent,
                                Owner = x.Owner,
                                Category = x.Category,
                                Comments = x.Comments,
                                ModifiedOn = x.ModifiedOn,
                                CreatedOn = x.CreatedOn,
                                ImageUrl = x.ImageUrl,
                                ModifiedBy = x.ModifiedBy,
                                Likes = x.Likes,
                                CategoryId = x.CategoryId,
                                Id = x.Id


                            }).ToList();

                            List<ShareBookUserViewModel> user = sharebookUserManager.ListQueryable().Where(x => (x.Username.Contains(search) || x.Name.Contains(search) || x.Surname.Contains(search)) && !x.isDeleted).Select(x => new ShareBookUserViewModel()
                            {

                                Comments = x.Comments,
                                activatedGuid = x.activatedGuid,
                                Email = x.Email,
                                Name = x.Name,
                                Username = x.Username,
                                Notes = x.Notes,
                                Password = x.Password,
                                ProfilPhoto = x.ProfilPhoto,
                                Surname = x.Surname,
                                Id = x.Id,
                                isActive = x.isActive,
                                CreatedOn = x.CreatedOn,
                                isAdmin = x.isAdmin,
                                ModifiedOn = x.ModifiedOn,
                                ModifiedBy = x.ModifiedBy,
                                isDeleted = x.isDeleted,
                                Likes = x.Likes


                            }).ToList();



                            return View(new SharingUserViewModel<List<ShareBookUserViewModel>, List<SharingViewModel>>()
                            {
                                s = share,
                                sbu = user

                            });
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
        }


        public ActionResult MostLiked()
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                List<SharingViewModel> model = sharingManager.ListQueryable().Where(x => !x.isDelete && !x.isDraft && !x.Owner.isDeleted).Select(x => new SharingViewModel()
                {
                    Category = x.Category,
                    Comments = x.Comments,
                    Likes = x.Likes,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    ShareContent = x.ShareContent,
                    Owner = x.Owner,
                    ModifiedOn = x.ModifiedOn,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    isDraft = x.isDraft,
                    isDelete = x.isDelete,
                    LikeCount = x.LikeCount,





                }).OrderByDescending(x => x.LikeCount).ToList();
                return View("Index", model);
            }
        }

        public ActionResult LastShares()
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                List<SharingViewModel> model = sharingManager.ListQueryable().Where(x => !x.isDelete && !x.isDraft && !x.Owner.isDeleted).Select(x => new SharingViewModel()
                {
                    Category = x.Category,
                    Comments = x.Comments,
                    Likes = x.Likes,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    ShareContent = x.ShareContent,
                    Owner = x.Owner,
                    ModifiedOn = x.ModifiedOn,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    isDraft = x.isDraft,
                    isDelete = x.isDelete,
                    LikeCount = x.LikeCount,





                }).OrderByDescending(x => x.ModifiedOn).ToList();
                return View("Index", model);
            }
        }
        public ActionResult About()
        {

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {
                if (ModelState.IsValid)
                {

                    BusinessLayerResult<ShareBookUser> res = sharebookUserManager.LoginUser(model);

                    if (res.Errors.Count > 0)
                    {
                        res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                        return View(model);
                    }
                    //Session["loginOk"] = res.Result;
                    CurrentSession.Set<ShareBookUser>("loginOk", res.Result);
                    return RedirectToAction("Index", "Home");


                }
                return View(model);
            }
        }
        public ActionResult Logout()
        {
            CurrentSession.RemoveSession("loginOk");
            return RedirectToAction("Index", "Home");
        }


        [AuthFilter]
        public ActionResult ShowProfile()
        {

            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {
                BusinessLayerResult<ShareBookUser> res = sharebookUserManager.getUserById(CurrentSession.shareBookUser.Id);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Tittle = "Bir Hata Oluştu...",
                        Items = res.Errors

                    };
                    return View("Error", errorNotifyObj);
                }
                return View(res.Result);
            }
        }


        [HttpPost]
        [AuthFilter]
        public ActionResult EditProfile(ShareBookUser model, HttpPostedFileBase file)
        {
            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {
                if (file == null)
                {
                    ModelState.Remove("ProfilPhoto");
                }
                ModelState.Remove("Email");
                ModelState.Remove("Password");
                if (ModelState.IsValid)
                {
                    string oldPhoto = string.Empty;
                    if (file != null)
                    {
                        FileHelper fileHelper = new FileHelper();
                        var imgPath = Server.MapPath("~/Images/UserProfilePhoto");

                        model.ProfilPhoto = fileHelper.SaveImage(file, imgPath, "/Images/UserProfilePhoto");
                    }

                    BusinessLayerResult<ShareBookUser> oldPhotoRes = sharebookUserManager.getUserById(model.Id);
                    string defaultImage = oldPhotoRes.Result.ProfilPhoto.Split('/')[3];

                    if (defaultImage != "default1.png" && defaultImage != "default2.png" && defaultImage != "default3.png" && defaultImage != "default4.png" && defaultImage != "default5.png" && defaultImage != "default6.png")
                    {
                        oldPhoto = HttpContext.Server.MapPath("~" + oldPhotoRes.Result.ProfilPhoto);
                    }



                    BusinessLayerResult<ShareBookUser> res = sharebookUserManager.UpdateProfile(model);


                    if (res.Errors.Count > 0)
                    {
                        ErrorViewModel errorObj = new ErrorViewModel()
                        {
                            Items = res.Errors,
                            Tittle = "Profil Güncellenemedi...",
                            RedirectingUrl = "/Home/ShowProfile",

                        };
                        return View("Error", errorObj);
                    }

                    if (file != null)
                    {
                        if (System.IO.File.Exists(oldPhoto))
                        {
                            System.IO.File.Delete(oldPhoto);
                        }

                    }

                    CurrentSession.Set<ShareBookUser>("loginOk", res.Result);

                    return RedirectToAction("ShowProfile", "Home");

                }
                else
                {
                    WarningViewModel warningNotifyObj = new WarningViewModel()
                    {
                        RedirectingUrl = "/Home/ShowProfile",

                    };
                    warningNotifyObj.Items.Add("Boş geçilmemesi gereken bir alanı boş geçtiniz...");

                    return View("Warning", warningNotifyObj);
                }
            }

        }

        [AuthFilter]

        public ActionResult DeleteUser()
        {

            return View("_PartialDeleteUserPage",CurrentSession.shareBookUser);
        }


        [AuthFilter]
        [HttpPost]
        public ActionResult DeleteUser(ShareBookUser usr)
        {

            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {
                ShareBookUser user = sharebookUserManager.Find(x => x.Password == usr.Password && x.Id == usr.Id/* CurrentSession.shareBookUser.Id*/);
                if (user != null)
                {
                    BusinessLayerResult<ShareBookUser> res = sharebookUserManager.RemoveByUserID(usr.Id);

                    if (res.Errors.Count > 0)
                    {
                        ErrorViewModel errorViewModel = new ErrorViewModel()
                        {
                            Items = res.Errors,
                            Tittle = "Hesap Silme İşlemi",
                            RedirectingUrl = "/Home/ShowProfile"

                        };
                        return View("Error", errorViewModel);
                    }
                    else
                    {


                        InfoViewModel infoViewModel = new InfoViewModel()
                        {
                            Tittle = "Hesap Silme Talebi",
                            RedirectingUrl = "/Home/Index",
                        };
                        infoViewModel.Items.Add("Hesabınızı kalıcı olarak silme işleminiz ile ilgili mail gönderilmiştir. Mail aracılığıyla hesabınızı kalıcı olarak silebilirsiniz...");
                        return View("Info", infoViewModel);
                    }
                }
                else
                {
                    ErrorViewModel errorViewModel = new ErrorViewModel()
                    {
                        Tittle = "İşlem Başarısız",
                        RedirectingUrl = "/Home/ShowProfile"

                    };
                    errorViewModel.Items.Add(new ErrorMessageObj()
                    {
                        Code = ErrorMessagesCode.PasswordisNotTrue,
                        Message = "Şifreyi yanlış girdiniz..."
                    });
                    return View("Error", errorViewModel);
                }
            }
        }



        
        public ActionResult DeleteUserAccount(Guid id)
        {
            using (SharebookUserManager sharebookUserManager= new SharebookUserManager())
            {
                ShareBookUser user = sharebookUserManager.Find(x => x.activatedGuid == id);
                if (user != null)
                {
                    BusinessLayerResult<ShareBookUser> res = sharebookUserManager.RemoveUser(user);
                    if (res.Result != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            ErrorViewModel errorViewModel = new ErrorViewModel()
                            {
                                Items = res.Errors,
                                Tittle = "Hesap Silme İşlemi",
                                RedirectingUrl = "/Home/ShowProfile"

                            };
                            return View("Error", errorViewModel);
                        }

                        CurrentSession.RemoveSession("loginOk");

                        OkViewModel okViewModel = new OkViewModel()
                        {
                            Tittle = "Kalıcı Hesap Silme  Talebi",

                            RedirectingUrl = "/Home/Index"

                        };
                        okViewModel.Items.Add("Hesabınız kalıcı olarak silinmiştir. Bir gün tekrar görüşmek dileğiyle...");
                        return View("Ok", okViewModel);
                    }
                    else
                    {
                        ErrorViewModel errorNotifyObj = new ErrorViewModel()
                        {
                            Tittle = "Geçersiz İşlem",


                        };
                        errorNotifyObj.Items.Add(new ErrorMessageObj()
                        {
                            Code = ErrorMessagesCode.UserNotFound,
                            Message = "Kullanıcı bulunamadı"
                        });
                        return View("Error", errorNotifyObj);
                    }
                }
                else
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Tittle = "Geçersiz İşlem",


                    };
                    errorNotifyObj.Items.Add(new ErrorMessageObj()
                    {
                        Code = ErrorMessagesCode.MailHasTimedOut,
                        Message = "Mail zaman aşımına uğramış."
                    });
                    return View("Error", errorNotifyObj);
                }
            }
        }


        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ShareBookUser model)
        {
            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {
                ShareBookUser user = sharebookUserManager.Find(x => x.Email == model.Email && !x.isDeleted);
                BusinessLayerResult<ShareBookUser> result = sharebookUserManager.iForgotPassword(user);
                if (result.Result != null)
                {
                    if (result.Errors.Count > 0)
                    {
                        ErrorViewModel errorNotifyObj = new ErrorViewModel()
                        {
                            Tittle = "Geçersiz İşlem",
                            Items = result.Errors

                        };
                        return View("Error", errorNotifyObj);

                    }
                    InfoViewModel infoViewModel = new InfoViewModel()
                    {
                        Tittle = "Şifre Değişikliği Talebi",

                        RedirectingUrl = "/Home/Index"

                    };
                    infoViewModel.Items.Add("Hesabınıza şifre değişikliği talebiniz ile ilgili mail gönderilmiştir mail aracılığıyla şifrenizi değiştirebilirsiniz...");
                    return View("Info", infoViewModel);
                }
                else
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Tittle = "Geçersiz İşlem",


                    };
                    errorNotifyObj.Items.Add(new ErrorMessageObj()
                    {
                        Code = ErrorMessagesCode.UserNotFound,
                        Message = "Kullanıcı bulunamadı"
                    });
                    return View("Error", errorNotifyObj);
                }
            }

        }

        public ActionResult ForgotPasswordActivateUser(Guid id)
        {

            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {
                BusinessLayerResult<ShareBookUser> res = sharebookUserManager.forgotActivateUser(id);
                if (res.Result != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        ErrorViewModel errorNotifyObj = new ErrorViewModel()
                        {
                            Tittle = "Geçersiz İşlem",
                            Items = res.Errors

                        };
                        return View("Error", errorNotifyObj);

                    }


                    return View(new ForgotPassUserViewModel<ShareBookUser, ForgotPasswordViewModel>()
                    {
                        User = res.Result
                    });
                }
                else
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Tittle = "Geçersiz İşlem",


                    };
                    errorNotifyObj.Items.Add(new ErrorMessageObj()
                    {
                        Code = ErrorMessagesCode.MailHasTimedOut,
                        Message = "Mail zaman aşımına uğramış."
                    });
                    return View("Error", errorNotifyObj);
                }
            }

        }
        [HttpPost]
        public ActionResult ForgotPasswordActivateUser(ForgotPassUserViewModel<ShareBookUser, ForgotPasswordViewModel> model)
        {
            using (SharebookUserManager sharebookUserManager = new SharebookUserManager())
            {

                ModelState.Remove("User.Password");
                ModelState.Remove("User.Username");
                ModelState.Remove("User.Email");



                if (ModelState.IsValid)
                {
                    ShareBookUser user = sharebookUserManager.Find(x => x.Id == model.User.Id);
                    if (user != null)
                    {

                        user.Password = model.NewPass.Password;

                        if (sharebookUserManager.Update(user) == 0)
                        {
                            ErrorViewModel errorViewModel = new ErrorViewModel()
                            {
                                Tittle = "Şifre değiştirme başarısız"
                            };

                            return View("Error", errorViewModel);
                        }
                        CurrentSession.Set<ShareBookUser>("loginOk", user);
                        if (CurrentSession.shareBookUser != null)
                        {
                            OkViewModel okViewModel2 = new OkViewModel()
                            {
                                Tittle = "Şifre değiştirme işlemi",

                                RedirectingUrl = "/Home/Index"
                            };
                            okViewModel2.Items.Add("Başarılı");

                            return View("Ok", okViewModel2);
                        }
                        OkViewModel okViewModel = new OkViewModel()
                        {
                            Tittle = "Şifre değiştirme işlemi",

                            RedirectingUrl = "/Home/Login"
                        };
                        okViewModel.Items.Add("Başarılı");
                        CurrentSession.Set("loginOk", user);

                        return View("Ok", okViewModel);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return View(model);
                }
            }

        }


       

    }





}
