// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;
using System.IO;
using IBBoard.Graphics;
using IBBoard.Relic.RelicTools.Exceptions;
using IBBoard.Relic.RelicTools.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for WTPFile.
	/// </summary>
	public class WTPFile:RelicChunkyFile
	{
		int width = 0;
		int height = 0;
		ChunkyDataATTR attr = null;
		
		public WTPFile(string filename, ChunkyFolder folder)
			:this(filename, new ChunkyStructureCollection(new RelicChunkyStructure(folder))){}
		
		public WTPFile(string filename, ChunkyCollection col)
			:this(filename, new ChunkyStructureCollection(new RelicChunkyStructure(col))){}

		public WTPFile(string filename, ChunkyStructureCollection col): base(filename, col)
		{
			ChunkyFolder folder = (ChunkyFolder)col[0].RootChunks[0];
			int children = folder.Children.Count;
			ChunkyChunk chunk = null;
			ChunkyDataINFOTPAT info = null;

			for (int i = 0; i<children; i++)
			{
				chunk = folder.Children[i];

				if (chunk is ChunkyDataINFOTPAT)
				{
					info = (ChunkyDataINFOTPAT)chunk;
					width = info.Width;
					height = info.Height;
				}
			}
		}

		public void SaveParts(DirectoryInfo destination)
		{
			string filenamebase = filename.Substring(0, filename.LastIndexOf('.'));

			saveFolder((ChunkyFolder)this.ChunkyStructures[0].RootChunks[0], destination, filenamebase);
		}

		private void saveFolder(ChunkyFolder folder, DirectoryInfo destination, string filenamebase)
		{
			int children = folder.Children.Count;
			ChunkyChunk chunk = null;

			for (int i = 0; i<children; i++)
			{
				chunk = folder.Children[i];

				if (chunk is ChunkyData)
				{
					((ChunkyData)chunk).Save(destination, filenamebase);
				}
				else if (chunk is ChunkyFolder)
				{
					this.saveFolder((ChunkyFolder)chunk, destination, filenamebase);
				}
				//else something is wrong!

				/*else if (chunk is ChunkyDataDATAIMAG)
				{
					((ChunkyDataDATAIMAG)chunk).Save(destination, filenamebase);
				}
				else if (chunk is ChunkyDataPTBD)
				{
					((ChunkyDataPTBD)chunk).Save(destination, filenamebase);
				}
				else if (chunk is ChunkyDataPTBN)
				{
					((ChunkyDataPTBN)chunk).Save(destination, filenamebase);
				}*/
			}
		}

		public static WTPFile Create(string filepath)
		{
			string baseFileName = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar)+1);
			string baseFileNameLower = baseFileName.ToLower();

			string directory = "";

			if (filepath.IndexOf(Path.DirectorySeparatorChar)!=-1)
			{
				directory = filepath.Substring(0, filepath.LastIndexOf(Path.DirectorySeparatorChar))+Path.DirectorySeparatorChar;
			}

			if (baseFileNameLower.EndsWith(".wtp"))
			{
				baseFileName = baseFileName.Substring(0, baseFileName.Length-4);
			}
			else if(baseFileNameLower.EndsWith(".tga"))
			{

				if (baseFileNameLower.EndsWith("_primary.tga"))
				{
					baseFileName = baseFileName.Substring(0, baseFileName.Length-12);
				}
				else if (baseFileNameLower.EndsWith("_secondary.tga"))
				{
					baseFileName = baseFileName.Substring(0, baseFileName.Length-14);
				}
				else if (baseFileNameLower.EndsWith("_weapon.tga")||baseFileNameLower.EndsWith("_banner.tga"))
				{
					baseFileName = baseFileName.Substring(0, baseFileName.Length-11);
				}
				else if (baseFileNameLower.EndsWith("_badge.tga"))
				{
					baseFileName = baseFileName.Substring(0, baseFileName.Length-10);
				}
				else if (baseFileNameLower.EndsWith("_eyes.tga") || baseFileNameLower.EndsWith("_trim.tga") || baseFileNameLower.EndsWith("_dirt.tga"))
				{
					baseFileName = baseFileName.Substring(0, baseFileName.Length-9);
				}
				else
				{					
					baseFileName = baseFileName.Substring(0, baseFileName.Length-4);
				}
			}
			else
			{
				throw new InvalidFileException("File path specified is not valid for a WTP file or one of its components");
			}

			string skinName = baseFileName.Substring(baseFileName.LastIndexOf('_')+1);//make it forwards compatible on the off-chance Relic introduce multiple textures

			ChunkyDataDATA defaultData = null;
			ChunkyData attr = null;
			FileStream fs = null;
			BinaryReader br = null;
			FileInfo file = null;
			byte [] data;

			int width = 0;
			int height = 0;

			if (File.Exists(directory+baseFileName+".tga"))
			{
				CompilationEvent("Reading "+baseFileName+".tga");
				file = new FileInfo(directory+baseFileName+".tga");
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(12,SeekOrigin.Begin);
				width = br.ReadInt16();
				height = br.ReadInt16();
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				data = br.ReadBytes((int)br.BaseStream.Length);

				defaultData = ChunkyDataDATAIMAG.CreateFromTGA(2, "", data);				
				
				br.Close();
				data = new byte[]{0x0, 0x0, 0x0, 0x0, (byte)width, (byte)(width>>8), (byte)(width>>16), (byte)(width>>24), (byte)(height), (byte)(height>>8), (byte)(height>>16), (byte)(height>>24), 0x1, 0x0, 0x0, 0x0};
				attr = new ChunkyDataUnknown("ATTR", 2, "", data);
			}
			else
			{
				throw new RelicTools.Exceptions.FileNotFoundException("WTP files must have a 32bit layer e.g. _default.tga layer");
			}

			string dirt_name = directory+baseFileName+"_Dirt.tga";

			ChunkyFolder defaultFolder = new ChunkyFolder("IMAG", 1, "TOOL:"+dirt_name);

			defaultFolder.Children.Add(attr);
			defaultFolder.Children.Add(defaultData);

			ChunkyDataPTLD primary = null;
			ChunkyDataPTLD secondary = null;
			ChunkyDataPTLD trim = null;
			ChunkyDataPTLD weapon = null;
			ChunkyDataPTLD eyes = null;
			ChunkyDataPTLD dirt = null;
			ChunkyDataPTBD badge = null;
			ChunkyDataPTBN banner = null;
			string primaryname = directory+baseFileName+"_Primary.tga";
			string secondaryname = directory+baseFileName+"_Secondary.tga";
			string trimname = directory+baseFileName+"_Trim.tga";
			string weaponname = directory+baseFileName+"_Weapon.tga";
			string eyesname = directory+baseFileName+"_Eyes.tga";
			string badgename = directory+baseFileName+"_Badge.tga";
			string bannername = directory+baseFileName+"_Banner.tga";

			//int byteRead = width*height+18;//length of TGA data plus header
			
			if (File.Exists(dirt_name))
			{
				CompilationEvent("Reading "+baseFileName+"_Dirt.tga");
				file = new FileInfo(dirt_name);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				dirt = ChunkyDataPTLD.CreateFromTGA(PTLD_Layers.Dirt, 1, "", data);
			}
			else
			{
				throw new RelicTools.Exceptions.FileNotFoundException("WTP Files must contain a dirt layer");
			}

			data = new byte[8];
			data[0] = (byte)(width);
			data[1] = (byte)(width >> 8);
			data[2] = (byte)(width >> 16);
			data[3] = (byte)(width >> 24);
			data[4] = (byte)(height);
			data[5] = (byte)(height >> 8);
			data[6] = (byte)(height >> 16);
			data[7] = (byte)(height >> 24);

			ChunkyData info = new ChunkyDataUnknown("INFO", 1, "", data);

			if (File.Exists(primaryname))
			{
				CompilationEvent("Reading "+baseFileName+"_Primary.tga");
				file = new FileInfo(primaryname);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				primary = ChunkyDataPTLD.CreateFromTGA(PTLD_Layers.Primary, 1, "", data);

				if (primary==null)
				{
					CompilationEvent("Skipped "+baseFileName+"_Primary.tga - no team colour data");
				}
			}

			if (File.Exists(secondaryname))
			{
				CompilationEvent("Reading "+baseFileName+"_Secondary.tga");
				file = new FileInfo(secondaryname);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				secondary = ChunkyDataPTLD.CreateFromTGA(PTLD_Layers.Secondary, 1, "", data);

				if (secondary==null)
				{
					CompilationEvent("Skipped "+baseFileName+"_Secondary.tga - no team colour data");
				}
			}

			if (File.Exists(trimname))
			{
				CompilationEvent("Reading "+baseFileName+"_Trim.tga");
				file = new FileInfo(trimname);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				trim = ChunkyDataPTLD.CreateFromTGA(PTLD_Layers.Trim, 1, "", data);

				if (trim==null)
				{
					CompilationEvent("Skipped "+baseFileName+"_Trim.tga - no team colour data");
				}
			}

			if (File.Exists(weaponname))
			{
				CompilationEvent("Reading "+baseFileName+"_Weapon.tga");
				file = new FileInfo(weaponname);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				weapon = ChunkyDataPTLD.CreateFromTGA(PTLD_Layers.Weapon, 1, "", data);

				if (weapon==null)
				{
					CompilationEvent("Skipped "+baseFileName+"_Weapon.tga - no team colour data");
				}
			}

			if (File.Exists(eyesname))
			{
				CompilationEvent("Reading "+baseFileName+"_Eyes.tga");
				file = new FileInfo(eyesname);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				eyes = ChunkyDataPTLD.CreateFromTGA(PTLD_Layers.Eyes, 1, "", data);

				if (eyes==null)
				{
					CompilationEvent("Skipped "+baseFileName+"_Eyes.tga - no team colour data");
				}
			}

			if (File.Exists(badgename))
			{
				CompilationEvent("Reading "+baseFileName+"_Badge.tga");
				file = new FileInfo(badgename);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				badge = ChunkyDataPTBD.CreateFromTGA(1, "", data);
			}

			if (File.Exists(bannername))
			{
				CompilationEvent("Reading "+baseFileName+"_Banner.tga");
				file = new FileInfo(bannername);
				fs = file.OpenRead();
				br = new BinaryReader(fs);
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				
				data = br.ReadBytes((int)br.BaseStream.Length);
				br.Close();

				banner = ChunkyDataPTBN.CreateFromTGA(1, "", data);
			}

			if (badge!=null && banner!=null)
			{
				if (((badge.Pos_x<=banner.Pos_x && (badge.Pos_x+badge.Width)>=banner.Pos_x)||
					(badge.Pos_x<=(banner.Pos_x+banner.Width) && (badge.Pos_x+badge.Width)>=(banner.Pos_x+banner.Width)))&&
					((badge.Pos_y<=banner.Pos_y && (badge.Pos_y+badge.Height)>=banner.Pos_y)||
					(badge.Pos_y<=(banner.Pos_y+banner.Height) && (badge.Pos_y+badge.Height)>=(banner.Pos_y+banner.Height))))
				{
					throw new InvalidFileException("Badge and banner position overlap");
				}
			}


			CompilationEvent("Compiling WTP File");
			ChunkyFolder tpat = new ChunkyFolder("TPAT", 3, skinName);
			tpat.Children.Add(info);
			tpat.Children.Add(primary);
			tpat.Children.Add(secondary);
			tpat.Children.Add(trim);
			tpat.Children.Add(weapon);
			tpat.Children.Add(eyes);
			tpat.Children.Add(dirt);
			tpat.Children.Add(defaultFolder);
			tpat.Children.Add(banner);
			tpat.Children.Add(badge);

			return new WTPFile(baseFileName+".wtp", tpat);

			//RelicChunkyFile.SaveChunky(directory+baseFileName+".wtp", tpat.GetBytes());
		}

		public byte[] CreateCompositeTGABytes(LayerCollection colours, string badgePath, string bannerPath)
		{
			if (colours[PTLD_Layers.Dirt].Length!=3)
			{
				throw new InvalidOperationException("LayerCollection 'colours' must contain RGB colour data in a 3-byte array");
			}

			FileInfo file = null;
			BinaryReader br = null;
			byte[] badge = new byte[64*64*4];
			byte[] banner = new byte[64*96*4];
			byte[] temp;

			CompilationEvent("Loading badge");
			if (File.Exists(badgePath))
			{
				file = new FileInfo(badgePath);
				br = new BinaryReader(file.OpenRead());
				temp = br.ReadBytes((int)file.Length);
				br.Close();

				if ((temp[12]+(temp[13]<<8))!=64 ||(temp[14]+(temp[15]<<8))!=64)
				{
					CompilationEvent("Invalid badge image - badge must be a 64x64 pixel 24/32-bit TGA image", true);
				}
				else if (!((temp[16]==32 || temp[16]==24) && temp[2]==2))
				{
					CompilationEvent("Invalid badge image - badge must be a 64x64 pixel 24/32-bit TGA image", true);
				}
				else
				{
					Buffer.BlockCopy(ImageConverter.TGAto32bitTGA(temp), 18, badge, 0, badge.Length);
				}
			}
			else
			{
				CompilationEvent("Invalid badge path - file must exist. Using blank badge.", true);
			}

			CompilationEvent("Loading banner");
			if (File.Exists(bannerPath))
			{
				file = new FileInfo(bannerPath);
				br = new BinaryReader(file.OpenRead());
				temp = br.ReadBytes((int)file.Length);
				br.Close();


				if ((temp[12]+(temp[13]<<8))!=64 ||(temp[14]+(temp[15]<<8))!=96)
				{
					CompilationEvent("Invalid banner image - banner must be a 64x96 pixel 24/32-bit TGA image", true);
				}
				else if (!((temp[16]==32 || temp[16]==24) && temp[2]==2))
				{
					CompilationEvent("Invalid banner image - banner must be a 64x64 pixel 24/32-bit TGA image", true);
				}
				else
				{
					Buffer.BlockCopy(ImageConverter.TGAto32bitTGA(temp), 18, banner, 0, banner.Length);
				}
			}
			else
			{
				CompilationEvent("Invalid banner path - file must exist. Using blank badge.", true);
			}

			ChunkyFolder root = (ChunkyFolder)this.ChunkyStructures[0].RootChunks[0];

			ChunkyChunk chunk = null;
			ChunkyDataPTLD ptld = null;
			ChunkyDataPTBN bannerLayer = null;
			ChunkyDataPTBD badgeLayer = null;
			ChunkyDataINFOTPAT info = (ChunkyDataINFOTPAT)root.Children[0];
			LayerCollection layers = new LayerCollection(info.Width*info.Height);
			byte[] mainImage = new byte[info.Width*info.Height];

			int badge_bottom = int.MaxValue;
			int badge_top = int.MaxValue;
			int badge_left = int.MaxValue;
			int badge_right = int.MaxValue;

			int banner_bottom = int.MaxValue;
			int banner_top = int.MaxValue;
			int banner_left = int.MaxValue;
			int banner_right = int.MaxValue;

			CompilationEvent("Loading colour layers");
			for (int i = 0; i<root.Children.Count; i++)
			{
				chunk = root.Children[i];

				if (chunk is ChunkyDataPTLD)
				{
					ptld = (ChunkyDataPTLD)root.Children[i];
					layers[(int)ptld.Layer] = ptld.GetColourBytes();
				}
				else if (chunk is ChunkyDataPTBD)
				{
					badgeLayer = (ChunkyDataPTBD)chunk;
					badge_bottom = (int)badgeLayer.Pos_y;
					badge_top = badge_bottom+(int)badgeLayer.Height;
					badge_left = (int)badgeLayer.Pos_x;
					badge_right = badge_left+(int)badgeLayer.Width;
				}
				else if (chunk is ChunkyDataPTBN)
				{
					bannerLayer = (ChunkyDataPTBN)chunk;
					banner_bottom = (int)bannerLayer.Pos_y;
					banner_top = banner_bottom+(int)bannerLayer.Height;
					banner_left = (int)bannerLayer.Pos_x;
					banner_right = banner_left+(int)bannerLayer.Width;
				}
				else if (chunk is ChunkyFolder)
				{
					mainImage = ((ChunkyData)((ChunkyFolder)chunk).Children[1]).GetDataBytes();
				}
			}

			int bytePos = 0;
			int bytePos32bit = 0;
			int bytePosBadge = 0;
			int bytePosBanner = 0;
			byte[] badgeByte;
			byte[] bannerByte;
			double ratio;
			double ratio2;
			double ratio3;
			double ratioTC;
			double tempByte = 0;
			double extra = 0;
			int badge_width = 0;
			
			if (badgeLayer != null)
			{
				badge_width = (int)badgeLayer.Width;
			}

			int banner_width = 0;
			
			if (bannerLayer!=null)
			{
				banner_width = (int)bannerLayer.Width;
			}

			CompilationEvent("Compiling TGA");
			for (int i = 0; i<info.Height; i++)
			{
				for (int j = 0; j<info.Width; j++)
				{
					bytePos = (i*info.Width)+j;
					bytePos32bit = bytePos*4;
					ratio = layers[PTLD_Layers.Dirt][bytePos]/255.0;

					if (badge_left<=j && badge_right>j && badge_bottom<=i && badge_top>i)
					{
						bytePosBadge = ((i-badge_bottom)*badge_width+(j-badge_left))*4;

						ratio2 = (badge[bytePosBadge+3]/255.0);
						badgeByte = new byte[]{badge[bytePosBadge], badge[bytePosBadge+1], badge[bytePosBadge+2]};
					}
					else
					{
						ratio2 = 0;
						badgeByte = new byte[3];
					}

					if (banner_left<=j && banner_right>j && banner_bottom<=i && banner_top>i)
					{
						bytePosBanner = ((i-banner_bottom)*banner_width+(j-banner_left))*4;

						ratio3 = (banner[bytePosBanner+3]/255.0);
						bannerByte = new byte[]{banner[bytePosBanner], banner[bytePosBanner+1], banner[bytePosBanner+2]};
					}
					else
					{
						ratio3 = 0;
						bannerByte = new byte[3];
					}

					ratioTC = 1-ratio;

					ratio2 = ratioTC*ratio2;
					ratio3 = ratioTC*ratio3;
					ratioTC = (ratioTC-ratio2-ratio3)*2;
					extra = 0;

					//TGA files run in reverse - little-endian ARGB, which ends up as BGRA in the file

					//red
					tempByte = (ratio*mainImage[bytePos32bit+2]) + (ratioTC)*((colours[PTLD_Layers.Primary][0]*layers[PTLD_Layers.Primary][bytePos]/255.0) + 
						(colours[PTLD_Layers.Secondary][0]*layers[PTLD_Layers.Secondary][bytePos]/255.0) + 
						(colours[PTLD_Layers.Weapon][0]*layers[PTLD_Layers.Weapon][bytePos]/255.0) + 
						(colours[PTLD_Layers.Trim][0]*layers[PTLD_Layers.Trim][bytePos]/255.0) + 
						(colours[PTLD_Layers.Eyes][0]*layers[PTLD_Layers.Eyes][bytePos]/255.0)) + ratio2*badgeByte[2] + ratio3*bannerByte[2];

					if (tempByte>byte.MaxValue)
					{
						//CompilationEvent("Colour overflow at X:"+j.ToString()+", Y:"+i.ToString()+". Colour will show as Magenta or other bright colour with some colour schemes.", true);
						mainImage[bytePos32bit+2] = byte.MaxValue;
						extra = (tempByte-byte.MaxValue);
					}
					else
					{
						mainImage[bytePos32bit+2] = (byte)tempByte;
					}

					//green
					tempByte = (ratio*mainImage[bytePos32bit+1]) + (ratioTC)*((colours[PTLD_Layers.Primary][1]*layers[PTLD_Layers.Primary][bytePos]/255.0) + 
						(colours[PTLD_Layers.Secondary][1]*layers[PTLD_Layers.Secondary][bytePos]/255.0) + 
						(colours[PTLD_Layers.Weapon][1]*layers[PTLD_Layers.Weapon][bytePos]/255.0) + 
						(colours[PTLD_Layers.Trim][1]*layers[PTLD_Layers.Trim][bytePos]/255.0) + 
						(colours[PTLD_Layers.Eyes][1]*layers[PTLD_Layers.Eyes][bytePos]/255.0)) + extra + ratio2*badgeByte[1] + ratio3*bannerByte[1];

					if (tempByte>byte.MaxValue)
					{
						//CompilationEvent("Colour overflow at X:"+j.ToString()+", Y:"+i.ToString()+". Colour will show as Magenta or other bright colour with some colour schemes.", true);
						mainImage[bytePos32bit+1] = byte.MaxValue;
						extra = extra + tempByte-byte.MaxValue;
						tempByte = mainImage[bytePos32bit+2]+extra;
						if (tempByte>byte.MaxValue)
						{
							tempByte = byte.MaxValue;
						}
						mainImage[bytePos32bit+2] = (byte)tempByte;
					}
					else
					{
						mainImage[bytePos32bit+1] = (byte)tempByte;
					}

					//blue
					tempByte = (ratio*mainImage[bytePos32bit]) + (ratioTC)*((colours[PTLD_Layers.Primary][2]*layers[PTLD_Layers.Primary][bytePos]/255.0) + 
						(colours[PTLD_Layers.Secondary][2]*layers[PTLD_Layers.Secondary][bytePos]/255.0) + 
						(colours[PTLD_Layers.Weapon][2]*layers[PTLD_Layers.Weapon][bytePos]/255.0) + 
						(colours[PTLD_Layers.Trim][2]*layers[PTLD_Layers.Trim][bytePos]/255.0) + 
						(colours[PTLD_Layers.Eyes][2]*layers[PTLD_Layers.Eyes][bytePos]/255.0)) + extra + ratio2*badgeByte[0] + ratio3*bannerByte[0];

					if (tempByte>byte.MaxValue)
					{
						//CompilationEvent("Colour overflow at X:"+j.ToString()+", Y:"+i.ToString()+". Colour will show as Magenta or other bright colour with some colour schemes.", true);
						mainImage[bytePos32bit] = byte.MaxValue;
						extra = extra + tempByte - byte.MaxValue;
						tempByte = mainImage[bytePos32bit+2]+extra;
						if (tempByte>byte.MaxValue)
						{
							tempByte = byte.MaxValue;
						}
						mainImage[bytePos32bit+2] = (byte)tempByte;
						tempByte = mainImage[bytePos32bit+1]+extra;
						if (tempByte>byte.MaxValue)
						{
							tempByte = byte.MaxValue;
						}
						mainImage[bytePos32bit+1] = (byte)tempByte;
					}
					else
					{
						mainImage[bytePos32bit] = (byte)tempByte;
					}
				}
			}

			return mainImage;
		}

		public void SaveCompositeTGA(DirectoryInfo destination, LayerCollection colours, string badgePath, string bannerPath)
		{
			byte[] tga = this.CreateCompositeTGABytes(colours, badgePath, bannerPath);
			
			ChunkyDataATTR attr = findFirstDataAttributes();
			
			ChunkyDataDATAIMAG data = new ChunkyDataDATAIMAG(2, "", tga);
			data.Attributes = attr;
			CompilationEvent("Saving compiled TGA");
			data.Save(destination, filename.Substring(0, filename.LastIndexOf('_')));
			CompilationEvent("Compilation Complete\r\n");
		}

		private ChunkyDataATTR findFirstDataAttributes()
		{
			if (attr == null)
			{
				ChunkyFolder root = (ChunkyFolder)this.ChunkyStructures[0].RootChunks[0];

				for (int i = 0; i<root.Children.Count; i++)
				{
					if (root.Children[i] is ChunkyFolder)
					{
						root = (ChunkyFolder)root.Children[i];
						break;
					}
				}

				if (root.Children[0] is ChunkyDataATTR)
				{
					attr = (ChunkyDataATTR)root.Children[0];
				}
			}
			
			return attr;
		}
	}
}