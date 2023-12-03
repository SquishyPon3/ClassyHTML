using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using ClassyHTML;
using System.Collections;

// TODO: Move all child list info to the "Tag" type. 
//  (Attribute also should not contain children)
// TODO: Fix Append function not behaving as expected. 
//  It should return a new  collection & not modify original collection.
// TODO: Create "Void/Empty Tag" Type for tags which do 
//  not contain child elements.

// Follow HTML Spec Guidlines at:
// https://html.spec.whatwg.org/multipage/ 

namespace ClassyHTML
{
    public abstract class Element : IEnumerable<Element>, IEnumerable
    {
        // Some of the Element names used do not match their
        // HTML element shorthand names. This dict allows translation
        // to shorthand names for parsing.
        public static readonly Dictionary<Type, string> HTML_TypeName 
            = new Dictionary<Type, string>() 
        {
            // Tables
            {typeof(TableData), "td"},
            {typeof(TableRow), "tr"},
            {typeof(Column), "col"},
            {typeof(ColumnGroup), "colgroup"},

            // Headings
            {typeof(Heading1), "h1"},
            {typeof(Heading2), "h2"},
            {typeof(Heading3), "h3"},
            {typeof(Heading4), "h4"},
            {typeof(Heading5), "h5"},
            {typeof(Heading6), "h6"}
        };

        protected virtual string _Name { get; set; } = "DEFAULT_ELEMENT";
        public string Name { get { return _Name; } }
        private Element[] _Children = Array.Empty<Element>();
        public Element[] Children { get { return _Children; } set { _Children = value; } }

        public Element() {}
        public Element(params Element[]? elements)
        {
            Add(elements);
        }

        // Might have been overcomplicating this, append is a part of
        // IEnumerable (but also original non-IEnumerable implementation)
        // Should look into simplifying and following Append & Add func 
        // standards for IEnumerables. Append does not modify original collection.
        // and returns new collection. Add modifies original collection and returns void.
        // https://stackoverflow.com/questions/52355682/difference-between-a-lists-add-and-append-method
        public void Add(params Element[]? elements)
        {
            if (elements is null)
            {
                throw new WarningException("Cannot add empty element array.");
            }                

            elements = elements.Where(element => element != null).ToArray();

            Element[] children = new Element[_Children.Length + elements.Length];

            for (int i = 0; i < _Children.Length; i++)
            {
                children[i] = _Children[i];
            }
            for (int i = 0; i < elements.Length; i++)
            {
                children[i + _Children.Length] = elements[i];
            }

            _Children = children;
        }

        public void Add(Element element)
        {
            Add(new Element[] {element});
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // uses the strongly typed IEnumerable<T> implementation
            return this.GetEnumerator();
        }

        public IEnumerator<Element> GetEnumerator()
        {
            foreach (Element element in this._Children)
            {
                yield return element;
            }
        }

        // TODO: Currently a virtual, should be an abstract to
        // enforce implementations. In Testing atm.
        public virtual Element Append(params Element[]? elements) 
        {
            throw new NotImplementedException(); 
        }
        // {
        //     if (elements is null)
        //     {
        //         throw new WarningException("Cannot append empty element array.");
        //     }                

        //     elements = elements.Where(element => element != null).ToArray();

        //     Element[] children = new Element[_Children.Length + elements.Length];

        //     for (int i = 0; i < _Children.Length; i++)
        //     {
        //         children[i] = _Children[i];
        //     }
        //     for (int i = 0; i < elements.Length; i++)
        //     {
        //         children[i + _Children.Length] = elements[i];
        //     }

        //     _Children = children;

        //     return this;
        // }
    }

    /// <summary>
    /// Used as an additional identifer for CSS & code behind.
    /// Must be unique for each HTML element applied to.
    /// </summary>
    public class HTML_ID
    {
        public string Value;

        public HTML_ID(string name)
        {
            Value = $"#{name}";
        }
    }

    public abstract class Tag : Element
    {
        protected override string _Name { get; set; } = "HTML";
        public Tag(params Element[] elements) : base(elements) { }
    }

    // Void elements are tags which do not have any child nodes.
    // we hold onto the child list solely for attributes.
    public abstract class VoidElement : Element
    {
        public VoidElement(params Attribute[] attributes) : base(attributes) { }
    }

    public abstract class Attribute : Element
    {
        protected virtual string _Value { get; set; } = "default_value";
        public string Value { get { return _Value; } }
        // Attributes should not have child elements...
        public Attribute(params Element[] elements) : base(elements) { }
    }

    public class HTML : Tag
    {
        protected override string _Name { get; set; } = "HTML";
        public HTML(params Element[] elements) : base(elements) { }
        public override HTML Append(params Element[]? elements)
        {
            return new HTML() { Children, elements };
        }
    }

