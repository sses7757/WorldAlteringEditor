using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TSMapEditor.GameMath;
using TSMapEditor.Models;

namespace TSMapEditor.Rendering
{
    public struct ObjectSpriteEntry(Texture2D paletteTexture, Texture2D texture, Rectangle drawingBounds, Color color, bool useRemap, bool useShadow, float depth)
    {
        public Texture2D PaletteTexture = paletteTexture; // 8 bytes
        public Texture2D Texture = texture;        // 16 bytes
        public Rectangle DrawingBounds = drawingBounds;        // 24 bytes
        public Color Color = color;              // 28 bytes
        public bool UseRemap = useRemap;            // 29 bytes
        public bool UseShadow = useShadow;           // 30 bytes
        public float Depth = depth;           // 34 bytes
    }

    public struct ObjectDetailEntry(Texture2D texture, Rectangle drawingBounds, Color color, float depth)
    {
        public Texture2D Texture = texture;
        public Rectangle DrawingBounds = drawingBounds;
        public Color Color = color;
        public float Depth = depth;
    }

    public struct ShadowEntry(Texture2D texture, Rectangle drawingBounds, float depth)
    {
        public Texture2D Texture = texture;
        public Rectangle DrawingBounds = drawingBounds;
        public float Depth = depth;
    }

    public struct TextEntry(string text, Color color, Point2D drawPoint)
    {
        public string Text = text;
        public Color Color = color;
        public Point2D DrawPoint = drawPoint;
    }

    public struct LineEntry(Vector2 source, Vector2 destination, Color color, int thickness, float depth)
    {
        public Vector2 Source = source;
        public Vector2 Destination = destination;
        public Color Color = color;
        public int Thickness = thickness;
        public float Depth = depth;
    }

    /// <summary>
    /// Makes it possible to batch sprites that are originally
    /// processed in any order, with any kinds of required shader settings.
    /// 
    /// Keeps track of sprites and other graphics that should be drawn as the
    /// renderer processes objects. Finally, the renderer can process the lists
    /// of this class to draw the objects.
    /// </summary>
    public class ObjectSpriteRecord
    {
        public Dictionary<(Texture2D, bool), List<ObjectDetailEntry>> SpriteEntries = [];
        public List<ObjectDetailEntry> NonPalettedSpriteEntries = [];
        public List<ShadowEntry> ShadowEntries = [];
        public List<TextEntry> TextEntries = [];
        public List<LineEntry> LineEntries = [];
        public HashSet<GameObject> ProcessedObjects = [];

        public void AddGraphicsEntry(in ObjectSpriteEntry entry)
        {
            if (entry.Texture == null)
                throw new ArgumentNullException(nameof(entry));

            // Shadows are handled separately
            if (entry.UseShadow)
            {
                ShadowEntries.Add(new ShadowEntry(entry.Texture, entry.DrawingBounds, entry.Depth));
                return;
            }

            // If the entry has no palette, we can store it separately
            if (entry.PaletteTexture == null)
            {
                NonPalettedSpriteEntries.Add(new ObjectDetailEntry(entry.Texture, entry.DrawingBounds, new Color(entry.Color.R / 255.0f, entry.Color.G / 255.0f, entry.Color.B / 255.0f, entry.Depth), entry.Depth));
                return;
            }

            // Paletted entries, with or without remap
            var key = (entry.PaletteTexture, entry.UseRemap);
            bool success = SpriteEntries.TryGetValue(key, out var list);
            if (!success)
            {
                list = [];
                SpriteEntries.Add(key, list);
            }

            list.Add(new ObjectDetailEntry(entry.Texture, entry.DrawingBounds, entry.Color, entry.Depth));
        }

        public void AddTextEntry(TextEntry textEntry) => TextEntries.Add(textEntry);

        public void AddLineEntry(LineEntry lineEntry) => LineEntries.Add(lineEntry);

        public void Clear(bool noShadow)
        {
            foreach (var list in SpriteEntries.Values)
            {
                list.Clear();
            }

            NonPalettedSpriteEntries.Clear();

            if (!noShadow)
                ShadowEntries.Clear();

            TextEntries.Clear();

            LineEntries.Clear();

            ProcessedObjects.Clear();
        }
    }
}
