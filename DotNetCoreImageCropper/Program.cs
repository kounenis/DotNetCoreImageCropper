using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DotNetCoreImageCropper
{
    class Program
    {
        public class Options
        {
            [Option("startx", Required = false, HelpText = "Crop x start coordinate.", Default = 0)]
            public int StartX { get; set; }

            [Option("starty", Required = false, HelpText = "Crop x start coordinate", Default = 0)]
            public int StartY { get; set; }

            [Option("width", Required = true, HelpText = "Crop width")]
            public int Width { get; set; }

            [Option("height", Required = false, HelpText = "Crop height")]
            public int Height { get; set; }

            [Option("source", Required = true, HelpText = "Source image")]
            public string Source { get; set; }

            [Option("target", Required = true, HelpText = "Target image")]
            public string Target { get; set; }
        }

        static void Main(string[] args)
        {
            var parsedArguments = Parser.Default.ParseArguments<Options>(args);

            parsedArguments.WithParsed<Options>(o =>
            {
                try
                {
                    CropImage(o);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to crop due to ${e.GetType()}: {e.Message}");
                }
            });
        }

        private static void CropImage(Options options)
        {
            if (!File.Exists(options.Source))
            {
                throw new Exception("Source file not found");
            }
            Image sourceImage = Image.Load(File.ReadAllBytes(options.Source));
            sourceImage.Mutate(ctx => ctx.Crop(new Rectangle(options.StartX, options.StartY, options.Width, options.Height)));
            sourceImage.Save(options.Target);
        }
    }
}