    public class Head : Tag
    {
        protected override string _Name { get; set; } = "head";
        public Head(params Element[] elements) : base(elements) { }
    }

    public class Body : Tag
    {
        protected override string _Name { get; set; } = "body";
        public Body(params Element[] elements) : base(elements) { }
        public override Body Append(params Element[]? elements)
        {
            return new Body() { Children, elements };
        }
    }

    public class Link : Tag
    {
        protected override string _Name { get; set; } = "link";
        public Link(params Attribute[] attributes) : base(attributes) {  }
    }

    public class Paragraph : Tag
    {
        protected override string _Name { get; set; } = "p";
        public Paragraph(params Element[] elements) : base(elements) { }
    }

    public class Heading1 : Tag 
    {
        protected override string _Name { get; set; } = "h1";
        public Heading1(params Element[] elements) : base(elements) { }
    }
    public class Heading2 : Tag 
    {
        protected override string _Name { get; set; } = "h2";
        public Heading2(params Element[] elements) : base(elements) { }
    }
    public class Heading3 : Tag 
    {
        protected override string _Name { get; set; } = "h3";
        public Heading3(params Element[] elements) : base(elements) { }
    }
    public class Heading4 : Tag 
    {
        protected override string _Name { get; set; } = "h4";
        public Heading4(params Element[] elements) : base(elements) { }
    }
    public class Heading5 : Tag 
    {
        protected override string _Name { get; set; } = "h5";
        public Heading5(params Element[] elements) : base(elements) { }
    }
    public class Heading6 : Tag 
    {
        protected override string _Name { get; set; } = "h6";
        public Heading6(params Element[] elements) : base(elements) { }
    }

    // Content
    public class Text : Tag
    {
        protected override string _Name { get; set; } = "";
        string _Value = "";
        public string Value { get { return _Value; } set { _Value = value; } }
        public Text(string value, params Element[] elements) : base(elements) 
        {
            _Value = value;
        }
    }

    // Images
    public class Image : Tag
    {
        protected override string _Name { get; set; } = "img";
        [Required]
        public Source Source;
        public AltText? AltText;
        public Width? Width;
        public Height? Height;
        public UseMap? UseMap;
        
        // Allows for { field } style construction. 
        public Image() : base() { Add(Source,AltText,Width,Height,UseMap); }

        // Realized I can enforce specific child types through the 
        // constructor like this, need better way to handle the null
        // refs though, currently handling in the append func but the
        // compiler is unhappy.
        public Image(Source source, AltText? altText = null,
            Width? width = null, Height? height = null, UseMap? useMap = null) 
                : base(source, altText, width, height, useMap) { }
    }

    public class Map : Tag
    {
        protected override string _Name { get; set; } = "map";
        public Name mapName;
        public Map(string name, params Area[] areas) : base()
        {
            mapName = new Name(name);
            Add(mapName);
            Add(areas);
        }
        public Map(Name name, params Area[] areas) : base()
        { 
            mapName = name;
            Add(mapName);
            Add(areas);
        }
    }

    public class Area : Tag
    {
        protected override string _Name { get; set; } = "area";
        // Defines a "Default" shape Area. Entire area is clickable.
        public Area(HyperTextReference href) : base(new DefaultShape(), href) { }
        public Area(Rectange rect, RectangeCoordinates coordinates, HyperTextReference href) 
            : base(rect,coordinates,href) { }
        public Area(Circle circle, CircleCoordinates coordinates, HyperTextReference href) 
            : base(circle,coordinates,href) { }
        public Area(Polygon poly, PolygonCoordinates coordinates, HyperTextReference href) 
            : base(poly,coordinates,href) { }
    }

    public class Picture : Tag
    {
        protected override string _Name { get; set; } = "picture";
        public Picture() : base() { throw new NotImplementedException(); }
    }

    // Tables
    public class Table : Tag
    {
        protected override string _Name { get; set;  } = "table";
        public Table(params Element[] elements) : base(elements) { }
    }

    public class Caption : Tag
    {
        protected override string _Name { get; set; } = "caption";
        public Caption(params Element[] elements) : base(elements) { }
    }

    public class Column : Tag
    {
        protected override string _Name { get; set; } = "col";
        public Column(params Element[] elements) : base(elements) { }
    }

    public class ColumnGroup : Tag
    {
        protected override string _Name { get; set; } = "colgroup";
        public ColumnGroup(params Element[] elements) : base(elements) { }
    }
    public class TableBody : Tag
    {
        protected override string _Name { get; set; } = "tbody";
        public TableBody(params Element[] elements) : base(elements) { }
    }

    public class TableData : Tag
    {
        protected override string _Name { get; set; } = "td";
        public TableData(params Element[] elements) : base(elements) { }
    }

