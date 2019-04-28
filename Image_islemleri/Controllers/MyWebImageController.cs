using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Image_islemleri.Controllers
{
    public class MyWebImageController : Controller
    {

        public string ImagePath
        {
            get { return Server.MapPath("~/images/snow.jpeg"); }
        }

        public ActionResult start(string cmd = "Original")
        {
            ViewBag.cmd = cmd;
            return View();
        }

        public void Process(string p)
        {
            WebImage image = new WebImage(this.ImagePath);
            image.Resize(width: 300, height: 300); //bu kodu ben ekledik resim çok büyük gelirse tasma oluyor o yuzden ne gelirse gelsin önce 300,300 küçültüyor image'i tabi çözünürlüğü bozmadan niye kare olmadı dersen bozmamak için otomatik ayarlıyor.
            switch (p)
            {
                case "Original":
                    break;
                case "RotateLeft":
                    image.RotateLeft();
                    break;
                case "RotateRight":
                    image.RotateRight();
                    break;
                case "FlipHorizontal":
                    image.FlipHorizontal();
                    break;
                case "FlipVertical":
                    image.FlipVertical();
                    break;
                case "Resize":
                    image.Resize(image.Width / 2, image.Height / 2, preserveAspectRatio: true); //preserveAspectRatio çözünürlük bozulmadan anlamında
                    break;
                case "AddTextWatermark":
                    image.AddTextWatermark("alican yilmaz", fontColor: "Red", fontSize: 14, horizontalAlign: "Center", verticalAlign: "Bottom"); //FontFamily,fontStyle,opacity,padding gibi daha bircok özellik ekleyebilirsiniz.
                    break;
                case "AddImageWatermark":
                    WebImage watermark = new WebImage(this.ImagePath);
                    watermark.Resize(50, 50);
                    watermark = watermark.Save(Server.MapPath("~/images/watermark.jpeg"), imageFormat: "jpeg");
                    image.AddImageWatermark(watermark.FileName, 50, 50, verticalAlign: "Top", horizontalAlign: "Right", opacity: 75);
                    break;
                case "Crop":
                    image.Crop(50, 50, 100, 100);
                    break;
                default:
                    break;

            }

            string savaPath = Server.MapPath("~/images/last.jpeg"); //Bu kod ile resimin switchden hangi işlem çıktıysa o hali ile  kaydedileceği Path i belirliyoruz.
            WebImage savedImage = image.Save(savaPath, imageFormat: "jpeg"); // İlgili Path e sayfaya göndermeden resimi kayıt ediyoruz.
            string Filename=savedImage.FileName; // Burada sadece savedImage.FileName; yani "FileName" ile kaydedilen path in adını alabildiğimizi belirtmek istedik.Cunku Path i okuyarak veri tabanına kayıt edebilirsiniz işlenmiş halini.


            image.Write(); //image işlem sonucunu kullanıcıya döndürür
        }
    }
}

// Crop işleminde biz direk sağdan soldan belirli ölçülerde kestiriyoruz ama sen javascript ile kullanıcıya crop işlemini ayarlatıp kordinatları yakalayıp buraya dönüp ona göre kesme işlemi yaptırabilirsin.
// Save() metodu bize WebImage nesnesi döner onu o yüzden o türden bir değişken ile yakalamalısın!

// Proje içerisinde belirtilen "savaPath" e resim üzerinde switch case deki işlemlerden hangisini uyguladıysanız View e göndermeden "savaPath" e resimin o halini kaydeder her defasında.