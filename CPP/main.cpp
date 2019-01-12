#include <iostream>
#include <cxxopts.hpp>
#include <fstream>
#include "Common/Solutions.h"

auto parse_options(int argc, char **argv) {
    cxxopts::Options options("Advent Of Code", "My solutions for the Advent of code using C++");
    options.add_options()
            ("y, year", "Year", cxxopts::value<int>())
            ("d, day", "Day", cxxopts::value<int>());
    return options.parse(argc, argv);
}

std::string get_input_path(int year, int day) {
    char buff[100];
    std::sprintf(buff, "../Input/%i/Day%02i.in", year, day);
    std::string str = buff;
    return str;
}

Challenge* get_challenge(int year, int day, const std::vector<std::string> &lines) {
    std::map<int, std::map<int, Challenge*>> map {
            {2015, {
                {1, new Year2015Day01(lines)},
                {2, new Year2015Day02(lines)},
                {3, new Year2015Day03(lines)}
            }}
    };
    return map[year][day];
}

inline std::vector<std::string> read_lines(const std::string& inputFile)
{
    std::vector<std::string> lines;
    std::fstream f(inputFile);

    while(!f.eof())
    {
        std::string line;
        if(std::getline(f, line))
        {
            line.erase(std::remove(line.begin(), line.end(), '\r'), line.end());
            line.erase(std::remove(line.begin(), line.end(), '\n'), line.end());
            lines.push_back(line);
        }
    }
    return lines;
}

void normalize_lines(std::vector<std::string>& lines) {
   try {
        int code = (int)lines[0][0];
        while(code < 0) {
            lines[0].erase(0, 1);
            code = (int)lines[0][0];
        }
    }
    catch (...) { }
}

void exec(int year, int day) {
    std::vector<std::string> lines = read_lines(get_input_path(year, day));
    normalize_lines(lines);
    auto challenge = get_challenge(year, day, lines);
    std::cout << "First part: " << challenge->part1() << std::endl;
    std::cout << "Second part: " << challenge->part2() << std::endl;
}

int main(int argc, char **argv) {
    auto result = parse_options(argc, argv);
    exec(result["y"].as<int>(), result["d"].as<int>());
    return 0;
}