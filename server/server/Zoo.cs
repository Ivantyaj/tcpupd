using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace server
{
    class Animal
    {
        private string name;
        private int count;

        public Animal(string name, int count)
        {
            this.name = name;
            this.count = count;
        }


        //public string Name => name; 
        public string Name
        {
            get { return name; }
        }

        public int Count
        {
            get { return count; }
        }

        public override string ToString()
        {
            return "Name: " + name + " Count: " + count + "\n";
        }
    }

    class Zoo
    {
        private List<Animal> animals;

        public Zoo()
        {
            animals = new List<Animal>();
        }

        public Zoo(string path)
        {
            readFromFile(path);
        }

        public void addAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        public int deleteAnimal(string name)
        {
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals.ElementAt(i).Name.Equals(name))
                {
                    animals.RemoveAt(i);
                    return i;
                }
            }
            return -1;
        }

        public String findByName(String name)
        {
            String animalsStrData;
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals.ElementAt(i).Name.Equals(name))
                {
                    animalsStrData = "Name: " + animals.ElementAt(i).Name + " Count: " + animals.ElementAt(i).Count;
                    return animalsStrData;
                }
            }
            animalsStrData = "Not found!";
            return animalsStrData;
        }

        public void showData()
        {
            Console.WriteLine("Vivod data");

            foreach (Animal animal in animals)
            {
                Console.WriteLine("Name: {0}", animal.Name);
                Console.WriteLine("Count: {0}", animal.Count);
            }
            Console.WriteLine();
        }

        public List<String> getData()
        {
            List<String> animalsStrData = new List<String>();
            foreach (Animal animal in animals)
            {
                animalsStrData.Add(animal.ToString());
            }
            return animalsStrData;
        }


        public void readFromFile(string path = "dataXml.xml")
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                string name = null;
                int count = -1;
                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "name")
                    {
                        name = childnode.InnerText;
                    }
                    if (childnode.Name == "count")
                    {
                        count = Int32.Parse(childnode.InnerText);

                    }
                    if (name != null && count != -1)
                    {
                        addAnimal(new Animal(name, count));

                        name = null;
                        count = -1;
                    }
                }
            }
        }

        public void writeToFile(string path = "dataXmlTo.xml")
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            XmlElement elementZoo = doc.CreateElement("zoo");
            doc.AppendChild(elementZoo);

            foreach (Animal animal in animals)
            {
                XmlElement elementAnimal = doc.CreateElement("animal");
                elementZoo.AppendChild(elementAnimal);

                XmlElement elementName = doc.CreateElement("name");
                XmlText textName = doc.CreateTextNode(animal.Name);
                elementName.AppendChild(textName);
                elementAnimal.AppendChild(elementName);

                XmlElement elementCount = doc.CreateElement("count");
                XmlText textCount = doc.CreateTextNode(animal.Count.ToString());
                elementCount.AppendChild(textCount);
                elementAnimal.AppendChild(elementCount);
            }

            doc.Save(path);
        }

        public void createReport(string path = "dataTxt.txt")
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            string head = "Zoo report \nDate: " + DateTime.Now.ToString("yyyy - MM - dd HH: mm\n") + "We have:\n";

            streamWriter.WriteLine(head);

            int totalAnimalCount = 0;

            foreach (Animal animal in animals)
            {
                string data = string.Empty;

                data += animal.Count + " of " + animal.Name;
                totalAnimalCount += animal.Count;

                streamWriter.WriteLine(data);
            }

            string footer = "\nTotal we have: " + totalAnimalCount.ToString() + " animals.";

            streamWriter.WriteLine(footer);

            streamWriter.Close();
            fileStream.Close();
        }
    }

}
