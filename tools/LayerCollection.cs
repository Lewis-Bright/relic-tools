// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for LayerCollection.
	/// </summary>
	public class LayerCollection
	{
		private byte[][] layers;
		private int layerSize;
		public LayerCollection(int layerSize_in)
		{
			layers = new byte[6][];
			layerSize = layerSize_in;

			layers[0] = new byte[layerSize];
			layers[1] = new byte[layerSize];
			layers[2] = new byte[layerSize];
			layers[3] = new byte[layerSize];
			layers[4] = new byte[layerSize];
			layers[5] = new byte[layerSize];
		}

		public byte[] this[int key]
		{
			get
			{
				return layers[key];
			}

			set
			{
				if (value.Length!=layerSize)
				{
					throw new InvalidOperationException("Data in layer collection must be the same size: "+layerSize.ToString()+" bytes");
				}

				layers[key] = value;
			}
		}

		public byte[] this[PTLD_Layers key]
		{
			get
			{
				return layers[(int)key];
			}

			set
			{
				if (value.Length!=layerSize)
				{
					throw new InvalidOperationException("Data in layer collection must be the same size: "+layerSize.ToString()+" bytes");
				}

				layers[(int)key] = value;
			}
		}
	}
}
