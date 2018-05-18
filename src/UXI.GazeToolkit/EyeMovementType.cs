using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.GazeToolkit
{
    public enum EyeMovementType
    {
        /// <summary>
        /// Eye movement represents a fixation.
        /// </summary>
        Fixation,

        /// <summary>
        /// Eye movement represents a saccade.
        /// </summary>
        Saccade,

        /// <summary>
        /// Eye movement is unknown because the eye were not seen by the tracker (e.g. blinks), or the real movement could have not been determined from the samples.
        /// </summary>
        Unknown
    }
}
