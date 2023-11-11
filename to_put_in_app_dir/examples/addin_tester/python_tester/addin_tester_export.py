# PyChrono model automatically generated using Chrono::SolidWorks add-in
# Assembly: C:\WORKSPACE\chrono-solidworks_source\to_put_in_app_dir\examples\addin_tester\cad\Assem1.SLDASM


import pychrono as chrono 
import builtins 

# Some global settings 
sphereswept_r = 0.001
chrono.ChCollisionModel.SetDefaultSuggestedEnvelope(0.003)
chrono.ChCollisionModel.SetDefaultSuggestedMargin(0.003)
chrono.ChCollisionSystemBullet.SetContactBreakingThreshold(0.002)

shapes_dir = 'addin_tester_export_shapes/' 

if hasattr(builtins, 'exported_system_relpath'): 
    shapes_dir = builtins.exported_system_relpath + shapes_dir 

exported_items = [] 

body_0 = chrono.ChBodyAuxRef()
body_0.SetName('ground')
body_0.SetBodyFixed(True)
exported_items.append(body_0)

# Rigid body part
body_1 = chrono.ChBodyAuxRef()
body_1.SetName('Part5-1')
body_1.SetPos(chrono.ChVectorD(0,0,0))
body_1.SetRot(chrono.ChQuaternionD(1,0,0,0))
body_1.SetMass(1.24926450899844)
body_1.SetInertiaXX(chrono.ChVectorD(0.0042627436999619,0.00483926639146044,0.00495832194017688))
body_1.SetInertiaXY(chrono.ChVectorD(2.93447621398516e-19,2.28301849064636e-19,1.31259899082468e-19))
body_1.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.28885634625101e-17,0.043249333885732,-9.38587017536795e-18),chrono.ChQuaternionD(1,0,0,0)))
body_1.SetBodyFixed(True)

# Visualization shape 
body_1_1_shape = chrono.ChModelFileShape() 
body_1_1_shape.SetFilename(shapes_dir +'body_1_1.obj') 
body_1.AddVisualShape(body_1_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_1)



# Rigid body part
body_2 = chrono.ChBodyAuxRef()
body_2.SetName('Part5-2')
body_2.SetPos(chrono.ChVectorD(0.7,0,1.73472347597681e-17))
body_2.SetRot(chrono.ChQuaternionD(1,0,0,0))
body_2.SetMass(1.24926450899844)
body_2.SetInertiaXX(chrono.ChVectorD(0.0042627436999619,0.00483926639146044,0.00495832194017688))
body_2.SetInertiaXY(chrono.ChVectorD(2.93447621398516e-19,2.28301849064636e-19,1.31259899082468e-19))
body_2.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.28885634625101e-17,0.043249333885732,-9.38587017536795e-18),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_1_1_shape = chrono.ChModelFileShape() 
body_1_1_shape.SetFilename(shapes_dir +'body_1_1.obj') 
body_2.AddVisualShape(body_1_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_2)



# Rigid body part
body_3 = chrono.ChBodyAuxRef()
body_3.SetName('Part4-2')
body_3.SetPos(chrono.ChVectorD(0.857449355002332,0.634568366309001,1.42367689219066e-16))
body_3.SetRot(chrono.ChQuaternionD(0.96370362534401,-8.49032280619758e-17,-1.33927507176121e-17,0.266974385473237))
body_3.SetMass(10.2238384765683)
body_3.SetInertiaXX(chrono.ChVectorD(0.980760799680617,2.70429795542421,3.68079882240625))
body_3.SetInertiaXY(chrono.ChVectorD(1.61647534643467,2.32693428735339e-16,-1.40723568573197e-16))
body_3.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(9.33355446105201e-16,-1.85596401963984e-17,-1.3100469563516e-18),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_3_1_shape = chrono.ChModelFileShape() 
body_3_1_shape.SetFilename(shapes_dir +'body_3_1.obj') 
body_3.AddVisualShape(body_3_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_3)



# Rigid body part
body_4 = chrono.ChBodyAuxRef()
body_4.SetName('Part3-1')
body_4.SetPos(chrono.ChVectorD(1.47292990417987,0.862647471026506,5.4876833193704e-17))
body_4.SetRot(chrono.ChQuaternionD(0.906986810505895,-3.21984491210804e-17,-1.10219199712073e-16,0.421159026459535))
body_4.SetMass(4.07201324699296)
body_4.SetInertiaXX(chrono.ChVectorD(0.135063421966252,0.0975555347740981,0.230922284554103))
body_4.SetInertiaXY(chrono.ChVectorD(0.110514737498251,3.28237616713828e-17,-3.85606861450939e-17))
body_4.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.7719809879356e-16,1.89499214802868e-17,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_4_1_shape = chrono.ChModelFileShape() 
body_4_1_shape.SetFilename(shapes_dir +'body_4_1.obj') 
body_4.AddVisualShape(body_4_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_4)



# Rigid body part
body_5 = chrono.ChBodyAuxRef()
body_5.SetName('Part4-1')
body_5.SetPos(chrono.ChVectorD(0.985709765604091,0.28845253928853,1.92139317440777e-16))
body_5.SetRot(chrono.ChQuaternionD(0.996421036912629,4.15481149113659e-19,1.18778610139098e-18,0.0845287950816819))
body_5.SetMass(10.2238384765683)
body_5.SetInertiaXX(chrono.ChVectorD(0.114650947155474,3.57040780794935,3.68079882240625))
body_5.SetInertiaXY(chrono.ChVectorD(0.608336878059569,1.85863814232006e-18,-2.3287958686344e-19))
body_5.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(9.33355446105201e-16,-1.85596401963984e-17,-1.3100469563516e-18),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_3_1_shape = chrono.ChModelFileShape() 
body_3_1_shape.SetFilename(shapes_dir +'body_3_1.obj') 
body_5.AddVisualShape(body_3_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_5)



# Rigid body part
body_6 = chrono.ChBodyAuxRef()
body_6.SetName('Part3-2')
body_6.SetPos(chrono.ChVectorD(2.08512792642936,1.08951016719144,2.3184356464553e-16))
body_6.SetRot(chrono.ChQuaternionD(0.996814403604971,4.21672496261848e-16,-4.35172150274502e-16,-0.0797561581676699))
body_6.SetMass(4.07201324699296)
body_6.SetInertiaXX(chrono.ChVectorD(0.00988282583492456,0.222736130905426,0.230922284554103))
body_6.SetInertiaXY(chrono.ChVectorD(-0.0351935396749617,2.08085648996931e-16,3.55897696799726e-17))
body_6.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.7719809879356e-16,1.89499214802868e-17,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_4_1_shape = chrono.ChModelFileShape() 
body_4_1_shape.SetFilename(shapes_dir +'body_4_1.obj') 
body_6.AddVisualShape(body_4_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_6)



# Rigid body part
body_7 = chrono.ChBodyAuxRef()
body_7.SetName('Part2-1')
body_7.SetPos(chrono.ChVectorD(0.965480549178704,0.348079104716603,1.70290139222766e-16))
body_7.SetRot(chrono.ChQuaternionD(1.03683306464997e-18,0.93768754251937,0.347479600270268,-5.46393466527026e-19))
body_7.SetMass(3.82201324699296)
body_7.SetInertiaXX(chrono.ChVectorD(0.0826341987985947,0.110554517840008,0.191596211119023))
body_7.SetInertiaXY(chrono.ChVectorD(-0.091582091471705,1.57919984855346e-18,1.51433626835054e-18))
body_7.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.85105473087521e-16,2.68372329572404e-17,8.72627072247286e-19),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_7_1_shape = chrono.ChModelFileShape() 
body_7_1_shape.SetFilename(shapes_dir +'body_7_1.obj') 
body_7.AddVisualShape(body_7_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_7)



# Rigid body part
body_8 = chrono.ChBodyAuxRef()
body_8.SetName('Part3-3')
body_8.SetPos(chrono.ChVectorD(2.21338833703225,0.743394340170001,8.42825665436867e-17))
body_8.SetRot(chrono.ChQuaternionD(-0.421159026459001,1.55141771415691e-16,-5.71798717584476e-17,0.906986810506143))
body_8.SetMass(4.07201324699296)
body_8.SetInertiaXX(chrono.ChVectorD(0.135063421965992,0.0975555347743583,0.230922284554103))
body_8.SetInertiaXY(chrono.ChVectorD(0.110514737498295,4.87217332267032e-17,-5.77489999426299e-17))
body_8.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.7719809879356e-16,1.89499214802868e-17,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_4_1_shape = chrono.ChModelFileShape() 
body_4_1_shape.SetFilename(shapes_dir +'body_4_1.obj') 
body_8.AddVisualShape(body_4_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_8)



