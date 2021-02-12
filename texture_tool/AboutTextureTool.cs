// This file is a part of the Texture Tool program and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace IBBoard.Relic.TextureTool
{
	/// <summary>
	/// Summary description for AboutTextureTool.
	/// </summary>
	public class AboutTextureTool : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button bttnClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutTextureTool()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			string version = Application.ProductVersion.Substring(0, Application.ProductVersion.LastIndexOf('.'));

			if (version.EndsWith(".0"))
			{
				version = version.Substring(0, version.Length-2);
			}

			label2.Text = "Dawn of War Texture Tool v"+version;
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
			this.bttnClose = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bttnClose
			// 
			this.bttnClose.Location = new System.Drawing.Point(200, 120);
			this.bttnClose.Name = "bttnClose";
			this.bttnClose.TabIndex = 0;
			this.bttnClose.Text = "Close";
			this.bttnClose.Click += new System.EventHandler(this.bttnClose_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(272, 32);
			this.label1.TabIndex = 1;
			this.label1.Text = "Created by IBBoard  for Skins @ Hive World Terra (http://skins.hiveworldterra.co." +
				"uk)";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(264, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Dawn of War Texture Tool v1.6";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(272, 32);
			this.label3.TabIndex = 3;
			this.label3.Text = "WTP, RSH and RTX Compiler and Extractor, and DDS to TGA and TGA to DDS converter";
			// 
			// AboutTextureTool
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(282, 145);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.bttnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "AboutTextureTool";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion

		private void bttnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
