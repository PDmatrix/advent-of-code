#include <iostream>
#include <cxxopts.hpp>
#include <fstream>
#include "Solutions/2015/1/Year2015Day01.h"
#include "Solutions/2015/2/Year2015Day02.h"

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
                {2, new Year2015Day02(lines)}
            }}
    };
    return map[year][day];
}

void exec(int year, int day) {
    std::ifstream input(get_input_path(year, day));
    std::vector<std::string> lines;
    std::copy(std::istream_iterator<std::string>(input),
              std::istream_iterator<std::string>(),
              std::back_inserter(lines));
    try {
        int code = (int)lines[0][0];
        while(code < 0) {
            lines[0].erase(0, 1);
            code = (int)lines[0][0];
        }
    }
    catch (...) {

    }
    auto challenge = get_challenge(year, day, lines);
    std::cout << "First part: " << challenge->part1() << std::endl;
    std::cout << "Second part: " << challenge->part2() << std::endl;
}

int main(int argc, char **argv) {
    auto result = parse_options(argc, argv);
    exec(result["y"].as<int>(), result["d"].as<int>());
    return 0;
}