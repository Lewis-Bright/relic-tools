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
	public class ChunkyDataFBIF : ChunkyData
	{
		string pluginName, username, burntime;
		int pluginVer;

		public ChunkyDataFBIF(int version_in, string name_in, byte[] innerData):base("FBIF", version_in, name_in)
		{
			int pos = 0;
			int length = innerData[0]+(innerData[1]<<8)+(innerData[2]<<16)+(innerData[3]<<24);
			pos = 4;
			pluginName = ByteArrayToTextString(innerData, 4, length);
			pos += length;
			pluginVer = innerData[pos]+(innerData[pos+1]<<8)+(innerData[pos+2]<<16)+(innerData[pos+3]<<24);
			pos+=4;
			length = innerData[pos]+(innerData[pos+1]<<8)+(innerData[pos+2]<<16)+(innerData[pos+3]<<24);
			pos+=4;
			username = ByteArrayToTextString(innerData, pos, length);
			pos += length;
			length = innerData[pos]+(innerData[pos+1]<<8)+(innerData[pos+2]<<16)+(innerData[pos+3]<<24);
			pos+=4;
			burntime = ByteArrayToTextString(innerData, pos, length);
		}

		public override string GetDisplayDetails()
		{		
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Plugin name:\t\t"+pluginName+Environment.NewLine+
				"Plugin version:\t\t"+pluginVer+Environment.NewLine+
				"Username:\t\t"+username+Environment.NewLine+
				"Burn time:\t\t"+burntime;
		}

		public override int DataLength
		{
			get
			{
				return 16+pluginName.Length+username.Length+burntime.Length;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[DataLength];
			int pos = 0;
			int temp;

			temp = pluginName.Length;
			data[pos++] = (byte)temp;
			data[pos++] = (byte)(temp>>8);
			data[pos++] = (byte)(temp>>16);
			data[pos++] = (byte)(temp>>24);

			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			
			enc.GetBytes(pluginName).CopyTo(data,pos);
			pos += temp;

			data[pos++] = (byte)pluginVer;
			data[pos++] = (byte)(pluginVer>>8);
			data[pos++] = (byte)(pluginVer>>16);
			data[pos++] = (byte)(pluginVer>>24);

			temp = username.Length;
			data[pos++] = (byte)temp;
			data[pos++] = (byte)(temp>>8);
			data[pos++] = (byte)(temp>>16);
			data[pos++] = (byte)(temp>>24);
			
			enc.GetBytes(username).CopyTo(data,pos);
			pos += temp;

			temp = burntime.Length;
			data[pos++] = (byte)temp;
			data[pos++] = (byte)(temp>>8);
			data[pos++] = (byte)(temp>>16);
			data[pos++] = (byte)(temp>>24);
			
			enc.GetBytes(burntime).CopyTo(data,pos);
			pos += temp;

			return data;
		}

        
	}
}
