#include <utility>

#ifndef CPP_YEAR2015DAY07_H
#define CPP_YEAR2015DAY07_H

#include "../../../Common/Challenge.h"
#include "../../../Common/Common.h"


class Year2015Day07 : public Challenge {
public:
    auto parse() {
        std::regex not_op("NOT");
        std::regex and_op("AND");
        std::regex or_op("OR");
        std::regex lshift_op("LSHIFT");
        std::regex rshift_op("RSHIFT");
        std::regex arrow("->");
        std::vector<Element*> elements;
        for(const auto &i : this->input) {
            if(std::regex_search(i, not_op)) {
                auto words = AdventOfCode::split(arrow, i);
                auto depend = AdventOfCode::trim_copy(AdventOfCode::split(not_op, words[0])[1]);
                int num = -1;
                std::vector<std::string> depends;
                if(is_number(depend)) {
                    num = std::stoi(depend);
                } else {
                    depends.push_back(depend);
                }
                auto op = new Operation([](int a){ return !a;});
                std::string str = AdventOfCode::trim_copy(words[1]);
                auto el = new Element(num, depends, *op, str);
                elements.push_back(el);
            } else if(std::regex_search(i, and_op)) {

            } else if(std::regex_search(i, or_op)) {

            } else if(std::regex_search(i, lshift_op)) {

            } else if(std::regex_search(i, rshift_op)) {

            } else {
                auto splt = AdventOfCode::split(arrow, i);
                int num = -1;
                auto depend = AdventOfCode::trim_copy(splt[0]);
                std::vector<std::string> depends;
                if(is_number(depend)) {
                    num = std::stoi(depend);
                } else {
                    depends.push_back(depend);
                }
                auto op = new Operation();
                auto el = new Element(num, depends, *op, AdventOfCode::trim_copy(splt[1]));
                elements.push_back(el);
            }
        }
        return elements;
    }

    std::string part1() override {
        //auto op = new Operation([](int a){ return 1;});
        //return op->unary_operation == nullptr ? "null" : "not null";
        auto res = parse();

        return "1";
    }

    std::string part2() override {
        return std::to_string(1);
    }

    explicit Year2015Day07(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;

    struct Operation {
        std::function<int(const int, const int)> binary_operation;
        std::function<int(const int)> unary_operation;
        explicit Operation()
                : binary_operation(nullptr), unary_operation(nullptr) {}
        explicit Operation(std::function<int(int, int)> bo)
                : binary_operation(std::move(bo)), unary_operation(nullptr) {}
        explicit Operation(std::function<int(int)> uo)
                : binary_operation(nullptr), unary_operation(std::move(uo)) {}
    };

    struct Element {
        std::vector<std::string> depends;
        int value;
        std::string element;
        Operation operation;
        explicit Element(
                int value,
                std::vector<std::string> depends,
                Operation operation,
                std::string el)
                :
                value(value),
                depends(std::move(depends)),
                operation(std::move(operation)),
                element(std::move(el)) {}
    };
    bool is_number(const std::string& s)
    {
        return !s.empty() &&
            std::find_if(s.begin(), s.end(), [](char c) { return !std::isdigit(c); }) == s.end();
    }
};

#endif //CPP_YEAR2015DAY07_H
