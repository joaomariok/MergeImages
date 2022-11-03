using System.Drawing;
using System.Runtime.InteropServices;

namespace MergeImages
{
    internal class Program
    {
        private static Bitmap MergeImages(Image image1, Image image2)
        {
            Bitmap bitmap = new(image1.Width + image2.Width, Math.Max(image1.Height, image2.Height));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(image1, 0, 0, image1.Width, image1.Height);
                g.DrawImage(image2, image1.Width, 0, image2.Width, image2.Height);
            }

            return bitmap;
        }

        static void Main(string[] args)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Error: Only supported in Windows");
                return;
            }

            if (args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("Error: Wrong number of arguments");
                return;
            }

            string filePath1 = args[0];
            string filePath2 = args[1];

            Console.WriteLine(string.Format("File 1: {0}", filePath1));
            Console.WriteLine(string.Format("File 2: {0}", filePath2));

            if (!File.Exists(filePath1) || !File.Exists(filePath2))
            {
                Console.WriteLine("Error: File does not exist.");
                return;
            }

            string folderPath1 = Path.GetDirectoryName(filePath1);
            string defaultMergedFileName = Path.GetFileNameWithoutExtension(filePath1) + "_" + Path.GetFileNameWithoutExtension(filePath2);
            string fileExtension1 = Path.GetExtension(filePath1);
            string defaultMergedFilePath = folderPath1 + "/" + defaultMergedFileName + fileExtension1;

            using (Image image1 = Image.FromFile(filePath1))
            using (Image image2 = Image.FromFile(filePath2))
            using (Image mergedImage = MergeImages(image1, image2))
            {
                string mergedFilePath = args.Length == 3 ? args[2] : defaultMergedFilePath;
                mergedImage.Save(mergedFilePath);
                Console.WriteLine(string.Format("Merged image saved in path: {0}.", mergedFilePath));
                Console.ReadLine();
            }

        }
    }
}