using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Platform.Avalonia
{
    public interface IImageSourceHandler : IRegisterable
    {
        Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancelationToken = default(CancellationToken));
    }

    public sealed class FileImageSourceHandler : IImageSourceHandler
    {
        public Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancelationToken = new CancellationToken())
        {
            global::Avalonia.Media.Imaging.Bitmap image = null;
            FileImageSource filesource = imagesoure as FileImageSource;
            if (filesource != null)
            {
                string file = filesource.File;
                image = new Bitmap(file);
            }
            return Task.FromResult(image);
        }
    }

    public sealed class StreamImageSourceHandler : IImageSourceHandler
    {
        public async Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = new CancellationToken())
        {
            Bitmap bitmapImage = null;
            StreamImageSource streamImageSource = imagesource as StreamImageSource;

            if (streamImageSource != null && streamImageSource.Stream != null)
            {
                using (Stream stream = await ((IStreamImageSource)streamImageSource).GetStreamAsync(cancelationToken))
                {
                    bitmapImage = new Bitmap(stream);
                }
            }

            return bitmapImage;
        }
    }

    public sealed class UriImageSourceHandler : IImageSourceHandler
    {
        public Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancelationToken = new CancellationToken())
        {
            Bitmap bitmapimage = null;
            var imageLoader = imagesoure as UriImageSource;
            if (imageLoader?.Uri != null)
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(imageLoader?.Uri);
                var responseStream = webRequest.GetResponse().GetResponseStream();
                byte[] buffer = new byte[1024];
                var memoryStream = new MemoryStream();
                while (true)
                {
                    int read = responseStream.Read(buffer, 0, buffer.Length);
                    if (read <= 0) break;
                    memoryStream.Write(buffer, 0, read);
                }
                memoryStream.Position = 0;
                bitmapimage = new Bitmap(memoryStream);
            }
            return Task.FromResult<Bitmap>(bitmapimage);
        }
    }

    public sealed class FontImageSourceHandler : IImageSourceHandler
    {
        public Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = new CancellationToken())
        {
            var fontsource = imagesource as FontImageSource;
            var image = CreateGlyph(
                    fontsource.Glyph,
                    new FontFamily(new Uri("pack://application:,,,"), fontsource.FontFamily),
                    FontStyle.Normal,
                    FontWeight.Normal,
                    fontsource.Size,
                    (fontsource.Color != Color.Default ? fontsource.Color : Color.White).ToBrush());
            return Task.FromResult(image);
        }

        static global::Avalonia.Media.Imaging.Bitmap CreateGlyph(
            string text,
            FontFamily fontFamily,
            FontStyle fontStyle,
            FontWeight fontWeight,
            //FontStretch fontStretch,
            double fontSize,
            Brush foreBrush)
        {
            if (fontFamily == null || string.IsNullOrEmpty(text))
            {
                return null;
            }
            var typeface = new Typeface(fontFamily, fontSize, fontStyle, fontWeight);
            //if (!typeface.TryGetGlyphTypeface(out GlyphTypeface glyphTypeface))
            //{
            //    //if it does not work 
            //    return null;
            //}

            //var glyphIndexes = new ushort[text.Length];
            //var advanceWidths = new double[text.Length];
            //for (int n = 0; n < text.Length; n++)
            //{
            //    var glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
            //    glyphIndexes[n] = glyphIndex;
            //    var width = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
            //    advanceWidths[n] = width;
            //}

            //var gr = new GlyphRun(glyphTypeface,
            //    0, false,
            //    fontSize,
            //    glyphIndexes,
            //    new global::Avalonia.Point(0, 0),
            //    advanceWidths,
            //    null, null, null, null, null, null);
            //var glyphRunDrawing = new GlyphRunDrawing(foreBrush, gr);
            //return new DrawingImage(glyphRunDrawing);
            // TODO:
            return null;
        }
    }

}