    public class TableFoot : Tag
    {
        protected override string _Name { get; set; } = "tfoot";
        public TableFoot(params Element[] elements) : base(elements) { }
    }

    public class TableHeader : Tag
    {
        protected override string _Name { get; set; } = "th";
        public TableHeader(params Element[] elements) : base(elements) { }
    }

    public class TableRowHeader : Tag
    {
        protected override string _Name { get; set; } = "thead";
        public TableRowHeader(params Element[] elements) : base(elements) { }
    }

    public class TableRow: Tag
    {
        protected override string _Name { get; set; } = "tr";
        public TableRow(params Element[] elements) : base(elements) { }
    }

    // Forms
    public class Button : Tag
    {
        protected override string _Name { get; set; } = "button";
        public Button(params Element[] elements) : base(elements) { }
    }

    // Attributes
    public class Name : Attribute
    {
        protected override string _Name { get; set; } = "name";
        public Name(string name) { _Value = name; }
    }

    public class Disabled : Attribute
    {
        protected override string _Name { get; set; } = "disabled";
        protected override string _Value { get; set; } = "disabled";
        public Disabled() { }
    }

    public class Relationship : Attribute
    {
        public enum RelType 
        { 
            alternate, author, bookmark, 
            external, help, license, next,
            nofollow, noopener, noreferrer,
            prev, search, tag, stylesheet
        }
        protected override string _Name { get; set; } = "rel";
        protected override string _Value { get; set; } = "";
        public Relationship(RelType relationship) 
        { 
            string? name = Enum.GetName(relationship);
            if (name == null)
                throw new Exception("RelType parameter relationship did" + 
                    "not provide a proper RelType Enum value.");
            _Value = name;
        }
    }

    public class InternetMediaType : Attribute
    {
        public enum Application 
        { 
            generic, pdf, zip, 
            pkcs8, octet_stream 
        }
        public enum Audio { generic }
        public enum Example { generic }
        public enum Font { generic }
        public enum Image { generic, apng, avif, gif, jpeg, png, svgxml }
        public enum Model { generic }

        public enum Text { generic, plain, css, html, javascript }
        public enum Video { generic }

