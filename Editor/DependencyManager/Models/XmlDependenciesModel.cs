using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AppodealInc.Mediation.DependencyManager.Editor
{
	[Serializable]
	[XmlRoot(ElementName = "dependencies")]
	public class XmlDependenciesModel
	{
		[XmlElement(ElementName = "iosPods")]
		public IosNode Ios { get; set; }

		[XmlElement(ElementName = "androidPackages")]
		public AndroidNode Android { get; set; }

		public override string ToString() => JsonUtility.ToJson(this);

		public bool IsDifferentFromRemote(XmlDependenciesModel remote) => DependenciesDiff.Get(this, remote).Any();
	}

	[Serializable]
	[XmlRoot(ElementName = "iosPods")]
	public class IosNode
	{
		[XmlElement(ElementName = "iosPod")]
		public List<PodNode> Pods { get; set; }

		[XmlArray("sources")]
		[XmlArrayItem("source")]
		public List<string> Sources { get; set; }
	}

	[Serializable]
	[XmlRoot(ElementName = "iosPod")]
	public class PodNode
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }

		[XmlAttribute(AttributeName = "minTargetSdk")]
		public string MinTargetSdk { get; set; }
	}

	[Serializable]
	[XmlRoot(ElementName = "androidPackages")]
	public class AndroidNode
	{
		[XmlElement(ElementName = "androidPackage")]
		public List<PackageNode> Packages { get; set; }

		[XmlArray("repositories")]
		[XmlArrayItem("repository")]
		public List<string> Repositories { get; set; }
	}

	[Serializable]
	[XmlRoot(ElementName = "androidPackage")]
	public class PackageNode
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "spec")]
		public string Spec { get; set; }
	}
}
