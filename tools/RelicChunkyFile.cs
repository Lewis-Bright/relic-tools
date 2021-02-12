// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Text;
using IBBoard.Relic.RelicTools.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for RelicChunkyFile.
	/// </summary>
	public class RelicChunkyFile
	{
		ChunkyStructureCollection structCol;
		protected string filename;

		public RelicChunkyFile(string name, ChunkyFolder folder)
			:this(name, new RelicChunkyStructure(folder))
		{}

		public RelicChunkyFile(string name, ChunkyCollection col)
			:this(name, new RelicChunkyStructure(col))
		{}

		public RelicChunkyFile(string name, RelicChunkyStructure chunkyRoot)
			:this(name, new ChunkyStructureCollection(chunkyRoot))
		{}
        
		public RelicChunkyFile(string name, ChunkyStructureCollection col)
		{
			structCol = col;
			filename = name;

			foreach (RelicChunkyStructure strct in structCol)
			{
				strct.ParentFile = this;
			}
		}

		public ChunkyStructureCollection ChunkyStructures
		{
			get{return structCol;}
		}

		public delegate void CompilationEventDelegate(string message, bool error);

		public static event CompilationEventDelegate OnCompilationEvent;

		public static void CompilationEvent(string message, bool error)
		{
			if (OnCompilationEvent!=null)
			{
				OnCompilationEvent(message, error);
			}
			//else no-one cares
		}

		public string Name
		{
			get{return filename;}
		}

		public static void CompilationEvent(string message)
		{
			CompilationEvent(message, false);
		}
		
		public void Save(DirectoryInfo destination)
		{
			FileStream fs = null;
			BinaryWriter bw = null;

			fs = new FileInfo(destination.FullName+"/"+filename).Open(FileMode.Create, FileAccess.Write);

			try
			{
				bw = new BinaryWriter(fs);

				foreach (RelicChunkyStructure strct in ChunkyStructures)
				{
					strct.Save(bw);
				}
				bw.Close();
				CompilationEvent("Saved.\r\n");
			}
			finally
			{
				if (fs!=null)
				{
					fs.Close();
				}

				if (bw!=null)
				{
					bw.Close();
				}
			}
		}

		public string GetValidationString()
		{
			int childCount = this.ChunkyStructures.Count;

			if (childCount>1)
			{
				StringBuilder sb = new StringBuilder();
				childCount--;

				for (int i = 0; i<childCount; i++)
				{
					sb.Append(ChunkyStructures[i].GetValidationString()+" ");
				}

				sb.Append(ChunkyStructures[childCount].GetValidationString());

				return sb.ToString();
			}
			else
			{
				return ChunkyStructures[0].GetValidationString();
			}
		}
	}
}
