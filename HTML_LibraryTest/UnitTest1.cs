using ClassyHTML;
using ClassyStyleSheets;
using System.Buffers;
using System.Drawing;
using System.Text;

namespace HTML_LibraryTest
{
    [TestClass]
    public class UnitTest1
    {
        static string WorkingDir = Environment.CurrentDirectory;
        static string ProjectDir =
            Directory.GetParent(WorkingDir).Parent.Parent.Parent.FullName;
        static string TestOutputDir = ProjectDir + "\\TestResults";

        [TestMethod]
        public void CompareElementConstructionMethods()
        {
            HTML root = new HTML();
            Body body = new Body();
            Paragraph p = new Paragraph();
            Text text = new Text("Hello World");

            root.Append(body);
            body.Append(p);
            p.Append(text);  

            HTML root2 = new HTML(new Body(new Paragraph(new Text("Hello World"))));

            string rootSerial = ClassyHTML.Serializer.Serialize(root);
            string rootSerial2 = ClassyHTML.Serializer.Serialize(root2);

            Assert.IsTrue(rootSerial == rootSerial2);            
        }

        [TestMethod] 
        public void GenerateExampleFile()
        {
            //string workingDirectory = Environment.CurrentDirectory;
            //string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            //string dir = $"{projectDirectory}\\TestResults\\Test.html";
            //using (FileStream fs = new FileStream(dir, FileMode.Create))
            //{
            //    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            //    {
            //        w.Write(rootSerial);
            //    }
            //}

            HTML root = new HTML();
            Body body = new Body();
            Button button = new Button(new Text("Enabled Button"));
            Button button2 = new Button(new Text("Disabled Button"), new Disabled());

            root.Append(body);
            body.Append(button);
            body.Append(button2);

            string rootSerial = ClassyHTML.Serializer.Serialize(root);

            using (FileStream fs = new FileStream(
                $"{TestOutputDir}\\Generic.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(rootSerial);
                }
            }
        }

        [TestMethod]
        public void GenerateTableFile()
        {
            HTML root = new HTML();
            Relationship rel = new Relationship(
                Relationship.RelType.stylesheet);
            HyperTextReference href = new HyperTextReference($"{TestOutputDir}\\Tables.css");
            Link link = new Link(rel, href);
            Head head = new Head(link);
            Body body = new Body();
            root.Append(head);
            root.Append(body);

            body.Append(new Heading1(new Text("Example Table")));

            Table table = new Table(
                new TableRow
                (
                    new TableData(new Text("ExampleData_1_1")), 
                    new TableData(new Text("ExampleData_1_2")), 
                    new TableData(new Text("ExampleData_1_3"))
                ),
                new TableRow
                (
                    new TableData(new Text("ExampleData_2_1")),
                    new TableData(new Text("ExampleData_2_2")),
                    new TableData(new Text("ExampleData_2_2"))
                ),
                new TableRow
                (
                    new TableData(new Text("ExampleData_3_1")),
                    new TableData(new Text("ExampleData_3_2")),
                    new TableData(new Text("ExampleData_3_3"))
                )
            );

            body.Append(table);

            string rootSerial = ClassyHTML.Serializer.Serialize(root);

            using (FileStream fs = new FileStream(
                $"{TestOutputDir}\\Tables.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(rootSerial);
                }
            }
        }

        [TestMethod]
        public void Test_CSS_Color()
        {
            string color = Color.Red.ToKnownColor().ToString().ToLower();
            Assert.IsTrue(color == "red");
        }

        [TestMethod]
        public void Generate_CSS()
        {
            string css;
            string cellcss;

            using (FileStream fs = new FileStream(
                $"{TestOutputDir}\\Tables.css", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    Property[] borderProps = new Property[] 
                    {
                        new BorderStyle(BorderStyle.Style.solid),
                        new BorderWidth(3),
                        new BorderColor(Color.Black),
                        new BorderCollapse(
                            BorderCollapse.CollapseValue.collapse)
                    };
                    Property[] props = new Property[] { 
                        new BackgroundColor(Color.LightGray),
                        new BorderStyle(BorderStyle.Style.solid),
                        new BorderWidth(3),
                        new BorderColor(Color.Black),
                        new BorderCollapse(
                            BorderCollapse.CollapseValue.collapse)
                    };
                    StyleSheet styleSheet = new StyleSheet(
                        typeof(Table), props);
                    StyleSheet cellSheet = new StyleSheet(
                        typeof(TableData), borderProps);
                    css = ClassyStyleSheets.Serializer.Serialize(styleSheet);
                    cellcss = ClassyStyleSheets.Serializer.Serialize(cellSheet);
                    w.Write(css + "\n" + cellcss);
                }
            }
        }

        [TestMethod]
        public void HTML_DisplayEveryElement()
        {
            HTML root = new HTML();

            Head head = new Head();
            Body body = new Body();
            root.Append(head, body);

            Paragraph para = new Paragraph(
                new Style(
                    new BackgroundColor(Color.LightPink),
                    new ColorProperty(Color.White)
                ),
                new Text("Hello World"));            

            Heading1 h1 = new Heading1(new Text("Heading 1"));
            Heading2 h2 = new Heading2(new Text("Heading 2"));
            Heading3 h3 = new Heading3(new Text("Heading 3"));
            Heading4 h4 = new Heading4(new Text("Heading 4"));
            Heading5 h5 = new Heading5(new Text("Heading 5"));
            Heading6 h6 = new Heading6(new Text("Heading 6"));
            
            body.Append(para,h1,h2,h3,h4,h5,h6);

            Image image = new Image(new Source($"{ProjectDir}\\OpenHTML\\image.png"),
                new AltText("Test image!"), new Width(256), new Height(256));
            
            // Testing out image Map stuff
            Map map = new Map(
                "testClick",
                new Area(new HyperTextReference($"{TestOutputDir}\\Tables.html")));
            Map rectMap = new Map(
                "rectTestClick",
                new Area(
                    new Rectange(), 
                    new RectangeCoordinates(
                        new Vector2Int(10,10), new Vector2Int(200,200)),
                    new HyperTextReference($"{TestOutputDir}\\Tables.html")
                )
            );
            Image clickImage = new Image(new Source($"{ProjectDir}\\OpenHTML\\image.png"),
                new AltText("Test image!"), new Width(256), new Height(256), new UseMap(map.mapName.Value));
            Image rectimage = new Image(new Source($"{ProjectDir}\\OpenHTML\\image.png"),
                new AltText("Test image!"), new Width(256), new Height(256), new UseMap(rectMap.mapName.Value));

            body.Append(image);
            body.Append(clickImage, map);
            body.Append(rectimage, rectMap);
            //

            string rootSerial = ClassyHTML.Serializer.Serialize(root);

            using (FileStream fs = new FileStream(
                $"{TestOutputDir}\\ElementZoo.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(rootSerial);
                }
            }
        }
    }
}