using stockLib_121;
using StockLib_121;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace StockWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ManagerNavigator.MainFrame = MainFrame;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AllProduct_Click(object sender, RoutedEventArgs e)
        {
            ManagerNavigator.MainFrame.Navigate(new AllProductsPage());
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            ManagerNavigator.MainFrame.Navigate(new OneProductPage());
        }

        private void LoadToXML_Click(object sender, RoutedEventArgs e)
        {
            //Создать новый документ
            XmlDocument xmlDoc = new XmlDocument();
            //Читаем из файла
            xmlDoc.Load("products.xml");
            //Получить корневой узел
            XmlElement rootNode = xmlDoc.DocumentElement;
            if (rootNode != null)
            {
                //Обходим все узлы в корневом узле
                foreach(XmlElement xNode in rootNode)
                {
                    //получаем атрибут name
                    XmlNode nameAttribute = xNode.Attributes.GetNamedItem("name");
                    string article = " ";
                    int price = 0;
                    double margine = 0;
                    //обходим все дочерние узлы элемента producti
                    foreach(XmlElement xElem in xNode.ChildNodes)
                    {
                        if (xElem.Name = "article")
                            article = xElem.InnerText;
                        if (xElem.Name = "price")
                            price = int.Parse(xElem.InnerText);
                        if (xElem.Name = "margine")
                            margine = double.Parse(xElem.InnerText);
                    }
                    Product product = new Product(nameAttribute.Value,
                        article, price, margine);

                }
            }
        }

        private void SaveToXML_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("stock");
            xmlDoc.AppendChild(rootNode);
            foreach(var product in ManagerModel.stock)
            {
                //создаем новый узел = товар
                XmlNode productNode = xmlDoc.CreateElement("product");
                //создаем атрибут
                XmlAttribute nameAttribute = xmlDoc.CreateAttribute("name");

                nameAttribute.Value = product.Name;
                //добавить в узел атрибут
                productNode.Attributes.Append(nameAttribute);
                //создаем птвый подъэлемент в узле товара

                XmlElement articleElem = xmlDoc.CreateElement("article");
                XmlText articleText = xmlDoc.CreateTextNode(product.Article);
                articleElem.AppendChild(articleText);
                //добавить к товару
                productNode.AppendChild(articleElem);

                XmlElement priceElem = xmlDoc.CreateElement("price");
                priceElem.AppendChild(xmlDoc.CreateTextNode(product.Price.ToString()));
                //добавить к товару
                productNode.AppendChild(priceElem);

                XmlElement margineElem = xmlDoc.CreateElement("margine");
                margineElem.AppendChild(xmlDoc.CreateTextNode(product.Margin.ToString()));
                //добавить к товару
                productNode.AppendChild(margineElem);

                //созданный узел добавить в структуру
                rootNode.AppendChild(productNode);
            }
            xmlDoc.Save("products.xml");
        }

        private void LoadFromXMLSer_Click(object sender, EventArgs e)
        {
            //задаем формат серии
            XmlSerializer formatter = new XmlSerializer(typeof(Stock));
            //сохранение списка в файл
            using (FileStream fs = new FileStream("productAll.xml", FileMode.Open))
            {
                ManagerModel.stock.AddRange(formatter.Deserialize(fs) as Stock);
            }
        }

        private void SaveToXMLSer_Click(object sender, EventArgs e)
        {
            //задаем формат серии
            XmlSerializer formatter = new XmlSerializer(typeof(Stock));
            //сохранение списка в файл
            using(FileStream fs = new FileStream("productAll.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ManagerModel.stock);
            }
        }
        private void LoadFromJSONSer_Click(object sender, EventArgs e)
        {
            ManagerModel.stock.AddRange(
                JsonSerializer.Deserialize<Stock>(File.ReadAllText("products.json")));
        }
        private void SaveToJSONSer_Click(object sender, EventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, //будут ли проблелы и отступы
                //русский язык без кодировки
                Encoder = System.Text.Encoding.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText("products.json",
                JsonSerializer.Serialize(ManagerModel.stock));
        }
    }
}
