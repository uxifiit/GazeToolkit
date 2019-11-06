# UXIsk GazeToolkit

[![Build Status](https://dev.azure.com/uxifiit/GazeToolkit/_apis/build/status/uxifiit.GazeToolkit?branchName=master)](https://dev.azure.com/uxifiit/GazeToolkit/_build/latest?definitionId=6&branchName=master) [![UXI.GazeToolkit package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/875e4574-b18a-49ff-8cf1-55b220af2355/_apis/public/Packaging/Feeds/f25beb4b-f7d5-4466-9073-a54052469941/Packages/d722b395-dbe8-41c8-9dd3-d1fcbf733861/Badge)](https://dev.azure.com/uxifiit/UXI.Libs/_packaging?_a=package&feed=f25beb4b-f7d5-4466-9073-a54052469941&package=d722b395-dbe8-41c8-9dd3-d1fcbf733861&preferRelease=true)

This repository contains `UXI.GazeToolkit` library for filtering and processing streams of gaze data, eye movement classification and data validations for .NET projects. Individual filters from the library are also available as separate console applications.

The toolkit implements I-VT fixation filter as described in the Tobii Pro whitepaper [1]. 
It can be used as a single application or composed from separate filters in for detailed data analysis and tuning its parameters. 
All filters are implemented as extension methods for observables of gaze data, using the `IObservable<T>` interface and Rx.NET library. 

The UXI.GazeToolkit does not directly depend on any SDK from eye tracker vendors.

Description of the projects in this repository:

* `UXI.GazeToolkit` - class library with gaze data classes, filters and utilities, including the implementation of the I-VT fixation filter. 
* `UXI.GazeToolkit.Serialization` - extension library with data converters into and from JSON and CSV formats.
   * Based on [UXIsk Serialization library](https://github.com/uxifiit/UXI.Serialization).
* `UXI.GazeToolkit.Validation` - class library for evaluating precision and accuracy of recorded gaze data.
* Set of `UXI.GazeFilter.[FILTER_NAME]` console applications, each wrapping single gaze data filter. Multiple filters may be chained together using pipes.
   * Based on [UXIsk Filters Framework](https://github.com/uxifiit/Filters).

The following sections describes individual gaze filters, including name of its project (replace in `UXI.GazeFilter.[FILTER_NAME]` for full name), name of the generated executable, type of input and output data, and parameters. 

### Velocity Threshold Eye Movement Classification Pipeline

From raw gaze data (recorded with an eye tracker) into classified eye movements (saccades, fixations), data is filtered through set of steps for filling in gaps of missing data with interpolated values, selecting a single eye data, denoising and measuring velocity of movements and threshold classification. After that, fixations close in time are merged and short ones can be discarded as invalid movements. 

The complete pipeline is available in this project:

* `FixationFilter` - velocity threshold eye movement classification (I-VT filter).
   * Executable: `i-vt.exe`
   * Input: `GazeData`, contains raw gaze data of both eyes.
   * Output: `EyeMovement`, classified eye movements, contains type of the movement, included samples, average sample.
   * Parameters: data frequency (required), switches for enabling individual steps and parameters specific to the individual steps.

Each step of the pipeline is also available as a separate filter project: 

* `FillInGaps` - interpolates missing data.
   * Executable: `fillin.exe`
   * Input: `GazeData`, contains raw gaze data of both eyes.
   * Output: `GazeData`
   * Parameters: maximum gap duration.
* `Selection` - extract data of a single eye for next steps.
   * Executable: `select.exe`
   * Input: `GazeData` 
   * Output: `SingleEyeGazeData`
   * Parameters: selected eye - Left, Right, Average (from both or any valid one), or Strict Average (only if data of both eyes are valid).
* `ReduceNoise` - reduces noise in data with a selected smoothing algorigthm.
   * Executable: `denoise.exe`
   * Input: `SingleEyeGazeData`
   * Output: `SingleEyeGazeData`
   * Parameters: smoothing algorithm (exponential or moving average), value of the alpha for exponential smoothing, size of the window for moving average.
* `Frequency` - measures frequency of data per selected window (1 second by default)).
   * Executable: `frequency.exe`
   * Input: any timestamped data
   * Output: frequency
   * Parameters: size of the window.
* `VelocityCalculation` - evaluates angular velocity of eyes in degrees per second.
   * Executable: `velocity.exe`
   * Input: `SingleEyeGazeData`
   * Output: `EyeVelocity`, contains `SingleEyeGazeData` and measured velocity in °/s (degrees per second).
   * Parameters: frequency of the input data, size of the window used for measurement (in milliseconds).
* `VelocityThresholdClassification` - classifies data samples by velocity into fixations, saccades or unknown movements and groups successives samples with the same type together.
   * Executable: `vt-classify.exe`
   * Input: `EyeVelocity`
   * Output: `EyeMovement`, contains type of the movement, included samples, average sample.
   * Parameters: threshold value in °/s.
* `MergeFixations` - merges that appeared close in time. 
   * Executable: `merge.exe`
   * Input: `EyeMovement`
   * Output: `EyeMovement`
   * Parameters: maximum time and angle between fixations.
* `DiscardFixations` - invalidates classified fixation movements to unknown movements if they were shorter than allowed threshold duration.
   * Executable: `discard.exe`
   * Input: `EyeMovement`
   * Output: `EyeMovement`
   * Parameters: minimum duration of a fixation to keep.

When running filters from the command line, use `--help` option to list all parameters with description. 

### Eye Tracking Data Validation

Recorded gaze data should be evaluated for precision and accuracy before further analysis.
To validate data (or eye tracker calibration), display validation points on the screen during the recording (simliar to calibration process) and instruct participants to focus on them.
The recorded gaze data can be then evaluated with precision and accuracy metrics against the validation points:

* `Accuracy` - average distance of samples from the validation point.
* `PrecisionSD` - standard deviation of distances between recorded samples and the mean sample.
* `PrecisionRMS` - root mean square of inter-sample distances.
* `ValidRatio` - ratio of valid samples for the validation point.

Use the following filter for data validation:

* `Validation` - evaluates precision and accuracy metrics of recorded data against validation points in either visual angles (degrees) or distance on screen (pixels).
   * Executable: `validation.exe`
   * Input: `GazeData` with raw gaze data of both eyes, and collection of `ValidationPoint`s
   * Output: `ValidationResult` with metrics values for each validation point 
   * Parameters: type of the validation (angular or pixel), path to the display area configuration for angular validation or screen resolution for pixel validation.

### Other Filters

Additional filters available in the toolkit:

* `Filter` - filters data by timestamps and can convert them between formats (JSON and CSV).
   * Executable: `filter.exe`
   * Input: `GazeData | SingleEyeGazeData | EyeMovement`
   * Output: same as input.
   * Parameters: timestamp range (from-to) and type of input data: `gaze`, `single-eye`, or `movement`.  


## Example Usage

We do not distribute binaries of the gaze filters, so in order to use them, you have to build them yourself.

### Build

First, install Microsoft Visual Studio 2015 or 2017 or Visual Studio Build Tools.

To build the solution using the Visual Studio: 
1. Open the `UXI.GazeToolkit.sln` in Visual Studio.
2. Set up build target to `Release`.
3. Build the solution (default hotkey <kbd>F6</kbd>).

To build the solution using the included batch script: 
1. Download [NuGet Windows Commandline](https://www.nuget.org/downloads), v4.9.x were tested.
2. Create new environment variable named `nuget` with path set to the `nuget.exe` executable, e.g., `C:\Program Files (x86)\NuGet\nuget.exe`.
3. Test the path in a new command line window with `echo %nuget%`.
4. Run the `build.bat` script.

Then, locate the build output in the `/build/Release/` directory.

### Prepare Input Data

The main type of input data is raw gaze data in [GazeData](src/UXI.GazeToolkit/GazeData.cs) which includes timestamp and [EyeData](src/UXI.GazeToolkit/EyeData.cs) for each eye:

* `Timestamp` (DateTime string | TimeSpan string | ticks count) - time when the data was received from or sampled by the Eye Tracker.
* `Validity` code for each eye may be of these values: `Invalid`, `Valid`, `Probably`, `Unknown`.
* `GazePoint2D` - gaze point on the display area, relative to its size.
* `GazePoint3D` - target gaze point of the eye on the display area in millimeters, starting in the center of the eye tracker surface area.
* `EyePosition3D` - position of the eye in 3D space in millimeters, starting in the center of the eye tracker surface area.

#### Data Format
Format your input data into JSON or CSV formats as below:
```javascript
[ 
    {
        "Timestamp": "2019-02-20T14:30:19.8526299+01:00",
        "LeftEye": {
            "Validity": "Valid",
            "GazePoint2D": {
                "X": 0.249588146805763,
                "Y": 0.363745391368866
            },
            "GazePoint3D": {
                "X": -130.047988891602,
                "Y": 208.728134155273,
                "Z": 59.1125564575195
            },
            "EyePosition3D": { 
                "X": -41.76611328125,
                "Y": 8.7668342590332, 
                "Z": 661.870300292969
            },
            "PupilDiameter": 3.10238647460938
        },
        "RightEye": {
            // same structure as for LeftEye    
        }
    },
    // other samples...
]
```

```csv
Timestamp,LeftValidity,LeftGazePoint2DX,LeftGazePoint2DY,LeftGazePoint3DX,LeftGazePoint3DY,LeftGazePoint3DZ,LeftEyePosition3DX,LeftEyePosition3DY,LeftEyePosition3DZ,LeftPupilDiameter,RightValidity,RightGazePoint2DX,RightGazePoint2DY,RightGazePoint3DX,RightGazePoint3DY,RightGazePoint3DZ,RightEyePosition3DX,RightEyePosition3DY,RightEyePosition3DZ,RightPupilDiameter
2019-02-20T14:30:19.8526299+01:00,Valid,0.249588146805763,0.363745391368866,-130.047988891602,208.728134155273,59.1125564575195,-41.76611328125,8.7668342590332,661.870300292969,3.10238647460938,Valid,0.216417774558067,0.350670248270035,-147.382720947266,212.744323730469,60.574333190918,21.4870147705078,4.3505334854126,660.007873535156,3.25660705566406
```

#### Flexible Timestamp Formatting

Although the timestamp of the example of input gaze data above is stored in the `Timestamp` field with a string value of both date and time, you may use other formats or field with different name. 
Timestamp serialization in GazeToolkit is borrowed from the [UXIsk Data Serialization Library](https://github.com/uxifiit/UXI.Serialization) and it supports these formats: 

* `date` for a formatted string of date and time (as shown above),
* `time` for formatted string of time value, or
* `ticks` for ticks in a chosen precision from an arbitrary timepoint (hundred nanoseconds by default, or microseconds, milliseconds). 

Example values:
* `date` - `2019-02-20T14:30:19.8526299+01:00`
* `time` - `14:30:19.8526299`
* `ticks:ns` (hundred nanoseconds) - `636862698198526299`
* `ticks:us` (microseconds, e.g., from the epoch 01/01/1970) - `1550669419852629.9`
* `ticks:ms` (milliseconds from the epoch 01/01/1970) - `1550669419852.63`

Use `--timestamp-format` option in gaze filters to specify format and `--timestamp-field` to specify name of the field with timestamp in your data.

### Fixation Filtering Example

Fixations filtering of gaze data with the velocity threshold filter `i-vt.exe`:

1. Prepare your data of `UXI.GazeToolkit.GazeData` objects in JSON or CSV format (for example `gaze.json`).
2. Run the `i-vt.exe` to classify gaze data in `gaze.json` into eye movements to `movements.json`, the caret character (`^`) is used to split up long line on the Windows command line:

```
i-vt.exe gaze.json ^
         --timestamp-format date ^
         --timestamp-field Timestamp ^
         --format JSON ^
         --frequency 60 ^
         --fillin --fillin-max-gap 75 ^
         --select Average ^
         --denoise --denoise-alpha 0.3 ^
         --threshold 30 ^
         --merge --merge-max-gap 75 --merge-max-angle 0.5 ^
         --discard --discard-min-duration 60 ^
         --output-format JSON ^
         --output movements.json
```

Description of used parameters: 
* `--timestamp-format` - format of timestamps in data, see [Flexible Timestamp Formatting](#Flexible-Timestamp-Formatting) above for more details. 
* `--timestamp-field` - name of the timestamp field in data; if not specified, `Timestamp` is used.
* `--format` - format of the input data; if ommitted, it is resolved from the input filename extension, or `JSON` is used, if the extension was not recognized or the input file was not specified (input is read from the standard input stream).
* `--frequency` - frequency of input data in number of samples per second.
* `--fillin` - enables interpolating missing data with max gap set in `--fillin-max-gap`.
* `--select` - which eye to select from the input gaze data: `Left`, `Right`, `Average` (average of both or any eye with valid data in the sample) or `StrictAverage` (average only if data of both eyes are valid, otherwise it is invalid sample).
* `--denoise` - exponential smoothing of gaze data with alpha value set with `--denoise-alpha`. 
* `--threshold` - threshold value for the classification algorithm in degrees per second.
* `--merge` - enables merging close fixations in time with `--merge-max-gap` and `--merge-max-angle` parameters for maximum gap (time in milliseconds) and visual angle (degrees).
* `--discard` - enables re-classifying fixations shorter than a threshold value set with `--discard-min-duration` to unknown movements (invalid data).
* `--output-format` - defines format of the output file, similar to `--format` option for input; if omitted, it is resolved from the output filename extension, or `JSON` is used if the extension was not recognized or the output file was not specified (output is written to the standard output stream).
* `--output` - path to the output file which will contain JSON array of [EyeMovement](src/UXI.GazeToolkit/EyeMovement.cs) objects - each containing either a `Fixation`, `Saccade`, or an `Unknown` movement.

Use `--help` to see default values for each option that are used, if certain options are specified. The same filter can be executed without default values:

```
i-vt.exe gaze.json ^
         --timestamp-format date ^
         --frequency 60 ^
         --fillin ^
         --select Average ^
         --denoise ^
         --merge ^
         --discard ^
         --output movements.json
```


### Fixation Filtering with Individual Filters

The exactly same process from the previous example can be executed with individual filters chained together with system pipes. However, we must always define format of timestamp, as well as data format for input and output (if other than default `JSON`), because it can not be resolved from the filenames.

The following command reads raw gaze data from `gaze.json` with timestamps in the data and time format:

```
fillingaps.exe --timestamp-format date < gaze.json | ^
select.exe --select Average --timestamp-format date | ^
denoise.exe exponential --alpha 0.3 --timestamp-format date | ^
velocity.exe --frequency 60 --timestamp-format date | ^
vt-classify.exe --threshold 30 --timestamp-format date | ^
merge.exe --max-gap 75 --max-angle 0.5 --timestamp-format date | ^
discard.exe --min-duration 60 --timestamp-format date > movements.json
```


### Using the CSV serialization format

JSON is the default serialization format of gaze filters, but CSV can also be used. Filters can read data in one format and output in other too. Use `--format` for specifying input data format and `--output-format` for output data format. If the data is read from or written to a file with `.json` or `.csv` extension, the formatting options can be ommitted.

The following line will execute `Filter` (see [Other Filters](#Other-Filters)) to read `GazeData` objects from `gaze.json` in JSON format with timestamps in `date` format and write them to `gaze.csv` in CSV format.

```
filter.exe gaze gaze.json --timestamp-format date --output gaze.csv 
```


## Installation


The following libraries from this repository are available as NuGet packages:

|Library |Package|
|--------|:-----:|
|UXI.GazeToolkit|[![UXI.GazeToolkit package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/875e4574-b18a-49ff-8cf1-55b220af2355/_apis/public/Packaging/Feeds/f25beb4b-f7d5-4466-9073-a54052469941/Packages/d722b395-dbe8-41c8-9dd3-d1fcbf733861/Badge)](https://dev.azure.com/uxifiit/UXI.Libs/_packaging?_a=package&feed=f25beb4b-f7d5-4466-9073-a54052469941&package=d722b395-dbe8-41c8-9dd3-d1fcbf733861&preferRelease=true)|
|UXI.GazeToolkit.Serialization|[![UXI.GazeToolkit.Serialization package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/875e4574-b18a-49ff-8cf1-55b220af2355/_apis/public/Packaging/Feeds/f25beb4b-f7d5-4466-9073-a54052469941/Packages/d19b3e2d-813c-40d3-8a58-3381bd21822a/Badge)](https://dev.azure.com/uxifiit/UXI.Libs/_packaging?_a=package&feed=f25beb4b-f7d5-4466-9073-a54052469941&package=d19b3e2d-813c-40d3-8a58-3381bd21822a&preferRelease=true)|
|UXI.GazeToolkit.Validation|[![UXI.GazeToolkit.Validation package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/875e4574-b18a-49ff-8cf1-55b220af2355/_apis/public/Packaging/Feeds/f25beb4b-f7d5-4466-9073-a54052469941/Packages/5c0d6bde-1556-40b2-908b-eba6d17fec24/Badge)](https://dev.azure.com/uxifiit/UXI.Libs/_packaging?_a=package&feed=f25beb4b-f7d5-4466-9073-a54052469941&package=5c0d6bde-1556-40b2-908b-eba6d17fec24&preferRelease=true)|
|UXI.GazeFilter|[![UXI.GazeFilter package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/875e4574-b18a-49ff-8cf1-55b220af2355/_apis/public/Packaging/Feeds/f25beb4b-f7d5-4466-9073-a54052469941/Packages/e73b111c-0101-42fe-b1c6-5df5f6501fa8/Badge)](https://dev.azure.com/uxifiit/UXI.Libs/_packaging?_a=package&feed=f25beb4b-f7d5-4466-9073-a54052469941&package=e73b111c-0101-42fe-b1c6-5df5f6501fa8&preferRelease=true)|

NuGet packages are available in the public Azure DevOps artifacts repository shared with [UXI.Libs](https://github.com/uxifiit/UXI.Libs):
```
https://pkgs.dev.azure.com/uxifiit/UXI.Libs/_packaging/Public/nuget/v3/index.json
```


### Add uxifiit/UXI.Libs package source
First, add a new package source to the solution or Visual Studio. Choose the way that fits you the best:
* Add new package source in [Visual Studio settings](https://docs.microsoft.com/en-us/azure/devops/artifacts/nuget/consume?view=azure-devops).
* Add new package source with the repository URL through command line:
```
nuget source Add -Name "UXI.Libs Public Feed" -Source "https://pkgs.dev.azure.com/uxifiit/UXI.Libs/_packaging/Public/nuget/v3/index.json"
```
* Create `NuGet.config` file in your project's solution directory where you specify this package source:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="UXI.Libs Public Feed" value="https://pkgs.dev.azure.com/uxifiit/UXI.Libs/_packaging/Public/nuget/v3/index.json" />
  </packageSources>
  <disabledPackageSources />
</configuration>
```


### Install UXIsk GazeToolkit packages

Then install the package to your project using the Visual Studio "Manage NuGet Packages..." window or use the Package Manage Console:
```
PM> Install-Package UXI.GazeToolkit
```
```
PM> Install-Package UXI.GazeToolkit.Serialization
```
```
PM> Install-Package UXI.GazeToolkit.Validation
```

```
PM> Install-Package UXI.GazeFilter
```


## Contributing

Use [Issues](issues) to request features, report bugs, or discuss ideas.

## Dependencies

* [UXIsk Data Serialization Library](https://github.com/uxifiit/UXI.Serialization)
* [UXIsk Filters Framework](https://github.com/uxifiit/Filters)
* [Rx.NET](https://github.com/Reactive-Extensions/Rx.NET)
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
* [CsvHelper](https://github.com/JoshClose/CsvHelper)
* [CommandLineParser](https://github.com/commandlineparser/commandline)

## Authors

* Martin Konopka - [@martinkonopka](https://github.com/martinkonopka)

## License

UXI.Filters is licensed under the 3-Clause BSD License - see [LICENSE.txt](LICENSE.txt) for details.

Copyright (c) 2019 Martin Konopka and Faculty of Informatics and Information Technologies, Slovak University of Technology in Bratislava.

## Contacts

* UXIsk 
  * User eXperience and Interaction Research Center, Faculty of Informatics and Information Technologies, Slovak University of Technology in Bratislava
  * Web: https://www.uxi.sk/
* Martin Konopka
  * E-mail: martin (underscore) konopka (at) stuba (dot) sk

## References

1. Anneli Olsen. 2012. The Tobii I-VT Fixation Filter: Algorithm description. Whitepaper, Tobii Pro, 2012, March 20, 21 p. Available at: [Eye movement classification - Tobii Pro](https://www.tobiipro.com/learn-and-support/learn/steps-in-an-eye-tracking-study/data/how-are-fixations-defined-when-analyzing-eye-tracking-data/)
