using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit;

namespace UXI.GazeToolkit.Extensions
{
    public static class GazeDataEx
    {
        /// <summary>
        /// Gets the eye distance for both, single, or none eye based on the <seealso cref="GazeData.Validity"/>.
        /// The distance is retrieved from the eye 3D position in millimeters in the User Coordinate System 
        /// (with origin at the centre of the frontal surface of the eye tracker). 
        /// </summary>
        /// <param name="gazeData">Instance of <seealso cref="GazeData"/> to resolve the eye distance from.</param>
        /// <param name="distance">Retrieved distance of the eye(s) in millimeters in the User Coordinate System. Mean distance if both eyes are valid, or distance of the only valid eye.</param>
        /// <returns>True if data for at least one eye is valid; otherwise false, if no eye is valid.</returns>
        public static bool GetEyeDistance(this GazeData gazeData, out double distance)
        {
            if (gazeData.Validity == GazeDataValidity.Both)
            {
                distance = (gazeData.LeftEye.EyePosition3D.Z + gazeData.RightEye.EyePosition3D.Z) / 2d;
                return true;
            }
            else if (gazeData.Validity.HasLeftEye())
            {
                distance = gazeData.LeftEye.EyePosition3D.Z;
                return true;
            }
            else if (gazeData.Validity.HasRightEye())
            {
                distance = gazeData.RightEye.EyePosition3D.Z;
                return true;
            }

            distance = Double.PositiveInfinity;
            return false;
        }


        /// <summary>
        /// Gets the relative eye distance for both, single, or none eye based on the <seealso cref="GazeData.Validity"/>.
        /// The distance is retrieved from the 3D eye position relative to the eye tracker trackbox (Trackbox Coordinate System).
        /// </summary>
        /// <param name="gazeData">Instance of <seealso cref="GazeData"/> to resolve the eye distance from.</param>
        /// <param name="distance">Retrieved distance of the eye(s) relative to the eye tracker trackbox. Mean distance if both eyes are valid, or distance of the only valid eye.</param>
        /// <returns>True if data for at least one eye is valid; otherwise false, if no eye is valid.</returns>
        public static bool GetRelativeEyeDistance(this GazeData gazeData, out double distance)
        {
            if (gazeData.Validity == GazeDataValidity.Both)
            {
                distance = (gazeData.LeftEye.EyePosition3DRelative.Z + gazeData.RightEye.EyePosition3DRelative.Z) / 2d;
                return true;
            }
            else if (gazeData.Validity.HasLeftEye())
            {
                distance = gazeData.LeftEye.EyePosition3DRelative.Z;
                return true;
            }
            else if (gazeData.Validity.HasRightEye())
            {
                distance = gazeData.RightEye.EyePosition3DRelative.Z;
                return true;
            }

            distance = Double.PositiveInfinity;
            return false;
        }
    }
}
