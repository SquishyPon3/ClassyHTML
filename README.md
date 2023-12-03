# ClassyHTML
### Description
ClassyHTML is a C# library with a class-based element tree implementation  
for writing to and reading from .HTML files inspired by OpenXML.

### Status
Early in development and constantly improving.  
The intention is to bring about incrememntal improvements rapidly.  
Currently contains early implementation for both HTML and CSS files.  
As well as some of the most common Tags/Attributes/Properties expected.   
**There is currently no support for deserialization of HTML / CSS files into objects.**

### Getting Started
ClassyHTML is integregated as a C# DLL and is tailored for C# development.  
Currently there are two main stages:
1. Element Tree Construction
2. Serialization
   
#### Element Tree Construction
##### HTML
HTML Files begin with a new HTML Tag object.  
Every tag can begin empty and have child elements added later.  
Standard practice is to create a new Tag, Void Tag, or Attribute Element  
and use the add function to add the specific element or list of elements  
to the end of the parent Tag's children defined in ***Elements.cs***  
Use this strategy to functionally build out an HTML Element tree hierarchy.  

##### CSS
CSS Files begin with a new StyleSheet object.  
StyleSheets are constructed with the inclusion of a **Selector.**  
* None (Applies to all HTML Elements)
* id (HTML_ID)
* Type ***WIP*** (ClassyHTML Element Types only)
* class name (HTML Element class name)

StyleSheets are then built out as a simple list of CSS Properties.  
These properties are all defined in ***ClassyStyleSheets.cs***

There is currently still a lot of of Elements and Properties that  
need to be added to match the spec. Though plenty for some experimentation!

#### Serialization
After you have built out your HTML and / or CSS trees you will  
then need to actually serialize the objects into their respective files.  
Both the ClassyHTML and ClassyCSS namespaces contain a Serializer static object.  
Use the Serializers Serialize() static function to read in the parent element / style sheet  
and return a formatted string containing every object's information perfectly ready  
to write into a file's stream. Open the file and observe your new simple web page!

## HTML
### Element
**Element** is an abstract base class every HTML element is built off of.  
An IEnumerable Element containing an array of it's own child Elements.  
Also contains a Dictionary translating ClassyHTML Element class names into their  
respective HTML Element names for future use in StyleSheets.

#### Tag
**Tag** is an abstract element class of which all HTML tags with content are defined.  
Tags contain an array of any possible child elements by default, though  
some specific tags have determined limitations in which child elements  
they can contain.
#### Void Tag
**Void Tag** is an abstract element class of which all Empty HTML tags are defined.  
They do not inherit from tag and so are not interchangeable and are instead an Element.  
Void tags contain an array of specifically attributes as empty HTML tags cannot  
contain any child tags.
#### Attribute
**Attribute** is an abstract element class of which all HTML attributes are defined.
Attributes cannot contain any children.
#### Serializer
The ClasstyHTML Serializer contains two overloads for it's Serialize function.  
1. Recieves a base **Tag**, usually an \<HTML\> tag, and iterates through it's children.  
If it detects another Tag with children it calls itself and continues down the tree.  
Otherwise it applies the element's information to the string output and formats  
according to it's depth in the tree.
2. Recieves a VoidElement and simply returns its  
serialized information. Usally called during the Tag serialization process  
as a child tag.

## CSS
#### StyleSheet
**StyleSheet** is the base class all Style Sheets are built from.
StyleSheets contain a Selector and an array of properties.

#### Property
**Property** is an abstract class of which all CSS properties are defined.  
Properties contain a Name and a Value string. These are effectively the  
pre-serialized forms of the information provided through their constructor.

#### Serializer
The ClassyCSS Serializer contains two overloads for it's Serialize function.
1. Recieves a StyleSheet parameter and serializes the sheet's Selector and Properties.
2. Recieves an array of properties, usually in the process of serializing a StyleSheet.
