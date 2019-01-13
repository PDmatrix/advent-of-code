#ifndef CPP_YEAR2015DAY01_H
#define CPP_YEAR2015DAY01_H

#include "../../../Common/Challenge.h"

class Year2015Day01 : public Challenge {
public:
    auto parse() {
        return this->input[0];
    }
    std::string part1() override {
        auto input = this->parse();
        int floor = 0;
        for (auto i: input) {
            floor += i == '(' ? 1 : -1;
        }
        return std::to_string(floor);
    }
    std::string part2() override {
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
    explicit Year2015Day01(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;
};

#endif //CPP_YEAR2015DAY01_H
