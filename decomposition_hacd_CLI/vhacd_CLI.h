// vhacd_CLI.h

#pragma once

#include "vhacdHACD.h"
#include "vhacdMeshDecimator.h"


using namespace System;


namespace vhacd_CLI {

	public ref class Vect3D
	{
	public:
		Vect3D(double mX, double mY, double mZ)
		{
			X = mX;
			Y = mY;
			Z = mZ;
		}
		Vect3D()
		{
			X = 0;
			Y = 0;
			Z = 0;
		}
		double X;
		double Y;
		double Z;
	};

	public ref class Triangle
	{
	public:
		Triangle(int mp1, int mp2, int mp3)
		{
			p1 = mp1;
			p2 = mp2;
			p3 = mp3;
		}
		int p1;
		int p2;
		int p3;
	};


	void myCallBack(const char * msg, double progress, double concavity, size_t nVertices)
	{
		std::cout << msg;
	}


	public ref class vhacd_CLI_wrapper
	{
	public:
		vhacd_CLI_wrapper()  
		{
			myMesh = new VHACD::Mesh();
			points = new std::vector< VHACD::Vec3<VHACD::Real> >;
			triangles = new std::vector< VHACD::Vec3<long> >;
			parts = new std::vector< VHACD::Mesh * >;

			this->targetNTrianglesDecimatedMesh = 1000;
			this->depth =10;
			this->concavity = 0.01;
			this->posSampling = 10;
			this->angleSampling = 10;
			this->posRefine = 5;
			this->angleRefine = 5;
			this->alpha = 0.01;
		}

		~vhacd_CLI_wrapper() { this->!vhacd_CLI_wrapper(); }

		!vhacd_CLI_wrapper() 
		{ 
			delete parts;
			delete points;
			delete triangles;
			delete myMesh;
		}

			/// Preferences-setting functions:
/*
		void SetVolumeWeight(double mval)
		{
			myHACD->SetVolumeWeight(mval);
		}
*/
		


			/// Pass the mesh data as an array of vertexes 
			/// and an array of triangles (indices of vertexes)
		void SetMesh(array<Vect3D^> ^managedpoints, array<Triangle^> ^managedtriangles)
		{
			// marshal array in:
			points->clear();
			for (int i = 0; i< managedpoints->GetLength(0); i++)
			{
				VHACD::Vec3<VHACD::Real> mpt(managedpoints[i]->X, managedpoints[i]->Y, managedpoints[i]->Z);
				points->push_back(mpt);
			}
			
			// marshal array in:
			triangles->clear();
			for (int i = 0; i< managedtriangles->GetLength(0); i++)
			{
				VHACD::Vec3<long> mtri(managedtriangles[i]->p1, managedtriangles[i]->p2, managedtriangles[i]->p3);
				triangles->push_back(mtri);
			}

			bool decimatedMeshComputed = false;
			size_t nTriangles = triangles->size();
			size_t nPoints    = points->size();
			VHACD::Vec3<VHACD::Real> * pPoints    = &((*points)[0]);
			VHACD::Vec3<long> *        pTriangles = &((*triangles)[0]);

			if (targetNTrianglesDecimatedMesh > 0 && targetNTrianglesDecimatedMesh < nTriangles)
			{
				decimatedMeshComputed = true;

				VHACD::MeshDecimator myMDecimator;
				//myMDecimator.SetCallBack(&myCallBack);
				myMDecimator.Initialize(nPoints, nTriangles, (VHACD::Vec3<VHACD::Float>*)&(points[0]), (VHACD::Vec3<long>*)&(triangles[0]));
				myMDecimator.Decimate(0, targetNTrianglesDecimatedMesh);

				nTriangles = myMDecimator.GetNTriangles();
				nPoints    = myMDecimator.GetNVertices();
				pPoints     = new VHACD::Vec3<VHACD::Real>[nPoints];
				pTriangles  = new VHACD::Vec3<long>[nTriangles];
				myMDecimator.GetMeshData(pPoints, pTriangles);
			}

			myMesh->ResizePoints(nPoints);
			myMesh->ResizeTriangles(nTriangles);
			for(size_t p = 0; p < nPoints;    ++p) myMesh->SetPoint(p, pPoints[p]);
			for(size_t t = 0; t < nTriangles; ++t) myMesh->SetTriangle(t, pTriangles[t]);
			if (decimatedMeshComputed)
			{
				delete [] pPoints;
				delete [] pTriangles;
			}
			
		}


			/// Compute the convex decomposition!!!
			/// Call this function after having set the preferences and 
			/// after having used SetMesh(), and fetch results via GetNClusters() and GetClusterConvexHull()
		int Compute(bool fullCH, bool exportDistPoints) 
		{
			VHACD::ApproximateConvexDecomposition(*this->myMesh, 
				depth, 
				posSampling, 
				angleSampling, 
				posRefine, 
				angleRefine, 
				alpha, 
				concavity, 
				*parts, 
				0);
			return parts->size();
		}
/*
		void Save(System::String^ mpath)
		{
			IntPtr p = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(mpath);
			char *pNewCharStr = static_cast<char*>(p.ToPointer());
 
			myMesh->SaveVRML2(std::string(pNewCharStr), false);

			System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
		}
*/

			/// Get the number of computed convex hulls
		int GetNClusters()
		{
			return parts->size();
		}

			/// Get the vertexes (and triangulation, if needed)
			/// of the Nth resulting convex hull.
		int GetClusterConvexHull(int ncluster, array<Vect3D^>^% managedpoints, array<Triangle^>^% managedtriangles)
		{
			VHACD::Mesh ch;
			(*parts)[ncluster]->ComputeConvexHull(ch);

			size_t nPoints = ch.GetNPoints();
			size_t nTriangles = ch.GetNTriangles();
			VHACD::Vec3<VHACD::Real> * pointsCH = new VHACD::Vec3<VHACD::Real>[nPoints];
			VHACD::Vec3<long> * trianglesCH = new VHACD::Vec3<long>[nTriangles];

			//myVHACD->GetCH(ncluster, pointsCH, trianglesCH);
			//marshal array out:
			Array::Resize(managedpoints,nPoints);
			for (int i=0;i<nPoints;i++)
				managedpoints[i] = gcnew Vect3D( ch.GetPoint(i).X(), ch.GetPoint(i).Y(), ch.GetPoint(i).Z());
			//marshal array out:
			Array::Resize(managedtriangles,nTriangles);
			for (int i=0;i<nTriangles;i++)
				managedtriangles[i] = gcnew Triangle( ch.GetTriangle(i).X(),  ch.GetTriangle(i).Y(),  ch.GetTriangle(i).Z()  );
			
			return ncluster;
		}

	private:	
		VHACD::Mesh* myMesh;

		std::vector< VHACD::Mesh * >* parts;

		std::vector< VHACD::Vec3<VHACD::Real> >*	points;
		std::vector< VHACD::Vec3<long> >*		triangles;
		
	public:
		int targetNTrianglesDecimatedMesh;
		int depth;
		int posSampling;
		int angleSampling;
		int posRefine;
		int angleRefine; 
		double alpha;
		double concavity;

	};



}
