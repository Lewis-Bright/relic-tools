// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for FileFormats.
	/// </summary>
	public class FileFormats
	{
		public enum ImgType {DXT1DDS, DXT3DDS, DXT5DDS, TGA, Unknown}

		public static string FormatAsString(string extension)
		{
			switch(extension)
			{
				//http://www.relic.com/rdn/wiki/DOWFileFormats&v=17ol
				case "rtx": return "Relic TeXture";
				case "rsh": return "Relic SHader";
				case "wtp": return "Team Coloured Texture";
				case "ai": return "Artificial Intelligence";
				case "scar": return "SCripting At Relic";
				case "rgd": return "Compiled game data";
				case "lua": return "Generic game data";
				case "nil": return "Inheritance game data";
				case "tga": return "Targa Image";
				case "dds": return "DirectDraw Surface";
				case "screen": return "Screen layout";
				case "nis": return "Non-Interactive Sequence";
				case "rat": return "Relic Audio Tool";
				case "sgb": return "Game map";
				case "camp": return "Campaign file";
				case "turn": return "Turning behaviour";
				case "events": return "Game Event";
				case "fda": return "Fast Digital Audio";
				case "con": return "Fake file";
				case "teamcolour": return "Team colour";
				case "whm": return "Game model";
				case "whe": return "Game model animations";
				case "bmp": return "Bitmap image";
				case "jpg": return "JPEG image";
				case "jpeg": return "JPEG image";
				case "sgm": return "OE-ready model";
				case "colours": return "Named colour list";
				case "styles": return "Style sheet";
				case "fnt": return "Font file";
				case "ttf": return "True Type Font";
				case "txt": return "Plain text";
				default: return "Unknown";
			}
		}
	}
}
