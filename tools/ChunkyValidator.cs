// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyValidator.
	/// </summary>
	public class ChunkyValidator
	{
		private class ValidatorToken
		{
			string tkn;
			int min, max;
			ValidatorToken[] children;

			public ValidatorToken(string token, int minOccurances, int maxOccurances, ValidatorToken[] childTokens)
			{
				tkn = token;
				min = minOccurances;
				max = maxOccurances;
				children = childTokens;
			}

			public ValidatorToken(string token, int minOccurances, int maxOccurances)
				:this(token, minOccurances, maxOccurances, new ValidatorToken[0]){}
			public ValidatorToken(string token, ValidatorToken[] childTokens):this(token, 1, 1, childTokens){}
			public ValidatorToken(string token):this(token, 1, 1){}

			public string Token
			{
				get{ return tkn; }
			}

			public int MinOccurances
			{
				get{ return min; }
			}

			public int MaxOccurances
			{
				get{ return max; }
			}

			public ValidatorToken[] SubTokens
			{
				get{ return children; }
				set{ children = value; }
			}
		}
		string type;
		ValidatorToken[] tokens;
		int part;

		public ChunkyValidator(FileInfo file)
		{
			type = file.Name.Substring(0, file.Name.IndexOf('.')).ToUpper();

			StreamReader sr = file.OpenText();
			//char[] chars = new char[file.Length];
			char prev = ' ';
			string chars = sr.ReadLine();
			
			while (chars!=null && chars.StartsWith("#"))
			{
				chars = sr.ReadLine();
			}

			sr.Close();

			if (chars == null)
			{
				throw new InvalidOperationException("Could not find validator string in file");
			}
			
			chars = chars.Trim();

			if (chars == "")
			{
				throw new InvalidOperationException("Could not find validator string in file");
			}

			string temp = "";
			int val = 0, min = -1, max = -1;
			ArrayList currTokens = new ArrayList();
			Stack items = new Stack();

			for (int i = 0; i<chars.Length; i++)
			{
				if (chars[i]==' ')
				{
					if (temp!="")
					{
						currTokens.Add(new ValidatorToken(temp, (min>-1?min:1), (max>-1?max:1)));
					}

					temp = "";					
					min = -1; max = -1; val = 0;
				}
				else if (chars[i]=='[')
				{
					currTokens.Add(new ValidatorToken(temp, (min>-1?min:1), (max>-1?max:1)));//create a token for a FOLD
					items.Push(currTokens);//store our current tokens
					currTokens = new ArrayList();//and clear the list
					temp = "";
				}
				else if (chars[i]==']')
				{
					ValidatorToken[] childTokens = (ValidatorToken[])currTokens.ToArray(typeof(ValidatorToken));
					currTokens = (ArrayList)items.Pop();
					((ValidatorToken)currTokens[currTokens.Count-1]).SubTokens = childTokens;
					temp = "";
				}
				//else if (chars[i]=='(')
				//{
					//min = 1; max = 1;
				//}
				else if (chars[i]==')')
				{
					if (min>-1)
					{
						max = (val > min ? val : min);
					}
					else
					{
						min = val;
						max = val;
					}
				}
				else if (Char.IsDigit(chars[i]))
				{
					if (prev=='(')
					{
						val = int.Parse(chars[i].ToString());
					}
					else if (prev=='-')
					{
						min = val;
						val = int.Parse(chars[i].ToString());
					}
					else if (Char.IsDigit(prev))
					{
						val*=10;
						val+= int.Parse(chars[i].ToString());
					}
					//else it's something it shouldn't be
				}
				else if (Char.IsLetter(chars[i]))
				{
					temp+=chars[i];
				}

				prev = chars[i];
			}

			if (prev!=']')
			{
			/*	ValidatorToken[] childTokens = (ValidatorToken[])currTokens.ToArray(typeof(ValidatorToken));
				currTokens = (ArrayList)items.Pop();
				((ValidatorToken)currTokens[currTokens.Count-1]).SubTokens = childTokens;
			}
			else
			{*/
				currTokens.Add(new ValidatorToken(temp, (min>-1?min:1), (max>-1?max:1)));
			}

			tokens = (ValidatorToken[])currTokens.ToArray(typeof(ValidatorToken));
		}

		public string FileType
		{
			get{ return type; }
		}

		public bool Validate(string validationString)
		{
			lock(this)
			{

				string[] parts = validationString.Split(' ');
				part = 0;
				int pos = 0;
				Stack layers = new Stack();
				Stack layerPositions = new Stack();

				try
				{
					while (pos < tokens.Length)
					{
						if (!validate(parts, tokens[pos]))
						{
							return false;
						}
						pos++;
					}
				}
				catch (IndexOutOfRangeException)
				{
					return false;
				}

				return true;
			}
		}

		private bool validate(string[] parts, ValidatorToken token)
		{
			int diff = 0;

			if (token.SubTokens.Length>0)
			{
				for (int j = 0; j<token.MinOccurances; j++)
				{
					if (parts[part]!=token.Token+"[")
					{
						return false;
					}
			
					part++;

					for (int k = 0; k<token.SubTokens.Length; k++)
					{
						if (!validate(parts, token.SubTokens[k]))
						{
							return false;
						}
					}

					if (parts[part]!="]")
					{
						return false;
					}

					part++;
				}

				diff = token.MaxOccurances - token.MinOccurances;

				for (int j = 0; j < diff; j++)
				{
					if (parts[part]!=token.Token+"[")
					{
						break;
					}	

					part++;				

					for (int k = 0; k<token.SubTokens.Length; k++)
					{
						if (!validate(parts, token.SubTokens[k]))
						{
							return false;
						}
					}

					if (parts[part]!="]")
					{
						return false;
					}
					
					part++;	
				}

				if (part<parts.Length && parts[part]==token.Token)
				{
					return false;
				}
			}
			else
			{
				for (int j = 0; j<token.MinOccurances; j++)
				{
					if (parts[part]!=token.Token)
					{
						return false;
					}

					part++;
				}

				diff = token.MaxOccurances - token.MinOccurances;

				for (int j = 0; j < diff; j++)
				{
					if (parts[part]!=token.Token)
					{
						break;
					}
					else
					{
						part++;
					}
				}

				if (part<parts.Length && parts[part]==token.Token)
				{
					return false;
				}
			}

			return true;
		}
	}
}