# Rigid body part
body_9 = chrono.ChBodyAuxRef()
body_9.SetName('Part3-4')
body_9.SetPos(chrono.ChVectorD(1.60119031478233,0.516531644005226,1.71754857525961e-16))
body_9.SetRot(chrono.ChQuaternionD(0.996814403605004,-6.66510217621709e-31,6.42865497377732e-30,-0.0797561581672549))
body_9.SetMass(4.07201324699296)
body_9.SetInertiaXX(chrono.ChVectorD(0.00988282583486595,0.222736130905484,0.230922284554103))
body_9.SetInertiaXY(chrono.ChVectorD(-0.0351935396747845,-8.43107948548348e-19,-2.59678311765443e-20))
body_9.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.7719809879356e-16,1.89499214802868e-17,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_4_1_shape = chrono.ChModelFileShape() 
body_4_1_shape.SetFilename(shapes_dir +'body_4_1.obj') 
body_9.AddVisualShape(body_4_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_9)



# Rigid body part
body_10 = chrono.ChBodyAuxRef()
body_10.SetName('Part8-1')
body_10.SetPos(chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17))
body_10.SetRot(chrono.ChQuaternionD(0.999976477958333,0,0,-0.00685882862065701))
body_10.SetMass(4.19339787401166)
body_10.SetInertiaXX(chrono.ChVectorD(0.0167735914960466,0.0167735914960466,0.0167735914960466))
body_10.SetInertiaXY(chrono.ChVectorD(-5.42101086242752e-20,0,0))
body_10.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(-2.29621274840129e-18,0,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_10_1_shape = chrono.ChModelFileShape() 
body_10_1_shape.SetFilename(shapes_dir +'body_10_1.obj') 
body_10.AddVisualShape(body_10_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

# Collision material 
mat_10 = chrono.ChMaterialSurfaceNSC()

# Collision shapes 
body_10.GetCollisionModel().ClearModel()
body_10.GetCollisionModel().AddSphere(mat_10, 0.1, chrono.ChVectorD(0,0,0))
body_10.GetCollisionModel().BuildModel()
body_10.SetCollide(True)

exported_items.append(body_10)



# Rigid body part
body_11 = chrono.ChBodyAuxRef()
body_11.SetName('Part9-1')
body_11.SetPos(chrono.ChVectorD(4.20022320835452,0.95,-2.425))
body_11.SetRot(chrono.ChQuaternionD(0.5,1.54250627047993e-17,0.866025403784439,1.31816476328183e-17))
body_11.SetMass(14.7)
body_11.SetInertiaXX(chrono.ChVectorD(3.34384761479592,0.818210459183675,2.98686284438776))
body_11.SetInertiaXY(chrono.ChVectorD(-5.00979807594479e-16,0.309157879937626,-1.5513739031605e-16))
body_11.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(-3.66829110549246e-17,-4.56137867462119e-16,0.0437755102040816),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_11_1_shape = chrono.ChModelFileShape() 
body_11_1_shape.SetFilename(shapes_dir +'body_11_1.obj') 
body_11.AddVisualShape(body_11_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

# Collision material 
mat_11 = chrono.ChMaterialSurfaceNSC()

# Collision shapes 
body_11.GetCollisionModel().ClearModel()

# Triangle mesh collision shape 
body_11_1_collision_mesh = chrono.ChTriangleMeshConnected.CreateFromWavefrontFile(shapes_dir + 'body_11_1_collision.obj', False, True) 
mr = chrono.ChMatrix33D()
mr[0,0]=1; mr[1,0]=0; mr[2,0]=0 
mr[0,1]=0; mr[1,1]=1; mr[2,1]=0 
mr[0,2]=0; mr[1,2]=0; mr[2,2]=1 
body_11_1_collision_mesh.Transform(chrono.ChVectorD(0, 0, 0), mr) 
body_11.GetCollisionModel().AddTriangleMesh(mat_11, body_11_1_collision_mesh, False, False, chrono.ChVectorD(0,0,0), chrono.ChMatrix33D(chrono.ChQuaternionD(1,0,0,0)), sphereswept_r) 
body_11.GetCollisionModel().BuildModel()
body_11.SetCollide(True)

exported_items.append(body_11)



# Rigid body part
body_12 = chrono.ChBodyAuxRef()
body_12.SetName('Part5-3')
body_12.SetPos(chrono.ChVectorD(3.45,-1.7,-1.38777878078145e-17))
body_12.SetRot(chrono.ChQuaternionD(1,0,0,0))
body_12.SetMass(1.24926450899844)
body_12.SetInertiaXX(chrono.ChVectorD(0.0042627436999619,0.00483926639146044,0.00495832194017688))
body_12.SetInertiaXY(chrono.ChVectorD(2.93447621398516e-19,2.28301849064636e-19,1.31259899082468e-19))
body_12.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.28885634625101e-17,0.043249333885732,-9.38587017536795e-18),chrono.ChQuaternionD(1,0,0,0)))
body_12.SetBodyFixed(True)

# Visualization shape 
body_1_1_shape = chrono.ChModelFileShape() 
body_1_1_shape.SetFilename(shapes_dir +'body_1_1.obj') 
body_12.AddVisualShape(body_1_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_12)



# Rigid body part
body_13 = chrono.ChBodyAuxRef()
body_13.SetName('Part4-3')
body_13.SetPos(chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17))
body_13.SetRot(chrono.ChQuaternionD(0.999976477958333,1.72531731076868e-43,2.34465582965004e-42,-0.00685882862065701))
body_13.SetMass(10.2238384765683)
body_13.SetInertiaXX(chrono.ChVectorD(0.0113787980347424,3.67367995707008,3.68079882240625))
body_13.SetInertiaXY(chrono.ChVectorD(-0.0502511947156836,1.081439425721e-17,2.27874263576526e-19))
body_13.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(9.33355446105201e-16,-1.85596401963984e-17,-1.3100469563516e-18),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_3_1_shape = chrono.ChModelFileShape() 
body_3_1_shape.SetFilename(shapes_dir +'body_3_1.obj') 
body_13.AddVisualShape(body_3_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_13)



