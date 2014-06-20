using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using System.IO;

/**
 * developed by : raza_ahmed@outlook.com 
 */

namespace RaaZ_TwitterConnect
{
    class XMLStuff
    {
        /// <summary>
        /// Write these three arguments to xml file. 
        /// </summary>
        /// <param name="screenName">screenName is twitter username</param>
        /// <param name="accessToken">accessToken is the token returned from twitter after authentication</param>
        /// <param name="accessTokenSecret">accessSecret is the value returned from twitter after authentication</param>
        public bool writeToXML(string screenName, string accessToken, string accessTokenSecret)
        //We are not saving consumerKey and consumerSecret since they are saved in App.cofig(application settings)
        {
            try
            {
                XmlTextWriter myXmlTextWriter = new XmlTextWriter("UserInfo.xml", null);
                myXmlTextWriter.Formatting = Formatting.Indented;
                myXmlTextWriter.WriteStartDocument(false);
                myXmlTextWriter.WriteDocType("UserInfo", null, null, null);
                myXmlTextWriter.WriteComment("This file stores current user tokens.");
                myXmlTextWriter.WriteStartElement("Details");
                myXmlTextWriter.WriteAttributeString("screenName", screenName);
                myXmlTextWriter.WriteAttributeString("accessToken", accessToken);
                myXmlTextWriter.WriteAttributeString("accessTokenSecret", accessTokenSecret);
                myXmlTextWriter.WriteEndElement();

                //Write the XML to file and close the myXmlTextWriter
                myXmlTextWriter.Flush();
                myXmlTextWriter.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Read Data from xml file and return in a generic string collection.
        /// </summary>
        /// <returns></returns>
        public List<string> readFromXml()
        {
            try
            {
                List<string> xmlData = new List<string>();
                XmlTextReader reader = new XmlTextReader("UserInfo.xml");
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:   //the node is an Element
                            // xmlData.Add(reader.Name);
                            while (reader.MoveToNextAttribute())
                            {
                                //xmlData.Add(reader.Name);                     
                                //the above line is commented because these attributes name and value contains same value.
                                //(uptil now) e.g <Details screenname="screenName" accessToken="accessToken" accessTokenSecret="accessTokenSecret" />
                                xmlData.Add(reader.Value);
                            }
                            break;
                        case XmlNodeType.DocumentType: //The node is a DocumentType
                            xmlData.Add("DocumentName: " + reader.Name);
                            // xmlData.Add("DocumentValue: " + reader.Value);    
                            //the above line is commented because this xml file contains no document value. so reader.
                            //Value will return blank string or null
                            break;
                    }

                }
                reader.Close();
                return xmlData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Logout from twitter.
        /// This method just delets the xml file that store user tokens. So user will no longer be loggedin and he have to login again.
        /// The return type is bool. If some error occurs, false value will be returned.
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            try
            {
                File.Delete(@"UserInfo.xml");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
