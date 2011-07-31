using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using GeCoSurvey.Data;
using Microsoft.Practices.Unity;
using GeCoSurvey.Web.IoC;
using GeCoSurvey.Service;
using GeCoSurvey.Data.Infrastructure;
using System.Configuration;

namespace GeCoSurvey.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Inizializzazione DB
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SetInitializerDb"]))
            {
                //Ricordarsi di cancellare la tabella EdmMetadata altrimenti non funziona GecoClient
                Database.SetInitializer(new SurveyContextInitializer());
                //Database.SetInitializer<SurveyContext>(null);
            }

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IUnityContainer container = GetUnityContainer();
            ControllerBuilder.Current.SetControllerFactory (new UnityControllerFactory(container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container)); //vedere il pacchetto Unity.Mvc3;
        }

        private IUnityContainer GetUnityContainer()
        {
            //Crea UnityContainer          
            IUnityContainer container = new UnityContainer()
                //.RegisterType<IControllerActivator, CustomControllerActivator>() // Non serve
                //.RegisterType<IFormsAuthenticationService, FormsAuthenticationService>()
                //.RegisterType<IMembershipService, AccountMembershipService>()
                //.RegisterInstance<MembershipProvider>(Membership.Provider)
            .RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>())
            .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>())
                //.RegisterType<ICategoryRepository, CategoryRepository>(new HttpContextLifetimeManager<ICategoryRepository>())
                //.RegisterType<IExpenseRepository, ExpenseRepository>(new HttpContextLifetimeManager<IExpenseRepository>())
                //.RegisterType<ICategoryService, CategoryService>(new HttpContextLifetimeManager<ICategoryService>())
                //.RegisterType<IExpenseService, ExpenseService>(new HttpContextLifetimeManager<IExpenseService>())
            .RegisterType<IDipendentiService, DipendentiService>(new HttpContextLifetimeManager<IDipendentiService>())
            .RegisterType<ISurveyService, SurveyService>(new HttpContextLifetimeManager<ISurveyService>())
            .RegisterType<IUserService, UserService>(new HttpContextLifetimeManager<IUserService>())
            .RegisterType(typeof(IRepository<>), typeof(RepositoryBase<>));
                //.RegisterType<ISurveyRepository, SurveyRepository>(new HttpContextLifetimeManager<ISurveyRepository>())
            //.RegisterType<IUserRepository, UserRepository>(new HttpContextLifetimeManager<IUserRepository>());
            //.RegisterType<IRoleRepository, RoleRepository>(new HttpContextLifetimeManager<IRoleRepository>())
            //.RegisterType<ISecurityService, SecurityService>(new HttpContextLifetimeManager<ISecurityService>());

            
            return container;
        }
    }
}