# Rigid body part
body_14 = chrono.ChBodyAuxRef()
body_14.SetName('SubAssem1-1/Part7-1')
body_14.SetPos(chrono.ChVectorD(2.64897398514048,1.43845265975283,-1.70259092415079))
body_14.SetRot(chrono.ChQuaternionD(-0.0604224435195309,0.939235124531335,0.225499629132351,-0.251667293121115))
body_14.SetMass(10.451484)
body_14.SetInertiaXX(chrono.ChVectorD(0.0848513792626409,0.142965282790807,0.150831930135058))
body_14.SetInertiaXY(chrono.ChVectorD(0.00259666547198675,0.0466103892210022,0.00752843850527692))
body_14.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(0.0335249042145593,0.0789272030651341,2.07623900982791e-17),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_14_1_shape = chrono.ChModelFileShape() 
body_14_1_shape.SetFilename(shapes_dir +'body_14_1.obj') 
body_14.AddVisualShape(body_14_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

# Collision material 
mat_14 = chrono.ChMaterialSurfaceNSC()

# Collision shapes 
body_14.GetCollisionModel().ClearModel()
pt_vect = chrono.vector_ChVectorD()
pt_vect.push_back(chrono.ChVectorD(-0.2,0.04,0.06))
pt_vect.push_back(chrono.ChVectorD(-0.2,0.04,-0.06))
pt_vect.push_back(chrono.ChVectorD(0.2,-0.04,0.06))
pt_vect.push_back(chrono.ChVectorD(-0.2,-0.04,0.06))
pt_vect.push_back(chrono.ChVectorD(-0.2,-0.04,-0.06))
pt_vect.push_back(chrono.ChVectorD(0.2,-0.04,-0.06))
pt_vect.push_back(chrono.ChVectorD(0.0499999999999999,0.24,-0.06))
pt_vect.push_back(chrono.ChVectorD(0.2,0.24,-0.06))
pt_vect.push_back(chrono.ChVectorD(0.0499999999999999,0.24,0.06))
pt_vect.push_back(chrono.ChVectorD(0.2,0.24,0.06))
body_14.GetCollisionModel().AddConvexHull(mat_14, pt_vect)
body_14.GetCollisionModel().BuildModel()
body_14.SetCollide(True)

exported_items.append(body_14)



# Rigid body part
body_15 = chrono.ChBodyAuxRef()
body_15.SetName('SubAssem1-1/Part3-1')
body_15.SetPos(chrono.ChVectorD(3.34915518237306,1.74782300650624,-2.04910569993885))
body_15.SetRot(chrono.ChQuaternionD(-0.0028037641306162,0.258817954761017,-0.000751266334566173,0.965921757079174))
body_15.SetMass(4.07201324699296)
body_15.SetInertiaXX(chrono.ChVectorD(0.0608973142989189,0.228398514344712,0.174245412650823))
body_15.SetInertiaXY(chrono.ChVectorD(0.00097244655418978,0.0981655675357989,-0.000569891741469255))
body_15.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.7719809879356e-16,1.89499214802868e-17,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_4_1_shape = chrono.ChModelFileShape() 
body_4_1_shape.SetFilename(shapes_dir +'body_4_1.obj') 
body_15.AddVisualShape(body_4_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_15)



# Rigid body part
body_16 = chrono.ChBodyAuxRef()
body_16.SetName('SubAssem1-1/Part3-2')
body_16.SetPos(chrono.ChVectorD(2.71004101034917,1.57539336922091,-1.73784798759358))
body_16.SetRot(chrono.ChQuaternionD(-0.22549962913235,0.251667293121115,-0.0604224435195309,0.939235124531335))
body_16.SetMass(4.07201324699296)
body_16.SetInertiaXX(chrono.ChVectorD(0.0954197457692436,0.193876082874387,0.174245412650824))
body_16.SetInertiaXY(chrono.ChVectorD(0.0677620548497138,0.0874668071942832,-0.0445686107643094))
body_16.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(1.7719809879356e-16,1.89499214802868e-17,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_4_1_shape = chrono.ChModelFileShape() 
body_4_1_shape.SetFilename(shapes_dir +'body_4_1.obj') 
body_16.AddVisualShape(body_4_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

exported_items.append(body_16)



# Rigid body part
body_17 = chrono.ChBodyAuxRef()
body_17.SetName('SubAssem1-1/Part1-1')
body_17.SetPos(chrono.ChVectorD(3.72390923627308,1.75,-2.15))
body_17.SetRot(chrono.ChQuaternionD(0.965925826289068,-2.51428505198776e-16,0.258819045102521,5.74693726168631e-17))
body_17.SetMass(4.20547568872596)
body_17.SetInertiaXX(chrono.ChVectorD(0.0180439259077252,0.0415892474927246,0.0309532827377262))
body_17.SetInertiaXY(chrono.ChVectorD(8.23750473832006e-19,-0.0111798309612991,9.95089532168635e-18))
body_17.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(-1.44372465132958e-17,1.61129538715447e-18,-1.71003785821234e-17),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_17_1_shape = chrono.ChModelFileShape() 
body_17_1_shape.SetFilename(shapes_dir +'body_17_1.obj') 
body_17.AddVisualShape(body_17_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

# Auxiliary marker (coordinate system feature)
marker_17_1 =chrono.ChMarker()
marker_17_1.SetName('MY_MARKER')
body_17.AddMarker(marker_17_1)
marker_17_1.Impose_Abs_Coord(chrono.ChCoordsysD(chrono.ChVectorD(3.72390923627308,1.75,-2.15),chrono.ChQuaternionD(0.965925826289068,-2.51428505198776E-16,0.258819045102521,5.74693726168631E-17)))

exported_items.append(body_17)



# Rigid body part
body_18 = chrono.ChBodyAuxRef()
body_18.SetName('Part8-2')
body_18.SetPos(chrono.ChVectorD(4.4499059129399,-1.59371733457401,-2.77555756156289e-17))
body_18.SetRot(chrono.ChQuaternionD(1,0,0,0))
body_18.SetMass(4.19339787401166)
body_18.SetInertiaXX(chrono.ChVectorD(0.0167735914960466,0.0167735914960466,0.0167735914960466))
body_18.SetInertiaXY(chrono.ChVectorD(0,0,0))
body_18.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(-2.29621274840129e-18,0,0),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_10_1_shape = chrono.ChModelFileShape() 
body_10_1_shape.SetFilename(shapes_dir +'body_10_1.obj') 
body_18.AddVisualShape(body_10_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

# Collision material 
mat_18 = chrono.ChMaterialSurfaceNSC()

# Collision shapes 
body_18.GetCollisionModel().ClearModel()
body_18.GetCollisionModel().AddSphere(mat_18, 0.1, chrono.ChVectorD(0,0,0))
body_18.GetCollisionModel().BuildModel()
body_18.SetCollide(True)

exported_items.append(body_18)



# Rigid body part
body_19 = chrono.ChBodyAuxRef()
body_19.SetName('Part6-1')
body_19.SetPos(chrono.ChVectorD(2.45535714285633,0.579883601762942,-0.0499999999999993))
body_19.SetRot(chrono.ChQuaternionD(0.707106781186547,6.22940772466283e-16,-7.12603076539096e-16,0.707106781186548))
body_19.SetMass(14.710920688726)
body_19.SetInertiaXX(chrono.ChVectorD(1.21802361978206,0.0400589183677246,1.20291658556707))
body_19.SetInertiaXY(chrono.ChVectorD(-1.31083052179195e-16,-2.85005728778339e-17,-1.28661301424007e-16))
body_19.SetFrame_COG_to_REF(chrono.ChFrameD(chrono.ChVectorD(-0.00919442385254511,1.84281645042619e-18,-4.78869851749592e-18),chrono.ChQuaternionD(1,0,0,0)))

# Visualization shape 
body_19_1_shape = chrono.ChModelFileShape() 
body_19_1_shape.SetFilename(shapes_dir +'body_19_1.obj') 
body_19.AddVisualShape(body_19_1_shape, chrono.ChFrameD(chrono.ChVectorD(0,0,0), chrono.ChQuaternionD(1,0,0,0)))

# Collision material 
mat_19 = chrono.ChMaterialSurfaceNSC()

# Collision shapes 
body_19.GetCollisionModel().ClearModel()
mr = chrono.ChMatrix33D()
mr[0,0]=-1; mr[1,0]=0; mr[2,0]=0 
mr[0,1]=0; mr[1,1]=1; mr[2,1]=5.04646829375071E-16 
mr[0,2]=0; mr[1,2]=5.04646829375071E-16; mr[2,2]=-1 
body_19.GetCollisionModel().AddBox(mat_19, 0.15,0.22,0.15,chrono.ChVectorD(-0.5,3.78485122031303E-17,5.55111512312578E-17),mr)
body_19.GetCollisionModel().BuildModel()
body_19.SetCollide(True)

exported_items.append(body_19)




# Mate constraint: Concentric1 [MateConcentric] type:1 align:0 flip:False
#   Entity 0: C::E name: body_1 , SW name: Part5-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_5 , SW name: Part4-1 ,  SW ref.type:2 (2)

link_1 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0,0.12,-0.075)
dA = chrono.ChVectorD(0,0,1)
cB = chrono.ChVectorD(1.33226762955019e-14,0.120000000000001,-0.0249999999999998)
dB = chrono.ChVectorD(2.43731035938427e-18,-6.2718405890427e-19,1)
link_1.Initialize(body_1,body_5,False,cA,cB,dA,dB)
link_1.SetName("Concentric1")
exported_items.append(link_1)

link_2 = chrono.ChLinkMateGeneric()
link_2.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(0,0.12,-0.075)
cB = chrono.ChVectorD(1.33226762955019e-14,0.120000000000001,-0.0249999999999998)
dA = chrono.ChVectorD(0,0,1)
dB = chrono.ChVectorD(2.43731035938427e-18,-6.2718405890427e-19,1)
link_2.Initialize(body_1,body_5,False,cA,cB,dA,dB)
link_2.SetName("Concentric1")
exported_items.append(link_2)


# Mate constraint: Coincident1 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part5-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Part4-1 ,  SW ref.type:4 (4)

link_3 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(0,0,0)
cB = chrono.ChVectorD(0.985709765604091,0.28845253928853,1.92139317440777e-16)
dA = chrono.ChVectorD(0,0,1)
dB = chrono.ChVectorD(2.43731035938427e-18,-6.2718405890427e-19,1)
link_3.Initialize(body_1,body_5,False,cA,cB,dB)
link_3.SetDistance(0)
link_3.SetName("Coincident1")
exported_items.append(link_3)

link_4 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(0,0,1)
cB = chrono.ChVectorD(0.985709765604091,0.28845253928853,1.92139317440777e-16)
dB = chrono.ChVectorD(2.43731035938427e-18,-6.2718405890427e-19,1)
link_4.Initialize(body_1,body_5,False,cA,cB,dA,dB)
link_4.SetName("Coincident1")
exported_items.append(link_4)


# Mate constraint: Concentric3 [MateConcentric] type:1 align:0 flip:False
#   Entity 0: C::E name: body_1 , SW name: Part5-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_3 , SW name: Part4-2 ,  SW ref.type:2 (2)

link_5 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0,0.12,-0.075)
dA = chrono.ChVectorD(0,0,1)
cB = chrono.ChVectorD(-4.27435864480685e-14,0.119999999999905,-0.0249999999999998)
dB = chrono.ChVectorD(-7.11472591128629e-17,1.56492054588209e-16,1)
link_5.Initialize(body_1,body_3,False,cA,cB,dA,dB)
link_5.SetName("Concentric3")
exported_items.append(link_5)

link_6 = chrono.ChLinkMateGeneric()
link_6.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(0,0.12,-0.075)
cB = chrono.ChVectorD(-4.27435864480685e-14,0.119999999999905,-0.0249999999999998)
dA = chrono.ChVectorD(0,0,1)
dB = chrono.ChVectorD(-7.11472591128629e-17,1.56492054588209e-16,1)
link_6.Initialize(body_1,body_3,False,cA,cB,dA,dB)
link_6.SetName("Concentric3")
exported_items.append(link_6)


# Mate constraint: Coincident2 [MateCoincident] type:0 align:1 flip:False
#   Entity 0: C::E name: body_1 , SW name: Part5-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_3 , SW name: Part4-2 ,  SW ref.type:2 (2)

link_7 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(0,0.12,-0.025)
cB = chrono.ChVectorD(-4.27435864480685e-14,0.119999999999905,-0.0249999999999998)
dA = chrono.ChVectorD(0,0,1)
dB = chrono.ChVectorD(7.11472591128629e-17,-1.56492054588209e-16,-1)
link_7.Initialize(body_1,body_3,False,cA,cB,dB)
link_7.SetDistance(0)
link_7.SetName("Coincident2")
exported_items.append(link_7)

link_8 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0,0.12,-0.025)
dA = chrono.ChVectorD(0,0,1)
cB = chrono.ChVectorD(-4.27435864480685e-14,0.119999999999905,-0.0249999999999998)
dB = chrono.ChVectorD(7.11472591128629e-17,-1.56492054588209e-16,-1)
link_8.SetFlipped(True)
link_8.Initialize(body_1,body_3,False,cA,cB,dA,dB)
link_8.SetName("Coincident2")
exported_items.append(link_8)


# Mate constraint: Coincident3 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_1 , SW name: Part5-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_2 , SW name: Part5-2 ,  SW ref.type:2 (2)

link_9 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(0,0,0)
cB = chrono.ChVectorD(0.7,0,1.73472347597681e-17)
dA = chrono.ChVectorD(0,-1,0)
dB = chrono.ChVectorD(0,-1,0)
link_9.Initialize(body_1,body_2,False,cA,cB,dB)
link_9.SetDistance(0)
link_9.SetName("Coincident3")
exported_items.append(link_9)

link_10 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(0,-1,0)
cB = chrono.ChVectorD(0.7,0,1.73472347597681e-17)
dB = chrono.ChVectorD(0,-1,0)
link_10.Initialize(body_1,body_2,False,cA,cB,dA,dB)
link_10.SetName("Coincident3")
exported_items.append(link_10)


# Mate constraint: Coincident5 [MateCoincident] type:0 align:2 flip:False
#   Entity 0: C::E name: body_1 , SW name: Part5-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_2 , SW name: Part5-2 ,  SW ref.type:1 (1)

link_11 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(-0.1,0.02,0.075)
cB = chrono.ChVectorD(0.6,0.02,0.075)
dA = chrono.ChVectorD(0,0,1)
dB = chrono.ChVectorD(0,-1,0)
link_11.Initialize(body_2,body_1,False,cB,cA,dA)
link_11.SetDistance(0)
link_11.SetName("Coincident5")
exported_items.append(link_11)

link_12 = chrono.ChLinkMateOrthogonal()
cA = chrono.ChVectorD(-0.1,0.02,0.075)
dA = chrono.ChVectorD(0,0,1)
cB = chrono.ChVectorD(0.6,0.02,0.075)
dB = chrono.ChVectorD(0,-1,0)
link_12.Initialize(body_2,body_1,False,cB,cA,dB,dA)
link_12.SetName("Coincident5")
exported_items.append(link_12)


# Mate constraint: Coincident8 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_4 , SW name: Part3-1 ,  SW ref.type:1 (1)
#   Entity 1: C::E name: body_6 , SW name: Part3-2 ,  SW ref.type:1 (1)

link_13 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0250000000000001)
dA = chrono.ChVectorD(-2.27056055777414e-16,-3.44324843526416e-17,1)
cB = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0249999999999999)
dB = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
link_13.Initialize(body_4,body_6,False,cA,cB,dA,dB)
link_13.SetName("Coincident8")
exported_items.append(link_13)

