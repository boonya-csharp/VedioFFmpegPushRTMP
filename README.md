### FFmpegSharp is a fluent api encapsulation of ffmpeg with C#

## Encode media(with snapshot)
```csharp
var currentDir =
new FileInfo(Uri.UnescapeDataString(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath));

var inputPath = Path.Combine(appPath, "test.mov");
var outputPath = Path.Combine(appPath, Guid.NewGuid().ToString());
var image = Path.Combine(appPath, "logo.png");

if (string.IsNullOrWhiteSpace(appPath))throw new ApplicationException("app path not found.");


var inputPath = Path.Combine(appPath, "test.mov");
var outputPath = Path.Combine(appPath, Guid.NewGuid().ToString());

Encoder.Create()
	.WidthInput(inputPath)
	.WithFilter(new X264Filter { Preset = X264Preset.Faster, ConstantQuantizer = 18 })
	.WithFilter(new ImageWatermarkFilter(image, WatermarkPosition.TopRight))
	.WithFilter(new ResizeFilter(Resolution.X720P))
	.WithFilter(new SnapshotFilter(Path.Combine(appPath,"snapshot","out.png"),320,180,10))//with snapshot
	.To<Mp4>(outputPath)
	.Execute();

```


## Push a file to RTMP Server
```csharp
Network.Create()
	.WithSource(inputPath)
	.WithDest("rtmp://192.168.10.12/live/stream")
	.WithFilter(new X264Filter{ConstantQuantizer = 20})
	.WithFilter(new ResizeFilter(980,550))
	.Push();
```


### FFmpegLib
if you want build this project,
please donwload ffmpeg lib first.

for x32 build with:
http://ffmpeg.zeranoe.com/builds/win32/shared/ffmpeg-20141117-git-3f07dd6-win32-shared.7z

for x64 build withd:
http://ffmpeg.zeranoe.com/builds/win64/shared/ffmpeg-20141117-git-3f07dd6-win64-shared.7z

after extract the files, copy the contents of the 'bin' folder to the path '/external/ffmpeg/x32(or x64)/'


## License

[MIT](https://github.com/at0717/FFmpegSharp/blob/master/LICENSE)