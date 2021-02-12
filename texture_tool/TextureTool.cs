// This file is a part of the Texture Tool program and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using IBBoard.Relic.RelicTools;
using IBBoard;
using IBBoard.Graphics;
using IBBoard.Graphics.OpenILPort;

namespace IBBoard.Relic.TextureTool
{
	/// <summary>
	/// Summary description for TextureTool.
	/// </summary>
	public class TextureTool : System.Windows.Forms.Form
	{
		private Preferences pref;

		private System.Windows.Forms.Button bttnMakeWTP;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Button bttnExtractWTP;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem miFile;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.Button bttnMakeRSH;
		private System.Windows.Forms.Button bttnMakeRTX;
		private System.Windows.Forms.Button bttnExtractRSH;
		private System.Windows.Forms.Button bttnExtractRTX;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem miBasicMode;
		private System.Windows.Forms.MenuItem miAdvancedMode;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem miOptions;
		private System.Windows.Forms.Button bttnCompileTGA;
		private System.Windows.Forms.Button bttnCompileDDS;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button bttnDDsToTGA;
		private System.Windows.Forms.Button bttnHardCodeRSH;
		private System.ComponentModel.IContainer components;

		private char folderSlash = Path.DirectorySeparatorChar;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem miErrorDetails;

		private string errorDetails = "";

