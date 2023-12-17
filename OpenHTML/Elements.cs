using System.ComponentModel;
using ClassyHTML.Tags;
using ClassyHTML.Attributes;
using ClassyHTML.VoidElements;
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
    public abstract class Element : ICloneable, IEnumerable<Element>, IEnumerable
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
            {typeof(TableColumn), "col"},
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
            Append(elements);
        }

        public void Append(params Element[]? elements)
        {
            if (elements is null)
            {
                throw new ArgumentNullException("Cannot add Null element array.");
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

        public void Append(Element element)
        {
            Append(new Element[] {element});
        }
        
        public Element AppendChild(Element element)
        {
            Append(new Element[] {element});
            return element;
        }

        public virtual object Clone()
        {
            // temporarily virtual, likely abstract in future...
            throw new NotImplementedException();
        }

        public bool Remove(Element element)
        {
            if (element == null)
                return false;

            // A little unfortunate and probably slow
            List<Element> elements = _Children.ToList();
            if (!elements.Remove(element))
            {
                return false;
            }

            _Children = elements.ToArray();
            return true;
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
    }

    public abstract class Tag : Element
    {
        protected override string _Name { get; set; } = "HTML";
        public Tag(params Element[] elements) : base(elements) { }
    }

    public abstract class Attribute : Element
    {
        protected virtual string? _Value { get; set; } = "default_value";
        public string? Value { get { return _Value; } }
        // Attributes should not have child elements...
        public Attribute() : base() { }
    }

    
    public abstract class GlobalAttribute : Attribute
    {
        public GlobalAttribute() : base() { }
    }

    // Void elements are tags which do not have any child nodes.
    // we hold onto the child list solely for attributes.
    public abstract class VoidElement : Element
    {
        public VoidElement(params Attribute[] attributes) : base(attributes) { }
        public VoidElement(GlobalAttribute[] globalAttributes, params Attribute[] attributes) 
            : base ( globalAttributes.Concat(attributes).ToArray() ) { }
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
                    attributes += $" {attribute.Name}";
                    // Some attributes don't need a value, if val is null
                    // it does not contain any value info, so we drop the =""
                        if (attribute.Value != null)
                            attributes += $"=\"{attribute.Value}\"";
                    continue;
                }
                if (type.IsSubclassOf(typeof(VoidElement)))
                {
                    if (type == typeof(Comment))
                    {
                        elements += $"\n{Serialize((Comment)element, depth++)}";
                        continue;
                    }

                    VoidElement voidElement = (VoidElement)element;
                    elements += $"\n{Serialize(voidElement, depth + 1)}";
                    continue;
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
            // TODO: Elements after a Comment seem to become 
            // children of the comment or something to that effect? Weird...

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

        public static string Serialize(Comment comment, int depth = 0)
        {
            string tabs = "";
            for (int i = 0; i < depth; i++) { tabs += tab; }
            return $"{tabs}<!-- {comment.Name} -->";
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