// This file (IBBXmlResolver.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Xml;

namespace IBBoard.Xml
{
	/// <summary>
	/// Summary description for IBBXmlResolver.
	/// </summary>
	public class IBBXmlResolver : XmlUrlResolver
	{
		private string baseFilePath = "";

		public IBBXmlResolver(string basePath)
		{
			baseFilePath = basePath;
		}

		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			if (relativeUri.StartsWith("dtds/"))
			{
				//Uri uri = base.ResolveUri(baseUri, relativeUri);
				return new Uri(Uri.UriSchemeFile + "://" + baseFilePath + "/" + relativeUri);
			}
			else
			{
				return base.ResolveUri(baseUri, relativeUri);
			}
		}
	}
}
