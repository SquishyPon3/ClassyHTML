namespace ClassyHTML.Attributes
{
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

    // Input Attributes
    public class InputType : Attribute
        {
            public enum InputTypes 
            {
                button, checkbox, color, date, datetime_local, email,
                file, hidden, image, month, number, password, radio, range,
                reset, search, submit, tel, text, time, url, week
            }

            public InputType(InputTypes inputType): base() 
            {
                if (inputType == InputTypes.datetime_local)
                    _Value = "datetime-local";
                else
                    _Value = Enum.GetName(inputType); 
            }
        }

        public class AutoComplete : Attribute
        {
            public enum AutoCompleteValue 
            {
                on, off, address_line1, address_line2, address_line3,
                address_level1, address_level2, address_level3,
                address_level4, street_address, county, country_name,
                postal_code, name, additional_name, family_name, give_name,
                honoric_prefix, honoric_suffic, nickname, organization_title,
                username, new_password, current_password, bday, bday_day,
                bday_month, bday_year, sex, one_time_code, organization,
                cc_name, cc_given_name, cc_additional_name, cc_number,
                cc_exp, cc_exp_month, cc_exp_year, cc_csc, cc_type,
                transaction_currency, transaction_amount, language, url,
                email, photo, tel, tel_country_code, tel_national,
                tel_area_code, tel_local, tel_local_prefix, tel_local_suffix,
                tel_extension, impp
            }

            public AutoComplete(AutoCompleteValue value)
            {
                string[] sets = Enum.GetName(value).Split("_");
                for (int i = 0; i < sets.Length; i++)
                {
                    if (i != 0)
                        _Value += "-";

                    _Value += sets[i];
                }
            }

            // Might move this elsewhere, perhaps in the serializer class?
            // any name which contains a "_" needs to be replaced with a "-"
            static string valueToString(AutoCompleteValue value)
            {
                string output = "";
                string[] sets = Enum.GetName(value).Split("_");
                for (int i = 0; i < sets.Length; i++)
                {
                    if (i != 0)
                        output += "-";

                    output += sets[i];
                }
                return output;
            }
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
}