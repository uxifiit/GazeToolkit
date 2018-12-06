# UXIsk GazeToolkit

[![NuGet Package](https://img.shields.io/myget/uxifiit/v/UXI.GazeToolkit.svg)](https://www.myget.org/feed/uxifiit/package/nuget/UXI.GazeToolkit)

Toolkit for stream processing of eye movement data in C#/.NET.
Contains gaze data filtering, mostly for eye movement classification into fixations/saccades, smoothing, eye selection, filling-in gaps, etc.
Gaze data filters are available as separate console applications for detailed data analysis and tuning parameters of fixation filtering. 

UXI GazeToolkit does not directly depend on any SDK from eye tracker vendors.

## Overview

Main features:

* I-VT fixation filter implementation based on the Tobii Pro whitepaper [1].
* Each step of the I-VT fixation filter is implemented separately:
  * Gaps Fill In - interpolates missing data.
  * Eye Selection: Left, Right, Average, Strict Average
  * Noise Reduction: Exponential Smoothing, Moving Average (not implemented), Median (not implemented).
  * Frequency Measure - measures frequency of data.
  * Velocity Calculation - evaluates angular velocity of eyes.
  * Velocity Threshold Classification - classifies eye movements into fixations or saccades based on their velocity.
  * Merge Adjacent Fixations
  * Discard Short Fixations
* All steps are implemented as operators on observable data streams, i.e., `IObservable<T>` with Rx.NET. 


Projects in these repository:

* `UXI.GazeToolkit` - class library with gaze data classes, filters and utilities, including implementation of the I-VT fixation filter. 
* Set of `UXI.GazeFilter.*` console applications, each for single gaze data filter. Multiple filters may be chained together using pipes. 


## Example Usage

Fixations filtering of gaze data with `UXI.GazeFilters.*`:

1. Build `UXI.GazeFilter.*` applications with Microsoft Visual Studio 2015. 
2. Convert your data into JSON array of `UXI.GazeToolkit.GazeData` objects (`dataset.json`).
3. In `/build/[Debug|Release]/` folder find executable filters
4. Run the following command to classify fixations for input file `dataset.json` with these filters:

```
fillingaps -g 75 < dataset.json | select -s Average | denoise exponential -a 0.3 | velocity -f 60 | vt-classify -t 30 | merge -g 75 | discard -d 60 > fixations.json
```
5. Filters and parameters description (these are the default values): 
   1. `fillingaps -g 75` - Gaps Fill In with *MaxGapLength* of 75ms. All gaps longer than this threshold are ignored.
   2. `select -s Average` - Selects single eye. If data of both eyes in the sample is valid, averages them into single eye, otherwise takes the only valid eye data or none.
   3. `denoise exponential -a 0.3` - Reduces noise in the data with *exponential* smoothing with *alpha* parameter set to 0.3.
   4. `velocity -f 60` - Measures angular velocity of eyes in data with *frequency* of 60 Hz (adjust based on your tracker device).
   5. `vt-classify -t 30` - Classifies each eye movement sample into:
      * `Saccade`, if the velocity was higher than *threshold* speed of 30°/s;
      * `Fixation`, if the velocity was lower than the *threshold*;
      * `Unknown`, if the sample is invalid. 
   6. `merge -g 75` - Merges adjacent fixations if the time gap between each fixation is lower or equal than the 75ms.
   7. `discard -d 60` - Discards fixations with duration shorter than 60ms. Such fixations are reclassified as `Unknown` movements. 

6. Output file `fixations.json` contains JSON array of [EyeMovement](src/UXI.GazeToolkit/EyeMovement.cs) objects - each containing either a `Fixation`, `Saccade`, or an `Unknown` movement.
7. You can omit certain steps in the pipeline, or save intermediate results of each filter for analysis or visualization.


## Contributing

Use [Issues](issues) to request features, report bugs, or discuss ideas.

## Dependencies

* [Rx.NET](https://github.com/Reactive-Extensions/Rx.NET)
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
* [CsvHelper](https://github.com/JoshClose/CsvHelper)
* [CommandLineParser](https://github.com/commandlineparser/commandline)

## Authors

* Martin Konopka - [@martinkonopka](https://github.com/martinkonopka)

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details

## Contacts

* UXIsk 
  * User eXperience and Interaction Research Center, Faculty of Informatics and Information Technologies, Slovak University of Technology in Bratislava
  * Web: https://www.uxi.sk/
* Martin Konopka
  * E-mail: martin (underscore) konopka (at) stuba (dot) sk

## References

1. Anneli Olsen. 2012. The Tobii I-VT Fixation Filter: Algorithm description. Whitepaper, Tobii Pro, 2012, March 20, 21 p. Available at: [Eye movement classification - Tobii Pro](https://www.tobiipro.com/learn-and-support/learn/steps-in-an-eye-tracking-study/data/how-are-fixations-defined-when-analyzing-eye-tracking-data/)
