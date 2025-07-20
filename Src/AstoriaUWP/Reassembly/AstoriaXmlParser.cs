using AndroidInteropLib.android.content.res;
using AndroidInteropLib.org.xmlpull.v1;
using AndroidXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DalvikUWPCSharp.Reassembly
{
    public class AstoriaXmlParser : XmlResourceParser
    {
        private XmlReader doc;

        public AstoriaXmlParser(XmlReader docx)
        {
            doc = docx;
            //doc.MoveToElement();
            //doc.MoveToContent();
        }

        public override void setFeature(string name, bool state)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] setFeature not implemented: {name}");
            // No-op fallback
        }

        public override bool getFeature(string name)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getFeature not implemented: {name}");
            return false;
        }

        public override void setProperty(string name, object value)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] setProperty not implemented: {name}");
            // No-op fallback
        }

        public override object getProperty(string name)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getProperty not implemented: {name}");
            return null;
        }

        public override string getInputEncoding()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaXmlParser] getInputEncoding not implemented");
            return "utf-8";
        }

        public override void defineEntityReplacementText(string entityName, string replacementText)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] defineEntityReplacementText not implemented: {entityName}");
            // No-op fallback
        }

        public override int getNamespaceCount(int depth)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getNamespaceCount not implemented: {depth}");
            return 0;
        }

        public override string getNamespacePrefix(int pos)
        {
            string expandedNameTemp = doc.Name;
            doc.MoveToAttribute(pos);
            string prefix = doc.Prefix;
            doc.MoveToAttribute(expandedNameTemp);
            return prefix;

        }

        public override string getNamespaceUri(int pos)
        {
            string expandedNameTemp = doc.Name;
            doc.MoveToAttribute(pos);
            string nUri = doc.NamespaceURI;
            doc.MoveToAttribute(expandedNameTemp);
            return nUri;
        }

        public override string getNamespace(string prefix)
        {
            return doc.LookupNamespace(prefix);
        }

        public override int getDepth()
        {
            return doc.Depth;
        }

        public override string getPositionDescription()
        {
            return $"Doc Name: {doc.Name}";
        }

        public override int getLineNumber()
        {
            return doc.Settings.LineNumberOffset;
        }

        public override int getColumnNumber()
        {
            return doc.Settings.LinePositionOffset;
        }

        public override bool isWhitespace()
        {
            return false;
        }

        public override string getText()
        {
            return doc.ReadContentAsString();
        }

        public override string getTextCharacters(int[] holderForStartAndLength)
        {
            //Not sure if this is correct?
            return doc.ReadContentAsString();
        }

        public override string getNamespace()
        {
            return doc.NamespaceURI;
        }

        public override string getName()
        {
            return doc.Name;
        }

        public override string getPrefix()
        {
            return doc.Prefix;
        }

        public override bool isEmptyElementTag()
        {
            return doc.IsEmptyElement;
        }

        public override int getAttributeCount()
        {
            return doc.AttributeCount;
        }

        public override string getAttributeNamespace(int index)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getAttributeNamespace not implemented: {index}");
            return string.Empty;
        }

        public override string getAttributeName(int index)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getAttributeName not implemented: {index}");
            return string.Empty;
        }

        public override string getAttributePrefix(int index)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getAttributePrefix not implemented: {index}");
            return string.Empty;
        }

        public override string getAttributeType(int index)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaXmlParser] getAttributeType not implemented: {index}");
            return "CDATA";
        }

        public override bool isAttributeDefault(int index)
        {
            return doc.IsDefault;
        }

        public override string getAttributeValue(int index)
        {
            return doc.GetAttribute(index);
        }

        public override string getAttributeValue(string nspace, string name)
        {
            if(nspace != null)
            {
                return doc.GetAttribute(name, nspace);
            }

            else
            {
                return doc.GetAttribute(name);
            }
        }

        public override int getEventType()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaXmlParser] getEventType not implemented");
            return -1;
        }

        public override int next()
        {
            doc.Read();

            switch (doc.NodeType)
            {
                case System.Xml.XmlNodeType.Text:
                    return TEXT;
                case System.Xml.XmlNodeType.Element:
                    return START_TAG;
                case System.Xml.XmlNodeType.EndElement:
                    return END_TAG;
                    
            }

            //bool b = doc.MoveToNextAttribute();
            //if (b)
            //return 1;
            //return -1;
            return 0;
        }

        public override int nextToken()
        {
            doc.Read();

            switch (doc.NodeType)
            {
                case System.Xml.XmlNodeType.CDATA:
                    return CDSECT;
                case System.Xml.XmlNodeType.Comment:
                    return COMMENT;
                case System.Xml.XmlNodeType.DocumentType:
                    return DOCDECL;
                case System.Xml.XmlNodeType.EntityReference:
                    return ENTITY_REF;
                case System.Xml.XmlNodeType.ProcessingInstruction:
                    return PROCESSING_INSTRUCTION;
                case System.Xml.XmlNodeType.Whitespace:
                    return IGNORABLE_WHITESPACE;
                case System.Xml.XmlNodeType.Text:
                    return TEXT;
                case System.Xml.XmlNodeType.Element:
                    return START_TAG;
                case System.Xml.XmlNodeType.EndElement:
                    return END_TAG;
            }

            if(doc.IsStartElement())
            {
                return START_DOCUMENT;
            }


            //if node is not found, end it
            return END_TAG;
        }

        public override void require(int type, string nspace, string name)
        {
            throw new NotImplementedException();
        }

        public override string nextText()
        {
            //doc.MoveToNextAttribute();
            return doc.ReadContentAsString();
        }

        public override int nextTag()
        {
            throw new NotImplementedException();
        }

        public override void close()
        {
            //doc.Dispose();
        }
    }
}
