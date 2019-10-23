using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game
{
    partial class GameClient
    {
        async Task StartScreenCycle()
        {
            var task = new Task(() =>
            {
                while (!HasFinished)
                {
                    lock (sender)
                    {
                        ReceiveAndFormScreen();
                    }

                    Thread.Sleep(TIME_TO_UPDATE_SCREEN_IN_MS);
                }
            });

            task.Start();
            await task;
        }

        void ReceiveAndFormScreen(bool? forceReset = false)
        {
            sender.Send(Encoding.ASCII.GetBytes($"screen"));

            var bytes = new byte[10240];
            var bytesRec = sender.Receive(bytes);

            var screenStr = Encoding.UTF8.GetString(bytes, 0, bytesRec);

            string[] rows = screenStr.Split("\n");

            if (forceReset.HasValue)
            {
                ResetScreen(rows.Length);
            }

            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(",");

                for (int j = 0; j < columns.Length; j++)
                {
                    Screen[i][j] = Point.FromText(columns[j]);
                }
            }

            OnScreenChange.Invoke();
        }

        void ResetScreen(int size)
        {
            Screen = new Point[size][];

            for (int i = 0; i < size; i++)
            {
                Screen[i] = new Point[size];
            }
        }
    }
}
