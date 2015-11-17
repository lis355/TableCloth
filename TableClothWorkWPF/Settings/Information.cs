using System;
using System.Reflection;

namespace TableClothWork
{
    class Information
    {
        const string _kTitleFormat = "{0} {1}.{2}";

        public static string CurrentAssemblyTitleName
        {
	        get
	        {
		        var assemblyName = Assembly.GetExecutingAssembly().GetName();
		        var result = String.Format( _kTitleFormat,
			        assemblyName.Name,
			        assemblyName.Version.Major,
			        assemblyName.Version.MajorRevision );

		        return result;
	        }
        }
    }
}
