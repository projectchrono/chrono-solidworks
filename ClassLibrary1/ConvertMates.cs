using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using ChronoEngineAddin;


namespace ChronoEngine_SwAddin
{
    class ConvertMates
    {

        public static bool ConvertMateToPython( 
                                    ref Feature swMateFeature,
                                    ref string asciitext, 
                                    ref ISldWorks mSWApplication,
                                    ref Hashtable saved_parts,
                                    ref int num_link,
                                    ref MathTransform roottrasf,
                                    ref Component2 assemblyofmates
                                    )
        {
            if (swMateFeature == null)
                return false;

            Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();

            if (swMate == null)
                return false;

            object foo =null;
            bool[] suppressedflags;
            suppressedflags = (bool[])swMateFeature.IsSuppressed2((int)swInConfigurationOpts_e.swThisConfiguration, foo);
            if (suppressedflags[0] == true)
                return false;

            if (swMate.GetMateEntityCount() >= 2)
            {
                 // Get the mated parts
                MateEntity2 swEntityA = swMate.MateEntity(0);
                MateEntity2 swEntityB = swMate.MateEntity(1);           
                Component2 swCompA = swEntityA.ReferenceComponent;
                Component2 swCompB = swEntityB.ReferenceComponent;
                double[] paramsA = (double[])swEntityA.EntityParams;
                double[] paramsB = (double[])swEntityB.EntityParams;
                 // this is needed because parts might reside in subassemblies, and mate params are expressed in parent subassembly
                MathTransform invroottrasf = (MathTransform)roottrasf.Inverse();
                MathTransform trA = roottrasf;
                MathTransform trB = roottrasf;
                
                if (assemblyofmates != null)
                {
                    MathTransform partrasfA = assemblyofmates.GetTotalTransform(true);
                    if (partrasfA != null)
                        trA = partrasfA.IMultiply(invroottrasf); // row-ordered transf. -> reverse mult.order!
                    MathTransform partrasfB = assemblyofmates.GetTotalTransform(true);
                    if (partrasfB != null)
                        trB = partrasfB.IMultiply(invroottrasf); // row-ordered transf. -> reverse mult.order!
                }
                

                 // Fetch the python names using hash map (python names added when scanning parts)
                ModelDocExtension swModelDocExt = default(ModelDocExtension);
                ModelDoc2 swModel = (ModelDoc2)mSWApplication.ActiveDoc;
                swModelDocExt = swModel.Extension;
                String name1 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompA)];
                String name2 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompB)];
                 
                 // Only constraints between two parts or part & layout can be created 
                if ( ((name1 != null) || (name2 != null)) && (name1 != name2) )
                {
                    CultureInfo bz = new CultureInfo("en-BZ");

                    if (name1 == null) 
                        name1 = "body_0";
                    if (name2 == null) 
                        name2 = "body_0";

                    // Add some comment in Python, to list the referenced SW items
                    asciitext += "\n# Mate constraint: " + swMateFeature.Name + " [" + swMateFeature.GetTypeName2() + "]" + " type:" + swMate.Type + " align:" + swMate.Alignment + " flip:" + swMate.Flipped + "\n";
                    for (int e = 0; e < swMate.GetMateEntityCount(); e++)
                    {
                        MateEntity2 swEntityN = swMate.MateEntity(e);
                        Component2 swCompN = swEntityN.ReferenceComponent;
                        String ce_nameN = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompN)];
                        asciitext += "#   Entity " + e + ": C::E name: " + ce_nameN + " , SW name: " + swCompN.Name2 + " ,  SW ref.type:" + swEntityN.Reference.GetType() + " (" + swEntityN.ReferenceType2 + ")\n";
                    }
                    asciitext += "\n";

                    //
                    // For each type of SW mate, see which C::E mate constraint(s) 
                    // must be created. Some SW mates correspond to more than one C::E constraints.
                    // 

                    bool swapAB_1 = false;
                    bool do_CHmate_Xdistance  = false;
                    double do_distance_val  = 0.0;
                    bool do_CHmate_parallel   = false;
                    bool   do_parallel_flip     = false;
                    bool do_CHmate_orthogonal = false;
                    bool do_CHmate_spherical  = false;
                    bool do_CHmate_pointline  = false;

                    // to simplify things later...
                    // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
                    // but in some cases the latter fails.
                    bool entity_0_as_FACE   = (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelFACES);
                    bool entity_0_as_EDGE   = (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelEDGES) ||
                                              (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                              (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelDATUMAXES);
                    bool entity_0_as_VERTEX = (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelVERTICES) ||
                                              (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                              (swMate.MateEntity(0).Reference.GetType() == (int)swSelectType_e.swSelDATUMPOINTS);

                    bool entity_1_as_FACE   = (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelFACES);
                    bool entity_1_as_EDGE   = (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelEDGES) ||
                                              (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                              (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelDATUMAXES);
                    bool entity_1_as_VERTEX = (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelVERTICES) ||
                                              (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                              (swMate.MateEntity(1).Reference.GetType() == (int)swSelectType_e.swSelDATUMPOINTS);

                    Point3D cA  = new Point3D(0, 0, 0);
                    Point3D cB  = new Point3D(0, 0, 0);
                    Vector3D dA = new Vector3D(1, 0, 0);
                    Vector3D dB = new Vector3D(1, 0, 0);

                    Point3D cAloc = new Point3D(paramsA[0], paramsA[1], paramsA[2]);
                    cA = SWTaskpaneHost.PointTransform(cAloc, ref trA);
                    Point3D cBloc = new Point3D(paramsB[0], paramsB[1], paramsB[2]);
                    cB = SWTaskpaneHost.PointTransform(cBloc, ref trB);

                    if (!entity_0_as_VERTEX)
                    {
                        Vector3D dAloc = new Vector3D(paramsA[3], paramsA[4], paramsA[5]);
                        dA = SWTaskpaneHost.DirTransform(dAloc, ref trA);
                    }

                    if (!entity_1_as_VERTEX)
                    {
                        Vector3D dBloc = new Vector3D(paramsB[3], paramsB[4], paramsB[5]);
                        dB = SWTaskpaneHost.DirTransform(dBloc, ref trB);
                    }
                    


                    if (swMateFeature.GetTypeName2() == "MateCoincident")
                    {
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_Xdistance = true; 
                            do_CHmate_parallel  = true; 
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_pointline = true;
                            do_CHmate_parallel  = true;
                        }
                        if ((entity_0_as_VERTEX) &&
                            (entity_1_as_VERTEX))
                        {
                            do_CHmate_spherical = true;
                        }
                        if ((entity_0_as_VERTEX) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_pointline = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_VERTEX))
                        {
                            do_CHmate_pointline = true;
                            swapAB_1 = true;
                        }
                        if ((entity_0_as_VERTEX) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_Xdistance = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_VERTEX))
                        {
                            do_CHmate_Xdistance = true;
                            swapAB_1 = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_Xdistance = true;
                            do_CHmate_orthogonal = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_Xdistance = true;
                            do_CHmate_orthogonal = true;
                            swapAB_1 = true;
                        }

                        if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                            do_parallel_flip = true;
                    }

                    if (swMateFeature.GetTypeName2() == "MateConcentric")
                    {
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_pointline = true;
                            do_CHmate_parallel = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_pointline = true;
                            do_CHmate_parallel = true;
                        }
                        if ((entity_0_as_VERTEX) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_pointline = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_VERTEX))
                        {
                            do_CHmate_pointline = true;
                            swapAB_1 = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_pointline = true;
                            do_CHmate_parallel = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_pointline = true;
                            do_CHmate_parallel = true;
                            swapAB_1 = true;
                        }

                        if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                            do_parallel_flip = true;
                    }

                    if (swMateFeature.GetTypeName2() == "MateParallel")
                    {
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_parallel = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_parallel = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_orthogonal = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_orthogonal = true;
                            swapAB_1 = true;
                        }

                        if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                            do_parallel_flip = true;
                    }


                    if (swMateFeature.GetTypeName2() == "MatePerpendicular")
                    {
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_orthogonal = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_orthogonal = true;
                        }
                        if ((entity_0_as_EDGE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_parallel = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_EDGE))
                        {
                            do_CHmate_parallel = true;
                            swapAB_1 = true;
                        }

                        if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                            do_parallel_flip = true;
                    }

                    if (swMateFeature.GetTypeName2() == "MateDistanceDim")
                    {
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_Xdistance  = true;
                            do_CHmate_parallel   = true;
                        }
                        if ((entity_0_as_VERTEX) &&
                            (entity_1_as_FACE))
                        {
                            do_CHmate_Xdistance = true;
                        }
                        if ((entity_0_as_FACE) &&
                            (entity_1_as_VERTEX))
                        {
                            do_CHmate_Xdistance = true;
                            swapAB_1 = true;
                        }

                        //***TO DO*** cases of distance line-vs-line and line-vs-vertex and vert-vert.
                        //           Those will require another .cpp ChLinkMate specialized class(es).

                        if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                            do_parallel_flip = true;

                        // Get the imposed distance value, in SI units
                        string confnames = "";
                        do_distance_val = swMate.DisplayDimension.GetDimension2(0).IGetSystemValue3((int)swInConfigurationOpts_e.swThisConfiguration, 0, ref confnames);

                        if (swMate.Flipped)
                            do_distance_val = -do_distance_val;
                    }

                    //// 
                    //// WRITE PYHTON CODE CORRESPONDING TO CONSTRAINTS
                    ////

                    if (do_CHmate_Xdistance)
                    {
                        num_link++;
                        String linkname = "link_" + num_link;
                        asciitext += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                        asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cA.X * ChScale.L,
                                  cA.Y * ChScale.L,
                                  cA.Z * ChScale.L);
                        asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cB.X * ChScale.L,
                                  cB.Y * ChScale.L,
                                  cB.Z * ChScale.L);
                        if (!entity_0_as_VERTEX)
                         asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  dA.X, dA.Y, dA.Z);
                        if (!entity_1_as_VERTEX)
                         asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  dB.X, dB.Y, dB.Z);

                        // Initialize link, by setting the two csys, in absolute space,
                        if (!swapAB_1)
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, name1, name2);
                        else
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dA)\n", linkname, name2, name1);

                        //if (do_distance_val!=0)
                            asciitext += String.Format(bz, "{0}.SetDistance({1})\n", linkname,
                                do_distance_val * ChScale.L *-1);

                        asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                        // Insert to a list of exported items
                        asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                    }

                    if (do_CHmate_parallel)
                    {
                        if (Math.Abs(Vector3D.DotProduct(dA, dB)) > 0.98)
                        {
                            num_link++;
                            String linkname = "link_" + num_link;
                            asciitext += String.Format(bz, "{0} = chrono.ChLinkMateParallel()\n", linkname);

                            asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      cA.X * ChScale.L,
                                      cA.Y * ChScale.L,
                                      cA.Z * ChScale.L);
                            asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      dA.X, dA.Y, dA.Z);
                            asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      cB.X * ChScale.L,
                                      cB.Y * ChScale.L,
                                      cB.Z * ChScale.L);
                            asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      dB.X, dB.Y, dB.Z);

                            if (do_parallel_flip)
                                asciitext += String.Format(bz, "{0}.SetFlipped(True)\n", linkname);

                            // Initialize link, by setting the two csys, in absolute space,
                            if (!swapAB_1)
                                asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, name1, name2);
                            else
                                asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, name2, name1);

                            asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                            // Insert to a list of exported items
                            asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                        }
                        else
                        {
                            asciitext += "\n# ChLinkMateParallel skipped because directions not parallel! \n";
                        }
                    }

                    if (do_CHmate_orthogonal)
                    {
                        if (Math.Abs(Vector3D.DotProduct(dA, dB)) < 0.02)
                        {
                            num_link++;
                            String linkname = "link_" + num_link;
                            asciitext += String.Format(bz, "{0} = chrono.ChLinkMateOrthogonal()\n", linkname);

                            asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      cA.X * ChScale.L,
                                      cA.Y * ChScale.L,
                                      cA.Z * ChScale.L);
                            asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      dA.X, dA.Y, dA.Z);
                            asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      cB.X * ChScale.L,
                                      cB.Y * ChScale.L,
                                      cB.Z * ChScale.L);
                            asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                      dB.X, dB.Y, dB.Z);

                            // Initialize link, by setting the two csys, in absolute space,
                            if (!swapAB_1)
                                asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, name1, name2);
                            else
                                asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, name2, name1);

                            asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                            // Insert to a list of exported items
                            asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                        } 
                        else
                        {
                            asciitext += "\n# ChLinkMateOrthogonal skipped because directions not orthogonal! \n";
                        }
                    }

                    if (do_CHmate_spherical)
                    {
                        num_link++;
                        String linkname = "link_" + num_link;
                        asciitext += String.Format(bz, "{0} = chrono.ChLinkMateSpherical()\n", linkname);

                        asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cA.X * ChScale.L,
                                  cA.Y * ChScale.L,
                                  cA.Z * ChScale.L);
                        asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cB.X * ChScale.L,
                                  cB.Y * ChScale.L,
                                  cB.Z * ChScale.L);

                        // Initialize link, by setting the two csys, in absolute space,
                        if (!swapAB_1)
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB)\n", linkname, name1, name2);
                        else
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA)\n", linkname, name2, name1);

                        asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                        // Insert to a list of exported items
                        asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                    }

                    if (do_CHmate_pointline)
                    {
                        num_link++;
                        String linkname = "link_" + num_link;
                        asciitext += String.Format(bz, "{0} = chrono.ChLinkMateGeneric()\n", linkname);
                        asciitext += String.Format(bz, "{0}.SetConstrainedCoords(False, True, True, False, False, False)\n", linkname);

                        asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cA.X * ChScale.L,
                                  cA.Y * ChScale.L,
                                  cA.Z * ChScale.L);
                        asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cB.X * ChScale.L,
                                  cB.Y * ChScale.L,
                                  cB.Z * ChScale.L); 
                        if (!entity_0_as_VERTEX)
                            asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n", dA.X, dA.Y, dA.Z);
                        else
                            asciitext += String.Format(bz, "dA = chrono.VNULL\n");
                        if (!entity_1_as_VERTEX)
                            asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n", dB.X, dB.Y, dB.Z);
                        else
                            asciitext += String.Format(bz, "dB = chrono.VNULL\n");

                        // Initialize link, by setting the two csys, in absolute space,
                        if (!swapAB_1)
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, name1, name2);
                        else
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, name2, name1);

                        asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                        // Insert to a list of exported items
                        asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                    }



                    // Now, do some other special mate type that did not fall in combinations
                    // of do_CHmate_pointline, do_CHmate_spherical, etc etc

                    if (swMateFeature.GetTypeName2() == "MateHinge")
                    {
                        // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                        if (Vector3D.DotProduct(dA, dB) < 0)
                            dB.Negate();

                        // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                        num_link++;
                        String linkname = "link_" + num_link;
                        asciitext += String.Format(bz, "{0} = chrono.ChLinkMateCoaxial()\n", linkname);

                        asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cA.X * ChScale.L,
                                  cA.Y * ChScale.L,
                                  cA.Z * ChScale.L);
                        asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  dA.X, dA.Y, dA.Z);
                        asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cB.X * ChScale.L,
                                  cB.Y * ChScale.L,
                                  cB.Z * ChScale.L);
                        asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  dB.X, dB.Y, dB.Z);

                        asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);

                        
                        // Initialize link, by setting the two csys, in absolute space,
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, name1, name2);

                        // Insert to a list of exported items
                        asciitext += String.Format(bz, "exported_items.append({0})\n", linkname);


                        // NOTE!!! The 'hinge' mate uses 4 references: fetch the two others remaining
                        // and build another C::E link, for point-vs-face mating

                        MateEntity2 swEntityC = swMate.MateEntity(2);
                        MateEntity2 swEntityD = swMate.MateEntity(3);
                        Component2 swCompC = swEntityC.ReferenceComponent;
                        Component2 swCompD = swEntityD.ReferenceComponent;
                        double[] paramsC = (double[])swEntityC.EntityParams;
                        double[] paramsD = (double[])swEntityD.EntityParams;
                        String name3 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompC)];
                        String name4 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompD)];
                        MathTransform trC = roottrasf;
                        MathTransform trD = roottrasf;
                        
                        if (assemblyofmates != null)
                        {
                            MathTransform partrasfC = assemblyofmates.GetTotalTransform(true);
                            if (partrasfC != null)
                                trC = partrasfC.IMultiply(invroottrasf);
                            MathTransform partrasfD = assemblyofmates.GetTotalTransform(true);
                            if (partrasfD != null)
                                trD = partrasfD.IMultiply(invroottrasf);
                        }

                        bool entity_2_as_VERTEX = (swMate.MateEntity(2).Reference.GetType() == (int)swSelectType_e.swSelVERTICES) ||
                                                  (swMate.MateEntity(2).Reference.GetType() == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                                  (swMate.MateEntity(2).Reference.GetType() == (int)swSelectType_e.swSelDATUMPOINTS);
                        bool entity_3_as_VERTEX = (swMate.MateEntity(3).Reference.GetType() == (int)swSelectType_e.swSelVERTICES) ||
                                                  (swMate.MateEntity(3).Reference.GetType() == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                                  (swMate.MateEntity(3).Reference.GetType() == (int)swSelectType_e.swSelDATUMPOINTS);
                        Point3D cC  = new Point3D(0, 0, 0);
                        Point3D cD  = new Point3D(0, 0, 0);
                        Vector3D dC = new Vector3D(1, 0, 0);
                        Vector3D dD = new Vector3D(1, 0, 0);

                        Point3D cCloc = new Point3D(paramsC[0], paramsC[1], paramsC[2]);
                        cC = SWTaskpaneHost.PointTransform(cCloc, ref trC);
                        Point3D cDloc = new Point3D(paramsD[0], paramsD[1], paramsD[2]);
                        cD = SWTaskpaneHost.PointTransform(cDloc, ref trD);

                        if (!entity_2_as_VERTEX)
                        {
                            Vector3D dCloc = new Vector3D(paramsC[3], paramsC[4], paramsC[5]);
                            dC = SWTaskpaneHost.DirTransform(dCloc, ref trC);
                        }

                        if (!entity_3_as_VERTEX)
                        {
                            Vector3D dDloc = new Vector3D(paramsD[3], paramsD[4], paramsD[5]);
                            dD = SWTaskpaneHost.DirTransform(dDloc, ref trD);
                        }

                        num_link++;
                        linkname = "link_" + num_link;
                        asciitext += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                        asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cC.X * ChScale.L,
                                  cC.Y * ChScale.L,
                                  cC.Z * ChScale.L);
                        asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  dC.X, dC.Y, dC.Z);
                        asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  cD.X * ChScale.L,
                                  cD.Y * ChScale.L,
                                  cD.Z * ChScale.L);
                        asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                                  dD.X, dD.Y, dD.Z);

                        asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                        // Initialize link, by setting the two csys, in absolute space,
                        if (entity_2_as_VERTEX)
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA)\n", linkname, name3, name4);
                        else
                            asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, name3, name4);

                        // Insert to a list of exported items
                        asciitext += String.Format(bz, "exported_items.append({0})\n", linkname);

                        return true;
                    }
                }
            }
            return false;
        }

    } // end class

} // end namespaces
