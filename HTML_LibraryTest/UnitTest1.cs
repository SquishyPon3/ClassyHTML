using ClassyHTML;
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
            P1 p1 = new P1();
            Text text = new Text("Hello World");

            root.Append(body);
            body.Append(p1);
            p1.Append(text);

            HTML root2 = new HTML(new Body(new P1(new Text("Hello World"))));

            string rootSerial = Serializer.Serialize(root);
            string rootSerial2 = Serializer.Serialize(root2);

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

            string rootSerial = Serializer.Serialize(root);

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
            Body body = new Body();
            root.Append(body);

            Table table = new Table(
                new TableRow
                (
                    new TableData(new Text("1")), 
                    new TableData(new Text("1")), 
                    new TableData(new Text("1"))
                ),
                new TableRow
                (
                    new TableData(new Text("2")),
                    new TableData(new Text("2")),
                    new TableData(new Text("2"))
                ),
                new TableRow
                (
                    new TableData(new Text("3")),
                    new TableData(new Text("3")),
                    new TableData(new Text("3"))
                )
            );

            root.Append(table);

            string rootSerial = Serializer.Serialize(root);

            using (FileStream fs = new FileStream(
                $"{TestOutputDir}\\Tables.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(rootSerial);
                }
            }
        }
    }
}