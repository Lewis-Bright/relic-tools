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
	/// Summary description for ErrorDetails.
	/// </summary>
	public class ErrorDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtDetails;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ErrorDetails(string details)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			txtDetails.Text = details;
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
			this.txtDetails = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtDetails
			// 
			this.txtDetails.Location = new System.Drawing.Point(0, 0);
			this.txtDetails.Multiline = true;
			this.txtDetails.Name = "txtDetails";
			this.txtDetails.ReadOnly = true;
			this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDetails.Size = new System.Drawing.Size(434, 270);
			this.txtDetails.TabIndex = 0;
			this.txtDetails.Text = "";
			this.txtDetails.WordWrap = false;
			// 
			// ErrorDetails
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(434, 270);
			this.Controls.Add(this.txtDetails);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ErrorDetails";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Error Details";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
