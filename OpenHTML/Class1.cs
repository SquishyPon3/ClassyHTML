using System.Security.Principal;

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

    public class Body : Tag
    {
        protected override string _Name { get; set; } = "Body";
        public Body(params Element[] elements) : base(elements) { }
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

    public class Button : Tag
    {
        protected override string _Name { get; set; } = "button";
        public Button(params Element[] elements) : base(elements) { }
    }

    public class Disabled : Attribute
    {
        protected override string _Name { get; set; } = "disabled";
        protected override string _Value { get; set; } = "disabled";
        public Disabled() { }
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