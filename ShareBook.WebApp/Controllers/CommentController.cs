using ShareBook.BusinessLayer;
using ShareBook.Entities;
using ShareBook.WebApp.Filters;
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
    public class CommentController : Controller
    {
      
        // GET: Comment
        public ActionResult ShowCommentsandLikes(int? id)
        {
            using (SharingManager sharingManager=new SharingManager())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Sharing share = sharingManager.ListQueryable().Include(x => x.Comments).Where(x => x.Id == id && !x.isDelete && !x.isDraft && !x.Owner.isDeleted).FirstOrDefault();

                if (share == null)
                {
                    return HttpNotFound();
                }
                var modelComments = share.Comments.Where(x => !x.isDelete && !x.Owner.isDeleted).ToList();
                var modelLikes = share.Likes.Where(x => !x.likedUser.isDeleted).ToList();

                return PartialView("_PartialComments", new LikeCommentViewModel<List<Liked>, List<Comment>>()
                {
                    Comments = modelComments,
                    Likes = modelLikes
                });
            }
            
        }


        [HttpPost]
        [AuthFilter]
        public ActionResult Edit(int? id,string text)
        {
            using (CommentManager commentManager=new CommentManager())
            {
                if (ModelState.IsValid)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    Comment comment = commentManager.Find(x => x.Id == id && !x.isDelete && !x.Owner.isDeleted);

                    if (comment == null)
                    {
                        return new HttpNotFoundResult();
                    }
                    else
                    {
                        if (comment.commentText != text)
                        {
                            comment.commentText = text;
                            if (commentManager.Update(comment) > 0)
                            {
                                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpGet]
        [AuthFilter]
        public ActionResult Delete(int? id)
        {
            using (CommentManager commentManager=new CommentManager())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Comment comment = commentManager.Find(x => x.Id == id && !x.isDelete && !x.Owner.isDeleted);

                if (comment == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.isDelete = true;

                if (commentManager.Update(comment) > 0)
                {
                    return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
           
        }


        [HttpPost]
        [AuthFilter]
        public ActionResult Insert(Comment comment,int? note_id)
        {
            using (SharingManager sharingManager = new SharingManager())
            {
                using (CommentManager commentManager = new CommentManager())
                {
                    if (ModelState.IsValid)
                    {
                        if (note_id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }

                        Sharing sharing = sharingManager.Find(x => x.Id == note_id && !x.isDelete && !x.isDraft && !x.Owner.isDeleted);

                        if (sharing == null)
                        {
                            return new HttpNotFoundResult();
                        }

                        if (comment != null)
                        {
                            comment.Sharing = sharing;
                            comment.Owner = CurrentSession.shareBookUser;

                            if (commentManager.Insert(comment) > 0)
                            {
                                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                            }
                        }



                        return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        
    }
}