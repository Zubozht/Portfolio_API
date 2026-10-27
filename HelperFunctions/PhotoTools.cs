using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;

namespace api.HelperFunctions
{
    public static class PhotoTools
    {
        public static string NormalizeShutterSpeed (float shutterspeedvalue)
                    {
                        float exposuretime = (float)Math.Pow(2, -shutterspeedvalue);

                        if (exposuretime >= 1)
                        {
                            return exposuretime.ToString("0.#");
                        }
                        else
                        {
                            return $"1/{(int)Math.Round(1/exposuretime)}";
                        }
                    }

        public static float ShutterspeedToFloat(string sh)
        {
            if (float.TryParse(sh, out float intsh))
            {
                return intsh;
            }
            else if (sh.Contains('/') && (sh.Count(c => c == '/') == 1) && !sh.StartsWith('/') && !sh.EndsWith('/'))
            {
                if (float.TryParse(sh.Substring(0, sh.IndexOf('/')), out float dividend) && float.TryParse(sh.Substring(sh.IndexOf('/') + 1), out float divisor))
                {
                    return dividend / divisor;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public static void SaveImage(this Photo photoModel)
        {
            if (photoModel.Path != null)
            {
                using (var mainimage = new MagickImage(photoModel.Path))
                using (var previewimage = new MagickImage(photoModel.Path))
                {
                    var profile = mainimage.GetExifProfile();

                    if (profile != null)
                    {
                        foreach (var value in profile.Values)
                        {
                            if (value.Tag == ExifTag.ExposureTime)
                            {
                                photoModel.Exposure = value.ToString();
                            }
                            if (value.Tag == ExifTag.ShutterSpeedValue)
                            {
                                var apexshutterspeed = ((SignedRational)value.GetValue()).ToDouble();
                                photoModel.Shutterspeed = (int)Math.Ceiling(Math.Pow(2, apexshutterspeed));
                            }
                            if (value.Tag == ExifTag.FNumber)
                            {
                                var rational = value.GetValue() as Rational?;
                                if (rational.HasValue)
                                {
                                    photoModel.Apperture = (float)rational.Value.ToDouble();
                                };
                            }
                            if (value.Tag == ExifTag.RecommendedExposureIndex)
                            {
                                int.TryParse(value.ToString(), out int intvalue);
                                photoModel.Iso = intvalue;
                            }
                            //_logger.LogInformation($"{value.Tag}({value.DataType}): {value.ToString()}");
                        };
                    };

                    mainimage.Strip();
                    previewimage.Strip();

                    if ((mainimage.Width > 2000) || (mainimage.Height > 2000))
                    {
                        mainimage.Resize(new MagickGeometry(2000, 2000)
                        {
                            IgnoreAspectRatio = false
                        });
                    }

                    previewimage.Resize(new MagickGeometry(1000, 1000)
                    {
                        IgnoreAspectRatio = false
                    });

                    uint mainquality = 100;
                    uint previewquality = 100;

                    mainimage.Format = MagickFormat.Jpg;
                    previewimage.Format = MagickFormat.Jpg;

                    while (mainquality > 5)
                    {
                        mainimage.Quality = mainquality;

                        if (mainimage.ToByteArray().Length > 2000 * 1024)
                        {
                            mainquality -= 5;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (previewquality > 5)
                    {
                        previewimage.Quality = previewquality;

                        if (previewimage.ToByteArray().Length > 500 * 1024)
                        {
                            previewquality -= 5;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"{photoModel.Id.ToString().PadLeft(6, '0')}");
                    File.Delete(photoModel.Path);
                    photoModel.Path = $"{imagepath}.jpg";
                    photoModel.PreviewPath = $"{imagepath}p.jpg";
                    mainimage.Write(photoModel.Path);
                    previewimage.Write(photoModel.PreviewPath);
                };
            }
            else
            {
                throw new InvalidOperationException("Photo path not set.");
            }

            //return photoModel;
        }
    }
}