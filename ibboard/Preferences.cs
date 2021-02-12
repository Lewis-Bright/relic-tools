// This file (Preferences.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Reflection;
using IBBoard.IO;

//TODO: Add import/export
namespace IBBoard
{
	/// <summary>
	/// Summary description for Preferences.
	/// </summary>
	public class Preferences
	{
		private static Type stringType = typeof(string);
		private static Type hashtableType = typeof(Hashtable);
		private Hashtable htGlobal;
		private Hashtable htLocal;

		private string app;
		private bool modified = false;

		
		public Preferences(string appName)
		{
			app = appName;
			LoadPreferences();
		}

		private void LoadPreferences()
		{
			htGlobal = new Hashtable();
			htLocal = new Hashtable();
			
			string globalPath = Path.Combine(Constants.ExecutablePath, app + "Pref.xml");
			
			if (File.Exists(globalPath))
			{
				XmlDocument xmld = new XmlDocument();
				xmld.Load(globalPath);
				XmlNodeList nl = xmld.LastChild.ChildNodes;
				XmlNodeList nlHash;
				MethodInfo m;
				Type t;
				Hashtable htTemp;

				if (nl == null || nl.Count==0)
				{
					throw new InvalidFileException("Preference file "+globalPath+" did not contain any preferences");
				}

				for (int i = 0; i<nl.Count; i++)
				{
					t = Type.GetType(nl[i].Attributes["type"].Value, true);

					if (t!=stringType)
					{
						if (t==hashtableType)
						{
							htTemp = new Hashtable();
							nlHash = nl[i].ChildNodes;

							for (int j = 0; j<nlHash.Count; j++)
							{
								if (nlHash[j].NodeType.GetType()==typeof(XmlElement))
								{
									t = Type.GetType(nlHash[j].Attributes["type"].Value, true);
									m = t.GetMethod("Parse", new Type[]{stringType});
									htTemp[nlHash[j].Attributes["key"].Value] = m.Invoke(null, new object[]{nlHash[j].InnerText});
								}
							}							

							htGlobal[nl[i].Attributes["id"].Value] = htTemp;
						}
						else if (t.IsEnum)
						{
							htGlobal[nl[i].Attributes["id"].Value] = Enum.Parse(t, nl[i].InnerText, true);
						}
						else
						{
							m = t.GetMethod("Parse", new Type[]{stringType});
							htGlobal[nl[i].Attributes["id"].Value] = m.Invoke(null, new object[]{nl[i].InnerText});
						}
					}
					else
					{
						htGlobal[nl[i].Attributes["id"].Value] = nl[i].InnerText;
					}
				}
				
				LoadLocalPreferences();
			}
			else
			{
				throw new FileNotFoundException("Could not find default preferences at "+globalPath);
			}
		}
		
		private void LoadLocalPreferences()
		{
			string path = Path.Combine(Constants.UserDataPath, "preferences.xml");

			if (File.Exists(path))
			{
				XmlDocument xmld = new XmlDocument();
				xmld.Load(path);
				XmlNodeList nl = xmld.LastChild.ChildNodes;
				XmlNodeList nlHash;
				MethodInfo m;
				Type t;
				Hashtable htTemp = new Hashtable();

				nl = xmld.LastChild.ChildNodes;

				for (int i = 0; i<nl.Count; i++)
				{
					if (!htGlobal.ContainsKey(nl[i].Attributes["id"].Value))
					{
						throw new InvalidFileException("User preferences file contains a value for key \""+nl[i].Attributes["id"].Value+"\" which is not contained in the main preferences");
					}

					t = Type.GetType(nl[i].Attributes["type"].Value, true);
					if (t!=stringType)
					{
						if (t==hashtableType)
						{
							htTemp = new Hashtable();
							nlHash = nl[i].ChildNodes;
							Hashtable htTempInner = new Hashtable();

							for (int j = 0; j<nlHash.Count; j++)
							{
								if (nlHash[j].NodeType==XmlNodeType.Element)
								{
									t = Type.GetType(nlHash[j].Attributes["type"].Value, true);
									m = t.GetMethod("Parse", new Type[]{stringType});
									htTempInner[nlHash[j].Attributes["key"].Value] = m.Invoke(null, new object[]{nlHash[j].InnerText});
								}
							}							

							htTemp[nl[i].Attributes["id"].Value] = htTempInner;
						}
						else if (t.IsEnum)
						{
							htTemp[nl[i].Attributes["id"].Value] = Enum.Parse(t, nl[i].InnerText, true);
						}
						else
						{
							m = t.GetMethod("Parse", new Type[]{stringType});
							
							if (m!=null)
							{
								htTemp[nl[i].Attributes["id"].Value] = m.Invoke(null, new object[]{nl[i].InnerText});
							}
						}
					}
					else
					{
						htTemp[nl[i].Attributes["id"].Value] = nl[i].InnerText;
					}
				}

				htLocal = htTemp;
			}
		}

