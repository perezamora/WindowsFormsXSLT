using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace WindowsFormsXSLT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TransformXMLtoHTML("docs/XSLTFile1.xslt", "docs/XMLFile1.xml", "dXMLFile1.html");
            FilterNodeXdocumentJson();
            FilterNodeXdocumentXml();
        }

        private void TransformXMLtoHTML(string urlXSLT, string urlXML, string urlHTML)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(urlXSLT);
            xslt.Transform(urlXML, urlHTML);
        }

        private void FilterNodeXdocumentJson()
        {
            var loadedCustomData = XDocument.Load("docs/XMLFile1.xml");
            var filteredData = from c in loadedCustomData.Descendants("cd")
                                .Where(r => r.Element("title").Value.ToUpper().StartsWith("S") ||
                                        r.Element("artist").Value.ToUpper().StartsWith("S") ||
                                        r.Element("country").Value.ToUpper().StartsWith("S") ||
                                        r.Element("company").Value.ToUpper().StartsWith("S"))
                               select new
                               {
                                   Title = (string)c.Element("title"),
                                   Artist = (string)c.Element("artist"),
                                   Country = (string)c.Element("country"),
                                   Company = (string)c.Element("company")
                               };

            listBox1.DataSource = filteredData.ToList();
        }

        private void FilterNodeXdocumentXml()
        {
            var loadedCustomData = XDocument.Load("docs/XMLFile1.xml");
            var filteredDataAux = from c in loadedCustomData.Descendants("cd")
                                    .Where(r => r.Element("title").Value.ToUpper().StartsWith("S") ||
                                            r.Element("artist").Value.ToUpper().StartsWith("S") ||
                                            r.Element("country").Value.ToUpper().StartsWith("S") ||
                                            r.Element("company").Value.ToUpper().StartsWith("S"))
                                  select c;

            listBox2.DataSource = filteredDataAux.ToList();

            using (XmlWriter xw = XmlWriter.Create("docs/filter.xml"))
            {
                xw.WriteStartElement("catalog");

                foreach (var element in filteredDataAux)
                {
                    element.WriteTo(xw);
                }

                xw.WriteEndElement();

            }
            TransformXMLtoHTML("docs/XSLTFile1.xslt", "docs/filter.xml", "dXMLFile2.html");
        }

    }
}
