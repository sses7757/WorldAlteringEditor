using Microsoft.Xna.Framework.Graphics;
using System;
using TSMapEditor.Models;

namespace TSMapEditor.Rendering.ObjectRenderers
{
    public struct RenderDependencies(Map map,
        TheaterGraphics theaterGraphics,
        EditorState editorState,
        GraphicsDevice graphicsDevice,
        ObjectSpriteRecord objectSpriteRecord,
        Effect palettedColorDrawEffect,
        Camera camera,
        Func<int> getCameraRightXCoord,
        Func<int> getCameraBottomYCoord)
    {
        public readonly Map Map = map;
        public readonly TheaterGraphics TheaterGraphics = theaterGraphics;
        public readonly EditorState EditorState = editorState;
        public readonly GraphicsDevice GraphicsDevice = graphicsDevice;
        public readonly ObjectSpriteRecord ObjectSpriteRecord = objectSpriteRecord;
        public readonly Effect PalettedColorDrawEffect = palettedColorDrawEffect;
        public readonly Camera Camera = camera;
        public readonly Func<int> GetCameraRightXCoord = getCameraRightXCoord;
        public readonly Func<int> GetCameraBottomYCoord = getCameraBottomYCoord;
    }
}