link_14 = chrono.ChLinkMateGeneric()
link_14.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0250000000000001)
cB = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0249999999999999)
dA = chrono.ChVectorD(-2.27056055777414e-16,-3.44324843526416e-17,1)
dB = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
link_14.Initialize(body_4,body_6,False,cA,cB,dA,dB)
link_14.SetName("Coincident8")
exported_items.append(link_14)


# Mate constraint: Coincident9 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_4 , SW name: Part3-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_6 , SW name: Part3-2 ,  SW ref.type:2 (2)

link_15 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(1.23096109835613,0.57615820943328,0.025)
cB = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0249999999999999)
dA = chrono.ChVectorD(-2.27056055777414e-16,-3.44324843526416e-17,1)
dB = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
link_15.Initialize(body_4,body_6,False,cA,cB,dB)
link_15.SetDistance(0)
link_15.SetName("Coincident9")
exported_items.append(link_15)

link_16 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(1.23096109835613,0.57615820943328,0.025)
dA = chrono.ChVectorD(-2.27056055777414e-16,-3.44324843526416e-17,1)
cB = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0249999999999999)
dB = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
link_16.Initialize(body_4,body_6,False,cA,cB,dA,dB)
link_16.SetName("Coincident9")
exported_items.append(link_16)


# Mate constraint: Coincident10 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_6 , SW name: Part3-2 ,  SW ref.type:5 (5)
#   Entity 1: C::E name: body_8 , SW name: Part3-3 ,  SW ref.type:5 (5)

link_17 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.45535714285511,1.02988360176314,-0.0249999999999995)
dA = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
cB = chrono.ChVectorD(2.45535714285633,1.02988360176294,-0.025)
dB = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
link_17.Initialize(body_6,body_8,False,cA,cB,dA,dB)
link_17.SetName("Coincident10")
exported_items.append(link_17)

link_18 = chrono.ChLinkMateGeneric()
link_18.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(2.45535714285511,1.02988360176314,-0.0249999999999995)
cB = chrono.ChVectorD(2.45535714285633,1.02988360176294,-0.025)
dA = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
dB = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
link_18.Initialize(body_6,body_8,False,cA,cB,dA,dB)
link_18.SetName("Coincident10")
exported_items.append(link_18)


# Mate constraint: Coincident12 [MateCoincident] type:0 align:2 flip:False
#   Entity 0: C::E name: body_8 , SW name: Part3-3 ,  SW ref.type:6 (6)
#   Entity 1: C::E name: body_9 , SW name: Part3-4 ,  SW ref.type:6 (6)

link_19 = chrono.ChLinkMateSpherical()
cA = chrono.ChVectorD(1.97141953120817,0.456905078577059,1.71754857526001e-16)
cB = chrono.ChVectorD(1.97141953120813,0.456905078577237,1.71754857525956e-16)
link_19.Initialize(body_8,body_9,False,cA,cB)
link_19.SetName("Coincident12")
exported_items.append(link_19)


# Mate constraint: Parallel1 [MateParallel] type:3 align:0 flip:False
#   Entity 0: C::E name: body_8 , SW name: Part3-3 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_9 , SW name: Part3-4 ,  SW ref.type:2 (2)

link_20 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.45535714285633,1.02988360176294,0.025)
dA = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
cB = chrono.ChVectorD(1.23096109835653,0.576158209433215,0.0250000000000002)
dB = chrono.ChVectorD(1.29226683360098e-29,3.03324324367862e-31,1)
link_20.Initialize(body_8,body_9,False,cA,cB,dA,dB)
link_20.SetName("Parallel1")
exported_items.append(link_20)


# Mate constraint: Coincident13 [MateCoincident] type:0 align:2 flip:False
#   Entity 0: C::E name: body_4 , SW name: Part3-1 ,  SW ref.type:5 (5)
#   Entity 1: C::E name: body_9 , SW name: Part3-4 ,  SW ref.type:6 (6)

link_21 = chrono.ChLinkMateGeneric()
link_21.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(1.23096109835613,0.57615820943328,-0.025)
cB = chrono.ChVectorD(1.23096109835653,0.576158209433215,1.71754857525966e-16)
dA = chrono.ChVectorD(-2.27056055777414e-16,-3.44324843526416e-17,1)
dB = chrono.VNULL
link_21.Initialize(body_9,body_4,False,cB,cA,dB,dA)
link_21.SetName("Coincident13")
exported_items.append(link_21)


