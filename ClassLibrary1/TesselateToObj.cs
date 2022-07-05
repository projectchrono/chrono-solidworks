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
        public static void Convert(Component2 swComp, ref string asciitext, bool saveUV, ref UserProgressBar swProgress, bool for_visualshapes, bool for_collshapes)
        {
            StringBuilder textbuilder = new StringBuilder();
            Body2 swBody = default(Body2);
            object[] vBodies = null;
            object[] vBodiesSolid = null;
            object[] vBodiesSheet = null;
            object vBodyInfo;
            int[] BodiesInfo = null;
            int j = 0;
            ModelDoc2 swModel;


            swModel = (ModelDoc2)swComp.GetModelDoc();

            vBodiesSolid = (object[])swComp.GetBodies3((int)swBodyType_e.swSolidBody, out vBodyInfo);
            vBodiesSheet = (object[])swComp.GetBodies3((int)swBodyType_e.swSheetBody, out vBodyInfo);
            BodiesInfo = (int[])vBodyInfo;

            if (vBodiesSolid != null && vBodiesSheet == null)
                vBodies = vBodiesSolid;

            if (vBodiesSolid == null && vBodiesSheet != null)
                vBodies = vBodiesSheet;

            if (vBodiesSolid != null && vBodiesSheet != null)
                vBodies = vBodiesSheet.Concat(vBodiesSolid).ToArray();

            // vBodies = (object[])swComp.GetBodies3((int)swBodyType_e.swSolidBody, out vBodyInfo);


            if (vBodies != null)
            {
                int iNumBodies = vBodies.Length;
                int iNumSolidBodies = 0;
                int iNumSheetBodies = 0;
                int iNumTesselatedBodies = 0;


                if (iNumBodies >0)
                    asciitext += "# Wavefront .OBJ file for tesselated shape: " + swComp.Name2 + " path: " + swModel.GetPathName() + "\n\n";

                int group_vstride = 0;
                int group_nstride = 0;

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
                         (
                          (for_visualshapes && 
                             (!swBody.Name.StartsWith("COLL.") && 
                             swBody.Visible)) 
                             ||
                          (for_collshapes &&
                             (swBody.Name.StartsWith("COLLMESH"))
                          )
                         )
                       )
                    {
                        iNumTesselatedBodies++;

                        CultureInfo bz = new CultureInfo("en-BZ");

                        //asciitext += "g body_" + iNumTesselatedBodies + "\n";
                        textbuilder.Append("g body_" + iNumTesselatedBodies + "\n");            

                        Face2 swFace = null;
                        Tessellation swTessellation = null;

                        bool bResult = false;

                        // Pass in null so the whole body will be tessellated
                        swTessellation = (Tessellation)swBody.GetTessellation(null);

                        //****DEBUG***
                        /*
                        System.Windows.Forms.MessageBox.Show("TESSELLATE: \n" +
                         String.Format(bz, "CurveChordAngleTolerance={0} \n", swTessellation.CurveChordAngleTolerance) + // DEFAULT 0.523
                         String.Format(bz, "CurveChordTolerance={0} \n", swTessellation.CurveChordTolerance) +  // DEFAULT CHANGES WITH OBJ
                         String.Format(bz, "SurfacePlaneAngleTolerance={0} \n", swTessellation.SurfacePlaneAngleTolerance) + // DEFAULT 0.523
                         String.Format(bz, "SurfacePlaneTolerance={0} \n", swTessellation.SurfacePlaneTolerance) +  // DEFAULT CHANGES WITH OBJ
                         String.Format(bz, "MaxFacetWidth={0} \n", swTessellation.MaxFacetWidth) +
                         String.Format(bz, "MinFacetWidth={0} \n", swTessellation.MinFacetWidth));
                        */

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
                        if (nBodyType == (int)swBodyType_e.swSheetBody)  // for sheets, save two-sided triangles
                            for (int iv = 0; iv < numv; iv++)
                            {
                                if ((swProgress != null) && (iv % 200 == 0))
                                    swProgress.UpdateTitle("Exporting (write " + iv + "-th normal in .obj) ...");
                                aNormal = (double[])swTessellation.GetVertexNormal(iv);
                                mline = "vn "     + (-aNormal[0]).ToString("f3", bz)
                                            + " " + (-aNormal[1]).ToString("f3", bz)
                                            + " " + (-aNormal[2]).ToString("f3", bz)
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
                                                     + (aVertexIds[0] + group_nstride +1);
                                    else
                                        mline += " " + (aVertexIds[0] + group_vstride + 1) + "//"
                                                     + (aVertexIds[0] + group_nstride + 1);
                                }

                                mline += "\n";
                                textbuilder.Append(mline);
                            }
                            swFace = (Face2)swFace.GetNextFace();
                        }

                        swFace = (Face2)swBody.GetFirstFace();
                        if (nBodyType == (int)swBodyType_e.swSheetBody)  // for sheets, save two-sided triangles
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

                                for (int iFinIdx = 2; iFinIdx >= 0; iFinIdx--)
                                {
                                    aVertexIds = (int[])swTessellation.GetFinVertices(aFinIds[iFinIdx]);

                                    if (saveUV)
                                        mline += " " + (aVertexIds[0] + group_vstride + 1) + "/"
                                                     + (aVertexIds[0] + group_vstride + 1) + "/"
                                                     + (aVertexIds[0] + swTessellation.GetVertexCount() + group_nstride + 1);
                                    else
                                        mline += " " + (aVertexIds[0] + group_vstride + 1) + "//"
                                                     + (aVertexIds[0] + swTessellation.GetVertexCount() + group_nstride + 1);
                                }

                                mline += "\n";
                                textbuilder.Append(mline);
                            }
                            swFace = (Face2)swFace.GetNextFace();
                        }

                        group_vstride += swTessellation.GetVertexCount();
                        group_nstride += swTessellation.GetVertexCount();

                        if (nBodyType == (int)swBodyType_e.swSheetBody)  // for sheets: two-sided triangles
                            group_nstride += swTessellation.GetVertexCount();
                    }

                } // end loop on bodies
            } // not null body

            asciitext += textbuilder.ToString();
        } 

    }
}