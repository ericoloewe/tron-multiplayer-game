namespace game
{
    partial class GameClient
    {
        private bool needToResetScreen = true;

        void FormScreen(string screenStr)
        {
            string[] rows = screenStr.Split("\n");

            if (needToResetScreen)
            {
                ResetScreen(rows.Length);
                needToResetScreen = false;
            }

            for (int i = 0; i < Screen.Length; i++)
            {
                string[] columns = rows[i].Split(",");

                for (int j = 0; j < Screen[i].Length; j++)
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
