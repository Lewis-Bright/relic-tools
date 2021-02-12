// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataDATA.
	/// </summary>
	public class ChunkyDataSSHR : ChunkyData
	{
		string filepath;

		public ChunkyDataSSHR(int version_in, string name_in, byte[] innerData_in):base("SSHR", version_in, name_in)
		{
			filepath = ByteArrayToTextString(innerData_in, 4);
		}

		public string ShaderPath
		{
			get{ return filepath; }
			set
			{
				if (value!=null)
				{
					filepath = value;
				}
				else
				{
					filepath = "";
				}
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] bytes = new byte[4+filepath.Length];
			bytes[0] = (byte)filepath.Length;
			bytes[1] = (byte)(filepath.Length>>8);
			bytes[2] = (byte)(filepath.Length>>16);
			bytes[3] = (byte)(filepath.Length>>24);
			System.Text.ASCIIEncoding.ASCII.GetBytes(filepath).CopyTo(bytes, 4);
			return bytes;
		}

		public override int DataLength
		{
			get
			{
				return filepath.Length+4;
			}
		}


		
		public override string GetDisplayDetails()
		{
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Shader path:\t"+filepath;
		}
	}
}