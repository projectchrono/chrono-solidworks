#-------------------------------------------------------------------------------
#
# This file shows how to simulate a conveyor and a stack of pebbles
#
# Author: Alessandro Tasora
#
# REMARK: this is part of Chrono::Solidworks add-in
#     - it assumes that you exported the .asm in this directory using the add-in
#     - PyChrono must be installed in your Python environment
#-------------------------------------------------------------------------------

import math
import random
import sys
import pychrono as chrono
import pychrono.irrlicht as chronoirr

print("Demo program that shows how to use the SolidWorks add-in.")
print(" 1) use the SolidWorks Add-in, load the .SLDASM,")
print(" 2) from the Add-in, 'save as Python..' in a directory X")
print(" 3) modify m_datapath in this file")
print(" 3) modify ImportSolidWorksSystem(..) to match what you exported")
print(" 4) move this .py file in directory X and execute it.")

# Create Chrono physical system
my_system = chrono.ChSystemNSC()
my_system.SetCollisionSystemType(chrono.ChCollisionSystem.Type_BULLET)

# Set the default outward/inward shape margins for collision detection,
# this is epecially important for very large or very small objects.
# This is a global setting to be put BEFORE creating objects/systems
chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(0.005)
chrono.ChCollisionModel.SetDefaultSuggestedMargin(0.005)


# Load the CAD file
exported_items = chrono.ImportSolidWorksSystem('./conveyor.py')

# Print exported items
for my_item in exported_items:
	print (my_item.GetName())

# Add items to the physical system
for my_item in exported_items:
    my_system.Add(my_item)


# Create a contact material (surface property) to share between all objects.
# The rolling and spinning parameters are optional - if enabled they double
# the computational time.
pebble_material = chrono.ChContactMaterialNSC()
pebble_material.SetFriction(0.6)
#pebble_material.SetDampingF(0.1)
#pebble_material.SetCompliance (0.0000000001)
#pebble_material.SetComplianceT(0.0000000001)
pebble_material.SetRollingFriction(0.005)
pebble_material.SetSpinningFriction(0.005)
# pebble_material.SetComplianceRolling(0.0000001)
# pebble_material.SetComplianceSpinning(0.0000001)


# Assign the surface material also to items made with CAD:
my_bodyfloor = my_system.SearchBody('floor_box^assembly_conveyor-1')
if not my_bodyfloor:
    sys.exit('Error: cannot find floor_box from its name in the C::E system!')

my_bodyfloor.GetCollisionModel().SetAllShapesMaterial(pebble_material)


# Create the pebbles
density_pebble = 2028 # kg/m^3
diameter_pebble = 0.045

mass_pebble = density_pebble * (4./3.) * chrono.CH_PI * math.pow((diameter_pebble*0.5),3)
inertia_pebble = (2./5.)*mass_pebble*(math.pow((diameter_pebble*0.5),2))

pebbles = chrono.ChParticleCloud()
pebbles.SetMass(mass_pebble)
pebbles.SetInertiaXX(chrono.ChVector3d(inertia_pebble,inertia_pebble,inertia_pebble))

mySphere = chrono.ChCollisionShapeSphere(pebble_material, diameter_pebble/2.)

pebbles.AddCollisionShape(mySphere)
pebbles.EnableCollision(True)

pebbles.AddParticle(chrono.ChCoordsysd()) # at least one particle or Irrlicht crashes

pebbles_visshape = chrono.ChVisualShapeSphere(diameter_pebble/2)
pebbles.AddVisualShape(pebbles_visshape)

my_system.Add(pebbles)

max_pebbles = 1000
created_pebbles = 0
n_outlet_x = 5
n_outlet_z = 5
size_outlet_x = 0.28
size_outlet_z = 0.28
#pos_outlet = chrono.ChVector3d(0,1.0,0)  # to drop stuff directly on vertical
pos_outlet = chrono.ChVector3d(-2,1.6,0)  # to drop stuff into the hopper


# Create the conveyor belt
conv_length = 2.5
conv_thick = 0.1
conv_width = 0.38
body_belt = chrono.ChConveyor(conv_length,conv_thick,conv_width)
body_belt.SetFixed(True)
body_belt.SetConveyorSpeed(0.5)  # m/s

