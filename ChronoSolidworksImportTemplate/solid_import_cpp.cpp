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
// A very simple example that can be used as template project for
// a Chrono::Engine simulator with 3D view.
// =============================================================================

#include "chrono/physics/ChSystemNSC.h"
#include "chrono/physics/ChBodyEasy.h"
#include "chrono/physics/ChLinkMate.h"
#include "chrono/assets/ChTexture.h"
#include "chrono/core/ChRealtimeStep.h"

#include "chrono_irrlicht/ChVisualSystemIrrlicht.h"
#include SOLIDWORKS_EXPORTED_HEADER

// Use the namespace of Chrono
using namespace chrono;
using namespace chrono::irrlicht;

int main(int argc, char* argv[]) {
    // Set path to Chrono data directory
    SetChronoDataPath(CHRONO_DATA_DIR);

    GetLog() << "Copyright (c) 2023 projectchrono.org\nChrono version: " << CHRONO_VERSION << "\n\n";

    
    // Create a Chrono physical system
    ChSystemNSC sys;

    sys.SetCollisionSystemType(chrono::ChCollisionSystem::Type::BULLET);

    std::unordered_map<std::string, std::shared_ptr<chrono::ChFunction>> motfun_map;
    ImportSolidworksSystemCpp(sys, &motfun_map);

    sys.SetSolverType(ChSolver::Type::BARZILAIBORWEIN);
    sys.SetSolverMaxIterations(400);

    // 4 - Create the Irrlicht visualization system
    ChVisualSystemIrrlicht vis;
    vis.SetWindowSize(1024, 768);
    vis.SetWindowTitle("ChronoSolidworksImportCPP");
    vis.Initialize();
    vis.AddLogo();
    vis.AddSkyBox();
    vis.AddTypicalLights();
    vis.AddCamera(ChVector<>(1, 1, 6), ChVector<>(0, 0, 0));
    vis.AttachSystem(&sys);

    vis.EnableCollisionShapeDrawing(true);


    // 5 - Simulation loop
    ChRealtimeStepTimer realtime_timer;
    double step_size = 0.01;

    while (vis.Run()) {
        // Render scene
        vis.BeginScene();
        vis.Render();
        vis.EndScene();

        // Perform the integration stpe
        sys.DoStepDynamics(step_size);

        // Spin in place to maintain soft real-time
        realtime_timer.Spin(step_size);
    }

    return 0;
}
