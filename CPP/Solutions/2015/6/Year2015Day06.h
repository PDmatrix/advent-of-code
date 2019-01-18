#include <utility>

#ifndef CPP_YEAR2015DAY06_H
#define CPP_YEAR2015DAY06_H

#include "../../../Common/Challenge.h"
#include "../../../Common/Common.h"

struct Instruction {
   int startX, endX, startY, endY;
   std::function<bool(const bool)> motion;
   Instruction(int sx, int ex, int sy, int ey, std::function<bool(bool)> fn)
   : startX(sx), endX(ex), startY(sy), endY(ey), motion(std::move(fn)){}
};

struct CorrectInstruction {
    int startX, endX, startY, endY;
    std::function<int(const int)> motion;
    CorrectInstruction(int sx, int ex, int sy, int ey, std::function<int(int)> fn)
            : startX(sx), endX(ex), startY(sy), endY(ey), motion(std::move(fn)){}
};
class Year2015Day06 : public Challenge {
public:
    auto parse() {
        std::vector<Instruction*> instructions;
        std::regex re("\\d*,\\d*");
        for(const auto &i : this->input) {
            std::vector<int> values;
            std::smatch match;
            std::string::const_iterator text_iter = i.cbegin();
            while(std::regex_search(text_iter, i.end(), match, re)) {
                std::regex rgx(",");
                auto split = AdventOfCode::split(rgx, match[0]);
                values.push_back(std::stoi(split[0]));
                values.push_back(std::stoi(split[1]));
                text_iter = match[0].second;
            }
            int sx = values[0], sy = values[1], ex = values[2], ey = values[3];
            values.clear();
            if(i.substr(5, 2) == "on") {
                instructions.push_back(new Instruction(sx, ex, sy, ey, [](bool b){ return true;}));
            } else if (i.substr(5, 3) == "off") {
                instructions.push_back(new Instruction(sx, ex, sy, ey, [](bool b){ return false;}));
            } else {
                instructions.push_back(new Instruction(sx, ex, sy, ey, [](bool b){ return !b;}));
            }
        }
        return instructions;
    }

    auto parse_part2() {
        std::vector<CorrectInstruction*> instructions;
        std::regex re("\\d*,\\d*");
        for(const auto &i : this->input) {
            std::vector<int> values;
            std::smatch match;
            std::string::const_iterator text_iter = i.cbegin();
            while(std::regex_search(text_iter, i.end(), match, re)) {
                std::regex rgx(",");
                auto split = AdventOfCode::split(rgx, match[0]);
                values.push_back(std::stoi(split[0]));
                values.push_back(std::stoi(split[1]));
                text_iter = match[0].second;
            }
            int sx = values[0], sy = values[1], ex = values[2], ey = values[3];
            values.clear();
            if(i.substr(5, 2) == "on") {
                instructions.push_back(new CorrectInstruction(sx, ex, sy, ey,
                        [](const int i){
                    int val = i;
                    return ++val;
                }));
            } else if (i.substr(5, 3) == "off") {
                instructions.push_back(new CorrectInstruction(sx, ex, sy, ey,
                        [](const int i){
                    int val = i;
                    return i == 0 ? 0 : --val;
                }));
            } else {
                instructions.push_back(new CorrectInstruction(sx, ex, sy, ey,
                        [](const int i){
                    int val = i;
                    return val += 2;
                }));
            }
        }
        return instructions;
    }

    std::string part1() override {
        std::vector<Instruction*> input = this->parse();
        std::vector<std::vector<bool>> grid(1000, std::vector<bool>(1000, false));
        for(const auto &instruction : input) {
            for(int x = instruction->startX; x <= instruction->endX; ++x) {
                for(int y = instruction->startY; y <= instruction->endY; ++y) {
                    grid[x][y] = instruction->motion(grid[x][y]);
                }
            }
        }
        long long sum = 0;
        for(int x = 0; x < 1000; ++x) {
            for(int y = 0; y < 1000; ++y) {
                sum += (int)grid[x][y];
            }
        }
        return std::to_string(sum);
    }
    std::string part2() override {
        std::vector<CorrectInstruction*> input = this->parse_part2();
        std::vector<std::vector<int>> grid(1000, std::vector<int>(1000, 0));
        for(const auto &instruction : input) {
            for(int x = instruction->startX; x <= instruction->endX; ++x) {
                for(int y = instruction->startY; y <= instruction->endY; ++y) {
                    grid[x][y] = instruction->motion(grid[x][y]);
                }
            }
        }
        long long sum = 0;
        for(int x = 0; x < 1000; ++x) {
            for(int y = 0; y < 1000; ++y) {
                sum += (int)grid[x][y];
            }
        }
        return std::to_string(sum);
    }
    explicit Year2015Day06(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;
};

#endif //CPP_YEAR2015DAY06_H
