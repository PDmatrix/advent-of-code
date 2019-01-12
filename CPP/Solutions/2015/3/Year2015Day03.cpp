#include <utility>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <numeric>
#include <set>
#include <tuple>

#include "Year2015Day03.h"

struct Point {
    int x;
    int y;
    Point() : x(0), y(0) {}
};

inline bool operator<(const Point& lhs, const Point& rhs) {
    return std::tie(lhs.x, lhs.y) < std::tie(rhs.x, rhs.y);
}

auto Year2015Day03::parse() {
    return this->input[0];
}

std::string Year2015Day03::part1() {
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

std::string Year2015Day03::part2() {
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

Year2015Day03::Year2015Day03(std::vector<std::string> lines) {
    this->input = std::move(lines);
}
