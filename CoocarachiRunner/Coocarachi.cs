using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CoocarachiRunner
{
    public class Coocarachi
    {
        public Coocarachi(Color background, Point corner, int speed)
        {
            Background = background;
            Corner = corner;
            Speed = speed;
            Ellipse = Build();
        }

        public Color Background { get; private set; }

        public Point Corner { get; private set; }

        public int Speed { get; private set; }

        public Ellipse Ellipse { get; private set; }

        public Ellipse Build()
        {
            return new Ellipse
            {
                Fill = new SolidColorBrush(Background),
                Height = 100,
                Width = 50,
                StrokeThickness=1
            }; ;
        }
     }
}
