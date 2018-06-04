namespace Services.Models
{
    public class DimensionsModel
    {
        public DimensionsModel(int height, int width) 
        {
            this.Height = height;
            this.Width = width;
        }
        
        public int Height { get; private set; }

        public int Width { get; private set; }
    }
}