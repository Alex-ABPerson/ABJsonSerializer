using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ABJson.GDISupport
{
    public static class ImageToText
    {

        public static string ConvertImageToText(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    // Convert Image to byte[]
            //    image.Save(ms, format);
            //    byte[] imageBytes = ms.ToArray();

            //    // Convert byte[] to Base64 String
            //    string base64String = Convert.ToBase64String(imageBytes);
            //    return base64String;
            //}

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                byte[] bitmapBytes = memoryStream.GetBuffer();
                return Convert.ToBase64String(bitmapBytes);
            }
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Bitmap ConvertTextToImage(string base64String)
        {
            Console.WriteLine("YOY-ISH!");
            //// Convert Base64 String to byte[]
            //byte[] imageBytes = Convert.FromBase64String(base64String);
            //MemoryStream ms = new MemoryStream(imageBytes, 0,
            //  imageBytes.Length);

            //// Convert byte[] to Image
            //ms.Write(imageBytes, 0, imageBytes.Length);
            //Image image = Image.FromStream(ms, true);
            //return image;

            byte[] bitmapBytes = Convert.FromBase64String(base64String);
            using (MemoryStream memoryStream = new MemoryStream(bitmapBytes))
                return (Bitmap)Image.FromStream(memoryStream);
        }
    }
}
