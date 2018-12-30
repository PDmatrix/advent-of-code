#include <utility>

#include "Year2015Day01.h"

auto Year2015Day01::parse() {
    return this->input[0];
}

std::string Year2015Day01::part1() {
    auto input = this->parse();
    return std::__cxx11::string(input);
}

std::string Year2015Day01::part2() {
    auto input = this->parse();
    return std::__cxx11::string(input);
}

Year2015Day01::Year2015Day01(std::vector<std::string> lines) {
    this->input = std::move(lines);
}