# Mate constraint: Concentric5 [MateConcentric] type:1 align:0 flip:False
#   Entity 0: C::E name: body_3 , SW name: Part4-2 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_6 , SW name: Part3-2 ,  SW ref.type:2 (2)

link_22 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(1.71489871000471,1.1491367326181,-0.0249999999999999)
dA = chrono.ChVectorD(-7.11472591128629e-17,1.56492054588209e-16,1)
cB = chrono.ChVectorD(1.71489871000361,1.14913673261973,-0.0250000000000001)
dB = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
link_22.Initialize(body_3,body_6,False,cA,cB,dA,dB)
link_22.SetName("Concentric5")
exported_items.append(link_22)

link_23 = chrono.ChLinkMateGeneric()
link_23.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(1.71489871000471,1.1491367326181,-0.0249999999999999)
cB = chrono.ChVectorD(1.71489871000361,1.14913673261973,-0.0250000000000001)
dA = chrono.ChVectorD(-7.11472591128629e-17,1.56492054588209e-16,1)
dB = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
link_23.Initialize(body_3,body_6,False,cA,cB,dA,dB)
link_23.SetName("Concentric5")
exported_items.append(link_23)


# Mate constraint: Coincident14 [MateCoincident] type:0 align:2 flip:False
#   Entity 0: C::E name: body_3 , SW name: Part4-2 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_4 , SW name: Part3-1 ,  SW ref.type:1 (1)

link_24 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(-4.27435864480685e-14,0.119999999999905,0.0250000000000002)
cB = chrono.ChVectorD(1.67670014179118,1.1813992400629,0.0250000000000001)
dA = chrono.ChVectorD(-7.11472591128629e-17,1.56492054588209e-16,1)
dB = chrono.ChVectorD(-0.645250148863313,-0.763971364248604,-1.72813385836049e-16)
link_24.Initialize(body_4,body_3,False,cB,cA,dA)
link_24.SetDistance(0)
link_24.SetName("Coincident14")
exported_items.append(link_24)

link_25 = chrono.ChLinkMateOrthogonal()
cA = chrono.ChVectorD(-4.27435864480685e-14,0.119999999999905,0.0250000000000002)
dA = chrono.ChVectorD(-7.11472591128629e-17,1.56492054588209e-16,1)
cB = chrono.ChVectorD(1.67670014179118,1.1813992400629,0.0250000000000001)
dB = chrono.ChVectorD(-0.645250148863313,-0.763971364248604,-1.72813385836049e-16)
link_25.Initialize(body_4,body_3,False,cB,cA,dB,dA)
link_25.SetName("Coincident14")
exported_items.append(link_25)


# Mate constraint: Coincident15 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_5 , SW name: Part4-1 ,  SW ref.type:1 (1)
#   Entity 1: C::E name: body_8 , SW name: Part3-3 ,  SW ref.type:1 (1)

link_26 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(1.97141953120817,0.456905078577059,0.0250000000000002)
dA = chrono.ChVectorD(2.43731035938427e-18,-6.2718405890427e-19,1)
cB = chrono.ChVectorD(1.97141953120817,0.456905078577059,0.0250000000000002)
dB = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
link_26.Initialize(body_5,body_8,False,cA,cB,dA,dB)
link_26.SetName("Coincident15")
exported_items.append(link_26)

link_27 = chrono.ChLinkMateGeneric()
link_27.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(1.97141953120817,0.456905078577059,0.0250000000000002)
cB = chrono.ChVectorD(1.97141953120817,0.456905078577059,0.0250000000000002)
dA = chrono.ChVectorD(2.43731035938427e-18,-6.2718405890427e-19,1)
dB = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
link_27.Initialize(body_5,body_8,False,cA,cB,dA,dB)
link_27.SetName("Coincident15")
exported_items.append(link_27)


# Mate constraint: Hinge2 [MateHinge] type:22 align:1 flip:False
#   Entity 0: C::E name: body_7 , SW name: Part2-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_2 , SW name: Part5-2 ,  SW ref.type:2 (2)
#   Entity 2: C::E name: body_7 , SW name: Part2-1 ,  SW ref.type:2 (2)
#   Entity 3: C::E name: body_2 , SW name: Part5-2 ,  SW ref.type:2 (2)

link_28 = chrono.ChLinkMateCoaxial()
cA = chrono.ChVectorD(0.700000000001494,0.119999999999474,0.0250000000000002)
dA = chrono.ChVectorD(-3.04136016049598e-19,-2.3241720634671e-18,-1)
cB = chrono.ChVectorD(0.7,0.12,-0.075)
dB = chrono.ChVectorD(0,0,-1)
link_28.SetName("Hinge2")
link_28.Initialize(body_7,body_2,False,cA,cB,dA,dB)
exported_items.append(link_28)
link_29 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(0.700000000001494,0.119999999999474,0.0250000000000002)
dA = chrono.ChVectorD(3.04136016049598e-19,2.3241720634671e-18,1)
cB = chrono.ChVectorD(0.7,0.12,0.025)
dB = chrono.ChVectorD(0,0,-1)
link_29.SetName("Hinge2")
link_29.Initialize(body_7,body_2,False,cA,cB,dB)
exported_items.append(link_29)

# Mate constraint: Hinge3 [MateHinge] type:22 align:0 flip:False
#   Entity 0: C::E name: body_7 , SW name: Part2-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_9 , SW name: Part3-4 ,  SW ref.type:2 (2)
#   Entity 2: C::E name: body_9 , SW name: Part3-4 ,  SW ref.type:2 (2)
#   Entity 3: C::E name: body_7 , SW name: Part2-1 ,  SW ref.type:2 (2)

link_30 = chrono.ChLinkMateCoaxial()
cA = chrono.ChVectorD(1.23096109835591,0.576158209433731,0.0250000000000002)
dA = chrono.ChVectorD(-3.04136016049598e-19,-2.3241720634671e-18,-1)
cB = chrono.ChVectorD(1.23096109835653,0.576158209433215,-0.0249999999999998)
dB = chrono.ChVectorD(-1.29226683360098e-29,-3.03324324367862e-31,-1)
link_30.SetName("Hinge3")
link_30.Initialize(body_7,body_9,False,cA,cB,dA,dB)
exported_items.append(link_30)
link_31 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(1.23096109835653,0.576158209433215,0.0250000000000002)
dA = chrono.ChVectorD(1.29226683360098e-29,3.03324324367862e-31,1)
cB = chrono.ChVectorD(0.700000000001494,0.119999999999474,0.0250000000000002)
dB = chrono.ChVectorD(3.04136016049598e-19,2.3241720634671e-18,1)
link_31.SetName("Hinge3")
link_31.Initialize(body_9,body_7,False,cA,cB,dB)
exported_items.append(link_31)

# Mate constraint: Concentric7 [MateConcentric] type:1 align:0 flip:False
#   Entity 0: C::E name: body_8 , SW name: Part3-3 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_19 , SW name: Part6-1 ,  SW ref.type:2 (2)

link_32 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.45535714285633,1.02988360176294,-0.025)
dA = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
cB = chrono.ChVectorD(2.45535714285633,1.02988360176294,-0.124999999999998)
dB = chrono.ChVectorD(-1.26801646453392e-16,-1.88874422440737e-15,1)
link_32.Initialize(body_8,body_19,False,cA,cB,dA,dB)
link_32.SetName("Concentric7")
exported_items.append(link_32)

link_33 = chrono.ChLinkMateGeneric()
link_33.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(2.45535714285633,1.02988360176294,-0.025)
cB = chrono.ChVectorD(2.45535714285633,1.02988360176294,-0.124999999999998)
dA = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
dB = chrono.ChVectorD(-1.26801646453392e-16,-1.88874422440737e-15,1)
link_33.Initialize(body_8,body_19,False,cA,cB,dA,dB)
link_33.SetName("Concentric7")
exported_items.append(link_33)


# Mate constraint: Coincident20 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_6 , SW name: Part3-2 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_19 , SW name: Part6-1 ,  SW ref.type:2 (2)

link_34 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0249999999999999)
cB = chrono.ChVectorD(2.40535714285633,0.0798836017629421,0.0249999999999997)
dA = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
dB = chrono.ChVectorD(-1.26801646453392e-16,-1.88874422440737e-15,1)
link_34.Initialize(body_6,body_19,False,cA,cB,dB)
link_34.SetDistance(0)
link_34.SetName("Coincident20")
exported_items.append(link_34)

link_35 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(1.71489871000361,1.14913673261973,0.0249999999999999)
dA = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
cB = chrono.ChVectorD(2.40535714285633,0.0798836017629421,0.0249999999999997)
dB = chrono.ChVectorD(-1.26801646453392e-16,-1.88874422440737e-15,1)
link_35.Initialize(body_6,body_19,False,cA,cB,dA,dB)
link_35.SetName("Coincident20")
exported_items.append(link_35)


