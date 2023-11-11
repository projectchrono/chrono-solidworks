'''
Test simulation to check integrity of Chrono::Solidworks Addin exporter
with respect to:
    1) bodies
    2) links
    3) visual shapes, colors, transparencies
    4) collision shapes
    5) markers
    6) rigid sub-assemblies, flexible sub-assemblies
'''

import pychrono as chrono
import pychrono.irrlicht as chronoirr

system = chrono.ChSystemNSC()
system.Set_G_acc(chrono.ChVectorD(0, -9.81, 0))

filepath = "./addin_tester_export"
imported_items = chrono.ImportSolidWorksSystem(filepath)
for ii in imported_items:
    system.Add(ii)

system.ShowHierarchy(chrono.GetLog())

# Customize contact material
mat = chrono.ChMaterialSurfaceNSC()
mat.SetFriction(0)
mat.SetRestitution(1)
for ii in system.Get_bodylist():
    ii.GetCollisionModel().SetAllShapesMaterial(mat)

# Add a spring
marker_spring = system.SearchMarker('MARKER_SPRING')
ground = system.SearchBody('ground')
part_4_3 = system.SearchBody('Part4-3')

spring = chrono.ChLinkTSDA()
spring.Initialize(part_4_3, ground, False, marker_spring.GetAbsFrame().GetPos(), marker_spring.GetAbsFrame().GetPos() + chrono.ChVectorD(0, 0.5, 0))
spring.SetSpringCoefficient(10000)
spring.SetDampingCoefficient(0)
spring.AddVisualShape(chrono.ChSpringShape())
system.Add(spring) 

# system.DoFullAssembly()
# system.RemoveRedundantConstraints(False, 1e-6, True)
# system.DoFullAssembly()

vis = chronoirr.ChVisualSystemIrrlicht(system, chrono.ChVectorD(0, 0, 5))

vis.EnableCollisionShapeDrawing(True)

timestep = 0.01
rt_timer = chrono.ChRealtimeStepTimer()

solver = chrono.ChSolverBB()
solver.SetMaxIterations(500)
# solver.EnableWarmStart(True)
# solver.EnableDiagonalPreconditioner(True)
system.SetSolver(solver)

while vis.Run():
    vis.BeginScene()
    vis.Render()
    
    chronoirr.drawCoordsys(vis, marker_spring.GetAbsCoord())

    if not vis.GetUtilityFlag():
        system.DoStepDynamics(timestep)
    vis.EndScene()
    rt_timer.Spin(timestep)