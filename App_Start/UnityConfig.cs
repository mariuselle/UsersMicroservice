using AutoMapper;
using FluentValidation;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;
using Users.Application.AsyncServices;
using Users.Application.Mappers;
using Users.Application.Repositories;
using Users.Application.Validators;
using Users.Core.Entities;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Repositories;

namespace MiniStore
{
    public static class UnityConfig
    {
        private static readonly IUnityContainer container = new UnityContainer();
        public static void RegisterComponents()
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ApplicationContext>();
            container.RegisterType<IMapper, Mapper>(
                new InjectionConstructor(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new UserProfile());
                }))
            );
            container.RegisterType<IValidator<User>, UserValidator>();
            container.RegisterType<IMessageBusClient, MessageBusClient>();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        public static IUnityContainer GetConfiguredContainer()
        {
            return container;
        }
    }
}