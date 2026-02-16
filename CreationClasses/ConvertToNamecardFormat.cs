using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace GenshinNamecardINICreator.CreationClasses
{
    public sealed class ConvertToNamecardFormat
    {
        private string _destinationFolder = "";

        public ConvertToNamecardFormat(string destinationFolder)
        {
            _destinationFolder = destinationFolder;
        }

        public async Task<bool> BeginConversion(string path)
        {
            bool result = false;
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                string exportFolder = Path.Combine(_destinationFolder, Path.GetFileNameWithoutExtension(fi.Name));
                if (!Directory.Exists(exportFolder))
                {
                    Directory.CreateDirectory(exportFolder);
                }
                else
                {
                    //Directory.Delete(exportFolder, true);
                    //Directory.CreateDirectory(exportFolder);
                }
                using (var img = System.Drawing.Image.FromFile(fi.FullName))
                {
                    var dimension = new FrameDimension(img.FrameDimensionsList.First());
                    string fileNameNoSpaces = Path.GetFileNameWithoutExtension(fi.Name).Replace(" ", "_");
                    if (fi.Extension.Equals(".gif"))
                    {
                        var frameCount = img.GetFrameCount(dimension);
                        foreach (var frame in Enumerable.Range(0, frameCount))
                        {
                            await Convert(img, dimension, exportFolder, fileNameNoSpaces, frame);
                        }
                    }
                    else
                    {
                        await Convert(img, dimension, exportFolder, fileNameNoSpaces);
                    }
                }
            }
            return result;
        }

        private async Task Convert(System.Drawing.Image img, FrameDimension dimension, string exportFolder, string fileNameNoSpaces, int frame = 0)
        {

            img.SelectActiveFrame(dimension, frame);
            var outputFile = Path.Combine(exportFolder, String.Format("{0}{1}.png", fileNameNoSpaces, frame + 1));
            var resized = await ResizeImageA(img, 840, 400);
            resized.RotateFlip(RotateFlipType.Rotate180FlipNone);
            resized.Save(outputFile, ImageFormat.Png);
            resized.Dispose();
            if (frame == 0)
            {
                var resizedD = await ResizeImageD(img, 256, 256);
                outputFile = Path.Combine(exportFolder, String.Format("{0}D.png", fileNameNoSpaces));
                resizedD.RotateFlip(RotateFlipType.Rotate180FlipNone);
                resizedD.Save(outputFile, ImageFormat.Png);
                resizedD.Dispose();

                var resizedE = await ResizeImageE(img, 1024, 140);
                outputFile = Path.Combine(exportFolder, String.Format("{0}E.png", fileNameNoSpaces));
                resizedE.RotateFlip(RotateFlipType.Rotate180FlipNone);
                resizedE.Save(outputFile, ImageFormat.Png);
                resizedE.Dispose();
            }
        }

        private async Task<Bitmap> ResizeImageA(System.Drawing.Image image, int width, int height)
        {
            var destImage = new Bitmap(width, height);
            await Task.Run(() =>
            {
                var destRect = new Rectangle(0, 0, width, height);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }
            });
            return destImage;
        }

        private async Task<Bitmap> ResizeImageD(System.Drawing.Image image, int width, int height)
        {
            var destImage = new Bitmap(width, height);
            await Task.Run(() =>
            {
                var destRect = new Rectangle(0, 0, width, height);
                var whiteRectRect = new Rectangle(16, 55, 224, 145);
                var imageDRect = new Rectangle(18, 58, 219, 139);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.FillRoundedRectangle(new SolidBrush(Color.White), whiteRectRect, new Size(8, 8));
                        graphics.DrawImage(image, imageDRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                    // At the very end make it transparent.
                    destImage.MakeTransparent();
                    graphics.DrawImage(destImage, destImage.Width, 0, destImage.Width, destImage.Height);
                }
            });
            return destImage;
        }

        private async Task<Bitmap> ResizeImageE(System.Drawing.Image image, int width, int height)
        {
            var destImage = new Bitmap(width, height);
            await Task.Run(() =>
            {
                var destRect = new Rectangle(0, 0, width, height);
                //image.RotateFlip(RotateFlipType.Rotate180FlipNone);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        var path = new GraphicsPath();
                        var points = new Point[]
                        {
                        new Point(765, 138),
                        new Point(65, 138),
                        new Point(31, 126),
                        new Point(21, 118),
                        new Point(3, 86),
                        new Point(2, 57),
                        new Point(5, 45),
                        new Point(15, 28),
                        new Point(21, 21),
                        new Point(29, 13),
                        new Point(53, 3),
                        new Point(68, 1),
                        new Point(765, 1),
                        };
                        path.AddLines(points);
                        path.CloseAllFigures();
                        graphics.Clip = new Region(path);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                    // At the very end make it transparent.
                    //destImage.MakeTransparent();
                    //graphics.DrawImage(destImage, destImage.Width, 0, destImage.Width, destImage.Height);
                }
            });
            return destImage;
        }
    }
}
