namespace FFmpegSharp.Codes
{
    public class CodeBase
    {
        public string Name { get; protected set; }
        public string Extension { get; protected set; }
        public CodeType CodeType { get; protected set; }
        public CodeBase()
        {
            
        }
    }
}