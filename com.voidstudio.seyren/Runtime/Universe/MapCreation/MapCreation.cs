namespace Seyren.Universe {
    public class MapCreation : IMapCreationStrategy<Map>
    {
        private int gridSizeX;
        private int gridSizeY;
        private bool visible;

        public MapCreation(int gridSizeX, int gridSizeY, bool visible)
        {
            this.gridSizeX = gridSizeX;
            this.gridSizeY = gridSizeY;
            this.visible = visible;
        }

        public Map CreateMap()
        {
            Map m = new Map(gridSizeX, gridSizeY);
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    m.SetCellProperties(x, y, visible, true);
                }
            }

            return m;
        }
    }
}