namespace TableClothKernel
{
    public static class Information
    {
        public static string KernelName
        {
            get
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                return assemblyName.Name;
            }
        }

        public static string KernelVersion
        {
            get
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                return assemblyName.Version.Major.ToString()
                       + '.' + assemblyName.Version.Minor.ToString();
            }
        }

        public static string KernelAssembly
        {
            get
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                return assemblyName.Version.Build.ToString()
                       + '.' + assemblyName.Version.Revision.ToString();
            }
        }
    }
}
