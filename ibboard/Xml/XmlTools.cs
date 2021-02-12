//  This file (XmlTools.cs) is a part of the IBBoard library and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.
// 
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.IO;

namespace IBBoard.Xml
{
	/// <summary>
	/// Some basic tools for handling XML files and retrieving their values
	/// </summary>
	public class XmlTools
	{
		private static Regex idRegex;
		private static Regex multiUnderscoreRegex;
		private static NumberFormatInfo doubleFormat;

		/// <summary>
		/// Gets the value of an attribute of an element as a boolean. Throws a FormatException if the attribute is not a boolean.
		/// </summary>
		/// <param name="elem">
		/// The <see cref="XmlElement"/> to get the attribute value of
		/// </param>
		/// <param name="attributeName">
		/// The name of the attribute to get as a boolean
		/// </param>
		/// <returns>
		/// The value of the attribute as an boolean
		/// </returns>
		public static bool GetBoolValueFromAttribute(XmlElement elem, string attributeName)
		{
			try
			{
				return bool.Parse(elem.GetAttribute(attributeName));
			}
			catch (FormatException)
			{
				throw new FormatException(String.Format("Attribute '{0}' of {1} with ID {2} was not a valid boolean", attributeName, elem.Name, elem.GetAttribute("id")));
			}
		}
		
		/// <summary>
		/// Gets the value of an attribute of an element as an integer. Throws a FormatException if the attribute is not an integer.
		/// </summary>
		/// <param name="elem">
		/// The <see cref="XmlElement"/> to get the attribute value of
		/// </param>
		/// <param name="attributeName">
		/// The name of the attribute to get as an integer
		/// </param>
		/// <returns>
		/// The value of the attribute as an integer
		/// </returns>
		public static int GetIntValueFromAttribute(XmlElement elem, string attributeName)
		{
			try
			{
				return int.Parse(elem.GetAttribute(attributeName));
			}
			catch (FormatException)
			{
				throw new FormatException(String.Format("Attribute '{0}' of {1} with ID {2} was not a valid number", attributeName, elem.Name, elem.GetAttribute("id")));
			}
		}
							
		/// <summary>
		/// Gets the value of an attribute of an element as a double. Throws a FormatException if the attribute is not a double.
		/// </summary>
		/// <param name="elem">
		/// The <see cref="XmlElement"/> to get the attribute value of
		/// </param>
		/// <param name="attributeName">
		/// The name of the attribute to get as a double
		/// </param>
		/// <returns>
		/// The value of the attribute as an double
		/// </returns>
		public static double GetDoubleValueFromAttribute(XmlElement elem, string attributeName)
		{
			double doubleVal = double.NaN;
			string attribValue = elem.GetAttribute(attributeName);
			
			if (attribValue == "INF")
			{
				doubleVal = double.PositiveInfinity;
			}
			else
			{
				try
				{
					return double.Parse(attribValue, GetNumberFormatInfo());
				}
				catch (FormatException)
				{
					throw new FormatException(String.Format("Attribute '{0}' of {1} with ID {2} was not a valid number", attributeName, elem.Name, elem.GetAttribute("id")));
				}
			}
			
			return doubleVal;
		}
		
		public static T GetEnumValueFromAttribute<T>(XmlElement elem, string attributeName)
		{
			return GetEnumValueFromAttribute<T>(elem, attributeName, true);
		}
		
		public static T GetEnumValueFromAttribute<T>(XmlElement elem, string attributeName, bool ignoreCase)
		{
			string attribValue = elem.GetAttribute(attributeName);
			
			try
			{
				return EnumTools.ParseEnum<T>(attribValue, ignoreCase);
			}
			catch (ArgumentException)
			{
				throw new FormatException(String.Format("Attribute '{0}' with value {1} for {2} with ID '{3}' was not a valid {4} enum", attributeName, attribValue, elem.Name, elem.GetAttribute("id"), typeof(T).Name));
			}
		}

		private static NumberFormatInfo GetNumberFormatInfo()
		{
			if (doubleFormat == null)
			{
				doubleFormat = NumberFormatInfo.InvariantInfo;
			}

			return doubleFormat;
		}

		private static Regex GetIdRegex()
		{
			if (idRegex == null)
			{
				idRegex = new Regex("[^a-zA-Z0-9:\\._-]+");
			}
			
			return idRegex;
		}
		
		private static Regex GetMultiUnderscoreRegex()
		{
			if (multiUnderscoreRegex == null)
			{
				multiUnderscoreRegex = new Regex("_{2,}");
			}
			
			return multiUnderscoreRegex;
		}
		
		/// <summary>
		/// Gets a valid XML ID for a given string that does not contain accented and non-ASCII characters. Matches the allowed characters
		/// in the XML spec (http://www.w3.org/TR/xml/#NT-NameStartChar) where the characters do not use Unicode character codes. If the ID
		/// starts with an invalid character then it will be prepended with an underscore.
		/// </summary>
		/// <param name="str">
		/// The <see cref="System.String"/> to turn in to a valid ID
		/// </param>
		/// <returns>
		/// The valid XML ID with all series of invalid characters replaced with an underscore
		/// </returns>
		public static string GetAsciiXmlIdForString(string str)
		{
			string id = GetIdRegex().Replace(str, "_");
			id = GetMultiUnderscoreRegex().Replace(id, "_");
			
			if (!IdStartsWithValidCharacter(id))
			{
				id = "_" + id;
			}
			
			return id;
		}
		
		private static bool IdStartsWithValidCharacter(string id)
		{
			bool valid = false;
			
			if (id.Length > 0)
			{
				char firstChar = id[0];
				valid = ('A' <= firstChar && firstChar <= 'Z') || ('a' <= firstChar && firstChar <= 'z') || firstChar == '_' || firstChar == ':';
			}
			
			return valid;
		}

		public static void AddSchemaToSetFromResource(XmlSchemaSet schemaSet, string targetNamespace, Assembly assm, string id)
		{
			Stream resStream = assm.GetManifestResourceStream(id);
			schemaSet.Add(targetNamespace, new XmlTextReader(resStream));
		}
	}
}
