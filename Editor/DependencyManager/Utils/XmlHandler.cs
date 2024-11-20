// ReSharper disable CheckNamespace

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
        public static Request<XmlDependenciesModel> DeserializeDependencies(FileInfo file)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(file.FullName);

                var serializer = new XmlSerializer(typeof(XmlDependenciesModel));

                using var reader = new StringReader(xmlDocument.OuterXml);
                return (XmlDependenciesModel)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        public static Request<XmlDependenciesModel> DeserializeDependencies(string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                var serializer = new XmlSerializer(typeof(XmlDependenciesModel));

                using var reader = new StringReader(xmlDocument.OuterXml);
                return (XmlDependenciesModel)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                return Error.Create(e.GetType().ToString(), e.Message);
            }
        }

        public static bool UpdateDependencies(XmlDependenciesModel model)
        {
            try
            {
                Directory.CreateDirectory(DmConstants.DependenciesDir);
                bool shouldRefreshDatabase = !File.Exists(DmConstants.DependenciesFilePath);

                var depsFileInfoRequest = DataLoader.GetDependenciesFileInfo();
                if (!depsFileInfoRequest.IsSuccess)
                {
                    LogHelper.LogError($"Dependencies update failed. Reason - '{depsFileInfoRequest.Error.Message}'.");
                    return false;
                }

                var xws = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true, IndentChars = "\t" };
                using var writer = XmlWriter.Create(depsFileInfoRequest.Value.FullName, xws);

                var serializer = new XmlSerializer(typeof(XmlDependenciesModel));
                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                serializer.Serialize(writer, model, ns);

                if (shouldRefreshDatabase) AssetDatabase.Refresh();

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogException(e);
                return false;
            }
        }
    }
}
