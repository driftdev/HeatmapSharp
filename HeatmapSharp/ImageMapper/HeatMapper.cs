namespace HeatmapSharp.ImageMapper;

public class HeatMapper
{
    private const string DefaultPng = "HeatmapSharp.Assets.default.png";
    private const string RevealPng = "HeatmapSharp.Assets.reveal.png";
    
    private readonly int _pointDiameter;
    private readonly float _pointStrength;
    private readonly float _opacity;
    private Rgba32[] _colorMap = null!;

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
        SetColor(colors);
    }
    
    private void SetColor(string colors)
    {
        Image<Rgba32> colorImage = null!;
        
        switch (colors)
        {
            case "default":
            {
                using var defaultPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(DefaultPng);
                if (defaultPngStream is not null) colorImage = Image.Load<Rgba32>(defaultPngStream);
                _colorMap = ExtractColorMapFromImage(colorImage);
                break;
            }
            case "reveal":
            {
                using var revealPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(RevealPng);
                if (revealPngStream is not null) colorImage = Image.Load<Rgba32>(revealPngStream);
                _colorMap = ExtractColorMapFromImage(colorImage);
                break;
            }
            default:
            {
                using var defaultPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(DefaultPng);
                if (defaultPngStream is not null) colorImage = Image.Load<Rgba32>(defaultPngStream);
                _colorMap = ExtractColorMapFromImage(colorImage);
                break;
            }
        }
    }

    private static Rgba32[] ExtractColorMapFromImage(Image<Rgba32> colorImage)
    {
        var colorMap = new List<Rgba32>();

        for (var x = 0; x < colorImage.Width; x++)
        {
            var pixel = colorImage[x, 0];
            colorMap.Add(pixel);
        }
        
        return colorMap.ToArray();
    }

    
    public Image<Rgba32> ImageToHeatMap(Image<Rgba32> image, IEnumerable<(int, int)> points)
    {
        GreyHeatMapper geryMapper = new(_pointDiameter, _pointStrength);
        
        var greyHeatMap = geryMapper.ImageToGreyHeatMap(image.Width, image.Height, points);
        
        var colorHeatMap = ColorHeatMap(greyHeatMap);
        
        return BlendImage(colorHeatMap, image);
    }
    
    private Image<Rgba32> ColorHeatMap(Image<L8> greyImage)
    {
        var colorHeatMap = new Image<Rgba32>(greyImage.Width, greyImage.Height);
        
        for (var y = 0; y < greyImage.Height; y++)
        {
            for (var x = 0; x < greyImage.Width; x++)
            {
                var pixelValue = greyImage[x, y];
                var index = (int)Math.Round((_colorMap.Length - 1) * (pixelValue.PackedValue / 255.0));
                var color = _colorMap[index];
                colorHeatMap[x, y] = color;
            }
        }

        return colorHeatMap;
    }
    
    private Image<Rgba32> BlendImage(Image<Rgba32> overlayImage, Image<Rgba32> sourceImage)
    {
        var overlay = overlayImage.Clone();
        
        var source = sourceImage.Clone();
        source.Mutate(s => s.DrawImage(overlay, new Point(0, 0), _opacity));
        
        return source.Clone();
    }
}