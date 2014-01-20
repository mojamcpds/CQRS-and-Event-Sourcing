using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CQRS.Configuration;
using Ninject;

namespace WebPart
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        internal static readonly IKernel PersonKarnel = new StandardKernel(new Ioc());

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(PersonKarnel));
            PersonBootStrapper.BootStrap();
        }

        public class NinjectControllerFactory : DefaultControllerFactory
        {
            private IKernel kernel = null;

            public NinjectControllerFactory(IKernel kernel)
            {
                this.kernel = kernel;
            }

            protected override IController GetControllerInstance(
                System.Web.Routing.RequestContext requestContext, Type controllerType)
            {
                return controllerType == null ? null : (IController)this.kernel.Get(controllerType);
            }
        }
    }
}