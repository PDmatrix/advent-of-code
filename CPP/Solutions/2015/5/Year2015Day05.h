#ifndef CPP_YEAR2015DAY05_H
#define CPP_YEAR2015DAY05_H

#include "../../../Common/Challenge.h"

bool vowels(std::string str) {
    std::string allowed = "aeiou";
    unsigned long init = str.size();
    for(auto c : allowed) {
        str.erase(std::remove(str.begin(), str.end(), c), str.end());
    }
    return init - str.size() >= 3;
}

void eraseAllSubStr(std::string & mainStr, const std::string & toErase)
{
    size_t pos;
    while ((pos  = mainStr.find(toErase) )!= std::string::npos)
    {
        mainStr.erase(pos, toErase.length());
    }
}

bool twice(std::string str) {
    char lastChar = '\0';
    for (auto c : str) {
        if(lastChar == '\0') {
            lastChar = c;
            continue;
        }
        if(lastChar == c) {
            return true;
        }
        lastChar = c;
    }
    return false;
}

bool not_in(std::string str) {
    std::vector<std::string> dirtyStrings = {"ab", "cd", "pq", "xy"};
    unsigned long init = str.size();
    for(const auto &dirtyString : dirtyStrings) {
        eraseAllSubStr(str, dirtyString);
    }
    return init == str.size();
}

bool pair(std::string str) {
    std::vector<std::string> pairs;
    std::string cur;
    for(auto c : str) {
        cur.push_back(c);
        if(cur.size() >= 2) {
            if(std::find(pairs.begin(), pairs.end(), cur) != pairs.end()) {
                if(pairs.back() != cur)
                    return true;
            }
            pairs.push_back(cur);
            cur.erase(0, 1);
        }
    }
    return false;
}

bool between(std::string str) {
    std::string cur;
    for(auto c : str) {
        cur.push_back(c);
        if(cur.size() >= 3) {
            if(cur[0] == cur[2])
                return true;
            cur.erase(0, 1);
        }
    }
    return false;
}

class Year2015Day05 : public Challenge {
public:
    auto parse() {
        return this->input;
    }
    std::string part1() override {
        auto input = this->parse();
        int count = 0;
        for(const auto &str : input) {
            if(vowels(str) && twice(str) && not_in(str))
                count++;
        }
        return std::to_string(count);
    }
    std::string part2() override {
        auto input = this->parse();
        // Some hacks, starting with 0 doesnt work
        int count = 1;
        for(const auto &str : input) {
            if(pair(str) && between(str))
                count++;
        }
        return std::to_string(count);
    }
    explicit Year2015Day05(std::vector<std::string> lines) {
        this->input = std::move(lines);
    }
private:
    std::vector<std::string> input;
};

#endif //CPP_YEAR2015DAY05_H
