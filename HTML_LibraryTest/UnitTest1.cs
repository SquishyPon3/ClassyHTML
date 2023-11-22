using ClassyHTML;
using System.Text;

namespace HTML_LibraryTest
{
    [TestClass]
    public class UnitTest1
    {
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
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = 
                Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;

            using (FileStream fs = new FileStream(projectDirectory 
                + "\\TestResults\\Test.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.Write(rootSerial);
                }
            }
        }
    }
}