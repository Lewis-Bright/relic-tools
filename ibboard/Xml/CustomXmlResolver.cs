// This file (CustomXmlResolver.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Collections.Generic;
using System.Xml;

namespace IBBoard.Xml
{
	/// <summary>
	/// A custom XML resolver that lets developers specify their own mappings of relative URIs to file locations.
	/// </summary>
	public class CustomXmlResolver : XmlUrlResolver
	{
		private Dictionary<string, Uri> relativeToUriMap = new Dictionary<string, Uri>();

		/// <summary>
		/// Adds a custom mapping of relative URI string to actual URI that will be returned by <code>ResolveUri(Uri, string)</code>
		/// </summary>
		/// <param name="relativeURI">the relative URI string</param>
		/// <param name="resolvedUri">the <code>Uri</code> that <code>ResolveUri(Uri, string)</code> should return for <paramref name="relativeURI"/> </param>
		public void AddMapping(string relativeURI, Uri resolvedUri)
		{
			relativeToUriMap.Add(relativeURI, resolvedUri);
		}

		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			Uri uri = DictionaryUtils.GetValue(relativeToUriMap, relativeUri);
			return (uri == null) ? base.ResolveUri(baseUri, relativeUri) : uri;
		}
	}
}
