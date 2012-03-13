﻿namespace Nexus.Client
{
	partial class GameModeSelectionForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbxRemember = new System.Windows.Forms.CheckBox();
			this.butOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gameModeListView1 = new Nexus.Client.GameModeListView();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbxRemember
			// 
			this.cbxRemember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxRemember.AutoSize = true;
			this.cbxRemember.Location = new System.Drawing.Point(342, 2);
			this.cbxRemember.Name = "cbxRemember";
			this.cbxRemember.Size = new System.Drawing.Size(136, 17);
			this.cbxRemember.TabIndex = 1;
			this.cbxRemember.Text = "Don\'t ask me next time.";
			this.cbxRemember.UseVisualStyleBackColor = true;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(403, 25);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(210, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Select the game you would like to manage:";
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(490, 22);
			this.panel1.TabIndex = 5;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.butOK);
			this.panel2.Controls.Add(this.cbxRemember);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 305);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(490, 60);
			this.panel2.TabIndex = 6;
			// 
			// gameModeListView1
			// 
			this.gameModeListView1.AutoSize = true;
			this.gameModeListView1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gameModeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gameModeListView1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.gameModeListView1.Location = new System.Drawing.Point(0, 22);
			this.gameModeListView1.Name = "gameModeListView1";
			this.gameModeListView1.Padding = new System.Windows.Forms.Padding(13, 6, 13, 6);
			this.gameModeListView1.Size = new System.Drawing.Size(490, 283);
			this.gameModeListView1.TabIndex = 4;
			// 
			// GameModeSelectionForm
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(490, 365);
			this.Controls.Add(this.gameModeListView1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GameModeSelectionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Game Selection";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cbxRemember;
		private System.Windows.Forms.Button butOK;
		private System.Windows.Forms.Label label1;
		private GameModeListView gameModeListView1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
	}
}