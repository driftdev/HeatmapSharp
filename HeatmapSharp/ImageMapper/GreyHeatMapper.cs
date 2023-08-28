namespace HeatmapSharp.ImageMapper;

public class GreyHeatMapper
{
    private const string PixelDotPng = "HeatmapSharp.Assets.450pxdot.png";
    
    private readonly int _pointDiameter;
    private readonly float _pointStrength;
    private readonly Image<Rgba32>? _pixelDotPng;
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="GreyHeatMapper" /> class.
    /// </summary>
    /// <param name="pointDiameter">Sets the diameter of the marking dot points (measured in pixels 1 = 1px)</param>
    /// <param name="pointStrength">Sets the strength of the marking dot points (measured from 0 to 1)</param>
    public GreyHeatMapper(int pointDiameter, float pointStrength)
    {
        _pointDiameter = pointDiameter;
        _pointStrength = pointStrength;
        
        using var pixelDotPngStream = typeof(HeatMapper).Assembly.GetManifestResourceStream(PixelDotPng);
        if (pixelDotPngStream is not null) _pixelDotPng = Image.Load<Rgba32>(pixelDotPngStream);
    }

    // creates a empty grey heatmap and uses the dot image (450pxdot.png) to map the points
    public Image<L8> ImageToGreyHeatMap(int width, int height,IEnumerable<(int, int)> points)
    {
        var greyHeatMap = new Image<L8>(width, height);
        for (var y = 0; y < greyHeatMap.Height; y++)
        {
            for (var x = 0; x < greyHeatMap.Width; x++)
            {
                greyHeatMap[x, y] = new L8(byte.MaxValue);
            }
        }
        
        if (_pixelDotPng == null) return greyHeatMap.Clone();
        var dot = _pixelDotPng.Clone();
        dot.Mutate(d => d.Resize(new Size(_pointDiameter, _pointDiameter)));
        dot.Mutate(d => d.Opacity(_pointStrength));
    
        foreach (var point in points)
        {
            var x = point.Item1 - _pointDiameter / 2;
            var y = point.Item2 - _pointDiameter / 2;
    
            greyHeatMap.Mutate(h =>
            {
                using var tempDot = dot.Clone();
                h.DrawImage(tempDot, new Point(x, y), 1f);
            });
        }

        greyHeatMap.Mutate(h => h.Grayscale());
        
        return greyHeatMap.Clone();
    }
}