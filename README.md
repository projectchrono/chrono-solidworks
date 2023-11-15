CHRONO::SOLIDWORKS
==================

Chrono::SolidWorks is a part of [Project Chrono](https://www.projectchrono.org). It is an add-in for the popular [SolidWorks](https://www.solidworks.com) 3D CAD, it allows to export mechanisms modeled in SolidWorks as .py, .cpp, or .json files that can be loaded and simulated with Chrono::Engine.


Main features
-------------

* add-in for [SolidWorks](https://www.solidworks.com)
* simple graphical interface to export mechanical systems including bodies, assemblies, constraints into Chrono::Engine
* SolidWorks parts, in assemblies, become rigid bodies in Chrono::Engine
* sub-assembles are exported as articulated or single bodies, depending on SolidWorks 'flexible' or 'rigid' solve flag
* inertia and mass properties of parts are correctly exported
* visualization shapes are exported as .obj meshes, for later editing or asset optimization
* coordinate systems are exported as ChMarker
* tool for creating custom collision shapes using SolidWorks interface
* tool for creating custom Chrono motors using SolidWorks interface

For more informations look at https://www.projectchrono.org 

How to install the add-in
------------------------------------
Visit the ProjectChrono website at the [pre-compiled binary webpage](https://www.projectchrono.org/download/) and look for the Chrono::Solidworks section.
The installer will prompt the user to locate the Solidworks directory. Dependencies will be installed together with the add-in.


How to build the add-in from source (advanced users)
------------------------------------
There is no need to build the add-in from source unless you are interested in expanding/fixing the add-in.

* the project is written in C#, so you must use Microsoft Visual Studio with the C# development module enabled
* double check the availability of the proper .NET Framework version on your computer
* you must have a [SolidWorks](https://www.solidworks.com) license installed on your computer
* clone this repository to any given folder (always suggested to avoid spaces and special characters in the path)
* run Visual Studio as Administrator
* load the **ChronoSolidworks.sln** solution located in the main directory
* set build configuration to *Release*
* right-click on the *ChronoSolidworksAddIn* target, click on *Manage NuGet Packages* and install the *Newtonsoft.JSON* package
* you may need to modify some settings of this solution, in order to reference the .COM assemblies of your SolidWorks
  In the Solution Explorer panel, expand *ChronoSolidworksAddIn*>*References* and make sure that the four references called "SolidWorks...." are properly set. If not:
  + right click on *References*>*Add Reference...*
  + go to the *Browse* tab and navigate to the SolidWorks installation folder
     (typically *C:\Program Files\SolidWorks 20XX\SolidWorks*) and add the following files (names might change depending on Solidworks version): 
    - *solidworkstools.dll*
    - *SolidWorks.Interop.sldworks.dll*
    - *SolidWorks.Interop.swcommands.dll*
    - *SolidWorks.Interop.swconst.dll*
    - *SolidWorks.Interop.swpublished.dll*  
* right-click on *ChronoSolidworksAddIn* project, click on *Properties*, *Reference Paths* and add the Solidworks installation folder
* build the solution. It will generate the **ChronoEngineAddIn.dll** into the *chrono-solidworks_install* together with all its dependencies; the DLLs are automatically registered by the post-build events
* start SolidWorks, you should find the Chrono::Engine panel in the Task Pane to the right. Note that add-ins can be enabled/disabled with the *Tools>Adds-In* menu in the toolbar

If you find problems to build the add-in, look at [this tutorial](https://www.angelsix.com/cms/products/tutorials/64-solidworks/67-creating-a-solidworks-add-in-from-scratch) for instructions about how to build add-ins for SolidWorks.

  
How to use the Chrono::SolidWorks add-in
----------------------------------------

See the [tutorials](https://api.projectchrono.org/development/tutorial_table_of_content_chrono_solidworks.html) for examples of C++ code and Python code that load systems exported with this add-in.

A place for discussions can be the [projectchrono group](https://groups.google.com/forum/#!forum/projectchrono).

