using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ERP_Love_Gid.Models;
namespace ERP_Love_Gid.Controllers
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext request, Type contrType)
        {
            if (contrType==null) return Activator.CreateInstance(typeof(HomeController), new DataManager()) as IController;

            return Activator.CreateInstance(contrType, new DataManager()) as IController;
        }
    }

}