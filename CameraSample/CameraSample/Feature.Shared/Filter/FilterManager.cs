using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Feature.Filter
{
    public static class FilterManager
    {
        /// <summary>
        /// GetImage by Filter
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async static Task<WriteableBitmap> GetImageByFilter(this IRandomAccessStream stream, IFilter filter)
        {
            stream.Seek(0);
            var imageFormat = ImageFormat.Jpeg;
            var imageType = await getImageType(stream);
            if (imageType == ImageType.PNG)
                imageFormat = ImageFormat.Png;
            using (var source = new RandomAccessStreamImageSource(stream, imageFormat))
            {
                // Create effect collection with the source stream
                using (var filters = new FilterEffect(source))
                {
                    // Initialize the filter and add the filter to the FilterEffect collection
                    filters.Filters = new IFilter[] { filter };

                    // Create a target where the filtered image will be rendered to
                    var target = new WriteableBitmap((int)(Window.Current.Bounds.Width), (int)(Window.Current.Bounds.Height));

                    // Create a new renderer which outputs WriteableBitmaps
                    using (var renderer = new WriteableBitmapRenderer(filters, target))
                    {
                        // Render the image with the filter(s)
                        target = await renderer.RenderAsync();
                        target.Invalidate();
                        return target;
                    }
                }
            }
        }

        /// <summary>
        /// Convert WriteableBitmap to byte
        /// </summary>
        /// <param name="writeableBitmap"></param>
        /// <returns></returns>
        public async static Task<byte[]> GetPhotoBytesAsync(this WriteableBitmap writeableBitmap)
        {
            if (writeableBitmap == null)
                return new byte[0] { };
            Stream stream = writeableBitmap.PixelBuffer.AsStream();
            byte[] pixels = new byte[stream.Length];
            await stream.ReadAsync(pixels, 0, pixels.Length);
            ConvertToRGBA(writeableBitmap.PixelHeight, writeableBitmap.PixelWidth, pixels);
            InMemoryRandomAccessStream ims = new InMemoryRandomAccessStream();
            var imsWriter = ims.AsStreamForWrite();
            await Task.Factory.StartNew(() => stream.CopyTo(imsWriter));
            stream.Flush();
            stream.Dispose();
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ims);
            encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)writeableBitmap.PixelWidth, (uint)writeableBitmap.PixelHeight, 96, 96, pixels);
            await encoder.FlushAsync();
            stream = ims.AsStreamForRead();
            byte[] pixeBuffer = new byte[ims.Size];
            await stream.ReadAsync(pixeBuffer, 0, pixeBuffer.Length);
            stream.Flush();
            stream.Dispose();
            return pixeBuffer;
        }

        /// <summary>
        /// Convert to RGBA
        /// </summary>
        /// <param name="pixelHeight"></param>
        /// <param name="pixelWidth"></param>
        /// <param name="pixels"></param>
        private static void ConvertToRGBA(int pixelHeight, int pixelWidth, byte[] pixels)
        {
            if (pixels == null)
                return;
            int offset;
            for (int row = 0; row < (uint)pixelHeight; row++)
            {
                for (int col = 0; col < (uint)pixelWidth; col++)
                {
                    offset = (row * (int)pixelWidth * 4) + (col * 4);
                    byte B = pixels[offset];
                    byte G = pixels[offset + 1];
                    byte R = pixels[offset + 2];
                    byte A = pixels[offset + 3];

                    // convert to RGBA format for BitmapEncoder
                    pixels[offset] = R; // Red
                    pixels[offset + 1] = G; // Green
                    pixels[offset + 2] = B; // Blue
                    pixels[offset + 3] = A; // Alpha
                }
            }
        }

        private async static Task<ImageType> getImageType(IRandomAccessStream stream)
        {
            ImageType imagetype = ImageType.VALIDFILE;
            var reader = new DataReader(stream.GetInputStreamAt(0));
            var bytes = new byte[stream.Size];
            await reader.LoadAsync((uint)stream.Size);
            reader.ReadBytes(bytes);
            try
            {
                string fileType = "";
                fileType += bytes[0].ToString();
                fileType += bytes[1].ToString();
                imagetype = (ImageType)Enum.Parse(typeof(ImageType), fileType, true);
            }
            catch (Exception)
            {
            }
            return imagetype;
        }

    }
    public enum ImageType
    {
        JPG = 255216,
        GIF = 7173,
        PNG = 13780,
        SWF = 6787,
        RAR = 8297,
        ZIP = 8075,
        _7Z = 55122,
        VALIDFILE = 9999999
    }
}
