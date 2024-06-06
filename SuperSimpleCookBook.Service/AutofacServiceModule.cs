using Autofac;
using SuperSimpleCookbook.Model;


namespace SuperSimpleCookbook.Service.Common
{
    public class AutofacServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorService>()
                .As<IAuthorService<Author, AuthorService>>().InstancePerLifetimeScope();

            builder.RegisterType<RecipeService>().
                As<IRecipeService<Recipe>>().InstancePerLifetimeScope();  
        }
    }
}
