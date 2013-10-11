using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using ChronoEngineAddin;



namespace ChronoEngine_SwAddin
{
    class TesselateToObj
    {
        /// Convert a SolidWorks component to Wavefront OBJ mesh
        /// 
        public static void Convert(Component2 swComp, ref string asciitext, bool saveUV, ref UserProgressBar swProgress)
        {
            StringBuilder textbuilder = new StringBuilder();
            Body2 swBody = default(Body2);
            object[] vBodies = null;
            object vBodyInfo;
            int[] BodiesInfo = null;
            int j = 0;
            ModelDoc2 swModel;


            swModel = (ModelDoc2)swComp.GetModelDoc();

            vBodies = (object[])swComp.GetBodies3((int)swBodyType_e.swSolidBody, out vBodyInfo);
            BodiesInfo = (int[])vBodyInfo;

            if (vBodies != null)
            {
                int iNumBodies = vBodies.Length;
                int iNumSolidBodies = 0;
                int iNumSheetBodies = 0;
                int iNumTesselatedBodies = 0;


                if (iNumBodies >0)
                    asciitext += "# Wavefront .OBJ file for tesselated shape: " + swComp.Name2 + " path: " + swModel.GetPathName() + "\n\n";

                int group_vstride = 0;

                // Loop through bodies
                for (j = 0; j <= vBodies.Length - 1; j++)
                {
                    swBody = (Body2)vBodies[j];

                    int nBodyType = (int)swBody.GetType();

                    if (nBodyType == (int)swBodyType_e.swSheetBody)
                        iNumSheetBodies++;

                    if (nBodyType == (int)swBodyType_e.swSolidBody)
                        iNumSolidBodies++;

                    if ((nBodyType == (int)swBodyType_e.swSheetBody ||
                         nBodyType == (int)swBodyType_e.swSolidBody) &&
                        !swBody.Name.StartsWith("COLL.") )
                    {
                        iNumTesselatedBodies++;

                        CultureInfo bz = new CultureInfo("en-BZ");

                        asciitext += "g body_" + iNumTesselatedBodies + "\n";

                        Face2 swFace = null;
                        Tessellation swTessellation = null;

                        bool bResult = false;

                        // Pass in null so the whole body will be tessellated
                        swTessellation = (Tessellation)swBody.GetTessellation(null);

                        // Set up the Tessellation object
                        swTessellation.NeedFaceFacetMap = true;
                        swTessellation.NeedVertexParams = true;
                        swTessellation.NeedVertexNormal = true;
                        swTessellation.ImprovedQuality = true;

                        // How to handle matches across common edges
                        swTessellation.MatchType = (int)swTesselationMatchType_e.swTesselationMatchFacetTopology;

                        // Do it
                        if (swProgress != null)
                            swProgress.UpdateTitle("Exporting (tesselate process) ...");
                        bResult = swTessellation.Tessellate();
                        
                        // Get the number of vertices and facets
                        //System.Windows.Forms.MessageBox.Show("Body n." + j + " vert.num=" + swTessellation.GetVertexCount());
                        //Debug.Print("Number of vertices: " + swTessellation.GetVertexCount());
                        //Debug.Print("Number of facets: " + swTessellation.GetFacetCount());

                        // Now get the facet data per face
                        int[] aFacetIds;
                        int iNumFacetIds;
                        int[] aFinIds;
                        int[] aVertexIds;
                        double[] aNormal;
                        double[] aVertexCoords1;
                        double[] aVertexCoords2;
                        double[] aVertexParams;

                        //  Add sketch for this bodyto show lines into
                        // swModel.SetAddToDB(true);
                        // swModel.SetDisplayWhenAdded(false);
                        // swModel.Insert3DSketch2(false);

                        int numv = swTessellation.GetVertexCount();

                        // Write all vertexes
                        string mline;

                        for (int iv = 0; iv < numv; iv++)
                        {
                            if ((swProgress != null)&&(iv%200==0))
                                swProgress.UpdateTitle("Exporting (write " + iv + "-th vertex in .obj) ...");
                            aVertexCoords1 = (double[])swTessellation.GetVertexPoint(iv);
                            mline = "v " +      (aVertexCoords1[0]*ChScale.L).ToString("f6", bz)
                                        + " " + (aVertexCoords1[1]*ChScale.L).ToString("f6", bz)
                                        + " " + (aVertexCoords1[2]*ChScale.L).ToString("f6", bz)
                                        + "\n";
                            textbuilder.Append(mline);
                        }

                        // Write all normals
                        for (int iv = 0; iv < numv; iv++)
                        {
                            if ((swProgress != null) && (iv % 200 == 0))
                                 swProgress.UpdateTitle("Exporting (write " + iv + "-th normal in .obj) ...");
                            aNormal = (double[])swTessellation.GetVertexNormal(iv);
                            mline = "vn " +     aNormal[0].ToString("f3", bz)
                                        + " " + aNormal[1].ToString("f3", bz)
                                        + " " + aNormal[2].ToString("f3", bz)
                                        + "\n";
                            textbuilder.Append(mline);
                        }

                        // Write all UV (also with '0' as third value, for compatibility with some OBJ reader)
                        if (saveUV)
                            for (int iv = 0; iv < numv; iv++)
                            {
                                if ((swProgress != null) && (iv % 200 == 0))
                                     swProgress.UpdateTitle("Exporting (write " + iv + "-th UV in .obj) ...");
                                aVertexParams = (double[])swTessellation.GetVertexParams(iv);
                                mline = "vt " + aVertexParams[0].ToString("f4", bz)
                                            + " " + aVertexParams[1].ToString("f4", bz)
                                            + " " + "0" 
                                            + "\n";
                                textbuilder.Append(mline);
                            }


                        // Loop over faces
                        swFace = (Face2)swBody.GetFirstFace();

                        while (swFace != null)
                        {
                            aFacetIds = (int[])swTessellation.GetFaceFacets(swFace);
                            
                            iNumFacetIds = aFacetIds.Length;

                            for (int iFacetIdIdx = 0; iFacetIdIdx < iNumFacetIds; iFacetIdIdx++)
                            {
                                if ((swProgress != null) && (iFacetIdIdx % 100 == 0))
                                     swProgress.UpdateTitle("Exporting (write " + iFacetIdIdx + "-th face in .obj) ...");

                                mline = "f";
                                
                                aFinIds = (int[])swTessellation.GetFacetFins(aFacetIds[iFacetIdIdx]);

                                // There should always be three fins per facet
                                for (int iFinIdx = 0; iFinIdx < 3; iFinIdx++)
                                {
                                    aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);

                                    // Three fins per face, two vertexes each fin, 
                                    // only the 1st vertex of two is needed (because of sharing)
                                    if (saveUV)
                                        mline += " " + (aVertexIds[0] + group_vstride +1) + "/"
                                                     + (aVertexIds[0] + group_vstride +1) + "/" 
                                                     + (aVertexIds[0] + group_vstride +1);
                                    else
                                        mline += " " + (aVertexIds[0] + group_vstride + 1) + "//"
                                                     + (aVertexIds[0] + group_vstride + 1);

                                    // ***debug: Create a line in sketch showing the fin of triangle
                                    //aVertexCoords1 = (double[])swTessellation.GetVertexPoint(aVertexIds[0]);
                                    //aVertexCoords2 = (double[])swTessellation.GetVertexPoint(aVertexIds[1]);
                                    //swModel.CreateLine2(aVertexCoords1[0], aVertexCoords1[1], aVertexCoords1[2], aVertexCoords2[0], aVertexCoords2[1], aVertexCoords2[2]);
                                }

                                mline += "\n";
                                textbuilder.Append(mline);
                            }
                            swFace = (Face2)swFace.GetNextFace();
                        }

                        group_vstride += swTessellation.GetVertexCount();

                        // ***debug Close sketch
                        // swModel.Insert3DSketch2(true);
                        //  Clear selection for next pass
                        // swModel.ClearSelection2(true);
                        //  Restore settings
                        //swModel.SetAddToDB(false);
                        //swModel.SetDisplayWhenAdded(true);
                    }

                } // end loop on bodies
            } // not null body

            asciitext += textbuilder.ToString();
        } 

    }
}