		public void ReloadPreferences()
		{
			htLocal.Clear();
			LoadLocalPreferences();
		}

		public object this[string key]
		{
			get { return this[key, true]; }

			set
			{
				if (!htGlobal.ContainsKey(key))
				{
					throw new InvalidOperationException("Preference must already exist in the Global Preferences");
				}

				if (htGlobal[key].GetType()!=value.GetType())
				{
					throw new InvalidOperationException("Preferences must be set with an object of the same type as the existing preference");
				}

				if (value is Hashtable)
				{
					throw new InvalidOperationException("Hashtables in Preferences cannot be set, they can only be added to or removed from.");
				}

				if (htGlobal[key].Equals(value))
				{
					if (htLocal.ContainsKey(key))
					{
						htLocal.Remove(key);
						modified = true;
					}
				}
				else if (!((htLocal[key]==null && value==null) || value.Equals(htLocal[key])))
				{
					htLocal[key] = value;
					modified = true;
				}
				//else nothing actually needs modifying
			}
		}
		
		public object this[string key, bool errorOnNoVal]
		{
			get
			{
				if (htLocal.ContainsKey(key))
				{
					return htLocal[key];
				}
				else if (htGlobal.ContainsKey(key))
				{
					if (htGlobal[key] is Hashtable)
					{
						htLocal[key] = ((Hashtable)htGlobal[key]).Clone();
						return htLocal[key];
					}
					else
					{
						return htGlobal[key];
					}
				}
				else if (errorOnNoVal)
				{
					throw new InvalidOperationException("Key \""+key+"\" was not associated with a preference value");
				}
								
				return null;
			}
		}
		
		public bool GetBooleanProperty(string key)
		{
			object obj = this[key, false];
			bool val = false;
			
			if (obj is bool)
			{
				obj = (bool)obj;
			}
			
			return val;
		}
		
		public string GetStringProperty(string key)
		{
			object obj = this[key, false];
			string str = null;
			
			if (obj is String)
			{
				str = (String)obj;
			}
			
			return str;
		}
		
		//public String

		public bool IsModified()
		{
			return modified;
		}

		public void Save()
		{
			string prefPath = Path.Combine(Constants.UserDataPath, "preferences.xml");

			if (htLocal.Count>0)
			{
				XmlDocument xmld = new XmlDocument();
				xmld.LoadXml("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>"+
					"<!DOCTYPE prefs["+
					"<!ELEMENT preferences (preferece*)> "+
					"<!ELEMENT preference (CDATA|prefsub)> "+
					"<!ELEMENT prefsub (CDATA)> "+
					"<!ATTLIST preference id ID #REQUIRED>"+
					"<!ATTLIST preference type CDATA #REQUIRED>"+
					"<!ATTLIST prefsub key CDATA #REQUIRED>"+
					"<!ATTLIST prefsub type CDATA #REQUIRED>"+
					"]>"+"<preferences></preferences>");
				XmlNode xmln = xmld.LastChild;
				XmlNode pref;
				XmlAttribute attr;
				XmlNode prefSub;
				XmlAttribute attrSub;
				object o;
				Hashtable htTemp;
				Hashtable htGlobalTemp;

				foreach (string key in htLocal.Keys)
				{
					pref = xmld.CreateNode(XmlNodeType.Element, "preference","");
					attr = xmld.CreateAttribute("id");
					attr.Value = key;
					pref.Attributes.Append(attr);
					attr = xmld.CreateAttribute("type");
					o = htLocal[key];

					attr.Value = o.GetType().AssemblyQualifiedName;					

					if (o.GetType().ToString() == "System.Collections.Hashtable")
					{
						htTemp = (Hashtable)o;
						htGlobalTemp = (Hashtable)htGlobal[key];

						foreach(object subkey in htTemp.Keys)
						{
							if (!htGlobalTemp.ContainsKey(subkey) || !htGlobalTemp[subkey].Equals(htTemp[subkey]))
							{
								prefSub = xmld.CreateNode(XmlNodeType.Element, "prefsub", "");
								attrSub = xmld.CreateAttribute("key");
								attrSub.Value = subkey.ToString();
								prefSub.Attributes.Append(attrSub);
								attrSub = xmld.CreateAttribute("type");
								attrSub.Value = htTemp[subkey].GetType().AssemblyQualifiedName;
								prefSub.Attributes.Append(attrSub);
								prefSub.InnerText = htTemp[subkey].ToString();
								pref.AppendChild(prefSub);
							}
						}
					}
					else
					{
						pref.InnerText = o.ToString();
					}

					pref.Attributes.Append(attr);
					xmln.AppendChild(pref);
				}
				
				if (!Directory.Exists(Constants.UserDataPath))
				{
					Directory.CreateDirectory(Constants.UserDataPath);
				}

				xmld.Save(prefPath);
			}
			else if (File.Exists(prefPath))
			{
				File.Delete(prefPath);
			}

			modified = false;
		}
	}
}
