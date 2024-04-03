
#include "chrono/physics/ChSystemNSC.h"
#include "chrono/physics/ChBodyEasy.h"
#include "chrono/physics/ChLinkMotorRotationSpeed.h"
#include "chrono/assets/ChTexture.h"
#include "chrono/core/ChRealtimeStep.h"
#include "chrono/serialization/ChArchiveJSON.h"

#undef CHRONO_VSG

#ifdef CHRONO_COLLISION
    #include "chrono/collision/multicore/ChCollisionSystemMulticore.h"
#endif

#ifdef CHRONO_IRRLICHT
    #include "chrono_irrlicht/ChVisualSystemIrrlicht.h"
using namespace chrono::irrlicht;
#endif
#ifdef CHRONO_VSG
    #include "chrono_vsg/ChVisualSystemVSG.h"
using namespace chrono::vsg3d;
#endif

using namespace chrono;

ChVisualSystem::Type vis_type = ChVisualSystem::Type::IRRLICHT;

ChCollisionSystem::Type collision_type = ChCollisionSystem::Type::BULLET;


int main(int argc, char* argv[]) {

    SetChronoDataPath(CHRONO_DATA_DIR);

    std::cout << "Copyright (c) 2023 projectchrono.org\nChrono version: " << CHRONO_VERSION << "\n\n";

    // Create the physical system
    ChSystemNSC sys;
    sys.SetCollisionSystemType(collision_type);

    // Settings specific to Chrono multicore collision system
    if (collision_type == ChCollisionSystem::Type::MULTICORE) {
#ifdef CHRONO_COLLISION
        auto collsys = std::static_pointer_cast<ChCollisionSystemMulticore>(sys.GetCollisionSystem());
        // Change the default number of broadphase bins
        collsys->SetBroadphaseGridResolution(ChVector3i(10, 10, 2));
        // Change default narrowphase algorithm
        ////collsys->SetNarrowphaseAlgorithm(ChNarrowphase::Algorithm::MPR);
        collsys->SetEnvelope(0.005);
        // Enable active bounding box
        collsys->EnableActiveBoundingBox(ChVector3d(-10, -10, -20), ChVector3d(+10, +10, +10));
        // Set number of threads used by the collision detection system
        collsys->SetNumThreads(4);
#endif
    }


    std::string jsonfile = SOLIDWORKS_EXPORTED_JSON;
    std::ifstream mfilei(jsonfile.c_str());

    // Use a JSON archive object to deserialize C++ objects from the file
    ChArchiveInJSON marchivein(mfilei);
    marchivein >> CHNVP(sys, "sys");



    // Create the run-time visualization system
#ifndef CHRONO_IRRLICHT
    if (vis_type == ChVisualSystem::Type::IRRLICHT)
        vis_type = ChVisualSystem::Type::VSG;
#endif
#ifndef CHRONO_VSG
    if (vis_type == ChVisualSystem::Type::VSG)
        vis_type = ChVisualSystem::Type::IRRLICHT;
#endif

    std::shared_ptr<ChVisualSystem> vis;
    switch (vis_type) {
        case ChVisualSystem::Type::IRRLICHT: {
#ifdef CHRONO_IRRLICHT
            auto vis_irr = chrono_types::make_shared<ChVisualSystemIrrlicht>();
            vis_irr->SetWindowSize(1024, 768);
            vis_irr->SetWindowTitle("ChronoSolidworksImportJSON");
            vis_irr->Initialize();
            vis_irr->AddLogo();
            vis_irr->AddSkyBox();
            vis_irr->AddTypicalLights();
            vis_irr->AddCamera(ChVector3d(1, 1, 6), ChVector3d(0, 0, 0));
            vis_irr->AttachSystem(&sys);
            vis_irr->EnableCollisionShapeDrawing(true);

            vis = vis_irr;
#endif
            break;
        }
        default:
        case ChVisualSystem::Type::VSG: {
#ifdef CHRONO_VSG
            auto vis_vsg = chrono_types::make_shared<ChVisualSystemVSG>();
            vis_vsg->AttachSystem(&sys);
            vis_vsg->SetWindowTitle("ChronoSolidworksImportJSON");
            vis_vsg->AddCamera(ChVector<>(0, 1, -1));
            vis_vsg->SetWindowSize(ChVector2<int>(1024, 768));
            vis_vsg->SetWindowPosition(ChVector2<int>(100, 100));
            vis_vsg->SetClearColor(ChColor(0.8f, 0.85f, 0.9f));
            vis_vsg->SetUseSkyBox(true);  // use built-in path
            vis_vsg->SetCameraVertical(CameraVerticalDir::Y);
            vis_vsg->SetCameraAngleDeg(40.0);
            vis_vsg->SetLightIntensity(1.0f);
            vis_vsg->SetLightDirection(1.5 * CH_C_PI_2, CH_C_PI_4);
            vis_vsg->SetWireFrameMode(false);
            vis_vsg->Initialize();

            vis = vis_vsg;
#endif
            break;
        }
    }



    // Modify some setting of the physical system for the simulation, if you want
    sys.SetSolverType(ChSolver::Type::BARZILAIBORWEIN);
    sys.GetSolver()->AsIterative()->SetMaxIterations(250);
    ////sys.SetUseSleeping(true);

    // Simulation loop
    ChRealtimeStepTimer rt;
    double step_size = 0.01;

    while (vis->Run()) {
        vis->BeginScene();
        vis->Render();
        vis->EndScene();

        sys.DoStepDynamics(step_size);

        ////std::cout << std::endl;
        ////auto frc = mixer->GetAppliedForce();
        ////auto trq = mixer->GetAppliedTorque();
        ////std::cout << sys.GetChTime() << "  force: " << frc << "  torque: " << trq << std::endl;
        ////auto c_frc = mixer->GetContactForce();
        ////auto c_trq = mixer->GetContactTorque();
        ////std::cout << sys.GetChTime() << "  ct force: " << c_frc << "  ct torque: " << c_trq << std::endl;

        rt.Spin(step_size);
    }

    return 0;
}
