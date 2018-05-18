using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public enum GazeDataValidity : byte
    {
        None = 0,
        Left = 1,
        ProbablyLeft = 2,
        UnknownWhichOne = 4,
        ProbablyRight = 8,
        Right = 16,
        Both = Left | Right,

        LeftEyeMask = Left | ProbablyLeft,
        RightEyeMask = Right | ProbablyRight,
        AllMask = LeftEyeMask | RightEyeMask | UnknownWhichOne
    }


    public static class GazeDataValidityEx
    {
        /// <summary>
        /// Checks the validity code for left eye, if it was seen or probably seen.
        /// </summary>
        /// <param name="validity"></param>
        /// <returns>True if the validity code has <seealso cref="GazeDataValidity.Left"/> or <seealso cref="GazeDataValidity.ProbablyLeft"/> flags; otherwise, false.</returns>
        public static bool HasLeftEye(this GazeDataValidity validity)
        {
            return (validity & GazeDataValidity.LeftEyeMask) != GazeDataValidity.None;
        }


        /// <summary>
        /// Checks the validity code for right eye, if it was seen or probably seen.
        /// </summary>
        /// <param name="validity"></param>
        /// <returns>True if the validity code has <seealso cref="GazeDataValidity.Right"/> or <seealso cref="GazeDataValidity.ProbablyRight"/> flags; otherwise, false.</returns>
        public static bool HasRightEye(this GazeDataValidity validity)
        {
            return (validity & GazeDataValidity.RightEyeMask) != GazeDataValidity.None;
        }
    }
}
