namespace FFmpegSharp.Codes
{
    public class Wav : CodeBase
    {
        public Wav()
        {
            CodeType = CodeType.Audio;
            Name = "WAV";
            Extension = ".wav";
        }
    }
}