# Mate constraint: Coincident22 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_13 , SW name: Part4-3 ,  SW ref.type:5 (5)
#   Entity 1: C::E name: body_12 , SW name: Part5-3 ,  SW ref.type:5 (5)

link_36 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.45,-1.58,-0.025)
dA = chrono.ChVectorD(4.68683462596568e-42,-3.7721853057659e-43,1)
cB = chrono.ChVectorD(3.45,-1.58,0.025)
dB = chrono.ChVectorD(0,0,1)
link_36.Initialize(body_13,body_12,False,cA,cB,dA,dB)
link_36.SetName("Coincident22")
exported_items.append(link_36)

link_37 = chrono.ChLinkMateGeneric()
link_37.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(3.45,-1.58,-0.025)
cB = chrono.ChVectorD(3.45,-1.58,0.025)
dA = chrono.ChVectorD(4.68683462596568e-42,-3.7721853057659e-43,1)
dB = chrono.ChVectorD(0,0,1)
link_37.Initialize(body_13,body_12,False,cA,cB,dA,dB)
link_37.SetName("Coincident22")
exported_items.append(link_37)


# Mate constraint: Coincident23 [MateCoincident] type:0 align:2 flip:False
#   Entity 0: C::E name: body_13 , SW name: Part4-3 ,  SW ref.type:1 (1)
#   Entity 1: C::E name: body_12 , SW name: Part5-3 ,  SW ref.type:2 (2)

link_38 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.44940822033139,-1.61627796107299,0.025)
cB = chrono.ChVectorD(3.45,-1.58,0.025)
dA = chrono.ChVectorD(0.999905912939905,-0.0137173345740089,-4.69156808826601e-42)
dB = chrono.ChVectorD(0,0,-1)
link_38.Initialize(body_13,body_12,False,cA,cB,dB)
link_38.SetDistance(0)
link_38.SetName("Coincident23")
exported_items.append(link_38)

link_39 = chrono.ChLinkMateOrthogonal()
cA = chrono.ChVectorD(2.44940822033139,-1.61627796107299,0.025)
dA = chrono.ChVectorD(0.999905912939905,-0.0137173345740089,-4.69156808826601e-42)
cB = chrono.ChVectorD(3.45,-1.58,0.025)
dB = chrono.ChVectorD(0,0,-1)
link_39.Initialize(body_13,body_12,False,cA,cB,dA,dB)
link_39.SetName("Coincident23")
exported_items.append(link_39)


# Mate constraint: Coincident24 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_2 , SW name: Part5-2 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_12 , SW name: Part5-3 ,  SW ref.type:2 (2)

link_40 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(0.6,0.02,0.075)
cB = chrono.ChVectorD(3.35,-1.68,0.075)
dA = chrono.ChVectorD(0,0,1)
dB = chrono.ChVectorD(0,0,1)
link_40.Initialize(body_2,body_12,False,cA,cB,dB)
link_40.SetDistance(0)
link_40.SetName("Coincident24")
exported_items.append(link_40)

link_41 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0.6,0.02,0.075)
dA = chrono.ChVectorD(0,0,1)
cB = chrono.ChVectorD(3.35,-1.68,0.075)
dB = chrono.ChVectorD(0,0,1)
link_41.Initialize(body_2,body_12,False,cA,cB,dA,dB)
link_41.SetName("Coincident24")
exported_items.append(link_41)


# Mate constraint: Distance9 [MateDistanceDim] type:5 align:0 flip:True
#   Entity 0: C::E name: body_0 , SW name: Part5-2 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Part5-1 ,  SW ref.type:4 (4)

link_42 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(0.7,0,1.73472347597681e-17)
cB = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(1,0,0)
dB = chrono.ChVectorD(1,0,0)
link_42.Initialize(body_2,body_1,False,cA,cB,dB)
link_42.SetDistance(0.7)
link_42.SetName("Distance9")
exported_items.append(link_42)

link_43 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(0.7,0,1.73472347597681e-17)
dA = chrono.ChVectorD(1,0,0)
cB = chrono.ChVectorD(0,0,0)
dB = chrono.ChVectorD(1,0,0)
link_43.Initialize(body_2,body_1,False,cA,cB,dA,dB)
link_43.SetName("Distance9")
exported_items.append(link_43)


# Mate constraint: Distance17 [MateDistanceDim] type:5 align:1 flip:False
#   Entity 0: C::E name: body_11 , SW name: Part9-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_0 , SW name: Assem1 ,  SW ref.type:4 (4)

link_44 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(4.35888346239237,0.2,-2.17019237886467)
cB = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(-1.353534468183e-17,-1,-3.82563461323107e-17)
dB = chrono.ChVectorD(0,1,0)
link_44.Initialize(body_11,body_0,False,cA,cB,dB)
link_44.SetDistance(0.2)
link_44.SetName("Distance17")
exported_items.append(link_44)

link_45 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(4.35888346239237,0.2,-2.17019237886467)
dA = chrono.ChVectorD(-1.353534468183e-17,-1,-3.82563461323107e-17)
cB = chrono.ChVectorD(0,0,0)
dB = chrono.ChVectorD(0,1,0)
link_45.SetFlipped(True)
link_45.Initialize(body_11,body_0,False,cA,cB,dA,dB)
link_45.SetName("Distance17")
exported_items.append(link_45)


# Mate constraint: Coincident44 [MateCoincident] type:0 align:1 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part9-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Assem1 ,  SW ref.type:4 (4)

link_46 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(4.20022320835452,0.95,-2.425)
cB = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(-0.500000000000001,3.98986399474666e-17,-0.866025403784438)
dB = chrono.ChVectorD(0.500000000000001,0,0.866025403784438)
link_46.Initialize(body_11,body_0,False,cA,cB,dB)
link_46.SetDistance(0)
link_46.SetName("Coincident44")
exported_items.append(link_46)

link_47 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(4.20022320835452,0.95,-2.425)
dA = chrono.ChVectorD(-0.500000000000001,3.98986399474666e-17,-0.866025403784438)
cB = chrono.ChVectorD(0,0,0)
dB = chrono.ChVectorD(0.500000000000001,0,0.866025403784438)
link_47.SetFlipped(True)
link_47.Initialize(body_11,body_0,False,cA,cB,dA,dB)
link_47.SetName("Coincident44")
exported_items.append(link_47)


# Mate constraint: Coincident45 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part3-2 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Part3-3 ,  SW ref.type:4 (4)

link_48 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.08512792642936,1.08951016719144,2.3184356464553e-16)
cB = chrono.ChVectorD(2.21338833703225,0.743394340170001,8.42825665436867e-17)
dA = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
dB = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
link_48.Initialize(body_6,body_8,False,cA,cB,dB)
link_48.SetDistance(0)
link_48.SetName("Coincident45")
exported_items.append(link_48)

link_49 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.08512792642936,1.08951016719144,2.3184356464553e-16)
dA = chrono.ChVectorD(-9.34833691496372e-16,-7.71243118060831e-16,1)
cB = chrono.ChVectorD(2.21338833703225,0.743394340170001,8.42825665436867e-17)
dB = chrono.ChVectorD(3.29586719110857e-16,2.6955935802425e-17,1)
link_49.Initialize(body_6,body_8,False,cA,cB,dA,dB)
link_49.SetName("Coincident45")
exported_items.append(link_49)


# Mate constraint: Coincident47 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part4-3 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Part8-1 ,  SW ref.type:4 (4)

link_50 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17)
cB = chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17)
dA = chrono.ChVectorD(4.68683462596568e-42,-3.7721853057659e-43,1)
dB = chrono.ChVectorD(0,0,1)
link_50.Initialize(body_13,body_10,False,cA,cB,dB)
link_50.SetDistance(0)
link_50.SetName("Coincident47")
exported_items.append(link_50)

link_51 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17)
dA = chrono.ChVectorD(4.68683462596568e-42,-3.7721853057659e-43,1)
cB = chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17)
dB = chrono.ChVectorD(0,0,1)
link_51.Initialize(body_13,body_10,False,cA,cB,dA,dB)
link_51.SetName("Coincident47")
exported_items.append(link_51)


# Mate constraint: Coincident51 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part4-3 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Part8-1 ,  SW ref.type:4 (4)

link_52 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17)
cB = chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17)
dA = chrono.ChVectorD(0.0137173345740088,0.999905912939905,3.12892160536614e-43)
dB = chrono.ChVectorD(0.0137173345740088,0.999905912939905,0)
link_52.Initialize(body_13,body_10,False,cA,cB,dB)
link_52.SetDistance(0)
link_52.SetName("Coincident51")
exported_items.append(link_52)

