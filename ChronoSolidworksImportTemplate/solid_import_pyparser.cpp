// =============================================================================
// PROJECT CHRONO - http://projectchrono.org
//
// Copyright (c) 2014 projectchrono.org
// All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found
// in the LICENSE file at the top level of the distribution and at
// http://projectchrono.org/license-chrono.txt.
//
// =============================================================================
// Authors: Alessandro Tasora
// =============================================================================
//
// Demo code about
// - loading a SolidWorks .py file saved with the Chrono::Engine add-in,
// - showing the system in Irrlicht.
//
// =============================================================================

#include "chrono_parsers/ChParserPython.h"
#include "chrono/core/ChRealtimeStep.h"

#include "chrono_irrlicht/ChVisualSystemIrrlicht.h"

using namespace chrono;
using namespace chrono::parsers;
using namespace chrono::irrlicht;

using namespace irr;

using namespace core;
using namespace scene;
using namespace video;
using namespace io;
using namespace gui;

int main(int argc, char* argv[]) {
    GetLog() << "Copyright (c) 2017 projectchrono.org\nChrono version: " << CHRONO_VERSION << "\n\n";

    SetChronoDataPath(CHRONO_DATA_DIR);

    // Cache current path to Chrono data files
    auto data_path = GetChronoDataPath();

    // Create a Chrono::Engine physical system
    ChSystemNSC sys;

    // Set the collision margins. This is expecially important for
    // very large or very small objects! Do this before creating shapes.
    ChCollisionModel::SetDefaultSuggestedEnvelope(0.001);
    ChCollisionModel::SetDefaultSuggestedMargin(0.001);
    sys.SetCollisionSystemType(ChCollisionSystem::Type::BULLET);

    //
    // LOAD THE SYSTEM
    //

    // The Python engine. This is necessary in order to parse the files that
    // have been saved using the SolidWorks add-in for Chrono::Engine.

    ChPythonEngine my_python;

    try {
        // This is the instruction that loads the .py (as saved from SolidWorks) and
        // fills the system.
        //   In this example, we load a mechanical system that represents
        // a (quite simplified & approximated) clock escapement, that has been
        // modeled in SolidWorks and saved using the Chrono Add-in for SolidWorks.

        my_python.ImportSolidWorksSystem(SOLIDWORKS_EXPORTED_PY,
                                         sys);  // note, don't type the .py suffix in filename..

    } catch (const ChException& myerror) {
        GetLog() << myerror.what();
    }

    // From this point, your ChSystem has been populated with objects and
    // assets load from the .py files. So you can proceed and fetch
    // single items, modify them, or add constraints between them, etc.
    // For example you can add other bodies, etc.

    // Log out all the names of the items inserted in the system:
    GetLog() << "SYSTEM ITEMS: \n";
    sys.ShowHierarchy(GetLog());


    //
    // THE VISUALIZATION
    //

    sys.SetSolverType(ChSolver::Type::BARZILAIBORWEIN);
    sys.SetSolverMaxIterations(400);

    // Create the Irrlicht visualization system
    auto vis = chrono_types::make_shared<ChVisualSystemIrrlicht>();
    vis->SetWindowSize(1024, 768);
    vis->SetWindowTitle("ChronoSolidworksImportPyParser");
    vis->Initialize();
    vis->AddLogo();
    vis->AddSkyBox();
    vis->AddTypicalLights();
    vis->AddCamera(ChVector<>(1, 1, 6), ChVector<>(0, 0, 0));
    vis->AttachSystem(&sys);

    vis->EnableCollisionShapeDrawing(true);

    //
    // THE SIMULATION LOOP
    //

    // set a low stabilization value because objects are small!
    sys.SetMaxPenetrationRecoverySpeed(0.002);

    // Simulation loop
    double timestep = 0.002;
    ChRealtimeStepTimer realtime_timer;
    while (vis->Run()) {
        vis->BeginScene();
        vis->Render();
        vis->EndScene();
        sys.DoStepDynamics(timestep);
        realtime_timer.Spin(timestep);
    }

    return 0;
}
