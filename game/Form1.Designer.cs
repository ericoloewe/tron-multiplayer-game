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
            this.arena.SuspendLayout();
            this.SuspendLayout();
            // 
            // arena
            // 
            this.arena.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arena.Location = new System.Drawing.Point(0, ArenaBorder);
            this.arena.Name = "arena";
            this.arena.Size = new System.Drawing.Size(Width, Height - ArenaBorder);
            this.arena.TabIndex = 9;
            this.arena.TabStop = false;
            this.arena.Text = "Graphics";
            this.arena.Paint += new System.Windows.Forms.PaintEventHandler(this.arena_Paint);

            // 
            // MainForm
            // 
            this.components = new System.ComponentModel.Container();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(Width, Height);
            this.Text = "Multiplayer game";
            this.Controls.Add(this.arena);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.arena.ResumeLayout(false);
            this.arena.PerformLayout();
            this.ResumeLayout(false);
        }


        #endregion
        private System.Windows.Forms.GroupBox arena;
    }
}