        public InternetMediaType(Application applicationSubType)
        {
            // html likes to use - character in names, need to
            // specifically check for these specific terms.
            if (applicationSubType == Application.generic)
                _Name = "application";
            if (applicationSubType == Application.octet_stream)
                _Name = $"application/octet-stream";
            else 
                _Name = $"application/{applicationSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Audio audioSubType)
        {
            if (audioSubType == Audio.generic)
                _Name = "audio";            
            else 
                _Name = $"audio/{audioSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Example exampleSubType)
        {
            if (exampleSubType == Example.generic)
                _Name = "example";            
            else 
                _Name = $"example/{exampleSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Font fontSubType)
        {
            if (fontSubType == Font.generic)
                _Name = "font";            
            else 
                _Name = $"font/{fontSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Image imageSubType)
        {
            if (imageSubType == Image.generic)
                _Name = "image";            
            else 
                _Name = $"image/{imageSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Model modelSubType)
        {
            if (modelSubType == Model.generic)
                _Name = "model";            
            else 
                _Name = $"model/{modelSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Text textSubType)
        {
            if (textSubType == Text.generic)
                _Name = "text";            
            else 
                _Name = $"text/{textSubType.ToString().ToLower()}";
        }
        public InternetMediaType(Video videoSubType)
        {
            if (videoSubType == Video.generic)
                _Name = "video";            
            else 
                _Name = $"video/{videoSubType.ToString().ToLower()}";
        }
    }

    public class HyperTextReference : Attribute
    {
        protected override string _Name { get; set; } = "href";
        public HyperTextReference(string destination)
        {
            _Value = destination;
        }

        // I want to make this work, to reference a stylesheet document directly
        // and have it reference the file path on compile, need to 
        // implement IDisposable 
        // public HyperTextReference(ClassyStyleSheets.StyleSheet styleSheet) { }
    }

    public class Source : Attribute
    {
        protected override string _Name { get; set; } = "src";
        public Source(string path)
        {
            _Value = path;
        }
    }

    public class Width : Attribute
    {
        protected override string _Name { get; set; } = "width";
        public Width(int pixels)
        {
            _Value = pixels.ToString();
        }
    }

    public class Height : Attribute
    {
        protected override string _Name { get; set; } = "height";
        public Height(int pixels)
        {
            _Value = pixels.ToString();
        }
    }

    // Alt text is used to define descriptions for users 
    // who browse with a screen reader.
    public class AltText : Attribute
    {
        protected override string _Name { get; set; } = "alt";
        public AltText(string description)
        {
            _Value = description;
        }
    }

    public class UseMap : Attribute
    {
        protected override string _Name { get; set; } = "usemap";
        public UseMap(string mapName)
        {
            _Value = $"#{mapName}";
        }
    } 

    // Various Shape types
    public abstract class Shape : Attribute { public Shape() { _Name = "shape"; } }
    public class DefaultShape : Shape
    {
        public DefaultShape(): base() { _Value = "default"; }
    }
    public class Rectange : Shape 
    {
        public Rectange(): base() { _Value = "rect"; }
    } 
    public class Circle : Shape 
    {
        public Circle(): base() { _Value = "circle"; }
    } 
    public class Polygon : Shape 
    {
        public Polygon(): base() { _Value = "poly"; }
    } 

    public abstract class Coordinates : Attribute 
    { 
        public Coordinates(params int[] components)
        {            
            _Name = "coords";
            _Value = "";
            foreach (int coordinate in components)
            {
                _Value += $"{coordinate},";
            }
        }
        public Coordinates(params Vector2Int[] coordinates)
        {
            _Name = "coords";
            _Value = "";
            foreach (Vector2Int position in coordinates)
            {
                _Value += $"{position.X},{position.Y},";
            }
        }
    }

    public class RectangeCoordinates : Coordinates
    {
        public RectangeCoordinates(Vector2Int topLeft, Vector2Int bottomRight) 
            : base(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y) { }
    }
    public class CircleCoordinates : Coordinates
    {
        public CircleCoordinates(int X, int Y, int radius) 
            : base(X, Y, radius) { }
    }
    public class PolygonCoordinates : Coordinates
    {
        public PolygonCoordinates(params Vector2Int[] coordinates)
            : base(coordinates) { }
    }

    // CSS
    public class Style : Attribute
    {
        protected override string _Name { get; set; } = "style";
        ClassyStyleSheets.Property[] _properties;
        public ClassyStyleSheets.Property[] properties 
        { 
            get { return _properties; } 
            set 
            { 
                _properties = value; 
                _Value = ClassyStyleSheets.Serializer.Serialize(value);
                // TODO: Make this a for loop that checks for these,
                // just way more efficient than making mult arrays...
                // Remove spaces and new line character
                _Value = _Value.Replace(" ", string.Empty);
                _Value = _Value.Replace("\n", string.Empty);
            } 
        }
        public Style(params ClassyStyleSheets.Property[] props)
        {
            properties = props;
        }
    }

    public class Serializer
    {
        static string tab = "    ";

        public static string Serialize(Tag rootElement, int depth = 0) 
        {
            string attributes = "";
            string head = rootElement.Name;      
            string end = rootElement.Name;
            string elements = "";
            string tabs = "";

            for (int i = 0; i < depth; i++) { tabs += tab; }

            foreach (Element element in rootElement.Children)
            {
                if (element is null)
                    continue;

                Type type = element.GetType();
                if (type.IsSubclassOf(typeof(Attribute)))
                {
                    Attribute attribute = (Attribute)element;
                    attributes += $" {attribute.Name}=\"{attribute.Value}\"";
                    continue;
                }
                if (type.IsSubclassOf(typeof(VoidElement)))
                {
                    VoidElement voidElement = (VoidElement)element;
                    elements += $"\n{Serialize(voidElement, depth + 1)}";
                }
                if (type == typeof(Text))
                {
                    Text text = (Text)element;
                    elements += $"\n{tabs}{tab}{text.Value}";
                    continue;
                }

                // Attributes & "Void Elements" cannot contain child tags
                // and thus will not be serialized. Need to add specific
                // differences in tag / void tag / attribute
                elements += $"\n{Serialize((Tag)element, depth + 1)}";
                //for (int i = 0; i < depth + 1; i++) { elements += tab; }
            }

            string output = $"{tabs}<{head}{attributes}>{elements}\n{tabs}</{end}>";

            return output;
        }

        public static string Serialize(VoidElement rootElement, int depth = 0)
        {
            string attributes = "";
            string tabs = "";

            for (int i = 0; i < depth; i++) { tabs += tab; }

            // Void Elements should only ever contain attributes.
            foreach (Attribute attribute in rootElement.Children)
            {
                if (attribute is null)
                    continue;

                attributes += $" {attribute.Name}=\"{attribute.Value}\"";
                continue;
            }
            
            string output = $"{tabs}<{rootElement.Name}{attributes}/>";

            return output;
        }
    }

    // Abstracts out x/y coordinates for use in some
    // elements which take specific pixel positions.
    public class Vector2Int
    {
        public int X, Y;
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // Not implemented
    // public class HTML_Document : IDisposable
    // {
    //     public static HTML_Document Create(string path)
    //     {
    //         return new HTML_Document();
    //     }

    //     public void Dispose()
    //     {
    //         // Serializer.Serialize();
    //         throw new NotImplementedException();
    //     }
    // }
}