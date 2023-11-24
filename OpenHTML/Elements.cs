using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using ClassyHTML;

namespace ClassyHTML
{
    public class Element
    {
        protected virtual string _Name { get; set; } = "DEFAULT_ELEMENT";
        public string Name { get { return _Name; } }
        private Element[] _Children = { };
        public Element[] Children { get { return _Children; } }

        public Element(params Element[] elements)
        {
            Append(elements);
        }

        public Element[] Append(params Element[] elements)
        {
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

            return _Children;
        }
    }

    /// <summary>
    /// Used as an additional identifer for CSS & code behind.
    /// Must be unique for each HTML element applied to.
    /// </summary>
    public class HTML_ID
    {
        public string Value;

        HTML_ID(string name)
        {
            Value = $"#{name}";
        }
    }

    public class Tag : Element
    {
        protected override string _Name { get; set; } = "HTML";
        public Tag(params Element[] elements) : base(elements) { }
    }

    public class Attribute : Element
    {
        protected virtual string _Value { get; set; } = "value";
        public string Value { get { return _Value; } }
        public Attribute(params Element[] elements) : base(elements) { }
    }

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

    public class Link : Tag
    {
        protected override string _Name { get; set; } = "link";
        public Link(params Attribute[] attributes) : base(attributes) {  }
    }

    public class P1 : Tag
    {
        protected override string _Name { get; set; } = "p1";
        public P1(params Element[] elements) : base(elements) { }
    }

    public class P2 : Tag
    {
        protected override string _Name { get; set; } = "p2";
        public P2(params Element[] elements) : base(elements) { }
    }

    public class P3 : Tag
    {
        protected override string _Name { get; set; } = "p3";
        public P3(params Element[] elements) : base(elements) { }
    }

    public class Text : Element
    {
        protected override string _Name { get; set; } = "";
        string _Value = "";
        public string Value { get { return _Value; } set { _Value = value; } }
        public Text(string value, params Element[] elements) : base(elements) 
        {
            _Value = value;
        }
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
    public class Disabled : Attribute
    {
        protected override string _Name { get; set; } = "disabled";
        protected override string _Value { get; set; } = "disabled";
        public Disabled() { }
    }

    public class Relationship : Attribute
    {
        public enum RelationshipValues 
        { 
            alternate, author, bookmark, 
            external, help, license, next,
            nofollow, noopener, noreferrer,
            prev, search, tag, stylesheet
        }
        protected override string _Name { get; set; } = "rel";
        protected override string _Value { get; set; } = "";
        public Relationship(RelationshipValues relationship) 
        { 
            _Value = Enum.GetName(relationship);
        }
    }

    public class InternetMediaType : Attribute
    {
        // Needs a lot of work on full implementation.
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
            if (applicationSubType == Application.octet_stream)
                _Name = $"application/{applicationSubType.ToString().ToLower()}";
        }
    }

    public class HyperTextReference : Attribute
    {
        protected override string _Name { get; set; } = "href";
        public HyperTextReference(string destination)
        {
            _Value = destination;
        }
    }

    public class Serializer
    {
        public static string Serialize(Element rootElement, int depth = 0) 
        {
            string attributes = "";
            string head = rootElement.Name;      
            string end = rootElement.Name;
            string elements = "";
            string tab = "    ";
            string tabs = "";

            for (int i = 0; i < depth; i++) { tabs += tab; }

            foreach (Element element in rootElement.Children)
            {
                if (element is null)
                    continue;

                //for (int i = 0; i < depth + 1; i++) { elements += tab; }
                Type type = element.GetType();
                if (type.IsSubclassOf(typeof(Attribute)))
                {
                    Attribute attribute = (Attribute)element;
                    attributes += $" {attribute.Name}=\"{attribute.Value}\"";
                    continue;
                }
                if (type == typeof(Text))
                {
                    Text text = (Text)element;
                    elements += $"\n{tabs}{tab}{text.Value}";
                    continue;
                }
                elements += $"\n{Serialize(element, depth + 1)}";
                //for (int i = 0; i < depth + 1; i++) { elements += tab; }
            }

            string output = $"{tabs}<{head}{attributes}>{elements}\n{tabs}</{end}>";

            return output;
        }
    }
}