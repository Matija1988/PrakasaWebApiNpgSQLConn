using Autofac;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository;
using SuperSimpleCookbook.Repository.Common;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using SuperSimpleCookbook.Service;
using SuperSimpleCookbook.Service.Common;


namespace SuperSimpleCookbook
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           builder.RegisterType<AuthorService>()
                .As<IAuthorService<Author, AuthorRecipe>>().InstancePerDependency();

            builder.RegisterType<RecipeService>()
                .As<IRecipeService<Recipe>>().InstancePerDependency();

            builder.RegisterType<AuthorRepository>()
                .As<IRepositoryAuthor<Author, AuthorRecipe>>().InstancePerDependency();

            builder.RegisterType<RecipeRepository>()
                .As<IRepositoryRecipe<Recipe>>().InstancePerDependency();
                
        }
    }
}
