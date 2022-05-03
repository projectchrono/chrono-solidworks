CHRONO::SOLIDWORKS
==================

Chrono::SolidWorks is a part of [Project Chrono](http://www.projectchrono.org). It is an add-in for the popular [SolidWorks](http://www.solidworks.com) 3D CAD, it allows to export mechanisms modeled in SolidWorks as .py files that can be load and simulated with Chrono::Engine.


Main features
-------------

* add-in for [SolidWorks](http://www.solidworks.com)
* once installed, the user finds a new panel in the CAD interface, with buttons for exporting mechanical systems into Chrono::Engine 
* most SolidWorks constraints are converted in Chrono::Engine constraints
* SolidWorks parts, in assemblies, become rigid bodies in Chrono::Engine
* sub-assembles are exported as articulated or single bodies
* inertia and mass properties of parts are correctly exported
* visualization shapes are exported as .obj meshes, for later editing or asset optimization
* coordinate systems are exported as markers
* tool for creating custom collision shapes using SolidWorks interface

For more informations look at http://www.projectchrono.org 


How to install and build the library
------------------------------------

* the project is written in C#, so you must use Microsoft Visual Studio
* you must have a [SolidWorks](http://www.solidworks.com) license installed on your computer.  
* run Visual Studio as Administrator
* load the .sln file in Visual Studio
* you may need to modify some settings of the solution, in order to reference the .COM assemblies of your SolidWorks. In the Solution Explorer, there should be four references called "SolidWorks....", if they are not active or are missing, do this: from the Solution Explorer, right-click on the Project and select Add Reference... Go to the Browse tab and navigate to the SolidWorks installation folder (typically C:\Program Files\SolidWorks 20XX\SolidWorks) and add the .dll files: "solidworkstools.dll" "SolidWorks.Interop.sldworks.dll" "SolidWorks.Interop.swcommands.dll" "SolidWorks.Interop.swconst.dll" "SolidWorks.Interop.swpublished.dll". In some releases of Solidorks you may find different names.
* also, depending on the directory where you installed SolidWorks, you may need to select the project in the solution explorer, open 'Properties', go to 'Build' tab, and set the 'Output path:' as the same directory of all other SolidWorks dlls. For example on my laptop it is "C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\".
* build the solution
* start SolidWorks, then you should find the Chrono::Engine panel in the Task Pane to the right. Note that add-ins can be enabled/disabled with the 'Adds-In' menu in the toolbar.

If you find problems to build the add-in, look at [this tutorial](http://www.angelsix.com/cms/products/tutorials/64-solidworks/67-creating-a-solidworks-add-in-from-scratch) for instructions about how to build add-ins for SolidWorks.

  
How to use the Chrono::SolidWorks add-in
----------------------------------------

See the [tutorials](http://www.projectchrono.org/mediawiki/index.php/Tutorials) for examples of C++ code and Python code that load systems exported with this add-in

A place for discussions can be the [projectchrono group](https://groups.google.com/forum/#!forum/projectchrono).

