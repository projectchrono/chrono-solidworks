#-------------------------------------------------------------------------------
#
# This file shows how to simulate a spyder robot.
#
# Author: Alessandro Tasora
#
# REMARK: this is part of Chrono::Solidworks add-in
#     - it assumes that you exported the .asm in this directory using the add-in
#     - PyChrono must be installed in your Python environment
#-------------------------------------------------------------------------------

import pychrono as chrono

import pychrono.irrlicht as chronoirr
 
print ("Load a model exported by SolidWorks")


# Utility class to use ChLinkMotorRotationAngle given the markers from the CAD
class SpiderRobotMotor(chrono.ChLinkMotorRotationAngle):
    def __init__(self):
        super().__init__()
        self.bodylist = []

    def Initialize(self, mark1, mark2):
        body1 = mark1.GetBody()
        body2 = mark2.GetBody()
        self.bodylist.append([body1, body2])
        frame = mark1.GetAbsFrame()
        super().Initialize(body1, body2, frame)

    def SetMotorFunction(self, rotfun):
        super().SetAngleFunction(rotfun)
 
 
# ---------------------------------------------------------------------
#
#  Create the simulation system and add items
#
 
mysystem = chrono.ChSystemNSC()
mysystem.SetCollisionSystemType(chrono.ChCollisionSystem.Type_BULLET)
chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(0.05)
chrono.ChCollisionModel.SetDefaultSuggestedMargin(0.005)

# Import model items from Solidworks and add to system 
parts = chrono.ImportSolidWorksSystem('./spider_robot.py')
 
for ib in parts:
    mysystem.Add(ib)
 
# Retrieve objects from their name as saved from the SolidWorks interface
# (look the spider_robot.py file to guess them, or look their name in SW) 
bbody    = mysystem.SearchBody('Part3^SPIDER_ROBOT-1')
bbody.SetFixed(False)
b1base    = mysystem.SearchBody('M-410iB-300 -1/ArmBase-1')
b1turret  = mysystem.SearchBody('M-410iB-300 -1/M-410iB-300-02-1')
b1bicept  = mysystem.SearchBody('M-410iB-300 -1/M-410iB-300-03-1')
b1forearm = mysystem.SearchBody('M-410iB-300 -1/M-410iB-300-06-1')
m1_1B = b1base.SearchMarker('marker_M1_B')
m1_1A = b1turret.SearchMarker('marker_M1_A')
m1_2B = b1turret.SearchMarker('marker_M2_B')
m1_2A = b1bicept.SearchMarker('marker_M2_A')
m1_3B = b1bicept.SearchMarker('marker_M3_B')
m1_3A = b1forearm.SearchMarker('marker_M3_A')
b2base    = mysystem.SearchBody('M-410iB-300 -2/ArmBase-1')
b2turret  = mysystem.SearchBody('M-410iB-300 -2/M-410iB-300-02-1')
b2bicept  = mysystem.SearchBody('M-410iB-300 -2/M-410iB-300-03-1')
b2forearm = mysystem.SearchBody('M-410iB-300 -2/M-410iB-300-06-1')
m2_1B = b2base.SearchMarker('marker_M1_B')
m2_1A = b2turret.SearchMarker('marker_M1_A')
m2_2B = b2turret.SearchMarker('marker_M2_B')
m2_2A = b2bicept.SearchMarker('marker_M2_A')
m2_3B = b2bicept.SearchMarker('marker_M3_B')
m2_3A = b2forearm.SearchMarker('marker_M3_A')
b3base    = mysystem.SearchBody('M-410iB-300 -3/ArmBase-1')
b3turret  = mysystem.SearchBody('M-410iB-300 -3/M-410iB-300-02-1')
b3bicept  = mysystem.SearchBody('M-410iB-300 -3/M-410iB-300-03-1')
b3forearm = mysystem.SearchBody('M-410iB-300 -3/M-410iB-300-06-1')
m3_1B = b3base.SearchMarker('marker_M1_B')
m3_1A = b3turret.SearchMarker('marker_M1_A')
m3_2B = b3turret.SearchMarker('marker_M2_B')
m3_2A = b3bicept.SearchMarker('marker_M2_A')
m3_3B = b3bicept.SearchMarker('marker_M3_B')
m3_3A = b3forearm.SearchMarker('marker_M3_A')
b7base    = mysystem.SearchBody('M-410iB-300 -7/ArmBase-1')
b7turret  = mysystem.SearchBody('M-410iB-300 -7/M-410iB-300-02-1')
b7bicept  = mysystem.SearchBody('M-410iB-300 -7/M-410iB-300-03-1')
b7forearm = mysystem.SearchBody('M-410iB-300 -7/M-410iB-300-06-1')
m7_1B = b7base.SearchMarker('marker_M1_B')
m7_1A = b7turret.SearchMarker('marker_M1_A')
m7_2B = b7turret.SearchMarker('marker_M2_B')
m7_2A = b7bicept.SearchMarker('marker_M2_A')
m7_3B = b7bicept.SearchMarker('marker_M3_B')
m7_3A = b7forearm.SearchMarker('marker_M3_A')
b8base    = mysystem.SearchBody('M-410iB-300 -8/ArmBase-1')
b8turret  = mysystem.SearchBody('M-410iB-300 -8/M-410iB-300-02-1')
b8bicept  = mysystem.SearchBody('M-410iB-300 -8/M-410iB-300-03-1')
b8forearm = mysystem.SearchBody('M-410iB-300 -8/M-410iB-300-06-1')
m8_1B = b8base.SearchMarker('marker_M1_B')
m8_1A = b8turret.SearchMarker('marker_M1_A')
m8_2B = b8turret.SearchMarker('marker_M2_B')
m8_2A = b8bicept.SearchMarker('marker_M2_A')
m8_3B = b8bicept.SearchMarker('marker_M3_B')
m8_3A = b8forearm.SearchMarker('marker_M3_A')
b9base    = mysystem.SearchBody('M-410iB-300 -9/ArmBase-1')
b9turret  = mysystem.SearchBody('M-410iB-300 -9/M-410iB-300-02-1')
b9bicept  = mysystem.SearchBody('M-410iB-300 -9/M-410iB-300-03-1')
b9forearm = mysystem.SearchBody('M-410iB-300 -9/M-410iB-300-06-1')
m9_1B = b9base.SearchMarker('marker_M1_B')
m9_1A = b9turret.SearchMarker('marker_M1_A')
m9_2B = b9turret.SearchMarker('marker_M2_B')
m9_2A = b9bicept.SearchMarker('marker_M2_A')
m9_3B = b9bicept.SearchMarker('marker_M3_B')
m9_3A = b9forearm.SearchMarker('marker_M3_A')

