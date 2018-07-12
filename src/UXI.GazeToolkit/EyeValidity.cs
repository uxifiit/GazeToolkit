using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit
{
    public enum EyeValidity : byte
    {
        Invalid = 0,
        Valid = 1,
        Probably = 2,
        Unknown = 4,
    }



    public static class EyeValidityEx
    {
        private const EyeValidity HasEyeMask = EyeValidity.Valid | EyeValidity.Probably;

        /// <summary>
        /// Checks whether the eye validity code is either Valid or Probably.
        /// </summary>
        /// <param name="validity">Validity code of the eye.</param>
        /// <returns></returns>
        public static bool HasEye(this EyeValidity validity)
        {
            return (validity & HasEyeMask) != EyeValidity.Invalid;
        }


        /// <summary>
        /// Checks whether the eye validity code is Valid.
        /// </summary>
        /// <param name="validity">Validity code of the eye.</param>
        /// <returns></returns>
        public static bool IsValid(this EyeValidity validity)
        {
            return validity == EyeValidity.Valid;
        }
    }
}
