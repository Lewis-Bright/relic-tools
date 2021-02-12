// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataCHAN.
	/// </summary>
	public class ChunkyDataCHAN : ChunkyData
	{
		public enum ChannelType{Texture, Specularity, Reflection, SelfIllum, Opacity, Unknown}
		public enum ChannelMethod{Texture = 1, Add = 2, Blend = 3, None = 0}

		private struct Coordinate
		{
			float x;
			float y;

			public Coordinate(float x_in, float y_in)
			{
				this.x = x_in;
				this.y = y_in;
			}

			public float X
			{
				get { return x; }
			}

			public float Y
			{
				get { return y; }
			}

			public override string ToString()
			{
				return x+","+y;
			}

		}

		int stringLength;
		ChannelType channel;
		ChannelMethod method;
		string channelName;
		byte[] colourMask;
		int numCoords;
		byte[] unknown, unknown2;
		Coordinate[,] coords;


		public ChunkyDataCHAN(int version_in, string name_in, byte[] innerData):base("CHAN", version_in, name_in)
		{
			stringLength = innerData[12]+(innerData[13]<<8)+(innerData[14]<<16)+(innerData[15]<<24);

			colourMask = new byte[4];
			colourMask[0] = innerData[8];
			colourMask[1] = innerData[9];
			colourMask[2] = innerData[10];
			colourMask[3] = innerData[11];

			if (innerData[4]==1)
			{
				method = ChannelMethod.Texture;
			}
			else if (innerData[4]==2)
			{
				method = ChannelMethod.Add;
			}
			else if (innerData[4]==3)
			{
				method = ChannelMethod.Blend;
			}
			else if (innerData[4]>3 || innerData[4] == 0)
			{
				method = ChannelMethod.None;
			}
			else 
			{
				throw new InvalidChunkValueException("Invalid value found for CHAN method", "Channel Method", innerData[4]);
			}

			channel = (ChannelType)innerData[0];
			channelName = ByteArrayToTextString(innerData, 16, stringLength);
			unknown = new byte[4];
			unknown[0] = innerData[stringLength+16];
			unknown[1] = innerData[stringLength+17];
			unknown[2] = innerData[stringLength+18];
			unknown[3] = innerData[stringLength+19];
			int coordPos = stringLength+20;
			numCoords = innerData[coordPos]+(innerData[coordPos+1]<<8)+(innerData[coordPos+2]<<16)+(innerData[coordPos+3]<<24);
			unknown2 = new byte[4];
			unknown2[0] = innerData[coordPos+4];
			unknown2[1] = innerData[coordPos+5];
			unknown2[2] = innerData[coordPos+6];
			unknown2[3] = innerData[coordPos+7];

			coords = new Coordinate[numCoords, 4];

			int pos = coordPos+8;

			for (int i = 0; i < numCoords && pos < innerData.Length; i++)
			{
				coords[i, 0] = new Coordinate(ByteArrayToSingle(innerData, pos), ByteArrayToSingle(innerData, pos+4));
				coords[i, 1] = new Coordinate(ByteArrayToSingle(innerData, pos+8), ByteArrayToSingle(innerData, pos+12));
				coords[i, 2] = new Coordinate(ByteArrayToSingle(innerData, pos+16), ByteArrayToSingle(innerData, pos+20));
				coords[i, 3] = new Coordinate(ByteArrayToSingle(innerData, pos+24), ByteArrayToSingle(innerData, pos+28));
				i++;
				pos = coordPos+8+(i*32);
			}
		}

		public ChannelType Channel
		{
			get{ return channel;}
		}

		public ChannelMethod Method
		{
			get
			{
				return method;
			}
		}

		public string ChannelName
		{
			get{ return channelName; }
			set
			{
				if (value!=null)
				{
					channelName = value;
				}
				else
				{
					channelName = "";
				}
			}
		}
		
		public override string GetDisplayDetails()
		{
			string coordString = "";
			
			for (int i = 0; i<coords.GetLength(0); i++)
			{
				coordString+="Coordinates "+(i+1)+":\t"+coords[i,0].ToString()+" "+coords[i,1].ToString()+" "+coords[i,2].ToString()+" "+coords[i,3].ToString()+" "+Environment.NewLine;
			}

			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Channel:\t"+channel.ToString()+Environment.NewLine+
				"Blend method:\t"+Method.ToString()+Environment.NewLine+
				"Colour Mask(?):\t"+ByteArrayToString(colourMask)+Environment.NewLine+
				"Name length:\t"+stringLength+Environment.NewLine+
				"Name:\t\t"+channelName+Environment.NewLine+
				"Channel used(?):"+(unknown[0]==1).ToString()+Environment.NewLine+
				"Num Coords:\t"+numCoords+Environment.NewLine+
				"Unknown:\t"+ByteArrayToString(unknown2)+Environment.NewLine+
				coordString;
		}

		public override int DataLength
		{
			get
			{
				return 156+ChannelName.Length;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[DataLength];
			int pos = 0;
			int temp = 0;
	
			temp = (int)channel;
			data[pos++] = (byte)temp;
			data[pos++] = (byte)(temp>>8);
			data[pos++] = (byte)(temp>>16);
			data[pos++] = (byte)(temp>>24);

			temp = (int)Method;
			if (temp>4)
			{
				temp = 0;
			}
			data[pos++] = (byte)temp;
			data[pos++] = (byte)(temp>>8);
			data[pos++] = (byte)(temp>>16);
			data[pos++] = (byte)(temp>>24);

			colourMask.CopyTo(data, pos);
			pos+=4;

			temp = channelName.Length;
			data[pos++] = (byte)temp;
			data[pos++] = (byte)(temp>>8);
			data[pos++] = (byte)(temp>>16);
			data[pos++] = (byte)(temp>>24);

			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			
			enc.GetBytes(channelName).CopyTo(data,pos);
			pos+= temp;

			unknown.CopyTo(data, pos);		
			pos+= 4;
			
			data[pos++] = (byte)numCoords;
			data[pos++] = (byte)(numCoords>>8);
			data[pos++] = (byte)(numCoords>>16);
			data[pos++] = (byte)(numCoords>>24);

			unknown2.CopyTo(data, pos);
			pos+=4;

			for (int i = 0; i<coords.GetLength(0); i++)
			{
				BitConverter.GetBytes(coords[i, 0].X).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 0].Y).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 1].X).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 1].Y).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 2].X).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 2].Y).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 3].X).CopyTo(data, pos);
				pos+=4;
				BitConverter.GetBytes(coords[i, 3].Y).CopyTo(data, pos);
				pos+=4;
			}			

			return data;
		}
	}
}
