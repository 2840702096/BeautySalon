using BeautySalon.Models.Context;
using BeautySalon.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BeautySalon.Models.Tools
{
    public class ImageHelper
    {
        public IWebHostEnvironment  _en;

        public ImageHelper(IWebHostEnvironment en)
        {
            _en = en;
        }

        public string AddImage(IFormFile ImageName, string imagePath, string thumbNailPath)
        {
            if (ImageName != null && ImageName.IsImage())
            {
                string NewImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(ImageName.FileName);

                string ImagePath = $"{_en.WebRootPath}{imagePath}{NewImageName}";

                using (var Stream = new FileStream(ImagePath, FileMode.Create))
                {
                    ImageName.CopyTo(Stream);
                }

                ImageConvertor convertor = new ImageConvertor();

                string ThumbPath = $"{_en.WebRootPath}{thumbNailPath}{NewImageName}";
                convertor.Image_resize(ImagePath, ThumbPath, 300);

                return NewImageName;
            }
            else
            {
                return "UnSuccessful";
            }
        }

        public string EditImage(IFormFile newImage, string currentImage, string imagePath, string thumbPath)
        {
            string CurrentImageName = "";
            if (newImage != null && newImage.IsImage())
            {
                if (currentImage != null && currentImage != "Default.png")
                {
                    string Path = $"{_en.WebRootPath}{imagePath}{currentImage}";
                    FileInfo file = new FileInfo(Path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    string Path1 = $"{_en.WebRootPath}{thumbPath}{currentImage}";
                    FileInfo file1 = new FileInfo(Path1);
                    if (file1.Exists)
                    {
                        file1.Delete();
                    }
                }
                CurrentImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(newImage.FileName);
                string ImagePath = $"{_en.WebRootPath}{imagePath}{CurrentImageName}";

                using (var Stream = new FileStream(ImagePath, FileMode.Create))
                {
                    newImage.CopyTo(Stream);
                }

                ImageConvertor convertor = new ImageConvertor();

                string ThumbPass = $"{_en.WebRootPath}{thumbPath}{CurrentImageName}";
                convertor.Image_resize(ImagePath, ThumbPass, 400);

                return CurrentImageName;
            }
            else
            {
                CurrentImageName = currentImage;
                return CurrentImageName;
            }
        }
    }
}