# Customize colors
for bb in mysystem.GetBodies():
    if bb.GetVisualModel():
        for i in range(bb.GetVisualModel().GetNumShapes()):
            bb.GetVisualShape(i).SetColor(chrono.ChColor(1.0, 1.0, 0.0))
 
# Define motion functions 
period = 2
mfunc_sineS   = chrono.ChFunctionSine(0, 1.0/period,  0.2)
mfunc_swingSa = chrono.ChFunctionRepeat(mfunc_sineS, 0, period)
mfunc_swingSb = chrono.ChFunctionRepeat(mfunc_sineS, period/2.0, period)
mfunc_sineD   = chrono.ChFunctionSine(0, 1.0/period, -0.2)
mfunc_swingDb = chrono.ChFunctionRepeat(mfunc_sineD, period/2.0, period)
mfunc_swingDa = chrono.ChFunctionRepeat(mfunc_sineD, 0, period)
  
mfunc_sigma  = chrono.ChFunctionPoly23()
mfunc_sigma.SetAmplitude(-0.2)
mfunc_sigma.SetEndArg(0.5)
mfunc_const  = chrono.ChFunctionConst()
mfunc_sigmb  = chrono.ChFunctionPoly23()
mfunc_sigmb.SetAmplitude(0.2)
mfunc_sigmb.SetEndArg(0.5)
mfunc_seq    = chrono.ChFunctionSequence()
mfunc_seq.InsertFunct(mfunc_sigma, 0.5, 1, True) # fx, duration, weight, C0 continuity
mfunc_seq.InsertFunct(mfunc_const, 1.0, 1, True) # fx, duration, weight, C0 continuity
mfunc_seq.InsertFunct(mfunc_sigmb, 0.5, 1, True) # fx, duration, weight, C0 continuity
mfunc_updownA = chrono.ChFunctionRepeat(mfunc_seq, 0, period)
mfunc_updownB = chrono.ChFunctionRepeat(mfunc_seq, 0, period, period/2.0)
 
# Add actuators to Leg n.1
 
motor1_1 = SpiderRobotMotor()
motor1_1.Initialize(m1_1A, m1_1B)
motor1_1.SetMotorFunction(mfunc_swingSa)
mysystem.Add(motor1_1)
 
motor1_2 = SpiderRobotMotor()
motor1_2.Initialize(m1_2A, m1_2B)
motor1_2.SetMotorFunction(mfunc_updownA)
mysystem.Add(motor1_2)
 
motor1_3 = SpiderRobotMotor()
motor1_3.Initialize(m1_3A, m1_3B)
motor1_3.SetMotorFunction(mfunc_const)
mysystem.Add(motor1_3)
 
# Add actuators to Leg n.2
 
motor2_1 = SpiderRobotMotor()
motor2_1.Initialize(m2_1A, m2_1B)
motor2_1.SetMotorFunction(mfunc_swingSb)
mysystem.Add(motor2_1)
 
