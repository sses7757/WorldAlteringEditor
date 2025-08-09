using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using TSMapEditor.CCEngine;

namespace TSMapEditor.UI.Controls
{
    public class TileSetListBox(WindowManager windowManager, int tileSetCount) : XNAListBox(windowManager)
    {
        private readonly bool[] tileSetIsFavourite = new bool[tileSetCount];

        private readonly Texture2D favouriteTileSetTexture = AssetLoader.LoadTexture("star.png");

        public bool IsTileSetFavourite(int tileSetIndex) => tileSetIsFavourite[tileSetIndex];

        public void SetTileSetAsFavourite(int tileSetIndex) => tileSetIsFavourite[tileSetIndex] = true;

        public void ClearFavouriteStatus(int tileSetIndex) => tileSetIsFavourite[tileSetIndex] = false;

        protected override void DrawListBoxItem(int index, int y)
        {
            base.DrawListBoxItem(index, y);
            var tileSet = (TileSet)Items[index].Tag;

            if (!IsTileSetFavourite(tileSet.Index))
                return;

            int lineWidth = Width - 2 - (EnableScrollbar && ScrollBar.IsDrawn() ? ScrollBar.Width : 0);

            int textureYOffset = (LineHeight - favouriteTileSetTexture.Height) / 2;

            DrawTexture(favouriteTileSetTexture, new Point(lineWidth - 1 - favouriteTileSetTexture.Width, y + textureYOffset), Color.White);
        }
    }
}
