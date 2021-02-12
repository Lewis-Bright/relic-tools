//------------------------------------------------------------------------------
/*
	@brief		Simple Squish Wrapper, based on DdsSquish.cs from Dean Ashton's Paint.NET plugin

	@note		Copyright (c) 2006 Dean Ashton         http://www.dmashton.co.uk
	@note		Copyright (c) 2007 IBBoard				http://www.ibboard.co.uk

	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the 
	"Software"), to	deal in the Software without restriction, including
	without limitation the rights to use, copy, modify, merge, publish,
	distribute, sublicense, and/or sell copies of the Software, and to 
	permit persons to whom the Software is furnished to do so, subject to 
	the following conditions:

	The above copyright notice and this permission notice shall be included
	in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
	OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
	SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IBBoard.Graphics.SquishWrapper
{	
	public enum SquishFlags
	{
		kDxt1						= ( 1 << 0 ),		// Use DXT1 compression.
		kDxt3						= ( 1 << 1 ),		// Use DXT3 compression.
		kDxt5						= ( 1 << 2 ), 		// Use DXT5 compression.
		
		kColourClusterFit			= ( 1 << 3 ),		// Use a slow but high quality colour compressor (the default).
		kColourRangeFit				= ( 1 << 4 ),		// Use a fast but low quality colour compressor.

		kColourMetricPerceptual		= ( 1 << 5 ),		// Use a perceptual metric for colour error (the default).
		kColourMetricUniform		= ( 1 << 6 ),		// Use a uniform metric for colour error.
	
		kWeightColourByAlpha		= ( 1 << 7 ),		// Weight the colour by alpha during cluster fit (disabled by default).

		kColourIterativeClusterFit	= ( 1 << 8 ),		// Use a very slow but very high quality colour compressor.
	}

	public enum OS {
		Windows,
		Mac,
		Linux,
		Other
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class SquishWrapper
	{
		private	static bool	Is64Bit()
		{
			return ( Marshal.SizeOf( IntPtr.Zero ) == 8 ); 
		}

		// OS detection taken from https://stackoverflow.com/a/38795621/283242, because there's not a guaranteed
		// cross-library way of doing it that we can rely on.
		private static OS DetectOS() {
			string windir = Environment.GetEnvironmentVariable("windir");
			Console.WriteLine(windir);

			if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir))
			{
				Console.WriteLine("Windows");
				return OS.Windows;
			}
			else if (File.Exists(@"/proc/sys/kernel/ostype"))
			{
				Console.WriteLine("Found ostype");
				string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");
				if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("Linux");
					// Note: Android gets here too
					return OS.Linux;
				}
				else
				{
					return OS.Other;
				}
			}
			else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
			{
				// Note: iOS gets here too
				return OS.Mac;
			}
			else
			{
				return OS.Other;
			}
		}

		private sealed class SquishInterface_32
		{
            [DllImport("squishinterface_x86.dll")]
			internal static extern unsafe void CompressImage( byte* rgba, int width, int height, byte* blocks, int flags );
            [DllImport("squishinterface_x86.dll")]
			internal static	extern unsafe void DecompressImage( byte* rgba, int width, int height, byte* blocks, int flags );
		}
   
		private sealed class SquishInterface_64
		{
			[DllImport("squishinterface_x64.dll", EntryPoint="SquishCompressImage")]
			internal static extern unsafe void CompressImage( byte* rgba, int width, int height, byte* blocks, int flags );
			[DllImport("squishinterface_x64.dll", EntryPoint="SquishDecompressImage")]
			internal static	extern unsafe void DecompressImage( byte* rgba, int width, int height, byte* blocks, int flags );
		}

		private sealed class SquishInterface_x86_64
		{
			// Ugly "mangled" C++ names are ugly, but we shouldn't be rebuilding this too often
			[DllImport("libsquish.so")]
			internal static extern unsafe void CompressImage( byte* rgba, int width, int height, byte* blocks, int flags );
			[DllImport("libsquish.so")]
			internal static	extern unsafe void DecompressImage( byte* rgba, int width, int height, byte* blocks, int flags );
		}

		private static unsafe void	CallCompressImage( byte[] rgba, int width, int height, byte[] blocks, int flags )
		{
			fixed ( byte* pRGBA = rgba )
			{
				fixed ( byte* pBlocks = blocks )
				{
					OS curr_os = DetectOS();
					if (curr_os == OS.Windows)
					{
						if ( Is64Bit() )
						{
							SquishInterface_64.CompressImage( pRGBA, width, height, pBlocks, flags );
						}
						else
						{
							SquishInterface_32.CompressImage( pRGBA, width, height, pBlocks, flags );
						}
					}
					else if (curr_os == OS.Linux)
					{
						if ( Is64Bit() )
						{
							SquishInterface_x86_64.CompressImage( pRGBA, width, height, pBlocks, flags );
						}
					}
				}
			}
		}

		// ---------------------------------------------------------------------------------------
		//	CompressImage
		// ---------------------------------------------------------------------------------------
		//
		//	Params
		//		pixelData		:	Source byte array containing 32-bit RGBA pixel data
		//		width			:	Width of the image to be compressed
		//		height			:	Height of the image to be compressed
		//		flags			:	Flags for squish compression control
		//
		//	Return	
		//		blockData		:	Array of bytes containing compressed blocks
		//
		// ---------------------------------------------------------------------------------------

		public static byte[] CompressImage(byte[] pixelData, int width, int height, int squishFlags)
		{
			// Compute size of compressed block area, and allocate 
			int blockCount = ( ( width + 3 )/4 ) * ( ( height + 3 )/4 );
			int blockSize = ( ( squishFlags & ( int )SquishFlags.kDxt1 ) != 0 ) ? 8 : 16;

			// Allocate room for compressed blocks
			byte[]	blockData		= new byte[ blockCount * blockSize ];
	
			// Invoke squish::CompressImage() with the required parameters
			CallCompressImage( pixelData, width, height, blockData, squishFlags );
				
			// Return our block data to caller..
			return	blockData;	
		}
	}
}
