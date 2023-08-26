using System.Resources;

namespace HeatmapSharp.ImageMapper;

public class HeatMapper
{
    private const string DefaultPng = "HeatmapSharp.Assets.default.png";
    private const string RevealPng = "HeatmapSharp.Assets.reveal.png";
    
    private readonly int _pointDiameter;
    private readonly float _pointStrength;
    private readonly float _opacity;
    private readonly string _colors;
    // private Image<Rgba32> _colorImage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HeatMapper" /> class.
    /// </summary>
    /// <param name="pointDiameter">Sets the diameter of the marking dot points (measured in pixels 1 = 1px) (if not specified the default diameter of 50 is used)</param>
    /// <param name="pointStrength">Sets the strength of the marking dot points (measured from 0 to 1) (if not specified the default strength of 0.2 is used)</param>
    /// <param name="opacity">Sets the blend opacity for the heatmap overlay (measured from 0 to 1) (if not specified the default opacity of 0.65 is used)</param>
    /// <param name="colors">Sets the colors modes which can be "reveal" or "default" (if not specified the "default" colors are used)</param>
    public HeatMapper(int pointDiameter = 50, float pointStrength = 0.2f, float opacity = 0.65f, string colors = "default")
    {
        _pointDiameter = pointDiameter;
        _pointStrength = pointStrength;
        _opacity = opacity;
        _colors =  colors;
    }
    
    // todo implement the color mapping using color images
    // private void SetColor(string colors)
    // {
    //     switch (colors)
    //     {
    //         case "default":
    //         {
    //             using var defaultPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(DefaultPng);
    //             if (defaultPngStream != null) _colorImage = Image.Load<Rgba32>(defaultPngStream);
    //             break;
    //         }
    //         case "reveal":
    //         {
    //             using var revealPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(RevealPng);
    //             if (revealPngStream != null) _colorImage = Image.Load<Rgba32>(revealPngStream);
    //             break;
    //         }
    //         default:
    //         {
    //             using var defaultPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(DefaultPng);
    //             if (defaultPngStream != null) _colorImage = Image.Load<Rgba32>(defaultPngStream);
    //             break;
    //         }
    //     }
    // }

    public Image<Rgba32> ImageToHeatMap(Image<Rgba32> image, IEnumerable<(int, int)> points)
    {
        GreyHeatMapper geryMapper = new(_pointDiameter, _pointStrength);
        
        var greyHeatMap = geryMapper.ImageToGreyHeatMap(image.Width, image.Height, points);
        
        var colorHeatMap = ColorHeatMap(greyHeatMap);
        
        return BlendImage(colorHeatMap, image);
    }
    
    private Image<Rgba32> ColorHeatMap(Image<L8> greyImage)
    {
        var colorMap = _colors switch
        {
            "default" => GenerateDefaultColorMap(),
            "reveal" => GenerateRevealColorMap(),
            _ => GenerateDefaultColorMap()
        };

        var colorHeatMap = new Image<Rgba32>(greyImage.Width, greyImage.Height);
        
        // todo find a better way to set the color rather than loops
        // loop through each pixel in the grey heatmap and according to the strength of the points set the color
        for (var y = 0; y < greyImage.Height; y++)
        {
            for (var x = 0; x < greyImage.Width; x++)
            {
                var pixelValue = greyImage[x, y];
                var index = (int)Math.Round((colorMap.Length - 1) * (pixelValue.PackedValue / 255.0));
                var color = colorMap[index];
                colorHeatMap[x, y] = color;
            }
        }

        return colorHeatMap;
    }
    
    private static Rgba32[] GenerateDefaultColorMap()
    {
        Rgba32[] colorMap = {
            new(255, 0, 0, 100),    // Red
            new(255, 255, 0, 100), // Yellow
            new(0, 255, 0, 100),   // Green
            new(0, 255, 255, 100), // Cyan
            new(0, 0, 255, 100),   // Blue
            new(255, 255, 255, 0) // transparent
        };
    
        return colorMap;
    }
    
    private static Rgba32[] GenerateRevealColorMap()
    {
        Rgba32[] colorMap = {
            new(0, 0, 255, 100),   // Black
            new(255, 255, 255, 0) // transparent
        };
    
        return colorMap;
    }
    
    private Image<Rgba32> BlendImage(Image<Rgba32> overlayImage, Image<Rgba32> sourceImage)
    {
        var overlay = overlayImage.Clone();
        
        var source = sourceImage.Clone();
        source.Mutate(s => s.DrawImage(overlay, new Point(0, 0), _opacity));
        
        return source.Clone();
    }
}