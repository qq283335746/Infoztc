using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.TableCacheDependency
{
    public class Ernie : MsSqlCacheDependency
    {
        public Ernie() : base("ErnieTableDependency") { }
    }
}
