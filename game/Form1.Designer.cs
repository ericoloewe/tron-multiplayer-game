using System;
using System.Windows.Forms;

namespace game
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.arena = new System.Windows.Forms.GroupBox();
            this.menu = new System.Windows.Forms.GroupBox();
            this.nomeLabel = new System.Windows.Forms.Label();
            this.nomeTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.arena.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // arena
            // 
            this.arena.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arena.Location = new System.Drawing.Point(0, ArenaBorder);
            this.arena.Name = "arena";
            this.arena.Size = new System.Drawing.Size(ArenaWidth, ArenaHeight);
            this.arena.TabIndex = 11;
            this.arena.TabStop = false;
            this.arena.Paint += new System.Windows.Forms.PaintEventHandler(this.arena_Paint);

            // 
            // menu
            // 
            this.menu.Controls.Add(this.nomeLabel);
            this.menu.Controls.Add(this.nomeTextBox);
            this.menu.Controls.Add(this.startButton);
            this.menu.Controls.Add(this.connectButton);
            this.menu.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(Width, ArenaBorder);
            this.menu.TabIndex = 10;
            this.menu.TabStop = false;

            // 
            // nomeLabel
            // 
            this.nomeLabel.AutoSize = true;
            this.nomeLabel.Location = new System.Drawing.Point(0, 0);
            this.nomeLabel.Name = "nomeLabel";
            this.nomeLabel.Size = new System.Drawing.Size(50, ArenaBorder);
            this.nomeLabel.TabIndex = 6;
            this.nomeLabel.Text = "Nome: ";

            // 
            // nomeTextBox
            // 
            this.nomeTextBox.AutoSize = true;
            this.nomeTextBox.Location = new System.Drawing.Point(60, 0);
            this.nomeTextBox.Name = "nomeTextBox";
            this.nomeTextBox.Size = new System.Drawing.Size(50, ArenaBorder);
            this.nomeTextBox.TabIndex = 7;
            this.nomeTextBox.Enabled = false;

            // 
            // startButton
            // 
            this.startButton.AutoSize = true;
            this.startButton.Location = new System.Drawing.Point(120, 0);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(50, ArenaBorder);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.Click += new EventHandler(this.startButton_Click);
            this.startButton.Enabled = false;

            // 
            // connectButton
            // 
            this.connectButton.AutoSize = true;
            this.connectButton.Location = new System.Drawing.Point(180, 0);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(50, ArenaBorder);
            this.connectButton.TabIndex = 9;
            this.connectButton.Text = "Connect";
            this.connectButton.Click += new EventHandler(this.connectButton_Click);


            // 
            // MainForm
            // 
            this.components = new System.ComponentModel.Container();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(Width, Height);
            this.Text = "Multiplayer game";
            this.Controls.Add(this.arena);
            this.Controls.Add(this.menu);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.arena.ResumeLayout(false);
            this.arena.PerformLayout();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
        }


        #endregion
        private System.Windows.Forms.GroupBox arena;
        private System.Windows.Forms.GroupBox menu;
        private System.Windows.Forms.TextBox nomeTextBox;
        private System.Windows.Forms.Label nomeLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button connectButton;
    }
}