link_53 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17)
dA = chrono.ChVectorD(0.0137173345740088,0.999905912939905,3.12892160536614e-43)
cB = chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17)
dB = chrono.ChVectorD(0.0137173345740088,0.999905912939905,0)
link_53.Initialize(body_13,body_10,False,cA,cB,dA,dB)
link_53.SetName("Coincident51")
exported_items.append(link_53)


# Mate constraint: Distance22 [MateDistanceDim] type:5 align:0 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part8-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Part4-3 ,  SW ref.type:4 (4)

link_54 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17)
cB = chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17)
dA = chrono.ChVectorD(0.999905912939905,-0.0137173345740088,0)
dB = chrono.ChVectorD(0.999905912939905,-0.0137173345740088,-4.69156808826601e-42)
link_54.Initialize(body_10,body_13,False,cA,cB,dB)
link_54.SetDistance(-1)
link_54.SetName("Distance22")
exported_items.append(link_54)

link_55 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.45009408706009,-1.56628266542599,-1.38777878078145e-17)
dA = chrono.ChVectorD(0.999905912939905,-0.0137173345740088,0)
cB = chrono.ChVectorD(3.45,-1.58,-1.38777878078145e-17)
dB = chrono.ChVectorD(0.999905912939905,-0.0137173345740088,-4.69156808826601e-42)
link_55.Initialize(body_10,body_13,False,cA,cB,dA,dB)
link_55.SetName("Distance22")
exported_items.append(link_55)


# Mate constraint: Distance23 [MateDistanceDim] type:5 align:1 flip:False
#   Entity 0: C::E name: body_0 , SW name: Part6-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Assem1 ,  SW ref.type:4 (4)

link_56 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.45535714285633,0.579883601762942,-0.0499999999999993)
cB = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(-1,-1.11022302462516e-16,-1.26801646453392e-16)
dB = chrono.ChVectorD(1,0,0)
link_56.Initialize(body_19,body_0,False,cA,cB,dB)
link_56.SetDistance(2.45535714285714)
link_56.SetName("Distance23")
exported_items.append(link_56)

link_57 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.45535714285633,0.579883601762942,-0.0499999999999993)
dA = chrono.ChVectorD(-1,-1.11022302462516e-16,-1.26801646453392e-16)
cB = chrono.ChVectorD(0,0,0)
dB = chrono.ChVectorD(1,0,0)
link_57.SetFlipped(True)
link_57.Initialize(body_19,body_0,False,cA,cB,dA,dB)
link_57.SetName("Distance23")
exported_items.append(link_57)


# Mate constraint: Coincident54 [MateCoincident] type:0 align:2 flip:False
#   Entity 0: C::E name: body_13 , SW name: Part4-3 ,  SW ref.type:6 (6)
#   Entity 1: C::E name: body_18 , SW name: Part8-2 ,  SW ref.type:System.__ComObject (25)

link_58 = chrono.ChLinkMateSpherical()
cA = chrono.ChVectorD(4.4499059129399,-1.59371733457401,-1.38777878078145e-17)
cB = chrono.ChVectorD(4.4499059129399,-1.59371733457401,-2.77555756156289e-17)
link_58.Initialize(body_13,body_18,False,cA,cB)
link_58.SetName("Coincident54")
exported_items.append(link_58)


# Mate constraint: Coincident61 [MateCoincident] type:0 align:0 flip:False
#   Entity 0: C::E name: body_0 , SW name: SubAssem1-1/Part1-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: Assem1 ,  SW ref.type:4 (4)

link_59 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.72390923627308,1.75,-2.15)
cB = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(0.500000000000001,5.82867087928207e-16,0.866025403784438)
dB = chrono.ChVectorD(0.500000000000001,0,0.866025403784438)
link_59.Initialize(body_17,body_0,False,cA,cB,dB)
link_59.SetDistance(0)
link_59.SetName("Coincident61")
exported_items.append(link_59)

link_60 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.72390923627308,1.75,-2.15)
dA = chrono.ChVectorD(0.500000000000001,5.82867087928207e-16,0.866025403784438)
cB = chrono.ChVectorD(0,0,0)
dB = chrono.ChVectorD(0.500000000000001,0,0.866025403784438)
link_60.Initialize(body_17,body_0,False,cA,cB,dA,dB)
link_60.SetName("Coincident61")
exported_items.append(link_60)


# Mate constraint: Distance24 [MateDistanceDim] type:5 align:0 flip:True
#   Entity 0: C::E name: body_17 , SW name: SubAssem1-1/Part1-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_0 , SW name: Assem1 ,  SW ref.type:4 (4)

link_61 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.72390923627308,1.8,-2.15)
cB = chrono.ChVectorD(0,0,0)
dA = chrono.ChVectorD(-3.05311331771918e-16,1,-3.88578058618805e-16)
dB = chrono.ChVectorD(0,1,0)
link_61.Initialize(body_17,body_0,False,cA,cB,dB)
link_61.SetDistance(1.8)
link_61.SetName("Distance24")
exported_items.append(link_61)

link_62 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.72390923627308,1.8,-2.15)
dA = chrono.ChVectorD(-3.05311331771918e-16,1,-3.88578058618805e-16)
cB = chrono.ChVectorD(0,0,0)
dB = chrono.ChVectorD(0,1,0)
link_62.Initialize(body_17,body_0,False,cA,cB,dA,dB)
link_62.SetName("Distance24")
exported_items.append(link_62)


# Mate constraint: Distance25 [MateDistanceDim] type:5 align:2 flip:True
#   Entity 0: C::E name: body_0 , SW name: SubAssem1-1/Part1-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name:  , SW name: Assem1 ,  SW ref.type:5 (5)


# Mate constraint: Distance26 [MateDistanceDim] type:5 align:1 flip:False
#   Entity 0: C::E name: body_17 , SW name: SubAssem1-1/Part1-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_11 , SW name: Part9-1 ,  SW ref.type:2 (2)

link_63 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.89131304684075,1.8,-2.16004809471617)
cB = chrono.ChVectorD(4.20022320835452,0.95,-2.425)
dA = chrono.ChVectorD(0.866025403784438,-8.32667268468867e-17,-0.500000000000001)
dB = chrono.ChVectorD(-0.866025403784438,-7.40622072271204e-18,0.500000000000001)
link_63.Initialize(body_17,body_11,False,cA,cB,dB)
link_63.SetDistance(0.4)
link_63.SetName("Distance26")
exported_items.append(link_63)

link_64 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.89131304684075,1.8,-2.16004809471617)
dA = chrono.ChVectorD(0.866025403784438,-8.32667268468867e-17,-0.500000000000001)
cB = chrono.ChVectorD(4.20022320835452,0.95,-2.425)
dB = chrono.ChVectorD(-0.866025403784438,-7.40622072271204e-18,0.500000000000001)
link_64.SetFlipped(True)
link_64.Initialize(body_17,body_11,False,cA,cB,dA,dB)
link_64.SetName("Distance26")
exported_items.append(link_64)


# Mate constraint: Concentric10 [MateConcentric] type:1 align:0 flip:False
#   Entity 0: C::E name: body_15 , SW name: SubAssem1-1/Part3-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_16 , SW name: SubAssem1-1/Part3-2 ,  SW ref.type:2 (2)

link_65 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.01190112847305,1.74564601301249,-1.88325949459386)
dA = chrono.ChVectorD(0.500000000000002,-4.05816873161324e-16,0.866025403784438)
cB = chrono.ChVectorD(2.98690112847487,1.74564601301157,-1.92656076478081)
dB = chrono.ChVectorD(0.499999999999998,-1.79869140415345e-16,0.86602540378444)
link_65.Initialize(body_15,body_16,False,cA,cB,dA,dB)
link_65.SetName("Concentric10")
exported_items.append(link_65)

link_66 = chrono.ChLinkMateGeneric()
link_66.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(3.01190112847305,1.74564601301249,-1.88325949459386)
cB = chrono.ChVectorD(2.98690112847487,1.74564601301157,-1.92656076478081)
dA = chrono.ChVectorD(0.500000000000002,-4.05816873161324e-16,0.866025403784438)
dB = chrono.ChVectorD(0.499999999999998,-1.79869140415345e-16,0.86602540378444)
link_66.Initialize(body_15,body_16,False,cA,cB,dA,dB)
link_66.SetName("Concentric10")
exported_items.append(link_66)


# Mate constraint: Coincident56 [MateCoincident] type:0 align:1 flip:False
#   Entity 0: C::E name: body_15 , SW name: SubAssem1-1/Part3-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_16 , SW name: SubAssem1-1/Part3-2 ,  SW ref.type:2 (2)

