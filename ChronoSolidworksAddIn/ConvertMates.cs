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

// for Json Export
using System.Text.Json.Nodes;


namespace ChronoEngine_SwAddin
{
    class ConvertMates
    {

        public struct LinkParams {
            public bool swapAB_1;
            public bool do_ChLinkMateXdistance;
            public double do_distance_val;
            public bool do_ChLinkMateParallel;
            public bool do_parallel_flip;
            public bool do_ChLinkMateOrthogonal;
            public bool do_ChLinkMateSpherical;
            public bool do_ChLinkMatePointLine;
            public int entity0_ref;
            public int entity1_ref;
            public int entity2_ref;
            public int entity3_ref;
            public bool entity_0_as_FACE;
            public bool entity_0_as_EDGE;
            public bool entity_0_as_VERTEX;
            public bool entity_1_as_FACE;
            public bool entity_1_as_EDGE;
            public bool entity_1_as_VERTEX;
            public bool entity_2_as_VERTEX;
            public bool entity_3_as_VERTEX;
            public Point3D cA;
            public Point3D cB;
            public Vector3D dA;
            public Vector3D dB;
            public Point3D cC;
            public Point3D cD;
            public Vector3D dC;
            public Vector3D dD;
            public string ref1;
            public string ref2;
            public string ref3;
            public string ref4;
        };

