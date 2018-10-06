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
	/// Summary description for YesNoToAllDialog.
	/// </summary>
	public class YesNoToAllDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button bttnYesAll;
		private System.Windows.Forms.Button bttnNoAll;
		private System.Windows.Forms.Button bttnNo;
		private System.Windows.Forms.Button bttnYes;
		private System.Windows.Forms.Label lblMessage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private bool all = false;

		public YesNoToAllDialog(string title, string message)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			lblMessage.Text = message;
			this.Text = title;
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
			this.bttnYesAll = new System.Windows.Forms.Button();
			this.bttnNoAll = new System.Windows.Forms.Button();
			this.bttnNo = new System.Windows.Forms.Button();
			this.bttnYes = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bttnYesAll
			// 
			this.bttnYesAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnYesAll.Location = new System.Drawing.Point(8, 88);
			this.bttnYesAll.Name = "bttnYesAll";
			this.bttnYesAll.TabIndex = 0;
			this.bttnYesAll.Text = "Yes To All";
			this.bttnYesAll.Click += new System.EventHandler(this.bttnYesAll_Click);
			// 
			// bttnNoAll
			// 
			this.bttnNoAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnNoAll.Location = new System.Drawing.Point(248, 88);
			this.bttnNoAll.Name = "bttnNoAll";
			this.bttnNoAll.TabIndex = 1;
			this.bttnNoAll.Text = "No To All";
			this.bttnNoAll.Click += new System.EventHandler(this.bttnNoAll_Click);
			// 
			// bttnNo
			// 
			this.bttnNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnNo.Location = new System.Drawing.Point(168, 88);
			this.bttnNo.Name = "bttnNo";
			this.bttnNo.TabIndex = 2;
			this.bttnNo.Text = "No";
			this.bttnNo.Click += new System.EventHandler(this.bttnNo_Click);
			// 
			// bttnYes
			// 
			this.bttnYes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.bttnYes.Location = new System.Drawing.Point(88, 88);
			this.bttnYes.Name = "bttnYes";
			this.bttnYes.TabIndex = 3;
			this.bttnYes.Text = "Yes";
			this.bttnYes.Click += new System.EventHandler(this.bttnYes_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.Location = new System.Drawing.Point(16, 8);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(304, 72);
			this.lblMessage.TabIndex = 4;
			this.lblMessage.Text = "label1";
			// 
			// YesNoToAllDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(330, 122);
			this.ControlBox = false;
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.bttnYes);
			this.Controls.Add(this.bttnNo);
			this.Controls.Add(this.bttnNoAll);
			this.Controls.Add(this.bttnYesAll);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "YesNoToAllDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "YesNoToAllDialog";
			this.ResumeLayout(false);

		}
		#endregion

		private void bttnYesAll_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			all = true;
		}

		private void bttnYes_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			all = false;
		}

		private void bttnNo_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			all = false;
		}

		private void bttnNoAll_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			all = true;
		}

		public bool ToAll
		{
			get{ return all; }
		}
	}
}