link_67 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.66140923627308,1.75,-2.25825317547306)
cB = chrono.ChVectorD(3.01190112847487,1.74564601301157,-1.88325949459158)
dA = chrono.ChVectorD(-0.500000000000002,4.05816873161324e-16,-0.866025403784438)
dB = chrono.ChVectorD(0.499999999999998,-1.79869140415345e-16,0.86602540378444)
link_67.Initialize(body_15,body_16,False,cA,cB,dB)
link_67.SetDistance(0)
link_67.SetName("Coincident56")
exported_items.append(link_67)

link_68 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.66140923627308,1.75,-2.25825317547306)
dA = chrono.ChVectorD(-0.500000000000002,4.05816873161324e-16,-0.866025403784438)
cB = chrono.ChVectorD(3.01190112847487,1.74564601301157,-1.88325949459158)
dB = chrono.ChVectorD(0.499999999999998,-1.79869140415345e-16,0.86602540378444)
link_68.SetFlipped(True)
link_68.Initialize(body_15,body_16,False,cA,cB,dA,dB)
link_68.SetName("Coincident56")
exported_items.append(link_68)


# Mate constraint: Coincident36 [MateCoincident] type:0 align:1 flip:False
#   Entity 0: C::E name: body_14 , SW name: SubAssem1-1/Part7-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_16 , SW name: SubAssem1-1/Part3-2 ,  SW ref.type:2 (2)

link_69 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.63324671958483,1.47409258339925,-1.69351078314862)
cB = chrono.ChVectorD(3.03156021041943,1.70109610845354,-1.89460967084429)
dA = chrono.ChVectorD(-0.393181638891139,0.890998091160603,0.227003525054216)
dB = chrono.ChVectorD(0.393181638891139,-0.890998091160603,-0.227003525054216)
link_69.Initialize(body_14,body_16,False,cA,cB,dB)
link_69.SetDistance(0)
link_69.SetName("Coincident36")
exported_items.append(link_69)

link_70 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.63324671958483,1.47409258339925,-1.69351078314862)
dA = chrono.ChVectorD(-0.393181638891139,0.890998091160603,0.227003525054216)
cB = chrono.ChVectorD(3.03156021041943,1.70109610845354,-1.89460967084429)
dB = chrono.ChVectorD(0.393181638891139,-0.890998091160603,-0.227003525054216)
link_70.SetFlipped(True)
link_70.Initialize(body_14,body_16,False,cA,cB,dA,dB)
link_70.SetName("Coincident36")
exported_items.append(link_70)


# Mate constraint: Coincident38 [MateCoincident] type:0 align:1 flip:False
#   Entity 0: C::E name: body_0 , SW name: SubAssem1-1/Part7-1 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_0 , SW name: SubAssem1-1/Part3-2 ,  SW ref.type:4 (4)

link_71 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.64897398514048,1.43845265975283,-1.70259092415079)
cB = chrono.ChVectorD(2.71004101034917,1.57539336922091,-1.73784798759358)
dA = chrono.ChVectorD(-0.499999999999999,5.55111512312578e-17,-0.866025403784439)
dB = chrono.ChVectorD(0.499999999999998,-1.79869140415345e-16,0.86602540378444)
link_71.Initialize(body_14,body_16,False,cA,cB,dB)
link_71.SetDistance(0)
link_71.SetName("Coincident38")
exported_items.append(link_71)

link_72 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.64897398514048,1.43845265975283,-1.70259092415079)
dA = chrono.ChVectorD(-0.499999999999999,5.55111512312578e-17,-0.866025403784439)
cB = chrono.ChVectorD(2.71004101034917,1.57539336922091,-1.73784798759358)
dB = chrono.ChVectorD(0.499999999999998,-1.79869140415345e-16,0.86602540378444)
link_72.SetFlipped(True)
link_72.Initialize(body_14,body_16,False,cA,cB,dA,dB)
link_72.SetName("Coincident38")
exported_items.append(link_72)


# Mate constraint: Distance20 [MateDistanceDim] type:5 align:1 flip:True
#   Entity 0: C::E name: body_0 , SW name: SubAssem1-1/Part3-2 ,  SW ref.type:4 (4)
#   Entity 1: C::E name: body_14 , SW name: SubAssem1-1/Part7-1 ,  SW ref.type:2 (2)

link_73 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(2.71004101034917,1.57539336922091,-1.73784798759358)
cB = chrono.ChVectorD(3.06170322823321,1.56489399342094,-1.30780157780029)
dA = chrono.ChVectorD(-0.771626981668526,-0.454007050108434,0.4454990455803)
dB = chrono.ChVectorD(0.771626981668526,0.454007050108434,-0.4454990455803)
link_73.Initialize(body_16,body_14,False,cA,cB,dB)
link_73.SetDistance(0)
link_73.SetName("Distance20")
exported_items.append(link_73)

link_74 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(2.71004101034917,1.57539336922091,-1.73784798759358)
dA = chrono.ChVectorD(-0.771626981668526,-0.454007050108434,0.4454990455803)
cB = chrono.ChVectorD(3.06170322823321,1.56489399342094,-1.30780157780029)
dB = chrono.ChVectorD(0.771626981668526,0.454007050108434,-0.4454990455803)
link_74.SetFlipped(True)
link_74.Initialize(body_16,body_14,False,cA,cB,dA,dB)
link_74.SetName("Distance20")
exported_items.append(link_74)


# Mate constraint: Concentric11 [MateConcentric] type:1 align:0 flip:False
#   Entity 0: C::E name: body_15 , SW name: SubAssem1-1/Part3-1 ,  SW ref.type:5 (5)
#   Entity 1: C::E name: body_17 , SW name: SubAssem1-1/Part1-1 ,  SW ref.type:2 (2)

link_75 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.66140923627308,1.75,-2.25825317547306)
dA = chrono.ChVectorD(0.500000000000002,-4.05816873161324e-16,0.866025403784438)
cB = chrono.ChVectorD(3.68640923627308,1.75,-2.21495190528384)
dB = chrono.ChVectorD(0.500000000000001,5.82867087928207e-16,0.866025403784438)
link_75.Initialize(body_15,body_17,False,cA,cB,dA,dB)
link_75.SetName("Concentric11")
exported_items.append(link_75)

link_76 = chrono.ChLinkMateGeneric()
link_76.SetConstrainedCoords(False, True, True, False, False, False)
cA = chrono.ChVectorD(3.66140923627308,1.75,-2.25825317547306)
cB = chrono.ChVectorD(3.68640923627308,1.75,-2.21495190528384)
dA = chrono.ChVectorD(0.500000000000002,-4.05816873161324e-16,0.866025403784438)
dB = chrono.ChVectorD(0.500000000000001,5.82867087928207e-16,0.866025403784438)
link_76.Initialize(body_15,body_17,False,cA,cB,dA,dB)
link_76.SetName("Concentric11")
exported_items.append(link_76)


# Mate constraint: Coincident57 [MateCoincident] type:0 align:1 flip:False
#   Entity 0: C::E name: body_15 , SW name: SubAssem1-1/Part3-1 ,  SW ref.type:2 (2)
#   Entity 1: C::E name: body_17 , SW name: SubAssem1-1/Part1-1 ,  SW ref.type:2 (2)

link_77 = chrono.ChLinkMateXdistance()
cA = chrono.ChVectorD(3.68640923627308,1.75,-2.21495190528384)
cB = chrono.ChVectorD(3.55650542570542,1.8,-2.13995190528384)
dA = chrono.ChVectorD(0.500000000000002,-4.05816873161324e-16,0.866025403784438)
dB = chrono.ChVectorD(-0.500000000000001,-5.82867087928207e-16,-0.866025403784438)
link_77.Initialize(body_15,body_17,False,cA,cB,dB)
link_77.SetDistance(0)
link_77.SetName("Coincident57")
exported_items.append(link_77)

link_78 = chrono.ChLinkMateParallel()
cA = chrono.ChVectorD(3.68640923627308,1.75,-2.21495190528384)
dA = chrono.ChVectorD(0.500000000000002,-4.05816873161324e-16,0.866025403784438)
cB = chrono.ChVectorD(3.55650542570542,1.8,-2.13995190528384)
dB = chrono.ChVectorD(-0.500000000000001,-5.82867087928207e-16,-0.866025403784438)
link_78.SetFlipped(True)
link_78.Initialize(body_15,body_17,False,cA,cB,dA,dB)
link_78.SetName("Coincident57")
exported_items.append(link_78)


# Auxiliary marker (coordinate system feature)
marker_0_1 =chrono.ChMarker()
marker_0_1.SetName('MARKER_SPRING')
body_0.AddMarker(marker_0_1)
marker_0_1.Impose_Abs_Coord(chrono.ChCoordsysD(chrono.ChVectorD(4.4499059129399,-1.59371733457401,-1.38777878078145E-17),chrono.ChQuaternionD(1,0,0,0)))