        public static bool ConvertMateToJson(
                                    in Feature swMateFeature,
                                    ref JsonArray ChSystemLinklistArray,
                                    in ISldWorks mSWApplication,
                                    in Hashtable saved_parts,
                                    ref int _object_ID_used,
                                    in MathTransform roottrasf,
                                    in Component2 assemblyofmates
                                    )
        {
            LinkParams link_params;
            GetLinkParameters(swMateFeature, out link_params, mSWApplication, saved_parts, roottrasf, assemblyofmates);


            // TODO: redundant part of code
            Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();
            // Fetch the python names using hash map (python names added when scanning parts)
            ModelDocExtension swModelDocExt = default(ModelDocExtension);
            ModelDoc2 swModel = (ModelDoc2)mSWApplication.ActiveDoc;
            swModelDocExt = swModel.Extension;


            CultureInfo bz = new CultureInfo("en-BZ");

            if (link_params.do_ChLinkMateXdistance)
            {
                var link_node = new JsonObject
                {
                    ["_object_ID"] = ++_object_ID_used,
                    ["_type"] = "ChLinkMateXdistance",
                    ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_pos_are_relative"] = false
                };

                if (!link_params.swapAB_1)
                {
                    link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                    link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                    link_node["_c_Initialize_pt1"] = new JsonArray(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L);
                        link_node["_c_Initialize_pt2"] = new JsonArray(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L);
                    link_node["_c_Initialize_dir2"] = new JsonArray(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                }
                else
                {
                    link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                    link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                    link_node["_c_Initialize_pt1"] = new JsonArray(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L);
                    link_node["_c_Initialize_pt2"] = new JsonArray(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L);
                    link_node["_c_Initialize_dir2"] = new JsonArray(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);

                }

                link_node["_c_SetDistance"] = link_params.do_distance_val * ChScale.L * -1;
                link_node["m_name"] = swMateFeature.Name;

                ChSystemLinklistArray.Add(link_node);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {
                    var link_node = new JsonObject
                    {
                        ["_object_ID"] = ++_object_ID_used,
                        ["_type"] = "ChLinkMateParallel",
                        ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                        ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                        ["_c_Initialize_pos_are_relative"] = false
                    };

                    if (!link_params.swapAB_1)
                    {
                        link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                        link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                        link_node["_c_Initialize_pt1"] = new JsonArray(
                            link_params.cA.X * ChScale.L,
                            link_params.cA.Y * ChScale.L,
                            link_params.cA.Z * ChScale.L);
                        link_node["_c_Initialize_pt2"] = new JsonArray(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L);
                        link_node["_c_Initialize_norm1"] = new JsonArray(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                        link_node["_c_Initialize_norm2"] = new JsonArray(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                    }
                    else
                    {
                        link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                        link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                        link_node["_c_Initialize_pt1"] = new JsonArray(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L);
                        link_node["_c_Initialize_pt2"] = new JsonArray(
                            link_params.cA.X * ChScale.L,
                            link_params.cA.Y * ChScale.L,
                            link_params.cA.Z * ChScale.L);
                        link_node["_c_Initialize_norm1"] = new JsonArray(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                        link_node["_c_Initialize_norm2"] = new JsonArray(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    }

                    if (link_params.do_parallel_flip)
                        link_node["_c_SetFlipped"] = true;
                    link_node["m_name"] = swMateFeature.Name;

                    ChSystemLinklistArray.Add(link_node);

                }
                else
                {
                    // ChLinkMateParallel skipped because directions not parallel
                }
            }

            if (link_params.do_ChLinkMateOrthogonal)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) < 0.02)
                {
                    var link_node = new JsonObject
                    {
                        ["_object_ID"] = ++_object_ID_used,
                        ["_type"] = "ChLinkMateOrthogonal",
                        ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                        ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                        ["_c_Initialize_pos_are_relative"] = false
                    };

                    if (!link_params.swapAB_1)
                    {
                        link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                        link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                        link_node["_c_Initialize_pt1"] = new JsonArray(
                            link_params.cA.X * ChScale.L,
                            link_params.cA.Y * ChScale.L,
                            link_params.cA.Z * ChScale.L);
                        link_node["_c_Initialize_pt2"] = new JsonArray(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L);
                        link_node["_c_Initialize_norm1"] = new JsonArray(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                        link_node["_c_Initialize_norm2"] = new JsonArray(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                    }
                    else
                    {
                        link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                        link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                        link_node["_c_Initialize_pt1"] = new JsonArray(
                            link_params.cB.X * ChScale.L,
                            link_params.cB.Y * ChScale.L,
                            link_params.cB.Z * ChScale.L);
                        link_node["_c_Initialize_pt2"] = new JsonArray(
                            link_params.cA.X* ChScale.L,
                            link_params.cA.Y* ChScale.L,
                            link_params.cA.Z* ChScale.L);
                        link_node["_c_Initialize_norm1"] = new JsonArray(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                        link_node["_c_Initialize_norm2"] = new JsonArray(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    }

                    link_node["m_name"] = swMateFeature.Name;

                    ChSystemLinklistArray.Add(link_node);

                }
                else
                {
                    // ChLinkMateOrthogonal skipped because directions not orthogonal
                }
            }

            if (link_params.do_ChLinkMateSpherical)
            {

                var link_node = new JsonObject
                {
                    ["_object_ID"] = ++_object_ID_used,
                    ["_type"] = "ChLinkMateSpherical",
                    ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_pos_are_relative"] = false
                };


                if (!link_params.swapAB_1) // TODO: optimize writing
                {
                    link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                    link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                    link_node["_c_Initialize_pt1"] = new JsonArray(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L);
                    link_node["_c_Initialize_pt2"] = new JsonArray(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L);
                }
                else
                {
                    link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                    link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                    link_node["_c_Initialize_pt1"] = new JsonArray(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L);
                    link_node["_c_Initialize_pt2"] = new JsonArray(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L);
                }

                link_node["m_name"] = swMateFeature.Name;

                ChSystemLinklistArray.Add(link_node);

            }

            if (link_params.do_ChLinkMatePointLine)
            {
                var link_node = new JsonObject
                {
                    ["_object_ID"] = ++_object_ID_used,
                    ["_type"] = "ChLinkMateGeneric",
                    ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_pos_are_relative"] = false
                };


                link_node["_c_SetConstrainedCoords"] = new JsonArray(false, true, true, false, false, false);

                Vector3D dA_temp;
                if (!link_params.entity_0_as_VERTEX)
                    dA_temp = link_params.dA;
                else
                    dA_temp = new Vector3D(0.0, 0.0, 0.0);

                Vector3D dB_temp;
                if (!link_params.entity_1_as_VERTEX)
                    dB_temp = link_params.dB;
                else
                    dB_temp = new Vector3D(0.0, 0.0, 0.0);


                if (!link_params.swapAB_1) // TODO: optimize writing
                {
                    link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                    link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                    link_node["_c_Initialize_pt1"] = new JsonArray(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L);
                    link_node["_c_Initialize_pt2"] = new JsonArray(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L);
                    link_node["_c_Initialize_norm1"] = new JsonArray(dA_temp.X, dA_temp.Y, dA_temp.Z);
                    link_node["_c_Initialize_norm2"] = new JsonArray(dB_temp.X, dB_temp.Y, dB_temp.Z);
                }
                else
                {
                    link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                    link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                    link_node["_c_Initialize_pt1"] = new JsonArray(
                        link_params.cB.X * ChScale.L,
                        link_params.cB.Y * ChScale.L,
                        link_params.cB.Z * ChScale.L);
                    link_node["_c_Initialize_pt2"] = new JsonArray(
                        link_params.cA.X * ChScale.L,
                        link_params.cA.Y * ChScale.L,
                        link_params.cA.Z * ChScale.L);
                    link_node["_c_Initialize_norm1"] = new JsonArray(dB_temp.X, dB_temp.Y, dB_temp.Z);
                    link_node["_c_Initialize_norm2"] = new JsonArray(dA_temp.X, dA_temp.Y, dA_temp.Z);
                }

                link_node["m_name"] = swMateFeature.Name;

                ChSystemLinklistArray.Add(link_node);

            }



            // Now, do some other special mate type that did not fall in combinations
            // of link_params.do_ChLinkMatePointLine, link_params.do_ChLinkMateSpherical, etc etc

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                if (Vector3D.DotProduct(link_params.dA, link_params.dB) < 0)
                    link_params.dB.Negate();

                // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                var link_node = new JsonObject
                {
                    ["_object_ID"] = ++_object_ID_used,
                    ["_type"] = "ChLinkMateCoaxial",
                    ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_pos_are_relative"] = false
                };

                link_node["_c_SetConstrainedCoords"] = new JsonArray(false, true, true, false, false, false);


                link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref1);
                link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref2);
                link_node["_c_Initialize_pt1"] = new JsonArray(
                    link_params.cA.X * ChScale.L,
                    link_params.cA.Y * ChScale.L,
                    link_params.cA.Z * ChScale.L);
                link_node["_c_Initialize_pt2"] = new JsonArray(
                    link_params.cB.X * ChScale.L,
                    link_params.cB.Y * ChScale.L,
                    link_params.cB.Z * ChScale.L);
                link_node["_c_Initialize_dir1"] = new JsonArray(link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                link_node["_c_Initialize_dir2"] = new JsonArray(link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                link_node["m_name"] = swMateFeature.Name + "_1";

                ChSystemLinklistArray.Add(link_node);


                ////////////
                ///
                // TODO: DARIOM provide function CreateLink so that these two links can be interpreted as ChLinkMateCoaxial + ChLinkMateXdistance
                // and the LinkParams struct can avoid duplicated entries 

                link_node = new JsonObject
                {
                    ["_object_ID"] = ++_object_ID_used,
                    ["_type"] = "ChLinkMateXdistance",
                    ["_c_Initialize_Body1"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_Body2"] = new JsonObject { ["_type"] = "ChBodyAuxRef" },
                    ["_c_Initialize_pos_are_relative"] = false
                };

                link_node["_c_Initialize_Body1"]["_reference_ID"] = Convert.ToInt64(link_params.ref3);
                link_node["_c_Initialize_Body2"]["_reference_ID"] = Convert.ToInt64(link_params.ref4);
                link_node["_c_Initialize_pt1"] = new JsonArray(
                    link_params.cC.X * ChScale.L,
                    link_params.cC.Y * ChScale.L,
                    link_params.cC.Z * ChScale.L);
                link_node["_c_Initialize_pt2"] = new JsonArray(
                    link_params.cD.X * ChScale.L,
                    link_params.cD.Y * ChScale.L,
                    link_params.cD.Z * ChScale.L);
                if (link_params.entity_2_as_VERTEX)
                {
                    link_node["_c_Initialize_dir2"] = new JsonArray(link_params.dC.X, link_params.dC.Y, link_params.dC.Z);
                }
                else
                {
                    link_node["_c_Initialize_dir2"] = new JsonArray(link_params.dD.X, link_params.dD.Y, link_params.dD.Z);
                }

                //link_node["_c_SetDistance"] = link_params.do_distance_val * ChScale.L * -1;
                link_node["m_name"] = swMateFeature.Name + "_2";

                ChSystemLinklistArray.Add(link_node);

            }


            return true;

        }

        public static bool ConvertMateToPython(
                            in Feature swMateFeature,
                            ref string asciitext,
                            in ISldWorks mSWApplication,
                            in Hashtable saved_parts,
                            ref int num_link,
                            in MathTransform roottrasf,
                            in Component2 assemblyofmates
                            )
        {

            LinkParams link_params;
            GetLinkParameters(swMateFeature, out link_params, mSWApplication, saved_parts, roottrasf, assemblyofmates);


            // TODO: redundant part of code
            Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();
            // Fetch the python names using hash map (python names added when scanning parts)
            ModelDocExtension swModelDocExt = default(ModelDocExtension);
            ModelDoc2 swModel = (ModelDoc2)mSWApplication.ActiveDoc;
            swModelDocExt = swModel.Extension;

            // Add some comment in Python, to list the referenced SW items
            asciitext += "\n# Mate constraint: " + swMateFeature.Name + " [" + swMateFeature.GetTypeName2() + "]" + " type:" + swMate.Type + " align:" + swMate.Alignment + " flip:" + swMate.Flipped + "\n";
            for (int e = 0; e < swMate.GetMateEntityCount(); e++)
            {
                MateEntity2 swEntityN = swMate.MateEntity(e);
                Component2 swCompN = swEntityN.ReferenceComponent;
                String ce_nameN = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompN)];
                if (ce_nameN == "")
                    ce_nameN = "body_0"; // reference assembly
                asciitext += "#   Entity " + e + ": C::E name: " + ce_nameN + " , SW name: " + swCompN.Name2 + " ,  SW ref.type:" + swEntityN.Reference.GetType() + " (" + swEntityN.ReferenceType2 + ")\n";
            }
            asciitext += "\n";

            //// 
            //// WRITE PYHTON CODE CORRESPONDING TO CONSTRAINTS
            ////
            CultureInfo bz = new CultureInfo("en-BZ");

            if (link_params.do_ChLinkMateXdistance)
            {
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                if (!link_params.entity_0_as_VERTEX)
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                             link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                if (!link_params.entity_1_as_VERTEX)
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                             link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dA)\n", linkname, link_params.ref2, link_params.ref1);

                //if (link_params.do_distance_val!=0)
                asciitext += String.Format(bz, "{0}.SetDistance({1})\n", linkname,
                    link_params.do_distance_val * ChScale.L * -1);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    asciitext += String.Format(bz, "{0} = chrono.ChLinkMateParallel()\n", linkname);

                    asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    if (link_params.do_parallel_flip)
                        asciitext += String.Format(bz, "{0}.SetFlipped(True)\n", linkname);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                    else
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                    asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                    // Insert to a list of exported items
                    asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                }
                else
                {
                    asciitext += "\n# ChLinkMateParallel skipped because directions not parallel! \n";
                }
            }

            if (link_params.do_ChLinkMateOrthogonal)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) < 0.02)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    asciitext += String.Format(bz, "{0} = chrono.ChLinkMateOrthogonal()\n", linkname);

                    asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                    else
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                    asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                    // Insert to a list of exported items
                    asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                }
                else
                {
                    asciitext += "\n# ChLinkMateOrthogonal skipped because directions not orthogonal! \n";
                }
            }

