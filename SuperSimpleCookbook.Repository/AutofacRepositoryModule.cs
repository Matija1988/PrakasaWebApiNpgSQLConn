using Autofac;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.Common
{
    public class AutofacRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           builder.RegisterType<AuthorRepository>()
                .As<IRepositoryAuthor<Author, AuthorRecipe>>().InstancePerLifetimeScope();

            builder.RegisterType<RecipeRepository>()
                .As<IRepositoryRecipe<Recipe>>().InstancePerLifetimeScope(); 
        }
    }
}
