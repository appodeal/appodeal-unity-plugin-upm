using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.Utils
{
    [SuppressMessage("ReSharper", "MemberInitializerValueIgnored")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
    public class XmlUtilities
    {
        public static int Num;
        public static bool ParseXmlTextFileElements(
            string filename,
            ParseElement parseElement)
        {
            if (!File.Exists(filename))
                return false;
            try
            {
                using var xmlTextReader = new XmlTextReader(new StreamReader(filename));
                var elementNameStack = new List<string>();
                Func<string> func = () => elementNameStack.Count > 0 ? elementNameStack[0] : "";
                var reader = new Reader(xmlTextReader);
                while (reader.Reading)
                {
                    var name = xmlTextReader.Name;
                    var parentElementName = func();
                    if (xmlTextReader.NodeType == XmlNodeType.Element)
                    {
                        if (parseElement(xmlTextReader, name, true, parentElementName, elementNameStack))
                            elementNameStack.Insert(0, name);
                        if (reader.XmlReaderIsAhead)
                        {
                            reader.Read();
                            continue;
                        }
                    }

                    if ((xmlTextReader.NodeType == XmlNodeType.EndElement ||
                         xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.IsEmptyElement) &&
                        !string.IsNullOrEmpty(parentElementName))
                    {
                        if (elementNameStack[0] == name)
                            elementNameStack.RemoveAt(0);
                        else
                            elementNameStack.Clear();
                        Num = parseElement(xmlTextReader, name, false, func(), elementNameStack) ? 1 : 0;
                    }

                    reader.Read();
                }
            }
            catch (XmlException ex)
            {
                Debug.Log($"Failed while parsing XML file {filename}\n{ex}\n");
                return false;
            }

            return true;
        }

        private class Reader
        {
            private int _lineNumber = -1;
            private int _linePosition = -1;
            private readonly XmlTextReader _reader;

            public Reader(XmlTextReader xmlReader)
            {
                _reader = xmlReader;
                Reading = _reader.Read();
                _lineNumber = _reader.LineNumber;
                _linePosition = _reader.LinePosition;
            }

            public bool Reading { private set; get; }

            public bool XmlReaderIsAhead
            {
                get
                {
                    if (_lineNumber == _reader.LineNumber)
                        return _linePosition != _reader.LinePosition;
                    return true;
                }
            }

            public bool Read()
            {
                var flag = false;
                if (Reading && !XmlReaderIsAhead)
                {
                    Reading = _reader.Read();
                    flag = true;
                }

                _lineNumber = _reader.LineNumber;
                _linePosition = _reader.LinePosition;
                return flag;
            }
        }

        public delegate bool ParseElement(
            XmlTextReader reader,
            string elementName,
            bool isStart,
            string parentElementName,
            List<string> elementNameStack);
    }
}
