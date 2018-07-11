using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public class EyeVelocity
    {
        public EyeVelocity(double velocity, SingleEyeGazeData eye)
        {
            Velocity = velocity;
            Eye = eye;
        }

        public double Velocity { get; }

        public SingleEyeGazeData Eye { get; }
    }
}
