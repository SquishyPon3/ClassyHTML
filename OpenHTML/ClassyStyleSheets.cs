using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClassyHTML;

namespace ClassyStyleSheets
{
    public class StyleSheet
    {
        // Turns Element Type names into their HTML written names.  


        public string Selector { get; }
        public Property[] Properties { get; }

        // Applies to all HTML elements of specified Element type
        public StyleSheet(Type htmlElement, Property[] properties)
        {
            if (Element.HTML_TypeName.ContainsKey(htmlElement))
                Selector = Element.HTML_TypeName[htmlElement];
            else
                Selector = htmlElement.Name.ToLower();
            Properties = properties;
        }

        // Applies to HTML element with specified ID
        public StyleSheet(HTML_ID id, Property[] properties)
        {
            Selector = id.Value;
            Properties = properties;
        }

        // Applies to HTML with specific class applied
        public StyleSheet(string className, Property[] properties)
        {
            Selector = $".{className}";
            Properties = properties;
        }

        // Applies to specific elements with specific HTML class
        public StyleSheet(string className, Type htmlElement, Property[] properties)
        {
            Selector = $"{htmlElement.Name.ToLower()}.{className}";
            Properties = properties;
        }

        // Applies to every HTML element
        public StyleSheet(Property[] properties)
        {
            Selector = "*";
            Properties = properties;
        }
    }

    public class RgbColor
    {
        // Used as optional replacer for C# color 
        // Might just get rid of it, but I like that it
        // sets limits from 0 - 255.
        int[] _color = new int[3] { 255, 255, 255 };
        int[] color
        {
            get { return _color; }
            set
            {
                _color = new int[3]
                {
                        Math.Clamp((int)value.GetValue(0), 0, 1),
                        Math.Clamp((int)value.GetValue(1), 0, 1),
                        Math.Clamp((int)value.GetValue(2), 0, 1)
                };
            }
        }
        public int R
        {
            get { return _color[0]; }
            set { _color[0] = Math.Clamp(value, 0, 255); }
        }
        public int G
        {
            get { return _color[1]; }
            set { _color[1] = Math.Clamp(value, 0, 255); }
        }
        public int B
        {
            get { return _color[2]; }
            set { _color[2] = Math.Clamp(value, 0, 255); }
        }

        public RgbColor(Color color)
        {
            _color[0] = color.R;
            _color[1] = color.G;
            _color[2] = color.B;
        }

        public RgbColor(int r, int g, int b)
        {
            _color[0] = r;
            _color[1] = g;
            _color[2] = b;
        }
    }

    public abstract class Property
    {
        protected virtual string _Name { get; set; } = "property-name";
        public string Name { get { return _Name; } }
        protected virtual string _Value { get; set; } = "value";
        public string Value { get { return _Value; } }
    }

    public class ColorProperty : Property
    {
        protected override string _Name { get; set; } = "color";
        protected override string _Value { get; set; } = "red";

        public ColorProperty(Color value)
        {
            _Value = value.ToKnownColor().ToString().ToLower();
        }

        // Might just want to make this r,g,b individual values
        public ColorProperty(RgbColor value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            _Value = $"rgb({value.R}, {value.G}, {value.B})";
        }
    }

    public class AccentColor : ColorProperty
    {
        protected override string _Name { get; set; } = "accent-color";
        public AccentColor(Color value) : base(value) { }
        public AccentColor(RgbColor value) : base(value) { }
    }

    public class BackgroundColor : ColorProperty
    {
        protected override string _Name { get; set; } = "background-color";
        public BackgroundColor(Color value) : base(value) { }
        public BackgroundColor(RgbColor value) : base(value) { }
    }

    public class BorderWidth : Property
    {
        protected override string _Name { get; set; } = "border-width";
        protected override string _Value { get; set; } = "";
        public BorderWidth(int pixelWidth)
        {
            _Value = $"{pixelWidth}px";
        }
    }

    public class BorderStyle : Property
    {
        protected override string _Name { get; set; } = "border-style";
        protected override string _Value { get; set; } = "";
        public enum Style { 
            dotted, dashed, solid, doubleStyle, groove, 
            ridge, inset, outset, none, hidden
        }
        
        public BorderStyle(params Style[] borderStyles)
        {
            foreach (Style style in borderStyles)
            {
                if (style == Style.doubleStyle)
                {
                    _Value += "double";
                    continue;
                }
                    
                _Value += style.ToString();
            }
        }
    }

    public class BorderColor : ColorProperty
    {
        protected override string _Name { get; set; } = "border-color";
        public BorderColor(Color value) : base(value) { }
        public BorderColor(RgbColor value) : base(value) { }
    }

    public class BorderCollapse : Property
    {
        protected override string _Name { get; set; } = "border-collapse";
        public enum CollapseValue { collapse, separate }
        public BorderCollapse(CollapseValue collapseValue) : base() 
        {
            _Value = collapseValue.ToString(); 
        }
    }

    public class Serializer
    {
        public static string Serialize(StyleSheet styleSheet)
        {            
            string props = Serialize(styleSheet.Properties);
            return $"{styleSheet.Selector} {{\n{props}}}";
        }

        // Serializes array of properties. Useful for the css serialization
        // process and for adding to HTML Style attributes.
        public static string Serialize(params Property[] properties)
        {
            string props = "";
            foreach (Property property in properties)
            {
                props += "    ";
                props += $"{property.Name}: {property.Value};\n";
            }
            return props; 
        }
    }

    // Not implemented
    // public class StyleSheetDocument : IDisposable
    // {
    //     public static StyleSheetDocument Create(string path)
    //     {
    //         return new StyleSheetDocument();
    //     }

    //     public void Dispose()
    //     {
    //         // Serializer.Serialize();
    //         throw new NotImplementedException();
    //     }
    // }
}
