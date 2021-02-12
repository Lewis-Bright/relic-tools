using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace IBBoard.Xml
{
	public class XmlResourceResolver : XmlUrlResolver
	{
		private Assembly assm;
		private Dictionary<string, string> relativeToAbsoluteMap = new Dictionary<string, string>();
		private Dictionary<string, string> absoluteToResourceMap = new Dictionary<string, string>();

		public XmlResourceResolver(Assembly assembly)
		{
			assm = assembly;
		}

		public void AddMapping(string relativeUri, string absoluteUri, string resourceName)
		{
			relativeToAbsoluteMap.Add(relativeUri, absoluteUri);
			AddMapping(absoluteUri, resourceName);
		}

		public void AddMapping(string absoluteUri, string resourceName)
		{
			absoluteToResourceMap[absoluteUri] = resourceName;
		}

		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			Uri resolved;

			if (relativeToAbsoluteMap.ContainsKey(relativeUri))
			{
				resolved = new Uri(relativeToAbsoluteMap[relativeUri], UriKind.Absolute);
			}
			else if (absoluteToResourceMap.ContainsKey(relativeUri))
			{
				resolved = new Uri(relativeUri, UriKind.Absolute);
			}
			else
			{
				resolved = base.ResolveUri(baseUri, relativeUri);
			}

			return resolved;
		}

		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			string absoluteUriString = absoluteUri == null ? null : absoluteUri.ToString();

			if (absoluteToResourceMap.ContainsKey(absoluteUriString))
			{
				string file = absoluteToResourceMap[absoluteUriString];
				return assm.GetManifestResourceStream(file);
			}

			return base.GetEntity(absoluteUri, role, ofObjectToReturn);
		}
	}
}

