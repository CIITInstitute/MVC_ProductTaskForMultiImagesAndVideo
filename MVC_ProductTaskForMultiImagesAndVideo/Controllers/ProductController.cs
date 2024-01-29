using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_ProductTaskForMultiImagesAndVideo.Models;
using System.IO;
namespace MVC_ProductTaskForMultiImagesAndVideo.Controllers
{
    public class ProductController : Controller
    {
        MVC_EveningDBEntities db;
        public ProductController()
        {
            db=new MVC_EveningDBEntities();
        }
        public ActionResult Index()
        {
            return View(db.tblproducts.ToList());
        }
        public ActionResult AddProduct()
        {
            tblproduct p = new tblproduct();
            return View(p);
        }
        [HttpPost]
        public ActionResult AddProduct(tblproduct p, HttpPostedFileBase[] photos,HttpPostedFileBase video)
        {
            string folderpath = Server.MapPath("~/Products/" + p.productName);
            if(!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            string vidname=p.productName+Path.GetExtension(video.FileName);
            string vidpath = folderpath + "/" + vidname;
            video.SaveAs(vidpath);

            string data = "";
            int i = 1;
            foreach(HttpPostedFileBase h in photos)
            {
                string imgname=i+Path.GetExtension (h.FileName);
                string imgpath = folderpath + "/" + imgname;
                h.SaveAs(imgpath);
                data = data + "," + imgname;
                i++;
            }
            data=data.Substring(1,data.Length - 1);
            p.productVideo = vidname;
            p.productPhotos = data;
            db.tblproducts.Add(p);
            db.SaveChanges();
            ViewBag.msg = "Product Added Successfully";
            ModelState.Clear();


            return View();
        }
    }
}