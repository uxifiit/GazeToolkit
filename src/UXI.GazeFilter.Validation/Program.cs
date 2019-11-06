﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeFilter.Validation.Serialization.Csv.Converters;
using UXI.GazeFilter.Validation.Serialization.Json.Converter;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Serialization;
using UXI.GazeToolkit.Serialization.Csv;
using UXI.GazeToolkit.Serialization.Json;
using UXI.GazeToolkit.Validation;
using UXI.GazeToolkit.Validation.Serialization.Csv;
using UXI.GazeToolkit.Validation.Serialization.Json;
using UXI.Filters;
using UXI.Serialization;
using UXI.Serialization.Extensions;
using UXI.GazeFilter.Validation.Serialization;
using UXI.Filters.Configuration;

namespace UXI.GazeFilter.Validation
{
    [Verb("points", HelpText = "Filters input gaze data with validation points and outputs these points.", Hidden = false)]
    public class ValidationOptions : BaseOptions
    {
        [Value(1, HelpText = "Path to the input file with validation points.", MetaName = "input validation points file", MetaValue = "POINTS_FILE", Required = true)]
        public string ValidationPointsFile { get; set; } 
    }



    [Verb("angular", HelpText = "Performs angular evaluation of gaze data.", Hidden = false)]
    public class AngularOptions : ValidationOptions
    {
        [Option('d', "display", Default = null, HelpText = "Path to the input file with eye tracker display area changed events.", Required = false)]
        public string DisplayAreaFile { get; set; }
    }



    [Verb("pixel", HelpText = "Performs pixel evaluation of gaze data.", Hidden = false)]
    public class PixelOptions : ValidationOptions
    {
        [Option('r', "resolution", Default = null, HelpText = "Screen resolution in pixels, in format [width]x[height]. This option overrides resolution dimensions specified separately. If not specified, the separate resolution parameters are used instead.", Required = false)]
        public string ScreenResolution { get; set; }

        [Option("resolution-width", Default = 1, HelpText = "Screen resolution width in pixels.", Required = false)]
        public int ScreenResolutionWidth { get; set; }

        [Option("resolution-height", Default = 1, HelpText = "Screen resolution height in pixels.", Required = false)]
        public int ScreenResolutionHeight { get; set; }
    }



    static class Program
    {
        static int Main(string[] args)
        {
            return new MultiFilterHost<GazeFilterContext>
            (
                configurations: new FilterConfiguration[]
                {
                    new ValidationDataSerializationFilterConfiguration()
                },
                filters: new IFilter[]
                {
                    new MapGazeDataToValidationPointsFilter<ValidationOptions>(),
                    new FilterPipeline<AngularOptions>
                    (
                        new MapGazeDataToValidationPointsFilter<AngularOptions>(),
                        new AngularValidationFilter()
                    ),
                    new FilterPipeline<PixelOptions>
                    (
                        new MapGazeDataToValidationPointsFilter<PixelOptions>(),
                        new PixelValidationFilter()
                    )
                }
            ).Execute(args);
        }
    }
}
