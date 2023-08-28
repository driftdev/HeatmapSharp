# HeatmapSharp a dotnet based image heat mapping helper built on ImageSharp

- Not as good as any python implementation built with matplotlib and numpy
- This doesn't have any smart mapping features because i am not an image expert. And only way to get smart mapping features is to use python libraries in dotnet using [Python.NET](https://github.com/pythonnet/pythonnet). Which means the runtime is dependent on python and it can break any dotnet docker builds. So python in dotnet is a no go.
- The basic mapping is done if a set of points are given and the image is inputted.
- External dependency
  -  [ImageSharp](https://github.com/SixLabors/ImageSharp).


## Features
- Map a heatmap to an Image when the image and the points are given. That's it.

## Progress
- [x] Basic image heat mapping.
- [x] Retrieve color maps from asset images.
- [ ] Make the image processing faster by using the low level image processing layer from ImageSharp.
- [ ] Make the mapping faster by understanding point density on point locations.
- [ ] optimize the jet based color pallet have more color points and use a color image for it.

## Usage

- Import the image mapper
```c#
using HeatmapSharp.ImageMapper;
```

- Create a instance of HeatMapper
```c#
/* 
HeatMapper is configurable with 
HeatMapper(int pointDiameter, float pointStrength, float opacity, string colors) 
*/

/* 
pointDiameter is an integer of the the diameter of the marking point
default pointDiameter = 50 which is 50px (1 = 1px)
*/

/* 
pointStrength is an float from 0-1 marks the strength or opacity of the point
default pointDiameter 0.2f
*/

/* 
opacity is an float from 0-1 is the opacity of the heatmap overlay on the original image
default opacity 0.65f
*/

/* 
colors is the heatmap color format used to color the heatmap
there are two formats "default" and "reveal"
if not set explicitly it will be "default"
- "default" is a jet style rainbow color map
- "reveal" is a dark and light style color map  
*/

HeatMapper heatMapper = new HeatMapper(50, 0.2f, 0.65f, "default");
```

- Use the ImageToHeatMap method to heatmap an image
```c#
// pass the image and the points to ImageToHeatMap(image, points)
// image is a Image<Rgba32> type image from ImageSharp
// points are an IEnumerable<(int, int)> of points

Image<Rgba32> heatmap = heatMapper.ImageToHeatMap();
```