// C++ Chrono::Engine model automatically generated using Chrono::SolidWorks add-in

#ifndef CH_IMPORT_SLDW_CPP_ADDIN_TESTER_EXPORT_H
#define CH_IMPORT_SLDW_CPP_ADDIN_TESTER_EXPORT_H

#include <vector>
#include <unordered_map>
#include "chrono/physics/ChBodyAuxRef.h"
#include "chrono/physics/ChLinkMate.h"
#include "chrono/motion_functions/ChFunction.h"
#include "chrono/physics/ChSystem.h"

/// Function to import Solidworks assembly directly into Chrono ChSystem.
void ImportSolidworksSystemCpp(chrono::ChSystem& system, std::unordered_map<std::string, std::shared_ptr<chrono::ChFunction>>* motfun_map = nullptr);

/// Function to import Solidworks bodies and mates into dedicated containers.
void ImportSolidworksSystemCpp(std::vector<std::shared_ptr<chrono::ChBodyAuxRef>>& bodylist, std::vector<std::shared_ptr<chrono::ChLinkBase>>& linklist, std::unordered_map<std::string, std::shared_ptr<chrono::ChFunction>>* motfun_map = nullptr);

#endif // end CH_IMPORT_SLDW_CPP_H
