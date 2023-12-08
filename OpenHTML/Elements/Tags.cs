using System.ComponentModel.DataAnnotations;
using ClassyHTML.Attributes;
using ClassyHTML.VoidElements;

namespace ClassyHTML.Tags
{
    public class HTML : Tag
    {
        protected override string _Name { get; set; } = "HTML";
        public HTML(params Element[] elements) : base(elements) { }
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
    }

    public class Title : Tag
    {
         protected override string _Name { get; set; } = "title";
        public Title(params Element[] elements) : base(elements) { }
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
    // Text should probably be a VoidElement as it only has attributes.
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

    public class Anchor : Tag 
    {
        protected override string _Name { get; set; } = "a";
        // TODO: add target, download, ping, hreflang, type, referrerpolicy
        public Anchor(HyperTextReference href, 
                Relationship relationship, Text text) 
            : base(href, relationship, text) { }
    }

    public class Emphasis : Tag
    {
        protected override string _Name { get; set; } = "em";
        public Emphasis(params Element[] elements) : base(elements) { }
    }

    public class Bold : Tag
    {
        protected override string _Name { get; set; } = "b";
        public Bold(params Element[] elements) : base(elements) { }
    }

    public class Italics : Tag
    {
        protected override string _Name { get; set; } = "i";
        public Italics(params Element[] elements) : base(elements) { }
    }

    public class Small : Tag
    {
        protected override string _Name { get; set; } = "small";
        public Small(params Element[] elements) : base(elements) { }
    }

    public class Underline : Tag
    {
        protected override string _Name { get; set; } = "u";
        public Underline(params Element[] elements) : base(elements) { }
    }

    public class StrikeThrough : Tag
    {
        protected override string _Name { get; set; } = "strik";
        public StrikeThrough(params Element[] elements) : base(elements) { }
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
        
        // // Allows for { field } style construction. 
        public Image() : base() { }

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
            Append(mapName);
            Append(areas);
        }
        public Map(Name name, params Area[] areas) : base()
        { 
            mapName = name;
            Append(mapName);
            Append(areas);
        }
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

    // TODO: This should be a void element and only contain attributes.
    public class TableColumn : Tag
    {
        protected override string _Name { get; set; } = "col";
        public TableColumn(params Element[] elements) : base(elements) { }
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
}