# set the position of the conveyor as in a CAD reference coordsys
my_marker = my_system.SearchMarker('Sistema di coordinate1')
if not my_marker :
    sys.exit('Error: cannot find marker from its name in the C::E system!')
body_belt.SetCoordsys(my_marker.GetAbsCoordsys())

# note: the CAD reference was on surface, so we must move down COG that is at middle of surface:
#body_belt.ConcatenatePreTransformation(chrono.ChFrameMovingD(chrono.ChVector3d(0, -0.005-conv_thick/2., 0)))

my_system.Add(body_belt)


# Create the room floor: a simple fixed rigid body with a collision shape and a visualization shape
body_floor = chrono.ChBody()
body_floor.SetFixed(True)
body_floor.SetPos(chrono.ChVector3d(0, -1, 0 ))

# Collision shape (shared by all particle clones)
myBox = chrono.ChCollisionShapeBox(pebble_material, 6, 2, 6)
body_floor.AddCollisionShape(myBox)
body_floor.EnableCollision(True)

# Visualization shape (shared by all particle clones)
body_floor_shape = chrono.ChVisualShapeBox(6, 2, 6)
body_floor_shape.SetTexture('concrete.jpg')
body_floor.AddVisualShape(body_floor_shape)

my_system.Add(body_floor)


# Create the Irrlicht visualization
vis = chronoirr.ChVisualSystemIrrlicht()
vis.AttachSystem(my_system)
vis.SetWindowSize(1024,768)
vis.SetWindowTitle('Conveyor')
vis.Initialize()
vis.AddLogo(chrono.GetChronoDataPath() + 'logo_pychrono_alpha.png')
vis.AddSkyBox()
vis.AddCamera(chrono.ChVector3d(2, 2, 2))
vis.AddTypicalLights()


# Modify solver settings
my_solver = chrono.ChSolverPSOR()
my_system.SetSolver(my_solver)
my_solver.SetMaxIterations(70)


# Simulation loop
timestep = 0.005
nstep = 0
max_fall_speed = 0.8

while vis.Run():
    if my_system.GetChTime() > 15:
        break

    # Update graphics
    vis.BeginScene()
    vis.Render()
    vis.EndScene()

    # Limit speed of particles a bit as as in viscous fall in fluid
    for ip in range(0,pebbles.GetNumParticles()):
        if (pebbles.Particle(ip).GetPosDt().Length() > max_fall_speed):
            vnspeed = pebbles.Particle(ip).GetPosDt().GetNormalized()
            pebbles.Particle(ip).SetPosDt(vnspeed*max_fall_speed)

    nstep = nstep + 1

    if math.fmod(nstep,6) == 0:
        for ix in range(0,n_outlet_x):
            for iz in range(0,n_outlet_z):
                if created_pebbles >= max_pebbles:
                    break

                offset_particle = chrono.ChVector3d( size_outlet_x * (-0.5+(ix/(n_outlet_x-1))),
                                                    0,
                                                    size_outlet_z * (-0.5+(iz/(n_outlet_z-1))) )
                jitter_particle = chrono.ChVector3d( 0.1*(size_outlet_x/n_outlet_x) * (random.uniform(-1.,1.)),
                                           0.1*diameter_pebble * (random.uniform(-1,1)),
                                           0.1*(size_outlet_z/n_outlet_z) * (random.uniform(-1.,1.)) )
                csys_particle   = chrono.ChCoordsysd( pos_outlet + offset_particle + jitter_particle )

                pebbles.AddParticle(csys_particle)
                my_system.GetCollisionSystem().BindAll()

                # trick: set speed as already falling to avoid overlapping at the top of fountain
                pebbles.Particle(pebbles.GetNumParticles()-1).SetPosDt(chrono.ChVector3d(0,-max_fall_speed,0))

                created_pebbles =created_pebbles+1

    # Advance simulation
    my_system.DoStepDynamics(timestep)

    print ('time=', my_system.GetChTime(), "  n particles=", pebbles.GetNumParticles())    