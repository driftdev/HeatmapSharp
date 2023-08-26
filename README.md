# HeatmapSharp a dotnet based image heat mapping helper built on ImageSharp

- Not as good as any python implementation built with matplotlib and numpy
- This doesn't have any smart mapping features because i am not an image expert. And only way to get smart mapping features is to use python libraries in dotnet using [Python.NET](https://github.com/pythonnet/pythonnet). Which means the runtime is dependent on python and it can break any dotnet docker builds. So python in dotnet is a no go.
- The basic mapping is done if a set of pints are given and the image is inputted.
- External dependency
  -  [ImageSharp](https://github.com/SixLabors/ImageSharp).


## Features
- Map a heatmap to an Image when the image and the points are given. Thats it.