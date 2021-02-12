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
	/// Summary description for OrganiseLayers.
	/// </summary>
	public class OrganiseLayers : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtLayer1;
		private System.Windows.Forms.TextBox txtLayer2;
		private System.Windows.Forms.TextBox txtLayer3;
		private System.Windows.Forms.TextBox txtLayer4;
		private System.Windows.Forms.TextBox txtLayer5;
		private System.Windows.Forms.TextBox txtLayer6;
		private System.Windows.Forms.Button bttnOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;

		string[] filepaths;

		public OrganiseLayers(string[] filepaths_in)
		{

			filepaths = filepaths_in;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			txtLayer1.Text = filepaths[0].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);

			if (filepaths.Length==3)
			{
				txtLayer2.Text = filepaths[1].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);				
				txtLayer3.Text = filepaths[2].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);
			}
			else if (filepaths.Length==2)
			{				
				txtLayer4.Text = filepaths[1].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);
			}
			else if (filepaths.Length==4)
			{
				txtLayer2.Text = filepaths[1].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);
				txtLayer3.Text = filepaths[2].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);
				txtLayer4.Text = filepaths[3].Substring(filepaths[0].LastIndexOf(IBBoard.Constants.DirectoryChar)+1);
			}
			else
			{
				throw new InvalidOperationException("Between one and three additional texture maps must be specified.");
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
            this.txtLayer1 = new System.Windows.Forms.TextBox();
            this.txtLayer2 = new System.Windows.Forms.TextBox();
            this.txtLayer3 = new System.Windows.Forms.TextBox();
            this.txtLayer4 = new System.Windows.Forms.TextBox();
            this.txtLayer5 = new System.Windows.Forms.TextBox();
            this.txtLayer6 = new System.Windows.Forms.TextBox();
            this.bttnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtLayer1
            // 
            this.txtLayer1.Enabled = false;
            this.txtLayer1.Location = new System.Drawing.Point(120, 8);
            this.txtLayer1.Name = "txtLayer1";
            this.txtLayer1.Size = new System.Drawing.Size(264, 20);
            this.txtLayer1.TabIndex = 0;
            // 
            // txtLayer2
            // 
            this.txtLayer2.Location = new System.Drawing.Point(120, 32);
            this.txtLayer2.Name = "txtLayer2";
            this.txtLayer2.Size = new System.Drawing.Size(264, 20);
            this.txtLayer2.TabIndex = 1;
            // 
            // txtLayer3
            // 
            this.txtLayer3.Location = new System.Drawing.Point(120, 56);
            this.txtLayer3.Name = "txtLayer3";
            this.txtLayer3.Size = new System.Drawing.Size(264, 20);
            this.txtLayer3.TabIndex = 2;
            // 
            // txtLayer4
            // 
            this.txtLayer4.Location = new System.Drawing.Point(120, 80);
            this.txtLayer4.Name = "txtLayer4";
            this.txtLayer4.Size = new System.Drawing.Size(264, 20);
            this.txtLayer4.TabIndex = 3;
            // 
            // txtLayer5
            // 
            this.txtLayer5.Location = new System.Drawing.Point(120, 104);
            this.txtLayer5.Name = "txtLayer5";
            this.txtLayer5.Size = new System.Drawing.Size(264, 20);
            this.txtLayer5.TabIndex = 4;
            // 
            // txtLayer6
            // 
            this.txtLayer6.Enabled = false;
            this.txtLayer6.Location = new System.Drawing.Point(120, 128);
            this.txtLayer6.Name = "txtLayer6";
            this.txtLayer6.Size = new System.Drawing.Size(264, 20);
            this.txtLayer6.TabIndex = 5;
            // 
            // bttnOK
            // 
            this.bttnOK.Location = new System.Drawing.Point(296, 176);
            this.bttnOK.Name = "bttnOK";
            this.bttnOK.Size = new System.Drawing.Size(88, 24);
            this.bttnOK.TabIndex = 12;
            this.bttnOK.Text = "OK";
            this.bttnOK.Click += new System.EventHandler(this.bttnOK_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Texture:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "Reflection:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "Specularity Map:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 16;
            this.label4.Text = "Self Illumination:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 17;
            this.label5.Text = "Opacity:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // OrganiseLayers
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(392, 210);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bttnOK);
            this.Controls.Add(this.txtLayer6);
            this.Controls.Add(this.txtLayer5);
            this.Controls.Add(this.txtLayer4);
            this.Controls.Add(this.txtLayer3);
            this.Controls.Add(this.txtLayer2);
            this.Controls.Add(this.txtLayer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OrganiseLayers";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Organise RSH Layers";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public string[] Organise(Form parent)
		{
			try
			{
				this.ShowDialog(parent);
				string[] paths = new string[6];
				paths[0] = filepaths[0];

				if (txtLayer2.Text!="")
				{
					for (int i = 1; i<filepaths.Length; i++)
					{
						if (filepaths[i].EndsWith(txtLayer2.Text))
						{
							paths[1] = filepaths[i];
							break;
						}
					}
				}

				if (txtLayer3.Text!="")
				{
					for (int i = 1; i<filepaths.Length; i++)
					{
						if (filepaths[i].EndsWith(txtLayer3.Text))
						{
							paths[2] = filepaths[i];
							break;
						}
					}
				}

				if (txtLayer4.Text!="")
				{
					for (int i = 1; i<filepaths.Length; i++)
					{
						if (filepaths[i].EndsWith(txtLayer4.Text))
						{
							paths[3] = filepaths[i];
							break;
						}
					}
				}

				if (txtLayer5.Text!="")
				{
					for (int i = 1; i<filepaths.Length; i++)
					{
						if (filepaths[i].EndsWith(txtLayer5.Text))
						{
							paths[4] = filepaths[i];
							break;
						}
					}
				}

				return paths;
			}
			finally
			{
				this.Dispose();
			}
		}

		private void bttnOK_Click(object sender, System.EventArgs e)
		{
			bool found = false;

			if (filepaths.Length >= 3)
			{
				for (int i = 1; i<filepaths.Length; i++)
				{
					if (filepaths[i].EndsWith(txtLayer2.Text))
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					MessageBox.Show(this, "The file "+txtLayer2.Text+" was not found in the original file list.", "Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				found = false;

				for (int i = 1; i<filepaths.Length; i++)
				{
					if (filepaths[i].EndsWith(txtLayer3.Text))
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					MessageBox.Show(this, "The file "+txtLayer3.Text+" was not found in the original file list.", "Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			if (filepaths.Length==2 || filepaths.Length==4)
			{

				found = false;

				for (int i = 1; i<filepaths.Length; i++)
				{
					if (filepaths[i].EndsWith(txtLayer4.Text))
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					MessageBox.Show(this, "The file "+txtLayer4.Text+" was not found in the original file list.", "Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			this.Close();
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
	}
}
