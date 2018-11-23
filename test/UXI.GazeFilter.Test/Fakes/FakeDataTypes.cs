using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeFilter.Fakes
{
    public interface IA { }
    public interface IA2 { }
    public class A : IA { }
    public class SubA : A, IA2 { }

    public interface IB { }
    public interface IB2 { }

    public class B : IB { }
    public class SubB : B, IB2 { }
}
