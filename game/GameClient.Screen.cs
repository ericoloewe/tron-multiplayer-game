namespace game
{
    partial class GameClient
    {
        void ReceiveAndFormScreen(bool? forceReset = false)
        {
            clientConnection.Send($"screen");

            var screenStr = clientConnection.Receive();

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
