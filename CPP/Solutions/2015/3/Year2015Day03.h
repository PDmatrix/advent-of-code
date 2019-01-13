#ifndef CPP_YEAR2015DAY03_H
#define CPP_YEAR2015DAY03_H

#include "../../../Common/Challenge.h"
#include <utility>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <numeric>
#include <set>
#include <tuple>

struct Point {
    int x;
    int y;
    Point() : x(0), y(0) {}
};

inline bool operator<(const Point& lhs, const Point& rhs) {
    return std::tie(lhs.x, lhs.y) < std::tie(rhs.x, rhs.y);
}

class Year2015Day03 : public Challenge {
public:
    auto parse() {
        return this->input[0];
    }
    std::string part1() override {
        std::string input = this->parse();
        auto santa = new Point();
        std::set<Point> points = {*santa};
        for (const auto &i: input) {
            switch (i){
                case '>':
                    santa->y += 1;
                    break;
                case '<':
                    santa->y -= 1;
                    break;
                case '^':
                    santa->x += 1;
                    break;
                case 'v':
                    santa->x -= 1;
                    break;
                default:
                    break;
            }
            points.insert(*santa);
        }
        return std::to_string(points.size());
    }
    std::string part2() override {
        auto santa = new Point();
        auto roboSanta = new Point();
        std::string input = this->parse();
        std::set<Point> points = {*santa};
        for(int i=0; i < input.size(); ++i) {
            Point* curSanta = i % 2 ? santa : roboSanta;
            switch (input[i]){
                case '>':
                    curSanta->y += 1;
                    break;
                case '<':
                    curSanta->y -= 1;
                    break;
                case '^':
                    curSanta->x += 1;
                    break;
                case 'v':
                    curSanta->x -= 1;
                    break;
                default:
                    break;
            }
            points.insert(*curSanta);
        }
        return std::to_string(points.size());
    }
    explicit Year2015Day03(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;
};

#endif //CPP_YEAR2015DAY03_H
