#--------------------------------------------------------------
# Project setup 
#--------------------------------------------------------------
cmake_minimum_required(VERSION 3.18)
cmake_policy(SET CMP0091 NEW)

project(ChronoSolidworksImport)

#--------------------------------------------------------------
# Find Chrono
#--------------------------------------------------------------
find_package(Chrono
             COMPONENTS Irrlicht
             OPTIONAL_COMPONENTS Parsers
             CONFIG)

# Return now if Chrono or a required component was not found.
if (NOT Chrono_FOUND)
  message("Could not find Chrono or one of its required modules")
  return()
endif()

if(APPLE)
    set(CMAKE_MACOSX_BUNDLE ON)
endif()

#--------------------------------------------------------------
# Query SolidWorks exported files
#--------------------------------------------------------------
set(SOLIDWORKS_EXPORTED_FILE_CPP "../addin_tester.cpp" CACHE FILEPATH "Path to Solidworks CPP exported file.")
set(SOLIDWORKS_EXPORTED_FILE_JSON "../addin_tester.json" CACHE FILEPATH "Path to Solidworks JSON exported file.")
set(SOLIDWORKS_EXPORTED_FILE_PY "../addin_tester.py" CACHE FILEPATH "Path to Solidworks PY exported file.")

get_filename_component(SLW_CPP_EXT ${SOLIDWORKS_EXPORTED_FILE_CPP} LAST_EXT)
if (NOT (SLW_CPP_EXT MATCHES ".cpp" OR SLW_CPP_EXT MATCHES ".h"))
  message(WARNING "SOLIDWORKS_EXPORTED_FILE_CPP expected extension is \".cpp\" or \".h\" but ${SLW_CPP_EXT} found. Is it correct?")
endif()

get_filename_component(SLW_JSON_EXT ${SOLIDWORKS_EXPORTED_FILE_JSON} LAST_EXT)
if (NOT SLW_JSON_EXT MATCHES ".json")
  message(WARNING "SOLIDWORKS_EXPORTED_FILE_JSON expected extension is \".json\" but ${SLW_JSON_EXT} found. Is it correct?")
endif()

get_filename_component(SLW_PY_EXT ${SOLIDWORKS_EXPORTED_FILE_PY} LAST_EXT)
if (NOT SLW_PY_EXT MATCHES ".py")
  message(WARNING "SOLIDWORKS_EXPORTED_FILE_PY expected extension is \".py\" but ${SLW_PY_EXT} found. Is it correct?")
endif()

#--------------------------------------------------------------
# Executables
#--------------------------------------------------------------

# ------------ CPP ---------------
if (EXISTS ${SOLIDWORKS_EXPORTED_FILE_CPP})
  message(STATUS "Run exported C++ model from C++.")

  get_filename_component(SLW_EXPORT_DIR ${SOLIDWORKS_EXPORTED_FILE_CPP} DIRECTORY)
  get_filename_component(SLW_EXPORT_FILENAME_NOEXT ${SOLIDWORKS_EXPORTED_FILE_CPP} NAME_WLE)

  # Copy shapes
  file(COPY "${SLW_EXPORT_DIR}/${SLW_EXPORT_FILENAME_NOEXT}_shapes/"
      DESTINATION "${CMAKE_BINARY_DIR}/${SLW_EXPORT_FILENAME_NOEXT}_shapes/")

  add_executable(solid_import_cpp solid_import_cpp.cpp ${SLW_EXPORT_DIR}/${SLW_EXPORT_FILENAME_NOEXT}.h ${SLW_EXPORT_DIR}/${SLW_EXPORT_FILENAME_NOEXT}.cpp)
  target_compile_definitions(solid_import_cpp PUBLIC "SOLIDWORKS_EXPORTED_HEADER=\"${SLW_EXPORT_DIR}/${SLW_EXPORT_FILENAME_NOEXT}.h\"")
  target_compile_definitions(solid_import_cpp PRIVATE "CHRONO_DATA_DIR=\"${CHRONO_DATA_DIR}\"") 
  if(MSVC)
      set_target_properties(solid_import_cpp PROPERTIES MSVC_RUNTIME_LIBRARY ${CHRONO_MSVC_RUNTIME_LIBRARY})
  endif()
  target_link_libraries(solid_import_cpp PRIVATE ${CHRONO_TARGETS})

  set(SLW_EXPORT_FILENAME_NOEXT_CPP ${SLW_EXPORT_FILENAME_NOEXT})

else()
  message(WARNING "C++ export file not found. Skipping CPP import for file: \"${SOLIDWORKS_EXPORTED_FILE_CPP}\".")

endif()

