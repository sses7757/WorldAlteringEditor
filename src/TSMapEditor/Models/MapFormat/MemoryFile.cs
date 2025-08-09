using System.IO;

namespace CNCMaps.FileFormats.VirtualFileSystem
{

    /// <summary>Virtual file from a memory buffer.</summary>
    public class MemoryFile(byte[] buffer, bool isBuffered = true) : VirtualFile(new MemoryStream(buffer), "MemoryFile", 0, buffer.Length, isBuffered)
    {
    }
}