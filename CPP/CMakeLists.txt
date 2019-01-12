cmake_minimum_required(VERSION 3.13)
project(CPP)

set(CMAKE_CXX_STANDARD 17)

list(APPEND SolutionFiles Solutions/2015/1/Year2015Day01.cpp Solutions/2015/1/Year2015Day01.h)
list(APPEND SolutionFiles Solutions/2015/2/Year2015Day02.cpp Solutions/2015/2/Year2015Day02.h)
list(APPEND SolutionFiles Solutions/2015/3/Year2015Day03.cpp Solutions/2015/3/Year2015Day03.h)

add_executable(CPP main.cpp Common/Solutions.h Common/Challenge.h ${SolutionFiles} Common/Common.h)