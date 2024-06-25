using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Model.Model
{
    public class Role : IEntity
    {
        public int Id { get ; set ; }

        public string Name { get ; set ; }
    }
}
