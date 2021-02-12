// This file is a part of the Texture Tool program and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using IBBoard;
using IBBoard.Graphics;

namespace IBBoard.Relic.TextureTool
{
	/// <summary>
	/// Summary description for Options.
	/// </summary>
	public class Options : System.Windows.Forms.Form
	{
		private Preferences pref;

		private System.Windows.Forms.Button bttnImportTeamcolour;
		private System.Windows.Forms.Button bttnOK;
		private System.Windows.Forms.Button bttnCancel;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label lblDoWPath;
		private System.Windows.Forms.Button bttnDoWPath;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Button bttnPrimary;
		private System.Windows.Forms.Button bttnSecondary;
		private System.Windows.Forms.Button bttnTrim;
		private System.Windows.Forms.Button bttnWeapon;
		private System.Windows.Forms.Button bttnTrim2;
		private System.Windows.Forms.PictureBox pbBadge;
		private System.Windows.Forms.Label lblBadge;
		private System.Windows.Forms.Label lblBanner;
		private System.Windows.Forms.PictureBox pbBanner;
		private System.Windows.Forms.Button bttnSave;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox txtDoWPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button bttnTexturePath;
		private System.Windows.Forms.TextBox txtTexturePath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button bttnTeamcolourPath;
		private System.Windows.Forms.TextBox txtTeamcolourPath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton rbBasicMode;
		private System.Windows.Forms.RadioButton rbAdvancedMode;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Options(Preferences p)
		{
			pref = p;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			pbBadge.Tag = "";
			pbBanner.Tag = "";

			colorDialog.FullOpen = true;
			txtDoWPath.Text = pref["DoWPath"].ToString();
			txtTeamcolourPath.Text = pref["TeamcolourPath"].ToString();
			txtTexturePath.Text = pref["TexturePath"].ToString();

			if ((bool)pref["settingBasic"])
			{
				rbBasicMode.Checked = true;
			}
			else
			{
				rbAdvancedMode.Checked = true;
			}

			SetButtonColour(bttnPrimary, Color.FromArgb((byte)pref["PrimaryRed"], (byte)pref["PrimaryGreen"], (byte)pref["PrimaryBlue"]));
			SetButtonColour(bttnSecondary, Color.FromArgb((byte)pref["SecondaryRed"], (byte)pref["SecondaryGreen"], (byte)pref["SecondaryBlue"]));
			SetButtonColour(bttnTrim, Color.FromArgb((byte)pref["TrimRed"], (byte)pref["TrimGreen"], (byte)pref["TrimBlue"]));
			SetButtonColour(bttnWeapon, Color.FromArgb((byte)pref["WeaponRed"], (byte)pref["WeaponGreen"], (byte)pref["WeaponBlue"]));
			SetButtonColour(bttnTrim2, Color.FromArgb((byte)pref["EyesRed"], (byte)pref["EyesGreen"], (byte)pref["EyesBlue"]));
			
			string temp = pref["BadgeName"].ToString();

			if (temp.LastIndexOf(IBBoard.Constants.DirectoryChar)==-1)
			{
				temp = pref["DoWPath"].ToString().TrimEnd(IBBoard.Constants.DirectoryChar)+IBBoard.Constants.DirectoryChar+"badges"+IBBoard.Constants.DirectoryChar+pref["BadgeName"].ToString();
			}

			if (File.Exists(temp))
			{
				SetPictureBoxImage(pbBadge, temp);
			}
			
			temp = pref["BannerName"].ToString();

			if (temp.LastIndexOf(IBBoard.Constants.DirectoryChar)==-1)
			{
				temp = pref["DoWPath"].ToString().TrimEnd(IBBoard.Constants.DirectoryChar)+IBBoard.Constants.DirectoryChar+"banners"+IBBoard.Constants.DirectoryChar+pref["BannerName"].ToString();
			}

			if (File.Exists(temp))
			{
				SetPictureBoxImage(pbBanner, temp);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.bttnImportTeamcolour = new System.Windows.Forms.Button();
			this.bttnOK = new System.Windows.Forms.Button();
			this.bttnCancel = new System.Windows.Forms.Button();
			this.txtDoWPath = new System.Windows.Forms.TextBox();
			this.lblDoWPath = new System.Windows.Forms.Label();
			this.bttnDoWPath = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.pbBanner = new System.Windows.Forms.PictureBox();
			this.lblBanner = new System.Windows.Forms.Label();
			this.lblBadge = new System.Windows.Forms.Label();
			this.pbBadge = new System.Windows.Forms.PictureBox();
			this.bttnTrim2 = new System.Windows.Forms.Button();
			this.bttnWeapon = new System.Windows.Forms.Button();
			this.bttnTrim = new System.Windows.Forms.Button();
			this.bttnSecondary = new System.Windows.Forms.Button();
			this.bttnPrimary = new System.Windows.Forms.Button();
			this.bttnSave = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.rbAdvancedMode = new System.Windows.Forms.RadioButton();
			this.rbBasicMode = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.bttnTeamcolourPath = new System.Windows.Forms.Button();
			this.txtTeamcolourPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.bttnTexturePath = new System.Windows.Forms.Button();
			this.txtTexturePath = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// bttnImportTeamcolour
			// 
			this.bttnImportTeamcolour.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnImportTeamcolour.Location = new System.Drawing.Point(16, 168);
			this.bttnImportTeamcolour.Name = "bttnImportTeamcolour";
			this.bttnImportTeamcolour.Size = new System.Drawing.Size(128, 23);
			this.bttnImportTeamcolour.TabIndex = 3;
			this.bttnImportTeamcolour.Text = "Import Teamcolour";
			this.bttnImportTeamcolour.Click += new System.EventHandler(this.bttnImportTeamcolour_Click);
			// 
			// bttnOK
			// 
			this.bttnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnOK.Location = new System.Drawing.Point(368, 240);
			this.bttnOK.Name = "bttnOK";
			this.bttnOK.TabIndex = 2;
			this.bttnOK.Text = "OK";
			this.bttnOK.Click += new System.EventHandler(this.bttnOK_Click);
			// 
			// bttnCancel
			// 
			this.bttnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnCancel.Location = new System.Drawing.Point(280, 240);
			this.bttnCancel.Name = "bttnCancel";
			this.bttnCancel.TabIndex = 1;
			this.bttnCancel.Text = "Cancel";
			this.bttnCancel.Click += new System.EventHandler(this.bttnCancel_Click);
			// 
			// txtDoWPath
			// 
			this.txtDoWPath.Cursor = System.Windows.Forms.Cursors.Default;
			this.txtDoWPath.Location = new System.Drawing.Point(112, 8);
			this.txtDoWPath.Name = "txtDoWPath";
			this.txtDoWPath.Size = new System.Drawing.Size(240, 20);
			this.txtDoWPath.TabIndex = 3;
			this.txtDoWPath.TabStop = false;
			this.txtDoWPath.Text = "";
			this.txtDoWPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			this.txtDoWPath.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.txtDoWPath.Leave += new System.EventHandler(this.txtDoWPath_Leave);
			// 
			// lblDoWPath
			// 
			this.lblDoWPath.Location = new System.Drawing.Point(8, 8);
			this.lblDoWPath.Name = "lblDoWPath";
			this.lblDoWPath.TabIndex = 4;
			this.lblDoWPath.Text = "Dawn of War Path:";
			this.lblDoWPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// bttnDoWPath
			// 
			this.bttnDoWPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnDoWPath.Location = new System.Drawing.Point(360, 8);
			this.bttnDoWPath.Name = "bttnDoWPath";
			this.bttnDoWPath.Size = new System.Drawing.Size(56, 23);
			this.bttnDoWPath.TabIndex = 4;
			this.bttnDoWPath.Text = "Select";
			this.bttnDoWPath.Click += new System.EventHandler(this.bttnDoWPath_Click);
			// 
			// pbBanner
			// 
			this.pbBanner.Location = new System.Drawing.Point(296, 64);
			this.pbBanner.Name = "pbBanner";
			this.pbBanner.Size = new System.Drawing.Size(64, 96);
			this.pbBanner.TabIndex = 8;
			this.pbBanner.TabStop = false;
			this.pbBanner.Click += new System.EventHandler(this.pbBanner_Click);
			// 
			// lblBanner
			// 
			this.lblBanner.Location = new System.Drawing.Point(232, 72);
			this.lblBanner.Name = "lblBanner";
			this.lblBanner.Size = new System.Drawing.Size(64, 23);
			this.lblBanner.TabIndex = 7;
			this.lblBanner.Text = "Banner:";
			this.lblBanner.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblBadge
			// 
			this.lblBadge.Location = new System.Drawing.Point(24, 72);
			this.lblBadge.Name = "lblBadge";
			this.lblBadge.Size = new System.Drawing.Size(48, 23);
			this.lblBadge.TabIndex = 6;
			this.lblBadge.Text = "Badge:";
			this.lblBadge.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// pbBadge
			// 
			this.pbBadge.BackColor = System.Drawing.SystemColors.Control;
			this.pbBadge.Location = new System.Drawing.Point(80, 72);
			this.pbBadge.Name = "pbBadge";
			this.pbBadge.Size = new System.Drawing.Size(64, 64);
			this.pbBadge.TabIndex = 5;
			this.pbBadge.TabStop = false;
			this.pbBadge.Click += new System.EventHandler(this.pbBadge_Click);
			// 
			// bttnTrim2
			// 
			this.bttnTrim2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bttnTrim2.Location = new System.Drawing.Point(336, 16);
			this.bttnTrim2.Name = "bttnTrim2";
			this.bttnTrim2.Size = new System.Drawing.Size(72, 40);
			this.bttnTrim2.TabIndex = 4;
			this.bttnTrim2.Tag = "Eye";
			this.bttnTrim2.Text = "Trim 2/Eye";
			this.bttnTrim2.Click += new System.EventHandler(this.bttnTrim2_Click);
			// 
			// bttnWeapon
			// 
			this.bttnWeapon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bttnWeapon.Location = new System.Drawing.Point(256, 16);
			this.bttnWeapon.Name = "bttnWeapon";
			this.bttnWeapon.Size = new System.Drawing.Size(72, 40);
			this.bttnWeapon.TabIndex = 3;
			this.bttnWeapon.Tag = "Weapon";
			this.bttnWeapon.Text = "Weapon";
			this.bttnWeapon.Click += new System.EventHandler(this.bttnWeapon_Click);
			// 
			// bttnTrim
			// 
			this.bttnTrim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bttnTrim.Location = new System.Drawing.Point(176, 16);
			this.bttnTrim.Name = "bttnTrim";
			this.bttnTrim.Size = new System.Drawing.Size(72, 40);
			this.bttnTrim.TabIndex = 2;
			this.bttnTrim.Tag = "Trim";
			this.bttnTrim.Text = "Trim";
			this.bttnTrim.Click += new System.EventHandler(this.bttnTrim_Click);
			// 
			// bttnSecondary
			// 
			this.bttnSecondary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bttnSecondary.Location = new System.Drawing.Point(96, 16);
			this.bttnSecondary.Name = "bttnSecondary";
			this.bttnSecondary.Size = new System.Drawing.Size(72, 40);
			this.bttnSecondary.TabIndex = 1;
			this.bttnSecondary.Tag = "Secondary";
			this.bttnSecondary.Text = "Secondary";
			this.bttnSecondary.Click += new System.EventHandler(this.bttnSecondary_Click);
			// 
			// bttnPrimary
			// 
			this.bttnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bttnPrimary.Location = new System.Drawing.Point(16, 16);
			this.bttnPrimary.Name = "bttnPrimary";
			this.bttnPrimary.Size = new System.Drawing.Size(72, 40);
			this.bttnPrimary.TabIndex = 0;
			this.bttnPrimary.Tag = "Primary";
			this.bttnPrimary.Text = "Primary";
			this.bttnPrimary.Click += new System.EventHandler(this.bttnPrimary_Click);
			// 
			// bttnSave
			// 
			this.bttnSave.Enabled = false;
			this.bttnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnSave.Location = new System.Drawing.Point(8, 240);
			this.bttnSave.Name = "bttnSave";
			this.bttnSave.Size = new System.Drawing.Size(112, 23);
			this.bttnSave.TabIndex = 7;
			this.bttnSave.Text = "Save Preferences";
			this.bttnSave.Click += new System.EventHandler(this.bttnSave_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(432, 224);
			this.tabControl1.TabIndex = 8;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.rbAdvancedMode);
			this.tabPage1.Controls.Add(this.rbBasicMode);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.bttnTeamcolourPath);
			this.tabPage1.Controls.Add(this.txtTeamcolourPath);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.bttnTexturePath);
			this.tabPage1.Controls.Add(this.txtTexturePath);
			this.tabPage1.Controls.Add(this.lblDoWPath);
			this.tabPage1.Controls.Add(this.bttnDoWPath);
			this.tabPage1.Controls.Add(this.txtDoWPath);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(424, 198);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Game/Path Settings";
			// 
			// rbAdvancedMode
			// 
			this.rbAdvancedMode.Location = new System.Drawing.Point(200, 120);
			this.rbAdvancedMode.Name = "rbAdvancedMode";
			this.rbAdvancedMode.Size = new System.Drawing.Size(80, 24);
			this.rbAdvancedMode.TabIndex = 13;
			this.rbAdvancedMode.Text = "Advanced";
			this.rbAdvancedMode.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
			// 
			// rbBasicMode
			// 
			this.rbBasicMode.Location = new System.Drawing.Point(120, 120);
			this.rbBasicMode.Name = "rbBasicMode";
			this.rbBasicMode.Size = new System.Drawing.Size(72, 24);
			this.rbBasicMode.TabIndex = 12;
			this.rbBasicMode.Text = "Basic";
			this.rbBasicMode.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 120);
			this.label3.Name = "label3";
			this.label3.TabIndex = 11;
			this.label3.Text = "Texture Mode:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 72);
			this.label2.Name = "label2";
			this.label2.TabIndex = 10;
			this.label2.Text = "Teamcolour Path:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// bttnTeamcolourPath
			// 
			this.bttnTeamcolourPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnTeamcolourPath.Location = new System.Drawing.Point(360, 72);
			this.bttnTeamcolourPath.Name = "bttnTeamcolourPath";
			this.bttnTeamcolourPath.Size = new System.Drawing.Size(56, 23);
			this.bttnTeamcolourPath.TabIndex = 9;
			this.bttnTeamcolourPath.Text = "Select";
			this.bttnTeamcolourPath.Click += new System.EventHandler(this.bttnTeamcolourPath_Click);
			// 
			// txtTeamcolourPath
			// 
			this.txtTeamcolourPath.Cursor = System.Windows.Forms.Cursors.Default;
			this.txtTeamcolourPath.Location = new System.Drawing.Point(112, 72);
			this.txtTeamcolourPath.Name = "txtTeamcolourPath";
			this.txtTeamcolourPath.Size = new System.Drawing.Size(240, 20);
			this.txtTeamcolourPath.TabIndex = 8;
			this.txtTeamcolourPath.TabStop = false;
			this.txtTeamcolourPath.Text = "";
			this.txtTeamcolourPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			this.txtTeamcolourPath.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.txtTeamcolourPath.Leave += new System.EventHandler(this.textbox_Leave);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.TabIndex = 7;
			this.label1.Text = "Texture Path:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// bttnTexturePath
			// 
			this.bttnTexturePath.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnTexturePath.Location = new System.Drawing.Point(360, 40);
			this.bttnTexturePath.Name = "bttnTexturePath";
			this.bttnTexturePath.Size = new System.Drawing.Size(56, 23);
			this.bttnTexturePath.TabIndex = 6;
			this.bttnTexturePath.Text = "Select";
			this.bttnTexturePath.Click += new System.EventHandler(this.bttnTexturePath_Click);
			// 
			// txtTexturePath
			// 
			this.txtTexturePath.Cursor = System.Windows.Forms.Cursors.Default;
			this.txtTexturePath.Location = new System.Drawing.Point(112, 40);
			this.txtTexturePath.Name = "txtTexturePath";
			this.txtTexturePath.Size = new System.Drawing.Size(240, 20);
			this.txtTexturePath.TabIndex = 5;
			this.txtTexturePath.TabStop = false;
			this.txtTexturePath.Text = "";
			this.txtTexturePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyDown);
			this.txtTexturePath.TextChanged += new System.EventHandler(this.textbox_TextChanged);
			this.txtTexturePath.Leave += new System.EventHandler(this.textbox_Leave);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.bttnTrim2);
			this.tabPage2.Controls.Add(this.pbBadge);
			this.tabPage2.Controls.Add(this.lblBadge);
			this.tabPage2.Controls.Add(this.lblBanner);
			this.tabPage2.Controls.Add(this.bttnImportTeamcolour);
			this.tabPage2.Controls.Add(this.pbBanner);
			this.tabPage2.Controls.Add(this.bttnWeapon);
			this.tabPage2.Controls.Add(this.bttnPrimary);
			this.tabPage2.Controls.Add(this.bttnSecondary);
			this.tabPage2.Controls.Add(this.bttnTrim);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(424, 198);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Team Colouring Settings";
			// 
			// Options
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(450, 270);
			this.ControlBox = false;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.bttnSave);
			this.Controls.Add(this.bttnCancel);
			this.Controls.Add(this.bttnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MinimizeBox = false;
			this.Name = "Options";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void bttnDoWPath_Click(object sender, System.EventArgs e)
		{
			setPath(txtDoWPath);
		}

		private void bttnOK_Click(object sender, System.EventArgs e)
		{
			setPrefs();

			this.Close();
		}

		private void bttnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void bttnPrimary_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = bttnPrimary.BackColor;
			DialogResult dr = colorDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetButtonColour(bttnPrimary, colorDialog.Color);
			}
		}

		private void bttnSecondary_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = bttnSecondary.BackColor;
			DialogResult dr = colorDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetButtonColour(bttnSecondary, colorDialog.Color);
			}		
		}

		private void bttnTrim_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = bttnTrim.BackColor;
			DialogResult dr = colorDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetButtonColour(bttnTrim, colorDialog.Color);
			}
		}

		private void bttnWeapon_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = bttnWeapon.BackColor;
			DialogResult dr = colorDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetButtonColour(bttnWeapon, colorDialog.Color);
			}
		}

		private void bttnTrim2_Click(object sender, System.EventArgs e)
		{
			colorDialog.Color = bttnTrim2.BackColor;
			DialogResult dr = colorDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetButtonColour(bttnTrim2, colorDialog.Color);
			}
		}

		private void SetButtonColour(Button bttn, Color colour)
		{
			if (bttn.BackColor!=colour)
			{
				bttn.BackColor = colour;

				if (colour.R<128 && colour.G<128 && colour.B<128)
				{
					bttn.ForeColor = Color.White;
				}
				else
				{
					bttn.ForeColor = Color.Black;
				}

				if (bttn == bttnSecondary)
				{
					pbBadge.BackColor = colour;
					SetPictureBoxImage(pbBadge, null);
					pbBanner.BackColor = colour;
					SetPictureBoxImage(pbBanner, null);
				}

				setSaveEnabled();
			}
		}

		private void pbBadge_Click(object sender, System.EventArgs e)
		{
			openFileDialog.InitialDirectory = txtDoWPath.Text.TrimEnd(IBBoard.Constants.DirectoryChar)+IBBoard.Constants.DirectoryChar+"badges"+IBBoard.Constants.DirectoryChar;
			openFileDialog.Filter = "Badge Image (*.tga)|*.tga";
			openFileDialog.FileName = "";
			openFileDialog.CheckFileExists = true;
			DialogResult dr = openFileDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetPictureBoxImage(pbBadge, openFileDialog.FileName);
			}
		}

		private void pbBanner_Click(object sender, System.EventArgs e)
		{
			openFileDialog.InitialDirectory = txtDoWPath.Text.TrimEnd(IBBoard.Constants.DirectoryChar)+IBBoard.Constants.DirectoryChar+"banners"+IBBoard.Constants.DirectoryChar;
			openFileDialog.Filter = "Banner Image (*.tga)|*.tga";
			openFileDialog.FileName = "";
			openFileDialog.CheckFileExists = true;
			DialogResult dr = openFileDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				SetPictureBoxImage(pbBanner, openFileDialog.FileName);
			}
		}

		private bool SetPictureBoxImage(PictureBox box, string filepath)
		{
			if (filepath == "")
			{
				box.Image = null;
				box.Tag = "";
				return true;
			}

			if (filepath==null)
			{
				filepath = (string)box.Tag;
			}

			bool success = false;

			if (File.Exists(filepath))
			{
				try
				{
					Bitmap bmp = IBBoard.Graphics.ImageConverter.TGAtoBMP(filepath, box.BackColor);

					if (bmp.Width!=64)
					{
						MessageBox.Show(this, "The image you selected was not the correct width. Badges and banners must be 64px wide.", "Invalid image");
					}
					else if (box == pbBadge && bmp.Height!=64)
					{
						MessageBox.Show(this, "The image you selected was not the correct size. Badges must be 64px high.", "Invalid image");
					}
					else if (box == pbBanner && bmp.Height!=96)
					{
						MessageBox.Show(this, "The image you selected was not the correct size. Banners must be 96px high.", "Invalid image");
					}
					else
					{
						box.Image = bmp;
						box.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
						box.Tag = filepath;
						success = true;
					}
				}
				catch(InvalidOperationException ex)
				{
					if (box==pbBanner)
					{
						MessageBox.Show(this, ex.Message, "Invalid banner");
					}
					else
					{
						MessageBox.Show(this, ex.Message, "Invalid badge");
					}
				}
			}
			else
			{
				box.Image = null;
				box.Tag = "";

			}

			setSaveEnabled();

			return success;
		}

		private void bttnImportTeamcolour_Click(object sender, System.EventArgs e)
		{
			openFileDialog.Filter = "Team Colour files (*.teamcolour)|*.teamcolour";
			openFileDialog.FileName = "";
			openFileDialog.InitialDirectory = txtTeamcolourPath.Text;
			openFileDialog.CheckFileExists = true;
			DialogResult dr = openFileDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				string error = "";
				string temp = "";

				FileInfo file = new FileInfo(openFileDialog.FileName);
				TextReader tr = file.OpenText();
				string content = tr.ReadToEnd();
				tr.Close();
				Match match = Regex.Match(content, "\"BADGES:([\\w\\.]+)\"");

				if (match.Success)
				{
					temp = txtDoWPath.Text.TrimEnd(IBBoard.Constants.DirectoryChar)+IBBoard.Constants.DirectoryChar+"badges"+IBBoard.Constants.DirectoryChar+match.Groups[1].Value;					
					SetPictureBoxImage(pbBadge, temp);
				}
				else
				{
					SetPictureBoxImage(pbBadge, "");
				}

				match = Regex.Match(content, "\"BANNERS:([\\w\\.]+)\"");
				if (match.Success)
				{
					temp = txtDoWPath.Text.ToString().TrimEnd(IBBoard.Constants.DirectoryChar)+IBBoard.Constants.DirectoryChar+"banners"+IBBoard.Constants.DirectoryChar+match.Groups[1].Value;
					
					SetPictureBoxImage(pbBanner, temp);
				}
				else
				{
					SetPictureBoxImage(pbBanner, "");
				}

				match = Regex.Match(content, "Primary =\\s+{\\s+b = ([12]?[0-9]?[0-9]),\\s+g = ([12]?[0-9]?[0-9]),\\s+r = ([12]?[0-9]?[0-9]),");
				if (match.Success)
				{
					SetButtonColour(bttnPrimary, Color.FromArgb(byte.Parse(match.Groups[3].Value), byte.Parse(match.Groups[2].Value), byte.Parse(match.Groups[1].Value)));
				}
				else
				{
					error+= "\r\n* Primary";
				}

				match = Regex.Match(content, "Secondary =\\s+{\\s+b = ([12]?[0-9]?[0-9]),\\s+g = ([12]?[0-9]?[0-9]),\\s+r = ([12]?[0-9]?[0-9]),");
				if (match.Success)
				{
					SetButtonColour(bttnSecondary, Color.FromArgb(byte.Parse(match.Groups[3].Value), byte.Parse(match.Groups[2].Value), byte.Parse(match.Groups[1].Value)));
				}
				else
				{
					error+= "\r\n* Secondary";
				}
				
				match = Regex.Match(content, "Trim =\\s+{\\s+b = ([12]?[0-9]?[0-9]),\\s+g = ([12]?[0-9]?[0-9]),\\s+r = ([12]?[0-9]?[0-9]),");
				if (match.Success)
				{
					SetButtonColour(bttnTrim, Color.FromArgb(byte.Parse(match.Groups[3].Value), byte.Parse(match.Groups[2].Value), byte.Parse(match.Groups[1].Value)));
				}
				else
				{
					error+= "\r\n* Trim";
				}

				match = Regex.Match(content, "Weapons =\\s+{\\s+b = ([12]?[0-9]?[0-9]),\\s+g = ([12]?[0-9]?[0-9]),\\s+r = ([12]?[0-9]?[0-9]),");
				if (match.Success)
				{
					SetButtonColour(bttnWeapon, Color.FromArgb(byte.Parse(match.Groups[3].Value), byte.Parse(match.Groups[2].Value), byte.Parse(match.Groups[1].Value)));
				}
				else
				{
					error+= "\r\n* Weapon";
				}

				match = Regex.Match(content, "Eyes =\\s+{\\s+b = ([12]?[0-9]?[0-9]),\\s+g = ([12]?[0-9]?[0-9]),\\s+r = ([12]?[0-9]?[0-9]),");
				if (match.Success)
				{
					SetButtonColour(bttnTrim2, Color.FromArgb(byte.Parse(match.Groups[3].Value), byte.Parse(match.Groups[2].Value), byte.Parse(match.Groups[1].Value)));
				}
				else
				{
					error+= "\r\n* Trim 2/Eye";
				}

				if (error!="")
				{
					MessageBox.Show(this, "Invalid .teamcolour file - Error loading:"+error, "Import error");
				}
			}
		}

		private void bttnSave_Click(object sender, System.EventArgs e)
		{
			setPrefs();
			pref.Save();
			bttnSave.Enabled = false;
		}

		private void textbox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{		
			if (sender is TextBox)
			{
				if(e.KeyData == (Keys.Control|Keys.A) && e.Control)
				{
					((TextBox)sender).SelectAll();
				}
			}
		}

		private void setSaveEnabled()
		{
			if (bttnOK.Enabled && (!folderMatches(txtDoWPath.Text, "DoWPath")
				|| !folderMatches(txtTexturePath.Text, "TexturePath")
				|| !folderMatches(txtTeamcolourPath.Text, "TeamcolourPath")
				|| modeChanged() || !coloursMatch(bttnPrimary, "Primary")
				|| !coloursMatch(bttnSecondary, "Secondary") || !coloursMatch(bttnTrim, "Trim")
				|| !coloursMatch(bttnTrim2, "Eyes") || !coloursMatch(bttnWeapon, "Weapon")
				|| pbBadge.Tag.ToString()!=pref["BadgeName"].ToString()
				|| pbBanner.Tag.ToString()!=pref["BannerName"].ToString()))
			{
				bttnSave.Enabled = true;
			}
			else
			{
				bttnSave.Enabled = pref.IsModified();
			}
		}

		private void txtDoWPath_Leave(object sender, System.EventArgs e)
		{
			textbox_Leave(sender, e);

			if (!foldersExist())
			{
				MessageBox.Show(this, "Could not find 'badges' and 'banners' folder in "+txtDoWPath.Text+"\r\nPlease check that it points to your Dawn of War/Dark Crusade installation folder", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void textbox_Leave(object sender, System.EventArgs e)
		{
			if (sender is TextBox)
			{
				TextBox tb = (TextBox)sender;
				if (!tb.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
				{
					tb.Text = tb.Text + Path.DirectorySeparatorChar;
				}
			}
		}

		private bool foldersExist()
		{
			if (!txtDoWPath.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				return (Directory.Exists(txtDoWPath.Text+Path.DirectorySeparatorChar+"Badges") &&
					    Directory.Exists(txtDoWPath.Text+Path.DirectorySeparatorChar+"Banners"));
			}
			else
			{
				return (Directory.Exists(txtDoWPath.Text+"Badges") &&
					    Directory.Exists(txtDoWPath.Text+"Banners"));
			}
		}

		private bool folderMatches(string path, string prefID)
		{
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				return (path.ToLower()+Path.DirectorySeparatorChar == pref[prefID].ToString().ToLower());
			}
			else
			{
				return (path.ToLower() == pref[prefID].ToString().ToLower());
			}
		}

		private bool coloursMatch(Button bttn, string colourName)
		{
			return (bttn.BackColor.R == (byte)pref[colourName+"Red"] && bttn.BackColor.G == (byte)pref[colourName+"Green"] && bttn.BackColor.B == (byte)pref[colourName+"Blue"]);
		}

		private void setPrefs()
		{			
			pref["DoWPath"] = txtDoWPath.Text;
			pref["TeamcolourPath"] = txtTeamcolourPath.Text;
			pref["TexturePath"] = txtTexturePath.Text;

			pref["settingBasic"] = rbBasicMode.Checked;
			
			pref["PrimaryRed"] = bttnPrimary.BackColor.R;
			pref["PrimaryGreen"] = bttnPrimary.BackColor.G;
			pref["PrimaryBlue"] = bttnPrimary.BackColor.B;
			pref["SecondaryRed"] = bttnSecondary.BackColor.R;
			pref["SecondaryGreen"] = bttnSecondary.BackColor.G;
			pref["SecondaryBlue"] = bttnSecondary.BackColor.B;
			pref["TrimRed"] = bttnTrim.BackColor.R;
			pref["TrimGreen"] = bttnTrim.BackColor.G;
			pref["TrimBlue"] = bttnTrim.BackColor.B;
			pref["WeaponRed"] = bttnWeapon.BackColor.R;
			pref["WeaponGreen"] = bttnWeapon.BackColor.G;
			pref["WeaponBlue"] = bttnWeapon.BackColor.B;
			pref["EyesRed"] = bttnTrim2.BackColor.R;
			pref["EyesGreen"] = bttnTrim2.BackColor.G;
			pref["EyesBlue"] = bttnTrim2.BackColor.B;

			pref["BadgeName"] = pbBadge.Tag.ToString();
			pref["BannerName"] = pbBanner.Tag.ToString();
		}

		private void textbox_TextChanged(object sender, System.EventArgs e)
		{
			setOkayEnabled();
			setSaveEnabled();		
		}

		private void bttnTexturePath_Click(object sender, System.EventArgs e)
		{	
			setPath(txtTexturePath);
		}

		private void bttnTeamcolourPath_Click(object sender, System.EventArgs e)
		{		
			setPath(txtTeamcolourPath);
		}

		private void setPath(TextBox textbox)
		{
			folderBrowserDialog.SelectedPath = textbox.Text;
			DialogResult dr = folderBrowserDialog.ShowDialog(this);

			if (dr==DialogResult.OK)
			{
				textbox.Text = folderBrowserDialog.SelectedPath + Path.DirectorySeparatorChar;
			}	
		}

		private void setOkayEnabled()
		{
			bool enabled = true;

			if (foldersExist())
			{
				txtDoWPath.ForeColor = SystemColors.WindowText;
			}
			else
			{
				enabled = false;
				txtDoWPath.ForeColor = Color.Red;
			}

			if (Directory.Exists(txtTexturePath.Text))
			{
				txtTexturePath.ForeColor = SystemColors.WindowText;
			}
			else
			{
				enabled = false;
				txtTexturePath.ForeColor = Color.Red;
			}

			if (Directory.Exists(txtTeamcolourPath.Text))
			{
				txtTeamcolourPath.ForeColor = SystemColors.WindowText;
			}
			else
			{
				enabled = false;
				txtTeamcolourPath.ForeColor = Color.Red;
			}

			bttnOK.Enabled = enabled;
		}

		private bool modeChanged()
		{
			bool basicMode = (bool)pref["settingBasic"];
			return ((rbAdvancedMode.Checked && basicMode) || (rbBasicMode.Checked && !basicMode));
		}

		private void rbMode_CheckedChanged(object sender, System.EventArgs e)
		{
			setSaveEnabled();
		}
	}
}
