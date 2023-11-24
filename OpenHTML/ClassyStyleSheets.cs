using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ClassyHTML;

namespace ClassyStyleSheets
{
    public class StyleSheet
    {
        public string Selector { get; }
        public Property[] Properties { get; }

        // Applies to all HTML elements of specified Element type
        public StyleSheet(Type htmlElement, Property[] properties)
        {
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

        RgbColor(Color color)
        {
            _color[0] = color.R;
            _color[1] = color.G;
            _color[2] = color.B;
        }

        RgbColor(int r, int g, int b)
        {
            _color[0] = r;
            _color[1] = g;
            _color[2] = b;
        }
    }

    public class Property
    {
        protected virtual string _Name { get; set; } = "property-name";
        public string Name { get { return _Name; } }
        protected virtual string _Value { get; set; } = "value";
        public string Value { get { return _Value; } }
    }

    // Might merge color properties into parent child classes?
    public class AccentColor : Property
    {
        protected override string _Name { get; set; } = "accent-color";
        protected override string _Value { get; set; } = "red";

        public AccentColor(Color value)
        {
            _Value = value.ToKnownColor().ToString().ToLower();
        }

        // Might just want to make this r,g,b individual values
        public AccentColor(RgbColor value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            _Value = $"rgb({value.R}, {value.G}, {value.B})";
        }
    }

    public class BackgroundColor : Property
    {
        protected override string _Name { get; set; } = "background-color";
        protected override string _Value { get; set; } = "red";

        public BackgroundColor(Color value)
        {
            _Value = value.ToKnownColor().ToString().ToLower();
        }

        // Might just want to make this r,g,b individual values
        public BackgroundColor(RgbColor value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            _Value = $"rgb({value.R}, {value.G}, {value.B})";
        }
    }

    public class Serializer
    {
        public static string Serialize(StyleSheet styleSheet)
        {
            string props = "";
            foreach (Property property in styleSheet.Properties)
            {
                props += "    ";
                props += $"{property.Name}: {property.Value};\n";
            }
            return $"{styleSheet.Selector} {{\n{props}}}";
        }
    }
}
