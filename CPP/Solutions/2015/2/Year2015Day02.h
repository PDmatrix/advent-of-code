#ifndef CPP_YEAR2015DAY02_H
#define CPP_YEAR2015DAY02_H

#include "../../../Common/Challenge.h"
#include "../../../Common/Common.h"
#include <utility>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <numeric>

class Year2015Day02 : public Challenge {
public:
    auto parse() {
        return this->input;
    }
    std::string part1() override {
        std::vector<std::string> input = this->parse();
        int result = 0;
        for (const auto &i: input) {
            std::regex re("x");
            auto dimensions = AdventOfCode::split(re, i);
            int l = std::stoi(dimensions[0]),
                    w = std::stoi(dimensions[1]),
                    h = std::stoi(dimensions[2]);
            int smallestSide = std::min(std::min(l * w, w * h), h * l);
            result += smallestSide + ((2 * l * w) + (2 * w * h) + (2 * h * l));
        }
        return std::to_string(result);
    }
    std::string part2() override {
        std::vector<std::string> input = this->parse();
        int result = 0;
        for (const auto &i: input) {
            std::regex re("x");
            auto dimensions = AdventOfCode::split(re, i);
            std::vector<int> intDimensions;
            std::for_each(dimensions.begin(), dimensions.end(),
                          [&intDimensions](std::string &str){ intDimensions.push_back(std::stoi(str)); });
            std::sort(intDimensions.begin(), intDimensions.end());
            result += (2 * intDimensions[0] + 2 * intDimensions[1])
                      + std::accumulate(intDimensions.begin(), intDimensions.end(), 1,
                                        [](int f, int s){ return f * s;});
        }
        return std::to_string(result);
    }
    explicit Year2015Day02(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;
};

#endif //CPP_YEAR2015DAY02_H
