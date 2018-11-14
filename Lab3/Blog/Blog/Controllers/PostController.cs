﻿using Blog.DAL;
using Blog.Models;
using Blog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: Post
        public ActionResult Index(int komentarzID = -1, string text = null)
        {
            //PUT
            if(Request.IsAjaxRequest())
            {
                var kom = db.Komentarz.Find(komentarzID);
                var postID = kom.PostID;
                kom.Body = text;
                db.SaveChanges();

                var komentarzePosta = db.Komentarz.Where(a => a.PostID == postID).ToList();

                return PartialView("_Comments", komentarzePosta);
            }

            var posty = db.Post.ToList();
            var vm = new List<PostViewModel>();

            posty.ForEach(a =>
            vm.Add(new PostViewModel() { Post = a,
                KomentarzePosta = db.Komentarz.Where(c => c.PostID == a.PostID).ToList(),
                Komentarz = new Comment() }));
            
            return View(vm);
        }

        [ChildActionOnly]
        public PartialViewResult Komentarz(int id)
        {
            ViewBag.ID = id;
            ViewBag.ID2 = Convert.ToString(id);
            Comment Komentarz = new Comment();
            return PartialView("_CommentForm", Komentarz);
        }

        [HttpPost]
        public ActionResult Index(Comment Komentarz)
        {
            Komentarz.DataWstawieniaKomentarza = DateTime.Now;
            db.Komentarz.Add(Komentarz);
            db.SaveChanges();

            var komentarzePosta = db.Komentarz.Where(a => a.PostID == Komentarz.PostID).ToList();
            
            return PartialView("_Comments", komentarzePosta);
        }

        public ActionResult Delete(int komentarzID)
        {
            var kom = db.Komentarz.Find(komentarzID);
            var postID = kom.PostID;
            db.Komentarz.Remove(kom);
            db.SaveChanges();

            var komentarzePosta = db.Komentarz.Where(a => a.PostID == postID).ToList();

            return PartialView("_Comments", komentarzePosta);
        }


        public ActionResult Update(int komentarzID, string text)
        {
            var kom = db.Komentarz.Find(komentarzID);
            var postID = kom.PostID;
            kom.Body = text;
            db.SaveChanges();

            var komentarzePosta = db.Komentarz.Where(a => a.PostID == postID).ToList();

            return PartialView("_Comments", komentarzePosta);
        }
    }
}