#ifndef CPP_YEAR2015DAY04_H
#define CPP_YEAR2015DAY04_H

#include "../../../Common/Challenge.h"
#include <utility>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <numeric>
#include <set>
#include <tuple>

#include "md5.h"

class Year2015Day04 : public Challenge {
public:
    auto parse() {
        return this->input[0];
    }
    std::string part1() override {
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
    std::string part2() override {
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
    explicit Year2015Day04(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;
};

#endif //CPP_YEAR2015DAY04_H