motor2_2 = SpiderRobotMotor()
motor2_2.Initialize(m2_2A, m2_2B)
motor2_2.SetMotorFunction(mfunc_updownB)
mysystem.Add(motor2_2)
 
motor2_3 = SpiderRobotMotor()
motor2_3.Initialize(m2_3A, m2_3B)
motor2_3.SetMotorFunction(mfunc_const)
mysystem.Add(motor2_3)
 
# Add actuators to Leg n.3
 
motor3_1 = SpiderRobotMotor()
motor3_1.Initialize(m3_1A, m3_1B)
motor3_1.SetMotorFunction(mfunc_swingSa)
mysystem.Add(motor3_1)
 
motor3_2 = SpiderRobotMotor()
motor3_2.Initialize(m3_2A, m3_2B)
motor3_2.SetMotorFunction(mfunc_updownA)
mysystem.Add(motor3_2)
 
motor3_3 = SpiderRobotMotor()
motor3_3.Initialize(m3_3A, m3_3B)
motor3_3.SetMotorFunction(mfunc_const)
mysystem.Add(motor3_3)
 
# Add actuators to Leg n.9
 
motor9_1 = SpiderRobotMotor()
motor9_1.Initialize(m9_1A, m9_1B)
motor9_1.SetMotorFunction(mfunc_swingDb)
mysystem.Add(motor9_1)
 
motor9_2 = SpiderRobotMotor()
motor9_2.Initialize(m9_2A, m9_2B)
motor9_2.SetMotorFunction(mfunc_updownB)
mysystem.Add(motor9_2)
 
motor9_3 = SpiderRobotMotor()
motor9_3.Initialize(m9_3A, m9_3B)
motor9_3.SetMotorFunction(mfunc_const)
mysystem.Add(motor9_3)
 
# Add actuators to Leg n.8
 
motor8_1 = SpiderRobotMotor()
motor8_1.Initialize(m8_1A, m8_1B)
motor8_1.SetMotorFunction(mfunc_swingDa)
mysystem.Add(motor8_1)
 
motor8_2 = SpiderRobotMotor()
motor8_2.Initialize(m8_2A, m8_2B)
motor8_2.SetMotorFunction(mfunc_updownA)
mysystem.Add(motor8_2)
 
motor8_3 = SpiderRobotMotor()
motor8_3.Initialize(m8_3A, m8_3B)
motor8_3.SetMotorFunction(mfunc_const)
mysystem.Add(motor8_3)
 
# Add actuators to Leg n.7
 
motor7_1 = SpiderRobotMotor()
motor7_1.Initialize(m7_1A, m7_1B)
motor7_1.SetMotorFunction(mfunc_swingDb)
mysystem.Add(motor7_1)
 
motor7_2 = SpiderRobotMotor()
motor7_2.Initialize(m7_2A, m7_2B)
motor7_2.SetMotorFunction(mfunc_updownB)
mysystem.Add(motor7_2)
 
motor7_3 = SpiderRobotMotor()
motor7_3.Initialize(m7_3A, m7_3B)
motor7_3.SetMotorFunction(mfunc_const)
mysystem.Add(motor7_3)
 

# Create a floor
mymat = chrono.ChContactMaterialNSC()
mymat.SetRestitution(0.0)

mfloor = chrono.ChBodyEasyBox(20, 1, 20, 1000, True, True, mymat)
mfloor.SetFixed(True)
mfloor.SetPos(chrono.ChVector3d(0,0.5,0))
mfloor.GetVisualShape(0).SetColor(chrono.ChColor(0.4, 0.4, 0.4))
mysystem.Add(mfloor)

 
# ---------------------------------------------------------------------
#
#  Create an Irrlicht application to visualize the system
#

# Create the Irrlicht visualization
vis = chronoirr.ChVisualSystemIrrlicht()
vis.AttachSystem(mysystem)
vis.SetWindowSize(1280,720)
vis.SetWindowTitle('Spider Robot')
vis.Initialize()
vis.AddLogo(chrono.GetChronoDataFile('logo_pychrono_alpha.png'))
vis.AddSkyBox()
vis.AddCamera(chrono.ChVector3d(5, 5, -7.5), chrono.ChVector3d(-1.0, 0, -2.5))
vis.AddLightWithShadow(chrono.ChVector3d(10,20,10), chrono.ChVector3d(0,2.6,0), 50, 10, 40, 60, 512)


# ---------------------------------------------------------------------
#
#  Run the simulation
#

solver = chrono.ChSolverBB()
solver.SetMaxIterations(200)
solver.SetTolerance(1e-6)
mysystem.SetSolver(solver)

while(vis.Run()):
    vis.BeginScene()
    vis.Render()
    mysystem.DoStepDynamics(0.001)
    vis.EndScene()