		public TextureTool()
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				pref = new Preferences("TextureTool");
				InitializeComponent();			
				this.miBasicMode.Checked = (bool)pref["settingBasic"];
				this.miAdvancedMode.Checked = !this.miBasicMode.Checked;
				SetModeLayout();
				WTPFile.OnCompilationEvent+=new IBBoard.Relic.RelicTools.RelicChunkyFile.CompilationEventDelegate(WTPFile_OnCompilationEvent);
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Major error: "+ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			try
			{
				Application.EnableVisualStyles();
				Application.Run(new TextureTool());
			}
			catch(Exception ex)
			{
				MessageBox.Show("Major error: "+ex.Message, "Unexpected Error");
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TextureTool));
			this.bttnMakeWTP = new System.Windows.Forms.Button();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.bttnExtractWTP = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.miFile = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.miBasicMode = new System.Windows.Forms.MenuItem();
			this.miAdvancedMode = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.miOptions = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.bttnMakeRSH = new System.Windows.Forms.Button();
			this.bttnMakeRTX = new System.Windows.Forms.Button();
			this.bttnExtractRSH = new System.Windows.Forms.Button();
			this.bttnExtractRTX = new System.Windows.Forms.Button();
			this.bttnCompileTGA = new System.Windows.Forms.Button();
			this.bttnCompileDDS = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.bttnDDsToTGA = new System.Windows.Forms.Button();
			this.bttnHardCodeRSH = new System.Windows.Forms.Button();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.miErrorDetails = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// bttnMakeWTP
			// 
			this.bttnMakeWTP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnMakeWTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnMakeWTP.Location = new System.Drawing.Point(8, 248);
			this.bttnMakeWTP.Name = "bttnMakeWTP";
			this.bttnMakeWTP.Size = new System.Drawing.Size(128, 23);
			this.bttnMakeWTP.TabIndex = 0;
			this.bttnMakeWTP.Text = "Make WTP";
			this.toolTip.SetToolTip(this.bttnMakeWTP, "Make a collection of TGA files in to a WTP file");
			this.bttnMakeWTP.Click += new System.EventHandler(this.bttnMakeWTP_Click);
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.Location = new System.Drawing.Point(8, 0);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ReadOnly = true;
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(404, 244);
			this.tbOutput.TabIndex = 1;
			this.tbOutput.TabStop = false;
			this.tbOutput.Text = "";
			// 
			// bttnExtractWTP
			// 
			this.bttnExtractWTP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnExtractWTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnExtractWTP.Location = new System.Drawing.Point(8, 280);
			this.bttnExtractWTP.Name = "bttnExtractWTP";
			this.bttnExtractWTP.Size = new System.Drawing.Size(128, 23);
			this.bttnExtractWTP.TabIndex = 2;
			this.bttnExtractWTP.Text = "Extract WTP";
			this.toolTip.SetToolTip(this.bttnExtractWTP, "Extract a collection of TGA files from a WTP file");
			this.bttnExtractWTP.Click += new System.EventHandler(this.bttnExtractWTP_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miFile,
																					  this.menuItem4,
																					  this.menuItem2});
			// 
			// miFile
			// 
			this.miFile.Index = 0;
			this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.menuItem1});
			this.miFile.Text = "&File";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "&Quit";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miBasicMode,
																					  this.miAdvancedMode,
																					  this.menuItem5,
																					  this.miOptions});
			this.menuItem4.Text = "&Edit";
			// 
			// miBasicMode
			// 
			this.miBasicMode.Checked = true;
			this.miBasicMode.Index = 0;
			this.miBasicMode.RadioCheck = true;
			this.miBasicMode.Text = "&Basic Mode";
			this.miBasicMode.Click += new System.EventHandler(this.miBasicMode_Click);
			// 
			// miAdvancedMode
			// 
			this.miAdvancedMode.Index = 1;
			this.miAdvancedMode.RadioCheck = true;
			this.miAdvancedMode.Text = "&Advanced Mode";
			this.miAdvancedMode.Click += new System.EventHandler(this.miAdvancedMode_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "-";
			// 
			// miOptions
			// 
			this.miOptions.Index = 3;
			this.miOptions.Text = "&Options";
			this.miOptions.Click += new System.EventHandler(this.miOptions_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.miErrorDetails,
																					  this.menuItem6,
																					  this.menuItem3});
			this.menuItem2.Text = "&Help";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "&About";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// bttnMakeRSH
			// 
			this.bttnMakeRSH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnMakeRSH.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnMakeRSH.Location = new System.Drawing.Point(144, 248);
			this.bttnMakeRSH.Name = "bttnMakeRSH";
			this.bttnMakeRSH.Size = new System.Drawing.Size(128, 23);
			this.bttnMakeRSH.TabIndex = 4;
			this.bttnMakeRSH.Text = "Make RSH";
			this.toolTip.SetToolTip(this.bttnMakeRSH, "Make a DDS file in to a RSH file");
			this.bttnMakeRSH.Click += new System.EventHandler(this.bttnMakeRSH_Click);
			// 
			// bttnMakeRTX
			// 
			this.bttnMakeRTX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnMakeRTX.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnMakeRTX.Location = new System.Drawing.Point(280, 248);
			this.bttnMakeRTX.Name = "bttnMakeRTX";
			this.bttnMakeRTX.Size = new System.Drawing.Size(128, 23);
			this.bttnMakeRTX.TabIndex = 5;
			this.bttnMakeRTX.Text = "Make RTX";
			this.toolTip.SetToolTip(this.bttnMakeRTX, "Make a DDS file in to an RTX file");
			this.bttnMakeRTX.Click += new System.EventHandler(this.bttnMakeRTX_Click);
			// 
			// bttnExtractRSH
			// 
			this.bttnExtractRSH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnExtractRSH.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnExtractRSH.Location = new System.Drawing.Point(144, 280);
			this.bttnExtractRSH.Name = "bttnExtractRSH";
			this.bttnExtractRSH.Size = new System.Drawing.Size(128, 23);
			this.bttnExtractRSH.TabIndex = 6;
			this.bttnExtractRSH.Text = "Extract RSH";
			this.toolTip.SetToolTip(this.bttnExtractRSH, "Extract a DDS file from an RSH file");
			this.bttnExtractRSH.Click += new System.EventHandler(this.bttnExtractRSH_Click);
			// 
			// bttnExtractRTX
			// 
			this.bttnExtractRTX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnExtractRTX.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnExtractRTX.Location = new System.Drawing.Point(280, 280);
			this.bttnExtractRTX.Name = "bttnExtractRTX";
			this.bttnExtractRTX.Size = new System.Drawing.Size(128, 23);
			this.bttnExtractRTX.TabIndex = 7;
			this.bttnExtractRTX.Text = "Extract RTX";
			this.toolTip.SetToolTip(this.bttnExtractRTX, "Extract a DDS file from an RTX file");
			this.bttnExtractRTX.Click += new System.EventHandler(this.bttnExtractRTX_Click);
			// 
			// bttnCompileTGA
			// 
			this.bttnCompileTGA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnCompileTGA.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnCompileTGA.Location = new System.Drawing.Point(8, 312);
			this.bttnCompileTGA.Name = "bttnCompileTGA";
			this.bttnCompileTGA.Size = new System.Drawing.Size(200, 23);
			this.bttnCompileTGA.TabIndex = 8;
			this.bttnCompileTGA.Text = "Compile TGA";
			this.toolTip.SetToolTip(this.bttnCompileTGA, "Compile a single TGA from team colouring and a WTP file");
			this.bttnCompileTGA.Click += new System.EventHandler(this.bttnCompileTGA_Click);
			// 
			// bttnCompileDDS
			// 
			this.bttnCompileDDS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnCompileDDS.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnCompileDDS.Location = new System.Drawing.Point(212, 344);
			this.bttnCompileDDS.Name = "bttnCompileDDS";
			this.bttnCompileDDS.Size = new System.Drawing.Size(196, 23);
			this.bttnCompileDDS.TabIndex = 9;
			this.bttnCompileDDS.Text = "TGA -> DDS";
			this.toolTip.SetToolTip(this.bttnCompileDDS, "Convert a TGA file to a DDS file");
			this.bttnCompileDDS.Click += new System.EventHandler(this.bttnCompileDDS_Click);
			// 
			// bttnDDsToTGA
			// 
			this.bttnDDsToTGA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnDDsToTGA.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnDDsToTGA.Location = new System.Drawing.Point(8, 344);
			this.bttnDDsToTGA.Name = "bttnDDsToTGA";
			this.bttnDDsToTGA.Size = new System.Drawing.Size(200, 23);
			this.bttnDDsToTGA.TabIndex = 10;
			this.bttnDDsToTGA.Text = "DDS -> TGA";
			this.toolTip.SetToolTip(this.bttnDDsToTGA, "Convert a DDS file to a TGA");
			this.bttnDDsToTGA.Click += new System.EventHandler(this.bttnDDsToTGA_Click);
			// 
			// bttnHardCodeRSH
			// 
			this.bttnHardCodeRSH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bttnHardCodeRSH.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnHardCodeRSH.Location = new System.Drawing.Point(212, 312);
			this.bttnHardCodeRSH.Name = "bttnHardCodeRSH";
			this.bttnHardCodeRSH.Size = new System.Drawing.Size(196, 23);
			this.bttnHardCodeRSH.TabIndex = 11;
			this.bttnHardCodeRSH.Text = "Enable/Disable Teamcolouring";
			this.toolTip.SetToolTip(this.bttnHardCodeRSH, "Modify an RSH file to load/not load team colouring");
			this.bttnHardCodeRSH.Click += new System.EventHandler(this.bttnHardCodeRSH_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.Text = "-";
			// 
			// miErrorDetails
			// 
			this.miErrorDetails.Index = 0;
			this.miErrorDetails.Text = "Error &details";
			this.miErrorDetails.Click += new System.EventHandler(this.miErrorDetails_Click);
			// 
			// TextureTool
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 377);
			this.Controls.Add(this.bttnHardCodeRSH);
			this.Controls.Add(this.bttnDDsToTGA);
			this.Controls.Add(this.bttnCompileDDS);
			this.Controls.Add(this.bttnCompileTGA);
			this.Controls.Add(this.bttnExtractRTX);
			this.Controls.Add(this.bttnExtractRSH);
			this.Controls.Add(this.bttnMakeRTX);
			this.Controls.Add(this.bttnMakeRSH);
			this.Controls.Add(this.bttnExtractWTP);
			this.Controls.Add(this.bttnMakeWTP);
			this.Controls.Add(this.tbOutput);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(424, 400);
			this.Name = "TextureTool";
			this.Text = "Dawn of War Texture Tool";
			this.ResumeLayout(false);

		}
		#endregion

		private void bttnMakeWTP_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "Texture Images (*.tga)|*.tga";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					bool error = false;

					int arrayLength = openFileDialog.FileNames.Length;
					bool [] use = new bool[arrayLength];
				
					string fileA;
					string fileB;
					int posOfDot_a, posOfDot_b;
					string fileA_Underscore, fileB_Underscore;

					for (int i = 0; i<arrayLength; i++)
					{
						fileA = openFileDialog.FileNames[i].ToLower();
						use[i] = true;
						posOfDot_a = fileA.LastIndexOf('.');
						fileA_Underscore = fileA.Substring(0, fileA.LastIndexOf('_'));

						for (int j = i+1; j<arrayLength; j++)
						{
							fileB = openFileDialog.FileNames[j].ToLower();
							posOfDot_b = fileB.LastIndexOf('.');
							fileB_Underscore = fileB.Substring(0, fileB.LastIndexOf('_'));

							//big complex OR to say "don't use it if a later file is from the same WTP"
							if (fileA_Underscore==fileB_Underscore//they're both sub-items e.g. _dirt and _primary
								|| fileA.Substring(0, posOfDot_a)==fileB_Underscore //one's the _default and the other is the _dirt
								|| fileA_Underscore==fileB.Substring(0, posOfDot_b)) //as above, but the other way around
							{
								use[i] = false;
								break;
							}
						}
					}

					//and always use the last one
					//use[arrayLength-1] = true;

					for (int i = 0; i<arrayLength; i++)
					{					
						if (!use[i])
						{
							continue;
						}
					
						try
						{
							int lastSlash = openFileDialog.FileNames[i].LastIndexOf(folderSlash);
							string fileName = openFileDialog.FileNames[i].Substring(lastSlash+1);
							AddContent("Compiling WTP for "+fileName+"\r\n");
							WTPFile file = WTPFile.Create(openFileDialog.FileNames[i]);
							file.Save(new DirectoryInfo(openFileDialog.FileNames[i].Substring(0,lastSlash)));
						}
						catch (Exception ex)
						{
							error = true;
							AddContent(ex);
						}
					}

					if (error && arrayLength>1)
					{
						AddContent("\r\nOne or more errors occured during mass compilation. Please read the output for details.\r\n\r\n");
					}

					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash));
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void bttnExtractWTP_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "WTP Textures (*.wtp)|*.wtp";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);

				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{
						try
						{
							string filename = openFileDialog.FileNames[i];
							int lastSlash = filename.LastIndexOf(folderSlash);
							string file = filename.Substring(lastSlash+1);
							string directory = filename.Substring(0,lastSlash);
							AddContent("Extracting "+file+"...");
							WTPFile wtp = (WTPFile)RelicChunkyReader.ReadChunkyFile(filename);
							wtp.SaveParts(new DirectoryInfo(directory));
							AddContent("complete\r\n\r\n");
						}
						catch (Exception ex)
						{
							AddContent(ex);
						}
					}
					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void WTPFile_OnCompilationEvent(string message, bool error)
		{
			if (error)
			{
				AddContent("ERROR: "+message+"\r\n");
			}
			else
			{
				AddContent(message+"\r\n");
			}
		}

		private void MajorError(Exception ex)
		{
			AddContent(ex);
			MessageBox.Show("Major error: "+ex.Message, "Unexpected Error");
		}

		private void AddContent(Exception ex)
		{
			AddContent("\r\nERROR: "+ex.Message+"\r\n");

			if (errorDetails == "")
			{
				errorDetails = "Texture Tool v"+Application.ProductVersion+"\r\n"+Application.ExecutablePath+"\r\n\r\n";
			}

			errorDetails+= DateTime.Now.ToString() + "\r\n " + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
		}

		private void AddContent(string content)
		{
			tbOutput.Text+=content;
			tbOutput.Select(tbOutput.Text.Length,0);
			tbOutput.ScrollToCaret();
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			AboutTextureTool window = new AboutTextureTool();
			window.ShowDialog(this);
			window.Dispose();
		}

		private void bttnExtractRSH_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "RSH Texture (*.rsh)|*.rsh";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);

				openFileDialog.CheckFileExists = false;
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{
						try
						{
							string filename = openFileDialog.FileNames[i];
							int lastSlash = filename.LastIndexOf(folderSlash);
							string file = filename.Substring(lastSlash+1);
							string directory = filename.Substring(0,lastSlash);
							AddContent("Extracting "+file+"...");
							RSHFile rsh = (RSHFile)RelicChunkyReader.ReadChunkyFile(filename);
							rsh.SaveParts(new DirectoryInfo(directory));
							AddContent("complete\r\n\r\n");
						}
						catch (Exception ex)
						{
							AddContent(ex);
						}
					}
					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void bttnExtractRTX_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "RTX Texture (*.rtx)|*.rtx";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);

				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{
						try
						{
							string filename = openFileDialog.FileNames[i];
							int lastSlash = filename.LastIndexOf(folderSlash);
							string file = filename.Substring(lastSlash+1);
							string directory = filename.Substring(0,lastSlash);
							AddContent("Extracting "+file+"...");
							RTXFile rtx = (RTXFile)RelicChunkyReader.ReadChunkyFile(filename);
							rtx.SaveParts(new DirectoryInfo(directory));
							AddContent("complete\r\n\r\n");
						}
						catch (Exception ex)
						{
							AddContent(ex);
						}
					}
					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}		
		}

		private void bttnMakeRTX_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "DDS Texture Images (*.dds)|*.dds";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					bool error = false;
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{					
						try
						{
							int lastSlash = openFileDialog.FileNames[i].LastIndexOf(folderSlash);
							string fileName = openFileDialog.FileNames[i].Substring(lastSlash+1);
							AddContent("Compiling RTX for "+fileName+"\r\n");
							RTXFile file = RTXFile.Create(openFileDialog.FileNames[i]);
							file.Save(new DirectoryInfo(openFileDialog.FileNames[i].Substring(0,lastSlash)));
						}
						catch (Exception ex)
						{
							error = true;
							AddContent(ex);
						}
					}

					if (error && openFileDialog.FileNames.Length>1)
					{
						AddContent("\r\nOne or more errors occured during mass compilation. Please read the output for details.\r\n\r\n");
					}
					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void bttnMakeRSH_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "DDS Texture Images (*.dds)|*.dds";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					if (openFileDialog.FileNames.Length==1 && (bool)pref["settingBasic"] == false)
					{
						DialogResult dr = MessageBox.Show(this, "Do you want to include additional maps?", "Add maps?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

						if (dr == DialogResult.Yes)
						{
							string main = openFileDialog.FileNames[0];
							string dir = main.Substring(0, main.LastIndexOf(folderSlash));

							openFileDialog.InitialDirectory = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash));
							openFileDialog.FileName = "";
							openFileDialog.Filter = "DDS Texture Images (*.dds)|*.dds";
							openFileDialog.Multiselect = true;
							openFileDialog.ShowDialog(this);

							if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
							{
								try
								{
									string[]files = new string[openFileDialog.FileNames.Length+1];
									files[0] = main;
									for (int i = 0; i<openFileDialog.FileNames.Length; i++)
									{	
										if (openFileDialog.FileNames[i].Substring(0, openFileDialog.FileNames[i].LastIndexOf(folderSlash)) != dir)
										{
											throw new InvalidOperationException("Main file and additional maps must come from the same folder");
										}

										files[i+1] = openFileDialog.FileNames[i];
									}

									OrganiseLayers org = new OrganiseLayers(files);
									files = org.Organise(this);

									
									AddContent("Compiling RSH for "+main.Substring(main.LastIndexOf(folderSlash)+1)+" with additional maps\r\n");
									RSHFile file = RSHFile.Create(files, main);
									file.Save(new DirectoryInfo(main.Substring(0,main.LastIndexOf(folderSlash))));
								}
								catch(Exception ex)
								{
									AddContent(ex);
								}
							}
							else
							{
								AddContent("\r\nRSH compilation cancelled\r\n\r\n");
							}

						}
						else
						{				
							try
							{
								string path = openFileDialog.FileNames[0];
								int lastSlash = path.LastIndexOf(folderSlash);
								string file = path.Substring(lastSlash+1);
								AddContent("Compiling RSH for "+file+"\r\n");
								RSHFile rshfile = RSHFile.Create(path);
								rshfile.Save(new DirectoryInfo(path.Substring(0, path.LastIndexOf(folderSlash))));

							}
							catch (Exception ex)
							{
								AddContent(ex);
							}
						}
					}
					else
					{					
						bool error = false;

						for (int i = 0; i<openFileDialog.FileNames.Length; i++)
						{					
							try
							{
								string path = openFileDialog.FileNames[i];
								int lastSlash = path.LastIndexOf(folderSlash);
								string file = path.Substring(lastSlash+1);
								AddContent("Compiling RSH for "+file+"\r\n");								
								RSHFile rshfile = RSHFile.Create(path);
								rshfile.Save(new DirectoryInfo(path.Substring(0, path.LastIndexOf(folderSlash))));
							}
							catch (Exception ex)
							{
								error = true;
								AddContent(ex);
							}
						}

						if (error && openFileDialog.FileNames.Length>1)
						{
							AddContent("\r\nOne or more errors occured during mass compilation. Please read the output for details.\r\n\r\n");
						}
					}
					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void miBasicMode_Click(object sender, System.EventArgs e)
		{
			miBasicMode.Checked = true;
			miAdvancedMode.Checked = false;
			pref["settingBasic"] = true;
			SetModeLayout();
		}

		private void SetModeLayout()
		{
			int height = this.Height;

			if (miBasicMode.Checked)
			{
				bttnMakeWTP.Top = height - 112;
				bttnMakeRSH.Top = height - 112;
				bttnMakeRTX.Top = height - 112;
				bttnExtractWTP.Top = height - 80;
				bttnExtractRSH.Top = height - 80;
				bttnExtractRTX.Top = height - 80;
				tbOutput.Height = height - 120;
				bttnCompileDDS.Visible = false;
				bttnCompileTGA.Visible = false;
				bttnHardCodeRSH.Visible = false;
				bttnDDsToTGA.Visible = false;
			}
			else
			{
				bttnMakeWTP.Top = height - 180;
				bttnMakeRSH.Top = height - 180;
				bttnMakeRTX.Top = height - 180;
				bttnExtractWTP.Top = height - 148;
				bttnExtractRSH.Top = height - 148;
				bttnExtractRTX.Top = height - 148;
				tbOutput.Height = height - 184;
				bttnCompileDDS.Visible = true;
				bttnCompileTGA.Visible = true;
				bttnHardCodeRSH.Visible = true;
				bttnDDsToTGA.Visible = true;
			}
		}

		private void miAdvancedMode_Click(object sender, System.EventArgs e)
		{
			miBasicMode.Checked = false;
			miAdvancedMode.Checked = true;
			pref["settingBasic"] = false;
			SetModeLayout();
		}

		private void bttnCompileTGA_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "WTP Textures (*.wtp)|*.wtp";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					bool error = false;
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{					
						try
						{
							string file = openFileDialog.FileNames[i].Substring(openFileDialog.FileNames[i].LastIndexOf(folderSlash)+1);
							AddContent("Compiling composite TGA for "+file+"\r\n");

							WTPFile wtp = (WTPFile)RelicChunkyReader.ReadChunkyFile(openFileDialog.FileNames[i]);
			
							LayerCollection layerColours = new LayerCollection(3);

							layerColours[PTLD_Layers.Primary] = new byte[]{(byte)pref["PrimaryRed"], (byte)pref["PrimaryGreen"], (byte)pref["PrimaryBlue"]};
							layerColours[PTLD_Layers.Secondary] = new byte[]{(byte)pref["SecondaryRed"], (byte)pref["SecondaryGreen"], (byte)pref["SecondaryBlue"]};
							layerColours[PTLD_Layers.Trim] = new byte[]{(byte)pref["TrimRed"], (byte)pref["TrimGreen"], (byte)pref["TrimBlue"]};
							layerColours[PTLD_Layers.Weapon] = new byte[]{(byte)pref["WeaponRed"], (byte)pref["WeaponGreen"], (byte)pref["WeaponBlue"]};
							layerColours[PTLD_Layers.Eyes] = new byte[]{(byte)pref["EyesRed"], (byte)pref["EyesGreen"], (byte)pref["EyesBlue"]};

							

							string badgepath = "";
							if (pref["BadgeName"].ToString()!="")
							{
								badgepath = ((pref["BadgeName"].ToString()[1]==':' || pref["BadgeName"].ToString()[0]=='/')?pref["BadgeName"].ToString():pref["DoWPath"].ToString().TrimEnd(folderSlash)+folderSlash+"badges"+folderSlash+pref["BadgeName"].ToString());
							}

							string bannerpath = "";
							if (pref["BadgeName"].ToString()!="")
							{
								bannerpath = ((pref["BannerName"].ToString()[1]==':' || pref["BadgeName"].ToString()[0]=='/')?pref["BannerName"].ToString():pref["DoWPath"].ToString().TrimEnd(folderSlash)+folderSlash+"banners"+folderSlash+pref["BannerName"].ToString());
							}

							wtp.SaveCompositeTGA(new DirectoryInfo(openFileDialog.FileNames[i].Substring(0, openFileDialog.FileNames[i].LastIndexOf(folderSlash))), layerColours, badgepath, bannerpath);
							
						}
						catch (Exception ex)
						{
							error = true;
							AddContent(ex);
						}
					}

					if (error && openFileDialog.FileNames.Length>1)
					{
						AddContent("\r\nOne or more errors occured during mass compilation. Please read the output for details.\r\n\r\n");
					}

					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void miOptions_Click(object sender, System.EventArgs e)
		{
			Options opt = new Options(pref);
			opt.ShowDialog(this);
			miBasicMode.Checked = (bool)pref["settingBasic"];
			miAdvancedMode.Checked = !miBasicMode.Checked;
			SetModeLayout();
			opt.Dispose();
		}

		private void bttnCompileDDS_Click(object sender, System.EventArgs e)
		{
			try
			{
                Converter.DXTType type = Converter.DXTType.None;
				DXTFormat format = new DXTFormat();
				format.ShowDialog(this);
				type = format.ChosenFormat;
				format.Dispose();

				openFileDialog.FileName = "";
				openFileDialog.Filter = "TGA images (*.tga)|*.tga";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					BinaryReader br;
					BinaryWriter bw;
					FileInfo fileInfo;

					bool overwrite = false;
					bool prompt = true;
					bool doIt = true;
					YesNoToAllDialog yesno;		

					bool error = false;
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{					
						try
						{
							doIt = false;
							string file = openFileDialog.FileNames[i].Substring(openFileDialog.FileNames[i].LastIndexOf(folderSlash)+1);
							AddContent("Converting "+file+" to DDS file...");
							string newfile = openFileDialog.FileNames[i].Replace(".tga", ".dds");

							if (File.Exists(newfile))
							{
								if (prompt)
								{
									yesno = new YesNoToAllDialog("Overwrite file?", file.Replace(".tga", ".dds")+" already exists. Do you want to overwrite it?");
									DialogResult dr = yesno.ShowDialog();


									if (yesno.ToAll)
									{
										overwrite = (dr==DialogResult.Yes);
										prompt = false;
									}

									if (dr == DialogResult.Yes)
									{
										doIt = true;
									}
									else
									{										
										doIt = false;
									}
								}
								else if (overwrite)
								{
									doIt = true;
								}
								else
								{
									doIt = false;
								}
							}
							else
							{
								doIt = true;
							}

							if (doIt)
							{
								
				                fileInfo = new FileInfo(openFileDialog.FileNames[i]);
								br = new BinaryReader(fileInfo.OpenRead());
								byte[] tgaData = br.ReadBytes(18);

								byte idLength = tgaData[0];

								if (tgaData[2]!=0x02)
								{
									throw new IBBoard.Relic.RelicTools.Exceptions.InvalidFileException("Image must be a valid 32-bit Targa image");
								}

								//check colour depth
								if (tgaData[16]!=0x20)
								{
									throw new IBBoard.Relic.RelicTools.Exceptions.InvalidFileException("Image must be a valid 32-bit Targa image (pixel depth reads as "+tgaData[16].ToString()+"-bit)");
								}

								int width = tgaData[12]+(tgaData[13]<<8);
								int height = tgaData[14]+(tgaData[15]<<8);

								br.BaseStream.Seek(18+idLength, SeekOrigin.Begin);

								try
								{
									DDSFile dds = DDSFile.MakeFrom32bitBGRA(br.ReadBytes(width*height*4), type, width, height);
									br.Close();
									bw = new BinaryWriter(new FileInfo(openFileDialog.FileNames[i].Replace(".tga",".dds")).Open(FileMode.Create, FileAccess.Write));
									bw.Write(dds.Bytes);
									bw.Close();
								}
								finally
								{
									if (br!=null)
										br.Close();
								}							

								AddContent("Converted\r\n");
							}
							else
							{
								AddContent("Skipped\r\n");
							}
						}
						catch (Exception ex)
						{
							error = true;
							AddContent(ex);
							AddContent(ex.GetType().FullName);
							AddContent(ex.StackTrace);
						}
					}

					if (error && openFileDialog.FileNames.Length>1)
					{
						AddContent("\r\nOne or more errors occured during mass compilation. Please read the output for details.\r\n\r\n");
					}

					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}
		}

		private void bttnDDsToTGA_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "DDS images (*.dds)|*.dds";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					YesNoToAllDialog yesno;
					FileInfo fileInfo;
					BinaryReader br;
					BinaryWriter bw;

					bool overwrite = false;
					bool prompt = true;
					bool doIt = true;
				
					byte[] data;

					bool error = false;

					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{					
						try
						{
							doIt = false;
							string file = openFileDialog.FileNames[i].Substring(openFileDialog.FileNames[i].LastIndexOf(folderSlash)+1);
							string newfile = openFileDialog.FileNames[i].Replace(".dds", ".tga");
							AddContent("Converting "+file+" to TGA file...");

							if (File.Exists(newfile))
							{
								if (prompt)
								{
									yesno = new YesNoToAllDialog("Overwrite file?", file.Replace(".dds", ".tga")+" already exists. Do you want to overwrite it?");
									DialogResult dr = yesno.ShowDialog();


									if (yesno.ToAll)
									{
										overwrite = (dr==DialogResult.Yes);
										prompt = false;
									}

									if (dr == DialogResult.Yes)
									{
										doIt = true;
									}
									else
									{										
										doIt = false;
									}
								}
								else if (overwrite)
								{
									doIt = true;
								}
								else
								{
									doIt = false;
								}
							}
							else
							{
								doIt = true;
							}

							if (doIt)
							{		
									
								fileInfo = new FileInfo(openFileDialog.FileNames[i]);
								br = new BinaryReader(fileInfo.OpenRead());

								data = new DDSFile(br.ReadBytes((int)fileInfo.Length)).GetTGAData();
								br.Close();
								bw = new BinaryWriter(new FileInfo(newfile).Open(FileMode.Create, FileAccess.Write));
								bw.Write(data);
								bw.Close();
							
								AddContent("Converted\r\n");
							}
							else
							{
								AddContent("Skipped\r\n");
							}							
						}
						catch (Exception ex)
						{
							if (ex is NullReferenceException)
							{
								AddContent("Skipped (file not found)");
							}
							else{
								error = true;
								AddContent(ex);
							}
						}
					}

					if (error && openFileDialog.FileNames.Length>1)
					{
						AddContent("\r\nOne or more errors occured during mass compilation. Please read the output for details.\r\n\r\n");
					}

					pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
				}
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}		
		}

		private void bttnHardCodeRSH_Click(object sender, System.EventArgs e)
		{
			try
			{
				openFileDialog.FileName = "";
				openFileDialog.Filter = "RSH Texture (*.rsh)|*.rsh";
				openFileDialog.InitialDirectory = pref["TexturePath"].ToString();
				openFileDialog.Multiselect = true;
				openFileDialog.ShowDialog(this);

				RelicChunkyFile file;
				RSHFile rsh;
				ChunkyChunk chunk;
				ChunkyDataCHAN chan;
			
				if (openFileDialog.FileName!="" && openFileDialog.FileNames.Length!=0)
				{
					for (int i = 0; i<openFileDialog.FileNames.Length; i++)
					{	
						AddContent("Loading "+openFileDialog.FileNames[i].Substring(openFileDialog.FileNames[i].LastIndexOf(folderSlash)+1)+" ... ");
						//TODO: fix array out of bounds error when re-reading modded RSH file
						file = RelicChunkyReader.ReadChunkyFile(openFileDialog.FileNames[i]);

						if (file is RSHFile)
						{
							rsh = (RSHFile)file;
							chunk = rsh.ChunkyStructures[0].RootChunks[0];

							if (chunk.Name.StartsWith("art/"))
							{
								AddContent("stopping file loading WTP ... ");
								chunk.Name = "r"+chunk.Name.Substring(1);
								chunk = rsh.GetSHDRFolder();
								chunk.Name = "r"+chunk.Name.Substring(1);
								chunk = rsh.GetBaseImageFolder();
								chunk.Name = "r"+chunk.Name.Substring(1);
								chan = rsh.GetBaseImageDataCHAN();
								chan.ChannelName = "r"+chan.ChannelName.Substring(1);
							}
							else
							{
								AddContent("making file load WTP ... ");
								chunk.Name = "a"+chunk.Name.Substring(1);
								chunk = rsh.GetSHDRFolder();
								chunk.Name = "a"+chunk.Name.Substring(1);
								chunk = rsh.GetBaseImageFolder();
								chunk.Name = "a"+chunk.Name.Substring(1);
								chan = rsh.GetBaseImageDataCHAN();
								chan.ChannelName = "a"+chan.ChannelName.Substring(1);
							}

							rsh.Save(new DirectoryInfo(openFileDialog.FileNames[i].Substring(0,openFileDialog.FileNames[i].LastIndexOf(folderSlash))));
						}
						else
						{
							AddContent("Could not hard-code colour: "+openFileDialog.FileNames[i]+" was not an RSH file\r\n");
						}
					}
				}

				pref["TexturePath"] = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf(folderSlash)+1);
			}
			catch(Exception ex)
			{
				MajorError(ex);
			}

		}

		private void miErrorDetails_Click(object sender, System.EventArgs e)
		{
			ErrorDetails details = new ErrorDetails(errorDetails);
			details.ShowDialog(this);
		}
	}
}
