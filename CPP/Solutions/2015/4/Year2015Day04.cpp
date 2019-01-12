#include <utility>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <numeric>
#include <set>
#include <tuple>

#include "Year2015Day04.h"
#include "md5.h"

auto Year2015Day04::parse() {
    return this->input[0];
}

std::string Year2015Day04::part1() {
    std::string input = this->parse();
    MD5 md5;
    int idx = 100000;
    std::string sb = md5(input + std::to_string(idx)).substr(0, 5);
    while(sb != "00000") {
        idx++;
        sb = md5(input + std::to_string(idx)).substr(0, 5);
    }
    return std::to_string(idx);
}

std::string Year2015Day04::part2() {
    std::string input = this->parse();
    MD5 md5;
    int idx = 1000000;
    std::string sb = md5(input + std::to_string(idx)).substr(0, 6);
    while(sb != "000000") {
        idx++;
        sb = md5(input + std::to_string(idx)).substr(0, 6);
    }
    return std::to_string(idx);
}

Year2015Day04::Year2015Day04(std::vector<std::string> lines) {
    this->input = std::move(lines);
}
