// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Text;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for RelicBinaryReader.
	/// </summary>
	public class RelicBinaryReader:BinaryReader
	{
		public RelicBinaryReader(Stream input):base(input)
		{
		}

		public override string ReadString()
		{
			return base.ReadString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public string ReadString(int start, int length)
		{
			return ReadString(start, length, false);
		}

		public string ReadString(int length)
		{
			return ReadString(-1, length, false);
		}

		public string ReadString(int length, bool unicode)
		{
			return ReadString(-1, length, unicode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="bytes"></param>
		/// <param name="unicode"></param>
		/// <returns></returns>
		public string ReadString(int start, int length, bool unicode)
		{
			if (length <= 0)
			{
				return "";
			}

			StringBuilder str = new StringBuilder();
			byte tempByte = 0;
			if (start>=0)
			{
				this.BaseStream.Seek(start, SeekOrigin.Begin);
			}
			//else read from where we are

			if (length>0)
			{
				for (long i = 0; i<length; i++)
				{
					tempByte = this.ReadByte();

					if (tempByte>=30)
						str.Append((char)tempByte);
				
					if (unicode)
						i++;
				}
			}
			else
			{
				try
				{
					do 
					{
						tempByte = this.ReadByte();

						if (tempByte>=30)
							str.Append((char)tempByte);

					} while (tempByte!=0);
				}
				catch{}
			}

			return str.ToString();
		}
	}
}