            if (link_params.do_ChLinkMateSpherical)
            {
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateSpherical()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA)\n", linkname, link_params.ref2, link_params.ref1);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }

            if (link_params.do_ChLinkMatePointLine)
            {
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateGeneric()\n", linkname);
                asciitext += String.Format(bz, "{0}.SetConstrainedCoords(False, True, True, False, False, False)\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                if (!link_params.entity_0_as_VERTEX)
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n", link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                else
                    asciitext += String.Format(bz, "dA = chrono.VNULL\n");
                if (!link_params.entity_1_as_VERTEX)
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n", link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                else
                    asciitext += String.Format(bz, "dB = chrono.VNULL\n");

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }



            // Now, do some other special mate type that did not fall in combinations
            // of link_params.do_ChLinkMatePointLine, link_params.do_ChLinkMateSpherical, etc etc

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                if (Vector3D.DotProduct(link_params.dA, link_params.dB) < 0)
                    link_params.dB.Negate();

                // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateCoaxial()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                // Initialize link, by setting the two csys, in absolute space,
                asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);

                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n", linkname);




                num_link++;
                linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cC.X * ChScale.L,
                          link_params.cC.Y * ChScale.L,
                          link_params.cC.Z * ChScale.L);
                asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dC.X, link_params.dC.Y, link_params.dC.Z);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cD.X * ChScale.L,
                          link_params.cD.Y * ChScale.L,
                          link_params.cD.Z * ChScale.L);
                asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dD.X, link_params.dD.Y, link_params.dD.Z);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                // Initialize link, by setting the two csys, in absolute space,
                if (link_params.entity_2_as_VERTEX)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA)\n", linkname, link_params.ref3, link_params.ref4);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, link_params.ref3, link_params.ref4);

                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n", linkname);
            }


            return true;
        }

        public static bool GetLinkParameters(
                                    in Feature swMateFeature,
                                    out LinkParams link_params,
                                    in ISldWorks mSWApplication,
                                    in Hashtable saved_parts,
                                    in MathTransform roottrasf,
                                    in Component2 assemblyofmates
                                    )
        {


            link_params = new LinkParams
            {
                swapAB_1 = false,
                do_ChLinkMateXdistance = false,
                do_distance_val = 0,
                do_ChLinkMateParallel = false,
                do_parallel_flip = false,
                do_ChLinkMateOrthogonal = false,
                do_ChLinkMateSpherical = false,
                do_ChLinkMatePointLine = false,
                entity0_ref = 0,
                entity1_ref = 0,
                entity2_ref = 0,
                entity3_ref = 0,
                entity_0_as_FACE = false,
                entity_0_as_EDGE = false,
                entity_0_as_VERTEX = false,
                entity_1_as_FACE = false,
                entity_1_as_EDGE = false,
                entity_1_as_VERTEX = false,
                entity_2_as_VERTEX = false,
                entity_3_as_VERTEX = false,
                cA = new Point3D(0, 0, 0),
                cB = new Point3D(0, 0, 0),
                dA = new Vector3D(1, 0, 0),
                dB = new Vector3D(1, 0, 0),
                cC = new Point3D(0, 0, 0),
                cD = new Point3D(0, 0, 0),
                dC = new Vector3D(1, 0, 0),
                dD = new Vector3D(1, 0, 0),
                ref1 = null,
                ref2 = null,
                ref3 = null,
                ref4 = null
            };

            if (swMateFeature == null)
                return false;

            Mate2 swMate = (Mate2)swMateFeature.GetSpecificFeature2();

            if (swMate == null)
                return false;

            object foo = null;
            bool[] suppressedflags;
            suppressedflags = (bool[])swMateFeature.IsSuppressed2((int)swInConfigurationOpts_e.swThisConfiguration, foo);
            if (suppressedflags[0] == true)
                return false;

            if (swMate.GetMateEntityCount() < 2)
                return false;

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
            link_params.ref1 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompA)];
            link_params.ref2 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompB)];

            // Only constraints between two parts or part & layout can be created
            if (link_params.ref1 == link_params.ref2) // covers both the case of both null or both equal
                return false;


            if (link_params.ref1 == null)
                link_params.ref1 = "body_0";

            if (link_params.ref2 == null)
                link_params.ref2 = "body_0";



            //
            // For each type of SW mate, see which C::E mate constraint(s) 
            // must be created. Some SW mates correspond to more than one C::E constraints.
            // 



            // to simplify things later...
            // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
            // but in some cases the latter fails.
            /*
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
            */

            // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
            // but in some cases the latter fails. However, sometimes swMate.MateEntity(0).Reference.GetType() is null ReferenceType2 is ok,
            // so do the following trick:
            link_params.entity0_ref = swMate.MateEntity(0).Reference.GetType();
            if (link_params.entity0_ref == (int)swSelectType_e.swSelNOTHING)
                link_params.entity0_ref = swMate.MateEntity(0).ReferenceType2;
            link_params.entity1_ref = swMate.MateEntity(1).Reference.GetType();
            if (link_params.entity1_ref == (int)swSelectType_e.swSelNOTHING)
                link_params.entity1_ref = swMate.MateEntity(1).ReferenceType2;

            link_params.entity_0_as_FACE = (link_params.entity0_ref == (int)swSelectType_e.swSelFACES) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelDATUMPLANES);
            link_params.entity_0_as_EDGE = (link_params.entity0_ref == (int)swSelectType_e.swSelEDGES) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelDATUMAXES);
            link_params.entity_0_as_VERTEX = (link_params.entity0_ref == (int)swSelectType_e.swSelVERTICES) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                                (link_params.entity0_ref == (int)swSelectType_e.swSelDATUMPOINTS);

            link_params.entity_1_as_FACE = (link_params.entity1_ref == (int)swSelectType_e.swSelFACES) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelDATUMPLANES);
            link_params.entity_1_as_EDGE = (link_params.entity1_ref == (int)swSelectType_e.swSelEDGES) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelSKETCHSEGS) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelDATUMAXES);
            link_params.entity_1_as_VERTEX = (link_params.entity1_ref == (int)swSelectType_e.swSelVERTICES) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                                (link_params.entity1_ref == (int)swSelectType_e.swSelDATUMPOINTS);


            Point3D cAloc = new Point3D(paramsA[0], paramsA[1], paramsA[2]);
            link_params.cA = SWTaskpaneHost.PointTransform(cAloc, ref trA);
            Point3D cBloc = new Point3D(paramsB[0], paramsB[1], paramsB[2]);
            link_params.cB = SWTaskpaneHost.PointTransform(cBloc, ref trB);

            if (!link_params.entity_0_as_VERTEX)
            {
                Vector3D dAloc = new Vector3D(paramsA[3], paramsA[4], paramsA[5]);
                link_params.dA = SWTaskpaneHost.DirTransform(dAloc, ref trA);
            }

            if (!link_params.entity_1_as_VERTEX)
            {
                Vector3D dBloc = new Vector3D(paramsB[3], paramsB[4], paramsB[5]);
                link_params.dB = SWTaskpaneHost.DirTransform(dBloc, ref trB);
            }



            if (swMateFeature.GetTypeName2() == "MateCoincident")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMateSpherical = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.swapAB_1 = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.swapAB_1 = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateOrthogonal = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }

            if (swMateFeature.GetTypeName2() == "MateConcentric")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.swapAB_1 = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMatePointLine = true;
                    link_params.do_ChLinkMateParallel = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }

            if (swMateFeature.GetTypeName2() == "MateParallel")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }


            if (swMateFeature.GetTypeName2() == "MatePerpendicular")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateOrthogonal = true;
                }
                if ((link_params.entity_0_as_EDGE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_EDGE))
                {
                    link_params.do_ChLinkMateParallel = true;
                    link_params.swapAB_1 = true;
                }

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                    link_params.do_parallel_flip = true;
            }

            if (swMateFeature.GetTypeName2() == "MateDistanceDim")
            {
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.do_ChLinkMateParallel = true;
                }
                if ((link_params.entity_0_as_VERTEX) &&
                    (link_params.entity_1_as_FACE))
                {
                    link_params.do_ChLinkMateXdistance = true;
                }
                if ((link_params.entity_0_as_FACE) &&
                    (link_params.entity_1_as_VERTEX))
                {
                    link_params.do_ChLinkMateXdistance = true;
                    link_params.swapAB_1 = true;
                }

                //***TO DO*** cases of distance line-vs-line and line-vs-vertex and vert-vert.
                //           Those will require another .cpp ChLinkMate specialized class(es).

                // Get the imposed distance value, in SI units
                string confnames = "";
                link_params.do_distance_val = swMate.DisplayDimension.GetDimension2(0).IGetSystemValue3((int)swInConfigurationOpts_e.swThisConfiguration, 0, ref confnames);

                // Get aligment and flipped properties
                int alignment = swMate.Alignment;
                bool isflipped = swMate.Flipped;

                if (swMate.Alignment == (int)swMateAlign_e.swMateAlignALIGNED)
                {
                    if (isflipped)
                        link_params.do_distance_val = -link_params.do_distance_val;
                }
                else if (alignment == (int)swMateAlign_e.swMateAlignANTI_ALIGNED)
                {
                    link_params.do_parallel_flip = true;
                    if (!isflipped)
                        link_params.do_distance_val = -link_params.do_distance_val;
                }

            }

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // NOTE!!! The 'hinge' mate uses 4 references: fetch the two others remaining
                // and build another C::E link, for point-vs-face mating

                MateEntity2 swEntityC = swMate.MateEntity(2);
                MateEntity2 swEntityD = swMate.MateEntity(3);
                Component2 swCompC = swEntityC.ReferenceComponent;
                Component2 swCompD = swEntityD.ReferenceComponent;
                double[] paramsC = (double[])swEntityC.EntityParams;
                double[] paramsD = (double[])swEntityD.EntityParams;
                link_params.ref3 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompC)];
                link_params.ref4 = (String)saved_parts[swModelDocExt.GetPersistReference3(swCompD)];
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

                // NOTE: swMate.MateEntity(0).Reference.GetType() seems equivalent to  swMate.MateEntity(0).ReferenceType2  
                // but in some cases the latter fails. However, sometimes swMate.MateEntity(0).Reference.GetType() is null ReferenceType2 is ok,
                // so do the following trick:
                link_params.entity2_ref = swMate.MateEntity(2).Reference.GetType();
                if (link_params.entity2_ref == (int)swSelectType_e.swSelNOTHING)
                    link_params.entity2_ref = swMate.MateEntity(2).ReferenceType2;
                link_params.entity3_ref = swMate.MateEntity(3).Reference.GetType();
                if (link_params.entity3_ref == (int)swSelectType_e.swSelNOTHING)
                    link_params.entity3_ref = swMate.MateEntity(3).ReferenceType2;

                link_params.entity_2_as_VERTEX = (link_params.entity2_ref == (int)swSelectType_e.swSelVERTICES) ||
                                            (link_params.entity2_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                            (link_params.entity2_ref == (int)swSelectType_e.swSelDATUMPOINTS);
                link_params.entity_3_as_VERTEX = (link_params.entity3_ref == (int)swSelectType_e.swSelVERTICES) ||
                                            (link_params.entity3_ref == (int)swSelectType_e.swSelSKETCHPOINTS) ||
                                            (link_params.entity3_ref == (int)swSelectType_e.swSelDATUMPOINTS);



                Point3D cCloc = new Point3D(paramsC[0], paramsC[1], paramsC[2]);
                link_params.cC = SWTaskpaneHost.PointTransform(cCloc, ref trC);
                Point3D cDloc = new Point3D(paramsD[0], paramsD[1], paramsD[2]);
                link_params.cD = SWTaskpaneHost.PointTransform(cDloc, ref trD);

                if (!link_params.entity_2_as_VERTEX)
                {
                    Vector3D dCloc = new Vector3D(paramsC[3], paramsC[4], paramsC[5]);
                    link_params.dC = SWTaskpaneHost.DirTransform(dCloc, ref trC);
                }

                if (!link_params.entity_3_as_VERTEX)
                {
                    Vector3D dDloc = new Vector3D(paramsD[3], paramsD[4], paramsD[5]);
                    link_params.dD = SWTaskpaneHost.DirTransform(dDloc, ref trD);
                }
            }

            return true;
        }

        private static void WriteToPython(in ISldWorks mSWApplication, in Feature swMateFeature, ref string asciitext, int num_link, in LinkParams link_params)
        {
            //// 
            //// WRITE PYHTON CODE CORRESPONDING TO CONSTRAINTS
            ////
            CultureInfo bz = new CultureInfo("en-BZ");

            if (link_params.do_ChLinkMateXdistance)
            {
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                if (!link_params.entity_0_as_VERTEX)
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                             link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                if (!link_params.entity_1_as_VERTEX)
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                             link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dA)\n", linkname, link_params.ref2, link_params.ref1);

                //if (link_params.do_distance_val!=0)
                asciitext += String.Format(bz, "{0}.SetDistance({1})\n", linkname,
                    link_params.do_distance_val * ChScale.L * -1);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }

            if (link_params.do_ChLinkMateParallel)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) > 0.98)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    asciitext += String.Format(bz, "{0} = chrono.ChLinkMateParallel()\n", linkname);

                    asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    if (link_params.do_parallel_flip)
                        asciitext += String.Format(bz, "{0}.SetFlipped(True)\n", linkname);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                    else
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                    asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                    // Insert to a list of exported items
                    asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                }
                else
                {
                    asciitext += "\n# ChLinkMateParallel skipped because directions not parallel! \n";
                }
            }

            if (link_params.do_ChLinkMateOrthogonal)
            {
                if (Math.Abs(Vector3D.DotProduct(link_params.dA, link_params.dB)) < 0.02)
                {
                    num_link++;
                    String linkname = "link_" + num_link;
                    asciitext += String.Format(bz, "{0} = chrono.ChLinkMateOrthogonal()\n", linkname);

                    asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cA.X * ChScale.L,
                              link_params.cA.Y * ChScale.L,
                              link_params.cA.Z * ChScale.L);
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                    asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.cB.X * ChScale.L,
                              link_params.cB.Y * ChScale.L,
                              link_params.cB.Z * ChScale.L);
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                              link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                    // Initialize link, by setting the two csys, in absolute space,
                    if (!link_params.swapAB_1)
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                    else
                        asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                    asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                    // Insert to a list of exported items
                    asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
                }
                else
                {
                    asciitext += "\n# ChLinkMateOrthogonal skipped because directions not orthogonal! \n";
                }
            }

            if (link_params.do_ChLinkMateSpherical)
            {
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateSpherical()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA)\n", linkname, link_params.ref2, link_params.ref1);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }

            if (link_params.do_ChLinkMatePointLine)
            {
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateGeneric()\n", linkname);
                asciitext += String.Format(bz, "{0}.SetConstrainedCoords(False, True, True, False, False, False)\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                if (!link_params.entity_0_as_VERTEX)
                    asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n", link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                else
                    asciitext += String.Format(bz, "dA = chrono.VNULL\n");
                if (!link_params.entity_1_as_VERTEX)
                    asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n", link_params.dB.X, link_params.dB.Y, link_params.dB.Z);
                else
                    asciitext += String.Format(bz, "dB = chrono.VNULL\n");

                // Initialize link, by setting the two csys, in absolute space,
                if (!link_params.swapAB_1)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cB,cA,dB,dA)\n", linkname, link_params.ref2, link_params.ref1);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);
                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n\n", linkname);
            }



            // Now, do some other special mate type that did not fall in combinations
            // of link_params.do_ChLinkMatePointLine, link_params.do_ChLinkMateSpherical, etc etc

            if (swMateFeature.GetTypeName2() == "MateHinge")
            {
                // auto flip direction if anti aligned (seems that this is assumed automatically in MateHinge in SW)
                if (Vector3D.DotProduct(link_params.dA, link_params.dB) < 0)
                    link_params.dB.Negate();

                // Hinge constraint must be splitted in two C::E constraints: a coaxial and a point-vs-plane
                num_link++;
                String linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateCoaxial()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cA.X * ChScale.L,
                          link_params.cA.Y * ChScale.L,
                          link_params.cA.Z * ChScale.L);
                asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dA.X, link_params.dA.Y, link_params.dA.Z);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cB.X * ChScale.L,
                          link_params.cB.Y * ChScale.L,
                          link_params.cB.Z * ChScale.L);
                asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dB.X, link_params.dB.Y, link_params.dB.Z);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                // Initialize link, by setting the two csys, in absolute space,
                asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA,dB)\n", linkname, link_params.ref1, link_params.ref2);

                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n", linkname);




                num_link++;
                linkname = "link_" + num_link;
                asciitext += String.Format(bz, "{0} = chrono.ChLinkMateXdistance()\n", linkname);

                asciitext += String.Format(bz, "cA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cC.X * ChScale.L,
                          link_params.cC.Y * ChScale.L,
                          link_params.cC.Z * ChScale.L);
                asciitext += String.Format(bz, "dA = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dC.X, link_params.dC.Y, link_params.dC.Z);
                asciitext += String.Format(bz, "cB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.cD.X * ChScale.L,
                          link_params.cD.Y * ChScale.L,
                          link_params.cD.Z * ChScale.L);
                asciitext += String.Format(bz, "dB = chrono.ChVectorD({0:g},{1:g},{2:g})\n",
                          link_params.dD.X, link_params.dD.Y, link_params.dD.Z);

                asciitext += String.Format(bz, "{0}.SetName(\"{1}\")\n", linkname, swMateFeature.Name);


                // Initialize link, by setting the two csys, in absolute space,
                if (link_params.entity_2_as_VERTEX)
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dA)\n", linkname, link_params.ref3, link_params.ref4);
                else
                    asciitext += String.Format(bz, "{0}.Initialize({1},{2},False,cA,cB,dB)\n", linkname, link_params.ref3, link_params.ref4);

                // Insert to a list of exported items
                asciitext += String.Format(bz, "exported_items.append({0})\n", linkname);
            }
        }

    } // end class

} // end namespaces
