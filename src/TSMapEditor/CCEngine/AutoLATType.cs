namespace TSMapEditor.CCEngine
{
    public struct AutoLATData(int transitionTypeTileSetIndex, int[] transitionMatchArray)
    {
        public int TransitionTypeIndex = transitionTypeTileSetIndex;
        public int[] TransitionMatchArray = transitionMatchArray;
    }

    public static class AutoLATType
    {
        // Transition types and related tileset indexes
        public const int SURROUNDS_TILE = 0;
        public const int NE_TRANSITION = 1;
        public const int SE_TRANSITION = 2;
        public const int NE_SE_TRANSITION = 3;
        public const int SW_TRANSITION = 4;
        public const int NE_SW_TRANSITION = 5;
        public const int SE_SW_TRANSITION = 6;
        public const int NE_SE_SW_TRANSITION = 7;
        public const int NW_TRANSITION = 8;
        public const int NE_NW_TRANSITION = 9;
        public const int NW_SE_TRANSITION = 10;
        public const int NE_NW_SE_TRANSITION = 11;
        public const int NW_SW_TRANSITION = 12;
        public const int NE_NW_SW_TRANSITION = 13;
        public const int NW_SE_SW_TRANSITION = 14;
        public const int TRANSITION_TO_ALL_DIRECTIONS = 15;

        // Array / matrix index definitions
        public const int NE_INDEX = 0;
        public const int NW_INDEX = 1;
        public const int CENTER_INDEX = 2;
        public const int SE_INDEX = 3;
        public const int SW_INDEX = 4;

        // Transition arrays for different transition types.
        // These are used to check which transition type a tile should use by checking the nearby tiles.
        // 1 means that the tile matches the ground type being placed, 0 means that the tile has something else.
        // If you "rotate" these 45 degrees clockwise, these match the isometric perspective.
        public static readonly int[] A_SURROUNDS_TILE =
        [ 1,
        1,0,1,
          1 ];

        public static readonly int[] A_NE_TRANSITION =
        [ 0,
        1,1,1,
          1 ];

        public static readonly int[] A_SE_TRANSITION =
        [ 1,
        1,1,0,
          1 ];

        public static readonly int[] A_NE_SE_TRANSITION =
        [ 0,
        1,1,0,
          1 ];

        public static readonly int[] A_SW_TRANSITION =
        [ 1,
        1,1,1,
          0 ];

        public static readonly int[] A_NE_SW_TRANSITION =
        [ 0,
        1,1,1,
          0 ];

        public static readonly int[] A_SE_SW_TRANSITION =
        [ 1,
        1,1,0,
          0 ];

        public static readonly int[] A_NE_SE_SW_TRANSITION =
        [ 0,
        1,1,0,
          0 ];

        public static readonly int[] A_NW_TRANSITION =
        [ 1,
        0,1,1,
          1 ];

        public static readonly int[] A_NE_NW_TRANSITION =
        [ 0,
        0,1,1,
          1 ];

        public static readonly int[] A_NW_SE_TRANSITION =
        [ 1,
        0,1,0,
          1 ];

        public static readonly int[] A_NE_NW_SE_TRANSITION =
        [ 0,
        0,1,0,
          1 ];

        public static readonly int[] A_NW_SW_TRANSITION =
        [ 1,
        0,1,1,
          0 ];

        public static readonly int[] A_NE_NW_SW_TRANSITION =
        [ 0,
        0,1,1,
          0 ];

        public static readonly int[] A_NW_SE_SW_TRANSITION =
        [ 1,
        0,1,0,
          0 ];

        public static readonly int[] A_TRANSITION_TO_ALL_DIRECTIONS =
        [ 0,
        0,1,0,
          0 ];

        /// <summary>
        /// Links transition indexes and related data arrays.
        /// </summary>
        public static AutoLATData[] AutoLATData { get; private set; }

        public static void InitArray()
        {
            AutoLATData =
            [
                new(SURROUNDS_TILE, A_SURROUNDS_TILE),
                new(NE_TRANSITION, A_NE_TRANSITION),
                new(SE_TRANSITION, A_SE_TRANSITION),
                new(NE_SE_TRANSITION, A_NE_SE_TRANSITION),
                new(SW_TRANSITION, A_SW_TRANSITION),
                new(NE_SW_TRANSITION, A_NE_SW_TRANSITION),
                new(SE_SW_TRANSITION, A_SE_SW_TRANSITION),
                new(NE_SE_SW_TRANSITION, A_NE_SE_SW_TRANSITION),
                new(NW_TRANSITION, A_NW_TRANSITION),
                new(NE_NW_TRANSITION, A_NE_NW_TRANSITION),
                new(NW_SE_TRANSITION, A_NW_SE_TRANSITION),
                new(NE_NW_SE_TRANSITION, A_NE_NW_SE_TRANSITION),
                new(NW_SW_TRANSITION, A_NW_SW_TRANSITION),
                new(NE_NW_SW_TRANSITION, A_NE_NW_SW_TRANSITION),
                new(NW_SE_SW_TRANSITION, A_NW_SE_SW_TRANSITION),
                new(TRANSITION_TO_ALL_DIRECTIONS, A_TRANSITION_TO_ALL_DIRECTIONS),
            ];
        }
    }
}
