public static class LevelSystem
{
    public static int[] Experience;

    static LevelSystem()
    {
        Experience = new int[100];

        for (int i = 0; i < 100; i++)
        {
            Experience[i] = i * 100;
            if(i != 0)
            {
                Experience[i] += Experience[i - 1];
            }
        }
    }
}
