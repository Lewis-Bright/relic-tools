// This file is a part of the Texture Tool program and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using IBBoard.Graphics;
using IBBoard.Graphics.OpenILPort;

namespace IBBoard.Relic.TextureTool
{
	/// <summary>
	/// Summary description for DXTFormat.
	/// </summary>
	public class DXTFormat : System.Windows.Forms.Form
	{
        public Converter.DXTType ChosenFormat = Converter.DXTType.None;

		private System.Windows.Forms.Button bttnDxt1;
		private System.Windows.Forms.Button bttnDxt3;
		private System.Windows.Forms.Button bttnDxt5;
        private System.Windows.Forms.Label lblMessage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DXTFormat()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		#region Windows Form Designer generated code fibble
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.bttnDxt1 = new System.Windows.Forms.Button();
            this.bttnDxt3 = new System.Windows.Forms.Button();
            this.bttnDxt5 = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bttnDxt1
            // 
            this.bttnDxt1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bttnDxt1.Location = new System.Drawing.Point(7, 90);
            this.bttnDxt1.Name = "bttnDxt1";
            this.bttnDxt1.Size = new System.Drawing.Size(94, 23);
            this.bttnDxt1.TabIndex = 0;
            this.bttnDxt1.Text = "DXT1";
            this.bttnDxt1.Click += new System.EventHandler(this.bttnDxt1_Click);
            // 
            // bttnDxt3
            // 
            this.bttnDxt3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bttnDxt3.Location = new System.Drawing.Point(107, 90);
            this.bttnDxt3.Name = "bttnDxt3";
            this.bttnDxt3.Size = new System.Drawing.Size(94, 23);
            this.bttnDxt3.TabIndex = 1;
            this.bttnDxt3.Text = "DXT3";
            this.bttnDxt3.Click += new System.EventHandler(this.bttnDxt3_Click);
            // 
            // bttnDxt5
            // 
            this.bttnDxt5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bttnDxt5.Location = new System.Drawing.Point(210, 90);
            this.bttnDxt5.Name = "bttnDxt5";
            this.bttnDxt5.Size = new System.Drawing.Size(94, 23);
            this.bttnDxt5.TabIndex = 2;
            this.bttnDxt5.Text = "DXT5";
            this.bttnDxt5.Click += new System.EventHandler(this.bttnDxt5_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(8, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(296, 72);
            this.lblMessage.TabIndex = 5;
            this.lblMessage.Text = "Please select a DXT Compression level to convert the images to.\r\n\r\nInformation on" +
                " DXT levels is available at http://en.wikipedia.org/wiki/S3TC";
            // 
            // DXTFormat
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(316, 121);
            this.ControlBox = false;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.bttnDxt5);
            this.Controls.Add(this.bttnDxt3);
            this.Controls.Add(this.bttnDxt1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DXTFormat";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DXTFormat";
            this.ResumeLayout(false);

		}
		#endregion

		private void bttnDxt1_Click(object sender, System.EventArgs e)
		{
            ChosenFormat = Converter.DXTType.DXT1;
			this.Close();
		}

		private void bttnDxt3_Click(object sender, System.EventArgs e)
		{
            ChosenFormat = Converter.DXTType.DXT3;
			this.Close();		
		}

		private void bttnDxt5_Click(object sender, System.EventArgs e)
		{
            ChosenFormat = Converter.DXTType.DXT5;
			this.Close();		
		}
	}
}
