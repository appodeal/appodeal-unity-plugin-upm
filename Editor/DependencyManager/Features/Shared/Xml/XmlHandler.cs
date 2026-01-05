using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class XmlHandler
    {
        public static Outcome<XmlDependencies> TryDeserializeDependencies(FileInfo file)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(file.FullName);

                var serializer = new XmlSerializer(typeof(XmlDependencies));

                using var reader = new StringReader(xmlDocument.OuterXml);
                return (XmlDependencies)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }

        public static Outcome<XmlDependencies> TryDeserializeDependencies(string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                var serializer = new XmlSerializer(typeof(XmlDependencies));

                using var reader = new StringReader(xmlDocument.OuterXml);
                return (XmlDependencies)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }

        public static Outcome<string> TrySerializeDependencies(XmlDependencies model)
        {
            try
            {
                var xws = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true, IndentChars = "\t", NewLineChars = "\n" };
                using var stringWriter = new Utf8StringWriter();
                using var writer = XmlWriter.Create(stringWriter, xws);

                var serializer = new XmlSerializer(typeof(XmlDependencies));
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                serializer.Serialize(writer, model, ns);

                string xmlContent = stringWriter.ToString();
                if (!xmlContent.EndsWith("\n")) xmlContent += "\n";
                return xmlContent;
            }
            catch (Exception ex)
            {
                return Failure.Create(ex.GetType().ToString(), ex.Message);
            }
        }

        public static bool TryUpdateDependencies(XmlDependencies model)
        {
            try
            {
                Directory.CreateDirectory(DmConstants.Validation.DependenciesDir);

                var serializationOutcome = TrySerializeDependencies(model);
                if (!serializationOutcome.IsSuccess)
                {
                    LogHelper.LogError($"Dependencies update failed. Reason - '{serializationOutcome.Failure.Message}'");
                    return false;
                }

                File.WriteAllText(DmConstants.Validation.DependenciesFilePath, serializationOutcome.Value, Encoding.UTF8);
                AssetDatabase.Refresh();

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                return false;
            }
        }
    }
}
