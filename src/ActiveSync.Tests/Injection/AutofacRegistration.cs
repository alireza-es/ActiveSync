using ActiveSync.Core.StateManagement;
using ActiveSync.MockImplementation.Service;
using ActiveSync.SyncContract.Service;
using Autofac;
using Bootstrap.Autofac;

namespace ActiveSync.Tests.Injection
{
    public class AutofacRegistration : IAutofacRegistration
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FolderService>().As<IFolderService>();
            containerBuilder.RegisterType<EmailService>().As<IEmailService>();
            containerBuilder.RegisterType<ContactService>().As<IContactService>();

            containerBuilder.RegisterType<FileStateMachine>().As<IStateMachine>();

        }
    }
}