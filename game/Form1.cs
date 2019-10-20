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

        private readonly string[] ALLOWED_KEYS = new string[4] { "right", "left", "up", "down" };

        public Form1()
        {
            InitializeComponent();
            client.OnScreenChange = () => arena.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var parsedPressedKey = keyData.ToString().ToLower();
            var isGameCommand = client.HasStarted && ALLOWED_KEYS.Any(ak => ak == parsedPressedKey);

            if (isGameCommand)
            {
                client.Move(parsedPressedKey);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void connectButton_Click(object sender, System.EventArgs e)
        {
            client.Connect();
            nomeTextBox.Enabled = true;
            startButton.Enabled = true;
            connectButton.Enabled = false;
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
            catch (ArgumentException)
            {
                MessageBox.Show("You have to fill your name first!");
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
                                e.Graphics.DrawLine(pen, i, j, i * 2, j * 2);
                            }
                        }
                    }
                }
            }

            e.Graphics.Dispose();
        }
    }
}
