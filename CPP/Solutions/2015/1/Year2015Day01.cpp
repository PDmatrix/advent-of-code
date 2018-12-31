#include <utility>

#include "Year2015Day01.h"

auto Year2015Day01::parse() {
    return this->input[0];
}

std::string Year2015Day01::part1() {
    auto input = this->parse();
    int floor = 0;
    for (auto i: input) {
        floor += i == '(' ? 1 : -1;
    }
    return std::to_string(floor);
}

std::string Year2015Day01::part2() {
    auto input = this->parse();
    int floor = 0;
    for (int i = 0; i < input.length(); ++i) {
        floor += input[i] == '(' ? 1 : -1;
        if (floor == -1) {
            return std::to_string(i + 1);
        }
    }
    throw std::exception();
}

Year2015Day01::Year2015Day01(std::vector<std::string> lines) {
    this->input = std::move(lines);
}
