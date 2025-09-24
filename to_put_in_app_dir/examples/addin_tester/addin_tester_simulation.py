#-------------------------------------------------------------------------------
#
# Test simulation to check integrity of Chrono::Solidworks Addin exporter
# with respect to:
#     1) bodies
#     2) links
#     3) visual shapes, colors, transparencies
#     4) collision shapes
#     5) markers
#     6) rigid sub-assemblies, flexible sub-assemblies
#
#-------------------------------------------------------------------------------

import pychrono as chrono
import pychrono.irrlicht as chronoirr

sys = chrono.ChSystemNSC()
sys.SetCollisionSystemType(chrono.ChCollisionSystem.Type_BULLET)
sys.SetGravitationalAcceleration(chrono.ChVector3d(0, -9.81, 0))

filepath = "./test_py_export.py"
imported_items = chrono.ImportSolidWorksSystem(filepath)
for ii in imported_items:
    sys.Add(ii)

# Customize contact material
mat = chrono.ChContactMaterialNSC()
mat.SetFriction(0.3)
mat.SetRestitution(0.1)
for ii in sys.GetBodies():
    if (ii.GetCollisionModel()):
        ii.GetCollisionModel().SetAllShapesMaterial(mat)

# sys.RemoveRedundantConstraints(False, 1e-6, True)
# sys.DoAssembly(chrono.FULL)

vis = chronoirr.ChVisualSystemIrrlicht()
vis.AttachSystem(sys)
vis.SetWindowSize(800, 600)
vis.SetWindowTitle("Chrono::Solidwords Addin Tester")
vis.Initialize()
vis.AddLogo()
vis.AddSkyBox()
vis.AddCamera(chrono.ChVector3d(0, 0, 5), chrono.ChVector3d(0, 0, 0))
vis.AddTypicalLights()
vis.EnableCollisionShapeDrawing(True)

timestep = 0.002
rt_timer = chrono.ChRealtimeStepTimer()

solver = chrono.ChSolverBB()
solver.SetMaxIterations(500)
solver.SetTolerance(1e-6)
# solver.EnableWarmStart(True)
# solver.EnableDiagonalPreconditioner(True)
sys.SetSolver(solver)

while vis.Run():
    vis.BeginScene()
    vis.Render()    
    if not vis.GetUtilityFlag():
        sys.DoStepDynamics(timestep)
    vis.EndScene()
    rt_timer.Spin(timestep)