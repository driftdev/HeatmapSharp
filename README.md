# HeatmapSharp a dotnet based image heat mapping helper built on ImageSharp

- Not as good as any python implementation built with matplotlib and numpy
- This doesn't have any smart mapping features because i am not an image processing expert. The only way to get reliable smart mapping features is to use python libraries in dotnet using [Python.NET](https://github.com/pythonnet/pythonnet). Which means the runtime is dependent on python and it can break dotnet docker builds. So python in dotnet is a no go.
- The basic mapping is done if a set of points are given and the image is inputted.
- External dependency
  -  [ImageSharp](https://github.com/SixLabors/ImageSharp).


## Features
- Map a heatmap to an Image when the image and the points are given. That's it.

## Progress
- [x] Basic image heat mapping.
- [x] Retrieve color maps from asset images.

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
default pointOpacity 0.2f
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
// points are an IEnumerable<PointModel> of points

Image<Rgba32> heatmap = heatMapper.ImageToHeatMap();
```

## Change Log

- v0.0.1
  - Dependency Update 
    - used ImageSharp v3.0.1

- v0.0.2
  - Dependency Update 
    - ImageSharp update to v3.0.2
  - Bug Fix
    - Fixed issue where if a point goes out of bound in an image it raises a exception. Fixed it to omit the point that goes out of bound in the mapping process 
