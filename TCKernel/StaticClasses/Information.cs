namespace TableClothKernel
{
    public static class Information
    {
        public static string KernelVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString()
                + '.' + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
        }

        public static string KernelAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString()
                 + '.' + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();
        }
    }
}
