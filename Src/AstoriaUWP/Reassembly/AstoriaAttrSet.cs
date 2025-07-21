using AndroidInteropLib.android.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalvikUWPCSharp.Reassembly
{
    public class AstoriaAttrSet : AttributeSet
    {
        public readonly XAttribute[] attributes;
        public readonly XElement xe1;
        //private readonly XElement element;
        //ANDROID: namespace = string p1nspace = "{http://schemas.android.com/apk/res/android}";


        public AstoriaAttrSet(XElement xe)
        {
            xe1 = xe;
            attributes = xe.Attributes().ToArray();
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] Initialized with {attributes.Length} attributes from element: {xe1.Name}");
        }

        public bool getAttributeBooleanValue(string nspace, string attribute, bool defaultValue)
        {
            try {
                var val = FindAttributeVal(nspace, attribute);
                if (val == null) throw new Exception("Attribute missing");
                return bool.Parse(val);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeBooleanValue failed for {attribute}: {ex.Message}");
                return defaultValue;
            }
        }

        public bool getAttributeBooleanValue(int index, bool defaultValue)
        {
            try { return bool.Parse(attributes[index].Value); }
            catch { return defaultValue; }
        }

        public int getAttributeCount()
        {
            return attributes.Length;
        }

        public float getAttributeFloatValue(int index, float defaultValue)
        {
            try { return float.Parse(attributes[index].Value); }
            catch { return defaultValue; }
        }

        public float getAttributeFloatValue(string nspace, string attribute, float defaultValue)
        {
            try {
                var val = FindAttributeVal(nspace, attribute);
                if (val == null) throw new Exception("Attribute missing");
                return float.Parse(val);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeFloatValue failed for {attribute}: {ex.Message}");
                return defaultValue;
            }
        }

        public int getAttributeIntValue(string nspace, string attribute, int defaultValue)
        {
            try {
                var val = FindAttributeVal(nspace, attribute);
                if (val == null) throw new Exception("Attribute missing");
                return int.Parse(val);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeIntValue failed for {attribute}: {ex.Message}");
                return defaultValue;
            }
        }

        public int getAttributeIntValue(int index, int defaultValue)
        {
            try { return int.Parse(attributes[index].Value); }
            catch { return defaultValue; }
        }

        public int getAttributeListValue(int index, string[] options, int defaultValue)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeListValue not implemented: index={index}");
            return defaultValue;
        }

        public int getAttributeListValue(string nspace, string attribute, string[] options, int defaultValue)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeListValue not implemented: {attribute}");
            return defaultValue;
        }

        public string getAttributeName(int index)
        {
            try { return attributes[index].Name.ToString(); }
            catch { return null; }
        }

        public int getAttributeNameResource(int index)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeNameResource not implemented: index={index}");
            return -1;
        }

        public int getAttributeResourceValue(string nspace, string attribute, int defaultValue)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeResourceValue not implemented: {attribute}");
            return defaultValue;
        }

        public int getAttributeResourceValue(int index, int defaultValue)
        {
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeResourceValue not implemented: index={index}");
            return defaultValue;
        }

        public uint getAttributeUnsignedIntValue(string nspace, string attribute, uint defaultValue)
        {
            try {
                var val = FindAttributeVal(nspace, attribute);
                if (val == null) throw new Exception("Attribute missing");
                return uint.Parse(val);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] getAttributeUnsignedIntValue failed for {attribute}: {ex.Message}");
                return defaultValue;
            }
        }

        public uint getAttributeUnsignedIntValue(int index, uint defaultValue)
        {
            try { return uint.Parse(attributes[index].Value); }
            catch { return defaultValue; }
        }

        public string getAttributeValue(int index)
        {
            return attributes[index].Value;
        }

        public string getAttributeValue(string nspace, string name)
        {
            return FindAttributeVal(nspace, name);
        }

        public string getClassAttribute()
        {
            return getAttributeValue(null, "class");
        }

        public string getIdAttribute()
        {
            return getAttributeValue(null, "id");
        }

        public int getIdAttributeResourceValue(int defaultValue)
        {
            return getAttributeResourceValue(null, "id", defaultValue);
        }

        public string getPositionDescription()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaAttrSet] getPositionDescription not implemented");
            return string.Empty;
        }

        public int getStyleAttribute()
        {
            System.Diagnostics.Debug.WriteLine("[AstoriaAttrSet] getStyleAttribute not implemented");
            return -1;
        }

        private string FindAttributeVal(string nspace, string attribute)
        {
            //Wrap nspace with curly brackets if not already
            if(!string.IsNullOrEmpty(nspace) && !nspace.StartsWith("{") && !nspace.EndsWith("}"))
            {
                nspace = "{" + nspace + "}";
            }
            string expandedName = (nspace ?? "") + attribute;
            foreach(XAttribute xa in attributes)
            {
                if(xa.Name.ToString().Equals(expandedName))
                {
                    System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] Found attribute: {expandedName} = {xa.Value}");
                    return xa.Value;
                }
            }
            System.Diagnostics.Debug.WriteLine($"[AstoriaAttrSet] Attribute not found: {expandedName}");
            return null;
        }
    }
}