# ------------ JSON ---------------
if (EXISTS ${SOLIDWORKS_EXPORTED_FILE_JSON})
  message(STATUS "Run exported JSON model from C++.")

  get_filename_component(SLW_EXPORT_DIR ${SOLIDWORKS_EXPORTED_FILE_JSON} DIRECTORY)
  get_filename_component(SLW_EXPORT_FILENAME_NOEXT ${SOLIDWORKS_EXPORTED_FILE_JSON} NAME_WLE)

  # Copy shapes
  file(COPY "${SLW_EXPORT_DIR}/${SLW_EXPORT_FILENAME_NOEXT}_shapes/"
      DESTINATION "${CMAKE_BINARY_DIR}/${SLW_EXPORT_FILENAME_NOEXT}_shapes/")

  add_executable(solid_import_json solid_import_json.cpp)
  target_compile_definitions(solid_import_json PUBLIC "SOLIDWORKS_EXPORTED_JSON=\"${SOLIDWORKS_EXPORTED_FILE_JSON}\"")
  target_compile_definitions(solid_import_json PRIVATE "CHRONO_DATA_DIR=\"${CHRONO_DATA_DIR}\"") 
  if(MSVC)
      set_target_properties(solid_import_json PROPERTIES MSVC_RUNTIME_LIBRARY ${CHRONO_MSVC_RUNTIME_LIBRARY})
  endif()
  target_link_libraries(solid_import_json PRIVATE ${CHRONO_TARGETS})
  set(SLW_EXPORT_FILENAME_NOEXT_JSON ${SLW_EXPORT_FILENAME_NOEXT})

else()
  message(WARNING "JSON export file not found. Skipping JSON import for file: \"${SOLIDWORKS_EXPORTED_FILE_JSON}\".")
endif()

# ------------ PYTHON ---------------
if (EXISTS ${SOLIDWORKS_EXPORTED_FILE_PY})
  message(STATUS "Run exported Python model from C++.")

  get_filename_component(SLW_EXPORT_DIR ${SOLIDWORKS_EXPORTED_FILE_PY} DIRECTORY)
  get_filename_component(SLW_EXPORT_FILENAME_NOEXT ${SOLIDWORKS_EXPORTED_FILE_PY} NAME_WLE)

  # ------------ PYCHRONO ---------------
  # Create PyChrono script with proper reference to exported file
  configure_file("${CMAKE_CURRENT_SOURCE_DIR}/solid_import_pychrono.py.in"
    "${PROJECT_BINARY_DIR}/pychrono/solid_import_pychrono.py"
    @ONLY)

  # no need to warn about copying shapes since they are not copied in case of PyChrono

  # ------------ PYTHON PARSER ---------------
  if (CHRONO_PARSERS_PYTHON)
    get_filename_component(SLW_EXPORT_DIR ${SOLIDWORKS_EXPORTED_FILE_PY} DIRECTORY)
    get_filename_component(SLW_EXPORT_FILENAME_NOEXT ${SOLIDWORKS_EXPORTED_FILE_PY} NAME_WLE)

    # # Copy shapes
    # file(COPY "${SLW_EXPORT_DIR}/${SLW_EXPORT_FILENAME_NOEXT}_shapes/"
    #     DESTINATION "${CMAKE_BINARY_DIR}/${SLW_EXPORT_FILENAME_NOEXT}_shapes/")


    add_executable(solid_import_pyparser solid_import_pyparser.cpp)
    target_compile_definitions(solid_import_pyparser PUBLIC "SOLIDWORKS_EXPORTED_PY=\"${SOLIDWORKS_EXPORTED_FILE_PY}\"")
    target_compile_definitions(solid_import_pyparser PRIVATE "CHRONO_DATA_DIR=\"${CHRONO_DATA_DIR}\"") 
    if(MSVC)
        set_target_properties(solid_import_pyparser PROPERTIES MSVC_RUNTIME_LIBRARY ${CHRONO_MSVC_RUNTIME_LIBRARY})
    endif()
    target_link_libraries(solid_import_pyparser PRIVATE ${CHRONO_TARGETS})

    set(SLW_EXPORT_FILENAME_NOEXT_PY ${SLW_EXPORT_FILENAME_NOEXT})
  else()
    message(WARNING "Chrono Parsers not found: skipping Python Parsing for file: \"${SOLIDWORKS_EXPORTED_FILE_PY}\".")

  endif()

else()
  message(WARNING "Python export file not found. Skipping PyChrono and Python Parsing for file: \"${SOLIDWORKS_EXPORTED_FILE_PY}\".")

endif()

# ------------ Warn about overwriting shapes ---------------

if (   ((DEFINED SLW_EXPORT_FILENAME_NOEXT_CPP) AND (DEFINED SLW_EXPORT_FILENAME_NOEXT_JSON) AND (${SLW_EXPORT_FILENAME_NOEXT_CPP} MATCHES ${SLW_EXPORT_FILENAME_NOEXT_JSON}))
    OR ((DEFINED SLW_EXPORT_FILENAME_NOEXT_PY)  AND (DEFINED SLW_EXPORT_FILENAME_NOEXT_CPP)  AND (${SLW_EXPORT_FILENAME_NOEXT_PY}  MATCHES ${SLW_EXPORT_FILENAME_NOEXT_CPP} ))
    OR ((DEFINED SLW_EXPORT_FILENAME_NOEXT_PY)  AND (DEFINED SLW_EXPORT_FILENAME_NOEXT_JSON) AND (${SLW_EXPORT_FILENAME_NOEXT_PY}  MATCHES ${SLW_EXPORT_FILENAME_NOEXT_JSON}))
    )
    message(WARNING "Exported files have the same name. This may cause problems since shapes will end up being overwritten.")
endif()

#--------------------------------------------------------------
# Aux
#--------------------------------------------------------------
add_CHRONO_DLLS_copy_command()