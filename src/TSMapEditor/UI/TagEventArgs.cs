using System;
using TSMapEditor.Models;

namespace TSMapEditor.UI
{
    public class TagEventArgs(Tag tag) : EventArgs
    {
        public Tag Tag { get; } = tag;
    }
}
