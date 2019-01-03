#include <utility>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <numeric>

#include "Year2015Day02.h"

auto Year2015Day02::parse() {
    return this->input;
}

std::vector<std::string> split(const std::string& s, char delimiter)
{
    std::vector<std::string> tokens;
    std::string token;
    std::istringstream tokenStream(s);
    while (std::getline(tokenStream, token, delimiter))
    {
        tokens.push_back(token);
    }
    return tokens;
}

std::string Year2015Day02::part1() {
    std::vector<std::string> input = this->parse();
    int result = 0;
    for (const auto &i: input) {
        auto dimensions = split(i, 'x');
        int l = std::stoi(dimensions[0]),
            w = std::stoi(dimensions[1]),
            h = std::stoi(dimensions[2]);
        int smallestSide = std::min(std::min(l * w, w * h), h * l);
        result += smallestSide + ((2 * l * w) + (2 * w * h) + (2 * h * l));
    }
    return std::to_string(result);
}

std::string Year2015Day02::part2() {
    std::vector<std::string> input = this->parse();
    int result = 0;
    for (const auto &i: input) {
        auto dimensions = split(i, 'x');
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

Year2015Day02::Year2015Day02(std::vector<std::string> lines) {
    this->input = std::move(lines);
}
