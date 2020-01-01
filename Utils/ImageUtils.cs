using sd = System.Drawing;
using si = System.IO;
namespace Base2D.Utils
{
    public static class ImageUtils
    {
        /// <summary>
        /// Note that this method is not take care for any IO problem thus need to checking before using
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static sd.Size GetImageSize(string path)
        {
            //sd.Image img = sd.Image.FromFile(path); 
            //return new sd.Size(img.Width, img.Height);
            return new sd.Size(0, 0);
        }
    }
}
