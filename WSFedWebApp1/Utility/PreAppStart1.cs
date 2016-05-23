using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Compilation;
[assembly: PreApplicationStartMethod(typeof(WSFedWebApp1.Utility.PreAppStart1), "PreAppInit")]
namespace WSFedWebApp1.Utility
{    
    public class PreAppStart1:BuildProvider
    {
        public static void PreAppInit()
        {
            Debug.WriteLine("Pre app init");
            var appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        }
    }
}