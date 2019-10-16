using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {
        private readonly int Width = 400;
        private readonly int Height = 400;
        private readonly int ArenaBorder = 30;
        private GameClient client = new GameClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, System.EventArgs e)
        {
            client.Connect();
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                client.StartGame(nomeTextBox.Text);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("You have to connect first!");
            }
        }

        private void arena_Paint(object sender, PaintEventArgs e)
        {
            // If there is an image and it has a location, 
            // paint it when the Form is repainted.
            base.OnPaint(e);

            if (client.Screen != null)
            {
                for (int i = 0; i < client.Screen.Length; i++)
                {
                    var row = client.Screen[i];

                    for (int j = 0; j < row.Length; j++)
                    {
                        Point point = row[j];

                        if (point != null)
                        {
                            using (Pen pen = new Pen(Color.Black))
                            {
                                e.Graphics.DrawLine(pen, i, j, i + 10, j + 10);
                            }
                        }
                    }
                }
            }

            e.Graphics.Dispose();
        }
    }
}
