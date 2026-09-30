using System.IO;
using System.Xml.Serialization;
using UnityEditor.PackageManager;
using NUnit.Framework;
using AppodealInc.Mediation.DependencyManager.Editor;
using AppodealInc.Mediation.Utils.Editor;
using AppodealStack.Monetization.Common;

namespace AppodealInc.Mediation.Editor.Tests
{
    public class PluginVersionTests
    {
        [Test]
        public void GetPluginVersion_ShouldMatchPackageJsonVersion_WhenPackageIsInstalled()
        {
            // Arrange
            string packageJsonVersion = GetPackageJsonVersion();

            // Act
            string pluginVersion = AppodealVersions.GetPluginVersion();

            // Assert
            Assert.AreEqual(packageJsonVersion, pluginVersion);
        }

        [Test]
        public void BundledDependenciesPluginVersion_ShouldMatchPackageJsonVersion_WhenPackageIsInstalled()
        {
            // Arrange
            string packageJsonVersion = GetPackageJsonVersion();
            var serializer = new XmlSerializer(typeof(XmlDependencies));

            // Act
            using var reader = File.OpenText(AppodealEditorConstants.BundledDependenciesFilePath);
            var dependencies = (XmlDependencies)serializer.Deserialize(reader);

            // Assert
            Assert.AreEqual(packageJsonVersion, dependencies.PluginVersion);
        }

        private static string GetPackageJsonVersion()
        {
            var packageInfo = PackageInfo.FindForAssembly(typeof(AppodealVersions).Assembly);
            Assert.IsNotNull(packageInfo, "Package info not found for the Common assembly");
            return packageInfo.version;
        }
    }
}
