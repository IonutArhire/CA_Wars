namespace Services.Models
{
    public class DimensionsModel
    {
        public DimensionsModel(int height, int width) 
        {
            this.Height = height;
            this.Width = width;
        }
        
        public int Height { get; set; }

        public int Width { get; set; }
    }
}