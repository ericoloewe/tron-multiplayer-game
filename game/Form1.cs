using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {
        private static readonly int FormWidth = 400;
        private static readonly int FormHeight = 400;
        private static readonly int ArenaBorder = 30;
        private static readonly int ArenaHeight = FormHeight - ArenaBorder;
        private static readonly int ArenaWidth = FormWidth;
        private GameClient client = new GameClient();

        private readonly string[] ALLOWED_KEYS = new string[4] { "right", "left", "up", "down" };

        public Form1()
        {
            InitializeComponent();
            client.OnScreenChange = () => arena.Invalidate();
            client.OnMessage = m => MessageBox.Show(m);
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
            readyButton.Enabled = true;
            connectButton.Enabled = false;
        }

        private void readyButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                client.Ready(nomeTextBox.Text);
                nomeTextBox.Enabled = false;
                readyButton.Enabled = false;
                startButton.Enabled = true;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("You have to fill your name first!");
            }
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            client.StartGame();
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
                    var xSize = ArenaWidth / client.Screen.Length;
                    var x = i * xSize;

                    for (int j = 0; j < row.Length; j++)
                    {
                        Point point = row[j];
                        var ySize = ArenaHeight / row.Length;
                        var y = j * ySize;

                        if (point != null)
                        {
                            if (point.Type == PointType.PLAYER)
                            {
                                using (SolidBrush myBrush = new SolidBrush(Color.Black))
                                {
                                    e.Graphics.FillRectangle(myBrush, x, y, xSize, ySize);
                                }
                            }
                            else
                            {
                                using (SolidBrush myBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                                {
                                    e.Graphics.FillRectangle(myBrush, x, y, xSize, ySize);
                                }
                            }
                        }
                    }
                }
            }

            e.Graphics.Dispose();
        }
    }
}
