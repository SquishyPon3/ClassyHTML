using ClassyHTML.Attributes;

namespace ClassyHTML.VoidElements
{
    class Input : VoidElement
    {
        protected override string _Name { get; set; } = "input";
        string Value = "";
        public Input(string value)
        {
            Value = value;
        }
    }
    
    public class Area : VoidElement
    {
        protected override string _Name { get; set; } = "area";
        // Defines a "Default" shape Area. Entire area is clickable.
        public Area(HyperTextReference href) : base(new DefaultShape(), href) { }
        public Area(Rectange rect, RectangeCoordinates coordinates, 
                HyperTextReference href) : base(rect,coordinates,href) { }
        public Area(Circle circle, CircleCoordinates coordinates, 
                HyperTextReference href) : base(circle,coordinates,href) { }
        public Area(Polygon poly, PolygonCoordinates coordinates, 
                HyperTextReference href) : base(poly,coordinates,href) { }
    }

    public class Base : VoidElement
    {
        protected override string _Name { get; set; } = "base";
        public Base(GlobalAttribute[] globalAttributes, HyperTextReference href, Target target) 
            : base(globalAttributes, href, target) { }
    }

    public class Break : VoidElement
    {
        protected override string _Name { get; set; } = "br";
        public Break(params GlobalAttribute[] globalAttributes) : base(globalAttributes) { }
    }

    public class Link : VoidElement
    {
        protected override string _Name { get; set; } = "link";
        // TODO: include all link specific attributes
        public Link(params Attribute[] attributes) : base(attributes) { }
    }
}